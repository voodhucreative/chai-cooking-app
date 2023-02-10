using System;
using System.Collections.Generic;
using TechExpo.Helpers;
using Xamarin.Forms;
using XFShapeView;

namespace TechExpo.Layouts.Custom
{
    public class RestaurantOptions
    {
        public Grid Content { get; set; }
        private StackLayout OptionsContainer;
        private Label TableLabel;
        private Label CollectLabel;
        private Label DeliverLabel;

        private ShapeView DividerDot1;
        private ShapeView DividerDot2;
        private List<Label> OptionsLabels;

        public RestaurantOptions(bool table, bool collect, bool deliver, LayoutOptions horizontalOptions)
        {
            OptionsLabels = new List<Label>();

            Content = new Grid
            {
                BackgroundColor = Color.White,
                HorizontalOptions = horizontalOptions,
                VerticalOptions = LayoutOptions.Center
            };

            OptionsContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = horizontalOptions,
                VerticalOptions = LayoutOptions.Center
            };

            TableLabel = new Label
            {
                FontFamily = Fonts.GetFont(FontName.MuliLight),
                TextColor = Color.Black,
                Text = "Table",
                VerticalOptions = LayoutOptions.Center
            };

            CollectLabel = new Label
            {
                FontFamily = Fonts.GetFont(FontName.MuliLight),
                TextColor = Color.Black,
                Text = "Collect",
                VerticalOptions = LayoutOptions.Center
            };

            DeliverLabel = new Label
            {
                FontFamily = Fonts.GetFont(FontName.MuliLight),
                TextColor = Color.Black,
                Text = "Deliver",
                VerticalOptions = LayoutOptions.Center
            };

            DividerDot1 = new ShapeView
            {
                ShapeType = ShapeType.Circle,
                HeightRequest = 6,
                WidthRequest = 6,
                Color = Color.Black,
                HorizontalOptions = LayoutOptions.Center,
                CornerRadius = 5,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(8, 0)
            };

            DividerDot2 = new ShapeView
            {
                ShapeType = ShapeType.Circle,
                HeightRequest = 6,
                WidthRequest = 6,
                Color = Color.Black,
                HorizontalOptions = LayoutOptions.Center,
                CornerRadius = 5,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(8, 0)
            };

            if (table) { OptionsLabels.Add(TableLabel); }
            if (collect) { OptionsLabels.Add(CollectLabel); }
            if (deliver) { OptionsLabels.Add(DeliverLabel); }

            switch (OptionsLabels.Count)
            {
                case 1:
                    OptionsContainer.Children.Add(OptionsLabels[0]);
                    break;
                case 2:
                    OptionsContainer.Children.Add(OptionsLabels[0]);
                    OptionsContainer.Children.Add(DividerDot1);
                    OptionsContainer.Children.Add(OptionsLabels[1]);
                    break;
                case 3:
                    OptionsContainer.Children.Add(OptionsLabels[0]);
                    OptionsContainer.Children.Add(DividerDot1);
                    OptionsContainer.Children.Add(OptionsLabels[1]);
                    OptionsContainer.Children.Add(DividerDot2);
                    OptionsContainer.Children.Add(OptionsLabels[2]);
                    break;
                default:
                    break;
            }
            Content.Children.Add(OptionsContainer, 0, 0);
        }
    }
}
