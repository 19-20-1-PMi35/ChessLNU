using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CompetitionApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Imaging;
using CompetitionApp.ViewModels;

namespace CompetitionApp.Controllers
{
    public class NewsController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;

        public NewsController(UserManager<User> userManager, ApplicationContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: News
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null || !user.IsAdmin)
            {
                return StatusCode(403);
            }
            else return View(await _context.News.ToListAsync());
        }

        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.News
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: News/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsViewModel model)
        {
            var news = new News { 
                Title = model.Title, 
                Content = model.Content, 
                PublicationDate = DateTime.Now, 
                PublicatorId = _userManager.GetUserId(HttpContext.User) 
            };

            if (model.Image != null)
            {
                byte[] imageData = null;

                using (var binaryReader = new BinaryReader(model.Image.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)model.Image.Length);
                }

                news.Image = imageData;
            }

            _context.Add(@news);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @news = await _context.News.FindAsync(id);
            if (@news == null)
            {
                return NotFound();
            }
            return View(@news);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NewsViewModel model)
        {
            News news = _context.News.FirstOrDefault(x => x.Id == int.Parse(Request.Form["NewsId"]));

            news.Title = model.Title;
            news.Content = model.Content;

            if (model.Image != null)
            {
                byte[] imageData = null;

                using (var binaryReader = new BinaryReader(model.Image.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)model.Image.Length);
                }

                news.Image = imageData;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@news);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(@news.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@news);
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @news = await _context.News
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@news == null)
            {
                return NotFound();
            }

            return View(@news);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @news = await _context.News.FindAsync(id);
            _context.News.Remove(@news);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.Id == id);
        }
    }
}