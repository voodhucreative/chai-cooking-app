using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Fields.Custom;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Modals
{
    class ForgotPasswordModal : StandardLayout
    {

        // Create variables

        StackLayout masterContainer, contentContainer, buttonContainer;

        StaticLabel title;

        FormInputField usernameInput, emailInput;

        ColourButton cancelButton;
        Components.Buttons.ImageButton submitButton;

        public ForgotPasswordModal()
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
            title = new StaticLabel(AppText.RESET_PASSWORD_INFO);
            title.Content.FontSize = Units.FontSizeM;
            title.Content.TextColor = Color.FromHex(Colors.CC_DARK_GREY);
            title.CenterAlign();
            #endregion

            // Initialise the entry fields and add them
            #region Entry Fields
            usernameInput = new FormInputField(AppText.USERNAME, AppText.USERNAME, Keyboard.Text, true);
            emailInput = new FormInputField(AppText.EMAIL_ADDRESS, AppText.EMAIL_ADDRESS, Keyboard.Email, true);
            contentContainer.Children.Add(usernameInput.Content);
            contentContainer.Children.Add(emailInput.Content);
            #endregion

            // Initialise the buttons
            #region Buttons
            cancelButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE),
                Color.White, AppText.CANCEL, new Models.Action((int)Actions.ActionName.HideForgotPassword));
            cancelButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            cancelButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            cancelButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            submitButton = new Components.Buttons.ImageButton("arrow_right_green_chevron.png", "arrow_right_green_chevron.png",
                AppText.SUBMIT, Color.Black, new Models.Action((int)Actions.ActionName.HideForgotPassword));
            submitButton.RightAlign();
            submitButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);

            TouchEffect.SetNativeAnimation(cancelButton.Content, true);
            TouchEffect.SetNativeAnimation(submitButton.Content, true);

            buttonContainer.Children.Add(cancelButton.Content);
            buttonContainer.Children.Add(submitButton.Content);
            #endregion

            // Add the content to the containers
            #region Containers
            masterContainer.Children.Add(title.Content);
            masterContainer.Children.Add(contentContainer);
            masterContainer.Children.Add(buttonContainer);
            Container.Children.Add(masterContainer);
            Content.Children.Add(Container);
            #endregion
        }
    }
}
