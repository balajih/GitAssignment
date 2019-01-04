using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TwitterCloneWebApp.DataAccess;

namespace TwitterCloneWebApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User Name")]      
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        [StringLength(30)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(50)]
        public string Email { get; set; }

    }
}
