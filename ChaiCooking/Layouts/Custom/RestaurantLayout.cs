using System;
using TechExpo.Components;
using TechExpo.Components.Images;
using TechExpo.Components.Labels;
using TechExpo.Helpers;
using TechExpo.Models.Custom;
using Xamarin.Forms;

namespace TechExpo.Layouts.Custom
{
    public class RestaurantLayout : ActiveComponent
    {
        // model
        public Restaurant Restaurant;

        public StaticImage Logo { get; set; }
        public StaticLabel NameLabel { get; set; }
        public StaticImage LocationPin { get; set; }
        public StaticLabel TagLine { get; set; }
        public StaticLabel Reviews { get; set; }
        public StaticLabel DeliveryCost { get; set; }
        public StaticLabel MinimumOrder { get; set; }
        public StaticLabel Distance { get; set; }
        public StackLayout StarsContainer { get; set; }
        public RestaurantOptions RestaurantOptions;

        public RestaurantLayout(Restaurant restaurant)
        {
            this.Restaurant = restaurant;
            this.NameLabel = new StaticLabel(this.Restaurant.Name);
            this.NameLabel.Content.HeightRequest = 18;
            this.NameLabel.Content.FontSize = 14;
            this.NameLabel.Content.FontAttributes = FontAttributes.Bold;

            this.Logo = new StaticImage(this.Restaurant.LogoImageSource, 48, 48, null);
            this.TagLine = new StaticLabel(this.Restaurant.Description);
            this.Reviews = new StaticLabel(this.Restaurant.NumberOfReviews + " reviews");

            StarsContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Start
            };

            for (int i = 0; i < restaurant.StarRating; i++)
            {
                StaticImage star = new StaticImage("rating_icon.png", 16, 16, null);
                StarsContainer.Children.Add(star.Content);
            }

            StarsContainer.Children.Add(this.Reviews.Content);

            Content = new Grid
            {
                Padding = new Thickness(Units.ScreenUnitXS),
                VerticalOptions = LayoutOptions.Start,
            };

            RestaurantOptions = new RestaurantOptions(this.Restaurant.IsTableAvailable, this.Restaurant.IsCollectAvailable, this.Restaurant.IsDeliverAvailable, LayoutOptions.StartAndExpand);
            RestaurantOptions.Content.VerticalOptions = LayoutOptions.Start;

            Content.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
            Content.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Content.Children.Add(this.Logo.Content, 0, 0);

            Grid rightSide = new Grid
            {
                Padding = new Thickness(Units.ScreenUnitS, Units.ScreenUnitS)
            };
            rightSide.Children.Add(this.StarsContainer, 0, 0);
            rightSide.Children.Add(this.NameLabel.Content, 0, 1);
            rightSide.Children.Add(RestaurantOptions.Content, 0, 2);

            Content.Children.Add(rightSide, 1, 0);
        }
    }
}
