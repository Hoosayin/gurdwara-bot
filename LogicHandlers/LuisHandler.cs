using GurdwaraBot.Helpers;
using GurdwaraBot.Models;
using GurdwaraBot.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GurdwaraBot.LogicHandlers
{
    public class LuisHandler
    {
        public static async Task LuisTurnAsync(ITurnContext turnContext, string luisKey, BotServices services, CancellationToken cancellationToken = default)
        {
            RecognizerResult recognizerResult = await services.LuisServices[luisKey].RecognizeAsync(turnContext, cancellationToken);
            (string intent, double score)? topIntent = recognizerResult?.GetTopScoringIntent();
            if (topIntent != null && topIntent.HasValue && topIntent.Value.intent != "None")
            {
                string entityFound = string.Empty;
                Activity reply = turnContext.Activity.CreateReply();
                Attachment attachment = new Attachment();

                switch (topIntent.Value.intent)
                {
                    case "GetAddress":
                        reply.Text = "Here's the location of Gurdwara Sri Panja Sahib. For more information, press **Get Directions**.";

                        reply.Attachments = new List<Attachment>
                            {
                                AdaptiveCardFactory.CreateAdaptiveCardAttachment(
                                    AdaptiveCardFactory.CreateAdaptiveCard(PathFactory.CreateAdaptiveCardsPath("LocationCard.json"))),
                            };

                        await turnContext.Activity.CreateDelayAsync(turnContext, cancellationToken);
                        await turnContext.SendActivityAsync(reply, cancellationToken);
                        break;
                    case "GetFestivalInformation":
                        entityFound = EntityExtractionFactory.CreateEntity(recognizerResult);
                        reply.Text = "I hope this information answers your question.";

                        reply.Attachments = new List<Attachment>
                            {
                                FestivalCardFactory.CreateFestivalCardAttachment(entityFound),
                            };

                        await turnContext.Activity.CreateDelayAsync(turnContext, cancellationToken);
                        await turnContext.SendActivityAsync(reply, cancellationToken);
                        break;
                    case "GetRoutineInformation":
                        entityFound = EntityExtractionFactory.CreateEntity(recognizerResult);                       

                        if (string.IsNullOrEmpty(entityFound))
                        {
                            reply.Text = "Here's today's schedule at Sri Panja Sahib.";
                            attachment = DailyRoutineCardFactory.CreateRoutineCardAttachment(string.Empty);
                        }
                        else
                        {
                            if (DailyRoutineCardFactory.SikhMonths.Contains(entityFound))
                            {
                                reply.Text = $"Hmm... Let me see if I can retrieve schedule for **{entityFound}**.";
                                attachment = DailyRoutineCardFactory.CreateRoutineCardAttachment(entityFound);
                            }
                            else
                            {
                                reply.Text = $"Please see the below schedule to find timings for **{entityFound}**.";
                                attachment = DailyRoutineCardFactory.CreateRoutineCardAttachment(string.Empty);
                            }
                        }

                        reply.Attachments = new List<Attachment>
                            {
                                attachment,
                            };

                        await turnContext.Activity.CreateDelayAsync(turnContext, cancellationToken);
                        await turnContext.SendActivityAsync(reply, cancellationToken);
                        break;
                    case "GetWeatherCondition":
                        reply.Text = "Well, here's the weather condition currently at Hasan Abdal, Pakistan.";

                        reply.Attachments = new List<Attachment>
                        {
                                WeatherCardFactory.CreateWeatherCardAttachment(),
                        };

                        await turnContext.Activity.CreateDelayAsync(turnContext, cancellationToken);
                        await turnContext.SendActivityAsync(reply, cancellationToken);
                        break;
                    case "GetMedia":
                        entityFound = EntityExtractionFactory.CreateEntity(recognizerResult);

                        if (string.IsNullOrEmpty(entityFound))
                        {
                            reply.Text = "Do you prefer videos or images?";
                        }
                        else
                        {
                            if (entityFound == "Video")
                            {
                                reply.Text = "Okay, check it out! No Worries if they're a few, you can watch more on YouTube.";
                                reply.Attachments = VideoCardFactory.CreateVideoAttachments();
                                reply.AttachmentLayout = AttachmentLayoutTypes.List;
                            }
                            else
                            {
                                reply.Text = "Here are some images from the web. I'm sorry for some of them may be repeats.";
                                reply.Attachments = ImageCardFactory.CreateImageAttachments();
                                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                            }
                        }

                        await turnContext.Activity.CreateDelayAsync(turnContext, cancellationToken);
                        await turnContext.SendActivityAsync(reply, cancellationToken);
                        break;
                    case "GetNearestRestaurants":
                        reply.Text = "I found these restaurants restaurants nearby Sri Panja Sahib that provide accommodation. You can book the hotels from any of the travel portals, or contact the restaurants directly.";
                        reply.Attachments = GuestHouseCardFactory.CreateGuesHouseAttachments();
                        reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                        await turnContext.Activity.CreateDelayAsync(turnContext, cancellationToken);
                        await turnContext.SendActivityAsync(reply, cancellationToken);
                        break;
                    case "GetPopularTimes":
                        entityFound = EntityExtractionFactory.CreateEntity(recognizerResult);

                        if (string.IsNullOrEmpty(entityFound))
                        {
                            reply.Text = "Hey! I retrieved these popular visiting hours for today.";
                            attachment = PopularTimesCardFactory.CreatePopularTimesCardAttachment(string.Empty);
                        }
                        else
                        {
                            reply.Text = $"Hmm... Let me check if I can assist you with popular times for **{entityFound}**.";
                            attachment = PopularTimesCardFactory.CreatePopularTimesCardAttachment(entityFound);
                        }

                        reply.Attachments = new List<Attachment>
                        {
                            attachment,
                        };

                        await turnContext.Activity.CreateDelayAsync(turnContext, cancellationToken);
                        await turnContext.SendActivityAsync(reply, cancellationToken);
                        break;
                    default:
                        if (turnContext.Activity.Text.Length > 10)
                        {
                            await CosmosDBFactory.InsertUnknownQuestionAsync(turnContext.Activity.Text);
                        }

                        string message = "Hmm, that's something I don't know.";
                        //string message = "I'm sorry, I can't answer to this right now. But, soon I’ll explore on this particular question.";
                        await turnContext.Activity.CreateDelayAsync(turnContext, cancellationToken);
                        await turnContext.SendActivityAsync(message, cancellationToken: cancellationToken);
                        break;
                }
            }
    }
}
}
