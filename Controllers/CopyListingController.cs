using LocoRealt.Data;
using LocoRealt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocoRealt.Controllers
{
    [Authorize]
    [Route("CopyListing")]
    public class CopyListingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CopyListingController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet("Copy/{id}")]
        public async Task<IActionResult> Copy(long id)
        {
            var original = await _context.Listings
                .Include(l => l.ListingImages)
                .FirstOrDefaultAsync(l => l.ListingId == id);

            if (original == null || original.UserId != User.Identity!.Name)
                return NotFound();

            var newListing = new Listing
            {
                TxtTitle = original.TxtTitle + " (копия)",
                MemDescription = original.MemDescription,
                DblPrice = original.DblPrice,
                UserId = original.UserId,
                DtCreated = DateTime.UtcNow
            };

            _context.Listings.Add(newListing);
            await _context.SaveChangesAsync();

            foreach (var image in original.ListingImages)
            {
                var newFileName = Guid.NewGuid() + Path.GetExtension(image.TxtImageUrl);
                var sourcePath = Path.Combine(_env.WebRootPath, "images", "listings", image.TxtImageUrl);
                var destPath = Path.Combine(_env.WebRootPath, "images", "listings", newFileName);

                if (System.IO.File.Exists(sourcePath))
                    System.IO.File.Copy(sourcePath, destPath);

                _context.ListingImages.Add(new ListingImage
                {
                    ListingId = newListing.ListingId,
                    TxtImageUrl = newFileName
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
