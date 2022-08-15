using GurdwaraBot.Helpers;
using GurdwaraBot.Models;
using GurdwaraBot.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.QnA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GurdwaraBot.LogicHandlers
{
    public class QnAHandler
    {
        public static async Task QnATurnAsync(ITurnContext turnContext, string appName, BotServices services, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrEmpty(turnContext.Activity.Text))
            {
                QueryResult[] results = await services.QnAServices[appName].GetAnswersAsync(turnContext);
                if (results.Any())
                {
                    await turnContext.Activity.CreateDelayAsync(turnContext, cancellationToken);
                    await turnContext.SendActivityAsync(results.First().Answer, cancellationToken: cancellationToken);
                }
                else
                {
                    if (turnContext.Activity.Text.Length > 10)
                    {
                        await CosmosDBFactory.InsertUnknownQuestionAsync(turnContext.Activity.Text);
                    }

                    string message = "Hmm, that's something I don't know.";
                    //string message = "I'm sorry, I can't answer to this right now. But, soon I’ll explore on this particular question.";
                    await turnContext.Activity.CreateDelayAsync(turnContext, cancellationToken);
                    await turnContext.SendActivityAsync(message, cancellationToken: cancellationToken);
                }
            }
        }
    }
}