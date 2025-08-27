using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LocoRealt.Models;

public class ListingDetailsViewModel
{
    // Обязательные свойства
    public required Listing Listing { get; set; }
    public required List<ListingImage> ListingImages { get; set; } = [];
    public required string DealType { get; set; }
    public required Category Category { get; set; }
    public required SubCategory SubCategory { get; set; }
    public required ListingType ListingType { get; set; }

    // Опциональные свойства
    public ListingGeo? ListingGeo { get; set; }
    public SpecFlat? SpecFlat { get; set; }
    public SpecHouse? SpecHouse { get; set; }
    public SpecCommercial? SpecCommercial { get; set; }
    public SpecLandPlot? SpecLandPlot { get; set; }
    public ListingStatus? ListingStatus { get; set; }
    public long? PrevListingId { get; set; }
    public long? NextListingId { get; set; }
    public required string? ReturnUrl { get; set; }
    public Country? Country { get; set; }
    public Region? Region { get; set; }
    public City? City { get; set; }


    // Словарь характеристик
    public Dictionary<string, string> SpecialCharacteristics { get; set; } = new();


    // Навигация и сервисные модели
    public BreadcrumbTrail? BreadcrumbTrail { get; set; }
    public CategoryListViewModel? CategoriesVm { get; set; }
    public GeoFiltersViewModel? GeoVm { get; set; }

    [SetsRequiredMembers]
    public ListingDetailsViewModel(
        Listing listing,
        List<ListingImage> images,
        string dealType,
        Category category,
        SubCategory subCategory,
        ListingType listingType)
    {
        Listing = listing;
        ListingImages = images;
        DealType = dealType;
        Category = category;
        SubCategory = subCategory;
        ListingType = listingType;
    }

  
    public void PopulateSpecialCharacteristics()
    {
        if (SpecFlat is not null)
        {
            SpecialCharacteristics["Количество комнат"] = SpecFlat.Rooms?.ToString() ?? "—";
            SpecialCharacteristics["Студия"] = SpecFlat.IsStudio == true ? "Да" : "Нет";
            SpecialCharacteristics["Комната"] = SpecFlat.IsRoom == true ? "Да" : "Нет";
            SpecialCharacteristics["Площадь кухни"] = SpecFlat.KitchenArea?.ToString("0.##") ?? "—";
            SpecialCharacteristics["Жилая площадь"] = SpecFlat.LivingArea?.ToString("0.##") ?? "—";
            SpecialCharacteristics["Общая площадь"] = SpecFlat.TotalArea?.ToString("0.##") ?? "—";
            SpecialCharacteristics["Санузел"] = SpecFlat.BathroomType ?? "—";
            SpecialCharacteristics["Балкон"] = string.IsNullOrWhiteSpace(SpecFlat.Balcony) ? "—" : SpecFlat.Balcony;
            SpecialCharacteristics["Этаж"] = SpecFlat.FloorNum?.ToString() ?? "—";
            SpecialCharacteristics["Этажей в доме"] = SpecFlat.TotalFloors?.ToString() ?? "—";
            SpecialCharacteristics["Вид из окон"] = SpecFlat.WindowView ?? "—";
            SpecialCharacteristics["Тип отопления"] = SpecFlat.HeatingType ?? "—";
            SpecialCharacteristics["Состояние ремонта"] = SpecFlat.RepairState ?? "—";
        }
    
        if (SpecHouse is not null)
        {
            SpecialCharacteristics["Этажей"] = SpecHouse.FloorCount?.ToString() ?? "—";
            SpecialCharacteristics["Материал стен"] = SpecHouse.WallMaterial ?? "—";
            SpecialCharacteristics["Площадь дома"] = SpecHouse.HouseArea?.ToString("0.##") ?? "—";
            SpecialCharacteristics["Площадь участка"] = SpecHouse.LandArea?.ToString("0.##") ?? "—";
            SpecialCharacteristics["Правовой статус"] = SpecHouse.LegalStatus ?? "—";
            SpecialCharacteristics["Состояние"] = SpecHouse.Condition ?? "—";
            SpecialCharacteristics["Отопление"] = SpecHouse.Heating ?? "—";
            SpecialCharacteristics["Водоснабжение"] = SpecHouse.WaterSupply ?? "—";
            SpecialCharacteristics["Канализация"] = SpecHouse.Sewerage ?? "—";
        }

        if (SpecCommercial is not null)
        {
            SpecialCharacteristics["Назначение"] = SpecCommercial.Purpose ?? "—";
            SpecialCharacteristics["Общая площадь"] = SpecCommercial.TotalArea?.ToString("0.##") ?? "—";
            SpecialCharacteristics["Этаж"] = SpecCommercial.FloorNum?.ToString() ?? "—";
            SpecialCharacteristics["Этажей в здании"] = SpecCommercial.TotalFloors?.ToString() ?? "—";
            SpecialCharacteristics["Состояние"] = SpecCommercial.Condition ?? "—";
            SpecialCharacteristics["Отопление"] = SpecCommercial.HasHeating == true ? "Да" : "Нет";
            SpecialCharacteristics["Вода"] = SpecCommercial.HasWater == true ? "Да" : "Нет";
            SpecialCharacteristics["Канализация"] = SpecCommercial.HasSewerage == true ? "Да" : "Нет";
            SpecialCharacteristics["Охрана"] = SpecCommercial.HasSecurity == true ? "Да" : "Нет";
        }

        if (SpecLandPlot is not null)
        {
            SpecialCharacteristics["Назначение"] = SpecLandPlot.Purpose ?? "—";
            SpecialCharacteristics["Газ"] = SpecLandPlot.HasGas == true ? "Да" : "Нет";
            SpecialCharacteristics["Электричество"] = SpecLandPlot.HasElectricity == true ? "Да" : "Нет";
            SpecialCharacteristics["Скважина"] = SpecLandPlot.HasWaterWell == true ? "Да" : "Нет";
            SpecialCharacteristics["Центральное водоснабжение"] = SpecLandPlot.HasCentralWater == true ? "Да" : "Нет";
            SpecialCharacteristics["Септик"] = SpecLandPlot.HasSeptic == true ? "Да" : "Нет";
        }

    }
    public string CategoryName => Category.TxtTitle ?? "Категория";
    public string SubCategoryName => SubCategory.TxtTitle ?? "Подкатегория";
    public long CategoryId => Category.CategoryId;
    public long SubCategoryId => SubCategory.SubCategoryId;

   }
