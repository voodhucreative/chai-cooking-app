using System;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.ShoppingBasket
{
    public class ShoppingBasketViewHeader : Grid
    {
        public static readonly BindableProperty SBEmptyViewProperty =
            BindableProperty.Create("SBEmptyView", typeof(StackLayout), typeof(ShoppingBasketViewHeader));

        public static readonly BindableProperty SBEmptyViewVisibleProperty =
            BindableProperty.Create("SBEmptyViewIsVisible", typeof(bool), typeof(ShoppingBasketViewHeader), defaultValue: true);

        public bool EmptyViewIsVisible
        {
            get { return (bool)GetValue(SBEmptyViewVisibleProperty); }
            set { SetValue(SBEmptyViewVisibleProperty, value); }
        }

        public StackLayout EmptyView
        {
            get { return (StackLayout)GetValue(SBEmptyViewProperty); }
            set { SetValue(SBEmptyViewProperty, value); }
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

        public ShoppingBasketViewHeader()
        {
            this.WidthRequest = Units.ScreenWidth;
            //Children.Add(BuildEmpty());
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
                            Text = "No recipes in basket.",
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
