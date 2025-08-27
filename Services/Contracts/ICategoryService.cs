using LocoRealt.Models;

public interface ICategoryService
{
    Task<CategoryListViewModel> LoadCategoriesAsync(ListingFilterParams filters);
    Task<Dictionary<long, int>> GetCategoryCountsAsync(ListingFilterParams filters);
    Task<Dictionary<long, int>> GetSubCategoryCountsAsync(ListingFilterParams filters);
    Task<(List<Category> Categories, List<SubCategory> SubCategories)> GetCategoryListsAsync();
}