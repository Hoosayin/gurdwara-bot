using AdaptiveCards;
using GurdwaraBot.Models;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace GurdwaraBot.Helpers
{
    class WeatherCardFactory
    {
        public static Attachment CreateWeatherCardAttachment()
        {
            WeatherInformation weatherInformation = GetWeatherInformation();
            AdaptiveCard card = AdaptiveCardFactory.CreateAdaptiveCard(PathFactory.CreateAdaptiveCardsPath("WeatherCard.json"));

            (AdaptiveCardFactory.CreateAdaptiveElement(card, "Date") as AdaptiveTextBlock).Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            (AdaptiveCardFactory.CreateAdaptiveElement(card, "Condition") as AdaptiveTextBlock).Text = 
                System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(weatherInformation.Weather[0].Description.ToLower());
            AdaptiveColumnSet columnSet = AdaptiveCardFactory.CreateAdaptiveElement(card, "ColumnSet") as AdaptiveColumnSet;

            (columnSet.Columns[0].Items[0] as AdaptiveImage).Url = new Uri(PathFactory.CreateImagePath($"{weatherInformation.Weather[0].Icon}.png"));    
            (columnSet.Columns[1].Items[0] as AdaptiveTextBlock).Text = ((int)KelvinToFahrenheitConverter(weatherInformation.Main.Temp)).ToString();
            (columnSet.Columns[4].Items[0] as AdaptiveTextBlock).Text = ((int)KelvinToCelsiusConverter(weatherInformation.Main.Temp)).ToString();
            (columnSet.Columns[6].Items[0] as AdaptiveFactSet).Facts = new List<AdaptiveFact>
            {
                new AdaptiveFact
                {
                    Title = "Humidity:",
                    Value = $"{weatherInformation.Main.Humidity}%",
                },
                new AdaptiveFact
                {
                    Title = "Wind:",
                    Value = $"{weatherInformation.Wind.Speed} m/s",
                }
            };

            return AdaptiveCardFactory.CreateAdaptiveCardAttachment(card);
        }

        private static WeatherInformation GetWeatherInformation()
        {
            string appId = "640005c1e6645d5e05a1d0d1ac2d5c8a";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q=Hasan%20Abdal,PK&appid={appId}";
            WeatherInformation weatherInformation;

            try
            {
                using (WebClient webClient = new WebClient())
                {
                    string jsonWeatherPayload = webClient.DownloadString(url);
                    return weatherInformation = JsonConvert.DeserializeObject<WeatherInformation>(jsonWeatherPayload);
                }
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        private static double KelvinToFahrenheitConverter(double kelvinTemperature)
        {
            return kelvinTemperature * 9 / 5 - 459.67;
        }

        private static double KelvinToCelsiusConverter(double kelvinTemperature)
        {
            return kelvinTemperature - 273.15;
        }
    }
}
