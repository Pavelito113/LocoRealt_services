using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using LocoRealt.Data;
using LocoRealt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace LocoRealt.Controllers
{
    [Authorize]
    [Route("EditListing")]
    public class EditListingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EditListingController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: show edit form
        [HttpGet("Edit/{id:long}")]
        public async Task<IActionResult> Edit(long id)
        {
            var listing = await _context.Listings
                .Include(l => l.ListingGeo)
                .Include(l => l.ListingImages)
                .Include(l => l.SpecFlat)
                .Include(l => l.SpecHouse)
                .Include(l => l.SpecCommercial)
                .Include(l => l.SpecLandPlot)
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.ListingId == id);

            if (listing == null) return NotFound();

            // only owner or admin
            if (listing.UserId != User.Identity!.Name && !User.IsInRole("Admin"))
                return Forbid();

            // map to view model
            var model = MapListingToViewModel(listing);

            // build SpecialCharacteristics dictionary
            var detailsVm = await BuildListingDetailsViewModel(listing);
            detailsVm.PopulateSpecialCharacteristics();
            ViewBag.SpecialCharacteristics = detailsVm.SpecialCharacteristics;

            // load dropdowns
            await LoadDropdownDataAsync();

            return View("Edit", model);
        }

        // POST: save changes
        [HttpPost("Edit/{id:long}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, CreateListingViewModel model, List<long>? deleteImages)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownDataAsync();
                return View("Edit", model);
            }

            var listing = await _context.Listings
                .Include(l => l.ListingGeo)
                .Include(l => l.ListingImages)
                .Include(l => l.SpecFlat)
                .Include(l => l.SpecHouse)
                .Include(l => l.SpecCommercial)
                .Include(l => l.SpecLandPlot)
                .FirstOrDefaultAsync(l => l.ListingId == id);

            if (listing == null) return NotFound();

            // only owner or admin
            if (listing.UserId != User.Identity!.Name && !User.IsInRole("Admin"))
                return Forbid();

            // Update basic listing info
            UpdateListingFromViewModel(listing, model);

            // Update or create geo info
            await UpdateListingGeoAsync(listing, model);

            // Handle images
            await HandleImageUpdatesAsync(listing.ListingId, model, deleteImages);

            // Update special characteristics based on category
            await UpdateSpecialCharacteristicsAsync(listing, model);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Listings", new { id = listing.ListingId });
        }

        private void UpdateListingFromViewModel(Listing listing, CreateListingViewModel model)
        {
            listing.TxtTitle = model.Title ?? string.Empty;
            listing.MemDescription = model.Description;
            listing.MemSEODescription = model.MemSEODescription;
            listing.MemSEOKeywords = model.MemSEOKeywords;
            listing.DblPrice = model.Price;
            listing.ListingTypeId = (int)model.ListingTypeId;
            listing.ListingStatusId = model.ListingStatusId;
            listing.IsForSale = model.DealType == "sale";
            listing.IsForRent = model.DealType == "rent";
            listing.IsBargain = model.IsBargain;
            listing.IsLongTerm = model.IsLongTerm;
            listing.CategoryId = model.CategoryId;
            listing.SubCategoryId = model.SubCategoryId;

            // Common amenities
            listing.HasGasHeating = model.HasGasHeating;
            listing.HasCentralWater = model.HasCentralWater;
            listing.HasCentralHeating = model.HasCentralHeating;
            listing.HasCentralSCanalisation = model.HasCentralSCanalisation;
            listing.HasInternet = model.HasInternet;
            listing.HasParking = model.HasParking;

            // Common properties
            listing.Rooms = model.Rooms;
            listing.Floor = model.Floor;
            listing.TotalArea = model.TotalArea;
            listing.LandArea = model.LandArea;
        }

        private async Task UpdateListingGeoAsync(Listing listing, CreateListingViewModel model)
        {
            if (listing.ListingGeo == null)
            {
                listing.ListingGeo = new ListingGeo
                {
                    ListingId = listing.ListingId
                };
                await _context.ListingGeos.AddAsync(listing.ListingGeo);
            }

            listing.ListingGeo.CountryId = model.CountryId;
            listing.ListingGeo.RegionId = model.RegionId;
            listing.ListingGeo.CityId = model.CityId;
            listing.ListingGeo.TxtAddress = model.Address;
            listing.ListingGeo.DblLatitude = model.Latitude;
            listing.ListingGeo.DblLongitude = model.Longitude;
            listing.ListingGeo.IntGoogleMap = model.ShowOnGoogleMap;
        }

        private async Task HandleImageUpdatesAsync(long listingId, CreateListingViewModel model, List<long>? deleteImages)
        {
            var imagesController = new ImagesController(_context, _env)
            {
                ControllerContext = this.ControllerContext
            };

            // Delete images if any
            if (deleteImages?.Any() == true)
            {
                foreach (var imgId in deleteImages)
                {
                    await imagesController.Delete(imgId);
                }
            }

            // Upload new images
            if (model.Images != null && model.Images.Any())
            {
                await imagesController.Upload(listingId, model.Images);
            }
        }

        private async Task UpdateSpecialCharacteristicsAsync(Listing listing, CreateListingViewModel model)
        {
            switch (model.CategoryId)
            {
                case 1: // Flat
                    await UpdateFlatSpecAsync(listing, model);
                    break;
                case 2: // House
                    await UpdateHouseSpecAsync(listing, model);
                    break;
                case 3: // Commercial
                    await UpdateCommercialSpecAsync(listing, model);
                    break;
                case 4: // Land
                    await UpdateLandPlotSpecAsync(listing, model);
                    break;
            }
        }

        private async Task UpdateFlatSpecAsync(Listing listing, CreateListingViewModel model)
        {
            if (listing.SpecFlat == null)
            {
                listing.SpecFlat = new SpecFlat { ListingId = listing.ListingId };
                await _context.SpecFlats.AddAsync(listing.SpecFlat);
            }

            listing.SpecFlat.Rooms = model.Rooms;
            listing.SpecFlat.IsStudio = model.IsStudio;
            listing.SpecFlat.IsRoom = model.IsRoom;
            listing.SpecFlat.KitchenArea = model.KitchenArea;
            listing.SpecFlat.LivingArea = model.LivingArea;
            listing.SpecFlat.TotalArea = model.TotalArea;
            listing.SpecFlat.BathroomType = model.BathroomType;
            listing.SpecFlat.Balcony = model.Balcony;
            listing.SpecFlat.FloorNum = model.FloorNum;
            listing.SpecFlat.TotalFloors = model.TotalFloors;
            listing.SpecFlat.WindowView = model.WindowView;
            listing.SpecFlat.HeatingType = model.HeatingType;
            listing.SpecFlat.RepairState = model.RepairState;
        }

        private async Task UpdateHouseSpecAsync(Listing listing, CreateListingViewModel model)
        {
            if (listing.SpecHouse == null)
            {
                listing.SpecHouse = new SpecHouse { ListingId = listing.ListingId };
                await _context.SpecHouses.AddAsync(listing.SpecHouse);
            }

            listing.SpecHouse.FloorCount = model.FloorCount;
            listing.SpecHouse.WallMaterial = model.WallMaterial;
            listing.SpecHouse.HouseArea = model.HouseArea;
            listing.SpecHouse.LandArea = model.LandArea;
            listing.SpecHouse.LegalStatus = model.LegalStatus;
            listing.SpecHouse.Condition = model.Condition;
            listing.SpecHouse.Heating = model.HeatingType;
            listing.SpecHouse.WaterSupply = model.WaterSupply;
            listing.SpecHouse.Sewerage = model.Sewerage;
        }

        private async Task UpdateCommercialSpecAsync(Listing listing, CreateListingViewModel model)
        {
            if (listing.SpecCommercial == null)
            {
                listing.SpecCommercial = new SpecCommercial { ListingId = listing.ListingId };
                await _context.SpecCommercials.AddAsync(listing.SpecCommercial);
            }

            listing.SpecCommercial.Purpose = model.Purpose;
            listing.SpecCommercial.TotalArea = model.TotalArea;
            listing.SpecCommercial.FloorNum = model.FloorNum;
            listing.SpecCommercial.TotalFloors = model.TotalFloors;
            listing.SpecCommercial.Condition = model.Condition;
            listing.SpecCommercial.HasHeating = model.HasHeating;
            listing.SpecCommercial.HasWater = model.HasWater;
            listing.SpecCommercial.HasSewerage = model.HasSewerage;
            listing.SpecCommercial.HasSecurity = model.HasSecurity;
        }

        private async Task UpdateLandPlotSpecAsync(Listing listing, CreateListingViewModel model)
        {
            if (listing.SpecLandPlot == null)
            {
                listing.SpecLandPlot = new SpecLandPlot { ListingId = listing.ListingId };
                await _context.SpecLandPlots.AddAsync(listing.SpecLandPlot);
            }

            listing.SpecLandPlot.Purpose = model.Purpose;
            listing.SpecLandPlot.HasGas = model.HasGas;
            listing.SpecLandPlot.HasElectricity = model.HasElectricity;
            listing.SpecLandPlot.HasWaterWell = model.HasWaterWell;
            listing.SpecLandPlot.HasCentralWater = model.HasCentralWater;
            listing.SpecLandPlot.HasSeptic = model.HasSeptic;
        }

        private CreateListingViewModel MapListingToViewModel(Listing listing)
        {
            var model = new CreateListingViewModel
            {
                ListingId = listing.ListingId,
                Title = listing.TxtTitle ?? string.Empty,
                Description = listing.MemDescription,
                MemSEODescription = listing.MemSEODescription,
                MemSEOKeywords = listing.MemSEOKeywords,
                Price = listing.DblPrice,
                ListingTypeId = listing.ListingTypeId,
                ListingStatusId = listing.ListingStatusId,
                DealType = listing.IsForSale ? "sale" : listing.IsForRent ? "rent" : string.Empty,
                IsBargain = listing.IsBargain,
                IsLongTerm = listing.IsLongTerm,
                CategoryId = listing.CategoryId,
                SubCategoryId = listing.SubCategoryId,
                ExistingImages = listing.ListingImages?.ToList() ?? new List<ListingImage>(),

                // Common amenities
                HasGasHeating = listing.HasGasHeating,
                HasCentralWater = listing.HasCentralWater,
                HasCentralHeating = listing.HasCentralHeating,
                HasCentralSCanalisation = listing.HasCentralSCanalisation,
                HasInternet = listing.HasInternet,
                HasParking = listing.HasParking,

                // Common properties
                Rooms = listing.Rooms,
                Floor = listing.Floor,
                TotalArea = listing.TotalArea,
                LandArea = listing.LandArea
            };

            // Geo info
            if (listing.ListingGeo != null)
            {
                model.CountryId = listing.ListingGeo.CountryId;
                model.RegionId = listing.ListingGeo.RegionId;
                model.CityId = listing.ListingGeo.CityId;
                model.Address = listing.ListingGeo.TxtAddress;
                model.Latitude = listing.ListingGeo.DblLatitude;
                model.Longitude = listing.ListingGeo.DblLongitude;
                model.ShowOnGoogleMap = listing.ListingGeo.IntGoogleMap;
            }

            // Category-specific properties
            if (listing.SpecFlat != null)
            {
                model.IsStudio = listing.SpecFlat.IsStudio ?? false;
                model.IsRoom = listing.SpecFlat.IsRoom ?? false;
                model.KitchenArea = listing.SpecFlat.KitchenArea;
                model.LivingArea = listing.SpecFlat.LivingArea;
                model.BathroomType = listing.SpecFlat.BathroomType;
                model.Balcony = listing.SpecFlat.Balcony;
                model.FloorNum = listing.SpecFlat.FloorNum;
                model.TotalFloors = listing.SpecFlat.TotalFloors;
                model.WindowView = listing.SpecFlat.WindowView;
                model.HeatingType = listing.SpecFlat.HeatingType;
                model.RepairState = listing.SpecFlat.RepairState;
            }

            if (listing.SpecHouse != null)
            {
                model.FloorCount = listing.SpecHouse.FloorCount;
                model.WallMaterial = listing.SpecHouse.WallMaterial;
                model.HouseArea = listing.SpecHouse.HouseArea;
                model.LegalStatus = listing.SpecHouse.LegalStatus;
                model.Condition = listing.SpecHouse.Condition;
                model.HeatingType = listing.SpecHouse.Heating;
                model.WaterSupply = listing.SpecHouse.WaterSupply;
                model.Sewerage = listing.SpecHouse.Sewerage;
            }

            if (listing.SpecCommercial != null)
            {
                model.Purpose = listing.SpecCommercial.Purpose;
                model.HasHeating = listing.SpecCommercial.HasHeating;
                model.HasWater = listing.SpecCommercial.HasWater;
                model.HasSewerage = listing.SpecCommercial.HasSewerage;
                model.HasSecurity = listing.SpecCommercial.HasSecurity;
            }

            if (listing.SpecLandPlot != null)
            {
                model.Purpose = listing.SpecLandPlot.Purpose;
                model.HasGas = listing.SpecLandPlot.HasGas ?? false;
                model.HasElectricity = listing.SpecLandPlot.HasElectricity ?? false;
                model.HasWaterWell = listing.SpecLandPlot.HasWaterWell ?? false;
                model.HasCentralWater = listing.SpecLandPlot.HasCentralWater ?? false;
                model.HasSeptic = listing.SpecLandPlot.HasSeptic ?? false;
            }

            return model;
        }

        private async Task LoadDropdownDataAsync()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.SubCategories = await _context.SubCategories.ToListAsync();
            ViewBag.Countries = await _context.Countries.ToListAsync();
            ViewBag.Regions = await _context.Regions.ToListAsync();
            ViewBag.Cities = await _context.Cities.ToListAsync();
            ViewBag.ListingTypes = await _context.ListingTypes.ToListAsync();
            ViewBag.ListingStatuses = await _context.ListingStatuses.ToListAsync();
        }

        private async Task<ListingDetailsViewModel> BuildListingDetailsViewModel(Listing listing)
        {
            var images = listing.ListingImages?.ToList() ?? new List<ListingImage>();
            var dealType = listing.IsForSale ? "Продажа" : listing.IsForRent ? "Аренда" : string.Empty;
            var category = await _context.Categories.FindAsync(listing.CategoryId) ?? new Category { TxtTitle = "—", CategoryId = listing.CategoryId };
            var subCategory = await _context.SubCategories.FindAsync(listing.SubCategoryId) ?? new SubCategory { TxtTitle = "—", SubCategoryId = listing.SubCategoryId };
            var listingType = await _context.ListingTypes.FindAsync(listing.ListingTypeId) ?? new ListingType();

            return new ListingDetailsViewModel(listing, images, dealType, category, subCategory, listingType)
            {
                ListingGeo = listing.ListingGeo,
                SpecFlat = listing.SpecFlat,
                SpecHouse = listing.SpecHouse,
                SpecCommercial = listing.SpecCommercial,
                SpecLandPlot = listing.SpecLandPlot
            };
        }
    }
}