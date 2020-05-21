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
        public IActionResult Index(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var up = db.UserProfiles.FirstOrDefault(m => m.Id == id);
            if (up == null)
            {
                return NotFound();
            }

            return View(up);
        }
    }
}