using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ChaiCooking.Models.Custom;

namespace ChaiCooking.Views.CollectionViews.MealPlanHolder
{
    public class MealPlanHolderCollectionViewSection : ObservableCollection<Recipe>
    {
        public MealPlanHolderCollectionViewSection(IEnumerable<Recipe> items)
        {
            IsHeaderVisible = (items != null && items.Count() != 0) ? true : false;
            IsFooterVisible = (items != null && items.Count() != 0) ? true : false;

            if (items != null)
            {
                foreach (Recipe item in items)
                {
                    this.Add(item);
                }
            }
        }
        public Recipe recipe
        {
            get;
            private set;
        }

        public bool IsHeaderVisible
        {
            get;
            private set;
        }

        public bool IsFooterVisible
        {
            get;
            set;
        }
    }
}
