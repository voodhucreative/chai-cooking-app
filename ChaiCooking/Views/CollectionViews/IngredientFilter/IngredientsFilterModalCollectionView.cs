using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.IngredientFilter
{
    public class IngredientsFilterModalCollectionView
    {
        public IngredientsFilterModalCollectionView()
        {
            AppSession.ingredientsFilterModalCollection = new ObservableCollection<IngredientsCollectionViewSection>();
            AppSession.ingredientsFilterModalCollectionView = new CollectionView
            {
                IsGrouped = true,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = Units.ScreenWidth,
                HeightRequest = 100,
                ItemsSource = AppSession.ingredientsFilterModalCollection,
                ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                BackgroundColor = Color.Transparent,
                SelectionMode = SelectionMode.None,
                ItemTemplate = new IngredientDataTemplateSelector(true),
                ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Horizontal)
                {
                    HorizontalItemSpacing = 5,
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
            AppSession.ingredientsFilterModalCollection.Clear();
        }

        public async void ShowIngredients()
        {
            await Task.Delay(10);
            AppSession.ingredientsFilterModalCollection.Clear();
            var ingredientsGroup = new IngredientsCollectionViewSection(AppDataContent.AvailableIngredients, BuildEmpty());
            AppSession.ingredientsFilterModalCollection.Add(ingredientsGroup);
        }

        public CollectionView GetCollectionView()
        {
            return AppSession.ingredientsFilterModalCollectionView;
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
