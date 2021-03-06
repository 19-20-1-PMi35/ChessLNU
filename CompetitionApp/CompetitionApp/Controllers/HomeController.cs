﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CompetitionApp.Models;
using Microsoft.EntityFrameworkCore;
using CompetitionApp.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Identity;

namespace CompetitionApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ApplicationContext db;

        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;

        public HomeController(UserManager<User> userManager, ApplicationContext context)
        {
            _context = context;
            _userManager = userManager;

            db = context;
            if (db.Users.Count() == 0)
            {
                //this user is useless
                User u1 = new User { UserName = "admin", Password = "admin", Email = "someAdmin@gmail.com", IsAdmin = true };

                News news1 = new News { Title = "News1", PublicationDate = DateTime.Now, Content = "Some Content1", PublicatorId = u1.Id };
                News news2 = new News { Title = "News2", PublicationDate = DateTime.Now, Content = "Some Content2", PublicatorId = u1.Id };
                News news3 = new News { Title = "News3", PublicationDate = DateTime.Now, Content = "Some Content3", PublicatorId = u1.Id };
                News news4 = new News { Title = "News4", PublicationDate = DateTime.Now, Content = "Some Content4", PublicatorId = u1.Id };
                News news5 = new News { Title = "News5", PublicationDate = DateTime.Now, Content = "Some Content5", PublicatorId = u1.Id };
                News news6 = new News { Title = "News6", PublicationDate = DateTime.Now, Content = "Some Content6", PublicatorId = u1.Id };

                Category category1 = new Category { Name = "Sport", ParentCategoryId = 0 };
                Category category2 = new Category { Name = "Science", ParentCategoryId = 0 };
                Category category3 = new Category { Name = "Football", ParentCategoryId = 1 };
                Category category4 = new Category { Name = "Voleyball", ParentCategoryId = 1 };
                Category category5 = new Category { Name = "Algebra", ParentCategoryId = 2 };

                Event event1 = new Event { Title = "Event1", DateTime = DateTime.Now, Place = "Street 1", Category = category3, IsFinished = false };
                Event event2 = new Event { Title = "Event2", DateTime = DateTime.Now, Place = "Street 2", Category = category3, IsFinished = false };
                Event event3 = new Event { Title = "Event3", DateTime = DateTime.Now, Place = "Street 3", Category = category4, IsFinished = false };
                Event event4 = new Event { Title = "Event4", DateTime = DateTime.Now, Place = "Street 4", Category = category5, IsFinished = false };
                Event event5 = new Event { Title = "Event5", DateTime = DateTime.Now, Place = "Street 5", Category = category4, IsFinished = false };

                db.Add(u1);
                db.News.AddRange(news1, news2, news3, news4, news5, news6);
                db.Categories.AddRange(category1, category2, category3, category4, category5);
                db.Events.AddRange(event1, event2, event3, event4, event5);
                db.SaveChanges();
            }
        }

        public async Task<IActionResult> NewsInfo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await db.News.FirstOrDefaultAsync(n => n.Id == id);

            if (news == null)
            {
                return NotFound();
            }

            ViewBag.UserManager = _userManager; 
            ViewBag.Comments = _context.NewsComments.Where(x => x.NewsId == news.Id).OrderByDescending(x => x.Date).ToList();

            return View(new Tuple<News, NewsComment>(news, new NewsComment()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comment([Bind("Id,Text,Date,Text,UserId,NewsId,ParentId")] NewsComment @comment)
        {
            News news = _context.News.FirstOrDefault(x => x.Id == int.Parse(Request.Form["NewsId"]));

            if (ModelState.IsValid)
            {
                NewsComment newComment = new NewsComment
                {
                    Number = _context.NewsComments.Where(x => x.NewsId == news.Id).Count() + 1,
                    Text = comment.Text,
                    Date = DateTime.Now,
                    UserId = _userManager.GetUserId(HttpContext.User),
                    NewsId = news.Id
                };

                _context.Add(@newComment);
                await _context.SaveChangesAsync();

                //return RedirectToAction(nameof(Index));
                return RedirectToAction("NewsInfo", new { id = news.Id });
            }
            return View(@news);
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 3;

            IQueryable<News> news = db.News;
            news = news.OrderByDescending(u => u.PublicationDate);
            var count = await news.CountAsync();
            var items = await news.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageView pageViewModel = new PageView(count, page, pageSize);
            IndexView viewModel = new IndexView
            {
                PageView = pageViewModel,
                News = items
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CreateNews(string Id)
        {
            if (Id == "")
                return Redirect("Index");
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNews(NewsViewModel _news)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.FirstOrDefault(u => u.Id == _news.PublicatorId) != null)
                {
                    User us = db.Users.FirstOrDefault(u => u.Id == _news.PublicatorId);
                    News news = new News() { Title = _news.Title, PublicationDate = DateTime.Now, Content = _news.Content, PublicatorId = us.Id };
                    if (_news.Image != null)
                    {
                        byte[] imageData = null;

                        using (var binaryReader = new BinaryReader(_news.Image.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)_news.Image.Length);
                        }

                        news.Image = imageData;
                    }
                    db.News.Add(news);
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }               
            }
            return View(_news);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
