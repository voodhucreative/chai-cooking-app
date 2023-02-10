using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.ShoppingBasket;
using ChaiCooking.Services.Storage;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.ShoppingBasket
{
    public class ShoppingBasketViewSection : ObservableCollection<Recipe>
    {
        public ShoppingBasketViewSection(IEnumerable<Recipe> items)//, StackLayout SBEmptyView)
        {
            if (AppSession.shoppingList == null)
            {
                AppSession.shoppingList = new ShoppingList();
            }
            SBEmptyViewIsVisible = AppSession.shoppingList.content.recipes.Count == 0;
            if (items != null)
            {
                foreach (Recipe item in items)
                {
                    Add(item);
                }
            }

            SBEmptyView = new StackLayout
            {
                BackgroundColor = Color.Firebrick
            };
        }

        public Recipe recipe
        {
            get;
            private set;
        }

        public StackLayout SBEmptyView
        {
            get;
            private set;
        }

        public bool SBEmptyViewIsVisible
        {
            get;
            private set;
        }
    }
}
