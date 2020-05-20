using CompetitionApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompetitionApp.ViewModels
{
    public class UserProfileViewModel
    {
        public UserProfile userProfile { get; set; }
        public IEnumerable<Event> userEvents { get; set; }
    }
}
