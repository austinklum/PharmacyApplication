using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyApplication.Data;
using PharmacyApplication.Models;
using PharmacyApplication.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApplication.Components
{
    public class PrescriptionListViewComponent : ViewComponent
    {
        private readonly PrescriptionContext _prescriptionContext;
        private readonly VerificationContext _verificationContext;
        private readonly DrugContext _drugContext;

        public PrescriptionListViewComponent(PrescriptionContext prescriptionContext, DrugContext drugContext, VerificationContext verificationContext)
        {
            _prescriptionContext = prescriptionContext;
            _drugContext = drugContext;
            _verificationContext = verificationContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Prescription> prescriptions = (from p in _prescriptionContext.Prescriptions select p).ToList();

            prescriptions = prescriptions.Where(p => p.BillCreated == false).ToList();

            PrescriptionsViewModel vm = new PrescriptionsViewModel
            {
                Prescriptions = prescriptions,
                IncludeProcessed = false,
            };
            HttpContext.Session.SetString(HomeController.PrescriptionFillValidation, "");
            return View(vm);
        }
    }
}
