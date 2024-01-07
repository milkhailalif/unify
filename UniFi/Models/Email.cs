using System.ComponentModel.DataAnnotations;

namespace UniFi.Models
{
    public class Email
    {
        public int Id { get; set; }
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAdd { get; set; }

    }
}
