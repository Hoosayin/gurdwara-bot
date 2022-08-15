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
    public class ImageCardFactory
    {
        public static List<Attachment> CreateImageAttachments()
        {
            List<AdaptiveCard> cards = new List<AdaptiveCard>();
            List<Attachment> attachments = new List<Attachment>();

            foreach (Image image in _images)
            {
                AdaptiveCard card = AdaptiveCardFactory.CreateAdaptiveCard(PathFactory.CreateAdaptiveCardsPath("ImageCard.json"));
                (AdaptiveCardFactory.CreateAdaptiveElement(card, "image") as AdaptiveImage).Url = image.ImageUri;
                (AdaptiveCardFactory.CreateAdaptiveElement(card, "title") as AdaptiveTextBlock).Text = image.Title;
                (AdaptiveCardFactory.CreateAdaptiveElement(card, "caption") as AdaptiveTextBlock).Text = image.Caption;
                attachments.Add(AdaptiveCardFactory.CreateAdaptiveCardAttachment(card));
            }

            return attachments;
        }

        private static readonly List<Image> _images = new List<Image>
        {
            new Image
            {
                ImageUri = new Uri(PathFactory.CreateImagePath("1.jpg")),
                Title = "Outside View - Enterance",
                Caption = "Entrance to Panja Sahib Gurdwara in Hasan Abdal.",
            },
            new Image
            {
                ImageUri = new Uri(PathFactory.CreateImagePath("2.jpg")),
                Title = "Inside View - Hall",
                Caption = "The interior of the Gurdwara at Panja sahib.",
            },
            new Image
            {
                ImageUri = new Uri(PathFactory.CreateImagePath("3.jpg")),
                Title = "Handprint - Fishes",
                Caption = "Fish in spring underneath hand print at Panja Sahib Gurdwara in Hasan Abdal.",
            },
            new Image
            {
                ImageUri = new Uri(PathFactory.CreateImagePath("4.jpg")),
                Title = "Handprint - Panja Sahib",
                Caption = "Handprint on the boulder which is believed by Sikhs to be that of Guru Nanak.",
            },
            new Image
            {
                ImageUri = new Uri(PathFactory.CreateImagePath("5.jpg")),
                Title = "Outside View - Another One",
                Caption = "Entrance to Panja Sahib Gurdwara in Hasan Abdal.",
            },
            new Image
            {
                ImageUri = new Uri(PathFactory.CreateImagePath("6.jpg")),
                Title = "Rear View - Enterance",
                Caption = "The rear side of the gurdwara features another enterance.",
            },
            new Image
            {
                ImageUri = new Uri(PathFactory.CreateImagePath("7.jpg")),
                Title = "Langar",
                Caption = "Langar being served to all the visitors, without distinction of religion, caste, gender, economic status or ethnicity.",
            },
            new Image
            {
                ImageUri = new Uri(PathFactory.CreateImagePath("8.jpg")),
                Title = "The Month of November",
                Caption = "Sikh pilgrims reach Gurdwara Panja Sahib in November.",
            },
            new Image
            {
                ImageUri = new Uri(PathFactory.CreateImagePath("9.jpg")),
                Title = "A Beautiful View",
                Caption = "A photograph of Sri Panja Sahib by Pakistan's famous photographer.",
            },
            new Image
            {
                ImageUri = new Uri(PathFactory.CreateImagePath("10.jpg")),
                Title = "Sri Panja Sahib - Vaisakhi",
                Caption = "As many as 6,000 Sikhs from across the world celebrate the three-day Vaisakhi festival that culminates in Hasanabdal.",
            },
        };
    }
}
