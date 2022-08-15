using AdaptiveCards;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Helpers
{
    public class MenuCardFactory
    {
        public static Attachment CreateMenuCardAttachment()
        {
            AdaptiveCard card = AdaptiveCardFactory.CreateAdaptiveCard(PathFactory.CreateAdaptiveCardsPath("MenuCard.json"));
            (AdaptiveCardFactory.CreateAdaptiveElement(card, "image") as AdaptiveImage).Url = new Uri(PathFactory.CreateImagePath("menu_card_image.png"));
            return AdaptiveCardFactory.CreateAdaptiveCardAttachment(card);
        }
    }
}
