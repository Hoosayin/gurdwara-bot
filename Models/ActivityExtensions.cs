using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GurdwaraBot.Models
{
    public static class ActivityExtensions
    {
        public static DataId GetDataId(this IMessageActivity activity)
        {
            Dictionary<string, object> submitActionResults = activity.ParseValue();

            switch (submitActionResults["dataId"]?.ToString())
            {
                case "overview":
                    return DataId.Overview;
                case "question":
                    return DataId.Question;
                case "room":
                    return DataId.Room;
                case "feedback":
                    return DataId.Feedback;
                case "feedback_data":
                    return DataId.FeedbackData;
                default:
                    return DataId.Menu;
            }
        }

        public static Dictionary<string, object> ParseValue(this IMessageActivity activity)
        {
            JObject jObject = activity.Value as JObject;
            return jObject.ToObject<Dictionary<string, object>>();
        }

        public static async Task CreateDelayAsync(this IMessageActivity activity, ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            Activity typingReply = (activity as Activity).CreateReply();
            typingReply.Type = ActivityTypes.Typing;
            typingReply.Text = null;
            await turnContext.SendActivityAsync(typingReply, cancellationToken);
            await Task.Delay(millisecondsDelay: 3000);
        }
    }
}
