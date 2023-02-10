using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ChaiCooking.AppData;
using ChaiCooking.Layouts.Custom.Lists;
using ChaiCooking.Models.Custom.InfluencerAPI;
using ChaiCooking.Models.Custom.MealPlanAPI;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.CreateOrSelect
{
    public class CreateOrSelectViewSection : ObservableCollection<object>
    {
        public CreateOrSelectViewSection(IEnumerable<Datum> items)
        {
            try
            {
                EmptyViewIsVisible = StaticData.userMealPlans.Data.Count == 0;
            }
            catch
            {
                EmptyViewIsVisible = false;
            }
            if (items != null)
            {
                foreach (Datum item in items)
                {
                    if (!item.name.StartsWith("%Chai_Internal%_"))
                    {
                        this.Add(item);
                    }
                }
            }
        }

        public CreateOrSelectViewSection(IEnumerable<InfluencerMealPlans.Datum> items)
        {
            if (items != null)
            {
                foreach (InfluencerMealPlans.Datum item in items)
                {
                    if (!item.name.StartsWith("%Chai_Internal%_"))
                    {
                        this.Add(item);
                    }
                }
            }
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
