using AdaptiveCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Models
{
    public class GuestHouse
    {
        public string Name { get; set; }
        public string Rating { get; set; }
        public List<AdaptiveFact> Facts { get; set; }
        public string Directions { get; set; }
        public Uri ImageUri { get; set; }
    }
}
