using GurdwaraBot.Helpers;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Services
{
    public class BotServices
    {
        public BotServices(IConfiguration configuration)
        {
            List<ConnectedService> connectedServices = ServiceFactory.GetServices(configuration);
            foreach (ConnectedService service in connectedServices)
            {
                switch (service.Type)
                {
                    case ServiceTypes.Luis:
                        LuisService luis = service as LuisService;
                        LuisApplication gurdwaraLuis = new LuisApplication(luis.AppId, luis.AuthoringKey, luis.GetEndpoint());
                        LuisRecognizer gurdwaraLuisRecognizer = new LuisRecognizer(gurdwaraLuis);
                        LuisServices.Add(luis.Name, gurdwaraLuisRecognizer);
                        break;
                    case ServiceTypes.Dispatch:
                        DispatchService dispatch = service as DispatchService;
                        LuisApplication gurdwaraDispatch = new LuisApplication(dispatch.AppId, dispatch.AuthoringKey, dispatch.GetEndpoint());
                        LuisRecognizer gurdwaraDispatchRecognizer = new LuisRecognizer(gurdwaraDispatch);
                        LuisServices.Add(dispatch.Name, gurdwaraDispatchRecognizer);
                        break;
                    case ServiceTypes.QnA:
                        QnAMakerService qna = service as QnAMakerService;

                        QnAMakerEndpoint qnaEndpoint = new QnAMakerEndpoint()
                        {
                            KnowledgeBaseId = qna.KbId,
                            EndpointKey = qna.EndpointKey,
                            Host = qna.Hostname,
                        };

                        QnAMaker qnaMaker = new QnAMaker(qnaEndpoint);
                        QnAServices.Add(qna.Name, qnaMaker);
                        break;
                }
            }
        }

        public Dictionary<string, QnAMaker> QnAServices { get; } = new Dictionary<string, QnAMaker>();
        public Dictionary<string, LuisRecognizer> LuisServices { get; } = new Dictionary<string, LuisRecognizer>();
    }
}
