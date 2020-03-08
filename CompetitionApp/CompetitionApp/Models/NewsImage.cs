using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetitionApp.Models
{
    public class NewsImage
    {
        public int Id { get; set; }
        public News News { get; set; }
        public byte[] Image { get; set; }
    }
}
