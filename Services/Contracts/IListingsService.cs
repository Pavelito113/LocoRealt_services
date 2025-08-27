using LocoRealt.Models;

public interface IListingsService
{
    Task<ListingsViewModel> BuildListingsViewModelAsync(ListingFilterParams filters, int page, int pageSize);
    Task<List<ListingItemDto>> GetListingsPageAsync(ListingFilterParams filters, int page, int pageSize);
    Task<int> GetListingsCountAsync(ListingFilterParams filters);
    Task<ListingDetailsViewModel?> BuildListingDetailsViewModelAsync(long id, string? dealType, string? returnUrl);

}
