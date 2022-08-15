using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveCards;
using GurdwaraBot.Dialogs;
using GurdwaraBot.Helpers;
using GurdwaraBot.LogicHandlers;
using GurdwaraBot.Models;
using GurdwaraBot.Services;
using GurdwaraBot.StateManagement;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;

namespace GurdwaraBot.Bots
{
    public class GurdwaraBot : ActivityHandler
    {                       
        public GurdwaraBot(BotServices services, BotAccessors accessors)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _accessors = accessors ?? throw new ArgumentNullException(nameof(accessors));

            _dialogSet = new DialogSet(_accessors.DialogStateAccessor);
            _dialogSet
                .Add(new RoomBookingDialog(_accessors))
                .Add(new FeedbackDialog(_accessors));
        }

        private readonly BotServices _services;
        private readonly BotAccessors _accessors;
        private DialogSet _dialogSet { get; set; }
        private ConversationData _conversationData = new ConversationData();

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.OnTurnAsync(turnContext, cancellationToken);
            await _accessors.ConversationDataAccessor.SetAsync(turnContext, _conversationData);
            await _accessors.ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            DialogContext dialogContext = await _dialogSet.CreateContextAsync(turnContext);

            _conversationData = await _accessors.ConversationDataAccessor.GetAsync(turnContext, () => new ConversationData(), cancellationToken);

            if (turnContext.Activity.Text == "user_said_yes_for_booking")
            {
                _conversationData.DataId = DataId.ConfirmedBooking;
            }
            else if (turnContext.Activity.Text == "user_said_no_for_booking")
            {
                _conversationData.DataId = DataId.Question;
                await SendMenuCardAsync(turnContext, cancellationToken);
            }

            if (turnContext.Activity.Text?.ToLower() == "menu")
            {
                if (dialogContext.ActiveDialog != null)
                {
                    if (dialogContext.ActiveDialog.Id == nameof(RoomBookingDialog))
                    {
                        await IsTurnInterruptedAsync(dialogContext, "RoomBooking");
                    }
                    else
                    {
                        await IsTurnInterruptedAsync(dialogContext, "Feedback");
                    }
                }

                _conversationData.DataId = DataId.Question;
                await SendMenuCardAsync(turnContext, cancellationToken);
            }
            else
            {
                if (turnContext.Activity.Value != null)
                {
                    _conversationData.DataId = turnContext.Activity.GetDataId();
                }

                Activity reply;

                switch (_conversationData.DataId)
                {
                    case DataId.Menu:
                        await turnContext.SendActivityAsync("Please select an option from the MENU.", cancellationToken: cancellationToken);
                        break;
                    case DataId.Overview:
                        reply = (turnContext as ITurnContext).Activity.CreateReply();

                        reply.Attachments = new List<Attachment>
                            {
                                AdaptiveCardFactory.CreateAdaptiveCardAttachment(
                                    AdaptiveCardFactory.CreateAdaptiveCard(PathFactory.CreateAdaptiveCardsPath("OverviewCard.json"))),
                            };

                        await turnContext.SendActivityAsync(reply, cancellationToken);
                        _conversationData.DataId = DataId.Question;
                        break;
                    case DataId.Question:
                        if (turnContext.Activity.Text != null)
                        {
                            await DispatchHandler.DispatchTurnAsync(turnContext, _services, cancellationToken);
                        }
                        else
                        {
                            await turnContext.Activity.CreateDelayAsync(turnContext, cancellationToken);
                            await turnContext.SendActivityAsync("I'm ready to answer; Ask me anything!", cancellationToken: cancellationToken);
                        }
                        break;
                    case DataId.Room:
                        reply = (turnContext as ITurnContext).Activity.CreateReply();
                        reply.Attachments = new List<Attachment>
                            {
                                AdaptiveCardFactory.CreateAdaptiveCardAttachment(AdaptiveCardFactory.CreateAdaptiveCard(
                                    PathFactory.CreateAdaptiveCardsPath("SaraiBookingInformationCard.json"))),
                            };

                        await turnContext.Activity.CreateDelayAsync(turnContext, cancellationToken);
                        await turnContext.SendActivityAsync(reply, cancellationToken);

                        reply = (turnContext as ITurnContext).Activity.CreateReply();

                        reply.Text = "I'll Help you schedule a Gudwara Getaway! Are you sure you want to book a room?\n\nThis cannot be undone.";

                        reply.SuggestedActions = new SuggestedActions()
                        {
                            Actions = new List<CardAction>()
                                {
                                    new CardAction()
                                    {
                                        Title = "Yes",
                                        Type = ActionTypes.PostBack,
                                        Value ="user_said_yes_for_booking"
                                    },
                                    new CardAction()
                                    {
                                        Title = "No",
                                        Type = ActionTypes.PostBack,
                                        Value ="user_said_no_for_booking"
                                    }
                                }
                        };

                        await turnContext.Activity.CreateDelayAsync(turnContext, cancellationToken);
                        await turnContext.SendActivityAsync(reply, cancellationToken);
                        break;
                    case DataId.Feedback:
                    case DataId.ConfirmedBooking:
                        DialogTurnResult dialogTurnResult = await dialogContext.ContinueDialogAsync();
                        switch (dialogTurnResult.Status)
                        {
                            case DialogTurnStatus.Empty:
                                switch (_conversationData.DataId)
                                {
                                    case DataId.ConfirmedBooking:
                                        await dialogContext.BeginDialogAsync(nameof(RoomBookingDialog));
                                        break;
                                    case DataId.Feedback:
                                        await dialogContext.BeginDialogAsync(nameof(FeedbackDialog));
                                        break;
                                    default:
                                        await dialogContext.Context.SendActivityAsync("I didn't understand what you just said to me.");
                                        break;
                                }
                                break;
                            case DialogTurnStatus.Cancelled:
                                break;
                            case DialogTurnStatus.Complete:
                                if (TempData.UserData != null && !TempData.UserData.DataCollected && !TempData.FeedbackData.DataCollected)
                                {
                                    await turnContext.Activity.CreateDelayAsync(turnContext, cancellationToken);
                                    await turnContext.SendActivityAsync("I apologize for this inconvenience. If you have any query, you can provide me feedback from the **MENU**.", cancellationToken: cancellationToken);

                                    reply = (turnContext as ITurnContext).Activity.CreateReply();
                                    reply.Text = "I've other options for you. These are some nearby restaurants. You can book for these restaurants from any of the travel portals or websites, or contact the restaurants directly.";
                                    reply.Attachments = GuestHouseCardFactory.CreateGuesHouseAttachments();
                                    reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                                    await turnContext.Activity.CreateDelayAsync(turnContext, cancellationToken);
                                    await turnContext.SendActivityAsync(reply, cancellationToken);

                                    _conversationData.DataId = DataId.Question;
                                }
                                if (TempData.UserData != null && TempData.UserData.DataCollected)
                                {
                                    TempData.UserData = await CosmosDBFactory.AssignGurdwaraRoomsAsync(TempData.UserData);
                                    TempData.UserData = await CosmosDBFactory.InsertGurdwaraUserAsync(TempData.UserData);

                                    if (TempData.UserData.RowKey != null)
                                    {
                                        if (await CosmosDBFactory.InsertUserRoomsAsync(TempData.UserData))
                                        {
                                            await turnContext.Activity.CreateDelayAsync(turnContext, cancellationToken);
                                            await turnContext.SendActivityAsync("Your meal preferences are recorded, and your booking request is confirmed.", cancellationToken: cancellationToken);

                                            reply = (turnContext as ITurnContext).Activity.CreateReply();
                                            reply.Text = $"Here is your request. Please note that it is valid till {TempData.UserData.ExpirationDate.ToLongDateString()}.";

                                            reply.Attachments = new List<Attachment>
                                                {
                                                    ReceiptCardFactory.CreateReceiptAttachment(TempData.UserData),
                                                };

                                            await turnContext.Activity.CreateDelayAsync(turnContext, cancellationToken);
                                            await turnContext.SendActivityAsync(reply, cancellationToken: cancellationToken);
                                        }
                                    }
                                }
                                _conversationData.DataId = DataId.Question;
                                await dialogContext.EndDialogAsync();
                                TempData.UserData = null;
                                TempData.FeedbackData = null;
                                break;
                            case DialogTurnStatus.Waiting:
                                break;
                            default:
                                await dialogContext.CancelAllDialogsAsync();
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }

            
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await SendWelcomeMessageAsync((turnContext as ITurnContext), cancellationToken);
                }
            }
        }

        private static async Task SendWelcomeMessageAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            Activity reply = turnContext.Activity.CreateReply();
            AdaptiveCard card = AdaptiveCardFactory.CreateAdaptiveCard(PathFactory.CreateAdaptiveCardsPath("WelcomeCard.json"));
            (AdaptiveCardFactory.CreateAdaptiveElement(card, "image") as AdaptiveImage).Url = new Uri(PathFactory.CreateImagePath("welcome_card_image.png"));

            reply.Attachments = new List<Attachment>()
            {
                AdaptiveCardFactory.CreateAdaptiveCardAttachment(card),
            };

            await turnContext.Activity.CreateDelayAsync(turnContext, cancellationToken);
            await turnContext.SendActivityAsync(reply, cancellationToken);
        }

        private static async Task SendMenuCardAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            Activity reply = turnContext.Activity.CreateReply();

            reply.Attachments = new List<Attachment>()
            {
                MenuCardFactory.CreateMenuCardAttachment(),
            };

            await turnContext.Activity.CreateDelayAsync(turnContext, cancellationToken);
            await turnContext.SendActivityAsync(reply, cancellationToken);
        }

        private async Task IsTurnInterruptedAsync(DialogContext dialogContext, string dialogName)
        {
            if (dialogName.Equals("RoomBooking"))
            {
                await dialogContext.CancelAllDialogsAsync();
                await dialogContext.Context.Activity.CreateDelayAsync(dialogContext.Context, cancellationToken: default);
                await dialogContext.Context.SendActivityAsync("I've cancelled the booking procudure.");
            }

            if (dialogName.Equals("Feedback"))
            {
                await dialogContext.CancelAllDialogsAsync();
                await dialogContext.Context.Activity.CreateDelayAsync(dialogContext.Context, cancellationToken: default);
                await dialogContext.Context.SendActivityAsync("I've discarded your feedback.");
            }
        }
    }
}
