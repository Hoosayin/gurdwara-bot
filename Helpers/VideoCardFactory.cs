using AdaptiveCards;
using GurdwaraBot.Models;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Helpers
{
    public class VideoCardFactory
    {
        public static List<Attachment> CreateVideoAttachments()
        {
            List<AdaptiveCard> cards = new List<AdaptiveCard>();
            List<Attachment> attachments = new List<Attachment>();

            foreach (Video video in _videos)
            {
                AdaptiveCard card = AdaptiveCardFactory.CreateAdaptiveCard(PathFactory.CreateAdaptiveCardsPath("VideoCard.json"));
                (AdaptiveCardFactory.CreateAdaptiveElement(card, "video") as AdaptiveMedia).Sources = video.MediaSources;
                (AdaptiveCardFactory.CreateAdaptiveElement(card, "video") as AdaptiveMedia).Poster = video.PosterUrl;
                (AdaptiveCardFactory.CreateAdaptiveElement(card, "title") as AdaptiveTextBlock).Text = video.Title;
                attachments.Add(AdaptiveCardFactory.CreateAdaptiveCardAttachment(card));
            }

            return attachments;
        }

        private static readonly List<Video> _videos = new List<Video>
        {
            new Video
            {
                MediaSources = new List<AdaptiveMediaSource>
                {
                    new AdaptiveMediaSource
                    {
                        Url = PathFactory.CreateImagePath("panja_video_1.mp4"),
                        MimeType = "video/mp4",
                    },
                },
                Title = "Sikh Channel Special: Yatra - Gurdwara Sri Panja Sahib",
                PosterUrl = PathFactory.CreateImagePath("panja_thumbnail_1.png"),
            },
            new Video
            {
                MediaSources = new List<AdaptiveMediaSource>
                {
                    new AdaptiveMediaSource
                    {
                        Url = PathFactory.CreateImagePath("panja_video_2.mp4"),
                        MimeType = "video/mp4",
                    },
                },
                Title = "Visit to Panja Sahib Gurdwara - Kartarpur Yatra Committee UK",
                PosterUrl = PathFactory.CreateImagePath("panja_thumbnail_2.png"),
            },
            new Video
            {
                MediaSources = new List<AdaptiveMediaSource>
                {
                    new AdaptiveMediaSource
                    {
                        Url = PathFactory.CreateImagePath("panja_video_3.mp4"),
                        MimeType = "video/mp4",
                    },
                },
                Title = "Gurdwara Panja Sahib - Vaisakhi Festival",
                PosterUrl = PathFactory.CreateImagePath("panja_thumbnail_3.png"),
            },
        };
    }
}


