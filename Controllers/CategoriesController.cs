using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocoRealt.Data;
using LocoRealt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CategoriesController : Controller
{
    private readonly ICategoryService _cats;

    public CategoriesController(ICategoryService cats) => _cats = cats;

    [HttpGet, Route("Categories/LoadCategories")]
    public async Task<IActionResult> LoadCategories([FromQuery] ListingFilterParams filters)
    {
        var vm = await _cats.LoadCategoriesAsync(filters);
        return Json(vm);
    }
}
