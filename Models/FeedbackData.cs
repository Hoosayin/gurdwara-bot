using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Models
{
    public class FeedbackData
    {
        public string FeedbackType { get; set; }
        public string Comment { get; set; } = "No Comments";
        public string Rating { get; set; }
        public bool DataCollected { get; set; }
    }
}
