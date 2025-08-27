// Services/BreadcrumbService.cs
using LocoRealt.Data;
using LocoRealt.Models;
using Microsoft.EntityFrameworkCore;

public class BreadcrumbService : IBreadcrumbService
{
    private readonly ApplicationDbContext _ctx;
    public BreadcrumbService(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task<BreadcrumbTrail> BuildForFiltersAsync(ListingFilterParams f)
    {
        var items = new List<BreadcrumbItem> {
            new() { Title = "Главная", Url = "/", IconClass = "fas fa-home breadcrumb-icon" }
        };

        // Функция для построения URL с сохранением всех текущих параметров
        string BuildUrlWithParams(string? newDealType = null)
        {
            var queryParams = new Dictionary<string, string>();

            // Сохраняем все текущие параметры
            if (f.CountryId.HasValue) queryParams["countryId"] = f.CountryId.Value.ToString();
            if (f.RegionId.HasValue) queryParams["regionId"] = f.RegionId.Value.ToString();
            if (f.CityId.HasValue) queryParams["cityId"] = f.CityId.Value.ToString();
            if (!string.IsNullOrEmpty(f.Street)) queryParams["street"] = f.Street;
            if (f.CategoryId.HasValue) queryParams["categoryId"] = f.CategoryId.Value.ToString();
            if (f.SubCategoryId.HasValue) queryParams["subCategoryId"] = f.SubCategoryId.Value.ToString();

            // Цена
            if (f.MinPrice.HasValue) queryParams["minPrice"] = f.MinPrice.Value.ToString();
            if (f.MaxPrice.HasValue) queryParams["maxPrice"] = f.MaxPrice.Value.ToString();

            // Комнаты
            if (f.MinRooms.HasValue) queryParams["minRooms"] = f.MinRooms.Value.ToString();
            if (f.MaxRooms.HasValue) queryParams["maxRooms"] = f.MaxRooms.Value.ToString();

            // Площадь
            if (f.MinArea.HasValue) queryParams["minArea"] = f.MinArea.Value.ToString();
            if (f.MaxArea.HasValue) queryParams["maxArea"] = f.MaxArea.Value.ToString();

            // Коммуникации
            if (f.HasGasHeating.HasValue) queryParams["hasGasHeating"] = f.HasGasHeating.Value.ToString();
            if (f.HasCentralWater.HasValue) queryParams["hasCentralWater"] = f.HasCentralWater.Value.ToString();
            if (f.HasCentralHeating.HasValue) queryParams["hasCentralHeating"] = f.HasCentralHeating.Value.ToString();
            if (f.HasCentralScanalisation.HasValue) queryParams["hasCentralScanalisation"] = f.HasCentralScanalisation.Value.ToString();
            if (f.HasInternet.HasValue) queryParams["hasInternet"] = f.HasInternet.Value.ToString();
            if (f.HasParking.HasValue) queryParams["hasParking"] = f.HasParking.Value.ToString();

            // Сортировка
            if (!string.IsNullOrEmpty(f.OrderBy)) queryParams["orderBy"] = f.OrderBy;

            // DealType - используем новый или текущий
            queryParams["dealType"] = newDealType ?? f.DealType ?? "sale";

            return "/?" + string.Join("&", queryParams.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
        }

        // DealType breadcrumb
        switch (f.DealType?.ToLowerInvariant())
        {
            case "rent":
            case "аренда":
                items.Add(new() { Title = "Аренда", Url = BuildUrlWithParams("rent"), IconClass = "fas fa-hand-holding-usd breadcrumb-icon" });
                break;
            case "hotel":
            case "гостиница":
                items.Add(new() { Title = "Гостиницы", Url = BuildUrlWithParams("hotel"), IconClass = "fas fa-hotel breadcrumb-icon" });
                break;
            default:
                items.Add(new() { Title = "Продажа", Url = BuildUrlWithParams("sale"), IconClass = "fas fa-tag breadcrumb-icon" });
                break;
        }

        // Country breadcrumb
        if (f.CountryId != null)
        {
            var c = await _ctx.Countries.FindAsync(f.CountryId);
            if (c != null) items.Add(new()
            {
                Title = c.TxtTitle,
                Url = BuildUrlWithParams(),
                IconClass = "fas fa-globe-europe breadcrumb-icon"
            });
        }

        // Region breadcrumb
        if (f.RegionId != null)
        {
            var r = await _ctx.Regions.FindAsync(f.RegionId);
            if (r != null) items.Add(new()
            {
                Title = r.TxtTitle,
                Url = BuildUrlWithParams(),
                IconClass = "fas fa-map-marked-alt breadcrumb-icon"
            });
        }

        // City breadcrumb
        if (f.CityId != null)
        {
            var ct = await _ctx.Cities.FindAsync(f.CityId);
            if (ct != null) items.Add(new()
            {
                Title = ct.TxtTitle,
                Url = BuildUrlWithParams(),
                IconClass = "fas fa-city breadcrumb-icon"
            });
        }

        // Category breadcrumb
        if (f.CategoryId != null)
        {
            var cat = await _ctx.Categories.FindAsync(f.CategoryId);
            if (cat != null) items.Add(new()
            {
                Title = cat.TxtTitle,
                Url = BuildUrlWithParams()
            });
        }

        // SubCategory breadcrumb (активный элемент)
        if (f.SubCategoryId != null)
        {
            var sc = await _ctx.SubCategories.FindAsync(f.SubCategoryId);
            if (sc != null) items.Add(new()
            {
                Title = sc.TxtTitle,
                IsActive = true,
                IconClass = "fas fa-road breadcrumb-icon"
            });
        }
        else if (items.Count > 0)
        {
            items[^1].IsActive = true;
        }

        return new BreadcrumbTrail { Items = items };
    }

    public async Task<BreadcrumbTrail> BuildForListingAsync(long listingId)
    {
        var l = await _ctx.Listings
            .Include(x => x.ListingGeo)
            .Include(x => x.Category)
            .Include(x => x.SubCategory)
            .FirstOrDefaultAsync(x => x.ListingId == listingId);

        var items = new List<BreadcrumbItem> {
            new() { Title = "Главная", Url = "/", IconClass = "fas fa-home breadcrumb-icon" }
        };

        // DealType
        if (l?.IsForRent == true)
            items.Add(new() { Title = "Аренда", Url = "/?dealType=rent", IconClass = "fas fa-hand-holding-usd breadcrumb-icon" });
        else if (l?.ListingTypeId == 3)
            items.Add(new() { Title = "Гостиницы", Url = "/?dealType=hotel", IconClass = "fas fa-hotel breadcrumb-icon" });
        else
            items.Add(new() { Title = "Продажа", Url = "/?dealType=sale", IconClass = "fas fa-tag breadcrumb-icon" });

        if (l?.ListingGeo?.CountryId is long cId)
        {
            var c = await _ctx.Countries.FindAsync(cId);
            if (c != null) items.Add(new() { Title = c.TxtTitle, IconClass = "fas fa-globe-europe breadcrumb-icon" });
        }
        if (l?.ListingGeo?.RegionId is long rId)
        {
            var r = await _ctx.Regions.FindAsync(rId);
            if (r != null) items.Add(new() { Title = r.TxtTitle, IconClass = "fas fa-map-marked-alt breadcrumb-icon" });
        }
        if (l?.ListingGeo?.CityId is long cityId)
        {
            var ct = await _ctx.Cities.FindAsync(cityId);
            if (ct != null) items.Add(new() { Title = ct.TxtTitle, IconClass = "fas fa-city breadcrumb-icon" });
        }

        if (l?.Category != null) items.Add(new() { Title = l.Category.TxtTitle });
        if (l?.SubCategory != null) items.Add(new() { Title = l.SubCategory.TxtTitle });
        items.Add(new() { Title = l?.TxtTitle ?? "Объявление", IsActive = true, IconClass = "fas fa-road breadcrumb-icon" });

        return new BreadcrumbTrail { Items = items };
    }
}
