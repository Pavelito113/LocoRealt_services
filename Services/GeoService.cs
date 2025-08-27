// Services/GeoService.cs
using LocoRealt.Data;
using LocoRealt.Models;
using LocoRealt.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class GeoService : IGeoService
{
    private readonly ApplicationDbContext _ctx;
    private readonly DistributedCacheService _cache;

    public GeoService(ApplicationDbContext ctx, DistributedCacheService cache)
    { _ctx = ctx; _cache = cache; }

    public async Task<(List<Country>, List<Region>, List<City>)>
        GetGeoListsAsync(long? countryId, long? regionId)
    {
        var countries = await _cache.GetOrCreateAsync("all_countries",
            async () => await _ctx.Countries.Where(c => c.IntShow).OrderBy(c => c.TxtTitle).ToListAsync(),
            TimeSpan.FromDays(1)) ?? new List<Country>();

        var regions = countryId.HasValue
            ? (await _cache.GetOrCreateAsync($"regions_{countryId}",
                async () => await _ctx.Regions.Where(r => r.IntShow && r.CountryId == countryId)
                    .OrderBy(r => r.TxtTitle).ToListAsync(),
                TimeSpan.FromHours(12))) ?? new List<Region>()
            : new List<Region>();

        var cities = regionId.HasValue
            ? (await _cache.GetOrCreateAsync($"cities_{regionId}",
                async () => await _ctx.Cities.Where(c => c.IntShow && c.RegionId == regionId)
                    .OrderBy(c => c.TxtTitle).ToListAsync(),
                TimeSpan.FromHours(12))) ?? new List<City>()
            : new List<City>();

        return (countries, regions, cities);
    }

    public async Task<GeoFiltersViewModel> LoadGeoFiltersAsync(
        long? countryId, long? regionId, long? cityId, string? txtAddress, string? dealType)
    {
        string cacheKey = $"geo:{countryId}:{regionId}:{cityId}:{txtAddress}:{dealType}";
        return await _cache.GetOrCreateAsync(cacheKey, async () =>
        {
            var query = _ctx.Listings
                .Include(l => l.ListingGeo)
                    .ThenInclude(g => g.Country)
                .Include(l => l.ListingGeo)
                    .ThenInclude(g => g.Region)
                .Include(l => l.ListingGeo)
                    .ThenInclude(g => g.City)
                .Where(l => l.IntShow);

            if (!string.IsNullOrEmpty(dealType))
            {
                if (dealType.Equals("sale", StringComparison.OrdinalIgnoreCase))
                    query = query.Where(l => l.IsForSale);
                else if (dealType.Equals("rent", StringComparison.OrdinalIgnoreCase))
                    query = query.Where(l => l.IsForRent);
            }

            if (countryId.HasValue)
                query = query.Where(l => l.ListingGeo != null && l.ListingGeo.CountryId == countryId);

            if (regionId.HasValue)
                query = query.Where(l => l.ListingGeo != null && l.ListingGeo.RegionId == regionId);

            if (cityId.HasValue)
                query = query.Where(l => l.ListingGeo != null && l.ListingGeo.CityId == cityId);

            if (!string.IsNullOrWhiteSpace(txtAddress))
                query = query.Where(l =>
                    l.ListingGeo != null &&
                    l.ListingGeo.TxtAddress != null &&
                    l.ListingGeo.TxtAddress.Contains(txtAddress));

            var filtered = await query.ToListAsync();

            // группировка только по ненулевым ID → ключ в словаре гарантированно long, а не long?
            var countryCounts = filtered
                .Where(l => l.ListingGeo?.CountryId != null)
                .GroupBy(l => l.ListingGeo!.CountryId!.Value)
                .ToDictionary(g => g.Key, g => g.Count());

            var regionCounts = filtered
                .Where(l => l.ListingGeo?.RegionId != null)
                .GroupBy(l => l.ListingGeo!.RegionId!.Value)
                .ToDictionary(g => g.Key, g => g.Count());

            var cityCounts = filtered
                .Where(l => l.ListingGeo?.CityId != null)
                .GroupBy(l => l.ListingGeo!.CityId!.Value)
                .ToDictionary(g => g.Key, g => g.Count());

            var (countries, regions, cities) = await GetGeoListsAsync(countryId, regionId);

            return new GeoFiltersViewModel
            {
                Countries = countries.Select(c =>
                    new SelectListItem { Value = c.CountryId.ToString(), Text = c.TxtTitle }),
                Regions = regions.Select(r =>
                    new SelectListItem { Value = r.RegionId.ToString(), Text = r.TxtTitle }),
                Cities = cities.Select(c =>
                    new SelectListItem { Value = c.CityId.ToString(), Text = c.TxtTitle }),
                CountryCounts = countryCounts,
                RegionCounts = regionCounts,
                CityCounts = cityCounts,
                SelectedCountryId = countryId,
                SelectedRegionId = regionId,
                SelectedCityId = cityId,
                TxtAddress = txtAddress
            };
        }, TimeSpan.FromMinutes(10));
    }
}
