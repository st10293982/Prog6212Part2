using Microsoft.AspNetCore.Mvc;
using Prog6212Part2.Data;
using Prog6212Part2.Models;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Prog6212Part2.Controllers
{
    //[Authorize(Roles = "Lecturer")]


    public class LecturerClaimsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<IdentityUser> _userManager;
        private ApplicationDbContext object1;
        private IWebHostEnvironment object2;

        //public LecturerClaimsController(ApplicationDbContext object1, IWebHostEnvironment object2)
        //{
        //    this.object1 = object1;
        //    this.object2 = object2;
        //}

        public LecturerClaimsController(ApplicationDbContext context, IWebHostEnvironment environment, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
        }

        // GET: LecturerClaims/Create
        public IActionResult Create()
        {
            return View();
        }

        //// POST: LecturerClaims/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("HourlyRate, HoursWorked, AdditionalNote")] LecturerClaim lecturerClaim, IFormFile supportingDocument)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (supportingDocument != null && supportingDocument.Length > 0)
        //        {
        //            // Get file extension and check if it's allowed
        //            var extension = Path.GetExtension(supportingDocument.FileName).ToLower();
        //            if (extension == ".pdf" || extension == ".docx" || extension == ".xlsx")
        //            {
        //                // Define the path to save the file
        //                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
        //                Directory.CreateDirectory(uploadsFolder); // Ensure the directory exists

        //                var uniqueFileName = Guid.NewGuid().ToString() + "_" + supportingDocument.FileName;
        //                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //                // Save the file to the server
        //                using (var stream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await supportingDocument.CopyToAsync(stream);
        //                }

        //                // Store the file path in the lecturer claim
        //                lecturerClaim.DocumentPath = "/uploads/" + uniqueFileName;
        //            }
        //            else
        //            {
        //                ModelState.AddModelError(string.Empty, "Only PDF, DOCX, and XLSX files are allowed.");
        //                return View(lecturerClaim);
        //            }
        //        }

        //        _context.Add(lecturerClaim);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(lecturerClaim);
        //}
        // POST: LecturerClaims/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HourlyRate, HoursWorked, AdditionalNote")] LecturerClaim lecturerClaim, IFormFile supportingDocument)
        {
            if (ModelState.IsValid)
            {
                // Get the logged-in user ID
                var userId = _userManager.GetUserId(User);
                lecturerClaim.UserId = userId;

                // File upload logic (as per your original code)
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

                _context.Add(lecturerClaim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lecturerClaim);
        }


        // GET: LecturerClaims/Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.LecturerClaims.ToListAsync());
        }
    }
}