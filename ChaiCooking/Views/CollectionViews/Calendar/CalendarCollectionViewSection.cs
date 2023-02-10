using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ChaiCooking.Models.Custom.MealPlanAPI;

namespace ChaiCooking.Views.CollectionViews.Calendar
{
    public class CalendarCollectionViewSection : ObservableCollection<UserMealTemplate.Datum>
    {
        public CalendarCollectionViewSection(IEnumerable<UserMealTemplate.Datum> items)
        {
            if (items != null)
            {
                foreach (UserMealTemplate.Datum item in items)
                {
                    this.Add(item);
                }
            }
        }

        public UserMealTemplate.Datum datum
        {
            get;
            private set;
        }
    }
}
