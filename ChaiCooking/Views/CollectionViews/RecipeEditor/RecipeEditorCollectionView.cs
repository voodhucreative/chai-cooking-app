using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.RecipeEditor
{
    public class RecipeEditorCollectionView
    {
        public RecipeEditorCollectionView(
            View header)
        {
            AppSession.recipeEditorCollection = new ObservableCollection<RecipeEditorViewSection>();
            AppSession.recipeEditorCollectionView = new CollectionView
            {
                IsGrouped = true,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight,
                ItemsSource = null,
                ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                BackgroundColor = Color.Transparent,
                SelectionMode = SelectionMode.None,
                ItemTemplate = new RecipeEditorDataTemplateSelector(),
                ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    VerticalItemSpacing = 10
                },
                Header = header,
                Footer = BuildFooter(),
            };
            AppSession.recipeEditorCollection.Clear();
        }

        public async void ShowBasket()
        {
            await Task.Delay(10);
            AppSession.recipeEditorCollection.Clear();
            AppSession.recipeEditorRecipes = new List<Recipe>();
            var recipeGroup = new RecipeEditorViewSection(AppSession.recipeEditorRecipes);
            AppSession.recipeEditorCollection.Add(recipeGroup);
            AppSession.recipeEditorCollectionView.ItemsSource = AppSession.recipeEditorCollection;
        }

        public CollectionView GetCollectionView()
        {
            return AppSession.recipeEditorCollectionView;
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
    }
}
