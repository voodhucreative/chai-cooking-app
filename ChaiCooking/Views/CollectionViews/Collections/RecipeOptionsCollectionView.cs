using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using ChaiCooking.Helpers;
using ChaiCooking.Models;
using ChaiCooking.Services;
using ChaiCooking.Models.Custom;
using System.Windows.Input;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Branding;
namespace ChaiCooking.Views.CollectionViews.Collections
{
    public class RecipeOptionsCollectionView : BindableObject
    {
        public Thickness collectionViewMargin
        {
            set { SetValue(marginProperty, value); }
            get { return (Thickness)GetValue(marginProperty); }
        }

        public static readonly BindableProperty marginProperty =
       BindableProperty.Create("collectionViewMargin", typeof(Thickness), typeof(CollectionsCollectionView));

        public double ScrollYPosition;

        CancellationTokenSource _tokenSource = null;

        CollectionView collectionView;

        public RecipeOptionsCollectionView()
        {
            AppSession.collectionsCollection = new ObservableCollection<CollectionsCollectionViewSection>();
            collectionView = new CollectionView
            {
                IsGrouped = true,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HeightRequest = 170,
                ItemsSource = AppSession.collectionsCollection,
                ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY),
                ItemTemplate = new RecipeOptionsDataTemplateSelector(),
                //SelectionChangedCommand = SelectedTagChanged,
                ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical)
                {
                },
                EmptyView = BuildEmpty(),
                SelectionMode = SelectionMode.Single,
            };
            _tokenSource = new CancellationTokenSource();
            AppSession.collectionsCollection.Clear();
        }

        public async void ShowRecipeOptionsCollection()
        {
            _tokenSource.Cancel();
            _tokenSource = new CancellationTokenSource();
            await Task.Delay(10);
            AppSession.collectionsCollection.Clear();
            AppSession.UserCollections = await DataManager.GetAlbums(_tokenSource.Token, null);
            var albumsGroup = new CollectionsCollectionViewSection(AppSession.UserCollections);
            AppSession.collectionsCollection.Add(albumsGroup);
        }

        public CollectionView GetCollectionView()
        {
            return collectionView;
        }

        private StackLayout BuildEmpty()
        {
            StackLayout emptyCont = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                Children =
                    {
                        new Label
                        {
                            Text = "You have no existing collections.",
                            FontSize = Units.FontSizeXL,
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Color.White,
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                    }
            };

            return emptyCont;
        }

        private StackLayout BuildContentFooter()
        {
            StackLayout ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                HeightRequest = 20,
                VerticalOptions = LayoutOptions.StartAndExpand

            };

            return ContentContainer;
        }
    }
}
