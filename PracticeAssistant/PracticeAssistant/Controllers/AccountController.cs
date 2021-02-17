using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PracticeAssistant.Models;
using PracticeAssistant.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeAssistant.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<PAUser> _signInManager;
        private UserManager<PAUser> _userManager;

        public AccountController(SignInManager<PAUser> signInManager, UserManager<PAUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new PAUser(model.Username, model.EmailAddress);
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //TO DO no sign in and email confirmation
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }
    }
}
