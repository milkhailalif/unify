using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class EmailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Emails

        public async Task<IActionResult> Index()
        {
            return _context.Emails != null ?
                        View(await _context.Emails.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Emails'  is null.");
        }

        // GET: Emails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Emails == null)
            {
                return NotFound();
            }

            var email = await _context.Emails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (email == null)
            {
                return NotFound();
            }

            return View(email);
        }

        public IActionResult Success()
        {
            return View();
        }

        // GET: Emails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Emails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmailAdd")] Email email)
        {
            if (ModelState.IsValid)
            {
                var mail = await _context.Emails
                .FirstOrDefaultAsync(m => m.EmailAdd == email.EmailAdd);

                if(mail != null)
                {
                    return RedirectToAction("Exists");
                }

                _context.Add(email);
                await _context.SaveChangesAsync();
                return RedirectToAction("Success");
            }
            return RedirectToAction("Error");
        }

        public IActionResult Exists()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        //GET: Emails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Emails == null)
            {
                return NotFound();
            }

            var email = await _context.Emails.FindAsync(id);
            if (email == null)
            {
                return NotFound();
            }
            return View(email);
        }

        // POST: Emails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmailAdd")] Email email)
        {
            if (id != email.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(email);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmailExists(email.Id))
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
            return View(email);
        }

        // GET: Emails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Emails == null)
            {
                return NotFound();
            }

            var email = await _context.Emails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (email == null)
            {
                return NotFound();
            }

            return View(email);
        }

        // POST: Emails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Emails == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Emails'  is null.");
            }
            var email = await _context.Emails.FindAsync(id);
            if (email != null)
            {
                _context.Emails.Remove(email);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public ActionResult ExportData()
        {
            var emails = _context.Emails.Select(u => u.EmailAdd).ToList();

            string mailCsv = string.Join(",", emails);

            return File(Encoding.ASCII.GetBytes(mailCsv), "text/csv", "Subscribers.csv");
        }

        private bool EmailExists(int id)
        {
            return (_context.Emails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
