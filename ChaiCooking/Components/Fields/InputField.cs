using System;
using ChaiCooking.Branding;
using ChaiCooking.Helpers;
using Xamarin.Forms;
using static ChaiCooking.Helpers.Fonts;

namespace ChaiCooking.Components.Fields
{
    public class InputField : ActiveComponent
    {
        public CustomEntry TextEntry { get; set; }

        public InputField(string title, string placeholder, Keyboard keyboard, bool required)
        {
            var titleString = new FormattedString();
            titleString.Spans.Add(new Span { Text = title, ForegroundColor = Color.Black });

            if (required)
            {
                titleString.Spans.Add(new Span { Text = " *", ForegroundColor = Color.FromHex(Colors.CC_MUSTARD) });
            }
            Container = new Grid { };
            Container.HeightRequest = Units.TapSizeM;
            Container.WidthRequest = Units.LargeButtonWidth;
            Container.VerticalOptions = LayoutOptions.CenterAndExpand;
            
            Content = new Grid
            {
                //HeightRequest = Units.InputHeight,
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = Units.TapSizeM,
                WidthRequest = Units.LargeButtonWidth,
                //Padding = new Thickness(Units.ScreenUnitM, 0)

            };

            TextEntry = new CustomEntry
            {
                FontFamily = ChaiCooking.Helpers.Fonts.GetFont(FontName.MuliRegular),
                Placeholder = placeholder,
                PlaceholderColor = Color.Gray,
                TextColor = Color.Black,
                Keyboard = keyboard,
                FontSize = 16,
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsPassword = false,
                HorizontalTextAlignment = TextAlignment.Center,
                //Margin = new Thickness(Units.ScreenUnitXS, Units.ScreenUnitXS),
                HeightRequest = Units.TapSizeM,
                WidthRequest = Units.LargeButtonWidth,
                
            };

            //if (Device.RuntimePlatform == Device.iOS)
            //{
                int marginX = (int)((Units.ScreenWidth - Units.LargeButtonWidth)/2);
                Console.WriteLine("Screen width: " + Units.ScreenWidth + ", Screen height: " + Units.ScreenHeight + ", Button width: " + Units.LargeButtonWidth + ", Margin X: " + marginX);
                Content.Padding = 0;
                Content.Margin = new Thickness((marginX/2), Units.ScreenUnitXS);
                TextEntry.Margin = 0;

            //}
            

            Content.Children.Add(TextEntry, 0, 0);
        }

        public void SetVerticalLayout()
        {

        }


    }
}
