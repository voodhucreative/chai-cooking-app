using System;
using System.Collections.Generic;
using ChaiCooking.Components.Images;
using FFImageLoading.Transformations;
using Xamarin.Forms;

namespace ChaiCooking.Components.Labels
{
    public class IconLabel
    {
        public Grid Content { get; set; }
        public StackLayout Container { get; set; }
        public StaticImage Icon { get; set; }
        public StaticLabel TextContent { get; set; }

        public IconLabel(string iconImageSource, string text, int width, int height, List<FFImageLoading.Work.ITransformation> transformations = null)
        {
            Content = new Grid
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = width,
                HeightRequest = height
            };

            Container = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Icon = new StaticImage(iconImageSource, height, height, transformations);

            Icon.Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            TextContent = new StaticLabel(text);
            TextContent.LeftAlign();

            Container.Children.Add(Icon.Content);
            Container.Children.Add(TextContent.Content);

            Content.Children.Add(Container);


        }

        public void SetIconSize(int width, int height)
        {
            Icon.Content.WidthRequest = width;
            Icon.Content.HeightRequest = height;
        }

        public void SetIconSize(int width, int height, int margin)
        {
            Icon.Content.WidthRequest = width;
            Icon.Content.HeightRequest = height;
            Icon.Content.Margin = margin;
        }

        public void CenterAlignVertical()
        {
            Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            Container.VerticalOptions = LayoutOptions.CenterAndExpand;
            Icon.CenterAlignVertical();
            TextContent.CenterAlignVertical();

        }

        public void SetIconLeft()
        {
            Container.Children.Clear();
            Container.Children.Add(Icon.Content);
            Container.Children.Add(TextContent.Content);
            TextContent.LeftAlign();
        }

        public void SetIconRight()
        {
            Container.Children.Clear();
            Container.Children.Add(TextContent.Content);
            Container.Children.Add(Icon.Content);
            TextContent.RightAlign();
        }

        public void SetIconColour(string colour)
        {
            TintTransformation colorTint = new TintTransformation
            {
                HexColor = colour,
                EnableSolidColor = true

            };
            Icon.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
            Icon.Content.Transformations.Add(colorTint);
        }
    }
}
