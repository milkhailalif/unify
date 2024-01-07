using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniFi.Data;
using UniFi.Models;
using TweetNaclSharp;
using System.Globalization;
using System.Drawing.Drawing2D;

namespace UniFi.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Profiles
        public async Task<IActionResult> Index()
        {
            return _context.Profiles != null ?
                        View(await _context.Profiles.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Profiles'  is null.");
        }

        public async Task<IActionResult> ConnectWallet()
        {
            return View();
        }

        public async Task<IActionResult> ConnectBrandWallet()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> AffilliateDashboard()
        {
            var affilID = HttpContext.User.Identity.Name;
            //var affilID = "ysenft2022@gmail.com";

            var afUser = await _context.Affiliates.FirstOrDefaultAsync(m => m.UserId == affilID);

            if (afUser == null)
            {
                return RedirectToAction("ConnectWallet", "Profiles");
            }

            ViewBag.Wallet = afUser.Wallet;

            var affillStats = await _context.AffiliateBrandLinks.Where(m => m.AffiliateUser == affilID).ToListAsync();

            var brandClicks = affillStats.GroupBy(m => m.BrandPage).Select(g => new { name = g.Key, count = g.Count() });
            var prdctClicks = affillStats.GroupBy(m => m.ProductId).Select(g => new { name = g.Key, count = g.Count() });

            ViewBag.TotalClicks = (double)affillStats.Count;

            ViewBag.MainLinkClicks = affillStats.Where(m => m.Source == "None").ToList().Count;
            ViewBag.FBClicks = affillStats.Where(m => m.Source == "Facebook").ToList().Count;
            ViewBag.TwClicks = affillStats.Where(m => m.Source == "Twitter").ToList().Count;
            ViewBag.InstaClicks = affillStats.Where(m => m.Source == "Instagram").ToList().Count;
            ViewBag.YTClicks = affillStats.Where(m => m.Source == "Youtube").ToList().Count;

            int days = 7;

            var dayList = new List<string>();

            dayList.Add(DateTime.Now.Date.ToString("dd/MM/yyyy"));

            for (int i = 1; i < days; i++)
            {
                dayList.Add(DateTime.Now.AddDays(-i).Date.ToString("dd/MM/yyyy"));
            }

            var dataList = new List<int>();
            CultureInfo provider = CultureInfo.InvariantCulture;

            foreach (var day in dayList)
            {
                DateTime cDate = DateTime.ParseExact(day, "dd/MM/yyyy", provider);
                DateTime cDate1 = cDate.AddDays(1);

                var clicksInDay = affillStats.Where(m => m.InsertDate >= cDate && m.InsertDate < cDate1).ToList().Count;
                dataList.Add(clicksInDay);
            }

            ViewBag.Days = dayList;
            ViewBag.DaysData = dataList;

            var Brands = affillStats.GroupBy(m => m.BrandPage).Select(g => new { name = g.Key, count = g.Count() });

            var Products = affillStats.GroupBy(m => m.ProductId).Select(g => new { name = g.Key, count = g.Count() });

            ViewBag.Affiliates = new List<ViewAffilliateClicks>();
            ViewBag.ProductClicks = new List<ViewAffilliateClicks>();

            if (Brands != null)
            {

                var aList = new List<ViewAffilliateClicks>();

                if (Brands.Count() > 0)
                {
                    foreach (var a in Brands)
                    {
                        ViewAffilliateClicks click = new ViewAffilliateClicks();
                        click.name = a.name;
                        click.count = a.count;

                        aList.Add(click);
                    }

                    if (aList.Count() >= 5)
                    {
                        ViewBag.Affiliates = aList.Take(5).ToList();
                    }
                    else
                    {
                        int runNum = 5 - aList.Count();
                        for (int i = 0; i < runNum; i++)
                        {
                            ViewAffilliateClicks click = new ViewAffilliateClicks();
                            click.name = "_";
                            click.count = 0;

                            aList.Add(click);
                        }
                        ViewBag.Affiliates = aList;
                    }
                }

            }

            if (Products != null)
            {
                var pUsers = Products.Where(m => m.name > 0).ToList().OrderByDescending(m => m.count);

                var pList = new List<ViewAffilliateClicks>();

                if (pUsers.Count() > 0)
                {

                    foreach (var a in pUsers)
                    {
                        ViewAffilliateClicks click1 = new ViewAffilliateClicks();
                        var prdct = await _context.Products.FirstOrDefaultAsync(m => m.Id == a.name);

                        if (prdct != null)
                        {
                            click1.name = prdct.Name;
                            click1.count = a.count;

                            pList.Add(click1);
                        }
                    }

                    if (pList.Count() >= 5)
                    {
                        ViewBag.ProductClicks = pList.Take(5).ToList();
                    }
                    else
                    {
                        int runNum = 5 - pList.Count();
                        for (int i = 0; i < runNum; i++)
                        {
                            ViewAffilliateClicks click = new ViewAffilliateClicks();
                            click.name = "_";
                            click.count = 0;

                            pList.Add(click);
                        }
                        ViewBag.ProductClicks = pList;
                    }
                }

            }

            if (affillStats == null)
            {
                affillStats = new List<AffiliateBrandLink>();
            }

            return View(affillStats);
        }

        [Authorize]
        public async Task<IActionResult> BrandDashboard()
        {
            var user = HttpContext.User.Identity.Name;
            //var user = "ysenft2022@gmail.com";

            var profile = await _context.Profiles.FirstOrDefaultAsync(m => m.UserId == user);
            var brand = await _context.Brand.FirstOrDefaultAsync(m => m.UserId == user);

            if (profile == null)
            {
                return RedirectToAction("Create", "Profiles");
            }

            if (brand == null)
            {
                return RedirectToAction("Create", "Brands");
            }

            if (profile.Wallet == null || profile.Wallet == "")
            {
                return RedirectToAction("ConnectBrandWallet", "Profiles");
            }

            var clicks = await _context.AffiliateBrandLinks.Where(m => m.BrandPage == brand.DisplayName).ToListAsync();

            ViewBag.TotalClicks = (double)clicks.Count;
            ViewBag.Organic = clicks.Where(m => m.AffiliateCode == "Organic").ToList().Count;
            ViewBag.AClicks = clicks.Where(m => m.AffiliateCode != "Organic").ToList().Count;
            ViewBag.BrandName = brand.DisplayName;

            int days = 7;

            var dayList = new List<string>();

            dayList.Add(DateTime.Now.Date.ToString("dd/MM/yyyy"));

            for(int i = 1; i < days; i++)
            {
                dayList.Add(DateTime.Now.AddDays(-i).Date.ToString("dd/MM/yyyy"));
            }

            var dataList = new List<int>();
            CultureInfo provider = CultureInfo.InvariantCulture;

            foreach (var day in dayList)
            {
                DateTime cDate = DateTime.ParseExact(day, "dd/MM/yyyy", provider);
                DateTime cDate1 = cDate.AddDays(1);

                var clicksInDay = clicks.Where(m => m.InsertDate >= cDate && m.InsertDate < cDate1).ToList().Count;
                dataList.Add(clicksInDay);
            }

            ViewBag.Days = dayList;
            ViewBag.DaysData = dataList;


            var Affilliates = clicks.GroupBy(m => m.AffiliateUser).Select(g => new { name = g.Key, count = g.Count() });

            var Products = clicks.GroupBy(m => m.ProductId).Select(g => new { name = g.Key, count = g.Count() });

            ViewBag.Affiliates = new List<ViewAffilliateClicks>();
            ViewBag.ProductClicks = new List<ViewAffilliateClicks>();

            if (Affilliates != null)
            {
                var AffilUsers = Affilliates.Where(m => m.name != "none" && m.name != "").ToList().OrderByDescending(m => m.count);

                var aList = new List<ViewAffilliateClicks>();

                if (AffilUsers.Count() > 0)
                {
                    foreach (var a in AffilUsers)
                    {
                        ViewAffilliateClicks click = new ViewAffilliateClicks();
                        click.name = a.name;
                        click.count = a.count;

                        aList.Add(click);
                    }

                    if (aList.Count() >= 5)
                    {
                        ViewBag.Affiliates = aList.Take(5).ToList();
                    }
                    else
                    {
                        int runNum = 5 - aList.Count();
                        for(int i = 0; i < runNum; i++)
                        {
                            ViewAffilliateClicks click = new ViewAffilliateClicks();
                            click.name = "_";
                            click.count = 0;

                            aList.Add(click);
                        }
                        ViewBag.Affiliates = aList;
                    }
                }

            }

            if(Products != null)
            {
                var pUsers = Products.Where(m => m.name > 0).ToList().OrderByDescending(m => m.count);

                var pList = new List<ViewAffilliateClicks>();

                if (pUsers.Count() > 0)
                {
                    var brandProducts = await _context.Products.Where(m => m.UserId == user).ToListAsync();

                    foreach (var a in pUsers)
                    {
                        ViewAffilliateClicks click1 = new ViewAffilliateClicks();
                        var prdct = brandProducts.FirstOrDefault(m => m.Id == a.name);

                        if (prdct != null)
                        {
                            click1.name = prdct.Name;
                            click1.count = a.count;

                            pList.Add(click1);
                        }
                    }

                    if (pList.Count() >= 5)
                    {
                        ViewBag.ProductClicks = pList.Take(5).ToList();
                    }
                    else
                    {
                        int runNum = 5 - pList.Count();
                        for (int i = 0; i < runNum; i++)
                        {
                            ViewAffilliateClicks click = new ViewAffilliateClicks();
                            click.name = "_";
                            click.count = 0;

                            pList.Add(click);
                        }
                        ViewBag.ProductClicks = pList;
                    }
                }

            }


            return View();
        }

        public class ViewAffilliateClicks
        {
            public string name { get; set; }
            public int count { get; set; }
        }

        public class AxiosRequest
        {
            public string? message { get; set; }
            public string? signature { get; set; }
            public string? pub { get; set; }
            public string? ver { get; set; }
        }

        [Authorize]
        [HttpPost]
        public async Task<bool> AddWallet([FromBody] AxiosRequest axiosRequest)
        {

            if (axiosRequest != null)
            {
                byte[] message = Convert.FromBase64String(axiosRequest.message);
                byte[] signature = Convert.FromBase64String(axiosRequest.signature);
                byte[] pub = Convert.FromBase64String(axiosRequest.pub);
                byte[] ver = Convert.FromBase64String(axiosRequest.ver);

                string pk = System.Text.Encoding.Default.GetString(pub);

                bool isVeri = Nacl.SignDetachedVerify(message, signature, ver);

                if (isVeri)
                {
                    string username = User.FindFirstValue(ClaimTypes.Name);
                    var wallet = await _context.Affiliates.FirstOrDefaultAsync(m => m.UserId == username);

                    if (wallet != null)
                    {
                        wallet.Wallet = pk;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        var code = username;

                        var codeExists = await _context.Affiliates.FirstOrDefaultAsync(m => m.AffiliateCode == code);

                        Affiliate wallets = new Affiliate();
                        wallets.Wallet = pk;
                        wallets.UserId = username;
                        wallets.Approved = false;

                        if (codeExists == null)
                        {
                            wallets.AffiliateCode = code;
                        }
                        else
                        {
                            Random random = new Random();
                            int randomNumber = random.Next(0, 1000);

                            wallets.AffiliateCode = code + randomNumber.ToString();
                        }

                        _context.Add(wallets);
                        await _context.SaveChangesAsync();
                    }

                }
                else
                {
                    return false;
                }

            }

            return true;
        }

        [Authorize]
        [HttpPost]
        public async Task<bool> AddBrandWallet([FromBody] AxiosRequest axiosRequest)
        {

            if (axiosRequest != null)
            {
                byte[] message = Convert.FromBase64String(axiosRequest.message);
                byte[] signature = Convert.FromBase64String(axiosRequest.signature);
                byte[] pub = Convert.FromBase64String(axiosRequest.pub);
                byte[] ver = Convert.FromBase64String(axiosRequest.ver);

                string pk = System.Text.Encoding.Default.GetString(pub);

                bool isVeri = Nacl.SignDetachedVerify(message, signature, ver);

                if (isVeri)
                {
                    string username = User.FindFirstValue(ClaimTypes.Name);
                    var wallet = await _context.Profiles.FirstOrDefaultAsync(m => m.UserId == username);

                    if (wallet != null)
                    {
                        wallet.Wallet = pk;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }

            return true;
        }

        // GET: Profiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Profiles == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        // GET: Profiles/Create
        [Authorize]
        public IActionResult Create()
        {
            var affilID = HttpContext.User.Identity.Name;

            var affillProfile = _context.Affiliates.FirstOrDefault(m => m.UserId == affilID);

            if (affillProfile.Approved == false)
            {
                return RedirectToAction("NotApproved", "Profiles");
            }

            return View();
        }

        // POST: Profiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,UserId,EmailAdd,FullName,Wallet,TwitterUsername,DiscordUsername,CompanyName,CompanyAge,CompanyDetails,Website,Twitter,Facebook,Instagram,DiscordInvite,Logo,Country,City")] Profile profile)
        {
            var affilID = HttpContext.User.Identity.Name;

            var affillProfile = await _context.Affiliates.FirstOrDefaultAsync(m => m.UserId == affilID);

            if (affillProfile.Approved == false)
            {
                return RedirectToAction("NotApproved", "Profiles");
            }

            profile.Wallet = "";
            profile.UserId = affilID;
            profile.EmailAdd = affilID;

            ModelState.Remove("Id");

            //if(profile.UserName != null)
            //{
            //    var usernameExists = await _context.Profiles.FirstOrDefaultAsync(m => m.UserName.ToLower() == profile.UserName.ToLower());

            //    if (usernameExists != null)
            //    {
            //        ModelState.AddModelError("UserName", "Username already Taken try another.");
            //        return View(profile);
            //    }
            //}
            
            if (ModelState.IsValid)
            {
                Brand brand = new Brand();

                // Assign values to its properties
                brand.UserId = affilID;
                brand.DisplayName = profile.CompanyName;
                brand.DisplayText = "Welcome to My Brand";
                brand.PrimaryColor = "#014594";
                brand.AccentColor = "#FF840E";
                brand.TextColor = "#fafafa";
                brand.BackgroundColor = "#0C072A";
                brand.BackgroundImage = "~/images/blank.png";
                brand.BackgroundImage1 = "~/images/blank.png";
                brand.BackgroundVideo = "~/images/blank.png";
                brand.BrandImage = "~/images/blank.png";

                profile.Logo = "~/images/logoplc.png";

                _context.Add(brand);
                _context.Add(profile);
                await _context.SaveChangesAsync();
                return RedirectToAction("AdminHome", "Brands");
            }
            return View(profile);
        }

        public IActionResult NotApproved()
        {
            return View();
        }

        // GET: Profiles/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (_context.Profiles == null)
            {
                return NotFound();
            }

            var affilID = HttpContext.User.Identity.Name;

            var profile = await _context.Profiles.FirstOrDefaultAsync(m => m.UserId == affilID);


            if (profile == null)
            {
                return NotFound();
            }

            if (affilID != profile.UserId)
            {
                return NotFound();
            }

            return View(profile);
        }

        // POST: Profiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,UserName,EmailAdd,FullName,Wallet,TwitterUsername,DiscordUsername,CompanyName,CompanyAge,CompanyDetails,Website,Twitter,Facebook,Instagram,DiscordInvite,Logo,Country,City")] Profile profile)
        {
            if (id != profile.Id)
            {
                return NotFound();
            }

            var affilID = HttpContext.User.Identity.Name;

            var userProfile = await _context.Profiles
                .FirstOrDefaultAsync(m => m.Id == id);

            if (affilID != userProfile.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    userProfile.FullName = profile.FullName;
                    userProfile.TwitterUsername = profile.Twitter;
                    userProfile.DiscordUsername = profile.DiscordInvite;
                    userProfile.CompanyName = profile.CompanyName;
                    userProfile.CompanyAge = profile.CompanyAge;
                    userProfile.CompanyDetails = profile.CompanyDetails;
                    userProfile.Website = profile.Website;
                    userProfile.Twitter = profile.Twitter;
                    userProfile.Facebook = profile.Facebook;
                    userProfile.Instagram = profile.Instagram;
                    userProfile.DiscordInvite = profile.DiscordInvite;
                    userProfile.Logo = profile.Logo;
                    userProfile.Country = profile.Country;
                    userProfile.City = profile.City;

                    _context.Update(userProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileExists(profile.Id))
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
            return View(profile);
        }

        // GET: Profiles/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Profiles == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        // POST: Profiles/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Profiles == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Profiles'  is null.");
            }
            var profile = await _context.Profiles.FindAsync(id);
            if (profile != null)
            {
                _context.Profiles.Remove(profile);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfileExists(int id)
        {
            return (_context.Profiles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
