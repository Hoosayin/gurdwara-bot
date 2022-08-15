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
    public class FestivalCardFactory
    {
        public static Attachment CreateFestivalCardAttachment(string festivalName)
        {
            if (string.IsNullOrEmpty(festivalName))
            {
                return AdaptiveCardFactory.CreateAdaptiveCardAttachment(
                    AdaptiveCardFactory.CreateAdaptiveCard(PathFactory.CreateAdaptiveCardsPath("FestivalsCard.json")));
            }
            else
            {
                AdaptiveCard card = AdaptiveCardFactory.CreateAdaptiveCard(PathFactory.CreateAdaptiveCardsPath("FestivalDetailsCard.json"));

                foreach (KeyValuePair<string, SikhFestival> sikhFestival in _sikhFestivals)
                {
                    if (sikhFestival.Key == festivalName)
                    {
                        card.Speak = sikhFestival.Value.Description;
                        (AdaptiveCardFactory.CreateAdaptiveElement(card, "festivalImage") as AdaptiveImage).Url = sikhFestival.Value.Image;
                        (AdaptiveCardFactory.CreateAdaptiveElement(card, "festivalName") as AdaptiveTextBlock).Text = sikhFestival.Value.Name;
                        (AdaptiveCardFactory.CreateAdaptiveElement(card, "festivalType") as AdaptiveTextBlock).Text = sikhFestival.Value.Type;
                        (AdaptiveCardFactory.CreateAdaptiveElement(card, "festivalDescription") as AdaptiveTextBlock).Text = sikhFestival.Value.Description;
                        (AdaptiveCardFactory.CreateAdaptiveElement(card, "festivalFacts") as AdaptiveFactSet).Facts = sikhFestival.Value.Facts;
                    }
                }

                return AdaptiveCardFactory.CreateAdaptiveCardAttachment(card);
            }
        }

        private static readonly Dictionary<string, SikhFestival> _sikhFestivals = new Dictionary<string, SikhFestival>
        {
            {
                "Vaisakhi",
                new SikhFestival
                {
                    Image = new Uri(PathFactory.CreateImagePath("festivals_vaisakhi.jpg")),
                    Name = "Vaisakhi",
                    Type = "Festival",
                    Description = "Vaisakhi, also known as Baisakhi, Vaishakhi, or Vasakhi is a historical and religious festival in Sikhism. It is usually celebrated on 13 or 14 April every year. Vaisakhi marks the Sikh new year and commemorates the formation of Khalsa panth of warriors under Guru Gobind Singh in 1699.",
                    Facts = new List<AdaptiveFact>
                    {
                        new AdaptiveFact("Observances:", "Prayers, Processions, Raising of the Nishan Sahib Flag"),
                        new AdaptiveFact("Date:", "April 14"),
                        new AdaptiveFact("Significance:", "Sikh New Year, Harvest Festival, Birth of the Khalsa"),
                        new AdaptiveFact("Celebrations:", "Parades and Nagar Kirtan, Fairs, Amrit Sanchaar (Baptism) for New Khalsa"),
                    },
                }
            },
            {
                "Gurpurab",
                new SikhFestival
                {
                    Image = new Uri(PathFactory.CreateImagePath("festivals_gurpurab.jpeg")),
                    Name = "Guru Nanak Gurpurab",
                    Type = "Birth Anniversary Celebration",
                    Description = "Guru Nanak Gurpurab, also known as Guru Nanak's Prakash Utsav and Guru Nanak Jayanti, celebrates the birth of the first Sikh Guru, Guru Nanak. This is one of the most sacred festivals in Sikhism, or Sikhi. The festivities in the Sikh religion revolve around the anniversaries of the 10 Sikh Gurus.",
                    Facts = new List<AdaptiveFact>
                    {
                        new AdaptiveFact("Observances:", "Celebration of an Anniversary related to the lives of the Sikh gurus."),
                        new AdaptiveFact("Date:", "November 12"),
                        new AdaptiveFact("Significance:", "Guru Nanak's Birth Anniversary"),
                    },
                }
            },
            {
                "Maghi",
                new SikhFestival
                {
                    Image = new Uri(PathFactory.CreateImagePath("festivals_maghi.jpg")),
                    Name = "Mela Maghi",
                    Type = "Festivity",
                    Description = "Maghi is the annual festival and one of the seasonal gathering of the Sikhs. It is celebrated at Muktsar in the memory of forty Sikh martyrs, who once had deserted the tenth and last human Guru.",
                    Facts = new List<AdaptiveFact>
                    {
                        new AdaptiveFact("Observances:", "Ritual Bathing, Eat Traditional Food"),
                        new AdaptiveFact("Date:", "January 14"),
                        new AdaptiveFact("Significance:", "Midwinter Festival, Celebration of Winter Solstice"),
                    },
                }
            },
            {
                "Martyrdom of Guru Arjan Dev",
                new SikhFestival
                {
                    Image = new Uri(PathFactory.CreateImagePath("festivals_martyrdom_of_guru_arjan.jpg")),
                    Name = "Martyrdom of Guru Arjan Dev",
                    Type = "Holiday",
                    Description = "He refused, was tortured and executed in 1606 CE. Historical records and the Sikh tradition are unclear whether Guru Arjan was executed by drowning or died during torture. His martyrdom is considered a watershed event in the history of Sikhism.",
                    Facts = new List<AdaptiveFact>
                    {
                        new AdaptiveFact("Date:", "June 16"),
                    },
                }
            },
            {
                "Bandhi Chhor Divas",
                new SikhFestival
                {
                    Image = new Uri(PathFactory.CreateImagePath("festivals_bandi_chhor_divas.jpg")),
                    Name = "Bandi Chhor Divas",
                    Type = "Celebration",
                    Description = "Bandi Chhor Divas is a Sikh holiday which coincides with the day of Diwali. Sikhs historically celebrated Diwali along with Hindus, with Guru Amar Das explicitly listing it along with Vaisakhi as a festival for Sikhs.",
                    Facts = new List<AdaptiveFact>
                    {
                        new AdaptiveFact("Observances:", "Eat Traditional Food"),
                        new AdaptiveFact("Date:", "October 12 (May Vary)"),
                        new AdaptiveFact("Significance:", "Guru Hargobind was released from prison by the Mughal Emperor Jahangir."),
                    },
                }
            },
            {
                "Hola Mohalla",
                new SikhFestival
                {
                    Image = new Uri(PathFactory.CreateImagePath("festivals_hola_mohalla.jpg")),
                    Name = "Hola Mohalla",
                    Type = "Festivity",
                    Description = "Hola Mohalla, also called Hola, is a one-day Sikh festival which most often falls in March and takes place on the second day of the lunar month of Chett, a day after the Hindu spring festival Holi but sometimes coincides with Holi. Hola Mohalla is a big festive event for Sikhs around the world.",
                    Facts = new List<AdaptiveFact>
                    {
                        new AdaptiveFact("Observances:", "Three-Day Fair at Anandpur Sahib ending on Hola Mohalla day. Martial Arts"),
                        new AdaptiveFact("Date:", "March 22 - March 24 (May Vary)"),
                    },
                }
            },
            {
                "Parkash Ustav Dasveh Patshah",
                new SikhFestival
                {
                    Image = new Uri(PathFactory.CreateImagePath("festivals_parkash_utsav_dasveh_patshah.jpg")),
                    Name = "Parkash Ustav Dasveh Patshah",
                    Type = "Holiday",
                    Description = "This festival's name, when translated, means the birth celebration of the 10th Divine Light, or Divine Knowledges. It commemorates the birth of Guru Gobind Singh, the tenth Sikh guru. The festival is one of the most widely celebrated event by Sikhs.",
                    Facts = new List<AdaptiveFact>
                    {
                        new AdaptiveFact("Observances:", "Prayers for prosperity are offered."),
                        new AdaptiveFact("Date:", "January 2 (May Vary)"),
                        new AdaptiveFact("Significance:", "Guru Gobind Singh Jayanti."),
                    },
                }
            },
        };
    }
}
