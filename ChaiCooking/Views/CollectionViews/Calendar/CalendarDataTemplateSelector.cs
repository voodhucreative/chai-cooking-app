using System;
using ChaiCooking.Layouts.Custom;
using Xamarin.Forms;
using static ChaiCooking.Models.Custom.MealPlanAPI.UserMealTemplate;

namespace ChaiCooking.Views.CollectionViews.Calendar
{
    public class CalendarDataTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var datum = (Datum)item;

            if (datum.Meals != null && datum.Meals.Count != 0 && !datum.Error)
            {
                return new CalendarCollectionViewDataTemplate(datum);
            }
            else
            {
                return new CalendarCollectionViewDataTemplateEmpty(datum);
            }
        }

        class CalendarCollectionViewDataTemplate : DataTemplate
        {
            public CalendarCollectionViewDataTemplate(Datum datum) : base(() => CreateDataTemplate(datum))
            {
            }

            static View CreateDataTemplate(Datum datum) => new MealPlannerUserRow(datum.rowIndex, datum.Meals, date: datum.date, id: AppSession.CurrentUser.defaultMealPlanID, isCalendarTile: true).GetContent();
        }

        class CalendarCollectionViewDataTemplateEmpty : DataTemplate
        {
            public CalendarCollectionViewDataTemplateEmpty(Datum datum) : base(() => CreateDataTemplate(datum))
            {
            }

            static View CreateDataTemplate(Datum datum) => new MealPlannerUserRow(datum.rowIndex, null, date: datum.date, id: AppSession.CurrentUser.defaultMealPlanID, isCalendarTile: true).GetContent();
        }
    }
}
