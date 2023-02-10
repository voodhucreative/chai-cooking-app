using System;
using TechExpo.Components;
using TechExpo.Components.Images;
using TechExpo.Components.Labels;
using TechExpo.Helpers;
using TechExpo.Models.Custom;
using Xamarin.Forms;

namespace TechExpo.Layouts.Custom
{
    public class OptionLayout : ActiveComponent
    {
        // model
        public Option Option;

        public StaticImage Logo { get; set; }
        public StaticLabel NameLabel { get; set; }
        public StaticLabel TagLine { get; set; }
        public StaticLabel Description { get; set; }
        public StaticLabel Price { get; set; }

        public OptionLayout(Option option)
        {
            this.Option = option;
            this.NameLabel = new StaticLabel(this.Option.Name);
            this.Logo = new StaticImage(this.Option.LogoImageSource, 48, 48, null);

            StackLayout container = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            Content = new Grid
            {
                WidthRequest = Units.ScreenWidth,
                Padding = Units.ScreenUnitXS
            };

            this.NameLabel.Content.VerticalTextAlignment = TextAlignment.Center;
            this.NameLabel.Content.VerticalOptions = LayoutOptions.Center;

            this.Logo.Content.Aspect = Aspect.AspectFit;
            this.Logo.Content.HorizontalOptions = LayoutOptions.Start;
            this.Logo.Content.WidthRequest = Units.TapSizeXL;
            this.Logo.Content.HeightRequest = Units.TapSizeXL;

            container.Children.Add(this.Logo.Content);
            container.Children.Add(this.NameLabel.Content);

            Content.Children.Add(container);
        }
    }
}
