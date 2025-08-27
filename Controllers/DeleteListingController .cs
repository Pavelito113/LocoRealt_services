using LocoRealt.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocoRealt.Controllers
{
    [Authorize]
    [Route("DeleteListing")]
    public class DeleteListingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public DeleteListingController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var listing = await _context.Listings
                .Include(l => l.ListingImages)
                .FirstOrDefaultAsync(l => l.ListingId == id);

            if (listing == null || listing.UserId != User.Identity!.Name)
                return NotFound();

            foreach (var image in listing.ListingImages)
            {
                var path = Path.Combine(_env.WebRootPath, "images", "listings", image.TxtImageUrl);
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            _context.Listings.Remove(listing);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
