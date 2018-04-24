using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WAD_CW2_00003905.Models
{
    public class RegistrationViewModel
    {
        [DisplayName("First Name")]
        [Required]
        public string Firstname { get; set; }
        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }
        [DisplayName("Email address")]
        [Required]
        public string Email{ get; set; }        
        [Required]
        public string Username{ get; set; }
        [Required]
        public string Password { get; set; }
        
    }
}