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
    public class ReceiptCardFactory
    {
        public static Attachment CreateReceiptAttachment(UserData userData)
        {
            AdaptiveCard card = AdaptiveCardFactory.CreateAdaptiveCard(PathFactory.CreateAdaptiveCardsPath("ReceiptCard.json"));
            Uri uri = new Uri(PathFactory.CreateImagePath("bill.png"));
            AdaptiveColumnSet columnSet = AdaptiveCardFactory.CreateAdaptiveElement(card, "ColumnSet") as AdaptiveColumnSet;

            List<AdaptiveFact> facts = new List<AdaptiveFact>
            {
                new AdaptiveFact
                {
                    Title = "Name:",
                    Value = userData.Name,
                },
                new AdaptiveFact
                {
                    Title = "Email:",
                    Value = userData.Email,
                },
                new AdaptiveFact
                {
                    Title = "Checkin Date:",
                    Value = userData.CheckInDate.ToLongDateString(),
                },
                new AdaptiveFact
                {
                    Title = "Checkout Date:",
                    Value = userData.CheckOutDate.ToLongDateString(),
                },
                new AdaptiveFact
                {
                    Title = "Expiration Date:",
                    Value = userData.ExpirationDate.ToLongDateString(),
                },
                new AdaptiveFact
                {
                    Title = "Attendees:",
                    Value = $"{userData.Attendees} People",
                },
                new AdaptiveFact
                {
                    Title = "Diet:",
                    Value = userData.MealPreference,
                }
            };

            facts = AddRoomNumbers(facts, userData.RoomIds);
            (columnSet.Columns[0].Items[0] as AdaptiveFactSet).Facts = facts;

            (columnSet.Columns[1].Items[0] as AdaptiveImage).Url = uri;
            return AdaptiveCardFactory.CreateAdaptiveCardAttachment(card);
        }

        private static List<AdaptiveFact> AddRoomNumbers(List<AdaptiveFact> facts, List<string> roomIds)
        {
            foreach (string roomId in roomIds)
            {
                facts.Add(new AdaptiveFact
                {
                    Title = "Room #",
                    Value = roomId,
                });
            }

            return facts;
        }
    }
}
