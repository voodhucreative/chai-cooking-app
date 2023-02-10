using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ChaiCooking.Models.Custom;

namespace ChaiCooking.Views.CollectionViews.Collections
{
    public class CollectionsCollectionViewSection : ObservableCollection<Album>
    {
        public CollectionsCollectionViewSection(IEnumerable<Album> items)
        {
            if (items != null)
            {

                foreach (Album item in items)
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
