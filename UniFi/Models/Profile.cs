using System.ComponentModel.DataAnnotations;

namespace UniFi.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Email address")]
        public string EmailAdd { get; set; }
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Your Full Name is required")]
        public string FullName { get; set; }
        [Display(Name = "Wallet")]
        public string Wallet { get; set; }

        [Display(Name = "Twitter Username")]
        [Required(ErrorMessage = "Twitter Username is required")]
        public string TwitterUsername { get; set; }

        [Display(Name = "Discord Username")]
        [Required(ErrorMessage = "Discord Username is required")]
        public string DiscordUsername { get; set; }

        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "Company Name is required")]
        public string CompanyName { get; set; }

        [Display(Name = "Company Age  (Months)")]
        [Required(ErrorMessage = "Company Age is required")]
        public int CompanyAge { get; set; }

        [Display(Name = "Company Details")]
        [Required(ErrorMessage = "Company Details is required")]
        public string CompanyDetails { get; set; }

        [Display(Name = "Website")]
        [Required(ErrorMessage = "Website is required")]
        public string Website { get; set; }

        [Display(Name = "Twitter")]
        [Required(ErrorMessage = "Twitter is required")]
        public string Twitter { get; set; }

        [Display(Name = "Facebook")]
        [Required(ErrorMessage = "Facebook is required")]
        public string Facebook { get; set; }

        [Display(Name = "Instagram")]
        [Required(ErrorMessage = "Instagram is required")]
        public string Instagram { get; set; }

        [Display(Name = "Discord Invite")]
        [Required(ErrorMessage = "Discord Invite is required")]
        public string DiscordInvite { get; set; }

        [Display(Name = "Logo Image URL (500 x 500)")]
        [Required(ErrorMessage = "Logo is required")]
        public string Logo { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

    }
}
