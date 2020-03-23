using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetitionApp.Models
{
    public class EventView
    {
        public IEnumerable<Event> Events { get; set; }
        public PageView PageView { get; set; }
        public EventFilterViewModel EventFilterViewModel { get; set; }
    }
}
