using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetitionApp.Models
{
    public class IndexView
    {
        public IEnumerable<News> News { get; set; }
        public PageView PageView { get; set; }
    }
}
