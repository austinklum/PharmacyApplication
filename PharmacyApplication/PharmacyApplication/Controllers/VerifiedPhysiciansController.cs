using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> Index(string searchString)
        {
            var physicians = from p in _verificationContext.VerifiedPhysicians select p;
            if(!string.IsNullOrEmpty(searchString))
            {
                physicians = physicians.Where(p => p.Name.Contains(searchString));
            }
            HttpContext.Session.SetString(HomeController.CreatePatientNameValidation, "");
            HttpContext.Session.SetString(HomeController.CreatePatientAddressValidation, "");
            HttpContext.Session.SetString(HomeController.CreatePatientPronounsValidation, "");
            HttpContext.Session.SetString(HomeController.CreatePatientDOBValidation, "");
            return View(await physicians.ToListAsync());
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

        public IActionResult Create()
        {
            VerifiedPhysician vp = new VerifiedPhysician();
            return View(vp);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VerifiedPhysician vp)
        {
            HttpContext.Session.SetString(HomeController.CreatePatientNameValidation, "");
            HttpContext.Session.SetString(HomeController.CreatePatientAddressValidation, "");
            HttpContext.Session.SetString(HomeController.CreatePatientPronounsValidation, "");
            HttpContext.Session.SetString(HomeController.CreatePatientDOBValidation, "");

            if (string.IsNullOrEmpty(vp.Name))
            {
                HttpContext.Session.SetString(HomeController.CreatePatientNameValidation, "Required Field");
                return View(vp);
            }
            if (string.IsNullOrEmpty(vp.LicenseNumber))
            {
                HttpContext.Session.SetString(HomeController.CreatePatientDOBValidation, "Required Field");
                return View(vp);
            }
            if (string.IsNullOrEmpty(vp.Pronouns))
            {
                HttpContext.Session.SetString(HomeController.CreatePatientPronounsValidation, "Required Field");
                return View(vp);
            }

            if(!ModelState.IsValid)
            {
                return View(vp);
            }
            _verificationContext.Add(vp);
            await _verificationContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Delete(int id)
        {
            VerifiedPhysician vp = _verificationContext.VerifiedPhysicians.FirstOrDefault(p => p.Id == id);
            _verificationContext.Remove(vp);
            _verificationContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
