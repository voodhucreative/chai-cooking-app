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
    public class CollectionsCollectionView : BindableObject
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

        public CollectionsCollectionView()
        {
            AppSession.collectionsCollection = new ObservableCollection<CollectionsCollectionViewSection>();
            AppSession.collectionsCollectionView = new CollectionView
            {
                IsGrouped = true,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight,
                ItemsSource = AppSession.collectionsCollection,
                ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                ItemTemplate = new CollectionsDataTemplateSelector(),
                //SelectionChangedCommand = SelectedTagChanged,
                ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical)
                {
                },
                EmptyView = BuildEmpty(),
                SelectionMode = SelectionMode.Multiple,
            };
            _tokenSource = new CancellationTokenSource();
            AppSession.collectionsCollection.Clear();
            //AppSession.collectionsCollectionView.SelectionChanged += CollectionView_SelectionChanged;
            //AppSession.collectionsCollectionView.Scrolled += OnCollectionViewScrolled;
        }

        public async void ShowAlbums()
        {
            _tokenSource.Cancel();
            _tokenSource = new CancellationTokenSource();
            await Task.Delay(10);
            AppSession.collectionsCollection.Clear();
            AppSession.UserCollections = await DataManager.GetAlbums(_tokenSource.Token, null);
            Album favAlbum = new Album()
            {
                Id = "-1",
                Name = "Favourites",
                FolderColor = "FFFFFF",
            };
            AppSession.UserCollections.Insert(0, favAlbum);
            var albumsGroup = new CollectionsCollectionViewSection(AppSession.UserCollections);
            AppSession.collectionsCollection.Add(albumsGroup);
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
            return AppSession.collectionsCollectionView;
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
                HeightRequest = 120,
                VerticalOptions = LayoutOptions.StartAndExpand

            };

            return ContentContainer;
        }

        //public Command SelectedTagChanged
        //{
        //    get
        //    {
        //        return new Command(() =>
        //        {
        //            Console.WriteLine();
        //        });
        //    }
        //}

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0)
                return;

            //var item = (Album)e.CurrentSelection[0];

            //AppSession.collectionsCollection[0].Single(i => i.Name == item.Name).isHighlighted = true;
            //perform action on selection or navigate to details view

            //((CollectionView)sender).SelectedItem = null;
        }

        void OnCollectionViewScrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            // Custom logic
        }
    }
}
