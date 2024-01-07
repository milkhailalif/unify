using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniFi.Data;
using UniFi.Models;

namespace UniFi.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AffiliatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AffiliatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Affiliates
        public async Task<IActionResult> Index()
        {
              return _context.Affiliates != null ? 
                          View(await _context.Affiliates.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Affiliates'  is null.");
        }

        // GET: Affiliates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Affiliates == null)
            {
                return NotFound();
            }

            var affiliate = await _context.Affiliates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (affiliate == null)
            {
                return NotFound();
            }

            return View(affiliate);
        }

        // GET: Affiliates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Affiliates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Wallet,AffiliateCode,Approved")] Affiliate affiliate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(affiliate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(affiliate);
        }

        // GET: Affiliates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Affiliates == null)
            {
                return NotFound();
            }

            var affiliate = await _context.Affiliates.FindAsync(id);
            if (affiliate == null)
            {
                return NotFound();
            }
            return View(affiliate);
        }

        // POST: Affiliates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Wallet,AffiliateCode,Approved")] Affiliate affiliate)
        {
            if (id != affiliate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(affiliate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AffiliateExists(affiliate.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(affiliate);
        }

        // GET: Affiliates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Affiliates == null)
            {
                return NotFound();
            }

            var affiliate = await _context.Affiliates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (affiliate == null)
            {
                return NotFound();
            }

            return View(affiliate);
        }

        // POST: Affiliates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Affiliates == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Affiliates'  is null.");
            }
            var affiliate = await _context.Affiliates.FindAsync(id);
            if (affiliate != null)
            {
                _context.Affiliates.Remove(affiliate);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AffiliateExists(int id)
        {
          return (_context.Affiliates?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
