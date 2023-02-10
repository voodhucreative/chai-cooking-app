using System;
using ChaiCooking.Layouts.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.MealPlanAPI;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.InfluencerMealPreview
{
    public class PreviewDataTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var itemConvert = (MealPlanModel.Datum)item;
            return new PreviewCollectionViewDataTemplate(itemConvert);
        }

        class PreviewCollectionViewDataTemplate : DataTemplate
        {
            public PreviewCollectionViewDataTemplate(MealPlanModel.Datum item) : base(() => CreateDataTemplate(item))
            {
            }

            static View CreateDataTemplate(MealPlanModel.Datum item) => new MealPlannerRow(item).GetContent();
        }
    }
}
