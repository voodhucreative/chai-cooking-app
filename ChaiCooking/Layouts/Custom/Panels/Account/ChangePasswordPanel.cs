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
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Panels.Account
{
    public class ChangePasswordPanel : StandardLayout
    {
        StackLayout ContentContainer;
        StackLayout ButtonsContainer;

        StaticLabel Title;

        FormInputField CurrentPasswordInput;
        FormInputField NewPasswordInput;
        FormInputField RepeatNewPasswordInput;
       

        ColourButton CancelButton;
        ColourButton SaveButton;

        public ChangePasswordPanel()
        {
            Container = new Grid { };
            Content = new Grid { };
            ContentContainer = new StackLayout { Orientation = StackOrientation.Vertical, Padding = Dimensions.GENERAL_COMPONENT_PADDING };

            ButtonsContainer = new StackLayout { Orientation = StackOrientation.Horizontal };

            Title = new StaticLabel(AppText.CHANGE_PASSWORD);
            Title.Content.FontSize = Units.FontSizeL;
            Title.Content.TextColor = Color.White;
            Title.LeftAlign();

            CurrentPasswordInput = new FormInputField(AppText.ENTER_CURRENT_PASSWORD, AppText.ENTER_CURRENT_PASSWORD, Keyboard.Text, false);
            CurrentPasswordInput.CenterAlign();
            CurrentPasswordInput.SetTitleColor(Color.White);
            CurrentPasswordInput.TextEntry.TextColor = Color.White;
            CurrentPasswordInput.TextEntry.WidthRequest = 240;
            CurrentPasswordInput.TextEntry.IsPassword = true;

            NewPasswordInput = new FormInputField(AppText.NEW_PASSWORD, AppText.NEW_PASSWORD, Keyboard.Text, false);
            NewPasswordInput.CenterAlign();
            NewPasswordInput.SetTitleColor(Color.White);
            NewPasswordInput.TextEntry.TextColor = Color.White;
            NewPasswordInput.TextEntry.WidthRequest = 240;
            NewPasswordInput.TextEntry.IsPassword = true;

            RepeatNewPasswordInput = new FormInputField(AppText.CONFIRM_NEW_PASSWORD, AppText.CONFIRM_NEW_PASSWORD, Keyboard.Text, false);
            RepeatNewPasswordInput.CenterAlign();
            RepeatNewPasswordInput.SetTitleColor(Color.White);
            RepeatNewPasswordInput.TextEntry.TextColor = Color.White;
            RepeatNewPasswordInput.TextEntry.WidthRequest = 240;
            RepeatNewPasswordInput.TextEntry.IsPassword = true;

            CancelButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CANCEL, null);
            CancelButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            CancelButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            CancelButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            CancelButton.LeftAlign();

            SaveButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.SAVE_CHANGES, null);
            SaveButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            SaveButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            SaveButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            SaveButton.RightAlign();

            ButtonsContainer.HorizontalOptions = LayoutOptions.CenterAndExpand;
            ButtonsContainer.Children.Add(CancelButton.Content);
            ButtonsContainer.Children.Add(SaveButton.Content);

            ContentContainer.Children.Add(Title.Content);
            ContentContainer.Children.Add(CurrentPasswordInput.Content);
            ContentContainer.Children.Add(NewPasswordInput.Content);
            ContentContainer.Children.Add(RepeatNewPasswordInput.Content);
            ContentContainer.Children.Add(ButtonsContainer);

            Container.Children.Add(ContentContainer);
            Content.Children.Add(Container);
        }
    }
}

