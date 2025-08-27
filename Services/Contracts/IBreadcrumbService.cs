
using LocoRealt.Models;

public class BreadcrumbItem
{
    public string Title { get; set; } = "";
    public string? Url { get; set; }
    public bool IsActive { get; set; }
    public string? IconClass { get; set; } // ✅ Новое поле для FontAwesome-иконки
}
public class BreadcrumbTrail
{
    public List<BreadcrumbItem> Items { get; set; } = new();
}

public interface IBreadcrumbService
{
    Task<BreadcrumbTrail> BuildForFiltersAsync(ListingFilterParams filters);
    Task<BreadcrumbTrail> BuildForListingAsync(long listingId);
}