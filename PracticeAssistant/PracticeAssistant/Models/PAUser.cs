using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeAssistant.Models
{
    public class PAUser : IdentityUser
    {
        public PAUser()
        {

        }

        public PAUser(string username, string email)
        {
            UserName = username;
            Email = email;
        }
    }
}
