using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Models
{
    public class UserFeedback
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Comments { get; set; }
        public FeedbackRating Rating { get; set; }
    }
}
