using LocoRealt.Data;
using LocoRealt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocoRealt.Controllers
{
    [Authorize]
    [Route("CreateListing")]
    public class CreateListingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CreateListingController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View(new CreateListingViewModel());
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateListingViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var listing = new Listing
            {
                TxtTitle = model.Title,
                MemDescription = model.Description,
                DblPrice = model.Price,
                UserId = User.Identity!.Name ?? "",
                DtCreated = DateTime.UtcNow
            };

            _context.Listings.Add(listing);
            await _context.SaveChangesAsync();

            if (model.Images != null)
            {
                foreach (var file in model.Images)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(_env.WebRootPath, "images", "listings", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    _context.ListingImages.Add(new ListingImage
                    {
                        ListingId = listing.ListingId,
                        TxtImageUrl = fileName
                    });
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
