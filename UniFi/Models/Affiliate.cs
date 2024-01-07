using System.ComponentModel.DataAnnotations;

namespace UniFi.Models
{
    public class Affiliate
    {
        public int Id { get; set; }
        [Display(Name = "User ID")]
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; }

        [Display(Name = "Wallet")]
        [Required(ErrorMessage = "Wallet is required")]
        public string Wallet { get; set; }

        [Display(Name = "Affiliate Code")]
        public string AffiliateCode { get; set; }
        [Display(Name = "Approved")]
        public bool Approved { get; set; }

    }
}
