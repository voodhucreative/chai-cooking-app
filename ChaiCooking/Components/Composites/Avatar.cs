using System;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using Xamarin.Forms;

namespace ChaiCooking.Components.Composites
{
    public class Avatar : ActiveComponent
    {
        public StackLayout ContentContainer { get; set; }
        public ActiveImage Icon { get; set; }
        public ActiveLabel Title { get; set; }

        public Avatar(string title, string iconImageSource, int width, int height)
        {
            Content = new Grid
            {
                BackgroundColor = Color.Transparent,
                WidthRequest = width,
                HeightRequest = height,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Icon = new ActiveImage(iconImageSource, width, height, null, null);
            Icon.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            Icon.Image.HorizontalOptions = LayoutOptions.CenterAndExpand;
            Icon.Image.BackgroundColor = Color.Transparent;
            

            Title = new ActiveLabel(title, Units.FontSizeM, Color.Transparent, Color.White, null);
            Title.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            Title.Container.HorizontalOptions = LayoutOptions.CenterAndExpand;
            Title.Label.HorizontalOptions = LayoutOptions.CenterAndExpand;
            Title.Label.HorizontalTextAlignment = TextAlignment.Center;

            ContentContainer.Children.Add(Icon.Content);
            ContentContainer.Children.Add(Title.Content);

            Content.Children.Add(ContentContainer);

        }
    }
}
