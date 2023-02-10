﻿using System;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.CreateOrSelect
{
    public class CreateOrSelectViewHeader : Grid
    {
        public static readonly BindableProperty EmptyViewProperty =
           BindableProperty.Create("EmptyView", typeof(StackLayout), typeof(CreateOrSelectViewHeader));

        public static readonly BindableProperty EmptyViewVisibleProperty =
            BindableProperty.Create("EmptyViewIsVisible", typeof(bool), typeof(CreateOrSelectViewHeader), defaultValue: true);

        public bool EmptyViewIsVisible
        {
            get { return (bool)GetValue(EmptyViewVisibleProperty); }
            set { SetValue(EmptyViewVisibleProperty, value); }
        }

        public StackLayout EmptyView
        {
            get { return (StackLayout)GetValue(EmptyViewProperty); }
            set { SetValue(EmptyViewProperty, value); }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                this.HeightRequest = EmptyViewIsVisible ? 50 : 0;
                this.WidthRequest = EmptyViewIsVisible ? Units.ScreenWidth : 0;
            }
        }

        public CreateOrSelectViewHeader()
        {
            this.WidthRequest = Units.ScreenWidth;
            Children.Add(BuildEmpty());
        }

        private StackLayout BuildEmpty()
        {
            StackLayout emptyCont = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                Children =
                    {
                        new Label
                        {
                            Text = "No other meal plans found.",
                            FontSize = Units.FontSizeL,
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Color.White,
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                    }
            };

            return emptyCont;
        }
    }
}
