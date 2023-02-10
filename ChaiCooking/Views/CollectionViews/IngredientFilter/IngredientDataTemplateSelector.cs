using System;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.IngredientFilter
{
    public class IngredientDataTemplateSelector : DataTemplateSelector
    {
        bool isModal { get; set; }

        public IngredientDataTemplateSelector(bool isModal)
        {
            this.isModal = isModal;
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var ingredient = (Ingredient)item;
            if (isModal)
            {
                return new RecipeCollectionViewDataTemplateModal(ingredient);
            }
            else
            {
                return new RecipeCollectionViewDataTemplate(ingredient);
            }
        }

        class RecipeCollectionViewDataTemplate : DataTemplate
        {
            public RecipeCollectionViewDataTemplate(Ingredient ingredient) : base(() => CreateDataTemplate(ingredient))
            {

            }

            static View CreateDataTemplate(Ingredient ingredient) => new IngredientFilterTile(ingredient, false).GetContent();


        }

        class RecipeCollectionViewDataTemplateModal : DataTemplate
        {
            public RecipeCollectionViewDataTemplateModal(Ingredient ingredient) : base(() => CreateDataTemplate(ingredient))
            {

            }

            static View CreateDataTemplate(Ingredient ingredient) => new IngredientFilterTile(ingredient, true).GetContent();
        }
    }
}
