using System;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Modals
{
    public class StartDateModal : StandardLayout
    {
        StackLayout buttonContainer,
            closeBtnCont;
        StaticLabel termsTitle, termsLink, privacyTitle, privacyLink;

        ColourButton confirmButton;

        public StartDateModal(Influencer.Datum influencer, string planLength, string planName)
        {
            Container = new Grid { }; Content = new Grid { };

            Label createMealPlanLabel = new Label
            {
                Text = "Select a Start Date",
                FontAttributes = FontAttributes.Bold,
                FontSize = Units.FontSizeXXL,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Start
            };

            Label descriptionLabel = new Label
            {
                //Text = "Your current Meal Plan is empty - choose a name for it, select a start date, and the length of your meal plan, then press create.",
                Text = "Choose a start date, if its valid the Influencer's Meal Plan will be added to your Meal Plans.",
                FontAttributes = FontAttributes.Bold,
                FontSize = Units.FontSizeM,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Start
            };

            if (AppSession.CurrentUser.Preferences.AccountType == Accounts.AccountType.ChaiPremiumFlex)
            {
                descriptionLabel.Text += "\n You're currently on a CHAI Flexitiarian plan. Changing the meals on your calendar can reduce the effectiveness of your chosen plan.";
            }

            if (AppSession.CurrentUser.Preferences.AccountType == Accounts.AccountType.ChaiPremiumTrans)
            {
                descriptionLabel.Text += "\n You're currently on a CHAI Transitioning plan. Changing the meals on your calendar can reduce the effectiveness of your chosen plan.";
            }


            StackLayout createMealPlanLabelContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    createMealPlanLabel,
                    descriptionLabel
                }
            };

            Label startLabel = new Label
            {
                Text = "Start Date",
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White
            };

            DatePicker startDatePicker = new DatePicker
            {
                Date = DateTime.Now,
                MinimumDate = DateTime.UtcNow,
                TextColor = Color.FromHex(Colors.CC_ORANGE),
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                Format = "dd/MM/yyyy",
                WidthRequest = Units.ScreenWidth40Percent,
                HorizontalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold
            };

            StackLayout datePickerContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                HorizontalOptions = LayoutOptions.Center,
                Margin = 5,
                Children =
                {
                    startLabel,
                    startDatePicker
                }
            };

            StackLayout errorContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                }
            };

            ActiveLabel CloseLabel = new ActiveLabel(AppText.CLOSE, Units.FontSizeS, Color.Transparent, Color.White, null);
            TouchEffect.SetNativeAnimation(CloseLabel.Content, true);
            TouchEffect.SetCommand(CloseLabel.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.HideStartDateMenu();
                    });
                }));
            CloseLabel.RightAlign();

            closeBtnCont = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.End,
                Children =
                {
                    CloseLabel.Content
                }
            };

            ColourButton createButton = new ColourButton(Color.FromHex(Colors.CC_GREEN), Color.White, AppText.CREATE_BUTTON_TEXT, null);
            createButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            createButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            createButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(createButton.Content, true);
            TouchEffect.SetCommand(createButton.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        StaticData.hypenedDate = startDatePicker.Date.ToString("yyyy-MM-dd");
                        var result = await App.ApiBridge.AddWeek(AppSession.CurrentUser, StaticData.hypenedDate);
                        if (result)
                        {
                            await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.HealthyLiving);
                            App.ShowAlert($"Successfully added {planName} to calendar starting from {startDatePicker.Date} lasting approximately {planLength}");
                        }
                        else
                        {
                            errorContainer.Children.Clear();
                            errorContainer.Children.Add(new Label
                            {
                                Text = StaticData.errorText,
                                TextColor = Color.Orange
                            });
                        }
                    });
                }));

            ColourButton createAndAutofillButton = new ColourButton(Color.FromHex(Colors.CC_GREEN), Color.White, AppText.CREATE_BUTTON_TEXT + " AND FILL", null);
            createAndAutofillButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            createAndAutofillButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            createAndAutofillButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            StackLayout buttonContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                WidthRequest = Units.ScreenWidth,//30Percent,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    createButton.Content,
                    //createAndAutofillButton.Content
                }
            };

            StackLayout ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                WidthRequest = Units.ScreenWidth - Units.ScreenWidth20Percent,
                Children =
                {
                    closeBtnCont,
                    createMealPlanLabelContainer,
                    datePickerContainer,
                    errorContainer,
                    buttonContainer
                }
            };

            Container.Children.Add(ContentContainer);
            Content.Children.Add(Container);
        }
    }
}
