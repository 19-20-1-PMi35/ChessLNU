using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetitionApp.Models
{
    public class UserHistory
    {
        public int Id { get; set; }
        public int Result { get; set; }
        public Event Event { get; set; }
        public User User { get; set; }
    }
}
