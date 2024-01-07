using System.ComponentModel.DataAnnotations;

namespace UniFi.Models
{
    public class Brand
    {
        public int Id { get; set; }
        [Display(Name = "User ID")]
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; }

        [Display(Name = "Display Name")]
        [Required(ErrorMessage = "Display Name is required")]
        public string DisplayName { get; set; }

        [Display(Name = "Display Text")]
        [Required(ErrorMessage = "Display Text is required")]
        public string DisplayText { get; set; }

        [Display(Name = "Primary Color")]
        [Required(ErrorMessage = "Primary Color is required")]
        public string PrimaryColor { get; set; }

        [Display(Name = "Accent Color")]
        [Required(ErrorMessage = "Accent Color is required")]
        public string AccentColor { get; set; }

        [Display(Name = "Text Color")]
        [Required(ErrorMessage = "Text Color is required")]
        public string TextColor { get; set; }

        [Display(Name = "Card Color")]
        [Required(ErrorMessage = "Background Color is required")]
        public string BackgroundColor { get; set; }

        [Display(Name = "Banner Image 1 URL (1200 x 590)")]
        [Required(ErrorMessage = "Banner Image is required")]
        public string BackgroundImage { get; set; }

        [Display(Name = "Banner Image 2 URL (1200 x 590)")]
        public string? BackgroundImage1 { get; set; }

        [Display(Name = "Banner Video (Youtube URL)")]
        public string? BackgroundVideo { get; set; }

        [Display(Name = "Brand Image URL (1200 x 590)")]
        [Required(ErrorMessage = "Brand Image is required")]
        public string? BrandImage { get; set; }


    }
}
