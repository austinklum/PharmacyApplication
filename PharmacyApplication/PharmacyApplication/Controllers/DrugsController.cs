﻿using System;
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
    public class DrugsController : Controller
    {
        private readonly DrugContext _context;

        public DrugsController(DrugContext context)
        {
            _context = context;
        }

        // GET: Drugs
        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString("DrugCountValidation", "");
            return View(await _context.Drugs.ToListAsync());
        }

        // GET: Drugs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drug = await _context.Drugs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drug == null)
            {
                return NotFound();
            }

            return View(drug);
        }

        public async Task<IActionResult> UpdateStock(string newStock, int drugId)
        {
            int numNewDrugs = 0;
            if (!string.IsNullOrEmpty(newStock))
            {
                try
                {
                    numNewDrugs = int.Parse(newStock);
                }
                catch
                {
                    HttpContext.Session.SetString("DrugCountValidation", "Must be a positive integer");
                    return RedirectToAction("Details", new { id = drugId });
                }
            }
            if (numNewDrugs <= 0)
            {
                HttpContext.Session.SetString("DrugCountValidation", "Must be a positive integer");
                return RedirectToAction("Details", new { id = drugId });
            }
            HttpContext.Session.SetString("DrugCountValidation", "");
            Drug drug = await _context.Drugs.FirstAsync(d => d.Id == drugId);
            drug.Stock += numNewDrugs;

            _context.Drugs.Update(drug);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = drugId });
        }

        private bool DrugExists(int id)
        {
            return _context.Drugs.Any(e => e.Id == id);
        }
    }
}
