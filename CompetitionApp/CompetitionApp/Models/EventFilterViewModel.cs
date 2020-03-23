using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompetitionApp.Models
{
    public class EventFilterViewModel
    {
        public EventFilterViewModel(List<Category> categories, int? category, string title)
        {
            categories.Insert(0, new Category { Id = 0, Name = "Усі", ParentCategoryId = 0 });
            Categories = new SelectList(categories, "Id", "Name", category);
            SelectedCategory = category;
            SelectedTitle = title;
        }

        public SelectList Categories { get; private set; }
        public int? SelectedCategory { get; private set; }
        public string SelectedTitle { get; private set; }
    }
}
