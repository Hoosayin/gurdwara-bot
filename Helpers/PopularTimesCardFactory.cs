using AdaptiveCards;
using GurdwaraBot.Models;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Helpers
{
    public class PopularTimesCardFactory
    {
        public static Attachment CreatePopularTimesCardAttachment(string luisEntity)
        {
            AdaptiveCard card = AdaptiveCardFactory.CreateAdaptiveCard(PathFactory.CreateAdaptiveCardsPath("PopularTimesCard.json"));

            if (string.IsNullOrEmpty(luisEntity))
            {
                DateTime today = DateTime.Today;

                foreach (PopularTime popularTime in _popularTimes)
                {
                    if (popularTime.Day == (today.DayOfWeek.ToString("g") + "s"))
                    {
                        card = PopulateCard(card, popularTime);
                    }
                }                
            }
            else if (luisEntity.Equals("Tomorrow"))
            {
                DateTime tomorrow = DateTime.Today.AddDays(1);

                foreach (PopularTime popularTime in _popularTimes)
                {
                    if (popularTime.Day == (tomorrow.DayOfWeek.ToString("g") + "s"))
                    {
                        card = PopulateCard(card, popularTime);
                    }
                }
            }
            else
            {
                foreach (PopularTime popularTime in _popularTimes)
                {
                    if (popularTime.Day == (luisEntity + "s"))
                    {
                        card = PopulateCard(card, popularTime);
                    }
                }
            }

            return AdaptiveCardFactory.CreateAdaptiveCardAttachment(card);
        }

        private static AdaptiveCard PopulateCard(AdaptiveCard card, PopularTime popularTime)
        {
            (AdaptiveCardFactory.CreateAdaptiveElement(card, "image") as AdaptiveImage).Url = popularTime.ImageUri;
            (AdaptiveCardFactory.CreateAdaptiveElement(card, "day") as AdaptiveTextBlock).Text = popularTime.Day;
            (AdaptiveCardFactory.CreateAdaptiveElement(card, "timings") as AdaptiveTextBlock).Text = popularTime.Timings;
            (AdaptiveCardFactory.CreateAdaptiveElement(card, "bestHour") as AdaptiveTextBlock).Text = popularTime.BestHour;
            return card;
        }

        private static readonly List<PopularTime> _popularTimes = new List<PopularTime>
        {
            new PopularTime
            {
                ImageUri = new Uri(PathFactory.CreateImagePath("mondays.jpg")),
                Day = "Mondays",
                Timings = "**Timings:** 9:00 AM - 9:00 PM",
                BestHour = "**Best Hour:** 2:00 PM",
            },
            new PopularTime
            {
                ImageUri = new Uri(PathFactory.CreateImagePath("tuesdays.jpg")),
                Day = "Tuesdays",
                Timings = "**Timings:** 8:00 AM - 9:00 PM",
                BestHour = "**Best Hour:** 6:00 PM",
            },
            new PopularTime
            {
                ImageUri = new Uri(PathFactory.CreateImagePath("wednesdays.jpg")),
                Day = "Wednesdays",
                Timings = "**Timings:** 8:00 AM - 9:00 PM",
                BestHour = "**Best Hour:** 6:00 PM",
            },
            new PopularTime
            {
                ImageUri = new Uri(PathFactory.CreateImagePath("thursdays.jpg")),
                Day = "Thursdays",
                Timings = "**Timings:** 9:00 AM - 10:00 PM",
                BestHour = "**Best Hour:** 2:00 PM",
            },
            new PopularTime
            {
                ImageUri = new Uri(PathFactory.CreateImagePath("fridays.jpg")),
                Day = "Fridays",
                Timings = "**Timings:** 8:00 AM - 1:00 AM",
                BestHour = "**Best Hour:** 6:00 PM",
            },
            new PopularTime
            {
                ImageUri = new Uri(PathFactory.CreateImagePath("saturdays.jpg")),
                Day = "Saturdays",
                Timings = "**Timings:** 8:00 AM - 4:00 AM",
                BestHour = "**Best Hour:** 1:00 PM",
            },
            new PopularTime
            {
                ImageUri = new Uri(PathFactory.CreateImagePath("sundays.jpg")),
                Day = "Sundays",
                Timings = "**Timings:** 5:00 AM - 11:00 PM",
                BestHour = "**Best Hour:** 1:00 PM",
            },
        };
    }
}
