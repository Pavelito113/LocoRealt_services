using LocoRealt.Models;
using LocoRealt.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LocoRealt.Controllers
{
    public class ListingsController : Controller
    {
        private readonly IListingsService _listings;
        private readonly IBreadcrumbService _breadcrumbs;
        private readonly ILogger<ListingsController> _logger;

        public ListingsController(IListingsService listings, IBreadcrumbService breadcrumbs,
            ILogger<ListingsController> logger)
        {
            _listings = listings;
            _breadcrumbs = breadcrumbs;
            _logger = logger;
        }

        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "id", "dealType" })]
        public async Task<IActionResult> Details(long id, string? dealType, string? returnUrl = null)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var viewModelTask = _listings.BuildListingDetailsViewModelAsync(id, dealType, returnUrl);
                var breadcrumbsTask = _breadcrumbs.BuildForListingAsync(id);

                await Task.WhenAll(viewModelTask, breadcrumbsTask);

                var viewModel = viewModelTask.Result;
                if (viewModel == null)
                {
                    return NotFound();
                }

                viewModel.BreadcrumbTrail = breadcrumbsTask.Result; // Исправлено на BreadcrumbTrail

                stopwatch.Stop();
                _logger.LogInformation("Listings/Details/{Id} loaded in {ElapsedMs}ms", id, stopwatch.ElapsedMilliseconds);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading listing details for id {ListingId}", id);
                return View("Error");
            }
        }
    }
}