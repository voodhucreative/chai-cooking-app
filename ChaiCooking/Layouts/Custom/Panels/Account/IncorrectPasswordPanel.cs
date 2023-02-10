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
    public class IncorrectPasswordPanel : StandardLayout
    {
        StackLayout ContentContainer;
        StackLayout ButtonsContainer;
        StackLayout WarningContainer;

        StaticImage WarningIcon;

        StaticLabel Title;
        StaticLabel Warning;
        StaticLabel DetailsDontMatch;
        StaticLabel PleaseTryAgain;
        StaticLabel WrongPasswordContact;
        StaticLabel ContactEmail;


        ColourButton OkButton;

        public IncorrectPasswordPanel()
        {
            Container = new Grid { };
            Content = new Grid { };
            ContentContainer = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.CenterAndExpand, Padding = Dimensions.GENERAL_COMPONENT_PADDING };

            ButtonsContainer = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = Dimensions.GENERAL_COMPONENT_SPACING };

            WarningContainer = new StackLayout { Orientation = StackOrientation.Horizontal, Margin = new Thickness(24, 16) };

            Title = new StaticLabel(AppText.RESET_PASSWORD_INFO);
            Title.Content.FontSize = Units.FontSizeM;
            Title.Content.TextColor = Color.FromHex(Colors.CC_DARK_GREY);
            Title.CenterAlign();

            WarningIcon = new StaticImage("pizza.png", 64, null);

            Warning = new StaticLabel(AppText.WARNING);
            Warning.Content.FontSize = Units.FontSizeM;
            Warning.Content.TextColor = Color.Black;
            Warning.LeftAlign();

            DetailsDontMatch = new StaticLabel(AppText.DETAILS_NOT_FOUND);
            DetailsDontMatch.Content.FontSize = Units.FontSizeL;
            DetailsDontMatch.Content.TextColor = Color.Black;
            DetailsDontMatch.CenterAlign();

            PleaseTryAgain = new StaticLabel(AppText.PLEASE_TRY_AGAIN);
            PleaseTryAgain.Content.FontSize = Units.FontSizeM;
            PleaseTryAgain.Content.TextColor = Color.FromHex(Colors.CC_DARK_GREY);
            PleaseTryAgain.CenterAlign();

            WrongPasswordContact = new StaticLabel(AppText.WRONG_PASSWORD_CONTACT_US);
            WrongPasswordContact.Content.FontSize = Units.FontSizeM;
            WrongPasswordContact.Content.TextColor = Color.FromHex(Colors.CC_DARK_GREY);
            WrongPasswordContact.CenterAlign();

            ContactEmail = new StaticLabel(AppText.CONTACT_EMAIL_ADDRESS);
            ContactEmail.Content.FontSize = Units.FontSizeL;
            ContactEmail.Content.TextColor = Color.Black;
            ContactEmail.CenterAlign();




            OkButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.OK, null);
            OkButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            OkButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            OkButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            

            WarningContainer.Children.Add(WarningIcon.Content);
            WarningContainer.Children.Add(DetailsDontMatch.Content);

            ButtonsContainer.Children.Add(OkButton.Content);

            //ContentContainer.Children.Add(Title.Content);
            ContentContainer.Children.Add(Warning.Content);
            ContentContainer.Children.Add(WarningContainer);
            ContentContainer.Children.Add(PleaseTryAgain.Content);
            ContentContainer.Children.Add(WrongPasswordContact.Content);
            ContentContainer.Children.Add(ContactEmail.Content);

            ContentContainer.Children.Add(ButtonsContainer);


            Container.Children.Add(ContentContainer);
            Content.Children.Add(Container);
        }
    }
}
