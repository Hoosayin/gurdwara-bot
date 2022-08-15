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
    public class GuestHouseCardFactory
    {
        public static List<Attachment> CreateGuesHouseAttachments()
        {
            List<Attachment> attachments = new List<Attachment>();

            foreach (GuestHouse guestHouse in _guestHouses)
            {
                AdaptiveCard card = AdaptiveCardFactory.CreateAdaptiveCard(PathFactory.CreateAdaptiveCardsPath("GuestHouseCard.json"));
                (AdaptiveCardFactory.CreateAdaptiveElement(card, "Image") as AdaptiveImage).Url = guestHouse.ImageUri;
                (AdaptiveCardFactory.CreateAdaptiveElement(card, "Name") as AdaptiveTextBlock).Text = guestHouse.Name;
                (AdaptiveCardFactory.CreateAdaptiveElement(card, "Rating") as AdaptiveTextBlock).Text = guestHouse.Rating;
                (AdaptiveCardFactory.CreateAdaptiveElement(card, "Facts") as AdaptiveFactSet).Facts = guestHouse.Facts;
                card.Actions[0].AdditionalProperties["url"] = guestHouse.Directions;
                attachments.Add(AdaptiveCardFactory.CreateAdaptiveCardAttachment(card));
            }

            return attachments;
        }

        private static readonly List<GuestHouse> _guestHouses = new List<GuestHouse>
        {
            new GuestHouse
            {
                ImageUri = new Uri(PathFactory.CreateImagePath("pof_guest_hotel.jpg")),
                Name = "POF Guest House",
                Rating = "4.1 ★★★★☆ (1,025) · Reviews",
                Directions = "https://www.google.com/maps/place/POF+Guest+House/@33.7801659,72.7346812,17z/data=!3m1!4b1!4m5!3m4!1s0x38dfa7bc26033ebf:0x933cf4a66ab9fa99!8m2!3d33.7801659!4d72.7368699",
                Facts = new List<AdaptiveFact>
                {
                    new AdaptiveFact
                    {
                        Title = "Address:",
                        Value = "Quaid Avenue, Officers Colony, Wah Cantt, Wah"
                    },
                    new AdaptiveFact
                    {
                        Title = "Phone:",
                        Value = "+92514539982",
                    }
                },
            },
            new GuestHouse
            {
                ImageUri = new Uri(PathFactory.CreateImagePath("royalson_hotel.jpg")),
                Name = "Royalson Hotel & Restaurant",
                Rating = "4.1 ★★★★☆ (923) · Reviews",
                Directions = "https://www.google.com/maps/place/Royalson+Hotel+%26+Restaurant/@33.7345446,72.8006562,17z/data=!3m1!4b1!4m5!3m4!1s0x38dfa6aa3f23e4e5:0xf3ece3cc74cbd070!8m2!3d33.7345446!4d72.8028449",
                Facts = new List<AdaptiveFact>
                {
                    new AdaptiveFact
                    {
                        Title = "Address:",
                        Value = "Grand Trunk Rd, Sarai Khola Taxila, Rawalpindi, Punjab"
                    },
                    new AdaptiveFact
                    {
                        Title = "Phone:",
                        Value = "+92514548400",
                    }
                },
            },
            new GuestHouse
            {
                ImageUri = new Uri(PathFactory.CreateImagePath("wah_continental.jpg")),
                Name = "Wah Continental Hotel",
                Rating = "4.0 ★★★★☆ (785) · Reviews",
                Directions = "https://www.google.com/maps/place/Wah+Continental+Hotel/@33.7984009,72.7103815,17z/data=!3m1!4b1!4m5!3m4!1s0x38dfa7f8d55ae023:0x5f80067262694568!8m2!3d33.7984009!4d72.7125702",
                Facts = new List<AdaptiveFact>
                {
                    new AdaptiveFact
                    {
                        Title = "Address:",
                        Value = "Near Mughal Gardens, G.T Road, Wah Cantt"
                    },
                    new AdaptiveFact
                    {
                        Title = "Phone:",
                        Value = "+92514532300",
                    }
                },
            },
            new GuestHouse
            {
                ImageUri = new Uri(PathFactory.CreateImagePath("wah_palace.jpg")),
                Name = "Wah Palace Hotel & Restaurant",
                Rating = "3.9 ★★★★☆ (227) · Reviews",
                Directions = "https://www.google.com/maps/place/Wah+Palace+Hotel+%26+Restaurant/@33.7546871,72.7477218,17z/data=!3m1!4b1!4m5!3m4!1s0x38dfa704b5c3c27f:0xaffa908b83bcca66!8m2!3d33.7546871!4d72.7499105",
                Facts = new List<AdaptiveFact>
                {
                    new AdaptiveFact
                    {
                        Title = "Address:",
                        Value = "Losar Stop, G.T Road, Wah Cantt، Wah"
                    },
                    new AdaptiveFact
                    {
                        Title = "Phone:",
                        Value = "+92514904200",
                    }
                },
            }
        };
    }
}
