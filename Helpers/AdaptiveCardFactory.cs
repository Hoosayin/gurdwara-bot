using AdaptiveCards;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Helpers
{
    public class AdaptiveCardFactory
    {
        public static AdaptiveCard CreateAdaptiveCard(string path)
        {
            try
            {
                var adaptiveCardJson = File.ReadAllText(path);
                AdaptiveCardParseResult result = AdaptiveCard.FromJson(adaptiveCardJson);
                 return result.Card;
            }
            catch (AdaptiveSerializationException)
            {
                throw;
            }
        }

        public static Attachment CreateAdaptiveCardAttachment(AdaptiveCard card)
        {
            return new Attachment
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = card,
            };
        }

        public static AdaptiveElement CreateAdaptiveElement(AdaptiveCard card, string adaptiveElementId)
        {
            return card.Body.Find(ae => ae.Id == adaptiveElementId);
        }
    }
}
