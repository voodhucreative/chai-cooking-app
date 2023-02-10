using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ChaiCooking.Models.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.IngredientFilter
{
    public class IngredientsCollectionViewSection : ObservableCollection<Ingredient>
    {
        public IngredientsCollectionViewSection(IEnumerable<Ingredient> items, StackLayout EmptyView)
        {
            EmptyViewIsVisible = AppDataContent.AvailableIngredients.Count == 0;
            if (items != null)
            {
                foreach (Ingredient item in items)
                {
                    this.Add(item);
                }
            }
        }

        public Ingredient recipe
        {
            get;
            private set;
        }

        public StackLayout EmptyView
        {
            get;
            private set;
        }

        public bool EmptyViewIsVisible
        {
            get;
            private set;
        }
    }
}
