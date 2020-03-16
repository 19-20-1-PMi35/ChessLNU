using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CompetitionApp.Models
{
    public class UserProfile
    {
        [Key, ForeignKey("User")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string University { get; set; }
        public string Faculty { get; set; }
        public User User { get; set; }
    }
}
