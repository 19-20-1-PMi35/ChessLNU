using CompetitionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetitionApp.ViewModels
{
    public class ShowParticipantsViewModel
    {
        public Event CurrentEvent { get; set; }
        public IEnumerable<User> Participants { get; set; }
    }
}
