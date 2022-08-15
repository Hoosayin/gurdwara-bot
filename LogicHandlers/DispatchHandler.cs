using GurdwaraBot.Helpers;
using GurdwaraBot.Models;
using GurdwaraBot.Services;
using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GurdwaraBot.LogicHandlers
{
    public class DispatchHandler
    {
        private static readonly string GurdwaraQnaKey = "GurdwaraQnA";
        private static readonly string ChitChatQnaKey = "ChitChatQnA";
        private static readonly string GurdwaraLuisKey = "GurdwaraLUIS";
        private static readonly string GurdwaraDispatchKey = "GurdwaraDispatch";
        private static Random _random = new Random();

        public static async Task DispatchTurnAsync(ITurnContext turnContext, BotServices services, CancellationToken cancellationToken = default)
        {
            if (!services.QnAServices.ContainsKey(GurdwaraQnaKey))
            {
                throw new ArgumentException($"Invalid configuration. Please check your '.bot' file for a QnA service named '{GurdwaraQnaKey}'.");
            }

            if (!services.QnAServices.ContainsKey(ChitChatQnaKey))
            {
                throw new ArgumentException($"Invalid configuration. Please check your '.bot' file for a QnA service named '{ChitChatQnaKey}'.");
            }

            if (!services.LuisServices.ContainsKey(GurdwaraLuisKey))
            {
                throw new ArgumentException($"Invalid configuration. Please check your '.bot' file for a Luis service named '{GurdwaraLuisKey}'.");
            }

            RecognizerResult recognizerResult = await services.LuisServices[GurdwaraDispatchKey].RecognizeAsync(turnContext, cancellationToken);
            (string intent, double score)? topIntent = recognizerResult?.GetTopScoringIntent();

            if (topIntent == null)
            {
                await turnContext.Activity.CreateDelayAsync(turnContext, cancellationToken);
                await turnContext.SendActivityAsync("Sorry, I can't understand anything you said. Can you rephrase it please?.");
            }
            else
            {
                await DispatchToTopIntentAsync(turnContext, topIntent, services, cancellationToken);
            }
        }

        private static async Task DispatchToTopIntentAsync(ITurnContext turnContext, (string intent, double score)? topIntent, BotServices services, CancellationToken cancellationToken = default)
        {
            switch (topIntent.Value.intent)
            {
                case "ChitChatIntent":
                    int randomOption = _random.Next(1, 3);
                    if (randomOption == 1)
                    {
                        await QnAHandler.QnATurnAsync(turnContext, GurdwaraQnaKey, services, cancellationToken);
                    }
                    else
                    {
                        await QnAHandler.QnATurnAsync(turnContext, ChitChatQnaKey, services, cancellationToken);
                    }
                    break;
                case "GurdwaraQnAIntent":
                    await QnAHandler.QnATurnAsync(turnContext, GurdwaraQnaKey, services, cancellationToken);
                    break;
                case "GurdwaraLuisIntent":
                    await LuisHandler.LuisTurnAsync(turnContext, GurdwaraLuisKey, services, cancellationToken);
                    break;
                case "None":
                    break;
                default:
                    if (turnContext.Activity.Text.Length > 10)
                    {
                        await CosmosDBFactory.InsertUnknownQuestionAsync(turnContext.Activity.Text);
                    }
                    
                    string message = "Hmm, that's something I don't know.";
                    await turnContext.Activity.CreateDelayAsync(turnContext, cancellationToken);
                    await turnContext.SendActivityAsync(message, cancellationToken: cancellationToken);
                    break;
            }
        }
    }
}
