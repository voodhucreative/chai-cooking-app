using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Panels
{
    class WhiskSignupPanel : StandardLayout
    {
        StackLayout masterContainer, contentContainer, buttonContainer;

        StaticLabel confirmationInfo, accountStatusText;

        StaticLabel WhiskTokenLabel;

        ColourButton cancelButton, confirmButton;

        WebView WhiskView;

        string WhiskCode;
        string ReturnUrl;

        public StoppableTimer PageTimer;

        public bool DoneProcessing;
        public bool AllowContinue;

        public WhiskSignupPanel()
        {
            PageTimer = new Services.StoppableTimer(TimeSpan.FromSeconds(1), TimedUpdate, true);
            PageTimer.Stop();

            AllowContinue = false;
            DoneProcessing = false;

            AppSession.AuthorizeTimerRunning = false;

            Container = new Grid
            {
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Content = new Grid
            {
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            // Initialise our container layouts 
            #region Containers
            masterContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING
            };

            contentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            buttonContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            #endregion

            WhiskView = new WebView
            {
                Source = "https://app.chai.cooking/whisk/authorize",
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight,
                HorizontalOptions = LayoutOptions.Center
            };

            WhiskCode = null;

            WhiskTokenLabel = new StaticLabel("");

            WhiskTokenLabel.Content.BackgroundColor = Color.White;
            WhiskTokenLabel.Content.TextColor = Color.Black;
            WhiskTokenLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            WhiskTokenLabel.Content.WidthRequest = Units.ScreenWidth;
            WhiskTokenLabel.CenterAlign();

            // Add the content to the containers
            #region Containers
            masterContainer.Children.Add(WhiskView);
            //masterContainer.Children.Add(WhiskTokenLabel.Content);
            masterContainer.Children.Add(buttonContainer);


            Container.Children.Add(masterContainer);
            Content.Children.Add(Container);
            #endregion

            if (!AppSession.AuthorizeTimerRunning)
            {
                PageTimer.Start();
                AppSession.AuthorizeTimerRunning = true;
            }
        }

        public bool AttemptAuthorize()
        {
            bool tokenSent = false;

            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    ReturnUrl = await WhiskView.EvaluateJavaScriptAsync("window.location.href");

                    Uri myUri = new Uri(ReturnUrl);

                    String access_token = HttpUtility.ParseQueryString(myUri.Query).Get("code");

                    if (access_token.Length > 0)
                    {
                        WhiskCode = access_token;
                        AppSession.CurrentWhiskToken = WhiskCode;

                        WhiskTokenLabel.Content.Text = "Whisk token: " + WhiskCode;
                        masterContainer.Children.Remove(WhiskView);
                    }

                    tokenSent = DataManager.Authorize(AppSession.CurrentWhiskToken).Result;

                    if (tokenSent)
                    {
                        Console.WriteLine("Sent code to API");
                        App.ShowAlert("Successfully extracted the Whisk access token: " + access_token + " and successfully processed by our API :) !!");
                        PageTimer.Stop();
                        AppSession.AuthorizeTimerRunning = false;
                        DoneProcessing = true;

                        //await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.LoginAndRegistration);

                        await App.GoToLoginOrRegister();

                        //await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.LoginAndRegistration);

                    }
                    else
                    {
                        Console.WriteLine("Code not sent to API");
                        App.ShowAlert("Successfully extracted the Whisk access token: " + access_token + " but not sent to our API!");
                        PageTimer.Stop();
                        AppSession.AuthorizeTimerRunning = false;
                        DoneProcessing = true;
                    }
                }
                catch (Exception e) { }
            });

            return tokenSent;
        }

        public void TimedUpdate()
        {
            if (DoneProcessing)
            {
                PageTimer.Stop();
                AppSession.AuthorizeTimerRunning = false;
                return;
            }
            else
            {

                Console.WriteLine("Update authorize");

                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        if (!AttemptAuthorize())
                        {
                            Console.WriteLine("NOT YET");
                        }
                        else
                        {
                            Console.WriteLine("SORTED, LET'S GO");
                            PageTimer.Stop();
                            AppSession.AuthorizeTimerRunning = false;
                        }
                    }
                    catch (Exception e)
                    {

                    }
                });
            }
        }
    }
}
