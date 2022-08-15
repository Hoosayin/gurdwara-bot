using Microsoft.Bot.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Helpers
{
    public class ServiceFactory
    {
        public static List<ConnectedService> GetServices(IConfiguration configuration)
        {
            List<ConnectedService> connectedServices = new List<ConnectedService>
            {
                new DispatchService
                {
                    Id = configuration.GetSection("DispatchId")?.Value,
                    Name = configuration.GetSection("DispatchName")?.Value,
                    Type = configuration.GetSection("DispatchType")?.Value,
                    AppId = configuration.GetSection("DispatchAppId")?.Value,
                    AuthoringKey = configuration.GetSection("DispatchAuthoringKey")?.Value,
                    Region = configuration.GetSection("DispatchRegion")?.Value,
                    SubscriptionKey = configuration.GetSection("DispatchSubscriptionKey")?.Value,
                    Version = configuration.GetSection("DispatchVersion")?.Value,
                },
                new LuisService
                {
                    Id = configuration.GetSection("GurdwaraLUISId")?.Value,
                    Name = configuration.GetSection("GurdwaraLUISName")?.Value,
                    Type = configuration.GetSection("GurdwaraLUISType")?.Value,
                    AppId = configuration.GetSection("GurdwaraLUISAppId")?.Value,
                    AuthoringKey = configuration.GetSection("GurdwaraLUISAuthoringKey")?.Value,
                    Region = configuration.GetSection("GurdwaraLUISRegion")?.Value,
                    SubscriptionKey = configuration.GetSection("GurdwaraLUISSubscriptionKey")?.Value,
                    Version = configuration.GetSection("GurdwaraLUISVersion")?.Value,
                },
                new QnAMakerService
                {
                    Id = configuration.GetSection("GurdwaraQnAId")?.Value,
                    Name = configuration.GetSection("GurdwaraQnAName")?.Value,
                    Type = configuration.GetSection("GurdwaraQnAType")?.Value,
                    EndpointKey = configuration.GetSection("GurdwaraQnAEndpointKey")?.Value,
                    Hostname = configuration.GetSection("GurdwaraQnAHostname")?.Value,
                    KbId = configuration.GetSection("GurdwaraQnAKbid")?.Value,
                    SubscriptionKey = configuration.GetSection("GurdwaraQnASubscriptionKey")?.Value,
                },
                new QnAMakerService
                {
                    Id = configuration.GetSection("ChitChatQnAId")?.Value,
                    Name = configuration.GetSection("ChitChatQnAName")?.Value,
                    Type = configuration.GetSection("ChitChatQnAType")?.Value,
                    EndpointKey = configuration.GetSection("ChitChatQnAEndpointKey")?.Value,
                    Hostname = configuration.GetSection("ChitChatQnAHostname")?.Value,
                    KbId = configuration.GetSection("ChitChatQnAKbid")?.Value,
                    SubscriptionKey = configuration.GetSection("ChitChatQnASubscriptionKey")?.Value,
                }
            };

            return connectedServices;
        }
    }
}
