using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ChaiCooking.Models.Custom;

namespace ChaiCooking.Views.CollectionViews.Favourites
{
    public class FavouritesCollectionViewSection : ObservableCollection<Recipe>
    {
        public FavouritesCollectionViewSection(IEnumerable<Recipe> items)
        {
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
    }
}
