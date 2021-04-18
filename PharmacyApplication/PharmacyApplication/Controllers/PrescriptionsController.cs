using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
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
        private readonly VerificationContext _verificationContext;
        private readonly DrugContext _drugContext;

        public PrescriptionsController(PrescriptionContext prescriptionContext, DrugContext drugContext, VerificationContext verificationContext)
        {
            _prescriptionContext = prescriptionContext;
            _drugContext = drugContext;
            _verificationContext = verificationContext;
        }

        // GET: Prescriptions
        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString("PrescriptionFillValidation", "");
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

        public async Task<IActionResult> CreateBill(int id)
        {

            var prescription = await _prescriptionContext.Prescriptions.FirstAsync(m => m.Id == id);
            if (prescription == null)
            {
                return NotFound();
            }

            List<PrescribedDrug> prescribedDrugs = _prescriptionContext.PrescribedDrugs.Where(p => p.PrescriptionId == id).ToList();

            foreach (PrescribedDrug pd in prescribedDrugs)
            {
                Drug drug = _drugContext.Drugs.First(d => d.Id == pd.DrugId);
                if(pd.Count > drug.Stock)
                {
                    HttpContext.Session.SetString("PrescriptionFillValidation", "Not enough " + drug.MedicalName);
                    return RedirectToAction("Details", new { id = id });
                }
            }

            // send messages to insurance to get values back
            SendPrescription(prescription, prescribedDrugs);

            prescription.BillCreated = null;
            _prescriptionContext.Prescriptions.Update(prescription);
            _prescriptionContext.SaveChanges();

            return RedirectToAction("Details", new { id = id });
        }

        private void SendPrescription(Prescription prescription, List<PrescribedDrug> prescribedDrugs)
        {
            int holderId = _verificationContext.VerifiedPatients.First(p => p.Name == prescription.PatientName).Id;
            PTransaction transaction = new PTransaction
            {
                Id = prescription.Id,
                Date = prescription.IssuedDate,
                HolderId = holderId
            };

            string json = JsonSerializer.Serialize(transaction);

            var bytes = Encoding.UTF8.GetBytes(json);
#if DEBUG
            var httpRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44306/api/PTransactionsAPI");
#else
            var httpRequest = (HttpWebRequest)WebRequest.Create("http://wngcsp86.intra.uwlax.edu:81/api/PTransactionsAPI");
#endif

            httpRequest.Method = "POST";
            httpRequest.ContentLength = bytes.Length;
            httpRequest.ContentType = "application/json";
            Stream dataStream = httpRequest.GetRequestStream();
            dataStream.Write(bytes, 0, bytes.Length);
            dataStream.Close();

            WebResponse response = httpRequest.GetResponse();

            foreach (PrescribedDrug pd in prescribedDrugs)
            {
                Subtransaction subtransaction = new Subtransaction
                {
                    Id = pd.Id,
                    PTransactionId = prescription.Id,
                    DrugId = pd.DrugId,
                    Count = pd.Count,
                    AmountPaid = 0,
                    Accepted = 0,
                };

                string subtransactionJson = JsonSerializer.Serialize(subtransaction);

                var subtransactionBytes = Encoding.UTF8.GetBytes(subtransactionJson);
#if DEBUG
                var request = (HttpWebRequest)WebRequest.Create("https://localhost:44306/api/SubtransactionsAPI");
#else
                var request = (HttpWebRequest)WebRequest.Create("http://wngcsp86.intra.uwlax.edu:81/api/SubtransactionsAPI");
#endif

                request.Method = "POST";
                request.ContentLength = subtransactionBytes.Length;
                request.ContentType = "application/json";
                Stream stream = request.GetRequestStream();
                stream.Write(subtransactionBytes, 0, subtransactionBytes.Length);
                stream.Close();

                WebResponse subtransactionResponse = request.GetResponse();
            }
        }

        private bool PrescriptionExists(int id)
        {
            return _prescriptionContext.Prescriptions.Any(e => e.Id == id);
        }
    }
}
