using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetitionApp.Models
{
    public class User : IdentityUser
    {
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public UserProfile Profile { get; set; }
        public List<UserHistory> History { get; set; }
    }
}
