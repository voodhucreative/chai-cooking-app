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
    public class PasswordSentPanel : StandardLayout
    {
        StackLayout ContentContainer;
        StackLayout ButtonsContainer;
        StackLayout SuccessContainer;

        StaticImage SuccessIcon;

        StaticLabel Title;
        
        StaticLabel EmailSent;
        
        StaticLabel IfFailedTryAgain;
        


        ColourButton OkButton;

        public PasswordSentPanel()
        {
            Container = new Grid { };
            Content = new Grid { };
            ContentContainer = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.CenterAndExpand, Padding = Dimensions.GENERAL_COMPONENT_PADDING };

            ButtonsContainer = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = Dimensions.GENERAL_COMPONENT_SPACING };

            SuccessContainer = new StackLayout { Orientation = StackOrientation.Horizontal, Margin = new Thickness(24, 16) };

            Title = new StaticLabel(AppText.RESET_PASSWORD_INFO);
            Title.Content.FontSize = Units.FontSizeM;
            Title.Content.TextColor = Color.FromHex(Colors.CC_DARK_GREY);
            Title.CenterAlign();

            SuccessIcon = new StaticImage("pizza.png", 64, null);

            EmailSent = new StaticLabel(AppText.PASSWORD_SENT);
            EmailSent.Content.FontSize = Units.FontSizeL;
            EmailSent.Content.TextColor = Color.Black;
            EmailSent.CenterAlign();

            IfFailedTryAgain = new StaticLabel(AppText.PASSWORD_NOT_RECEIVED_INSTRUCTIONS);
            IfFailedTryAgain.Content.FontSize = Units.FontSizeM;
            IfFailedTryAgain.Content.TextColor = Color.FromHex(Colors.CC_DARK_GREY);
            IfFailedTryAgain.CenterAlign();

            OkButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.OK, null);
            OkButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            OkButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            OkButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;


            SuccessContainer.Children.Add(SuccessIcon.Content);
            SuccessContainer.Children.Add(EmailSent.Content);

            ButtonsContainer.Children.Add(OkButton.Content);

            ContentContainer.Children.Add(SuccessContainer);
            ContentContainer.Children.Add(IfFailedTryAgain.Content);

            ContentContainer.Children.Add(ButtonsContainer);


            Container.Children.Add(ContentContainer);
            Content.Children.Add(Container);
        }
    }
}
