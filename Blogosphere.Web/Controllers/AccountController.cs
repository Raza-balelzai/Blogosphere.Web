using Blogosphere.Web.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Blogosphere.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var identityUser = new IdentityUser
                {
                    UserName = registerViewModel.UserName,
                    Email = registerViewModel.Email,
                };
                var IdentityResult = await _userManager.CreateAsync(identityUser, registerViewModel.Password);

                if (IdentityResult.Succeeded)
                {
                    //assign this "User" the userRole
                    var roleIdentityResult = await _userManager.AddToRoleAsync(identityUser, "User");
                    if (roleIdentityResult.Succeeded)
                    {
                        //Show Success notification
                        return RedirectToAction("Register");
                    }
                }
            }
           
            //Show failure Message
            
            return View(registerViewModel);
            
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var signInResult=await signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password,false,false);
            if (signInResult.Succeeded && signInResult != null)
            {
                //Show Success Message
                return RedirectToAction("Index", "Home");
            
            }
            //Show Failure Message
            return View(loginViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
