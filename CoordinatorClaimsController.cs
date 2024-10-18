using Microsoft.AspNetCore.Mvc;
using Prog6212Part2.Data;
using Prog6212Part2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Prog6212Part2.Controllers
{
    [Authorize(Roles = "Coordinator,Manager")]
    public class CoordinatorClaimsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoordinatorClaimsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CoordinatorClaims/Pending
        public async Task<IActionResult> Pending()
        {
            // Retrieve only claims that are pending (IsApproved is null)
            var pendingClaims = await _context.LecturerClaims
                .Where(c => c.IsApproved == null)
                .ToListAsync();

            return View(pendingClaims);
        }

        // POST: CoordinatorClaims/Approve/5
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var claim = await _context.LecturerClaims.FindAsync(id);
            if (claim == null)
            {
                return NotFound();
            }

            claim.IsApproved = true;
            claim.ReviewedBy = User.Identity.Name;  // Stores the name of the reviewer
            claim.ReviewedOn = DateTime.Now;

            _context.Update(claim);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Pending));
        }

        // POST: CoordinatorClaims/Reject/5
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var claim = await _context.LecturerClaims.FindAsync(id);
            if (claim == null)
            {
                return NotFound();
            }

            claim.IsApproved = false;
            claim.ReviewedBy = User.Identity.Name;  // Stores the name of the reviewer
            claim.ReviewedOn = DateTime.Now;

            _context.Update(claim);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Pending));
        }
    }
}
