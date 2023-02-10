using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.Feed;
using ChaiCooking.Models.Custom.MealPlanAPI;

namespace ChaiCooking.Views.CollectionViews.MealPlanner
{
    public class MealPlannerCollectionViewSection : ObservableCollection<object>
    {
        public MealPlannerCollectionViewSection(IEnumerable<UserMealTemplate.Datum> items)
        {
            FooterIsVisible = false;
            if (items != null)
            {
                foreach (UserMealTemplate.Datum item in items)
                {
                    this.Add(item);
                }
            }
        }

        public MealPlannerCollectionViewSection(IEnumerable<MealPlanModel.Datum> items)
        {
            FooterIsVisible = false;
            if (items != null)
            {
                foreach (MealPlanModel.Datum item in items)
                {
                    this.Add(item);
                }
            }
        }

        public bool FooterIsVisible
        {
            get;
            private set;
        }
    }
}
