using LocoRealt.Models;

public interface IGeoService
{
    Task<GeoFiltersViewModel> LoadGeoFiltersAsync(
        long? countryId, long? regionId, long? cityId,
        string? txtAddress, string? dealType);

    Task<(List<Country> Countries, List<Region> Regions, List<City> Cities)>
        GetGeoListsAsync(long? countryId, long? regionId);
}