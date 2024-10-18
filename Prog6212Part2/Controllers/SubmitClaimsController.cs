using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Prog6212Part2.Data;
using Prog6212Part2.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Prog6212Part2.Controllers
{
    //[Authorize(Roles = "Lecturer")]
    public class SubmitClaimsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public SubmitClaimsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        // Display the claim submission form
        public IActionResult Submit()
        {
            return View();
        }

        // Submit a new claim with supporting document
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit([Bind("HourlyRate, HoursWorked, AdditionalNote")] LecturerClaim lecturerClaim, IFormFile supportingDocument)
        {
            if (ModelState.IsValid)
            {
                if (supportingDocument != null && supportingDocument.Length > 0)
                {
                    var extension = Path.GetExtension(supportingDocument.FileName).ToLower();
                    if (extension == ".pdf" || extension == ".docx" || extension == ".xlsx")
                    {
                        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                        Directory.CreateDirectory(uploadsFolder);

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + supportingDocument.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await supportingDocument.CopyToAsync(stream);
                        }

                        lecturerClaim.DocumentPath = "/uploads/" + uniqueFileName;
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Only PDF, DOCX, and XLSX files are allowed.");
                        return View(lecturerClaim);
                    }
                }

                var userId = _userManager.GetUserId(User);
                lecturerClaim.UserId = userId;
                lecturerClaim.DateSubmitted = DateTime.Now;
                lecturerClaim.IsApproved = null;

                _context.Add(lecturerClaim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TrackClaimsController.Track), "TrackClaims");
            }
            return View(lecturerClaim);
        }
    }
}
