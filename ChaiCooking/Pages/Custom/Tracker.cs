using System;
using ChaiCooking.Branding;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class Tracker : Page
    {
        StackLayout ContentContainer;

        StaticLabel Title;

        public Tracker()
        {
            this.IsScrollable = true;
            this.IsRefreshable = true;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;


            this.Id = (int)AppSettings.PageNames.Tracker;
            this.Name = AppData.AppText.TRACKER;
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

            Title = new StaticLabel(AppData.AppText.TRACKER + ".");
            Title.Content.TextColor = Color.White;
            Title.Content.FontSize = Units.FontSizeXXL;
            Title.Content.Padding = Dimensions.GENERAL_COMPONENT_SPACING;
            Title.CenterAlign();

            ContentContainer.Children.Add(Title.Content);

            Grid Seperator = new Grid { WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) };
            StaticLabel ComingSoon = new StaticLabel("Coming soon..");
            ComingSoon.Content.TextColor = Color.White;
            ComingSoon.Content.FontFamily = Fonts.GetRegularAppFont();
            ComingSoon.CenterAlign();

            ContentContainer.Children.Add(Seperator);
            ContentContainer.Children.Add(ComingSoon.Content);




            PageContent.Children.Add(ContentContainer);

            return ContentContainer;
        }
    }
}
