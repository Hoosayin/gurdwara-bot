using GurdwaraBot.Helpers;
using GurdwaraBot.Models;
using GurdwaraBot.StateManagement;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GurdwaraBot.Dialogs
{
    public class RoomBookingDialog : ComponentDialog
    {
        public RoomBookingDialog(BotAccessors accessors) : base(nameof(RoomBookingDialog))
        {
            _accessors = accessors ?? throw new ArgumentNullException(nameof(accessors));

            WaterfallStep[] waterfallSteps = new WaterfallStep[]
            {
                InitializeStateStepAsync,
                CheckInStepAsync,
                CheckOutStepAsync,
                NumberofRoomsStepAsync,
                NumberofPeopleStepAsync,
                FoodPreferenceStepAsync,
                NameStepAsync,
                EmailStepAsync,
                EmailConfirmationStepAsync,
                AcknowledgementStepAsync,
            };

            AddDialog(new WaterfallDialog(WaterFallDialogId, waterfallSteps));
            AddDialog(new DateTimePrompt(CheckInPromptId, CheckInDateValidatorAsync));
            AddDialog(new DateTimePrompt(CheckOutPromptId, CheckOutDateValidatorAsync));
            AddDialog(new NumberPrompt<int>(NumberOfRoomsPromptId, NumberOfRoomsValidatorAsync));
            AddDialog(new NumberPrompt<int>(NumberOfPeoplePromptId, NumberOfPeopleValidatorAsync));
            AddDialog(new ChoicePrompt(FoodPreferencePromptId));
            AddDialog(new TextPrompt(NamePromptId, NameValidatorAsync));
            AddDialog(new TextPrompt(EmailPromptId, EmailValidatorAsync));
            AddDialog(new TextPrompt(ConfirmationCodePromptId, ConfirmationCodeValidatorAsync));

            TempData.UserData = new UserData();
        }

        private const string WaterFallDialogId = "WaterFallDialog";
        private const string CheckInPromptId = "CheckInPrompt";
        private const string CheckOutPromptId = "CheckOutPrompt";
        private const string NumberOfRoomsPromptId = "NumberOfRoomsPrompt";
        private const string NumberOfPeoplePromptId = "NumberOfPeoplePrompt";
        private const string FoodPreferencePromptId = "FoodPreferencePrompt";
        private const string NamePromptId = "NamePrompt";
        private const string EmailPromptId = "EmailPrompt";
        private const string ConfirmationCodePromptId = "ConfirmationCodePrompt";

        private readonly string[] _foodOptions = new string[]
        {
            "Aloo Methi Pratha",
            "Standard Pratha",
            "Vegetable Rice",
            "White Rice",
            "Chapati Daal",
        };

        private BotAccessors _accessors;

        private async Task<DialogTurnResult> InitializeStateStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userData = await _accessors.UserDataAccessor.GetAsync(stepContext.Context, () => null);
            if (userData == null)
            {
                var userDataOpt = stepContext.Options as UserData;
                if (userDataOpt != null)
                {
                    await _accessors.UserDataAccessor.SetAsync(stepContext.Context, userDataOpt);
                }
                else
                {
                    await _accessors.UserDataAccessor.SetAsync(stepContext.Context, new UserData());
                }
            }

            return await stepContext.NextAsync();
        }

        private async Task<DialogTurnResult> CheckInStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            PromptOptions promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("What date would you like to visit Sri Panja Sahib?"),
                RetryPrompt = MessageFactory.Text("This is not a valid date. What Date should I make your booking for?"),
            };

            await stepContext.Context.Activity.CreateDelayAsync(stepContext.Context, cancellationToken);
            return await stepContext.PromptAsync(CheckInPromptId, promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> CheckOutStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            DateTimeResolution dateTimeResolution = (stepContext.Result as IList<DateTimeResolution>).First();
            string dateString = dateTimeResolution.Value ?? dateTimeResolution.Start;
            DateTime date = Convert.ToDateTime(dateString, new CultureInfo("en-US"));

            TempData.UserData = await GetUserDataAsync(stepContext.Context, cancellationToken);
            TempData.UserData.CheckInDate = date;
            await SetUserDataAsync(stepContext.Context, TempData.UserData, cancellationToken);

            PromptOptions promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("What date would you be leaving Sri Panja Sahib?"),
                RetryPrompt = MessageFactory.Text("This is not a valid date. What Date would you like to leave Sri Panja Sahib?"),
            };

            await stepContext.Context.Activity.CreateDelayAsync(stepContext.Context, cancellationToken);
            return await stepContext.PromptAsync(CheckOutPromptId, promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> NumberofRoomsStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            DateTimeResolution dateTimeResolution = (stepContext.Result as IList<DateTimeResolution>).First();
            string dateString = dateTimeResolution.Value ?? dateTimeResolution.Start;
            DateTime date = Convert.ToDateTime(dateString, new CultureInfo("en-US"));

            TempData.UserData = await GetUserDataAsync(stepContext.Context, cancellationToken);
            TempData.UserData.CheckOutDate = date;
            TempData.UserData.ExpirationDate = date;
            await SetUserDataAsync(stepContext.Context, TempData.UserData, cancellationToken);

            PromptOptions promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("How many rooms do you want to book?"),
                RetryPrompt = MessageFactory.Text("How many rooms is this booking for?"),
            };

            await stepContext.Context.Activity.CreateDelayAsync(stepContext.Context, cancellationToken);
            return await stepContext.PromptAsync(NumberOfRoomsPromptId, promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> NumberofPeopleStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            int numberOfRooms = (int)stepContext.Result;

            TempData.UserData = await GetUserDataAsync(stepContext.Context, cancellationToken);

            if (TempData.UserData.CanCancelDialog)
            {
                await stepContext.CancelAllDialogsAsync(cancellationToken);
                return new DialogTurnResult(DialogTurnStatus.Cancelled);
            }

            TempData.UserData.NumberOfRooms = numberOfRooms;
            await SetUserDataAsync(stepContext.Context, TempData.UserData, cancellationToken);

            PromptOptions promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("How many people will be joining you?"),
                RetryPrompt = MessageFactory.Text("How many people will be joining you?"),
            };

            await stepContext.Context.Activity.CreateDelayAsync(stepContext.Context, cancellationToken);
            return await stepContext.PromptAsync(NumberOfPeoplePromptId, promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> FoodPreferenceStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            int numberOfPeople = (int)stepContext.Result;

            TempData.UserData = await GetUserDataAsync(stepContext.Context, cancellationToken);
            TempData.UserData.Attendees = numberOfPeople;
            await SetUserDataAsync(stepContext.Context, TempData.UserData, cancellationToken);

            await stepContext.Context.Activity.CreateDelayAsync(stepContext.Context, cancellationToken);
            await stepContext.Context.SendActivityAsync($"Excellent, {numberOfPeople} of your best buds!", cancellationToken: cancellationToken);
            await stepContext.Context.Activity.CreateDelayAsync(stepContext.Context, cancellationToken);
            await stepContext.Context.SendActivityAsync("Before I finish up booking, just letting you know that Sri Panja Sahib comes with a delicious Langar meal!", cancellationToken: cancellationToken);

            PromptOptions promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Please let me know your protein preference:"),
                Choices = ChoiceFactory.ToChoices(_foodOptions),
            };

            await stepContext.Context.Activity.CreateDelayAsync(stepContext.Context, cancellationToken);
            return await stepContext.PromptAsync(FoodPreferencePromptId, promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> NameStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string foodPreference = (stepContext.Result as FoundChoice).Value;

            TempData.UserData = await GetUserDataAsync(stepContext.Context, cancellationToken);
            TempData.UserData.MealPreference = foodPreference;
            await SetUserDataAsync(stepContext.Context, TempData.UserData, cancellationToken);

            await stepContext.Context.Activity.CreateDelayAsync(stepContext.Context, cancellationToken);
            await stepContext.Context.SendActivityAsync($"{foodPreference}, nice choice!", cancellationToken: cancellationToken);
            await stepContext.Context.Activity.CreateDelayAsync(stepContext.Context, cancellationToken);
            await stepContext.Context.SendActivityAsync("Tell me about yourself.", cancellationToken: cancellationToken);
            await stepContext.Context.Activity.CreateDelayAsync(stepContext.Context, cancellationToken);
            await stepContext.Context.SendActivityAsync("I just need a few more details to get you booked for the trip of a lifetime!", cancellationToken: cancellationToken);
            await stepContext.Context.Activity.CreateDelayAsync(stepContext.Context, cancellationToken);
            await stepContext.Context.SendActivityAsync("Don’t worry, I’ll never share or sell your information.", cancellationToken: cancellationToken);

            PromptOptions promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("So, what's your full name?"),
                RetryPrompt = MessageFactory.Text("What's your name again?"),
            };

            await stepContext.Context.Activity.CreateDelayAsync(stepContext.Context, cancellationToken);
            return await stepContext.PromptAsync(NamePromptId, promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> EmailStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string name = stepContext.Result.ToString();

            TempData.UserData = await GetUserDataAsync(stepContext.Context, cancellationToken);
            TempData.UserData.Name = name;
            await SetUserDataAsync(stepContext.Context, TempData.UserData, cancellationToken);

            PromptOptions promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("What's your email address? I'll send you a confirmation code, so please try to enter a valid email."),
                RetryPrompt = MessageFactory.Text("What's your email again?"),
            };

            await stepContext.Context.Activity.CreateDelayAsync(stepContext.Context, cancellationToken);
            return await stepContext.PromptAsync(EmailPromptId, promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> EmailConfirmationStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string email = stepContext.Result.ToString();
            TempData.UserData = await GetUserDataAsync(stepContext.Context, cancellationToken);

            if (TempData.UserData.CanCancelDialog)
            {
                await stepContext.CancelAllDialogsAsync(cancellationToken);
                return new DialogTurnResult(DialogTurnStatus.Cancelled);
            }

            TempData.UserData.Email = email;
            await SetUserDataAsync(stepContext.Context, TempData.UserData, cancellationToken);

            PromptOptions promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"I've sent a 5-Digit confirmation code at {email}. Please enter that code here!"),
                RetryPrompt = MessageFactory.Text("Please enter the confirmation code."),
            };

            await stepContext.Context.Activity.CreateDelayAsync(stepContext.Context, cancellationToken);
            return await stepContext.PromptAsync(ConfirmationCodePromptId, promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> AcknowledgementStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.Activity.CreateDelayAsync(stepContext.Context, cancellationToken);
            await stepContext.Context.SendActivityAsync("Please wait while I generate your request.", cancellationToken: cancellationToken);
            await stepContext.Context.Activity.CreateDelayAsync(stepContext.Context, cancellationToken);
            await stepContext.Context.SendActivityAsync("Be patient! It may take a few minutes. I need to make entries in the database.", cancellationToken: cancellationToken);
            TempData.UserData = await GetUserDataAsync(stepContext.Context, cancellationToken);
            TempData.UserData.DataCollected = true;
            await SetUserDataAsync(stepContext.Context, TempData.UserData, cancellationToken);
            return await stepContext.EndDialogAsync(cancellationToken);
        }

        private async Task<bool> CheckInDateValidatorAsync(PromptValidatorContext<IList<DateTimeResolution>> promptContext, CancellationToken cancellationToken)
        {
            if (!promptContext.Recognized.Succeeded)
            {
                await promptContext.Context.Activity.CreateDelayAsync(promptContext.Context, cancellationToken);
                await promptContext.Context.SendActivityAsync("I'm sorry, I do not understand. Please enter the date for your booking.", cancellationToken: cancellationToken);
                return false;
            }

            DateTime dateTime = DateTime.Now.AddHours(1.0);
            DateTimeResolution value = promptContext.Recognized.Value.FirstOrDefault(v => DateTime.TryParse(v.Value ?? v.Start, out DateTime time) && DateTime.Compare(dateTime, time) <= 0);

            if (value != null)
            {
                string dateString = value.Value;
                DateTime date = Convert.ToDateTime(dateString, new CultureInfo("en-US"));

                if (DateTime.Compare(date, DateTime.Now.AddDays(2)) > 0)
                {
                    await promptContext.Context.Activity.CreateDelayAsync(promptContext.Context, cancellationToken);
                    await promptContext.Context.SendActivityAsync("Sorry, I can't take reservations later than two days from now.", cancellationToken: cancellationToken);
                    return false;
                }

                promptContext.Recognized.Value.Clear();
                promptContext.Recognized.Value.Add(value);
                return true;
            }

            await promptContext.Context.Activity.CreateDelayAsync(promptContext.Context, cancellationToken);
            await promptContext.Context.SendActivityAsync("I'm sorry, I can't take reservations earlier than an hour from now.", cancellationToken: cancellationToken);
            return false;
        }

        private async Task<bool> CheckOutDateValidatorAsync(PromptValidatorContext<IList<DateTimeResolution>> promptContext, CancellationToken cancellationToken)
        {
            if (!promptContext.Recognized.Succeeded)
            {
                await promptContext.Context.Activity.CreateDelayAsync(promptContext.Context, cancellationToken);
                await promptContext.Context.SendActivityAsync("I'm sorry, I do not understand. Please enter the date for checking out.", cancellationToken: cancellationToken);
                return false;
            }

            TempData.UserData = await GetUserDataAsync(promptContext.Context, cancellationToken);
            DateTime dateTime = TempData.UserData.CheckInDate.AddHours(1.0);

            if (DateTime.Compare(dateTime, DateTime.Now.AddDays(2)) > 0)
            {
                await promptContext.Context.Activity.CreateDelayAsync(promptContext.Context, cancellationToken);
                await promptContext.Context.SendActivityAsync("Sorry, I can't take reservations later than two days from now.", cancellationToken: cancellationToken);
                return false;
            }

            DateTimeResolution value = promptContext.Recognized.Value.FirstOrDefault(v => DateTime.TryParse(v.Value ?? v.Start, out DateTime time) && DateTime.Compare(dateTime, time) <= 0);

            if (value != null)
            {
                string dateString = value.Value;
                DateTime date = Convert.ToDateTime(dateString, new CultureInfo("en-US"));

                if (DateTime.Compare(date, DateTime.Now.AddDays(4)) > 0)
                {
                    await promptContext.Context.Activity.CreateDelayAsync(promptContext.Context, cancellationToken);
                    await promptContext.Context.SendActivityAsync("Sorry, I can't take reservations more than four days from now.", cancellationToken: cancellationToken);
                    return false;
                }

                promptContext.Recognized.Value.Clear();
                promptContext.Recognized.Value.Add(value);
                return true;
            }

            await promptContext.Context.Activity.CreateDelayAsync(promptContext.Context, cancellationToken);
            await promptContext.Context.SendActivityAsync("I'm sorry, I can't take checkouts earlier than an hour from checkins.", cancellationToken: cancellationToken);
            return false;
        }

        private async Task<bool> NumberOfRoomsValidatorAsync(PromptValidatorContext<int> promptContext, CancellationToken cancellationToken)
        {
            if (!promptContext.Recognized.Succeeded)
            {
                await promptContext.Context.Activity.CreateDelayAsync(promptContext.Context, cancellationToken);
                await promptContext.Context.SendActivityAsync($"I'm sorry, I do not understand. Please enter the number of rooms for booking.", cancellationToken: cancellationToken);
                return false;
            }

            int numberOfRooms = promptContext.Recognized.Value;

            if (numberOfRooms < 1 || numberOfRooms > 3)
            {
                await promptContext.Context.Activity.CreateDelayAsync(promptContext.Context, cancellationToken);
                await promptContext.Context.SendActivityAsync("Sorry, I can only take bookings for 1 up to 3 rooms.", cancellationToken: cancellationToken);
                return false;
            }

            await promptContext.Context.Activity.CreateDelayAsync(promptContext.Context, cancellationToken);
            await promptContext.Context.SendActivityAsync($"Please wait while I search if we have any available rooms. This may take several minutes.", cancellationToken: cancellationToken);

            if (!await CosmosDBFactory.AreRoomsAvailableAsync(numberOfRooms))
            {
                if (numberOfRooms == 1)
                {
                    await promptContext.Context.Activity.CreateDelayAsync(promptContext.Context, cancellationToken);
                    await promptContext.Context.SendActivityAsync("I'm very sorry! No rooms are available for booking at the moment.", cancellationToken: cancellationToken);
                    TempData.UserData = await GetUserDataAsync(promptContext.Context, cancellationToken);
                    TempData.UserData.CanCancelDialog = true;
                    await SetUserDataAsync(promptContext.Context, TempData.UserData, cancellationToken);
                    return true;
                }
                else
                {
                    await promptContext.Context.Activity.CreateDelayAsync(promptContext.Context, cancellationToken);
                    await promptContext.Context.SendActivityAsync($"Sorry! {numberOfRooms} room(s) are available for booking at the moment. Maybe a single room is still available for booking. Take a try!", cancellationToken: cancellationToken);
                }
                return false;
            }

            await promptContext.Context.Activity.CreateDelayAsync(promptContext.Context, cancellationToken);
            await promptContext.Context.SendActivityAsync("Okay, the rooms are available!", cancellationToken: cancellationToken);
            return true;
        }

        private async Task<bool> NumberOfPeopleValidatorAsync(PromptValidatorContext<int> promptContext, CancellationToken cancellationToken)
        {
            if (!promptContext.Recognized.Succeeded)
            {
                await promptContext.Context.Activity.CreateDelayAsync(promptContext.Context, cancellationToken);
                await promptContext.Context.SendActivityAsync($"I'm sorry, I do not understand. Please enter the number of people who are joining you.", cancellationToken: cancellationToken);
                return false;
            }

            int numberOfPeople = promptContext.Recognized.Value;

            if (numberOfPeople < 1 || numberOfPeople > 6)
            {
                await promptContext.Context.Activity.CreateDelayAsync(promptContext.Context, cancellationToken);
                await promptContext.Context.SendActivityAsync("Sorry, I can take bookings only for 1 to 6 people.", cancellationToken: cancellationToken);
                return false;
            }

            return true;
        }

        private async Task<bool> NameValidatorAsync(PromptValidatorContext<string> promptContext, CancellationToken cancellationToken)
        {
            if (!promptContext.Recognized.Succeeded)
            {
                await promptContext.Context.Activity.CreateDelayAsync(promptContext.Context, cancellationToken);
                await promptContext.Context.SendActivityAsync($"I'm sorry, I do not understand. Please your real name.", cancellationToken: cancellationToken);
                return false;
            }

            string name = promptContext.Recognized.Value;

            if (!Validator.IsValidName(name))
            {
                await promptContext.Context.Activity.CreateDelayAsync(promptContext.Context, cancellationToken);
                await promptContext.Context.SendActivityAsync("Sorry, this name is invalid. Enter a valid name please.", cancellationToken: cancellationToken);
                return false;
            }

            return true;
        }

        private async Task<bool> EmailValidatorAsync(PromptValidatorContext<string> promptContext, CancellationToken cancellationToken)
        {
            if (!promptContext.Recognized.Succeeded)
            {
                await promptContext.Context.Activity.CreateDelayAsync(promptContext.Context, cancellationToken);
                await promptContext.Context.SendActivityAsync($"I'm sorry, I do not understand. Please your valid email.", cancellationToken: cancellationToken);
                return false;
            }

            string email = promptContext.Recognized.Value;

            if (!Validator.IsValidEmail(email))
            {
                await promptContext.Context.Activity.CreateDelayAsync(promptContext.Context, cancellationToken);
                await promptContext.Context.SendActivityAsync("Sorry, this email is invalid. Bookings are only done if I get valid emails.", cancellationToken: cancellationToken);
                return false;
            }

            if (!await CosmosDBFactory.IsEmailEligibleAsync(email))
            {
                await promptContext.Context.Activity.CreateDelayAsync(promptContext.Context, cancellationToken);
                await promptContext.Context.SendActivityAsync("Sorry, you are not eligible for sarai booking. You'll only be able to request again after the expiration of previous request.", cancellationToken: cancellationToken);
                TempData.UserData = await GetUserDataAsync(promptContext.Context, cancellationToken);
                TempData.UserData.CanCancelDialog = true;
                await SetUserDataAsync(promptContext.Context, TempData.UserData, cancellationToken);
                return true;
            }

            try
            {
                TempData.UserData = await GetUserDataAsync(promptContext.Context, cancellationToken);
                TempData.UserData.ConfirmationCode = EmailFactory.SendConfirmationCode(email);
                await SetUserDataAsync(promptContext.Context, TempData.UserData, cancellationToken);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> ConfirmationCodeValidatorAsync(PromptValidatorContext<string> promptContext, CancellationToken cancellationToken)
        {
            if (!promptContext.Recognized.Succeeded)
            {
                await promptContext.Context.Activity.CreateDelayAsync(promptContext.Context, cancellationToken);
                await promptContext.Context.SendActivityAsync($"I'm sorry, I do not understand. Please enter the confirmation code.", cancellationToken: cancellationToken);
                return false;
            }

            string code = promptContext.Recognized.Value;
            TempData.UserData = await GetUserDataAsync(promptContext.Context, cancellationToken);

            if (!(code == TempData.UserData.ConfirmationCode))
            {
                await promptContext.Context.Activity.CreateDelayAsync(promptContext.Context, cancellationToken);
                await promptContext.Context.SendActivityAsync("Sorry, this code in not valid. Please re-check your email.", cancellationToken: cancellationToken);
                return false;
            }

            return true;
        }

        private async Task SetUserDataAsync(ITurnContext turnContext, UserData userData, CancellationToken cancellationToken = default)
        {
            await _accessors.UserDataAccessor.SetAsync(turnContext, userData, cancellationToken);
            await _accessors.UserState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        private async Task<UserData> GetUserDataAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            return await _accessors.UserDataAccessor.GetAsync(turnContext, () => new UserData(), cancellationToken);
        }
    }
}
