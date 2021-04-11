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
        private readonly VerificationContext _verificationContext;
        private readonly PrescriptionContext _prescriptionContext;
        private readonly DrugContext _drugContext;

        public VerifiedPhysiciansController(VerificationContext verificationContext, PrescriptionContext prescriptionContext, DrugContext drugContext)
        {
            _verificationContext = verificationContext;
            _prescriptionContext = prescriptionContext;
            _drugContext = drugContext;
        }

        // GET: VerifiedPhysicians
        public async Task<IActionResult> Index()
        {
            return View(await _verificationContext.VerifiedPhysicians.ToListAsync());
        }

        // GET: VerifiedPhysicians/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var verifiedPhysician = await _verificationContext.VerifiedPhysicians
                .FirstOrDefaultAsync(m => m.Id == id);
            if (verifiedPhysician == null)
            {
                return NotFound();
            }

            return View(verifiedPhysician);
        }

        public IActionResult Verify(int id)
        {
            Prescription prescription = _prescriptionContext.Prescriptions.First(p => p.Id == id);

            VerifiedPhysician physician = _verificationContext.VerifiedPhysicians.FirstOrDefault(p => p.Name == prescription.PhysicianName && p.LicenseNumber == prescription.PhysicianLicenseNumber);

            prescription.PhysicianVerified = physician != null;
            _prescriptionContext.Prescriptions.Update(prescription);
            _prescriptionContext.SaveChanges();

            return RedirectToAction("Details", "Prescriptions", new { id = prescription.Id });
        }

        private bool VerifiedPhysicianExists(int id)
        {
            return _verificationContext.VerifiedPhysicians.Any(e => e.Id == id);
        }
    }
}
