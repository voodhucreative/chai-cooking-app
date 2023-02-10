using System;
using System.Collections.Generic;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.MealPlanner
{
    public class MealPlannerFooter : Grid
    {
        public MealPlannerFooter()
        {
            this.WidthRequest = Units.ScreenWidth;
            //Children.Add(BuildAddDay());
        }
        public static readonly BindableProperty FooterNotVisibleViewProperty =
            BindableProperty.Create("FooterNotVisible", typeof(StackLayout), typeof(MealPlannerFooter));

        public static readonly BindableProperty FooterIsVisibleProperty =
            BindableProperty.Create("FooterIsVisible", typeof(bool), typeof(MealPlannerFooter), defaultValue: true);

        public bool FooterIsVisible
        {
            get { return (bool)GetValue(FooterIsVisibleProperty); }
            set { SetValue(FooterIsVisibleProperty, value); }
        }

        public StackLayout FooterNotVisible
        {
            get { return (StackLayout)GetValue(FooterNotVisibleViewProperty); }
            set { SetValue(FooterNotVisibleViewProperty, value); }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                this.HeightRequest = FooterIsVisible ? 50 : 0;
                this.WidthRequest = FooterIsVisible ? Units.ScreenWidth : 0;
            }
        }

        private StackLayout BuildAddDay()
        {
            StaticImage addIcon = new StaticImage("plus.png", 10, 10, null);

            StackLayout addIconContent = new StackLayout
            {
                Children =
                {
                    addIcon.Content
                }
            };

            Frame addFrame = new Frame
            {
                Content = addIconContent,
                CornerRadius = 20,
                Padding = 10,
                HeightRequest = 20,
                WidthRequest = 60,
                BackgroundColor = Color.Orange,
                HasShadow = false,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
            };

            TouchEffect.SetNativeAnimation(addFrame, true);
            TouchEffect.SetCommand(addFrame,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            MealPlanModel.Datum datum = new MealPlanModel.Datum();
                            datum.day_Number = AppSession.day;
                            datum.mealTemplates = new List<MealTemplate>();
                            MealTemplate mealTemplate1 = new MealTemplate();
                            MealTemplate mealTemplate2 = new MealTemplate();
                            MealTemplate mealTemplate3 = new MealTemplate();
                            MealTemplate mealTemplate4 = new MealTemplate();
                            datum.mealTemplates.Add(mealTemplate1);
                            datum.mealTemplates.Add(mealTemplate2);
                            datum.mealTemplates.Add(mealTemplate3);
                            datum.mealTemplates.Add(mealTemplate4);
                            AppSession.mealTemplate.Data.Add(datum);

                            var mealPlannerGroup = new MealPlannerCollectionViewSection(AppSession.mealTemplate.Data);
                            AppSession.mealPlannerCollection.Add(mealPlannerGroup);
                            AppSession.mealPlannerCollection.RemoveAt(0);
                            AppSession.day = AppSession.day + 1;
                        }
                        catch
                        {
                        }
                    });
                }));

            StaticLabel addText = new StaticLabel("Add Day");
            addText.Content.FontSize = Units.FontSizeL;
            addText.Content.FontFamily = Fonts.GetBoldAppFont();
            addText.Content.TextColor = Color.White;
            addText.Content.VerticalTextAlignment = TextAlignment.Center;
            addText.Content.HorizontalTextAlignment = TextAlignment.Center;

            StackLayout buttonCont = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                Children =
                {
                    addFrame,
                    addText.Content
                }
            };

            return buttonCont;
        }
    }
}
