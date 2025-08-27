using LocoRealt.Data;
using LocoRealt.Models;
using LocoRealt.Models.Helpers;
using Microsoft.EntityFrameworkCore;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _ctx;

    public CategoryService(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task<(List<Category>, List<SubCategory>)> GetCategoryListsAsync()
    {
        var categories = await _ctx.Categories
            .Where(c => c.IntShow)
            .OrderBy(c => c.LngSort)
            .ToListAsync();

        var subCategories = await _ctx.SubCategories
            .Where(s => s.IntShow)
            .OrderBy(s => s.LngSort)
            .ToListAsync();

        return (categories, subCategories);
    }

    public async Task<Dictionary<long, int>> GetCategoryCountsAsync(ListingFilterParams filters)
    {
        var f = filters.Clone();
        // Сбрасываем только CategoryId и SubCategoryId, но сохраняем ВСЕ остальные фильтры
        f.CategoryId = null;
        f.SubCategoryId = null;

        return await _ctx.Listings
            .Include(l => l.ListingGeo)
            .Where(l => l.IntShow)
            .ApplyFilters(f) // Применяем все фильтры, включая ListingTypeId
            .GroupBy(l => l.CategoryId)
            .ToDictionaryAsync(g => (long)g.Key, g => g.Count());
    }

    public async Task<Dictionary<long, int>> GetSubCategoryCountsAsync(ListingFilterParams filters)
    {
        var f = filters.Clone();
        // Сбрасываем только CategoryId и SubCategoryId, но сохраняем ВСЕ остальные фильтры
        f.CategoryId = null;
        f.SubCategoryId = null;

        return await _ctx.Listings
            .Include(l => l.ListingGeo)
            .Where(l => l.IntShow)
            .ApplyFilters(f) // Применяем все фильтры, включая ListingTypeId
            .GroupBy(l => l.SubCategoryId)
            .ToDictionaryAsync(g => (long)g.Key, g => g.Count());
    }

    public async Task<CategoryListViewModel> LoadCategoriesAsync(ListingFilterParams filters)
    {
        var (cats, subs) = await GetCategoryListsAsync();
        var catCounts = await GetCategoryCountsAsync(filters);
        var subCounts = await GetSubCategoryCountsAsync(filters);

        var items = cats.Select(c => new CategoryItemViewModel
        {
            Id = c.CategoryId,
            Title = c.TxtTitle,
            IconClass = "fa-folder",
            Count = catCounts.GetValueOrDefault(c.CategoryId),
            SubCategories = subs
                .Where(sc => sc.CategoryId == c.CategoryId && sc.IntShow)
                .OrderBy(sc => sc.LngSort)
                .Select(sc => new SubCategoryItemViewModel
                {
                    Id = sc.SubCategoryId,
                    Title = sc.TxtTitle,
                    Count = subCounts.GetValueOrDefault(sc.SubCategoryId)
                }).ToList()
        }).ToList();

        return new CategoryListViewModel
        {
            Categories = items,
            CurrentCategoryId = filters.CategoryId ?? 0,
            CurrentSubCategoryId = filters.SubCategoryId ?? 0,
            CountryId = filters.CountryId?.ToString(),
            RegionId = filters.RegionId?.ToString(),
            CityId = filters.CityId?.ToString(),
            Street = filters.Street,
            DealType = filters.DealType ?? "sale" // Правильно передаем DealType
        };
    }
}