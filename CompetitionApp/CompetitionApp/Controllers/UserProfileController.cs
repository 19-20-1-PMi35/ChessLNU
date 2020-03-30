using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CompetitionApp.Models;


namespace CompetitionApp.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly ApplicationContext _context;
        ApplicationContext db;

        public UserProfileController(ApplicationContext context)
        {
           db = context;
        }
        public IActionResult Index()
        {
            UserProfile up = db.UserProfiles.FirstOrDefault();
            return View(up);
        }
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @up = await _context.UserProfiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@up == null)
            {
                return NotFound();
            }

            return View(@up);
        }
    }
}