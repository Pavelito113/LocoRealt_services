using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using LocoRealt.Models; // ApplicationUser

namespace LocoRealt.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Admin", "Agent", "User" };

            // Создаём роли
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Создание пользователя: Admin
            var adminEmail = "admin@locorealt.com";
            var adminPassword = "Admin123!";
            if (await userManager.FindByEmailAsync(adminEmail) is null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            // Создание пользователя: Agent
            var agentEmail = "agent@locorealt.com";
            var agentPassword = "Agent123!";
            if (await userManager.FindByEmailAsync(agentEmail) is null)
            {
                var agent = new ApplicationUser
                {
                    UserName = agentEmail,
                    Email = agentEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(agent, agentPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(agent, "Agent");
                }
            }

            // Создание пользователя: User
            var userEmail = "user@locorealt.com";
            var userPassword = "User123!";
            if (await userManager.FindByEmailAsync(userEmail) is null)
            {
                var user = new ApplicationUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
            }
        }
    }
}
