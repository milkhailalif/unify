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
    public class BrandProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BrandProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BrandProducts
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = HttpContext.User.Identity.Name;

            var product = await _context.Products
                .Where(m => m.UserId == user).ToListAsync();

            if(product == null)
            {
                product = new List<Product> { };
            }

            return View(product);
        }

        // GET: BrandProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: BrandProducts/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: BrandProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,UserId,Brand,Name,Description,Price,Quantity,Image,AltImage1,AltImage2,AltImage3,Feature1,Feature2,Feature3,Feature4,Feature5,Feature6,Service,Disabled")] Product product)
        {
            var user = HttpContext.User.Identity.Name;
            var brand = await _context.Brand.FirstOrDefaultAsync(m => m.UserId == user);

            product.UserId = user;
            product.Brand = brand.DisplayName;


            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: BrandProducts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);

            var user = HttpContext.User.Identity.Name;

            if (product == null || product.UserId != user)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: BrandProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Brand,Name,Description,Price,Quantity,Image,AltImage1,AltImage2,AltImage3,Feature1,Feature2,Feature3,Feature4,Feature5,Feature6,Service,Disabled")] Product product)
        {
            var user = HttpContext.User.Identity.Name;

            if (id != product.Id || product.UserId != user)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: BrandProducts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            var user = HttpContext.User.Identity.Name;

            if (product == null || product.UserId != user)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: BrandProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);

            var user = HttpContext.User.Identity.Name;
            if (product.UserId != user)
            {
                return NotFound();
            }

            if (product != null)
            {
                product.Disabled = true;
                //_context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
