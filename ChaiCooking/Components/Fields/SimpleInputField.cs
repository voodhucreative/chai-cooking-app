using System;
using ChaiCooking.Branding;
using ChaiCooking.Helpers;
using Xamarin.Forms;
using static ChaiCooking.Helpers.Fonts;

namespace ChaiCooking.Components.Fields
{
    public class SimpleInputField : ActiveComponent
    {

        public Label RequiredStar { get; set; }
        public CustomEntry TextEntry { get; set; }

        public SimpleInputField(string placeholder, Keyboard keyboard)
        {
            Content = new Grid
            {
                HeightRequest = Units.InputHeight,
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(Units.ScreenUnitXS, 0),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            TextEntry = new CustomEntry
            {
                FontFamily = ChaiCooking.Helpers.Fonts.GetFont(FontName.MuliRegular),
                Placeholder = placeholder,
                PlaceholderColor = Color.Gray,
                TextColor = Color.Black,
                Keyboard = keyboard,
                FontSize = 24,
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.End,
                IsPassword = false,
            };

            Content.Children.Add(TextEntry, 0, 0);
        }
    }
}
