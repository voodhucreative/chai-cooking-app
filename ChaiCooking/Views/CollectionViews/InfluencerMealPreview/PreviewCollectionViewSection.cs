using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ChaiCooking.Helpers;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.MealPlanAPI;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.InfluencerMealPreview
{
    public class PreviewCollectionViewSection : ObservableCollection<MealPlanModel.Datum>
    {
        public PreviewCollectionViewSection(IEnumerable<MealPlanModel.Datum> items)
        {
            if (items != null)
            {
                foreach (MealPlanModel.Datum item in items)
                {
                    this.Add(item);
                }
            }
        }

        public MealPlanModel.Datum data
        {
            get;
            private set;
        }
    }
}
