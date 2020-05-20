using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CompetitionApp.ViewModels;
using CompetitionApp.Models;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.EntityFrameworkCore;

namespace CompetitionApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            //Uncomment it to create Admin User
            //User user1 = new User { UserName = "admin", Password = "Aaqwerty!1", Email = "someAdmin@gmail.com", IsAdmin = true };

            //// додавання користувача
            //var result1 = await _userManager.CreateAsync(user1, user1.Password);

            //if (result1.Succeeded)
            //{
            //    UserProfile profile = new UserProfile
            //    {
            //        Id = user1.Id,
            //        Name = model.Name,
            //        Surname = model.Surname,
            //        BirthDate = model.BirthDate,
            //        University = model.University,
            //        Faculty = model.Faculty
            //    };

            //    _context.UserProfiles.Add(profile);
            //    _context.SaveChanges();

            //    // встановлення cookies
            //    await _signInManager.SignInAsync(user1, false);
            //    return RedirectToAction("Index", "Home");
            //}


            if (ModelState.IsValid)
            {  
                User user = new User { Email = model.Email, UserName = model.UserName, IsAdmin = false };

                // додавання користувача
                var result = await _userManager.CreateAsync(user, model.Password);
  
                if (result.Succeeded)
                {
                    UserProfile profile = new UserProfile
                    {
                        Id = user.Id,
                        Name = model.Name,
                        Surname = model.Surname,
                        BirthDate = model.BirthDate,
                        University = model.University,
                        Faculty = model.Faculty
                    };

                    _context.UserProfiles.Add(profile);
                    _context.SaveChanges();

                    // встановлення cookies
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    // перевіряємо, чи належить URL застосунку
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильний логін або пароль");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // видаляємо аутентифікаційні cookies
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}