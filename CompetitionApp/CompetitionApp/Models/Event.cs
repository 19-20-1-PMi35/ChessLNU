using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetitionApp.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Category Category { get; set; }
        public DateTime DateTime { get; set; }
        public string Place { get; set; }
        public bool IsFinished { get; set; }
    }
}
