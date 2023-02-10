using System;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews
{
    public class UserRecipeDataTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var recipeItem = (Recipe)item;
            return new RecipeCollectionViewDataTemplate(recipeItem);
        }

        class RecipeCollectionViewDataTemplate : DataTemplate
        {
            public RecipeCollectionViewDataTemplate(Recipe recipe) : base(() => CreateDataTemplate(recipe))
            {
            }

            static View CreateDataTemplate(Recipe recipe) => new UserRecipeTile(recipe).GetContent();
        }
    }
}