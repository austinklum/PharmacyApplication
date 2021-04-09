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
    public class VerifiedPatientsController : Controller
    {
        private readonly VerificationContext _context;

        public VerifiedPatientsController(VerificationContext context)
        {
            _context = context;
        }

        // GET: VerifiedPatients
        public async Task<IActionResult> Index()
        {
            return View(await _context.VerifiedPatients.ToListAsync());
        }

        // GET: VerifiedPatients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var verifiedPatient = await _context.VerifiedPatients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (verifiedPatient == null)
            {
                return NotFound();
            }

            return View(verifiedPatient);
        }

        private bool VerifiedPatientExists(int id)
        {
            return _context.VerifiedPatients.Any(e => e.Id == id);
        }
    }
}
