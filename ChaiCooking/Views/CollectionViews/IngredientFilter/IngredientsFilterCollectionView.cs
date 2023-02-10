using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Services;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.IngredientFilter
{
    public class IngredientsFilterCollectionView
    {
        public IngredientsFilterCollectionView()
        {
            AppSession.ingredientsCollection = new ObservableCollection<IngredientsCollectionViewSection>();
            AppSession.ingredientsCollectionView = new CollectionView
            {
                IsGrouped = true,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start,
                WidthRequest = Units.ScreenWidth,
                HeightRequest = 110,
                ItemsSource = AppSession.ingredientsCollection,
                ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                BackgroundColor = Color.Transparent,
                SelectionMode = SelectionMode.None,
                ItemTemplate = new IngredientDataTemplateSelector(false),
                ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical)
                {
                    VerticalItemSpacing = 5,
                },
                GroupHeaderTemplate = new DataTemplate(typeof(IngredientFilterViewHeader))
                {
                    Bindings =
                    {
                        {  IngredientFilterViewHeader.EmptyViewProperty, new Binding("EmptyView")},
                        {  IngredientFilterViewHeader.EmptyViewVisibleProperty, new Binding("EmptyViewIsVisible")}
                    }
                },
            };
            AppSession.ingredientsCollection.Clear();
        }

        public async void ShowIngredients()
        {
            await Task.Delay(10);
            AppSession.ingredientsCollection.Clear();
            var ingredientsGroup = new IngredientsCollectionViewSection(AppDataContent.AvailableIngredients, BuildEmpty());
            AppSession.ingredientsCollection.Add(ingredientsGroup);
        }

        public CollectionView GetCollectionView()
        {
            return AppSession.ingredientsCollectionView;
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
                            Text = "No ingredients added.",
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
