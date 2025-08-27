using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocoRealt.Data;
using LocoRealt.Models;
using LocoRealt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class GeoController : Controller
{
    private readonly IGeoService _geo;

    public GeoController(IGeoService geo) => _geo = geo;

    [HttpGet, Route("Geo/LoadGeoFilters")]
    public async Task<IActionResult> LoadGeoFilters(
        long? countryId, long? regionId, long? cityId, string? txtAddress, string? dealType)
    {
        var vm = await _geo.LoadGeoFiltersAsync(countryId, regionId, cityId, txtAddress, dealType);
        return PartialView("_GeoFilters", vm);
    }
}
