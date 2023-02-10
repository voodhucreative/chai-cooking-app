using System;
using ChaiCooking.Helpers;
using Xamarin.Forms;
using XFShapeView;

namespace ChaiCooking.Components.Composites
{
    public class DividerWithText
    {
        public Grid Content;

        public DividerWithText(Color color, Color bgcolor, double opacity, string text)
        {
            Content = new Grid
            {
                HeightRequest = 16,
                Margin = 0,
                Padding = 0,
                BackgroundColor = Color.Transparent,
                Opacity = 1,
                VerticalOptions = LayoutOptions.Center
            };

            Content.RowDefinitions.Add(new RowDefinition { Height = new GridLength(15) });
            Content.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            ShapeView line = new ShapeView
            {
                ShapeType = ShapeType.Box,
                HeightRequest = 4,
                WidthRequest = Units.ScreenWidth,
                Color = color,
                HorizontalOptions = LayoutOptions.Center,
                CornerRadius = 2,
                Margin = new Thickness(Units.ScreenUnitXS, 7),
                Opacity = opacity
            };

            StackLayout labelHolder = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = bgcolor
            };

            //labelHolder.Margin= new Thickness(4, 0);
            labelHolder.Padding = new Thickness(8, 0);
            Label dividertext = new Label
            {
                Text = text,
                BackgroundColor = bgcolor,
                TextColor = color
            };

            //dividertext.Margin = new Thickness(8);

            dividertext.FontSize = Units.FontSizeM;
            dividertext.BackgroundColor = bgcolor;
            dividertext.Opacity = opacity;

            labelHolder.Children.Add(dividertext);
            Content.Children.Add(line, 0, 0);
            Content.Children.Add(labelHolder, 0, 0);
        }
    }
}
