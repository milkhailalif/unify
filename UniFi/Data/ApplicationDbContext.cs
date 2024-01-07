using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniFi.Models;

namespace UniFi.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Email> Emails { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Affiliate> Affiliates { get; set; }
        
        public DbSet<AffiliateBrandLink> AffiliateBrandLinks { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}