using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using UniFi.Data;
using UniFi.Models;

namespace UniFi.Controllers
{

    [EnableCors("HomePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<ActionResult> _Products()
        {
            
            return PartialView();
        }

        public async Task<ActionResult> _Services()
        {
            
            return PartialView();
        }

        public async Task<ActionResult> _Brands()
        {
            
            return PartialView();
        }
        [HttpGet]
        [Route("Test")]
        public IActionResult Test()
        {
            return Ok();
        }

        [HttpGet]
        [Route("Brands")]
        public async Task<IActionResult> Brands()
        {
            return _context.Brand != null ?
                        Ok(await _context.Brand.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Products'  is null.");
        }
        [HttpGet]
        [Route("Products")]
        public async Task<IActionResult> Products()
        {
            return _context.Products != null ?
                        Ok(await _context.Products.Where(m => m.Service == false && m.Disabled == false).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Products'  is null.");
        }
        [HttpGet]
        [Route("Services")]
        public async Task<IActionResult> Services()
        {
            return _context.Products != null ?
                        Ok(await _context.Products.Where(m => m.Service == true && m.Disabled == false).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Products'  is null.");
        }
        [HttpGet]
        [Authorize]
        [Route("GenerateLink")]
        public async Task<ActionResult> GenerateLink()
        {
            var affilID = HttpContext.User.Identity.Name;
            var afUser = await _context.Affiliates.FirstOrDefaultAsync(m => m.UserId == affilID);

            if (afUser == null)
            {
                return RedirectToAction("ConnectWallet", "Profiles");
            }

            var url = Request.Headers["Referer"].ToString();

            string urlShort = "";

            if (url.Contains("?"))
            {
                urlShort = url.Substring(0, url.IndexOf("?"));
            }
            else
            {
                urlShort = url;
            }

            var user = HttpContext.User.Identity.Name;

            var affilCode = afUser.AffiliateCode;

            string mLink = urlShort + "?code=" + affilCode;

            ViewBag.MainLink = mLink;
            ViewBag.FaceBook = mLink + "&&source=Facebook";
            ViewBag.Twitter = mLink + "&&source=Twitter";
            ViewBag.Instagram = mLink + "&&source=Instagram";
            ViewBag.Youtube = mLink + "&&source=Youtube";

            return Ok();
        }
        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var product = await _context.Products.Where(m => m.Disabled == false).ToListAsync();
            var brand = await _context.Brand.ToListAsync();

            var tupleModel = new Tuple<List<Product>, List<Brand>>(product, brand);

            var user = HttpContext.User.Identity.Name;

            if(user != null)
            {
                var affilUser = await _context.Affiliates
                .FirstOrDefaultAsync(m => m.UserId == user);

                if(affilUser == null)
                {
                    return RedirectToAction("ConnectWallet", "Profiles");
                }
            }

            return Ok(tupleModel);
        }
        [HttpGet]
        [Authorize]
        [Route("Profile")]
        public async Task<IActionResult> Profile()
        {
            var id = HttpContext.User.Identity.Name;
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            ViewBag.Id = id;
            ViewBag.Email = user.Email;

            var profile = await _context.UserProfiles.FirstOrDefaultAsync(m => m.UserId == id);
            var wallet = await _context.Affiliates.FirstOrDefaultAsync(m => m.UserId == id);
            var brand = await _context.Brand.FirstOrDefaultAsync(m => m.UserId == id);

            if(brand == null)
            {
                ViewBag.Brand = "False";
            }
            else
            {
                ViewBag.Brand = "True";
                ViewBag.BrandName = brand.DisplayName;
            }

            if (wallet == null)
            {
                return RedirectToAction("ConnectWallet", "Profiles");
            }

            ViewBag.Wallet = wallet.Wallet;

            if (profile == null)
            {
                UserProfile pro = new UserProfile();
                pro.UserId = id;
                _context.Add(pro);
                await _context.SaveChangesAsync();
                ViewBag.PFP = "../images/pfp.png";
                return Ok(pro);
            }

            if (profile.PFPUrl == null || profile.PFPUrl == "")
            {
                ViewBag.PFP = "../images/pfp.png";
            }
            else
            {
                ViewBag.PFP = profile.PFPUrl;
            }

            return Ok(profile);
        }

        // POST: Emails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Profile/{id}")]
        public async Task<IActionResult> Profile(int id, [Bind("Id,UserId,PFPUrl,Bio")] UserProfile profile)
        {
            var user = HttpContext.User.Identity.Name;
            if (id != profile.Id || user != profile.UserId)
            {
                return NotFound();
            }

            var userE = await _userManager.GetUserAsync(User);
            var wallet = await _context.Affiliates.FirstOrDefaultAsync(m => m.UserId == user);

            var brand = await _context.Brand.FirstOrDefaultAsync(m => m.UserId == user);

            if (brand == null)
            {
                ViewBag.Brand = "False";
            }
            else
            {
                ViewBag.Brand = "True";
            }

            ViewBag.Id = user;
            ViewBag.Email = userE.Email;
            ViewBag.Wallet = wallet.Wallet;

            if (profile.PFPUrl == null || profile.PFPUrl == "")
            {
                ViewBag.PFP = "../images/pfp.png";
            }
            else
            {
                ViewBag.PFP = profile.PFPUrl;
            }

            if (ModelState.IsValid)
            {
                    _context.Update(profile);
                    await _context.SaveChangesAsync();
                
                //return RedirectToAction(nameof(Index));
                return Ok(profile);
            }
            return Ok(profile);
        }
        [HttpGet]
        [Authorize]
        [Route("GetPFP")]
        public async Task<IActionResult> GetPFP()
        {
            var id = HttpContext.User.Identity.Name;
            var profile = await _context.UserProfiles.FirstOrDefaultAsync(m => m.UserId == id);
            var pfp = "";

            if (profile == null || profile.PFPUrl == "" || profile.PFPUrl == null)
            {
                pfp = "~/images/pfp.png";
            }
            else
            {
                pfp = profile.PFPUrl;
            }

            return Ok(pfp);
        }
        [HttpGet]
        [Route("Privacy")]
        public IActionResult Privacy()
        {
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        
        [HttpGet]
        [Route("Error")]
        public IActionResult Error()
        {
            return Ok(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
