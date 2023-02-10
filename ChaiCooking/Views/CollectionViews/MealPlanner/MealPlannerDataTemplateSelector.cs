using System;
using ChaiCooking.Layouts.Custom;
using ChaiCooking.Models.Custom;
using Xamarin.Forms;
using static ChaiCooking.Models.Custom.MealPlanAPI.UserMealTemplate;

namespace ChaiCooking.Views.CollectionViews.MealPlanner
{
    public class MealPlannerDataTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is Datum)
            {
                var datum = (Datum)item;
                if (datum.date != "EMPTY")
                {

                    if (datum.Meals != null && datum.Meals.Count != 0 && !datum.Error)
                    {
                        return new MealPlanRows(datum);
                    }
                    else
                    {
                        return new MealPlanEmptyRows(datum);
                    }
                }
                else
                {
                    return new CreateNew(datum);
                }
            }
            else
            {
                var datum = (MealPlanModel.Datum)item;
                if (datum.id == -1)
                {
                    return new MealTemplateEmpty(datum);
                }
                else
                {
                    return new MealTemplateCreateRows(datum);
                }
            }

        }

        class MealPlanRows : DataTemplate
        {
            public MealPlanRows(Datum datum) : base(() => CreateDataTemplate(datum))
            {
            }

            static View CreateDataTemplate(Datum datum) => new MealPlannerUserRow(datum.rowIndex, datum.Meals, date: datum.date, id: AppSession.CurrentUser.defaultMealPlanID).GetContent();
        }

        class MealPlanEmptyRows : DataTemplate
        {
            public MealPlanEmptyRows(Datum datum) : base(() => CreateDataTemplate(datum))
            {
            }

            static View CreateDataTemplate(Datum datum) => new MealPlannerUserRow(datum.rowIndex, date: datum.date, id: AppSession.CurrentUser.defaultMealPlanID).GetContent();
        }

        class CreateNew : DataTemplate
        {
            public CreateNew(Datum datum) : base(() => CreateDataTemplate(datum))
            {
            }

            static View CreateDataTemplate(Datum datum) => new MealPlannerEmpty().GetContent();
        }

        class MealTemplateCreateRows : DataTemplate
        {
            public MealTemplateCreateRows(MealPlanModel.Datum datum) : base(() => CreateDataTemplate(datum))
            {
            }

            static View CreateDataTemplate(MealPlanModel.Datum datum) => new MealTemplateRow(datum).GetContent();
        }

        class MealTemplateEmpty : DataTemplate
        {
            public MealTemplateEmpty(MealPlanModel.Datum datum) : base(() => CreateDataTemplate(datum))
            {
            }

            static View CreateDataTemplate(MealPlanModel.Datum datum) => new MealTemplateRow(datum).GetContent();
        }
    }
}
