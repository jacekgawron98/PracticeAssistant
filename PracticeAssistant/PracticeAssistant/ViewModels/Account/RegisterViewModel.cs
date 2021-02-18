using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeAssistant.ViewModels.Account
{
    public class RegisterViewModel : LoginViewModel
    {   
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
