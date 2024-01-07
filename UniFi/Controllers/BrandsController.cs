using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniFi.Data;
using UniFi.Models;

namespace UniFi.Controllers
{
    public class BrandsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BrandsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Brands
        public async Task<IActionResult> Index()
        {
            return _context.Brand != null ?
                        View(await _context.Brand.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Brand'  is null.");
        }

        public async Task<IActionResult> Home(string id, string? code, string? source)
        {
            //Home?id=Pixel%20Lordz&&code=test123&&source=Facebook

            if (id == null || _context.Brand == null)
            {
                return NotFound();
            }

            var brand = await _context.Brand
                .FirstOrDefaultAsync(m => m.DisplayName == id);

            if (brand == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles
                .FirstOrDefaultAsync(m => m.UserId == brand.UserId);

            Color color = ColorTranslator.FromHtml(brand.PrimaryColor);
            int r = Convert.ToInt16(color.R);
            int g = Convert.ToInt16(color.G);
            int b = Convert.ToInt16(color.B);

            string rgb = string.Format("rgba({0}, {1}, {2}, 0.10)", r, g, b);

            Color color1 = ColorTranslator.FromHtml(brand.AccentColor);
            int r1 = Convert.ToInt16(color1.R);
            int g1 = Convert.ToInt16(color1.G);
            int b1 = Convert.ToInt16(color1.B);

            string rgb1 = string.Format("rgba({0}, {1}, {2}, 0.10)", r1, g1, b1);


            ViewBag.BrandName = brand.DisplayName;
            ViewBag.Primary = string.Format("linear-gradient(180deg, {0} 0%, rgba(255, 255, 255, 0.00) 100%)", rgb);
            ViewBag.Accent = string.Format("linear-gradient(180deg, {0} 0%, rgba(255, 255, 255, 0.00) 100%)", rgb1);
            ViewBag.NavCol = brand.PrimaryColor;
            ViewBag.DisText = brand.DisplayText;
            ViewBag.TextColor = brand.TextColor;
            ViewBag.CardColor = brand.BackgroundColor;

            ViewBag.IMG = brand.BackgroundImage;
            ViewBag.IMG1 = brand.BackgroundImage1;
            ViewBag.Vid = brand.BackgroundVideo;

            if(brand.BackgroundVideo != null)
            {
                int lastSlashIndex = brand.BackgroundVideo.LastIndexOf('/');
                string vidId = brand.BackgroundVideo.Substring(lastSlashIndex + 1);

                ViewBag.Vid = brand.BackgroundVideo + "?autoplay=1&mute=1&playlist=" + vidId + "&loop=1";
            }

            ViewBag.Web = profile.Website;
            ViewBag.Twit = profile.Twitter;
            ViewBag.Disc = profile.DiscordInvite;
            ViewBag.Logo = profile.Logo;


            var affiliateClick = new AffiliateBrandLink();

            var affilID = HttpContext.User.Identity.Name;

            if (affilID == null)
            {
                affiliateClick.Visitor = "Anonymous";
            }
            else
            {
                affiliateClick.Visitor = affilID;
            }

            if (code == null)
            {
                affiliateClick.AffiliateCode = "Organic";
                affiliateClick.Source = "Organic";
                affiliateClick.AffiliateUser = "none";
            }
            else
            {
                affiliateClick.AffiliateCode = code;

                if(source == null)
                {
                    affiliateClick.Source = "None";
                }
                else
                {
                    affiliateClick.Source = source;
                }
                

                var affilUser = await _context.Affiliates
                .FirstOrDefaultAsync(m => m.AffiliateCode == code);

                if(affilUser == null)
                {
                    affiliateClick.AffiliateUser = "none";
                }
                else
                {
                    affiliateClick.AffiliateUser = affilUser.UserId;
                }
            }

            affiliateClick.BrandPage = brand.DisplayName;
            affiliateClick.ProductId = 0;
            affiliateClick.InsertDate = DateTime.Now;

            _context.Add(affiliateClick);
            await _context.SaveChangesAsync();

            var user = HttpContext.User.Identity.Name;

            if (user != null)
            {
                var affilUser = await _context.Affiliates
                .FirstOrDefaultAsync(m => m.UserId == user);

                if (affilUser == null)
                {
                    return RedirectToAction("ConnectWallet", "Profiles");
                }
            }

            var product = await _context.Products
                .Where(m => m.Brand == brand.DisplayName && m.Disabled == false).ToListAsync();

            return View(product);
        }

        [Authorize]
        public async Task<IActionResult> AdminHome()
        {
            //Home?id=Pixel%20Lordz&&code=test123&&source=Facebook

            var user = HttpContext.User.Identity.Name;
            var brand = await _context.Brand
                .FirstOrDefaultAsync(m => m.UserId == user);


            if (brand == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles
                .FirstOrDefaultAsync(m => m.UserId == brand.UserId);

            Color color = ColorTranslator.FromHtml(brand.PrimaryColor);
            int r = Convert.ToInt16(color.R);
            int g = Convert.ToInt16(color.G);
            int b = Convert.ToInt16(color.B);

            string rgb = string.Format("rgba({0}, {1}, {2}, 0.10)", r, g, b);

            Color color1 = ColorTranslator.FromHtml(brand.AccentColor);
            int r1 = Convert.ToInt16(color1.R);
            int g1 = Convert.ToInt16(color1.G);
            int b1 = Convert.ToInt16(color1.B);

            string rgb1 = string.Format("rgba({0}, {1}, {2}, 0.10)", r1, g1, b1);


            ViewBag.BrandName = brand.DisplayName;
            ViewBag.Primary = string.Format("linear-gradient(180deg, {0} 0%, rgba(255, 255, 255, 0.00) 100%)", rgb);
            ViewBag.Accent = string.Format("linear-gradient(180deg, {0} 0%, rgba(255, 255, 255, 0.00) 100%)", rgb1);
            ViewBag.NavCol = brand.PrimaryColor;
            ViewBag.DisText = brand.DisplayText;
            ViewBag.TextColor = brand.TextColor;
            ViewBag.CardColor = brand.BackgroundColor;

            ViewBag.IMG = brand.BackgroundImage;
            ViewBag.IMG1 = brand.BackgroundImage1;
            ViewBag.Vid = brand.BackgroundVideo;

            if (brand.BackgroundVideo != null)
            {
                int lastSlashIndex = brand.BackgroundVideo.LastIndexOf('/');
                string vidId = brand.BackgroundVideo.Substring(lastSlashIndex + 1);

                ViewBag.Vid = brand.BackgroundVideo + "?autoplay=1&mute=1&playlist=" + vidId + "&loop=1";
            }

            ViewBag.Web = profile.Website;
            ViewBag.Twit = profile.Twitter;
            ViewBag.Disc = profile.DiscordInvite;
            ViewBag.Logo = profile.Logo;

            var product = await _context.Products
                .Where(m => m.Brand == brand.DisplayName && m.Disabled == false).ToListAsync();

            return View(product);
        }

        public async Task<IActionResult> ProductDetails(int id, string? code, string? source)
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

            var affiliateClick = new AffiliateBrandLink();

            var affilID = HttpContext.User.Identity.Name;

            if (affilID == null)
            {
                affiliateClick.Visitor = "Anonymous";
            }
            else
            {
                affiliateClick.Visitor = affilID;
            }

            if (code == null)
            {
                affiliateClick.AffiliateCode = "Organic";
                affiliateClick.Source = "Organic";
                affiliateClick.AffiliateUser = "none";
            }
            else
            {
                affiliateClick.AffiliateCode = code;

                if (source == null)
                {
                    affiliateClick.Source = "None";
                }
                else
                {
                    affiliateClick.Source = source;
                }


                var affilUser = await _context.Affiliates
                .FirstOrDefaultAsync(m => m.AffiliateCode == code);

                if (affilUser == null)
                {
                    affiliateClick.AffiliateUser = "none";
                }
                else
                {
                    affiliateClick.AffiliateUser = affilUser.UserId;
                }
            }

            affiliateClick.BrandPage = product.Brand;
            affiliateClick.ProductId = product.Id;
            affiliateClick.InsertDate = DateTime.Now;

            _context.Add(affiliateClick);
            await _context.SaveChangesAsync();

            return View(product);
        }

        [Authorize]
        public async Task<IActionResult> SaveText(string id)
        {
            var user = HttpContext.User.Identity.Name;
            var userBrand = await _context.Brand
                .FirstOrDefaultAsync(m => m.UserId == user);

            if (id != null)
            {
                userBrand.DisplayText = id;

                await _context.SaveChangesAsync();
            }


            return RedirectToAction("AdminHome", "Brands");
        }

        [Authorize]
        public async Task<IActionResult> SaveNav(string id)
        {
            var user = HttpContext.User.Identity.Name;
            var userBrand = await _context.Brand
                .FirstOrDefaultAsync(m => m.UserId == user);

            if (id != null)
            {
                userBrand.PrimaryColor = id;

                await _context.SaveChangesAsync();
            }


            return RedirectToAction("AdminHome", "Brands");
        }

        [Authorize]
        public async Task<IActionResult> SaveVid(string id)
        {
            var user = HttpContext.User.Identity.Name;
            var userBrand = await _context.Brand
                .FirstOrDefaultAsync(m => m.UserId == user);

            if (id != null)
            {
                string decodedUrl = HttpUtility.UrlDecode(id);
                userBrand.BackgroundVideo = decodedUrl;

                await _context.SaveChangesAsync();
            }


            return RedirectToAction("AdminHome", "Brands");
        }

        [Authorize]
        public async Task<IActionResult> SaveCard(string cColor, string aColor, string tColor)
        {
            var user = HttpContext.User.Identity.Name;
            var userBrand = await _context.Brand
                .FirstOrDefaultAsync(m => m.UserId == user);

            if (cColor != null && aColor != null && tColor != null)
            {
                userBrand.BackgroundColor = cColor;
                userBrand.AccentColor = aColor;
                userBrand.TextColor = tColor;

                await _context.SaveChangesAsync();
            }


            return RedirectToAction("AdminHome", "Brands");
        }

        [Authorize]
        public async Task<IActionResult> SaveImages(string img1, string img2, string img3, string img4)
        {
            var user = HttpContext.User.Identity.Name;
            var userBrand = await _context.Brand
                .FirstOrDefaultAsync(m => m.UserId == user);

            if (img1 != "NA")
            {
                userBrand.BackgroundImage = img1;                
            }
            if (img2 != "NA")
            {
                userBrand.BackgroundImage1 = img2;
            }
            if (img3 != "NA")
            {
                userBrand.BrandImage = img3;
            }
            if (img4 != "NA")
            {
                var userProfile = await _context.Profiles
                .FirstOrDefaultAsync(m => m.UserId == user);
                userProfile.Logo = img4;
            }


            await _context.SaveChangesAsync();

            return RedirectToAction("AdminHome", "Brands");
        }

        [Authorize]
        public IActionResult UploadImages()
        {
            return View();
        }


        // GET: Brands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Brand == null)
            {
                return NotFound();
            }

            var brand = await _context.Brand
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        [Authorize]
        public async Task<Photos> UserPhotos()
        {
            var user = User.FindFirstValue(ClaimTypes.Name);

            var userBrand = await _context.Brand
                .FirstOrDefaultAsync(m => m.UserId == user);

            var userProfile = await _context.Profiles
                .FirstOrDefaultAsync(m => m.UserId == user);

            Photos userPhotos = new Photos();

            if(userBrand.BackgroundImage == "~/images/blank.png")
            {
                userPhotos.image1 = "";
            }
            else
            {
                userPhotos.image1 = userBrand.BackgroundImage;
            }

            if (userBrand.BackgroundImage1 == "~/images/blank.png")
            {
                userPhotos.image2 = "";
            }
            else
            {
                userPhotos.image2 = userBrand.BackgroundImage1;
            }

            if (userBrand.BrandImage == "~/images/blank.png")
            {
                userPhotos.image3 = "";
            }
            else
            {
                userPhotos.image3 = userBrand.BrandImage;
            }

            if (userProfile.Logo == "~/images/logoplc.png")
            {
                userPhotos.logo = "";
            }
            else
            {
                userPhotos.logo = userProfile.Logo;
            }

            return userPhotos;
        }

        // GET: Brands/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,UserId,DisplayText,PrimaryColor,AccentColor,TextColor,BackgroundColor,BackgroundImage,DisplayName,BackgroundImage1,BackgroundVideo")] Brand brand)
        {
            var user = HttpContext.User.Identity.Name;

            brand.UserId = user;

			var brandName = await _context.Brand
				.FirstOrDefaultAsync(m => m.DisplayName == brand.DisplayName);

			var brandExists = await _context.Brand
				.FirstOrDefaultAsync(m => m.UserId == user);

			if (brandExists != null)
			{
				ModelState.AddModelError("DisplayName", "You can only create one Brand Page.");
				return View(brand);
			}

			if (brandName != null)
            {
                ModelState.AddModelError("DisplayName", "Brand Name Already taken please choose an altenative name.");
				return View(brand);
			}

			if (ModelState.IsValid)
            {
                _context.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction("BrandDashboard", "Profiles");
            }
            return View(brand);
        }

        // GET: Brands/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (_context.Brand == null)
            {
                return NotFound();
            }

            var user = HttpContext.User.Identity.Name;
            var brand = await _context.Brand.FirstOrDefaultAsync(m => m.UserId == user);
            

            if (brand == null || brand.UserId != user)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,DisplayText,PrimaryColor,AccentColor,TextColor,BackgroundColor,BackgroundImage,DisplayName,BackgroundImage1,BackgroundVideo")] Brand brand)
        {
            var user = HttpContext.User.Identity.Name;
            var userBrand = await _context.Brand
                .FirstOrDefaultAsync(m => m.Id == id);

            if (id != brand.Id || userBrand.UserId != user)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    userBrand.DisplayText = brand.DisplayText;
                    userBrand.PrimaryColor = brand.PrimaryColor;
                    userBrand.AccentColor = brand.AccentColor;
                    userBrand.TextColor = brand.TextColor;
                    userBrand.BackgroundColor = brand.BackgroundColor;
                    userBrand.BackgroundImage = brand.BackgroundImage;
                    userBrand.BackgroundImage1 = brand.BackgroundImage1;
                    userBrand.BackgroundVideo = brand.BackgroundVideo;

                    _context.Update(userBrand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("BrandDashboard", "Profiles");
            }
            return View(brand);
        }

        // GET: Brands/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Brand == null)
            {
                return NotFound();
            }

            var brand = await _context.Brand
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: Brands/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Brand == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Brand'  is null.");
            }
            var brand = await _context.Brand.FindAsync(id);
            if (brand != null)
            {
                _context.Brand.Remove(brand);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrandExists(int id)
        {
            return (_context.Brand?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public class Photos
        {
            public string? image1 { get; set; }
            public string? image2 { get; set; }
            public string? image3 { get; set; }
            public string? logo { get; set; }
        }
    }
}
