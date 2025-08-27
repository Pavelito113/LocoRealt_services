using Microsoft.AspNetCore.Mvc;
using LocoRealt.Models;
using LocoRealt.Services;

public class HomeController : Controller
{
    private readonly IListingsService _listings;
    private readonly IBreadcrumbService _breadcrumbs;
    private readonly ICategoryService _cats;
    private readonly IGeoService _geo;

    public HomeController(
        IListingsService listings,
        IBreadcrumbService breadcrumbs,
        ICategoryService cats,
        IGeoService geo)
    {
        _listings = listings;
        _breadcrumbs = breadcrumbs;
        _cats = cats;
        _geo = geo;
    }

    public async Task<IActionResult> Index(ListingFilterParams filters, int page = 1)
    {
        // Преобразуем текстовый dealType в ListingTypeId
        if (!string.IsNullOrEmpty(filters.DealType))
        {
            filters.ListingTypeId = filters.DealType.ToLower() switch
            {
                "sale" or "продажа" => 1,
                "rent" or "аренда" => 2,
                "hotel" or "гостиница" => 3,
                _ => null
            };
        }
        // Последовательная загрузка данных для страницы поиска
        var vm = await _listings.BuildListingsViewModelAsync(filters, page, pageSize: 20);

        // Построение хлебных крошек
        vm.BreadcrumbTrail = await _breadcrumbs.BuildForFiltersAsync(filters);

        // Загрузка категорий и гео-фильтров
        vm.CategoriesVm = await _cats.LoadCategoriesAsync(filters);
        vm.GeoVm = await _geo.LoadGeoFiltersAsync(
            filters.CountryId, filters.RegionId, filters.CityId,
            filters.Street, filters.DealType);

        return View(vm);
    }
}
