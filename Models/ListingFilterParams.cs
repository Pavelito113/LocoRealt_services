namespace LocoRealt.Models
{
    public class ListingFilterParams
    {
        // География
        public long? CountryId { get; set; }
        public long? RegionId { get; set; }
        public long? CityId { get; set; }
        public string? Street { get; set; }

        // Категории
        public long? CategoryId { get; set; }
        public long? SubCategoryId { get; set; }

        // Тип сделки
        public string? DealType { get; set; } // "sale" или "rent"
        public int? ListingTypeId { get; set; }

        // Цена
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // Комнаты
        public int? MinRooms { get; set; }
        public int? MaxRooms { get; set; }

        // Площадь
        public decimal? MinArea { get; set; }
        public decimal? MaxArea { get; set; }

        // Коммуникации
        public bool? HasGasHeating { get; set; }
        public bool? HasCentralWater { get; set; }
        public bool? HasCentralHeating { get; set; }
        public bool? HasCentralScanalisation { get; set; }
        public bool? HasInternet { get; set; }
        public bool? HasParking { get; set; }

        // Сортировка
        public string? OrderBy { get; set; }

        public bool HasFilters =>
            CountryId.HasValue || RegionId.HasValue || CityId.HasValue ||
            !string.IsNullOrEmpty(Street) || CategoryId.HasValue ||
            SubCategoryId.HasValue || !string.IsNullOrEmpty(DealType) ||
            MinPrice.HasValue || MaxPrice.HasValue || MinRooms.HasValue ||
            MaxRooms.HasValue || MinArea.HasValue || MaxArea.HasValue ||
            HasGasHeating.HasValue || HasCentralWater.HasValue ||
            HasCentralHeating.HasValue || HasCentralScanalisation.HasValue ||
            HasInternet.HasValue || HasParking.HasValue;

        public ListingFilterParams Clone()
        {
            return new ListingFilterParams
            {
                CountryId = this.CountryId,
                RegionId = this.RegionId,
                CityId = this.CityId,
                Street = this.Street,
                CategoryId = this.CategoryId,
                SubCategoryId = this.SubCategoryId,

                DealType = this.DealType,
                ListingTypeId = this.ListingTypeId, // ✅ теперь сохраняется

                MinPrice = this.MinPrice,
                MaxPrice = this.MaxPrice,
                MinRooms = this.MinRooms,
                MaxRooms = this.MaxRooms,
                MinArea = this.MinArea,
                MaxArea = this.MaxArea,

                HasGasHeating = this.HasGasHeating,
                HasCentralWater = this.HasCentralWater,
                HasCentralHeating = this.HasCentralHeating,
                HasCentralScanalisation = this.HasCentralScanalisation,
                HasInternet = this.HasInternet,
                HasParking = this.HasParking,

                OrderBy = this.OrderBy
            };
        }
    }
}
