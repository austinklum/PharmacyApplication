using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyApplication.Data;
using PharmacyApplication.Models;

namespace PharmacyApplication.Controllers
{
    [Route("api/PrescribedDrugsAPI")]
    [ApiController]
    public class PrescribedDrugsAPIController : ControllerBase
    {
        private readonly PrescriptionContext _context;

        public PrescribedDrugsAPIController(PrescriptionContext context)
        {
            _context = context;
        }

        // GET: api/PrescribedDrugsAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescribedDrug>>> GetPrescribedDrugs()
        {
            return await _context.PrescribedDrugs.ToListAsync();
        }

        // GET: api/PrescribedDrugsAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PrescribedDrug>> GetPrescribedDrug(int id)
        {
            var prescribedDrug = await _context.PrescribedDrugs.FindAsync(id);

            if (prescribedDrug == null)
            {
                return NotFound();
            }

            return prescribedDrug;
        }

        // PUT: api/PrescribedDrugsAPI/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrescribedDrug(int id, PrescribedDrug prescribedDrug)
        {
            if (id != prescribedDrug.Id)
            {
                return BadRequest();
            }

            _context.Entry(prescribedDrug).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrescribedDrugExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PrescribedDrugsAPI
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PrescribedDrug>> PostPrescribedDrug(PrescribedDrug prescribedDrug)
        {
            PrescribedDrug found = _context.PrescribedDrugs.First(p => p.Id == prescribedDrug.Id);
            found.CoveredAmount = prescribedDrug.CoveredAmount;
            _context.PrescribedDrugs.Update(found);

            Prescription prescription = _context.Prescriptions.First(p => p.Id == found.PrescriptionId);
            prescription.SentToInsurance = true;
            _context.Prescriptions.Update(prescription);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPrescribedDrug), new { id = prescribedDrug.Id }, prescribedDrug);
        }

        // DELETE: api/PrescribedDrugsAPI/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PrescribedDrug>> DeletePrescribedDrug(int id)
        {
            var prescribedDrug = await _context.PrescribedDrugs.FindAsync(id);
            if (prescribedDrug == null)
            {
                return NotFound();
            }

            _context.PrescribedDrugs.Remove(prescribedDrug);
            await _context.SaveChangesAsync();

            return prescribedDrug;
        }

        private bool PrescribedDrugExists(int id)
        {
            return _context.PrescribedDrugs.Any(e => e.Id == id);
        }
    }
}
