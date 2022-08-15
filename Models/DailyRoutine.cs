using AdaptiveCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Models
{
    public class DailyRoutine
    {
        public string Name { get; set; }
        public List<DateTime> Duration { get; set; }
        public List<AdaptiveFact> Facts { get; set; }
    }
}
