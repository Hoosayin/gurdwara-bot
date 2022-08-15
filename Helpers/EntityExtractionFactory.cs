using Microsoft.Bot.Builder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Helpers
{
    public class EntityExtractionFactory
    {
        public static string CreateEntity(RecognizerResult recognizerResult)
        {
            string result = string.Empty;

            foreach (KeyValuePair<string, JToken> entity in recognizerResult.Entities)
            {
                JToken routineTaskFound = JObject.Parse(entity.Value.ToString())["RoutineTask"];
                JToken sikhFestivalFound = JObject.Parse(entity.Value.ToString())["SikhFestival"];
                JToken sikhMonthFound = JObject.Parse(entity.Value.ToString())["SikhMonth"];
                JToken videoFound = JObject.Parse(entity.Value.ToString())["Video"];
                JToken imageFound = JObject.Parse(entity.Value.ToString())["Image"];
                JToken dayFound = JObject.Parse(entity.Value.ToString())["Day"];

                if (routineTaskFound != null)
                {
                    dynamic o = JsonConvert.DeserializeObject<dynamic>(entity.Value.ToString());

                    if (o.RoutineTask[0] != null)
                    {
                        return EntityResolutionFactory.ResolveEntity(o.RoutineTask[0].text.ToString());
                    }
                }

                if (sikhFestivalFound != null)
                {
                    dynamic o = JsonConvert.DeserializeObject<dynamic>(entity.Value.ToString());

                    if (o.SikhFestival[0] != null)
                    {
                        return EntityResolutionFactory.ResolveEntity(o.SikhFestival[0].text.ToString());
                    }
                }

                if (sikhMonthFound != null)
                {
                    dynamic o = JsonConvert.DeserializeObject<dynamic>(entity.Value.ToString());

                    if (o.SikhMonth[0] != null)
                    {
                        return EntityResolutionFactory.ResolveEntity(o.SikhMonth[0].text.ToString());
                    }
                }

                if (videoFound != null)
                {
                    dynamic o = JsonConvert.DeserializeObject<dynamic>(entity.Value.ToString());

                    if (o.Video[0] != null)
                    {
                        return o.Video[0].type.ToString();
                    }
                }

                if (imageFound != null)
                {
                    dynamic o = JsonConvert.DeserializeObject<dynamic>(entity.Value.ToString());

                    if (o.Image[0] != null)
                    {
                        return o.Image[0].type.ToString();
                    }
                }

                if (dayFound != null)
                {
                    dynamic o = JsonConvert.DeserializeObject<dynamic>(entity.Value.ToString());

                    if (o.Day[0] != null)
                    {
                        string ent = EntityResolutionFactory.ResolveEntity(o.Day[0].text.ToString());
                        return EntityResolutionFactory.ResolveEntity(o.Day[0].text.ToString());
                    }
                }
            }

            return result;
        }
    }
}
