using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace LocoRealt.Models
{
    public class ListingsViewModel
    {
        public IEnumerable<ListingItemDto> Listings { get; set; } = Enumerable.Empty<ListingItemDto>();

        public long? CategoryId { get; set; }
        public long? SubCategoryId { get; set; }

        // География (SelectList для UI)
        public SelectList Countries { get; set; } = new(Enumerable.Empty<object>());
        public SelectList Regions { get; set; } = new(Enumerable.Empty<object>());
        public SelectList Cities { get; set; } = new(Enumerable.Empty<object>());

        // Категории (SelectList + списки)
        public SelectList Categories { get; set; } = new(Enumerable.Empty<object>());
        public SelectList SubCategories { get; set; } = new(Enumerable.Empty<object>());
        public List<Category> CategoryList { get; set; } = new();
        public List<SubCategory> SubCategoryList { get; set; } = new();

        // Выбранные значения фильтров
        public long? CountryId { get; set; } = 1;
        public long? RegionId { get; set; }
        public long? CityId { get; set; }
        public string? Street { get; set; }
        public string? DealType { get; set; }
        public List<string> DealTypes { get; set; } = new();
        public Dictionary<long, int> CategoryCounts { get; set; } = new();
        public Dictionary<long, int> SubCategoryCounts { get; set; } = new();

        // Доп. фильтры
        public bool? IsForSale { get; set; }
        public bool? IsForRent { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinRooms { get; set; }
        public int? MaxRooms { get; set; }
        public decimal? MinArea { get; set; }
        public decimal? MaxArea { get; set; }
        public bool? HasGasHeating { get; set; }
        public bool? HasCentralWater { get; set; }
        public bool? HasCentralHeating { get; set; }
        public bool? HasCentralScanalisation { get; set; }
        public bool? HasInternet { get; set; }
        public bool? HasParking { get; set; }

        // Сортировка
        public string? OrderBy { get; set; }

        // Навигация
        public PaginationModel Pagination { get; set; } = new();

        // UI хелперы
        public ListingImage ViewImages { get; set; } = new();
        public ListingFilterParams Filters { get; set; } = new();

        // Новые свойства для сервисов
        public GeoFiltersViewModel? GeoVm { get; set; }
        public CategoryListViewModel? CategoriesVm { get; set; }
        public BreadcrumbTrail? BreadcrumbTrail { get; set; }
        public Dictionary<string, int>? DealTypeCounts { get; set; }


    }
}
