using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetitionApp.Models
{
    public class News
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Content { get; set; }
        public User Publicator { get; set; }
        public byte[] Image { get; set; }
    }
}
