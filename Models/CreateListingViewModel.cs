using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LocoRealt.Models
{
    public class CreateListingViewModel
    {
        public long ListingId { get; set; }
        public List<ListingImage> ExistingImages { get; set; } = new();

        // Basic listing info
        [Required]
        [Display(Name = "Заголовок")]
        public string Title { get; set; } = "";

        [Required]
        [Display(Name = "Описание")]
        public string Description { get; set; } = "";

        [Display(Name = "SEO Описание")]
        public string? MemSeodescription { get; set; }

        [Display(Name = "SEO Ключевые слова")]
        public string? MemSeokeywords { get; set; }

        [Required]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Тип объявления")]
        public long ListingTypeId { get; set; }

        [Display(Name = "Статус объявления")]
        public long ListingStatusId { get; set; } = 1;

        [Required]
        [Display(Name = "Тип сделки")]
        public string DealType { get; set; } = "";

        [Display(Name = "Торг")]
        public bool IsBargain { get; set; }

        [Display(Name = "Долгосрочная аренда")]
        public bool IsLongTerm { get; set; }

        // Categories
        [Required]
        [Display(Name = "Категория")]
        public long CategoryId { get; set; }

        [Required]
        [Display(Name = "Подкатегория")]
        public long SubCategoryId { get; set; }

        // Location info
        [Required]
        [Display(Name = "Страна")]
        public long? CountryId { get; set; }

        [Required]
        [Display(Name = "Регион")]
        public long? RegionId { get; set; }

        [Required]
        [Display(Name = "Город")]
        public long? CityId { get; set; }

        [Display(Name = "Адрес")]
        public string? Address { get; set; }

        [Display(Name = "Широта")]
        public decimal? Latitude { get; set; }

        [Display(Name = "Долгота")]
        public decimal? Longitude { get; set; }

        [Display(Name = "Показывать на Google Maps")]
        public bool ShowOnGoogleMap { get; set; } = true;

        // Amenities
        [Display(Name = "Газовое отопление")]
        public bool HasGasHeating { get; set; }

        [Display(Name = "Центральное водоснабжение")]
        public bool HasCentralWater { get; set; }

        [Display(Name = "Центральное отопление")]
        public bool HasCentralHeating { get; set; }

        [Display(Name = "Центральная канализация")]
        public bool HasCentralSCanalisation { get; set; }

        [Display(Name = "Интернет")]
        public bool HasInternet { get; set; }

        [Display(Name = "Парковка")]
        public bool HasParking { get; set; }

        // Common properties
        [Display(Name = "Количество комнат")]
        public int? Rooms { get; set; }

        [Display(Name = "Этаж")]
        public int? Floor { get; set; }

        [Display(Name = "Общая площадь")]
        public decimal? TotalArea { get; set; }

        [Display(Name = "Площадь участка")]
        public decimal? LandArea { get; set; }

        // Images
        [Display(Name = "Фотографии")]
        public List<IFormFile>? Images { get; set; }

        // Flat-specific
        public bool IsStudio { get; set; }
        public bool IsRoom { get; set; }
        public decimal? KitchenArea { get; set; }
        public decimal? LivingArea { get; set; }
        public string? BathroomType { get; set; }

        [Display(Name = "Балкон / лоджия")]
        public string? Balcony { get; set; } = "";

        // Список доступных значений для выпадающего списка
        public List<SelectListItem> BalconyOptions { get; set; } = new()
        {
            new SelectListItem { Value = "", Text = "— Выберите —" },
            new SelectListItem { Value = "Нет", Text = "Нет" },
            new SelectListItem { Value = "Балкон", Text = "Балкон" },
            new SelectListItem { Value = "Лоджия", Text = "Лоджия" }
        };

        public int? FloorNum { get; set; }
        public int? TotalFloors { get; set; }
        public string? WindowView { get; set; }
        public string? HeatingType { get; set; }
        public string? RepairState { get; set; }

        // House-specific
        public int? FloorCount { get; set; }
        public string? WallMaterial { get; set; }
        public decimal? HouseArea { get; set; }
        public string? LegalStatus { get; set; }
        public string? Condition { get; set; }
        public string? WaterSupply { get; set; }
        public string? Sewerage { get; set; }

        // Commercial-specific
        public string? Purpose { get; set; }
        public bool? HasHeating { get; set; }
        public bool? HasWater { get; set; }
        public bool? HasSewerage { get; set; }
        public bool? HasSecurity { get; set; }

        // Land plot-specific
        public bool? HasGas { get; set; }
        public bool? HasElectricity { get; set; }
        public bool? HasWaterWell { get; set; }
        public bool? HasSeptic { get; set; }

        // ** Добавляем SEO свойства **
        [Display(Name = "SEO Описание")]
        public string? MemSEODescription { get; set; }

        [Display(Name = "SEO Ключевые слова")]
        public string? MemSEOKeywords { get; set; }
    }
}
