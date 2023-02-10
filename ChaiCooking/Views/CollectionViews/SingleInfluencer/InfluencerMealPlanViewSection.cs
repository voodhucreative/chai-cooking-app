using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ChaiCooking.Models.Custom.InfluencerAPI;

namespace ChaiCooking.Views.CollectionViews.SingleInfluencer
{
    public class InfluencerMealPlanViewSection : ObservableCollection<InfluencerMealPlans.Datum>
    {
        public InfluencerMealPlanViewSection(IEnumerable<InfluencerMealPlans.Datum> items)
        {
            if (AppSession.singleInfluencerMealPlans == null)
            {
                AppSession.singleInfluencerMealPlans = new InfluencerMealPlans();
            }

            if (items != null)
            {
                foreach (InfluencerMealPlans.Datum item in items)
                {
                    Add(item);
                }
            }
        }

        public InfluencerMealPlans.Datum mealPlan
        {
            get;
            private set;
        }
    }
}
