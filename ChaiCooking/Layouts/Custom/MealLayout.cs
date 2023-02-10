using System;
using TechExpo.Components;
using TechExpo.Components.Images;
using TechExpo.Components.Labels;
using TechExpo.Helpers;
using TechExpo.Models.Custom;
using Xamarin.Forms;

namespace TechExpo.Layouts.Custom
{
    public class MealLayout : ActiveComponent
    {
        // model
        public Meal Meal;
        public StaticImage Logo { get; set; }
        public StaticImage Background { get; set; }
        public StaticLabel NameLabel { get; set; }
        public StaticLabel TagLine { get; set; }
        public StaticLabel Description { get; set; }
        public StaticLabel Price { get; set; }

        public MealLayout(Meal meal)
        {
            this.Meal = meal;

            this.NameLabel = new StaticLabel(this.Meal.Name);
            this.Logo = new StaticImage(this.Meal.LogoImageSource, 48, 48, null);
            this.Background = new StaticImage(this.Meal.LogoImageSource, 48, 48, null);

            this.TagLine = new StaticLabel("Tag line");
            this.Description = new StaticLabel("Description text");
            this.Price = new StaticLabel("£" + this.Meal.Price);

            StackLayout container = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            Content = new Grid
            {
                WidthRequest = Units.ScreenWidth,
                //Padding = Units.ScreenUnitXS
            };

            this.NameLabel.Content.VerticalTextAlignment = TextAlignment.Center;
            this.NameLabel.Content.VerticalOptions = LayoutOptions.Center;

            this.Logo.Content.Aspect = Aspect.AspectFit;
            this.Logo.Content.HorizontalOptions = LayoutOptions.Start;
            this.Logo.Content.WidthRequest = Units.TapSizeXL;
            this.Logo.Content.HeightRequest = Units.TapSizeXL;

            this.Background.Content.Aspect = Aspect.Fill;
            this.Background.Content.Opacity = 0.12;

            container.Children.Add(this.Logo.Content);
            container.Children.Add(this.NameLabel.Content);

            Content.Children.Add(this.Background.Content, 0, 0);

            Content.Children.Add(container, 0, 0);



        }
    }
}
