using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompetitionApp.ViewModels
{
    public class NewsViewModel
    {
        [Required]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Текст")]
        public string Content { get; set; }
        public string PublicatorId { get; set; }

        [Display(Name = "Картинка")]
        public IFormFile Image { get; set; }
    }
}
