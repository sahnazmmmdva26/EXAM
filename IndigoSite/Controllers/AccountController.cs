using IndigoSite.Models;
using IndigoSite.Utilies.Enum;
using IndigoSite.ViewModel;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IndigoSite.Controllers
{
    
    public class AccountController : Controller
    {
        UserManager<AppUser> _userManager { get; }
        SignInManager<AppUser> _signInManager { get; }
        RoleManager<IdentityRole> _roleManager { get; }

        public AccountController(RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterVM registerVM)
        {
            AppUser user = await _userManager.FindByNameAsync(registerVM.Username);
            if (user != null)
            {
                ModelState.AddModelError("", "Bu istifadeci artiq movcuddur");
                return View();
            }
            if (!ModelState.IsValid) return View();
            user = new AppUser
            {
                FirstName = registerVM.Name,
                LastName = registerVM.Surname,
                UserName = registerVM.Username,
                Email = registerVM.Email
            };
            var result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            await _signInManager.SignInAsync(user, true);
            return RedirectToAction(nameof(Login));

        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginVM loginVM)
        {
            AppUser user = await _userManager.FindByNameAsync(loginVM.NameOrUsername);
            if (user is null)
            {
                user = await _userManager.FindByEmailAsync(loginVM.NameOrUsername);
                if (user is null)
                {
                    ModelState.AddModelError("", "username ve ya parol yanlishdi");
                    return View();
                }
            }
            if (!ModelState.IsValid) return View();
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.IsPersistense, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "username ve ya parol yanlishdi");
                return View();
            }
            await _signInManager.SignInAsync(user, loginVM.IsPersistense);
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> AddRoles()
        {
            foreach (var item in Enum.GetValues(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
