using System;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.Collections
{
    public class CollectionsDataTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var collectionItem = (Album)item;
            return new CollectionsCollectionViewDataTemplate(collectionItem);
        }
    }

    class CollectionsCollectionViewDataTemplate : DataTemplate
    {
        public CollectionsCollectionViewDataTemplate(Album album) : base(() => CreateDataTemplate(album))
        {
        }

        static View CreateDataTemplate(Album album) => new CollectionsTile(album).GetContent();
    }
}