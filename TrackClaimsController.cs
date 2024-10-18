using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Prog6212Part2.Data;
using Prog6212Part2.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Prog6212Part2.Controllers
{
    //[Authorize(Roles = "Lecturer")]
    public class TrackClaimsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TrackClaimsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Display claims tracking page with progress bars
        public async Task<IActionResult> Track()
        {
            var userId = _userManager.GetUserId(User); // Get the current user ID
            var claims = await _context.LecturerClaims
                .Where(c => c.UserId == userId) // Filter claims by the current user
                .ToListAsync();

            return View(claims);
        }

        // Real-time status updates for tracking
        [HttpGet]
        public async Task<IActionResult> TrackStatus()
        {
            var userId = _userManager.GetUserId(User);
            var claims = await _context.LecturerClaims
                .Where(c => c.UserId == userId)
                .Select(c => new
                {
                    c.Id,
                    StatusClass = GetStatusClass(c),
                    Progress = GetProgress(c),
                    StatusText = GetStatusText(c)
                })
                .ToListAsync();

            return Json(claims);
        }

        private string GetStatusClass(LecturerClaim claim)
        {
            if (claim.IsApproved == null)
            {
                return "bg-warning"; // Pending
            }
            else if (claim.IsApproved == true)
            {
                return "bg-success"; // Approved
            }
            else
            {
                return "bg-danger"; // Rejected
            }
        }

        private int GetProgress(LecturerClaim claim)
        {
            if (claim.IsApproved == null)
            {
                return 50; // Pending
            }
            else
            {
                return 100; // Completed
            }
        }

        private string GetStatusText(LecturerClaim claim)
        {
            if (claim.IsApproved == null)
            {
                return "Pending";
            }
            else if (claim.IsApproved == true)
            {
                return "Approved";
            }
            else
            {
                return "Rejected";
            }
        }
    }
}
