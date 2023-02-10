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
    public class ForgotPasswordPanel : StandardLayout
    {
        StackLayout ContentContainer;
        StackLayout ButtonsContainer;
        StaticLabel Title;
        

        FormInputField UsernameInput;
        FormInputField EmailInput;

        private const int CANCEL_CHOICE = 0;
        private const int CANCEL_CONFIRM = 1;

        ColourButton CancelButton;
        //ColourButton SubmitButton;
        Components.Buttons.ImageButton SubmitButton;


        public ForgotPasswordPanel()
        {
            Container = new Grid { };
            Content = new Grid { };
            StackLayout subContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            ContentContainer = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.CenterAndExpand, Padding = Dimensions.GENERAL_COMPONENT_PADDING };

            ButtonsContainer = new StackLayout { Orientation = StackOrientation.Horizontal, Padding = Dimensions.GENERAL_COMPONENT_PADDING };

            Title = new StaticLabel(AppText.RESET_PASSWORD_INFO);
            Title.Content.FontSize = Units.FontSizeM;
            Title.Content.TextColor = Color.FromHex(Colors.CC_DARK_GREY);
            Title.CenterAlign();

            UsernameInput = new FormInputField(AppText.USERNAME, AppText.USERNAME, Keyboard.Text, true);
            EmailInput = new FormInputField(AppText.EMAIL_ADDRESS, AppText.EMAIL_ADDRESS, Keyboard.Email, true);
            subContainer.Children.Add(UsernameInput.TextEntry);
            subContainer.Children.Add(EmailInput.TextEntry);

            CancelButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE),
                Color.White, AppText.CANCEL, new Models.Action((int)Actions.ActionName.HideForgotPassword));
            CancelButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            CancelButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            CancelButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            SubmitButton = new Components.Buttons.ImageButton("arrow_right_green_chevron.png", "arrow_right_green_chevron.png", AppText.SUBMIT, Color.Black, new Models.Action((int)Actions.ActionName.HideForgotPassword));
            SubmitButton.RightAlign();
            SubmitButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);

            ButtonsContainer.Children.Add(CancelButton.Content);
            ButtonsContainer.Children.Add(SubmitButton.Content);

            ContentContainer.Children.Add(Title.Content);
            ContentContainer.Children.Add(subContainer);
            ContentContainer.Children.Add(ButtonsContainer);
            Container.Children.Add(ContentContainer);
            Content.Children.Add(Container);
      
            //Container = new Grid { };
            //Content = new Grid { };
            //ContentContainer = new StackLayout { Orientation = StackOrientation.Vertical, Padding = Dimensions.GENERAL_COMPONENT_PADDING };

            //ButtonsContainer = new StackLayout { Orientation = StackOrientation.Vertical };

            //Title = new StaticLabel(AppText.RESET_PASSWORD_INFO);
            //Title.Content.FontSize = Units.FontSizeM;
            //Title.Content.TextColor = Color.FromHex(Colors.CC_DARK_GREY);
            //Title.CenterAlign();

            //UsernameInput = new FormInputField(AppText.USERNAME, AppText.USERNAME, Keyboard.Text, true);
            //EmailInput = new FormInputField(AppText.EMAIL_ADDRESS, AppText.EMAIL_ADDRESS, Keyboard.Email, true);

            //CancelButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CANCEL, new Models.Action((int)Actions.ActionName.HideForgotPassword));
            //CancelButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            //CancelButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            //CancelButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            //SubmitButton = new Components.Buttons.ImageButton("arrow_right_green_chevron.png", "arrow_right_green_chevron.png", AppText.SUBMIT, Color.Black, new Models.Action((int)Actions.ActionName.HideForgotPassword));
            //SubmitButton.RightAlign();
            //SubmitButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);

            //ButtonsContainer.Children.Add(CancelButton.Content);
            //ButtonsContainer.Children.Add(SubmitButton.Content);

            //ContentContainer.Children.Add(Title.Content);
            //ContentContainer.Children.Add(EmailInput.Content);
            //ContentContainer.Children.Add(UsernameInput.Content);

            //ContentContainer.Children.Add(ButtonsContainer);


            //Container.Children.Add(ContentContainer);
            //Content.Children.Add(Container);

        }

        private void Btn_Clicked(object sender, EventArgs e)
        {
            this.ContentContainer.IsVisible = false;
        }

        public void SetLayoutHeight(int height)
        {
            Content.HeightRequest = height;
        }
    }

}
