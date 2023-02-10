using System;
using TechExpo.Components;
using TechExpo.Components.Images;
using TechExpo.Components.Labels;
using TechExpo.Helpers;
using TechExpo.Models.Custom;
using Xamarin.Forms;

namespace TechExpo.Layouts.Custom
{
    public class TrendingSliderTileLayout : ActiveComponent
    {
        // model
        //public Category Category;

        public StaticImage Logo { get; set; }
        public StaticLabel NameLabel { get; set; }
        public StaticLabel Link { get; set; }
        public StackLayout StarsContainer { get; set; }
        public StaticLabel LastUsedGlobally { get; set; }
        public StaticLabel LastUsedByUser { get; set; }

        public TrendingSliderTileLayout(Restaurant restaurant)
        {
            this.NameLabel = new StaticLabel(restaurant.Name);
            this.NameLabel.Content.HorizontalOptions = LayoutOptions.Center;
            this.NameLabel.Content.FontFamily = TechExpo.Helpers.Fonts.GetFont(FontName.MuliBold);
            this.NameLabel.Content.FontSize = 10;


            this.Link = new StaticLabel(restaurant.Description);
            this.Link.Content.HorizontalOptions = LayoutOptions.Center;
            this.Link.Content.FontSize = 8;

            this.LastUsedGlobally = new StaticLabel(restaurant.LastUsedGlobally);
            this.LastUsedGlobally.Content.HorizontalOptions = LayoutOptions.Center;
            this.LastUsedGlobally.Content.FontFamily = TechExpo.Helpers.Fonts.GetFont(FontName.MuliRegular);
            this.LastUsedGlobally.Content.FontSize = 10;

            this.Logo = new StaticImage(restaurant.LogoImageSource, 64, 64, null);

            Content = new Grid
            {
                BackgroundColor = Color.White,
                WidthRequest = Units.ScreenWidth30Percent,
                HeightRequest = Units.ScreenWidth30Percent,
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(Units.ScreenUnitXS)
            };

            StackLayout ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical
            };

            StackLayout StarContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center
            };

            for (int i = 0; i < restaurant.StarRating; i++)
            {
                StaticImage star = new StaticImage("rating_icon.png", 16, 16, null);
                StarContainer.Children.Add(star.Content);
            }

            ContentContainer.Children.Add(this.Logo.Content);
            //ContentContainer.Children.Add(StarContainer);
            ContentContainer.Children.Add(this.LastUsedGlobally.Content);
            ContentContainer.Children.Add(this.NameLabel.Content);
            //ContentContainer.Children.Add(this.Link.Content);
            Content.Children.Add(ContentContainer);
        }
    }
}


