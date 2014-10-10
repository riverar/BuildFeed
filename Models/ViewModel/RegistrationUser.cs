using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BuildFeed.Models.ViewModel
{
    public class RegistrationUser
    {
        [Required]
        [DisplayName("Username")]
        public string UserName { get; set; }

        [Required]
        [MinLength(12)]
        [DisplayName("Enter Password")]
        public string Password { get; set; }

        [Required]
        [MinLength(12)]
        [DisplayName("Confirm Password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }
    }
}