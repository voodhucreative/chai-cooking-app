using System;
using ChaiCooking.Layouts.Custom.Lists;
using ChaiCooking.Models.Custom.InfluencerAPI;
using ChaiCooking.Models.Custom.MealPlanAPI;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.CreateOrSelect
{
    public class CreateOrSelectDataTemplateSelector : DataTemplateSelector
    {
        Action buildConfirm { get; set; }

        public CreateOrSelectDataTemplateSelector(Action buildConfirm)
        {
            this.buildConfirm = buildConfirm;
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is Datum)
            {
                var datumItem = (Datum)item;
                return new CreateOrSelectCollectionViewDataTemplate(datumItem, buildConfirm);
            }
            else
            {
                var datumItem = (InfluencerMealPlans.Datum)item;
                return new CreateOrSelectMealTemplates(datumItem, buildConfirm);
            }
        }

        class CreateOrSelectCollectionViewDataTemplate : DataTemplate
        {
            public CreateOrSelectCollectionViewDataTemplate(Datum mealPlan, Action buildConfirm) : base(() => CreateDataTemplate(mealPlan, buildConfirm))
            {
            }

            static View CreateDataTemplate(Datum mealPlan, Action buildConfirm) => new UserMealPlanList(mealPlan, buildConfirm).GetContent();
        }

        class CreateOrSelectMealTemplates : DataTemplate
        {
            public CreateOrSelectMealTemplates(InfluencerMealPlans.Datum mealPlan, Action buildConfirm) : base(() => CreateDataTemplate(mealPlan, buildConfirm))
            {
            }

            static View CreateDataTemplate(InfluencerMealPlans.Datum mealPlan, Action buildConfirm) => new UserMealPlanList(mealPlan, buildConfirm).GetContent();
        }

    }
}
