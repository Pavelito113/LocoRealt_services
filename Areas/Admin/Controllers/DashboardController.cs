using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Area("Admin")]
[Route("admin")]
[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(ILogger<DashboardController> logger)
    {
        _logger = logger;
    }

    [HttpGet("")]
    [HttpGet("dashboard")]
    public IActionResult Index()
    {
        _logger.LogInformation("Admin panel accessed");
        ViewBag.Title = "Панель администратора";
        return View();
    }
}