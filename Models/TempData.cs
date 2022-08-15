using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Models
{
    public class TempData
    {
        public static UserData UserData { get; set; }
        public static FeedbackData FeedbackData { get; set; }
        public static IHostingEnvironment HostingEnvironment { get; set; }
    }
}
