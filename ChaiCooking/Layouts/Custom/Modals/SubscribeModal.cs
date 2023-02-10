using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Composites;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Types;
using ChaiCooking.Services;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Modals
{
    public class SubscribeModal : ActiveComponent
    {
        //List<RadioButton> Durations = new List<RadioButton>();
        //List<RadioButton> AccountTypes = new List<RadioButton>();

        List<Components.Composites.CheckBox> Durations = new List<Components.Composites.CheckBox>();
        List<Components.Composites.CheckBox> AccountTypes = new List<Components.Composites.CheckBox>();

        Label TitleLabel;
        Label InfoLabel;
        Label DurationInfoLabel;
        Label AccountTypeInfoLabel;

        Label PlanInfoLabel;

        StackLayout TitleContainer;

        Label CloseLabel;
        StackLayout CloseLabelContainer;

        StackLayout TitleContent;

        ColourButton ConfirmBtn;

        StackLayout BtnCont;

        StackLayout MasterContainer;

        int SelectedDuration;

        Components.Composites.CheckBox FlexDiet;
        Components.Composites.CheckBox TransDiet;
        Components.Composites.CheckBox VeganDiet;

        Components.Composites.CheckBox AnnualSub;
        Components.Composites.CheckBox MonthlySub;

        public SubscribeModal()
        {
            SelectedDuration = 12;

            TitleLabel = new Label
            {
                Text = "PREMIUM SUBSCRIPTION",
                FontSize = Units.FontSizeL,
                TextColor = Color.White,
                FontFamily = Fonts.GetBoldAppFont(),
                HorizontalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Start
            };

            InfoLabel = new Label
            {
                Text = "This is a premium feature. Please subscribe to unlock this area of the app.",
                FontSize = Units.FontSizeML,
                TextColor = Color.White,
                FontFamily = Fonts.GetRegularAppFont()
            };

            DurationInfoLabel = new Label
            {
                Text = "Please choose your subscription duration. The subscription will renew automatically at the end of each billing period. You can cancel your subscription at any time.",
                FontSize = Units.FontSizeML,
                TextColor = Color.White,
                FontFamily = Fonts.GetRegularAppFont()
            };

            AccountTypeInfoLabel = new Label
            {
                Text = "Please choose your plan.",
                FontSize = Units.FontSizeML,
                TextColor = Color.White,
                FontFamily = Fonts.GetRegularAppFont()
            };

            FormattedString subsInfo = new FormattedString();

            foreach (AttributedText line in AppDataContent.SubscriptionTypeText)
            {
                subsInfo.Spans.Add(new Span
                {
                    Text = line.TextContent,
                    ForegroundColor = line.TextColor,
                    FontFamily = line.TextFont,
                    FontSize = line.TextFontSize,
                    TextColor = line.TextColor

                }
               );
            }

            PlanInfoLabel = new Label
            {
                FormattedText = subsInfo,
                FontSize = Units.FontSizeML,
                TextColor = Color.White,
                FontFamily = Fonts.GetRegularAppFont()
            };

            TitleContainer = new StackLayout
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Padding = 0,//Dimensions.GENERAL_COMPONENT_PADDING,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    TitleLabel
                }
            };

            CloseLabel = new Label
            {
                Text = AppText.CLOSE,
                FontSize = Units.FontSizeML,
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                HorizontalTextAlignment = TextAlignment.End,
                FontFamily = Fonts.GetRegularAppFont()
            };

            CloseLabelContainer = new StackLayout
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                Padding = 0,//Dimensions.GENERAL_COMPONENT_PADDING,
                //BackgroundColor = Color.Blue,
                //WidthRequest = 128,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    CloseLabel
                }
            };

            TitleContent = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    TitleContainer,
                    CloseLabelContainer
                }
            };

            TouchEffect.SetNativeAnimation(CloseLabel, true);
            TouchEffect.SetCommand(CloseLabel,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.HideModalAsync();
                    });
                }));

            FlexDiet = new Components.Composites.CheckBox("Flexitarian Diet", "tick.png", "tickbg.png", 340, 30, false);
            TransDiet = new Components.Composites.CheckBox("Transition to a Vegan Diet", "tick.png", "tickbg.png", 340, 30, false);
            VeganDiet = new Components.Composites.CheckBox("Vegan Diet", "tick.png", "tickbg.png", 340, 30, false);

            AnnualSub = new Components.Composites.CheckBox("Annual\n(£9.99)", "tick.png", "tickbg.png", 280, 30, true);
            MonthlySub = new Components.Composites.CheckBox("Monthly\n(£0.99)", "tick.png", "tickbg.png", 280, 30, false);


            AnnualSub.Title.Content.FormattedText = new FormattedString();
            AnnualSub.Title.Content.FormattedText.Spans.Add(new Span { Text = "Annual", TextColor = Color.White, FontFamily = Fonts.GetBoldAppFont(), FontSize = Units.FontSizeML });
            AnnualSub.Title.Content.FormattedText.Spans.Add(new Span { Text = "\n(£9.99 per year)", TextColor = Color.White, FontFamily = Fonts.GetRegularAppFont(), FontSize = Units.FontSizeM });

            MonthlySub.Title.Content.FormattedText = new FormattedString();
            MonthlySub.Title.Content.FormattedText.Spans.Add(new Span { Text = "Monthly", TextColor = Color.White, FontFamily = Fonts.GetBoldAppFont(), FontSize = Units.FontSizeML });
            MonthlySub.Title.Content.FormattedText.Spans.Add(new Span { Text = "\n(£0.99 per month)", TextColor = Color.White, FontFamily = Fonts.GetRegularAppFont(), FontSize = Units.FontSizeM });


            FlexDiet.Title.Content.FormattedText = new FormattedString();
            FlexDiet.Title.Content.FormattedText.Spans.Add(new Span { Text = "Flexitarian Diet", TextColor = Color.White, FontFamily = Fonts.GetBoldAppFont(), FontSize = Units.FontSizeML });
            FlexDiet.Title.Content.FormattedText.Spans.Add(new Span { Text = "\n(Cut down the amount of meat you eat)", TextColor = Color.White, FontFamily = Fonts.GetRegularAppFont(), FontSize = Units.FontSizeM });

            TransDiet.Title.Content.FormattedText = new FormattedString();
            TransDiet.Title.Content.FormattedText.Spans.Add(new Span { Text = "Transition to a Vegan Diet", TextColor = Color.White, FontFamily = Fonts.GetBoldAppFont(), FontSize = Units.FontSizeML });
            TransDiet.Title.Content.FormattedText.Spans.Add(new Span { Text = "\n(Transition to a vegan diet over 12 months)", TextColor = Color.White, FontFamily = Fonts.GetRegularAppFont(), FontSize = Units.FontSizeM });

            VeganDiet.Title.Content.FormattedText = new FormattedString();
            VeganDiet.Title.Content.FormattedText.Spans.Add(new Span { Text = "Vegan Diet", TextColor = Color.White, FontFamily = Fonts.GetBoldAppFont(), FontSize = Units.FontSizeML });
            VeganDiet.Title.Content.FormattedText.Spans.Add(new Span { Text = "\n(Enjoy a fully vegan diet and lifestyle)", TextColor = Color.White, FontFamily = Fonts.GetRegularAppFont(), FontSize = Units.FontSizeM });


            FlexDiet.Title.Content.TextColor = Color.White;
            TransDiet.Title.Content.TextColor = Color.White;
            VeganDiet.Title.Content.TextColor = Color.White;
            AnnualSub.Title.Content.TextColor = Color.White;
            MonthlySub.Title.Content.TextColor = Color.White;

            FlexDiet.Title.Content.FontSize = Units.FontSizeML;
            TransDiet.Title.Content.FontSize = Units.FontSizeML;
            VeganDiet.Title.Content.FontSize = Units.FontSizeML;
            AnnualSub.Title.Content.FontSize = Units.FontSizeML;
            MonthlySub.Title.Content.FontSize = Units.FontSizeML;

            FlexDiet.OverrideGestures();
            TransDiet.OverrideGestures();
            VeganDiet.OverrideGestures();
            AnnualSub.OverrideGestures();
            MonthlySub.OverrideGestures();

            //Automatically selecting vegan diet option
            FlexDiet.UnSelect();
            TransDiet.UnSelect();
            VeganDiet.Select();

            //Asks user to select an account type if this is ChaiFree
            AppSession.SelectedAccountType = Accounts.AccountType.ChaiPremiumVegan;

            TouchEffect.SetNativeAnimation(FlexDiet.Content, true);
            TouchEffect.SetCommand(FlexDiet.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    //AppSession.CurrentUser.Preferences.AccountType = Accounts.AccountType.ChaiPremiumFlex;
                    AppSession.SelectedAccountType = Accounts.AccountType.ChaiPremiumFlex;
                    FlexDiet.Select();
                    TransDiet.UnSelect();
                    VeganDiet.UnSelect();
                    
                });
            }));

            TouchEffect.SetNativeAnimation(TransDiet.Content, true);
            TouchEffect.SetCommand(TransDiet.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    //AppSession.CurrentUser.Preferences.AccountType = Accounts.AccountType.ChaiPremiumTrans;
                    AppSession.SelectedAccountType = Accounts.AccountType.ChaiPremiumTrans;
                    FlexDiet.UnSelect();
                    TransDiet.Select();
                    VeganDiet.UnSelect();
                });
            }));

            TouchEffect.SetNativeAnimation(VeganDiet.Content, true);
            TouchEffect.SetCommand(VeganDiet.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    //AppSession.CurrentUser.Preferences.AccountType = Accounts.AccountType.ChaiPremiumVegan;
                    AppSession.SelectedAccountType = Accounts.AccountType.ChaiPremiumVegan;
                    FlexDiet.UnSelect();
                    TransDiet.UnSelect();
                    VeganDiet.Select();
                });
            }));

            TouchEffect.SetNativeAnimation(AnnualSub.Content, true);
            TouchEffect.SetCommand(AnnualSub.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    SelectedDuration = 12;
                    MonthlySub.UnSelect();
                    AnnualSub.Select();
                });
            }));

            TouchEffect.SetNativeAnimation(MonthlySub.Content, true);
            TouchEffect.SetCommand(MonthlySub.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    SelectedDuration = 1;
                    MonthlySub.Select();
                    AnnualSub.UnSelect();
                });
            }));

            Durations.Add(AnnualSub);
            Durations.Add(MonthlySub);

            AccountTypes.Add(FlexDiet);
            //AccountTypes.Add(TransDiet);
            AccountTypes.Add(VeganDiet);

            ConfirmBtn = new ColourButton
            (Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CONFIRM, null);
            ConfirmBtn.Content.WidthRequest = 150;
            ConfirmBtn.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            ConfirmBtn.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(ConfirmBtn.Content, true);
            TouchEffect.SetCommand(ConfirmBtn.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (AppSession.SelectedAccountType != Accounts.AccountType.ChaiFree)
                        {
                            ConfirmBtn.ButtonShape.Color = Color.FromHex(Colors.CC_DARK_ORANGE);
                            await Task.Delay(50);
                            ConfirmBtn.ButtonShape.Color = Color.FromHex(Colors.CC_ORANGE);
                            await App.PurchaseSubscription(SelectedDuration);
                        }
                        else
                        {
                            App.ShowAlert("Please select an account type.");
                        }
                    });
                }));

            BtnCont = new StackLayout
            {
                WidthRequest = 150,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Children =
                {
                    ConfirmBtn.Content
                }
            };


            StackLayout durationsContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            StackLayout accountTypeContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            /*
            foreach(RadioButton button in Durations)
            {
                durationsContainer.Children.Add(button);
            }

            foreach (RadioButton button in AccountTypes)
            {
                accountTypeContainer.Children.Add(button);
            }*/

            foreach (Components.Composites.CheckBox button in Durations)
            {
                durationsContainer.Children.Add(button.Content);
            }

            foreach (Components.Composites.CheckBox button in AccountTypes)
            {
                accountTypeContainer.Children.Add(button.Content);
            }

            MasterContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                WidthRequest = Units.ScreenWidth * 0.9,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Spacing = Dimensions.GENERAL_COMPONENT_PADDING,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,

                Children =
                {
                    TitleContent,
                    InfoLabel,
                    DurationInfoLabel,
                    durationsContainer,
                    AccountTypeInfoLabel,
                    //PlanInfoLabel,
                    //accountTypeContainer,
                    VeganDiet.Content,
                    FlexDiet.Content,
                    TransDiet.Content,
                    BtnCont
                }
            };


            Frame frame = new Frame
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Content = MasterContainer,
                CornerRadius = 5,
                IsClippedToBounds = true,
                Padding = 0,
            };

            Content.Children.Add(frame);
        }


        private void PerformSubscribe()
        {

            // test
            /*
            AppSession.CurrentUser.Preferences.AccountType = Accounts.AccountType.ChaiPremiumVegan;
            //AppSession.CurrentUser.Preferences.AccountType = Accounts.AccountType.ChaiPremiumTrans;
            //AppSession.CurrentUser.Preferences.AccountType = Accounts.AccountType.ChaiPremiumFlex;

            bool sub = await App.ApiBridge.CreateChaiSubscriptionPlan(AppSession.CurrentUser);

            List<AccountType> subs = await App.ApiBridge.GetSubscriptions(AppSession.CurrentUser);
            AccountType test = await App.ApiBridge.GetCurrentSubscription(AppSession.CurrentUser);

            Console.WriteLine("Hello " + test.ToString());
            //bool del = await App.ApiBridge.DeletePlan(AppSession.CurrentUser, "flexitarian");
            */
        }

    }
}
