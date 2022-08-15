using GurdwaraBot.Helpers;
using GurdwaraBot.Models;
using GurdwaraBot.StateManagement;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GurdwaraBot.Dialogs
{
    public class FeedbackDialog : ComponentDialog
    {
        public FeedbackDialog(BotAccessors accessors) : base(nameof(FeedbackDialog))
        {
            _accessors = accessors ?? throw new ArgumentNullException(nameof(accessors));

            WaterfallStep[] waterfallSteps = new WaterfallStep[]
            {
                InitializeStateStepAsync,
                FeedbackTypeStepAsync,
                CommentStepAsync,
                RatingStepAsync,
                SendFeedbackStepAsync,
            };

            AddDialog(new WaterfallDialog(WaterFallDialogId, waterfallSteps));
            AddDialog(new ChoicePrompt(FeedbackChoicePromptId) { Style = ListStyle.SuggestedAction, });
            AddDialog(new TextPrompt(CommentPromptId));

            TempData.FeedbackData = new FeedbackData();
        }

        private const string WaterFallDialogId = "WaterFallDialog";
        private const string CommentPromptId = "CommentPrompt";
        private const string FeedbackChoicePromptId = "FeedbackChoicePrompt";

        private BotAccessors _accessors;

        private async Task<DialogTurnResult> InitializeStateStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            FeedbackData feedbackData = await _accessors.FeedbackDataAccessor.GetAsync(stepContext.Context, () => null);
            if (feedbackData == null)
            {
                FeedbackData feedbackDataOutput = stepContext.Options as FeedbackData;
                if (feedbackDataOutput != null)
                {
                    await _accessors.FeedbackDataAccessor.SetAsync(stepContext.Context, feedbackDataOutput);
                }
                else
                {
                    await _accessors.FeedbackDataAccessor.SetAsync(stepContext.Context, new FeedbackData());
                }
            }

            return await stepContext.NextAsync();
        }

        private async Task<DialogTurnResult> FeedbackTypeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await SetFeedbackDataAsync(stepContext.Context, cancellationToken);
            PromptOptions promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("What kind of feedback do you have?"),
                Choices = ChoiceFactory.ToChoices(new List<string> { "Suggest a New Feature", "Feedback on an Existing Feature", "Report a Problem" }),
            };

            await stepContext.Context.Activity.CreateDelayAsync(stepContext.Context, cancellationToken);
            return await stepContext.PromptAsync(FeedbackChoicePromptId, promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> CommentStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string feedbackType = (stepContext.Result as FoundChoice).Value;

            TempData.FeedbackData = await GetFeedbackDataAsync(stepContext.Context, cancellationToken);
            TempData.FeedbackData.FeedbackType = feedbackType;
            await SetFeedbackDataAsync(stepContext.Context, cancellationToken);

            string prompt = string.Empty;

            switch (feedbackType)
            {
                case "Suggest a New Feature":
                    prompt = "Cool! What’s your feature idea?";
                    break;
                case "Feedback on an Existing Feature":
                    prompt = "Great. Go ahead and enter your feedback now.";
                    break;
                case "Report a Problem":
                    prompt = "OK, Let us know what type of problem you are facing?";
                    break;
                default:
                    break;
            }

            PromptOptions promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text(prompt),
                RetryPrompt = MessageFactory.Text(prompt),
            };

            await stepContext.Context.Activity.CreateDelayAsync(stepContext.Context, cancellationToken);
            return await stepContext.PromptAsync(CommentPromptId, promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> RatingStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string comment = stepContext.Result.ToString();

            TempData.FeedbackData = await GetFeedbackDataAsync(stepContext.Context, cancellationToken);
            TempData.FeedbackData.Comment = comment;
            await SetFeedbackDataAsync(stepContext.Context, cancellationToken);

            await stepContext.Context.Activity.CreateDelayAsync(stepContext.Context, cancellationToken);
            await stepContext.Context.SendActivityAsync("Your comments have been saved. We'll send them to our Gurdwara Management. Soon, they'll review your comments.");

            PromptOptions promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Just one more question! How would you rate our services?"),
                Choices = ChoiceFactory.ToChoices(new List<string> { "Excellent", "Good", "Average", "Not Good" }),
            };

            await stepContext.Context.Activity.CreateDelayAsync(stepContext.Context, cancellationToken);
            return await stepContext.PromptAsync(FeedbackChoicePromptId, promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> SendFeedbackStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string rating = (stepContext.Result as FoundChoice).Value;

            TempData.FeedbackData = await GetFeedbackDataAsync(stepContext.Context, cancellationToken);
            TempData.FeedbackData.Rating = rating;
            TempData.FeedbackData.DataCollected = true;
            await SetFeedbackDataAsync(stepContext.Context, cancellationToken);

            EmailFactory.SendFeedbackMail(TempData.FeedbackData);
            await stepContext.Context.Activity.CreateDelayAsync(stepContext.Context, cancellationToken);
            await stepContext.Context.SendActivityAsync("Thank you for providing us feedback.");
            return await stepContext.EndDialogAsync(cancellationToken);
        }

        private async Task SetFeedbackDataAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            await _accessors.FeedbackDataAccessor.SetAsync(turnContext, TempData.FeedbackData, cancellationToken);
            await _accessors.UserState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        private async Task<FeedbackData> GetFeedbackDataAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            return await _accessors.FeedbackDataAccessor.GetAsync(turnContext, () => new FeedbackData(), cancellationToken);
        }
    }
}
