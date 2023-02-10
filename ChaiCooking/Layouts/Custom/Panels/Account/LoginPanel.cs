using System;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Composites;
using ChaiCooking.Components.Fields;
using ChaiCooking.Components.Fields.Custom;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.DebugData.Custom;
using Xamarin.Forms;
using ChaiCooking.Services;
using ChaiCooking.Services.Storage;
using ChaiCooking.Models;
using Xamarin.CommunityToolkit.Effects;

namespace ChaiCooking.Layouts.Custom.Panels.Account
{
    public class LoginPanel : StandardLayout
    {
        StackLayout ContentContainer;
        StackLayout TopButtonsContainer;
        StackLayout BottomButtonsContainer;

        FormInputField EmailInput;

        FormInputField PasswordInput;

        Components.Composites.CheckBox RememberMe;

        ColourButton LoginButton;
        IconButton SignInWithFaceBookButton;
        IconButton SignInWithAppleButton;
        IconButton SignInWithGoogleButton;

        StaticLabel OrLabel;
        StaticLabel ForgotPasswordLabel;
        bool rememberedTick;

        public LoginPanel()
        {
            Container = new Grid { };
            Content = new Grid { };
            ContentContainer = new StackLayout { Orientation = StackOrientation.Vertical, Padding = Dimensions.GENERAL_COMPONENT_PADDING };

            TopButtonsContainer = new StackLayout { VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, Orientation = StackOrientation.Horizontal, Padding = new Thickness(0, 24, 0, 0) };
            BottomButtonsContainer = new StackLayout { Orientation = StackOrientation.Horizontal, Padding = new Thickness(0, 24, 0, 0) };

            EmailInput = new FormInputField(AppText.EMAIL_ADDRESS, AppText.EMAIL_ADDRESS, Keyboard.Email, true);

            PasswordInput = new FormInputField("Chai " + AppText.PASSWORD, AppText.PASSWORD, Keyboard.Text, true);
            PasswordInput.TextEntry.IsPassword = true;
            PasswordInput.TextEntry.IsSpellCheckEnabled = false;
            PasswordInput.TextEntry.IsTextPredictionEnabled = false;
            PasswordInput.TextEntry.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeNone);

            try
            {
                EmailInput.TextEntry.Text = AppSession.CurrentUser.EmailAddress;
            }
            catch (Exception e)
            {

            }

            if (AppSettings.SimplifiedLoginAndRegister)
            {
                try
                {
                    EmailInput.TextEntry.Text = AppSession.CurrentUser.EmailAddress;
                }
                catch (Exception e)
                {

                }
            }
            else
            {
                if (AppSession.CurrentUser.RememberMe)
                {
                    rememberedTick = true;

                    if (AppSession.CurrentUser.EmailAddress != null)
                    {
                        EmailInput.TextEntry.Text = AppSession.CurrentUser.EmailAddress;
                        PasswordInput.TextEntry.Text = "";
                    }
                }
                else
                {
                    rememberedTick = false;
                    EmailInput.TextEntry.Text = "";
                    PasswordInput.TextEntry.Text = "";
                }
            }

            RememberMe = new Components.Composites.CheckBox(AppText.REMEMBER_ME, "tick.png", "tickbg.png", 180, 24, rememberedTick);
            RememberMe.Title.Content.TextColor = Color.Black;
            RememberMe.IsChecked = rememberedTick;

            LoginButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.LOGIN, null);
            LoginButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            LoginButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            LoginButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            LoginButton.CenterAlign();

            TouchEffect.SetNativeAnimation(LoginButton.Content, true);
            TouchEffect.SetCommand(LoginButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        // if(EmailInput)
                        AppSession.CurrentUser.EmailAddress = EmailInput.TextEntry.Text.ToString();
                        //

                        AppSession.CurrentUser.Password = PasswordInput.TextEntry.Text.ToString();
                    }
                    catch (Exception e)
                    {
                        App.ShowAlert("Please enter a valid password");
                    }


                    if (await App.ApiBridge.LoginUser(AppSession.CurrentUser))
                    {
                        if (RememberMe.IsChecked)
                        {
                            AppSession.CurrentUser.RememberMe = true;
                        }
                        else
                        {
                            AppSession.CurrentUser.RememberMe = false;
                        }
                        LocalDataStore.SaveAll();
                        AppSession.CurrentUser.IsRegistered = true;
                        var result = await App.ApiBridge.SwapWhiskToken(AppSession.CurrentUser);

                        // get saved user preferences here
                        UserPreferences userPrefs = await App.ApiBridge.GetPreferences(AppSession.CurrentUser);

                        if (userPrefs != null)
                        {
                            AppSession.CurrentUser.Preferences = userPrefs;
                        }

                        LocalDataStore.SaveAll();
                        await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Landing);
                    }
                    else
                    {

                    }
                });
            }));

            SignInWithFaceBookButton = new IconButton(210, Dimensions.STANDARD_BUTTON_HEIGHT, Color.FromHex(Colors.FACEBOOK_BLUE), Color.White, AppText.SIGN_IN_WITH, "fb_word.png", null);
            SignInWithFaceBookButton.SetIconSize(52, 16);
            SignInWithFaceBookButton.SetIconRight();
            SignInWithFaceBookButton.Label.HorizontalOptions = LayoutOptions.EndAndExpand;
            SignInWithFaceBookButton.Label.HorizontalTextAlignment = TextAlignment.End;
            SignInWithFaceBookButton.Icon.Content.Margin = new Thickness(0, 0, 0, 3);

            SignInWithAppleButton = new IconButton(210, Dimensions.STANDARD_BUTTON_HEIGHT, Color.Black, Color.White, AppText.SIGN_IN_WITH + " " + AppText.APPLE, "appleicon.png", null);
            SignInWithAppleButton.SetIconSize(16, 16);

            SignInWithGoogleButton = new IconButton(210, Dimensions.STANDARD_BUTTON_HEIGHT, Color.Black, Color.White, AppText.SIGN_IN_WITH + " " + AppText.GOOGLE, "googleicon.png", null);
            SignInWithGoogleButton.SetIconSize(16, 16);

            OrLabel = new StaticLabel(AppText.OR);
            OrLabel.Content.FontSize = 12;
            OrLabel.Content.WidthRequest = 24;
            OrLabel.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            OrLabel.Content.HorizontalTextAlignment = TextAlignment.Center;
            OrLabel.Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            OrLabel.Content.VerticalTextAlignment = TextAlignment.Center;

            ForgotPasswordLabel = new StaticLabel(AppText.FORGOTTEN_YOUR_PASSWORD);
            ForgotPasswordLabel.Content.WidthRequest = 180;
            ForgotPasswordLabel.Content.FontSize = 12;


            ForgotPasswordLabel.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               //CurrentSection = CANCEL_CONFIRM;
                               //SetSection(CurrentSection);
                               //await App.ShowForgotPassword();
                               if (await AccountManager.ForgotPassword(AppSession.CurrentUser.EmailAddress))
                               {
                                   App.ShowAlert("We've emailed you");
                               }
                           });
                       })
                   }
               );


            TopButtonsContainer.Children.Add(LoginButton.Content);
            //TopButtonsContainer.Children.Add(OrLabel.Content);
            //TopButtonsContainer.Children.Add(SignInWithFaceBookButton.Content);

            BottomButtonsContainer.Children.Add(ForgotPasswordLabel.Content);

            /*
            if(Device.RuntimePlatform == Device.iOS)
            {
                BottomButtonsContainer.Children.Add(SignInWithAppleButton.Content);
            }
            else
            {
                BottomButtonsContainer.Children.Add(SignInWithGoogleButton.Content);
            }
            */




            // DEBUG
            //EmailInput.TextEntry.SetBinding(InputView.TextProperty, new Binding(nameof(AppSession.CurrentUser.EmailAddress), source: AppSession.CurrentUser));
            //PasswordInput.TextEntry.SetBinding(InputView.TextProperty, new Binding(nameof(AppSession.CurrentUser.Password), source: AppSession.CurrentUser));
            //ContentContainer.Children.Add(EmailInput.Content);
            //if (!AppSettings.SimplifiedLoginAndRegister)
            //{


            EmailInput.DisableInput();

            ContentContainer.Children.Add(EmailInput.Content);
            //}
            ContentContainer.Children.Add(PasswordInput.Content);

            if (!AppSettings.SimplifiedLoginAndRegister)
            {
                ContentContainer.Children.Add(RememberMe.Content);
            }
            ContentContainer.Children.Add(TopButtonsContainer);
            ContentContainer.Children.Add(BottomButtonsContainer);
            Container.Children.Add(ContentContainer);
            Content.Children.Add(Container);
        }
    }
}
