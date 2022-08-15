using GurdwaraBot.Helpers;
using GurdwaraBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GurdwaraBot.LogicHandlers
{
    public class FeedbackHandler
    {
        public static async Task FeedbackTurnAsync(ITurnContext turnContext, ConversationData conversationData, CancellationToken cancellationToken = default)
        {
            Dictionary<string, object> feedbackResults = turnContext.Activity.ParseValue();

            if (Validator.HasMissingValues(feedbackResults.Values))
            {
                await turnContext.SendActivityAsync("You must fill all the fields.", cancellationToken: cancellationToken);
                return;
            }

            if (!Validator.IsValidName(feedbackResults["Name"].ToString()))
            {
                await turnContext.SendActivityAsync("Please enter a valid name.", cancellationToken: cancellationToken);
                return;
            }

            if (!Validator.IsValidEmail(feedbackResults["Email"].ToString()))
            {
                await turnContext.SendActivityAsync("Please enter a Email address.", cancellationToken: cancellationToken);
                return;
            }

            int.TryParse(feedbackResults["Rating"].ToString(), out int rating);

            UserFeedback userFeedback = new UserFeedback
            {
                Name = feedbackResults["Name"].ToString(),
                Email = feedbackResults["Email"].ToString(),
                Comments = feedbackResults["Comments"].ToString(),
                Rating = (FeedbackRating)rating,
            };

            //EmailFactory.SendFeedbackMail(_userFeedback);
            await turnContext.SendActivityAsync("Thank You for providing us feedback.", cancellationToken: cancellationToken);
            conversationData.DataId = DataId.Question;

            Activity reply = turnContext.Activity.CreateReply();

            reply.Attachments = new List<Attachment>()
            {
                MenuCardFactory.CreateMenuCardAttachment(),
            };

            await turnContext.SendActivityAsync(reply, cancellationToken);
        }
    }
}
