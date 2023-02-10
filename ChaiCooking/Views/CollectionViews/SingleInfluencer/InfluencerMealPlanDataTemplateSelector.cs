using System;
using ChaiCooking.Layouts.Custom.Lists;
using ChaiCooking.Models.Custom.InfluencerAPI;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.SingleInfluencer
{
    public class InfluencerMealPlanDataTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var datumItem = (InfluencerMealPlans.Datum)item;
            return new InfluencerMealPlanCollectionViewDataTemplate(datumItem);
        }

        class InfluencerMealPlanCollectionViewDataTemplate : DataTemplate
        {
            public InfluencerMealPlanCollectionViewDataTemplate(InfluencerMealPlans.Datum datum) : base(() => CreateDataTemplate(datum))
            {
            }

            static View CreateDataTemplate(InfluencerMealPlans.Datum datum) => new InfluencerMealPlanList(datum).GetContent();
        }
    }
}
