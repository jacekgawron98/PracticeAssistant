using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeAssistant.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required, MaxLength(255)]
        public string Username { get; set; }
        
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
        
        [DataType(DataType.Password),Compare(nameof(Password))]
        [Display(Name = "Confirm your password")]
        public string ConfirmPassword { get; set; }
        
        [Required,DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }

        [DataType(DataType.EmailAddress),Compare(nameof(EmailAddress))]
        [Display(Name = "Confirm your email address")]
        public string ConfirmEmail { get; set; }
    }
}
