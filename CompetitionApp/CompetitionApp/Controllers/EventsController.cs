using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CompetitionApp.Models;
using CompetitionApp.ViewModels;

namespace CompetitionApp.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;

        public EventsController(UserManager<User> userManager, ApplicationContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.Include(c => c.Category).ToListAsync());
        }

        public async Task<IActionResult> Show(int? parentCategory, int? category, string title, int page = 1)
        {
            int pageSize = 3;

            IQueryable<Event> events = _context.Events.Include(c => c.Category);

            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId != null)
            {
                List<UserHistory> @userHistory = await _context.UserHistories.Include(e => e.Event).ToListAsync();
                if (@userHistory != null)
                {
                    List<int> userEventsId = new List<int>();
                    for (int i = 0; i < @userHistory.Count(); i++)
                    {
                        userEventsId.Add(@userHistory[i].EventId);
                    }
                    ViewBag.Id = userEventsId;
                }
                else
                {
                    ViewBag.Id = null;
                }
            }
            else
            {
                ViewBag.Id = null;
            }

            //filtration by category
            if (parentCategory != 0 && parentCategory != null)
            {
                events = events.Where(x => x.Category.ParentCategoryId == parentCategory);

                if (category != 0 && category != null && events.FirstOrDefault(e => e.Category.Id == category) != null)
                {
                    events = events.Where(c => c.Category.Id == category);
                }
            }
            else
            {
                if (category != 0 && category != null)
                {
                    events = events.Where(c => c.Category.Id == category);
                }
            }

            //filtration by name
            if (!String.IsNullOrEmpty(title))
            {
                events = events.Where(e => e.Title.Contains(title));
            }

            //sorting by date
            events = events.OrderBy(d => d.DateTime);

            //pagination
            var count = await events.CountAsync();
            var items = await events.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            EventView viewModel = null;
            if (parentCategory != 0 && parentCategory != null)
            {
                viewModel = new EventView
                {
                    PageView = new PageView(count, page, pageSize),
                    EventFilterViewModel = new EventFilterViewModel(_context.Categories.Where(c => c.ParentCategoryId == 0).ToList(), _context.Categories.Where(x => x.ParentCategoryId == parentCategory).ToList(), parentCategory, category, title),
                    Events = items
                };
            }
            else
            {
                viewModel = new EventView
                {
                    PageView = new PageView(count, page, pageSize),
                    EventFilterViewModel = new EventFilterViewModel(_context.Categories.Where(c => c.ParentCategoryId == 0).ToList(), _context.Categories.Where(x => x.ParentCategoryId != 0).ToList(), parentCategory, category, title),
                    Events = items
                };
            }
            return View(viewModel);
        }
        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        public async Task<IActionResult> UserRegistrationOnEvent(int eventId)
        {
            var @event = await _context.Events.FindAsync(eventId);
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _context.Users.Include(e => e.History).FirstOrDefault(u => u.Id == userId);

            if (@event != null && user != null)
            {
                UserHistory userHistory = new UserHistory { Event = @event, EventId = @event.Id, UserId = @user.Id, User = @user };
                user.History.Add(userHistory);
                _context.UserHistories.Add(userHistory);
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Show));
        }

        public async Task<IActionResult> CanselUserRegistrationOnEvent(int eventId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _context.Users.Include(e => e.Profile).FirstOrDefault(u => u.Id == userId);
            var userHistory = _context.UserHistories.Include(e => e.Event).FirstOrDefault(u => u.UserId == userId && u.EventId == eventId);

            if (user != null && userHistory != null)
            {
                user.History.Remove(userHistory);
                _context.Entry(user).State = EntityState.Modified;
                _context.Entry(userHistory).State = EntityState.Deleted;
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Show));
        }

        // GET: Events/Create
        public async Task<IActionResult> Create()
        {

            List<Category> parentCategories = await _context.Categories.Where(p => p.ParentCategoryId == 0).ToListAsync();
            List<SelectListGroup> selectListGroups = new List<SelectListGroup>();
            for (int i = 0; i < parentCategories.Count(); i++)
            {
                SelectListGroup selectGroup = new SelectListGroup { Name = parentCategories[i].Name };
                selectListGroups.Add(selectGroup);
            }

            List<Category> categories = await _context.Categories.Where(p => p.ParentCategoryId != 0).ToListAsync();
            var selectCategories = new List<SelectListItem>();
            foreach (var item in categories)
            {
                string groupName = _context.Categories.Find(item.ParentCategoryId).Name;
                SelectListItem selectListItem = new SelectListItem { Text = item.Name, Value = item.Id.ToString(), Group = selectListGroups.FirstOrDefault(x => x.Name == groupName), Selected = false };
                selectCategories.Add(selectListItem);
            }
            SelectListItem selectList = new SelectListItem();
            selectCategories.Insert(0, selectList);

            ViewBag.Categories = selectCategories;
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel @event)
        {
            if (ModelState.IsValid)
            {
                Event newEvent = new Event { Id = @event.Id, Title = @event.Title, DateTime = @event.DateTime, Place = @event.Place, Category = await _context.Categories.FindAsync(@event.CategoryId) };
                _context.Update(newEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.Include(c => c.Category).FirstOrDefaultAsync(e => e.Id == id);
            if (@event == null)
            {
                return NotFound();

            }
            EventViewModel eventsView = new EventViewModel { Id = @event.Id, Title = @event.Title, CategoryId = @event.Category.Id, DateTime = @event.DateTime, Place = @event.Place, IsFinished = @event.IsFinished };

            List<Category> parentCategories = await _context.Categories.Where(p => p.ParentCategoryId == 0).ToListAsync();
            List<SelectListGroup> selectListGroups = new List<SelectListGroup>();
            for (int i = 0; i < parentCategories.Count(); i++)
            {
                SelectListGroup selectGroup = new SelectListGroup { Name = parentCategories[i].Name };
                selectListGroups.Add(selectGroup);
            }

            List<Category> categories = await _context.Categories.Where(p => p.ParentCategoryId != 0).ToListAsync();
            var selectCategories = new List<SelectListItem>();
            foreach (var item in categories)
            {
                bool isSelected = false;
                if (item.Id == eventsView.CategoryId)
                    isSelected = true;
                string groupName = _context.Categories.Find(item.ParentCategoryId).Name;
                SelectListItem selectListItem = new SelectListItem { Text = item.Name, Value = item.Id.ToString(), Group = selectListGroups.FirstOrDefault(x => x.Name == groupName), Selected = isSelected };
                selectCategories.Add(selectListItem);
            }

            ViewBag.Categories = selectCategories;
            return View(eventsView);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventViewModel @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Event updateEvent = new Event { Id = @event.Id, Title = @event.Title, DateTime = @event.DateTime, Place = @event.Place, Category = await _context.Categories.FindAsync(@event.CategoryId) };
                    _context.Update(updateEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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
            return View(@event);
        }
        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
