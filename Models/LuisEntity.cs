using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Models
{
    public class LuisEntity
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public List<string> Resolution { get; set; }
        public float Score { get; set; }
    }
}
