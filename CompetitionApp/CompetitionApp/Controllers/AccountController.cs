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
    }
}