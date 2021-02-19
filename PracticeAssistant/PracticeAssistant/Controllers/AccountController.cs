using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PracticeAssistant.Models;
using PracticeAssistant.Services;
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
        private IEmailService _emailService;

        public AccountController(SignInManager<PAUser> signInManager, UserManager<PAUser> userManager, IEmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager
                    .PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToLocalURL(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty,"Invalid login attempt.");
                }
            }
            return View(model);
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
                    var confirmCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmLink = Url.Action(nameof(ConfirmEmail), "Account", 
                        new { confirmCode, email = user.Email }, Request.Scheme);
                    await _emailService.SendEmailAsync(model.EmailAddress, "Email confirmation link", confirmLink);
                    return RedirectToAction(nameof(RegisterComplete));
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult RegisterComplete()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string confirmCode, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return RedirectToAction(nameof(Error));
            }
            var result = await _userManager.ConfirmEmailAsync(user, confirmCode);
            if (!result.Succeeded)
            {
                return RedirectToAction(nameof(Error));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }

        private IActionResult RedirectToLocalURL(string localURL)
        {
            if (Url.IsLocalUrl(localURL))
            {
                return Redirect(localURL);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

    }
}
