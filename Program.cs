using System.Diagnostics;
using System.Text.Json;
using LocoRealt.Data;
using LocoRealt.Models;
using LocoRealt.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

// 1. Подключение EF Core (SQL Server)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IGeoService, GeoService>();
builder.Services.AddScoped<IListingsService, ListingsService>();
builder.Services.AddScoped<IBreadcrumbService, BreadcrumbService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });

    options.EnableSensitiveDataLogging();
    options.EnableDetailedErrors();

    options.LogTo(
        message =>
        {
            if (!message.Contains("Microsoft.EntityFrameworkCore.Infrastructure") &&
                !message.Contains("Microsoft.EntityFrameworkCore.Database.Connection"))
            {
                Console.WriteLine(message);
                Debug.WriteLine(message);
            }
        },
        LogLevel.Information,
        DbContextLoggerOptions.DefaultWithLocalTime | DbContextLoggerOptions.SingleLine);
});

// Developer exception page
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Logging
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Information);

// 2. Identity + роли
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 3. MVC и Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// 4. Redis Cache (если нужен)
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis")
        ?? throw new InvalidOperationException("Connection string 'Redis' not found.");
});

// 5. Наш сервис кэширования
builder.Services.AddScoped<DistributedCacheService>();

// 6. Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAgentRole", policy => policy.RequireRole("Agent"));
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
});

// 7. HealthChecks + UI
var healthChecksBuilder = builder.Services.AddHealthChecks();
healthChecksBuilder.AddDbContextCheck<ApplicationDbContext>();

// Правильное добавление SQL Server health check
healthChecksBuilder.AddSqlServer(
    connectionString: connectionString,
    name: "SQL Server",
    tags: new[] { "database", "sql" });

// Правильное добавление Redis health check
var redisConnection = builder.Configuration.GetConnectionString("Redis");
if (!string.IsNullOrEmpty(redisConnection))
{
    healthChecksBuilder.AddRedis(
        redisConnectionString: redisConnection, // Правильное имя параметра
        name: "Redis",
        tags: new[] { "cache", "redis" });
}

builder.Services.AddHealthChecksUI()
    .AddInMemoryStorage();

// 8. Заглушка для IEmailSender
builder.Services.AddTransient<IEmailSender, ConsoleEmailSender>();

// 9. HttpClient для внешних API
builder.Services.AddHttpClient("GeoAPI", client =>
{
    client.Timeout = TimeSpan.FromSeconds(5);
});

// 10. Response caching
builder.Services.AddResponseCaching();

var app = builder.Build();

// 11. Seed roles/users
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await SeedData.InitializeAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ошибка при инициализации базы данных");
    }
}

// 12. HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Razor Pages для Identity
app.MapRazorPages();

// HealthCheck endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
    options.ResourcesPath = "/health-ui-resources";
});

// Routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "admin",
    pattern: "admin/{controller=Dashboard}/{action=Index}/{id?}",
    defaults: new { area = "Admin" });

app.MapControllerRoute(
    name: "agent",
    pattern: "agent/{controller=Dashboard}/{action=Index}/{id?}",
    defaults: new { area = "Agent" });

app.MapControllerRoute(
    name: "user",
    pattern: "user/{controller=Dashboard}/{action=Index}/{id?}",
    defaults: new { area = "User" });

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.Run();

// Заглушка IEmailSender
public class ConsoleEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        Console.WriteLine("------ Email ------");
        Console.WriteLine($"To: {email}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Content:\n{htmlMessage}");
        Console.WriteLine("-------------------");
        return Task.CompletedTask;
    }
}