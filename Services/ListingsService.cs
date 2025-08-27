using LocoRealt.Data;
using LocoRealt.Models;
using LocoRealt.Models.Helpers;
using LocoRealt.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class ListingsService : IListingsService
{
    private readonly ApplicationDbContext _ctx;
    private readonly IGeoService _geo;
    private readonly ICategoryService _cats;
    private readonly DistributedCacheService _cache;

    public ListingsService(ApplicationDbContext ctx, IGeoService geo, ICategoryService cats, DistributedCacheService cache)
    {
        _ctx = ctx; _geo = geo; _cats = cats; _cache = cache;
    }

    public async Task<int> GetListingsCountAsync(ListingFilterParams filters)
    {
        var key = $"listings_total_{filters.DealType}_" + BuildFilterKey(filters);
        return await _cache.GetOrCreateAsync(key, async () =>
            await _ctx.Listings.Where(l => l.IntShow)
                .ApplyFilters(filters)
                .CountAsync(),
            TimeSpan.FromMinutes(10));
    }
    // 1. Хелпер для переключения типа сделки, сохраняя остальные параметры
    public Dictionary<string, string> BuildRouteForDealType(ListingFilterParams f, string dealType)
    {
        var rv = BuildRouteValues(f);
        rv["dealType"] = dealType; // жёстко переопределяем
        return rv;
    }

    public async Task<List<ListingItemDto>> GetListingsPageAsync(ListingFilterParams filters, int page, int pageSize)
    {
        var key = $"listings_{filters.DealType}_page{page}_" + BuildFilterKey(filters);
        return await _cache.GetOrCreateAsync(key, async () =>
            await _ctx.Listings
                .Include(l => l.ListingImages)
                .Include(l => l.ListingGeo)
                .Where(l => l.IntShow)
                .ApplyFilters(filters)
                .ApplySorting(filters.OrderBy)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(l => new ListingItemDto
                {
                    ListingId = l.ListingId,
                    TxtTitle = l.TxtTitle,
                    DblPrice = l.DblPrice,
                    TxtImageUrl = $"/Images/GetByListingId/{l.ListingId}",
                    Location = l.ListingGeo != null ? l.ListingGeo.TxtAddress : null,
                    DtCreated = l.DtCreated,
                    IsBargain = l.IsBargain,
                    IsForSale = l.IsForSale,
                    IsForRent = l.IsForRent,
                    Category = l.Category,
                    ListingType = l.ListingType,
                    SubCategory = l.SubCategory,
                    Rooms = l.Rooms,
                    Floor = l.Floor,
                    TotalArea = l.TotalArea,
                    LandArea = l.LandArea,
                    Status = l.Status
                })
                .ToListAsync(),
            TimeSpan.FromMinutes(10)) ?? new();
    }

    public async Task<ListingsViewModel> BuildListingsViewModelAsync(ListingFilterParams filters, int page, int pageSize)
    {
        // Если вообще не передано ничего — только тогда ставим по умолчанию "sale"
        if (string.IsNullOrEmpty(filters.DealType) && filters.ListingTypeId == null)
            filters.DealType = "sale";

        var (countries, regions, cities) = await _geo.GetGeoListsAsync(filters.CountryId, filters.RegionId);
        var (categories, subCategories) = await _cats.GetCategoryListsAsync();

        var totalItems = await GetListingsCountAsync(filters);
        var listings = await GetListingsPageAsync(filters, page, pageSize);

        return new ListingsViewModel
        {
            Listings = listings,
            Countries = new SelectList(countries, "CountryId", "TxtTitle", filters.CountryId),
            Regions = new SelectList(regions, "RegionId", "TxtTitle", filters.RegionId),
            Cities = new SelectList(cities, "CityId", "TxtTitle", filters.CityId),

            Categories = new SelectList(categories, "CategoryId", "TxtTitle", filters.CategoryId),
            SubCategories = new SelectList(subCategories, "SubCategoryId", "TxtTitle", filters.SubCategoryId),

            DealTypes = ListingType.GetDealTypes().Select(x => x.Title).ToList(),

            Pagination = new PaginationModel
            {
                CurrentPage = page,
                TotalItems = totalItems,
                ItemsPerPage = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                RouteValues = BuildRouteValues(filters)
            },

            CountryId = filters.CountryId,
            RegionId = filters.RegionId,
            CityId = filters.CityId,
            Street = filters.Street,
            CategoryId = filters.CategoryId,
            SubCategoryId = filters.SubCategoryId,
            DealType = filters.DealType,
            MinPrice = filters.MinPrice,
            MaxPrice = filters.MaxPrice,
            MinRooms = filters.MinRooms,
            MaxRooms = filters.MaxRooms,
            MinArea = filters.MinArea,
            MaxArea = filters.MaxArea,
            OrderBy = filters.OrderBy,
            HasGasHeating = filters.HasGasHeating,
            HasCentralWater = filters.HasCentralWater,
            HasCentralHeating = filters.HasCentralHeating,
            HasCentralScanalisation = filters.HasCentralScanalisation,
            HasInternet = filters.HasInternet,
            HasParking = filters.HasParking,

            CategoryCounts = await _cats.GetCategoryCountsAsync(filters),
            SubCategoryCounts = await _cats.GetSubCategoryCountsAsync(filters)
        };
    }

    // ✅ Реализация под твой интерфейс и модель
    public async Task<ListingDetailsViewModel?> BuildListingDetailsViewModelAsync(long id, string? dealType, string? returnUrl)
    {
        var listing = await _ctx.Listings
            .Include(l => l.ListingImages)
            .Include(l => l.ListingGeo)
            .Include(l => l.Category)
            .Include(l => l.SubCategory)
            .Include(l => l.ListingType)
            .FirstOrDefaultAsync(l => l.ListingId == id && l.IntShow);

        if (listing == null) return null;

        return new ListingDetailsViewModel(
            listing,
            listing.ListingImages.ToList(),
            returnUrl,
            listing.Category,
            listing.SubCategory,
            listing.ListingType
        );
    }

    private static string BuildFilterKey(ListingFilterParams f) =>
        $"_c{f.CategoryId}_sc{f.SubCategoryId}_r{f.RegionId}_ct{f.CityId}_s{f.Street}_co{f.CountryId}" +
        $"_minp{f.MinPrice}_maxp{f.MaxPrice}_minr{f.MinRooms}_maxr{f.MaxRooms}_mina{f.MinArea}_maxa{f.MaxArea}_o{f.OrderBy}" +
        $"_gas{f.HasGasHeating}_water{f.HasCentralWater}_heat{f.HasCentralHeating}_scan{f.HasCentralScanalisation}_net{f.HasInternet}_park{f.HasParking}";

    private static Dictionary<string, string> BuildRouteValues(ListingFilterParams f)
    {
        var dict = new Dictionary<string, string>();

        if (!string.IsNullOrEmpty(f.DealType))
            dict["dealType"] = f.DealType;

        foreach (var p in typeof(ListingFilterParams).GetProperties())
        {
            if (p.Name is "ListingTypeId" or "DealType") continue; // <- добавили "DealType" сюда

            var val = p.GetValue(f)?.ToString();
            if (!string.IsNullOrEmpty(val))
                dict[p.Name] = val;
        }
        return dict;
    }
}