using System;
using TechExpo.Components;
using TechExpo.Components.Images;
using TechExpo.Components.Labels;
using TechExpo.Helpers;
using TechExpo.Models.Custom;
using Xamarin.Forms;

namespace TechExpo.Layouts.Custom
{
    public class RestaurantSummaryLayout : ActiveComponent
    {
        // model
        public Restaurant Restaurant;

        public StaticImage Logo { get; set; }
        public StaticLabel NameLabel { get; set; }
        public StaticImage LocationPin { get; set; }
        public StaticLabel TagLine { get; set; }
        public StaticLabel DeliveryCost { get; set; }
        public StaticLabel MinimumOrder { get; set; }
        public StaticLabel Distance { get; set; }
        public StaticLabel Reviews { get; set; }
        public StaticLabel OpenStatus { get; set; }
        public StaticLabel DeliveryFee { get; set; }

        public RestaurantSummaryLayout(Restaurant restaurant)
        {
            this.Restaurant = restaurant;
            this.NameLabel = new StaticLabel(this.Restaurant.Name);
            this.NameLabel.Content.FontFamily = TechExpo.Helpers.Fonts.GetFont(FontName.MuliBold);
            this.NameLabel.Content.FontSize = 16;

            this.Logo = new StaticImage(this.Restaurant.LogoImageSource, Units.TapSizeXL, Units.TapSizeXL, null);
            this.TagLine = new StaticLabel(this.Restaurant.Description);

            this.Reviews = new StaticLabel(this.Restaurant.NumberOfReviews + " reviews");
            this.Reviews.Content.FontFamily = TechExpo.Helpers.Fonts.GetFont(FontName.MuliRegular);
            this.Reviews.Content.FontSize = 10;
            this.Reviews.Content.VerticalOptions = LayoutOptions.Center;

            this.OpenStatus = new StaticLabel("Closed");
            this.OpenStatus.Content.FontFamily = TechExpo.Helpers.Fonts.GetFont(FontName.MuliBold);
            this.OpenStatus.Content.FontSize = 14;
            this.OpenStatus.Content.VerticalOptions = LayoutOptions.Center;

            this.DeliveryFee = new StaticLabel("Delivery fee: " + this.Restaurant.DeliveryFee);
            this.DeliveryFee.Content.FontFamily = TechExpo.Helpers.Fonts.GetFont(FontName.MuliRegular);
            this.DeliveryFee.Content.FontSize = 12;
            this.DeliveryFee.Content.VerticalOptions = LayoutOptions.Start;

            if (this.Restaurant.IsOpenNow)
            {
                this.OpenStatus.Content.TextColor = Color.Green;
                this.OpenStatus.Content.Text = "Open";
            }
            else
            {
                this.OpenStatus.Content.TextColor = Color.Red;
                this.OpenStatus.Content.Text = "Closed";
            }

            Content = new Grid
            {
                Padding = Units.ScreenUnitM
            };

            StackLayout StarContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Start
            };

            for (int i = 0; i < restaurant.StarRating; i++)
            {
                StaticImage star = new StaticImage("rating_icon.png", 16, 16, null);
                StarContainer.Children.Add(star.Content);
            }



            Content.Children.Add(this.Logo.Content, 0, 0);
            Content.Children.Add(this.NameLabel.Content, 1, 0);
            Content.Children.Add(StarContainer, 1, 1);
            Content.Children.Add(this.Reviews.Content, 2, 1);

            //Content.Children.Add(this.TagLine.Content, 2, 0);
            Content.Children.Add(this.OpenStatus.Content, 1, 2);
            Content.Children.Add(this.DeliveryFee.Content, 1, 3);

            Grid.SetRowSpan(this.Logo.Content, 4);





        }
    }
}
