using Blog_Website.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Website.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            var IdentityUser = new IdentityUser
            {
                UserName = registerVM.Username,
                Email = registerVM.Email,
            };

            var identityResult = await _userManager.CreateAsync(IdentityUser, registerVM.Password);

            if (identityResult.Succeeded)
            {
                //Assign this role to user
                var roleIdentityResult = await _userManager.AddToRoleAsync(IdentityUser, "User");

                if (roleIdentityResult.Succeeded)
                {
                    // Show success notification
                    return RedirectToAction("Register");
                }

            }
            return View();
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            var model = new LoginVM 
            
            { 
                ReturnUrl = ReturnUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false);

            if(signInResult != null && signInResult.Succeeded) 
            {
                if (!string.IsNullOrWhiteSpace(loginVM.ReturnUrl))
                {
                    return Redirect(loginVM.ReturnUrl);
                }



                // Show success notification
                return RedirectToAction("Index", "Home");
            }
            //show errors
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }

    }
