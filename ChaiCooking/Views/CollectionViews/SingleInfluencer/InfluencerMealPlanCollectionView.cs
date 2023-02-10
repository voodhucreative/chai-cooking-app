using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Services;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.SingleInfluencer
{
    public class InfluencerMealPlanCollectionView
    {
        public InfluencerMealPlanCollectionView()
        {
            AppSession.influencerMealPlanCollection = new ObservableCollection<InfluencerMealPlanViewSection>();
            AppSession.influencerMealPlanCollectionView = new CollectionView
            {
                IsGrouped = true,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Start,
                WidthRequest = Units.ScreenWidth,
                //HeightRequest = Units.ScreenHeight,
                ItemsSource = AppSession.influencerMealPlanCollection,
                ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                BackgroundColor = Color.Transparent,
                SelectionMode = SelectionMode.None,
                ItemTemplate = new InfluencerMealPlanDataTemplateSelector(),
                ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    VerticalItemSpacing = 10
                },
                Footer = BuildFooter(),
                //EmptyView = BuildEmpty(),
            };
            AppSession.influencerMealPlanCollection.Clear();
        }

        public async void ShowMealPlans()
        {
            await Task.Delay(10);
            AppSession.influencerMealPlanCollection.Clear();
            AppSession.singleInfluencerMealPlans = await App.ApiBridge.GetInfluencerMealPlans(AppSession.CurrentUser);
            var mealPlanGroup = new InfluencerMealPlanViewSection(AppSession.singleInfluencerMealPlans.Data);
            AppSession.influencerMealPlanCollection.Add(mealPlanGroup);
            UpdateHeight();
        }

        public async void ShowMealPlansAll()
        {
            await Task.Delay(10);
            AppSession.influencerMealPlanCollection.Clear();
            AppSession.singleInfluencerMealPlans = await App.ApiBridge.BrowseMealPlans(AppSession.CurrentUser);
            var mealPlanGroup = new InfluencerMealPlanViewSection(AppSession.singleInfluencerMealPlans.Data);
            AppSession.influencerMealPlanCollection.Add(mealPlanGroup);
            UpdateHeight();
        }

        private void UpdateHeight()
        {
            try
            {
                //For some reason the collection view was still taking up a full screen in height so this forces the height to the amount of children.
                AppSession.influencerMealPlanCollectionView.HeightRequest = AppSession.singleInfluencerMealPlans.Data.Count * 135 > Units.ScreenHeight ? Units.ScreenHeight : AppSession.singleInfluencerMealPlans.Data.Count * 135;
                AppSession.influencerMealPlanCollectionView.ScrollTo(0, animate: false);
            }
            catch(Exception e)
            {

            }
        }

        public CollectionView GetCollectionView()
        {
            return AppSession.influencerMealPlanCollectionView;
        }

        private StackLayout BuildFooter()
        {
            StackLayout emptyCont = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HeightRequest = 10,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
            };

            return emptyCont;
        }

        private StackLayout BuildEmpty()
        {
            StackLayout emptyCont = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                Children =
                    {
                        new Label
                        {
                            Text = "No meal plans found.",
                            FontSize = Units.FontSizeXL,
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
