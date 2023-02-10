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
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Panels
{
    class VerifyAccountPanel : StandardLayout
    {
        StackLayout masterContainer, contentContainer, buttonContainer;

        StaticLabel confirmationInfo, accountStatusText;

        ColourButton cancelButton, confirmButton;

        public VerifyAccountPanel() {
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
                Padding = Dimensions.GENERAL_COMPONENT_PADDING
            };
            #endregion

            confirmationInfo = new StaticLabel("Thanks " + AppSession.CurrentUser.FirstName + "! Your account: " + AppSession.CurrentUser.EmailAddress + " was created successfully.");
            confirmationInfo.Content.FontSize = Units.FontSizeM;
            confirmationInfo.Content.TextColor = Color.FromHex(Colors.CC_DARK_GREY);
            confirmationInfo.CenterAlign();
            accountStatusText = new StaticLabel("Account status: Activated");
            accountStatusText.Content.FontFamily = Fonts.GetBoldAppFont();
            accountStatusText.Content.FontSize = Units.FontSizeM;
            accountStatusText.CenterAlign();

            cancelButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CANCEL, new Models.Action((int)Actions.ActionName.HideVerifyAccount));
            cancelButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            cancelButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            cancelButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            cancelButton.Content.VerticalOptions = LayoutOptions.CenterAndExpand;

            confirmButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CONFIRM, new Models.Action((int)Actions.ActionName.LogIn));
            confirmButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            confirmButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            confirmButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            confirmButton.RightAlign();

            // Add the content to the containers
            #region Containers
            masterContainer.Children.Add(confirmationInfo.Content);
            masterContainer.Children.Add(accountStatusText.Content);
            //masterContainer.Children.Add(contentContainer);
            buttonContainer.Children.Add(cancelButton.Content);
            buttonContainer.Children.Add(confirmButton.Content);
            masterContainer.Children.Add(buttonContainer);
            Container.Children.Add(masterContainer);
            Content.Children.Add(Container);
            #endregion
        }
    }
}
