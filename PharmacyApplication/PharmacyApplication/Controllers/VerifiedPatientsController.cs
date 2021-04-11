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
        public async Task<IActionResult> Index()
        {
            return View(await _verificationContext.VerifiedPatients.ToListAsync());
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

            List<PrescribedDrug> prescribedDrugs = _prescriptionContext.PrescribedDrugs.Where(p => p.PrescriptionId == prescription.Id).ToList();

            foreach (PrescribedDrug pd in prescribedDrugs)
            {
                pd.DrugName = _drugContext.Drugs.First(d => d.Id == pd.DrugId).MedicalName;
            }

            VerifiedPatient patient = _verificationContext.VerifiedPatients.FirstOrDefault(p => p.Name == prescription.PatientName && p.Address == p.Address && p.DateOfBirth == prescription.PatientDOB);

            prescription.PatientVerified = patient != null;
            _prescriptionContext.Prescriptions.Update(prescription);
            _prescriptionContext.SaveChanges();

            PrescriptionDetailsViewModel vm = new PrescriptionDetailsViewModel { CurrentPrescription = prescription, PrescribedDrugs = prescribedDrugs };
            return RedirectToAction("Details", "Prescriptions", new {id = prescription.Id});
        }

        private bool VerifiedPatientExists(int id)
        {
            return _verificationContext.VerifiedPatients.Any(e => e.Id == id);
        }
    }
}
