using LocoRealt.Data;
using LocoRealt.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LocoRealt.Models.Helpers
{
    
    public static class ListingFilterExtensions
    {
        public static IQueryable<Listing> ApplyFilters(this IQueryable<Listing> query, ListingFilterParams filters)
        {
            if (filters.ListingTypeId.HasValue)
            {
                query = query.Where(l => l.ListingTypeId == filters.ListingTypeId.Value);
            }

            if (filters == null) return query;

            // Геофильтры
            if (filters.CountryId.HasValue)
                query = query.Where(l => l.ListingGeo != null && l.ListingGeo.CountryId == filters.CountryId);

            if (filters.RegionId.HasValue)
                query = query.Where(l => l.ListingGeo != null && l.ListingGeo.RegionId == filters.RegionId);

            if (filters.CityId.HasValue)
                query = query.Where(l => l.ListingGeo != null && l.ListingGeo.CityId == filters.CityId);

            if (!string.IsNullOrWhiteSpace(filters.Street))
                query = query.Where(l => l.ListingGeo != null &&
                                         l.ListingGeo.TxtAddress != null &&
                                         l.ListingGeo.TxtAddress.Contains(filters.Street));

            // Категории
            if (filters.CategoryId.HasValue)
                query = query.Where(l => l.CategoryId == filters.CategoryId);

            if (filters.SubCategoryId.HasValue)
                query = query.Where(l => l.SubCategoryId == filters.SubCategoryId);

       
            // Цена
            if (filters.MinPrice.HasValue)
                query = query.Where(l => l.DblPrice >= filters.MinPrice);

            if (filters.MaxPrice.HasValue)
                query = query.Where(l => l.DblPrice <= filters.MaxPrice);

            // Комнаты
            if (filters.MinRooms.HasValue)
                query = query.Where(l => l.Rooms >= filters.MinRooms);

            if (filters.MaxRooms.HasValue)
                query = query.Where(l => l.Rooms <= filters.MaxRooms);

            // Площадь
            if (filters.MinArea.HasValue)
                query = query.Where(l => l.TotalArea >= filters.MinArea);

            if (filters.MaxArea.HasValue)
                query = query.Where(l => l.TotalArea <= filters.MaxArea);

            // Коммуникации
            if (filters.HasGasHeating.HasValue)
                query = query.Where(l => l.HasGasHeating == filters.HasGasHeating.Value);

            if (filters.HasCentralWater.HasValue)
                query = query.Where(l => l.HasCentralWater == filters.HasCentralWater.Value);

            if (filters.HasCentralHeating.HasValue)
                query = query.Where(l => l.HasCentralHeating == filters.HasCentralHeating.Value);

            if (filters.HasCentralScanalisation.HasValue)
                query = query.Where(l => l.HasCentralSCanalisation == filters.HasCentralScanalisation.Value);

            if (filters.HasInternet.HasValue)
                query = query.Where(l => l.HasInternet == filters.HasInternet.Value);

            if (filters.HasParking.HasValue)
                query = query.Where(l => l.HasParking == filters.HasParking.Value);

            return query;
        }

        public static IQueryable<Listing> ApplySorting(this IQueryable<Listing> query, string? orderBy)
        {
            return orderBy?.ToLower() switch
            {
                "price_asc" => query.OrderBy(l => l.DblPrice),
                "price_desc" => query.OrderByDescending(l => l.DblPrice),
                "date" => query.OrderByDescending(l => l.DtCreated),
                "area_asc" => query.OrderBy(l => l.TotalArea),
                "area_desc" => query.OrderByDescending(l => l.TotalArea),
                _ => query.OrderByDescending(l => l.DtCreated)
            };
        }

        public static SelectList ToSelectList<T>(this IEnumerable<T> items, string valueField, string textField, object? selectedValue = null)
        {
            return new SelectList(items ?? [], valueField, textField, selectedValue);
        }
    }
}