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
    public class VerifiedPatientsController : Controller
    {
        private readonly VerificationContext _verificationContext;
        private readonly PrescriptionContext _prescriptionContext;
        private readonly DrugContext _drugContext;

        public VerifiedPatientsController(VerificationContext verificationContext, PrescriptionContext prescriptionContext, DrugContext drugContext)
        {
            _verificationContext = verificationContext;
            _prescriptionContext = prescriptionContext;
            _drugContext = drugContext;
        }

        // GET: VerifiedPatients
        public async Task<IActionResult> Index(string searchString)
        {
            HttpContext.Session.SetString(HomeController.CreatePatientNameValidation, "");
            HttpContext.Session.SetString(HomeController.CreatePatientAddressValidation, "");
            HttpContext.Session.SetString(HomeController.CreatePatientPronounsValidation, "");
            HttpContext.Session.SetString(HomeController.CreatePatientDOBValidation, "");
            var patients = from p in _verificationContext.VerifiedPatients select p;
            if(!string.IsNullOrEmpty(searchString))
            {
                patients = patients.Where(p => p.Name.Contains(searchString) || p.Address.Contains(searchString));
            }
            return View(await patients.ToListAsync());
        }

        // GET: VerifiedPatients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var verifiedPatient = await _verificationContext.VerifiedPatients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (verifiedPatient == null)
            {
                return NotFound();
            }

            return View(verifiedPatient);
        }

        public IActionResult Verify(int id)
        {
            Prescription prescription = _prescriptionContext.Prescriptions.First(p => p.Id == id);

            VerifiedPatient patient = _verificationContext.VerifiedPatients.FirstOrDefault(p => p.Name == prescription.PatientName && p.Address == prescription.PatientAddress && p.DateOfBirth == prescription.PatientDOB);

            prescription.PatientVerified = patient != null;
            _prescriptionContext.Prescriptions.Update(prescription);
            _prescriptionContext.SaveChanges();

            return RedirectToAction("Details", "Prescriptions", new {id = prescription.Id});
        }

        private bool VerifiedPatientExists(int id)
        {
            return _verificationContext.VerifiedPatients.Any(e => e.Id == id);
        }


        public IActionResult Create()
        {
            VerifiedPatient vp = new VerifiedPatient();
            return View(vp);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VerifiedPatient vp)
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
            if (vp.DateOfBirth == DateTime.MinValue)
            {
                HttpContext.Session.SetString(HomeController.CreatePatientDOBValidation, "Required Field");
                return View(vp);
            }
            if (string.IsNullOrEmpty(vp.Pronouns))
            {
                HttpContext.Session.SetString(HomeController.CreatePatientPronounsValidation, "Required Field");
                return View(vp);
            }
            if (string.IsNullOrEmpty(vp.Address))
            {
                HttpContext.Session.SetString(HomeController.CreatePatientAddressValidation, "Required Field");
                return View(vp);
            }
            
            DateTime currentYear = new DateTime(DateTime.Now.Year, 1, 1);
            if (vp.DateOfBirth >= currentYear)
            {
                HttpContext.Session.SetString(HomeController.CreatePatientDOBValidation, "Policyholder cannot be born in this year");
                return View(vp);
            }
            
            _verificationContext.Add(vp);
            await _verificationContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Delete(int id)
        {
            VerifiedPatient vp = _verificationContext.VerifiedPatients.FirstOrDefault(p => p.Id == id);
            _verificationContext.Remove(vp);
            _verificationContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
