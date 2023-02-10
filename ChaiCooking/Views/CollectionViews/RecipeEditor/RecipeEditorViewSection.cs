using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ChaiCooking.Models.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.RecipeEditor
{
    public class RecipeEditorViewSection : ObservableCollection<Recipe>
    {
        public RecipeEditorViewSection(IEnumerable<Recipe> items)
        {

            if (AppSession.recipeEditorRecipes == null)
            {
                AppSession.recipeEditorRecipes = new List<Recipe>();
            }

            if (items != null)
            {
                foreach (Recipe item in items)
                {
                    Add(item);
                }
            }
        }


        public Recipe recipe
        {
            get;
            private set;
        }
    }

}
