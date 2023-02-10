using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using ChaiCooking.Services.Storage;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.ShoppingBasket
{
    public class ShoppingBasketCollectionView
    {
        public ShoppingBasketCollectionView()
        {
            AppSession.shoppingBasketCollection = new ObservableCollection<ShoppingBasketViewSection>();
            AppSession.shoppingBasketCollectionView = new CollectionView
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
                ItemTemplate = new ShoppingBasketDataTemplateSelector(),
                ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    VerticalItemSpacing = 10
                },
                GroupHeaderTemplate = new DataTemplate(typeof(ShoppingBasketViewHeader))
                {
                    Bindings =
                    {
                        { ShoppingBasketViewHeader.SBEmptyViewProperty, new Binding("SBEmptyView") },
                        { ShoppingBasketViewHeader.SBEmptyViewVisibleProperty, new Binding("SBEmptyViewIsVisible") }
                    }
                },
                Footer = BuildFooter(),
            };
            AppSession.shoppingBasketCollection.Clear();
        }

        public async void ShowBasket()
        {
            await Task.Delay(10);
            AppSession.shoppingBasketCollection.Clear();
            //AppSession.shoppingList = DataManager.GetShoppingList().Result;
            AppSession.shoppingList = new Models.Custom.ShoppingBasket.ShoppingList();
            AppSession.shoppingList.content = new Models.Custom.ShoppingBasket.ShoppingList.Content();
            AppSession.shoppingList.content.recipes = new List<Recipe>();
            var shoppingBasketGroup = new ShoppingBasketViewSection(AppSession.shoppingList.content.recipes);//, StaticData.BuildEmpty());
            AppSession.shoppingBasketCollection.Add(shoppingBasketGroup);
            AppSession.shoppingBasketCollectionView.ItemsSource = AppSession.shoppingBasketCollection;
        }

        public CollectionView GetCollectionView()
        {
            return AppSession.shoppingBasketCollectionView;
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
