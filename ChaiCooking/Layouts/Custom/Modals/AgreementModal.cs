using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Modals
{
    class AgreementModal : StandardLayout
    {
        // Create variables

        StackLayout masterContainer, contentContainer, buttonContainer;

        StaticLabel termsTitle, termsLink, privacyTitle, privacyLink;

        ColourButton cancelButton, confirmButton;

        public AgreementModal()
        {
            Container = new Grid { }; Content = new Grid { };

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

            buttonContainer = new StackLayout { Orientation = StackOrientation.Horizontal, Padding = Dimensions.GENERAL_COMPONENT_PADDING };
            #endregion

            // Initialise title
            #region Title
            termsTitle = new StaticLabel("By creating an account, you agree to");
            termsLink = new StaticLabel("CHAI's Terms of Service");
            termsLink.Content.FontFamily = Fonts.GetBoldAppFont();
            termsLink.Content.FontSize = Units.FontSizeM;

            privacyTitle = new StaticLabel("To learn more about how CHAI collects, uses, shares and protects your personal data please read");
            privacyLink = new StaticLabel("CHAI's Privacy Policy");
            privacyLink.Content.FontFamily = Fonts.GetBoldAppFont();
            ////PrivacyText.Content.FontSize = Units.FontSizeM;
            #endregion

            // Initialise the buttons
            #region Buttons
            cancelButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE),
                Color.White, AppText.CANCEL, null);
            cancelButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            cancelButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            cancelButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(cancelButton.Content, true);
            TouchEffect.SetCommand(cancelButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        //await App.ApiBridge.CreateUser(AppSession.CurrentUser);

                        if (AppSession.CurrentUser.AuthToken != null)
                        {
                            await App.PerformActionAsync(new Models.Action((int)Actions.ActionName.HideAccountConfirmationModal));

                        }
                    }
                    catch (Exception e)
                    {
                        App.ShowAlert("Failed to create account.");
                        // GO TO WHISK?
                    }
                });
            }));

            confirmButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE),
            Color.White, AppText.CONFIRM, null);

            confirmButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            confirmButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            confirmButton.RightAlign();
            confirmButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(confirmButton.Content, true);
            TouchEffect.SetCommand(confirmButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        //bool result = await App.ApiBridge.CreateUser(AppSession.CurrentUser);
                        //if (result)
                        //{
                            new Models.Action((int)Actions.ActionName.ShowVerifyAccount);
                        //}
                        //else
                        //{
                            //MainThread.BeginInvokeOnMainThread(() =>
                            //{
                            //    // Code to run on the main thread
                            //    App.ShowAlert("ERROR", "Failed to Create Account.");
                            //});
                        //}
                    }
                    catch (Exception e)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            // Code to run on the main thread
                            App.ShowAlert("ERROR", "Failed to Create Account.");
                        });
                    }
                });
            }));

            buttonContainer.Children.Add(cancelButton.Content);
            buttonContainer.Children.Add(confirmButton.Content);
            #endregion

            // Add the content to the containers
            #region Containers
            contentContainer.Children.Add(termsTitle.Content);
            contentContainer.Children.Add(termsLink.Content);
            contentContainer.Children.Add(privacyTitle.Content);
            contentContainer.Children.Add(privacyLink.Content);
            masterContainer.Children.Add(contentContainer);
            masterContainer.Children.Add(buttonContainer);
            Container.Children.Add(masterContainer);
            Content.Children.Add(Container);
            #endregion
        }
    }
}
