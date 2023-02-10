using System;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Composites;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom.Panels.Account;
using ChaiCooking.Models.Types;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class Legals : Page
    {

        StackLayout ContentContainer;

        StackLayout HeaderContainer;
        StaticLabel Title;
        StaticImage HeaderIcon;
        ActiveLabel CloseLabel;

        Grid Seperator;
        StackLayout TopSectionContainer;
        StackLayout InfoContainer;



        public Legals()
        {
            this.IsScrollable = true;
            this.IsRefreshable = true;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;


            this.Id = (int)AppSettings.PageNames.Legals;
            this.Name = AppData.AppText.ACCOUNT_PREFERENCES;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.SlideInFromRight;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.SlideOutToRight;

            PageContent = new Grid
            {
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY)
            };

            ContentContainer = BuildContent();
        }

        public StackLayout BuildContent()
        {
            ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING)
            };

            HeaderContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = Dimensions.GENERAL_COMPONENT_SPACING,
                Padding = new Thickness(0, Dimensions.GENERAL_COMPONENT_PADDING)
            };

            TopSectionContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            InfoContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_SPACING, Dimensions.GENERAL_COMPONENT_SPACING, Dimensions.GENERAL_COMPONENT_SPACING, Units.ScreenWidth)
            };

            HeaderIcon = new StaticImage("privacyicon.png", Dimensions.STANDARD_ICON_WIDTH, null);

            Title = new StaticLabel(AppData.AppText.PRIVACY_POLICY);
            Title.Content.TextColor = Color.White;
            Title.Content.FontSize = Units.FontSizeL;
            Title.Content.FontFamily = Fonts.GetBoldAppFont();
            Title.LeftAlign();

            CloseLabel = new ActiveLabel(AppText.CLOSE, Units.FontSizeM, Color.Transparent, Color.White, null);
            CloseLabel.CenterAlign();

            CloseLabel.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await App.PerformActionAsync((int)Actions.ActionName.GoToPage, AppSession.LastPageId);
                                await App.ShowMenu();
                            });
                        })
                    }
                );

            Seperator = new Grid { WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) };

            HeaderContainer.Children.Add(HeaderIcon.Content);
            HeaderContainer.Children.Add(Title.Content);
            HeaderContainer.Children.Add(CloseLabel.Content);
           
            

            foreach(AttributedText text in AppDataContent.PrivacyPolicyText)
            {
                StaticLabel line = new StaticLabel(text.TextContent);
                line.Content.FontFamily = text.TextFont;
                line.Content.FontSize = text.TextFontSize;
                line.Content.TextColor = text.TextColor;

                InfoContainer.Children.Add(line.Content);
            }

            ContentContainer.Children.Add(HeaderContainer);
            ContentContainer.Children.Add(Seperator);
            ContentContainer.Children.Add(TopSectionContainer);
            ContentContainer.Children.Add(InfoContainer);


            PageContent.Children.Add(ContentContainer);

            return ContentContainer;
        }
    }
}
