using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom.MealPlanAPI;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.InfluencerMealPreview
{
    public class PreviewCollectionView : BindableObject
    {
        public PreviewCollectionView()
        {
            AppSession.influencerMealPreviewCollection = new ObservableCollection<PreviewCollectionViewSection>();
            AppSession.influencerMealPreviewCollectionView = new CollectionView
            {
                IsGrouped = true,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight,
                ItemsSource = AppSession.influencerMealPreviewCollection,
                ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                BackgroundColor = Color.Transparent,
                SelectionMode = SelectionMode.None,
                ItemTemplate = new PreviewDataTemplateSelector(),
                ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    VerticalItemSpacing = 5,
                },
                EmptyView = BuildEmpty(),
            };
            //AppSession.mealPlannerCollection.Clear();
            //AppSession.mealPlanTokenSource = new CancellationTokenSource();
        }

        public async void ShowCalendar(string week, string mealPlanName)
        {
            var result = await App.ApiBridge.GetInfluencerMealPlanWeek(AppSession.CurrentUser, week.ToString());
            StaticData.storedInfluencerMealPlan = new StoredInfluencerMealPlan();
            StaticData.storedInfluencerMealPlan.mealPlanModel = result;
            StaticData.storedInfluencerMealPlan.week = week;
            StaticData.storedInfluencerMealPlan.mealPlanName = mealPlanName;

            if (StaticData.storedInfluencerMealPlan == null ||
                StaticData.storedInfluencerMealPlan.mealPlanModel == null ||
                StaticData.storedInfluencerMealPlan.mealPlanModel.Data.Count == 0)
            {
                // NO PREVIEW AKA SOMETHING WENT WRONG
            }
            else
            {
                await Task.Delay(10);

                var mealPreviewGroup = new PreviewCollectionViewSection(StaticData.storedInfluencerMealPlan.mealPlanModel.Data);
                AppSession.influencerMealPreviewCollection.Add(mealPreviewGroup);
                AppSession.influencerMealPreviewCollectionView.ItemsSource = AppSession.influencerMealPreviewCollection;
            }
        }

        public CollectionView GetCollectionView()
        {
            return AppSession.influencerMealPreviewCollectionView;
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
                            Text = "Obtaining meal plan...",
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
