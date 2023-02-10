using System;
using ChaiCooking.Components;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom
{
    public class TopSliderItemLayout : ActiveComponent
    {
        // model
        //public Category Category;

        public StaticImage Logo { get; set; }
        public StaticLabel NameLabel { get; set; }
        public StaticLabel Link { get; set; }


        public TopSliderItemLayout(string logoImageSource, string name, string link)
        {
            this.NameLabel = new StaticLabel(name);
            this.NameLabel.Content.HorizontalOptions = LayoutOptions.Center;

            this.Link = new StaticLabel(link);
            this.Link.Content.HorizontalOptions = LayoutOptions.Center;
            this.Link.Content.FontSize = 6;

            this.Logo = new StaticImage(logoImageSource, 64, 64, null);

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

            ContentContainer.Children.Add(this.NameLabel.Content);
            ContentContainer.Children.Add(this.Logo.Content);
            //ContentContainer.Children.Add(this.Link.Content);
            Content.Children.Add(ContentContainer);
        }
    }
}

