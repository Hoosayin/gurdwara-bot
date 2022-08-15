using AdaptiveCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Models
{
    public class PopularTime
    {
        public string Day { get; set; }
        public Uri ImageUri { get; set; }
        public string Timings { get; set; }
        public string BestHour { get; set; }
    }
}
