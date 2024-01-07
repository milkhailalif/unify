using System.ComponentModel.DataAnnotations;

namespace UniFi.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Display(Name = "User ID")]
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; }

        [Display(Name = "Brand Name")]
        [Required(ErrorMessage = "Brand Name is required")]
        public string Brand { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }

        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }

        [Display(Name = "Image URL (350 x 350)")]
        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; }

        [Display(Name = "Alt Image1 URL (350 x 350)")]
        [Required(ErrorMessage = "Alt Image1 URL is required")]
        public string AltImage1 { get; set; }

        [Display(Name = "Alt Image2 URL (350 x 350)")]
        [Required(ErrorMessage = "Alt Image2 URL is required")]
        public string AltImage2 { get; set; }

        [Display(Name = "Alt Image3 URL (350 x 350)")]
        [Required(ErrorMessage = "Alt Image3 URL is required")]
        public string AltImage3 { get; set; }

        [Display(Name = "Feature1")]
        [StringLength(15)]
        public string? Feature1 { get; set; }
        [Display(Name = "Feature2")]
        [StringLength(15)]
        public string? Feature2 { get; set; }
        [Display(Name = "Feature3")]
        [StringLength(15)]
        public string? Feature3 { get; set; }
        [Display(Name = "Feature4")]
        [StringLength(15)]
        public string? Feature4 { get; set; }

        [Display(Name = "Feature5")]
        [StringLength(15)]
        public string? Feature5 { get; set; }

        [Display(Name = "Feature6")]
        [StringLength(15)]
        public string? Feature6 { get; set; }


        [Display(Name = "Service")]
        [Required(ErrorMessage = "Service is required")]
        public bool Service { get; set; }

        [Display(Name = "Disabled")]
        [Required(ErrorMessage = "Service is required")]
        public bool Disabled { get; set; }

    }
}
