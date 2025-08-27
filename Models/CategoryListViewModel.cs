using System;
using System.Collections.Generic;

namespace LocoRealt.Models;

public class CategoryListViewModel
{
    public required List<CategoryItemViewModel> Categories { get; set; }
    public long CurrentCategoryId { get; set; }
    public long CurrentSubCategoryId { get; set; }

    // Фильтры
    public string? CountryId { get; set; }
    public string? RegionId { get; set; }
    public string? CityId { get; set; }
    public string? Street { get; set; }

    // Тип сделки: "sale" или "rent"
    public string DealType { get; set; } = string.Empty;

    // Удобные вычисляемые свойства
    public bool IsSale => string.IsNullOrEmpty(DealType) || DealType.Equals("sale", StringComparison.OrdinalIgnoreCase);
    public bool IsRentMode => DealType.Equals("rent", StringComparison.OrdinalIgnoreCase);

   
}

public class CategoryItemViewModel
{
    public required long Id { get; set; }
    public required string Title { get; set; }
    public required string IconClass { get; set; }
    public int Count { get; set; }
    public required List<SubCategoryItemViewModel> SubCategories { get; set; }
}

public class SubCategoryItemViewModel
{
    public required long Id { get; set; }
    public required string Title { get; set; }
    public int Count { get; set; }
}
