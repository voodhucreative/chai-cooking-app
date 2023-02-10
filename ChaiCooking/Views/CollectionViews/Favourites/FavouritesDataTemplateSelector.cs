using System;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.Favourites
{
    public class FavouritesDataTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var recipe = (Recipe)item;
            return new FavouritesCollectionViewDataTemplate(recipe);
        }

        class FavouritesCollectionViewDataTemplate : DataTemplate
        {
            public FavouritesCollectionViewDataTemplate(Recipe recipe) : base(() => CreateDataTemplate(recipe))
            {

            }

            static View CreateDataTemplate(Recipe recipe) => new FavouritesTile(recipe).GetContent();
        }
    }
}
