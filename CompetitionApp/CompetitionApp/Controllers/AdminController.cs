using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompetitionApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompetitionApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;

        public AdminController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null || !user.IsAdmin) 
            {
                return StatusCode(403);
            } else return View();
        }
    }
}