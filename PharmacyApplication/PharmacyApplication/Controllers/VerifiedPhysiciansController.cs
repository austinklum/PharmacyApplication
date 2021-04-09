using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PharmacyApplication.Data;
using PharmacyApplication.Models;

namespace PharmacyApplication.Controllers
{
    public class VerifiedPhysiciansController : Controller
    {
        private readonly VerificationContext _context;

        public VerifiedPhysiciansController(VerificationContext context)
        {
            _context = context;
        }

        // GET: VerifiedPhysicians
        public async Task<IActionResult> Index()
        {
            return View(await _context.VerifiedPhysicians.ToListAsync());
        }

        // GET: VerifiedPhysicians/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var verifiedPhysician = await _context.VerifiedPhysicians
                .FirstOrDefaultAsync(m => m.Id == id);
            if (verifiedPhysician == null)
            {
                return NotFound();
            }

            return View(verifiedPhysician);
        }

        private bool VerifiedPhysicianExists(int id)
        {
            return _context.VerifiedPhysicians.Any(e => e.Id == id);
        }
    }
}
