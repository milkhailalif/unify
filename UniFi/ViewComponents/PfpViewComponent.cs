using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniFi.Data;

namespace UniFi.ViewComponents
{
    public class PfpViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public PfpViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var id = HttpContext.User.Identity.Name;
            var profile = await _context.UserProfiles.FirstOrDefaultAsync(m => m.UserId == id);
            string pfp = "";

            if (profile == null || profile.PFPUrl == "" || profile.PFPUrl == null)
            {
                pfp = "/images/pfp.png";
            }
            else
            {
                pfp = profile.PFPUrl;
            }

            return View("Default", pfp);

        }
    }
}
