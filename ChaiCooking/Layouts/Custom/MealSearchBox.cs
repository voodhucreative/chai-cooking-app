﻿using System;
using TechExpo.Components;
using TechExpo.Components.Fields;
using TechExpo.Components.Images;
using TechExpo.Helpers;
using Xamarin.Forms;
using XFShapeView;

namespace TechExpo.Layouts.Custom
{
    public class LocationSearchBox : ActiveComponent
    {
        SimpleInputField PostCodeField;
        ActiveImage CurrentLocation;
        ActiveImage Search;
        ShapeView SearchBoxContainerOuter;
        ShapeView SearchBoxContainerInner;
        StackLayout ContentContainer;
        ShapeView Divider1;
        ShapeView Divider2;

        public LocationSearchBox()
        {
            Content = new Grid
            {
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.TapSizeM,
                BackgroundColor = Color.Transparent,
                Margin = new Thickness(Units.ScreenUnitXS)
            };

            SearchBoxContainerOuter = new ShapeView
            {
                ShapeType = ShapeType.Box,
                HeightRequest = 4,
                WidthRequest = Units.ScreenWidth,
                Color = Color.Gray,
                HorizontalOptions = LayoutOptions.Center,
                CornerRadius = 5,
                Opacity = 1
            };

            SearchBoxContainerInner = new ShapeView
            {
                ShapeType = ShapeType.Box,
                HeightRequest = 4,
                WidthRequest = Units.ScreenWidth,
                Color = Color.White,
                HorizontalOptions = LayoutOptions.Center,
                CornerRadius = 5,
                Opacity = 1,
                Padding = 1
            };

            Divider1 = new ShapeView
            {
                ShapeType = ShapeType.Box,
                HeightRequest = Units.TapSizeM,
                WidthRequest = 1,
                Color = Color.Gray,
                HorizontalOptions = LayoutOptions.Start,
            };

            Divider2 = new ShapeView
            {
                ShapeType = ShapeType.Box,
                HeightRequest = Units.TapSizeM,
                WidthRequest = 1,
                Color = Color.Gray,
                HorizontalOptions = LayoutOptions.Start,
            };

            CurrentLocation = new ActiveImage("location_icon.png", Units.TapSizeXS, Units.TapSizeXS, null, null);
            CurrentLocation.Content.Padding = new Thickness(8, 0);

            Search = new ActiveImage("search_icon.png", Units.TapSizeXS, Units.TapSizeXS, null, null);
            Search.Content.Padding = new Thickness(8, 0, 16, 0);

            ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };

            PostCodeField = new SimpleInputField("Post Code", Keyboard.Text);
            PostCodeField.Content.WidthRequest = Units.HalfScreenWidth;


            ContentContainer.Children.Add(PostCodeField.Content);
            ContentContainer.Children.Add(Divider1);
            ContentContainer.Children.Add(CurrentLocation.Content);
            ContentContainer.Children.Add(Divider2);
            ContentContainer.Children.Add(Search.Content);

            Content.Children.Add(SearchBoxContainerOuter, 0, 0);
            Content.Children.Add(SearchBoxContainerInner, 0, 0);
            Content.Children.Add(ContentContainer, 0, 0);


        }
    }
}
