using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChaiCooking.Branding;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.MealPlanHolder
{
    public class MealPlanHolderCollectionView : BindableObject
    {
        public MealPlanHolderCollectionView()
        {
            AppSession.mealPlanHolderCollection = new ObservableCollection<MealPlanHolderCollectionViewSection>();
            AppSession.mealPlanHolderCollectionView = new CollectionView
            {
                IsGrouped = true,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = Units.ScreenWidth,
                HeightRequest = 85,
                ItemsSource = AppSession.mealPlanHolderCollection,
                ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                BackgroundColor = Color.Transparent,
                SelectionMode = SelectionMode.None,
                ItemTemplate = new MealPlanHolderDataTemplateSelector(),
                ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Horizontal)
                {
                    HorizontalItemSpacing = 10,
                },
                EmptyView = BuildEmpty(),
            };
            //AppSession.mealPlanHolderCollection.Clear();
        }

        public async void ShowRecipes()
        {
            await Task.Delay(100);
            var mealPlanHolderGroup = new MealPlanHolderCollectionViewSection(AppSession.CurrentUser.recipeHolder);
            AppSession.mealPlanHolderCollection.Add(mealPlanHolderGroup);
            AppSession.mealPlanHolderCollectionView.ItemsSource = AppSession.mealPlanHolderCollection;
        }

        public CollectionView GetCollectionView()
        {
            return AppSession.mealPlanHolderCollectionView;
        }

        private Label BuildEmpty()
        {
            return new Label
            {
                Text = "No recipes found.",
                FontSize = Units.FontSizeL,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };
        }

        private StackLayout BuildContentFooter()
        {
            StackLayout footerContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HeightRequest = 80,
                BackgroundColor = Color.Green
            };

            return footerContentContainer;
        }
    }
}
