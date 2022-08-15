using AdaptiveCards;
using GurdwaraBot.Models;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Helpers
{
    public class DailyRoutineCardFactory
    {
        public static Attachment CreateRoutineCardAttachment(string luisEntity)
        {
            AdaptiveCard card = AdaptiveCardFactory.CreateAdaptiveCard(PathFactory.CreateAdaptiveCardsPath("DailyRoutineCard.json"));

            if (string.IsNullOrEmpty(luisEntity))
            {
                DateTime today = DateTime.Today;

                foreach (DailyRoutine dailyRoutine in _dailyRoutines)
                {
                    if ((DateTime.Compare(dailyRoutine.Duration[0], today) <= 0) &&
                        (DateTime.Compare(dailyRoutine.Duration[1], today) >= 0))
                    {
                        card = PopulateCard(card, dailyRoutine);
                    }
                }

                return AdaptiveCardFactory.CreateAdaptiveCardAttachment(card);
            }
            else
            {
                foreach (DailyRoutine dailyRoutine in _dailyRoutines)
                {
                    if (dailyRoutine.Name == luisEntity)
                    {
                        card = PopulateCard(card, dailyRoutine);
                    }                   
                }

                return AdaptiveCardFactory.CreateAdaptiveCardAttachment(card);
            }
        }

        private static AdaptiveCard PopulateCard(AdaptiveCard card, DailyRoutine dailyRoutine)
        {
            (AdaptiveCardFactory.CreateAdaptiveElement(card, "name") as AdaptiveTextBlock).Text = dailyRoutine.Name;
            (AdaptiveCardFactory.CreateAdaptiveElement(card, "duration") as AdaptiveTextBlock).Text = dailyRoutine.Duration[0].ToString("MMMM dd") + " - " + dailyRoutine.Duration[1].ToString("MMMM dd");
            (AdaptiveCardFactory.CreateAdaptiveElement(card, "facts") as AdaptiveFactSet).Facts = dailyRoutine.Facts;
            return card;
        }

        public static readonly List<string> SikhMonths = new List<string>
        {
            "Chet",
            "Vaisakh",
            "Jeth",
            "Harh",
            "Sawan",
            "Bhadon",
            "Assu",
            "Kattak",
            "Maggar",
            "Poh",
            "Magh",
            "Phaggan",
        };

        private static readonly List<DailyRoutine> _dailyRoutines = new List<DailyRoutine>
        {
            new DailyRoutine
            {
                Name = "Chet",
                Duration = new List<DateTime>
                {
                    new DateTime(DateTime.Now.Year, 3, 14),
                    new DateTime(DateTime.Now.Year, 4, 13),
                },
                Facts = new List<AdaptiveFact>
                {
                    new AdaptiveFact("Kiwad (Portals) Opening Time:", "2:30 AM"),
                    new AdaptiveFact("Kirtan:", "2:30 AM"),
                    new AdaptiveFact("Asa Di Var:", "3:30 AM"),
                    new AdaptiveFact("Departure of Palki Sahib from Sri Akal Takhat Sahib:", "4:30 AM"),
                    new AdaptiveFact("First Hukamnama:", "5:00 AM"),
                    new AdaptiveFact("First Ardas:", "5:30 AM"),
                    new AdaptiveFact("Asa Di War Samapti:", "6:30 AM"),
                    new AdaptiveFact("Second Ardas and Hukamnama:", "6:45 AM"),
                    new AdaptiveFact("Hukamnama at Night:", "9:45 PM"),
                    new AdaptiveFact("Departure of Palki Sahib fromSri Harimandir Sahib:", "10:00 PM"),
                    new AdaptiveFact("Sukh-Aasan Sahib at Sri Akal Takhat Sahib:", "10:15 PM")
                }
            },
            new DailyRoutine
            {
                Name = "Vaisakh",
                Duration = new List<DateTime>
                {
                    new DateTime(DateTime.Now.Year, 4, 14),
                    new DateTime(DateTime.Now.Year, 5, 13),
                },
                Facts = new List<AdaptiveFact>
                {
                    new AdaptiveFact("Kiwad (Portals) Opening Time:", "2:15 AM"),
                    new AdaptiveFact("Kirtan:", "2:15 AM"),
                    new AdaptiveFact("Asa Di Var:", "3:15 AM"),
                    new AdaptiveFact("Departure of Palki Sahib from Sri Akal Takhat Sahib:", "4:15 AM"),
                    new AdaptiveFact("First Hukamnama:", "4:45 AM"),
                    new AdaptiveFact("First Ardas:", "5:15 AM"),
                    new AdaptiveFact("Asa Di War Samapti:", "6:15 AM"),
                    new AdaptiveFact("Second Ardas and Hukamnama:", "6:30 AM"),
                    new AdaptiveFact("Hukamnama at Night:", "10:15 PM"),
                    new AdaptiveFact("Departure of Palki Sahib fromSri Harimandir Sahib:", "10:30 PM"),
                    new AdaptiveFact("Sukh-Aasan Sahib at Sri Akal Takhat Sahib:", "10:45 PM")
                }
            },
            new DailyRoutine
            {
                Name = "Jeth",
                Duration = new List<DateTime>
                {
                    new DateTime(DateTime.Now.Year, 5, 14),
                    new DateTime(DateTime.Now.Year, 6, 14),
                },
                Facts = new List<AdaptiveFact>
                {
                    new AdaptiveFact("Kiwad (Portals) Opening Time:", "2:00 AM"),
                    new AdaptiveFact("Kirtan:", "2:00 AM"),
                    new AdaptiveFact("Asa Di Var:", "3:00 AM"),
                    new AdaptiveFact("Departure of Palki Sahib from Sri Akal Takhat Sahib:", "4:00 AM"),
                    new AdaptiveFact("First Hukamnama:", "4:30 AM"),
                    new AdaptiveFact("First Ardas:", "5:00 AM"),
                    new AdaptiveFact("Asa Di War Samapti:", "6:00 AM"),
                    new AdaptiveFact("Second Ardas and Hukamnama:", "6:15 AM"),
                    new AdaptiveFact("Hukamnama at Night:", "10:30 PM"),
                    new AdaptiveFact("Departure of Palki Sahib fromSri Harimandir Sahib:", "10:45 PM"),
                    new AdaptiveFact("Sukh-Aasan Sahib at Sri Akal Takhat Sahib:", "11:00 PM")
                }
            },
            new DailyRoutine
            {
                Name = "Harh",
                Duration = new List<DateTime>
                {
                    new DateTime(DateTime.Now.Year, 6, 15),
                    new DateTime(DateTime.Now.Year, 7, 15),
                },
                Facts = new List<AdaptiveFact>
                {
                    new AdaptiveFact("Kiwad (Portals) Opening Time:", "2:00 AM"),
                    new AdaptiveFact("Kirtan:", "2:00 AM"),
                    new AdaptiveFact("Asa Di Var:", "3:00 AM"),
                    new AdaptiveFact("Departure of Palki Sahib from Sri Akal Takhat Sahib:", "4:00 AM"),
                    new AdaptiveFact("First Hukamnama:", "4:30 AM"),
                    new AdaptiveFact("First Ardas:", "5:00 AM"),
                    new AdaptiveFact("Asa Di War Samapti:", "6:00 AM"),
                    new AdaptiveFact("Second Ardas and Hukamnama:", "6:15 AM"),
                    new AdaptiveFact("Hukamnama at Night:", "10:30 PM"),
                    new AdaptiveFact("Departure of Palki Sahib fromSri Harimandir Sahib:", "10:45 PM"),
                    new AdaptiveFact("Sukh-Aasan Sahib at Sri Akal Takhat Sahib:", "11:00 PM")
                }
            },
            new DailyRoutine
            {
                Name = "Sawan",
                Duration = new List<DateTime>
                {
                    new DateTime(DateTime.Now.Year, 7, 16),
                    new DateTime(DateTime.Now.Year, 8, 15),
                },
                Facts = new List<AdaptiveFact>
                {
                    new AdaptiveFact("Kiwad (Portals) Opening Time:", "2:15 AM"),
                    new AdaptiveFact("Kirtan:", "2:15 AM"),
                    new AdaptiveFact("Asa Di Var:", "3:15 AM"),
                    new AdaptiveFact("Departure of Palki Sahib from Sri Akal Takhat Sahib:", "4:15 AM"),
                    new AdaptiveFact("First Hukamnama:", "4:45 AM"),
                    new AdaptiveFact("First Ardas:", "5:15 AM"),
                    new AdaptiveFact("Asa Di War Samapti:", "6:15 AM"),
                    new AdaptiveFact("Second Ardas and Hukamnama:", "6:30 AM"),
                    new AdaptiveFact("Hukamnama at Night:", "10:15 PM"),
                    new AdaptiveFact("Departure of Palki Sahib fromSri Harimandir Sahib:", "10:30 PM"),
                    new AdaptiveFact("Sukh-Aasan Sahib at Sri Akal Takhat Sahib:", "10:45 PM")
                }
            },
            new DailyRoutine
            {
                Name = "Sawan",
                Duration = new List<DateTime>
                {
                    new DateTime(DateTime.Now.Year, 8, 16),
                    new DateTime(DateTime.Now.Year, 9, 15),
                },
                Facts = new List<AdaptiveFact>
                {
                    new AdaptiveFact("Kiwad (Portals) Opening Time:", "2:30 AM"),
                    new AdaptiveFact("Kirtan:", "2:30 AM"),
                    new AdaptiveFact("Asa Di Var:", "3:30 AM"),
                    new AdaptiveFact("Departure of Palki Sahib from Sri Akal Takhat Sahib:", "4:30 AM"),
                    new AdaptiveFact("First Hukamnama:", "5:00 AM"),
                    new AdaptiveFact("First Ardas:", "5:30 AM"),
                    new AdaptiveFact("Asa Di War Samapti:", "6:30 AM"),
                    new AdaptiveFact("Second Ardas and Hukamnama:", "6:45 AM"),
                    new AdaptiveFact("Hukamnama at Night:", "10:15 PM"),
                    new AdaptiveFact("Departure of Palki Sahib fromSri Harimandir Sahib:", "10:30 PM"),
                    new AdaptiveFact("Sukh-Aasan Sahib at Sri Akal Takhat Sahib:", "10:45 PM")
                }
            },
            new DailyRoutine
            {
                Name = "Assu",
                Duration = new List<DateTime>
                {
                    new DateTime(DateTime.Now.Year, 9, 16),
                    new DateTime(DateTime.Now.Year, 10, 16),
                },
                Facts = new List<AdaptiveFact>
                {
                    new AdaptiveFact("Kiwad (Portals) Opening Time:", "2:30 AM"),
                    new AdaptiveFact("Kirtan:", "2:30 AM"),
                    new AdaptiveFact("Asa Di Var:", "3:30 AM"),
                    new AdaptiveFact("Departure of Palki Sahib from Sri Akal Takhat Sahib:", "4:30 AM"),
                    new AdaptiveFact("First Hukamnama:", "5:00 AM"),
                    new AdaptiveFact("First Ardas:", "5:30 AM"),
                    new AdaptiveFact("Asa Di War Samapti:", "6:30 AM"),
                    new AdaptiveFact("Second Ardas and Hukamnama:", "6:45 AM"),
                    new AdaptiveFact("Hukamnama at Night:", "10:00 PM"),
                    new AdaptiveFact("Departure of Palki Sahib fromSri Harimandir Sahib:", "10:15 PM"),
                    new AdaptiveFact("Sukh-Aasan Sahib at Sri Akal Takhat Sahib:", "10:30 PM")
                }
            },
            new DailyRoutine
            {
                Name = "Kattak",
                Duration = new List<DateTime>
                {
                    new DateTime(DateTime.Now.Year, 10, 17),
                    new DateTime(DateTime.Now.Year, 11, 15),
                },
                Facts = new List<AdaptiveFact>
                {
                    new AdaptiveFact("Kiwad (Portals) Opening Time:", "3:00 AM"),
                    new AdaptiveFact("Kirtan:", "3:00 AM"),
                    new AdaptiveFact("Asa Di Var:", "4:00 AM"),
                    new AdaptiveFact("Departure of Palki Sahib from Sri Akal Takhat Sahib:", "5:00 AM"),
                    new AdaptiveFact("First Hukamnama:", "5:30 AM"),
                    new AdaptiveFact("First Ardas:", "6:00 AM"),
                    new AdaptiveFact("Asa Di War Samapti:", "7:00 AM"),
                    new AdaptiveFact("Second Ardas and Hukamnama:", "7:15 AM"),
                    new AdaptiveFact("Hukamnama at Night:", "9:30 PM"),
                    new AdaptiveFact("Departure of Palki Sahib fromSri Harimandir Sahib:", "9:45 PM"),
                    new AdaptiveFact("Sukh-Aasan Sahib at Sri Akal Takhat Sahib:", "10:00 PM")
                }
            },
            new DailyRoutine
            {
                Name = "Maggar",
                Duration = new List<DateTime>
                {
                    new DateTime(DateTime.Now.Year, 11, 16),
                    new DateTime(DateTime.Now.Year, 12, 15),
                },
                Facts = new List<AdaptiveFact>
                {
                    new AdaptiveFact("Kiwad (Portals) Opening Time:", "3:00 AM"),
                    new AdaptiveFact("Kirtan:", "3:00 AM"),
                    new AdaptiveFact("Asa Di Var:", "4:00 AM"),
                    new AdaptiveFact("Departure of Palki Sahib from Sri Akal Takhat Sahib:", "5:00 AM"),
                    new AdaptiveFact("First Hukamnama:", "5:30 AM"),
                    new AdaptiveFact("First Ardas:", "6:00 AM"),
                    new AdaptiveFact("Asa Di War Samapti:", "7:00 AM"),
                    new AdaptiveFact("Second Ardas and Hukamnama:", "7:15 AM"),
                    new AdaptiveFact("Hukamnama at Night:", "9:30 PM"),
                    new AdaptiveFact("Departure of Palki Sahib fromSri Harimandir Sahib:", "9:45 PM"),
                    new AdaptiveFact("Sukh-Aasan Sahib at Sri Akal Takhat Sahib:", "10:00 PM")
                }
            },
            new DailyRoutine
            {
                Name = "Poh",
                Duration = new List<DateTime>
                {
                    new DateTime(DateTime.Now.Year, 12, 16),
                    new DateTime(DateTime.Now.Year, 1, 13),
                },
                Facts = new List<AdaptiveFact>
                {
                    new AdaptiveFact("Kiwad (Portals) Opening Time:", "3:00 AM"),
                    new AdaptiveFact("Kirtan:", "3:00 AM"),
                    new AdaptiveFact("Asa Di Var:", "4:00 AM"),
                    new AdaptiveFact("Departure of Palki Sahib from Sri Akal Takhat Sahib:", "5:00 AM"),
                    new AdaptiveFact("First Hukamnama:", "5:30 AM"),
                    new AdaptiveFact("First Ardas:", "6:00 AM"),
                    new AdaptiveFact("Asa Di War Samapti:", "7:00 AM"),
                    new AdaptiveFact("Second Ardas and Hukamnama:", "7:15 AM"),
                    new AdaptiveFact("Hukamnama at Night:", "9:30 PM"),
                    new AdaptiveFact("Departure of Palki Sahib fromSri Harimandir Sahib:", "9:45 PM"),
                    new AdaptiveFact("Sukh-Aasan Sahib at Sri Akal Takhat Sahib:", "10:00 PM")
                }
            },
            new DailyRoutine
            {
                Name = "Magh",
                Duration = new List<DateTime>
                {
                    new DateTime(DateTime.Now.Year, 1, 14),
                    new DateTime(DateTime.Now.Year, 2, 12),
                },
                Facts = new List<AdaptiveFact>
                {
                    new AdaptiveFact("Kiwad (Portals) Opening Time:", "3:00 AM"),
                    new AdaptiveFact("Kirtan:", "3:00 AM"),
                    new AdaptiveFact("Asa Di Var:", "4:00 AM"),
                    new AdaptiveFact("Departure of Palki Sahib from Sri Akal Takhat Sahib:", "5:00 AM"),
                    new AdaptiveFact("First Hukamnama:", "5:30 AM"),
                    new AdaptiveFact("First Ardas:", "6:00 AM"),
                    new AdaptiveFact("Asa Di War Samapti:", "7:00 AM"),
                    new AdaptiveFact("Second Ardas and Hukamnama:", "7:15 AM"),
                    new AdaptiveFact("Hukamnama at Night:", "9:30 PM"),
                    new AdaptiveFact("Departure of Palki Sahib fromSri Harimandir Sahib:", "9:45 PM"),
                    new AdaptiveFact("Sukh-Aasan Sahib at Sri Akal Takhat Sahib:", "10:00 PM")
                }
            },
            new DailyRoutine
            {
                Name = "Phaggan",
                Duration = new List<DateTime>
                {
                    new DateTime(DateTime.Now.Year, 2, 13),
                    new DateTime(DateTime.Now.Year, 3, 14),
                },
                Facts = new List<AdaptiveFact>
                {
                    new AdaptiveFact("Kiwad (Portals) Opening Time:", "2:45 AM"),
                    new AdaptiveFact("Kirtan:", "2:45 AM"),
                    new AdaptiveFact("Asa Di Var:", "3:45 AM"),
                    new AdaptiveFact("Departure of Palki Sahib from Sri Akal Takhat Sahib:", "4:45 AM"),
                    new AdaptiveFact("First Hukamnama:", "5:15 AM"),
                    new AdaptiveFact("First Ardas:", "5:45 AM"),
                    new AdaptiveFact("Asa Di War Samapti:", "6:45 AM"),
                    new AdaptiveFact("Second Ardas and Hukamnama:", "7:00 AM"),
                    new AdaptiveFact("Hukamnama at Night:", "9:45 PM"),
                    new AdaptiveFact("Departure of Palki Sahib fromSri Harimandir Sahib:", "10:00 PM"),
                    new AdaptiveFact("Sukh-Aasan Sahib at Sri Akal Takhat Sahib:", "10:15 PM")
                }
            },
        };
    }
}
