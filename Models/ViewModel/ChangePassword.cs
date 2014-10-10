using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BuildFeed.Models.ViewModel
{
    public class ChangePassword
    {
        [Required]
        [MinLength(12)]
        [DisplayName("Enter Old Password")]
        public string OldPassword { get; set; }

        [Required]
        [MinLength(12)]
        [DisplayName("Enter New Password")]
        public string NewPassword { get; set; }

        [Required]
        [MinLength(12)]
        [DisplayName("Confirm New Password")]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }
}