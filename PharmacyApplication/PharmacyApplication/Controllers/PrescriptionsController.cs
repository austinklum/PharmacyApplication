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
    public class PrescriptionsController : Controller
    {
        private readonly PrescriptionContext _prescriptionContext;
        private readonly DrugContext _drugContext;

        public PrescriptionsController(PrescriptionContext prescriptionContext, DrugContext drugContext)
        {
            _prescriptionContext = prescriptionContext;
            _drugContext = drugContext;
        }

        // GET: Prescriptions
        public async Task<IActionResult> Index()
        {
            return View(await _prescriptionContext.Prescriptions.ToListAsync());
        }

        // GET: Prescriptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescription = await _prescriptionContext.Prescriptions.FirstAsync(m => m.Id == id);
            if (prescription == null)
            {
                return NotFound();
            }

            List<PrescribedDrug> prescribedDrugs = _prescriptionContext.PrescribedDrugs.Where(p => p.PrescriptionId == id).ToList();

            foreach(PrescribedDrug pd in prescribedDrugs)
            {
                pd.DrugName = _drugContext.Drugs.First(d => d.Id == pd.DrugId).MedicalName;
            }

            PrescriptionDetailsViewModel vm = new PrescriptionDetailsViewModel { CurrentPrescription = prescription, PrescribedDrugs = prescribedDrugs };

            return View(vm);
        }

        private bool PrescriptionExists(int id)
        {
            return _prescriptionContext.Prescriptions.Any(e => e.Id == id);
        }
    }
}
