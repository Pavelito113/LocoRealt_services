using LocoRealt.Models;
using LocoRealt.Services;
using Microsoft.AspNetCore.Mvc;

namespace LocoRealt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly IGeoService _geo;
        private readonly ICategoryService _cats;
        private readonly IListingsService _listings;
        private readonly ILogger<ApiController> _logger;

        public ApiController(IGeoService geo, ICategoryService cats,
            IListingsService listings, ILogger<ApiController> logger)
        {
            _geo = geo;
            _cats = cats;
            _listings = listings;
            _logger = logger;
        }

        [HttpGet("geo-filters")]
        [ResponseCache(Duration = 300, VaryByQueryKeys = new[] { "*" })]
        public async Task<IActionResult> GetGeoFilters([FromQuery] ListingFilterParams filters)
        {
            try
            {
                var result = await _geo.LoadGeoFiltersAsync(
                    filters.CountryId, filters.RegionId, filters.CityId,
                    filters.Street, filters.DealType);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading geo filters");
                return StatusCode(500);
            }
        }

        [HttpGet("category-filters")]
        [ResponseCache(Duration = 300, VaryByQueryKeys = new[] { "*" })]
        public async Task<IActionResult> GetCategoryFilters([FromQuery] ListingFilterParams filters)
        {
            try
            {
                var result = await _cats.LoadCategoriesAsync(filters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading category filters");
                return StatusCode(500);
            }
        }

        [HttpGet("listings-count")]
        public async Task<IActionResult> GetListingsCount([FromQuery] ListingFilterParams filters)
        {
            try
            {
                var count = await _listings.GetListingsCountAsync(filters);
                return Ok(new { count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting listings count");
                return StatusCode(500);
            }
        }
    }
}