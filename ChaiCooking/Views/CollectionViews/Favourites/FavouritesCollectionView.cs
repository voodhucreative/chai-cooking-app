using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Services;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.Favourites
{
    public class FavouritesCollectionView : BindableObject
    {
        public Thickness collectionViewMargin
        {
            set { SetValue(marginProperty, value); }
            get { return (Thickness)GetValue(marginProperty); }
        }

        public static readonly BindableProperty marginProperty =
       BindableProperty.Create("favouritesCollectionViewMargin", typeof(Thickness), typeof(FavouritesCollectionView));

        public double ScrollYPosition;

        CancellationTokenSource _tokenSource = null;

        public FavouritesCollectionView()
        {
            AppSession.favouritesCollection = new ObservableCollection<FavouritesCollectionViewSection>();
            AppSession.favouritesCollectionView = new CollectionView
            {
                IsGrouped = true,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight,
                ItemsSource = AppSession.favouritesCollection,
                ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                BackgroundColor = Color.Transparent,
                SelectionMode = SelectionMode.None,
                ItemTemplate = new FavouritesDataTemplateSelector(),
                ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    VerticalItemSpacing = 10,
                },
                EmptyView = BuildEmpty(),
            };
            _tokenSource = new CancellationTokenSource();
            AppSession.favouritesCollection.Clear();
        }

        public async void ShowFavourites()
        {
            _tokenSource.Cancel();
            _tokenSource = new CancellationTokenSource();
            await Task.Delay(10);
            AppSession.favouritesCollection.Clear();
            AppSession.UserCollectionRecipes = await DataManager.GetFavouriteRecipes(_tokenSource.Token, null);
            var favouritesGroup = new FavouritesCollectionViewSection(null);
            AppSession.favouritesCollection.Add(favouritesGroup);
        }

        public CollectionView GetCollectionView()
        {
            return AppSession.favouritesCollectionView;
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
                            Text = "No recipes found.",
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
    }
}
