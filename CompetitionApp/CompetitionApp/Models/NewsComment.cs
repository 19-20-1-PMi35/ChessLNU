
using System;
using System.Collections.Generic;

namespace CompetitionApp.Models
{
    public class NewsComment
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public int NewsId { get; set; }
        public int ParentId { get; set; }
    }
}
