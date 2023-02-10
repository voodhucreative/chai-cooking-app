using System;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.Collections
{
    public class RecipeOptionsDataTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var collectionItem = (Album)item;
            return new RecipeOptionsCollectionViewDataTemplate(collectionItem);
        }

        class RecipeOptionsCollectionViewDataTemplate : DataTemplate
        {
            public RecipeOptionsCollectionViewDataTemplate(Album album) : base(() => CreateDataTemplate(album))
            {
            }

            static View CreateDataTemplate(Album album) => new RecipeOptionsTile(album).GetContent();
        }
    }
}
