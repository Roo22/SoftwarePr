using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SoftwarePr.Models
{
    public class UserLoginSignUp
    {
        [Key]
        public int userId { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Name Required")]
        public string Name { get; set; }

        [StringLength(50)]
        [EmailAddress]
        [Required(ErrorMessage = "Email Required")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Does not Match!")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password not Matched")]
        public string ConfirmPassword { get; set; }

    }
}