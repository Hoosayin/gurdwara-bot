using GurdwaraBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.StateManagement
{
    public class BotAccessors
    {
        public BotAccessors(ConversationState conversationState, UserState userState)
        {
            ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
            UserState = userState ?? throw new ArgumentNullException(nameof(userState));
        }

        public const string ConversationDataKey = "ConversationDataKey";
        public const string UserDataKey = "UserDataKey";
        public const string DialogStateKey = "DialogStateKey";
        public const string FeedbackDataKey = "FeedbackDataKey";

        public IStatePropertyAccessor<ConversationData> ConversationDataAccessor { get; set; }
        public IStatePropertyAccessor<DialogState> DialogStateAccessor { get; set; }
        public IStatePropertyAccessor<UserData> UserDataAccessor { get; set; }
        public IStatePropertyAccessor<FeedbackData> FeedbackDataAccessor { get; set; }

        public ConversationState ConversationState { get; }
        public UserState UserState { get; }
    }
}
