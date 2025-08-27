using LocoRealt.Models;
using LocoRealt.Services;
using Microsoft.AspNetCore.Mvc;

public class FiltersController : Controller
{
    private readonly IListingsService _listingsService;

    public FiltersController(IListingsService listingsService)
    {
        _listingsService = listingsService;
    }

    public async Task<IActionResult> Index([FromQuery] ListingFilterParams filters, int page = 1, int pageSize = 20)
    {
        var model = await _listingsService.BuildListingsViewModelAsync(filters, page, pageSize);
        return View(model); // передаем полноценный ListingsViewModel
    }
}
