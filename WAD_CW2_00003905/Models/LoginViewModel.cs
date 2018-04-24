using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WAD_CW2_00003905.Models
{
    public class LoginViewModel
    {
        [Required]
        [DisplayName("Username or Email")]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}