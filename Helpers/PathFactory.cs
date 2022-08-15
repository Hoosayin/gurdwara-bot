using GurdwaraBot.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Helpers
{
    public class PathFactory
    {
        public static string CreateAdaptiveCardsPath(string filename)
        {
            return Path.Combine(".", "wwwroot", "AdaptiveCards", $"{filename}");
        }

        public static string CreateImagePath(string filename)
        {
            return new Uri(new Uri($"https://{Environment.ExpandEnvironmentVariables("gurdwarainformationbot")}.azurewebsites.net"), 
                Path.Combine("images", $"{filename}")).ToString();
        }
    }
}
