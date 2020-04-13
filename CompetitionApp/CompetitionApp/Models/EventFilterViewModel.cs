using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompetitionApp.Models
{
    public class EventFilterViewModel
    {
        public EventFilterViewModel(List<Category> parentCategories, List<Category> categories, int? parentCategory, int? category, string title)
        {
            parentCategories.Insert(0, new Category { Id = 0, Name = "Усі", ParentCategoryId = 0 });
            categories.Insert(0, new Category { Id = 0, Name = "Усі", ParentCategoryId = 0 });
            ParentCategories = new SelectList(parentCategories, "Id", "Name", parentCategory);
            Categories = new SelectList(categories, "Id", "Name", category);
            SelectedParentCategory = parentCategory;
            SelectedCategory = category;
            SelectedTitle = title;
        }

        public SelectList ParentCategories { get; private set; }
        public SelectList Categories { get; private set; }
        public int? SelectedParentCategory { get; private set; }
        public int? SelectedCategory { get; private set; }
        public string SelectedTitle { get; private set; }
    }
}
