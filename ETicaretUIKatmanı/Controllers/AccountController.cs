using ETicaretDataKatmanı.Context;
using ETicaretDataKatmanı.Helpers;
using ETicaretDataKatmanı.Identity;
using ETicaretDataKatmanı.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETicaretUIKatmanı.Controllers
{
    public class AccountController : Controller
    {
        private readonly DbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, ETicaretContext context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            var user = _userManager.FindByNameAsync(loginViewModel.UserName).Result;
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("List", "Home");
                }
            }
            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
            return View(loginViewModel);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new AppUser
            {
                Name = model.FirstName,
                Surname = model.LastName,
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogoutPost()
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "Cart");
            if (cart != null)
            {
                cart.Clear();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "Cart", cart);
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("List", "Home");
        }
        public IActionResult Index()
        {
            return View();
        }

    }
}
