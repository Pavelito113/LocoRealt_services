using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocoRealt.Controllers
{
    [Authorize]
    [Route("Listings")]
    public class ManageListingsController : Controller
    {
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return RedirectToAction("Create", "CreateListing");
        }

        [HttpGet("Edit/{id}")]
        public IActionResult Edit(long id)
        {
            return RedirectToAction("Edit", "EditListing", new { id });
        }

        [HttpGet("Delete/{id}")]
        public IActionResult Delete(long id)
        {
            return RedirectToAction("Delete", "DeleteListing", new { id });
        }

        [HttpGet("Copy/{id}")]
        public IActionResult Copy(long id)
        {
            return RedirectToAction("Copy", "CopyListing", new { id });
        }
    }
}
