using System;
using Xamarin.Forms;

namespace ChaiCooking.Pages
{
    public class ErrorPage : ContentPage
    {
        StackLayout Items;

        public ErrorPage(string error)
        {
            Items = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Padding = 16,
                Spacing = 16,
                BackgroundColor = Color.Transparent,
            };

            Items.Children.Add(new Image { WidthRequest = 52, HeightRequest = 52, Source = "chaiface.png" });
            Items.Children.Add(new Label { FontSize = 14, Text = error, TextColor = Color.Black, VerticalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center });

            Content = new Grid
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Padding = 32,
                BackgroundColor = Color.Transparent,
                Children = { Items }
            }; 
        }
    }
}
