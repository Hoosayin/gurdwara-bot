using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Helpers
{
    public class EntityResolutionFactory
    {
        public static string ResolveEntity(string name)
        {
            if (name != null)
            {
                string entity = ExtractEntity(_sikhFestivals, name);
                if (!string.IsNullOrEmpty(entity))
                {
                    return entity;
                }

                entity = ExtractEntity(_routineTasks, name);
                if (!string.IsNullOrEmpty(entity))
                {
                    return entity;
                }

                entity = ExtractEntity(_sikhMonths, name);
                if (!string.IsNullOrEmpty(entity))
                {
                    return entity;
                }

                entity = ExtractEntity(_days, name);
                if (!string.IsNullOrEmpty(entity))
                {
                    return entity;
                }
            }

            return string.Empty;
        }

        private static string ExtractEntity(Dictionary<string, List<string>> collection, string name)
        {
            foreach (KeyValuePair<string, List<string>> item in collection)
            {
                if (item.Value.Contains(name))
                {
                    return item.Key;
                }
            }
            return string.Empty;
        }

        private static readonly Dictionary<string, List<string>> _sikhFestivals = new Dictionary<string, List<string>>
        {
            {
                "Vaisakhi",
                new List<string>
                {
                    "baisakhi",
                    "vaishakhi",
                    "vasakhi",
                    "vaisakhi",
                }
            },
            {
                "Gurpurab",
                new List<string>
                {
                    "guru nanak gurpurab",
                    "guru nanak's birth anniversary",
                    "parkash guru nanak dev ji",
                    "guru nanak's prakash utsav",
                    "guru nanak jayanti",
                    "birthday of guru nanak",
                    "gurpurab",
                }
            },
            {
                "Maghi",
                new List<string>
                {
                    "mela maghi",
                    "maghi",
                }
            },
            {
                "Martyrdom of Guru Arjan Dev",
                new List<string>
                {
                    "martyrdom of guru arjan",
                    "martyrdom of arjan dev",
                    "martyrdom of guru arjan dev",
                }
            },
            {
                "Bandhi Chhor Divas",
                new List<string>
                {
                    "diwali",
                    "day of liberation",
                    "bandhi chhor divas",
                }
            },
            {
                "Hola Mohalla",
                new List<string>
                {
                    "hola",
                    "hola mohalla",
                }
            },
            {
                "Parkash Ustav Dasveh Patshah",
                new List<string>
                {
                    "guru gobind singh jayanti",
                    "parkash ustav dasveh patshah",
                }
            },
        };

        private static readonly Dictionary<string, List<string>> _routineTasks = new Dictionary<string, List<string>>
        {
            {
                "Kiwad",
                new List<string>
                {
                    "kiwad",
                }
            },
            {
                "kirtan",
                new List<string>
                {
                    "kirtan",
                }
            },
            {
                "Asa di var",
                new List<string>
                {
                    "asa di var",
                }
            },
            {
                "departure of palki sahib",
                new List<string>
                {
                    "departure of palki sahib",
                }
            },
            {
                "hukamnama",
                new List<string>
                {
                    "hukamnama",
                }
            },
            {
                "ardas",
                new List<string>
                {
                    "ardas",
                }
            },
            {
                "sukh-asan Sahib",
                new List<string>
                {
                    "sukh-asan Sahib",
                }
            },
            {
                "Asa di war",
                new List<string>
                {
                    "asa di war",
                }
            },
            {
                "Gurbani",
                new List<string>
                {
                    "gurbani",
                    "hyms",
                }
            },
            {
                "Rehras",
                new List<string>
                {
                    "rehras",
                    "Evening Scripture",
                }
            },
        };

        private static readonly Dictionary<string, List<string>> _sikhMonths = new Dictionary<string, List<string>>
        {
            {
                "Chet",
                new List<string>
                {
                    "chet",
                }
            },
            {
                "Vaisakh",
                new List<string>
                {
                    "vaisakh",
                }
            },
            {
                "Jeth",
                new List<string>
                {
                    "jeth",
                }
            },
            {
                "Harh",
                new List<string>
                {
                    "harh",
                }
            },
            {
                "Sawan",
                new List<string>
                {
                    "sawan",
                }
            },
            {
                "Bhadon",
                new List<string>
                {
                    "bhadon",
                }
            },
            {
                "Assu",
                new List<string>
                {
                    "assu",
                }
            },
            {
                "Kattak",
                new List<string>
                {
                    "kattak",
                }
            },
            {
                "Maggar",
                new List<string>
                {
                    "maggar",
                }
            },
            {
                "Poh",
                new List<string>
                {
                    "poh",
                    "Evening Scripture",
                }
            },
            {
                "Magh",
                new List<string>
                {
                    "magh",
                }
            },
            {
                "Phaggan",
                new List<string>
                {
                    "phaggan",
                }
            },
        };

        private static readonly Dictionary<string, List<string>> _days = new Dictionary<string, List<string>>
        {
            {
                "Monday",
                new List<string>
                {
                    "mon",
                    "mondays",
                    "monday",
                }
            },
            {
                "Tuesday",
                new List<string>
                {
                    "tue",
                    "tuesdays",
                    "tuesday",
                }
            },
            {
                "Wednesday",
                new List<string>
                {
                    "wed",
                    "wednesdays",
                    "wednesday",
                }
            },
            {
                "Thursday",
                new List<string>
                {
                    "thurs",
                    "thursdays",
                    "thursday",
                }
            },
            {
                "Friday",
                new List<string>
                {
                    "fri",
                    "fridays",
                    "friday",
                }
            },
            {
                "Saturday",
                new List<string>
                {
                    "sat",
                    "saturdays",
                    "saturday",
                }
            },
            {
                "Sunday",
                new List<string>
                {
                    "sun",
                    "sundays",
                    "sunday",
                }
            },
            {
                "Tomorrow",
                new List<string>
                {
                    "tomorrow",
                    "coming day",
                }
            },
        };
    }
}
