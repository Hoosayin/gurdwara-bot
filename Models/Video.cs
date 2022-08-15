using AdaptiveCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Models
{
    public class Video
    {
        public List<AdaptiveMediaSource> MediaSources { get; set; }
        public string PosterUrl { get; set; }
        public string Title { get; set; }
    }
}
