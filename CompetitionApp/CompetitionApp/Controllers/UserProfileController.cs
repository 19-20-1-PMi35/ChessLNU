using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CompetitionApp.Models;
using CompetitionApp.ViewModels;

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
            var userHistories = db.UserHistories.Where(m => m.UserId == id).ToList();
            var events = new List<Event>();
            foreach(var uh in userHistories)
            {
                events.Add(db.Events.Where(e => e.Id == uh.EventId).FirstOrDefault());
            }
            var userProfile = db.UserProfiles.FirstOrDefault(m => m.Id == id);
            if (userProfile == null)
            {
                return NotFound();
            }

            return View(new UserProfileViewModel { userEvents = events, userProfile = userProfile });
        }
    }
}