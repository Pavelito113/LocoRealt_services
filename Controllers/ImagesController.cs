using LocoRealt.Data;
using LocoRealt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocoRealt.Controllers
{
    [Authorize]
    [Route("Images")]
    public class ImagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ImagesController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ✅ Получение главного фото по объявлению
        [HttpGet("GetByListingId/{listingId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByListingId(long listingId)
        {
            var image = await _context.ListingImages
                .Where(i => i.ListingId == listingId)
                .OrderBy(i => i.IntSort)
                .Select(i => i.TxtImageUrl ?? i.TxtImage)
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(image))
                return await GetPlaceholderAsync();

            if (image.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return Redirect(image);

            var imagePath = Path.Combine(_env.WebRootPath, "images", "listings", image);
            if (!System.IO.File.Exists(imagePath))
                return await GetPlaceholderAsync();

            var ext = Path.GetExtension(imagePath).ToLowerInvariant();
            var mimeType = ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };

            var bytes = await System.IO.File.ReadAllBytesAsync(imagePath);
            return File(bytes, mimeType);
        }

        // ✅ Загрузка фото (для Create/Edit)
        [HttpPost("Upload/{listingId}")]
        public async Task<IActionResult> Upload(long listingId, List<IFormFile> files)
        {
            var listing = await _context.Listings
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.ListingId == listingId); // Changed from Id to ListingId

            if (listing == null || listing.User?.UserName != User.Identity?.Name)
                return Unauthorized();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(_env.WebRootPath, "images", "listings", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                        await file.CopyToAsync(stream);

                    _context.ListingImages.Add(new ListingImage
                    {
                        ListingId = listingId,
                        TxtImageUrl = fileName,
                        IntSort = 0
                    });
                }
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        // ✅ Удаление фото
        [HttpPost("Delete/{imageId}")]
        public async Task<IActionResult> Delete(long imageId)
        {
            var image = await _context.ListingImages.FindAsync(imageId);
            if (image == null)
                return NotFound();

            var listing = await _context.Listings
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.ListingId == image.ListingId); // Changed from Id to ListingId

            if (listing == null || listing.User?.UserName != User.Identity?.Name)
                return Unauthorized();

            var filePath = Path.Combine(_env.WebRootPath, "images", "listings", image.TxtImageUrl ?? "");
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            _context.ListingImages.Remove(image);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private async Task<IActionResult> GetPlaceholderAsync()
        {
            var placeholderPath = Path.Combine(_env.WebRootPath, "images", "no-image.png");
            if (!System.IO.File.Exists(placeholderPath))
                return NotFound("no-image.png not found");

            var bytes = await System.IO.File.ReadAllBytesAsync(placeholderPath);
            return File(bytes, "image/png");
        }
    }
}