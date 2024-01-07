using System.ComponentModel.DataAnnotations;

namespace UniFi.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        [Display(Name = "User ID")]
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; }

        [Display(Name = "PFP url")]
        public string? PFPUrl { get; set; }

        [Display(Name = "Write Bio")]
        public string? Bio { get; set; }

    }
}
