using System;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.MealPlanHolder
{
    public class MealPlanHolderDataTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var recipeItem = (Recipe)item;

            return new MealPlanHolderCollectionViewDataTemplate(recipeItem);
        }
    }

    class MealPlanHolderCollectionViewDataTemplate : DataTemplate
    {
        public MealPlanHolderCollectionViewDataTemplate(Recipe recipe) : base(() => CreateDataTemplate(recipe))
        {
        }

        static View CreateDataTemplate(Recipe recipe) => new MealPlanHolderTile(recipe).GetContent();
    }
}
