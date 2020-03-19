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

namespace CompetitionApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ApplicationContext db;

        public HomeController(ApplicationContext context)
        {
            db = context;
            if (db.Users.Count() == 0)
            {
                User u1 = new User { UserName = "some_user", Password = "Some2020", Email = "some@gmail.com", IsAdmin = false };

                News news1 = new News { Title = "Event1", PublicationDate = DateTime.Now, Content = "Some Content1", Publicator = u1 };
                News news2 = new News { Title = "Event2", PublicationDate = DateTime.Now, Content = "Some Content2", Publicator = u1 };
                News news3 = new News { Title = "Event3", PublicationDate = DateTime.Now, Content = "Some Content3", Publicator = u1 };
                News news4 = new News { Title = "Event4", PublicationDate = DateTime.Now, Content = "Some Content4", Publicator = u1 };
                News news5 = new News { Title = "Event5", PublicationDate = DateTime.Now, Content = "Some Content5", Publicator = u1 };
                News news6 = new News { Title = "Event6", PublicationDate = DateTime.Now, Content = "Some Content6", Publicator = u1 };

                
                db.Add(u1);
                db.News.AddRange(news1, news2, news3, news4, news5, news6);
                db.SaveChanges();
            }
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 3;

            IQueryable<News> news = db.News;
            news = news.OrderBy(u => u.PublicationDate);
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
                    News news = new News() { Title = _news.Title, PublicationDate = DateTime.Now, Content = _news.Content, Publicator = us };
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
