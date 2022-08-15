using AdaptiveCards;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Helpers
{
    public class FeedbackCardFactory
    {
        public static Attachment CreateFeebackCardAttachment()
        {
            AdaptiveCard card = AdaptiveCardFactory.CreateAdaptiveCard(PathFactory.CreateAdaptiveCardsPath("FeedbackCard.json"));
            (AdaptiveCardFactory.CreateAdaptiveElement(card, "Image") as AdaptiveImage).Url = new Uri(PathFactory.CreateImagePath("feedback.png"));
            return AdaptiveCardFactory.CreateAdaptiveCardAttachment(card);
        }
    }
}
