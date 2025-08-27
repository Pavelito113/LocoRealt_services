using Microsoft.AspNetCore.Mvc.Rendering;

namespace LocoRealt.Models
{
    public class GeoFiltersViewModel
    {
        public IEnumerable<SelectListItem> Countries { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Regions { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Cities { get; set; } = new List<SelectListItem>();

        public long? SelectedCountryId { get; set; }
        public long? SelectedRegionId { get; set; }
        public long? SelectedCityId { get; set; }
        public string? TxtAddress { get; set; }

        public Dictionary<long, int> CountryCounts { get; set; } = new();
        public Dictionary<long, int> RegionCounts { get; set; } = new();
        public Dictionary<long, int> CityCounts { get; set; } = new();
    }
}

