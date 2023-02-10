using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using ChaiCooking.Branding;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Services;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.SearchResults
{
    public class SearchResultsCollectionView : BindableObject
    {
        public Thickness collectionViewMargin
        {
            set { SetValue(marginProperty, value); }
            get { return (Thickness)GetValue(marginProperty); }
        }

        CollectionView collectionView;

        public static readonly BindableProperty marginProperty =
       BindableProperty.Create("recommendedRecipesCollectionViewMargin", typeof(Thickness), typeof(SearchResultsCollectionView));

        public double ScrollYPosition;

        CancellationTokenSource _tokenSource = null;

        public SearchResultsCollectionView()
        {
            AppSession.searchResultsCollection = new ObservableCollection<RecipesCollectionViewSection>();
            collectionView = new CollectionView
            {
                IsGrouped = true,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight,
                ItemsSource = AppSession.searchResultsCollection,
                ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                BackgroundColor = Color.Transparent,
                SelectionMode = SelectionMode.None,
                ItemTemplate = new RecipeDataTemplateSelector(),
                ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    ItemSpacing = 10
                },
                EmptyView = BuildEmpty(),
                Header = BuildContentHeader(),
                Footer = BuildContentFooter()
            };
            _tokenSource = new CancellationTokenSource();
            AppSession.searchResultsCollection.Clear();
        }

        public async void ShowRecipes(Action action)
        {
            _tokenSource.Cancel();
            _tokenSource = new CancellationTokenSource();
            await Task.Delay(10);
            AppSession.searchResultsCollection.Clear();
            AppSession.UpdateSearch = true;
            AppSession.SearchedRecipes = DataManager.SearchRecipes(AppSession.CurrentUser, AppSession.UpdateSearch);
            var searchResultsGroup = new RecipesCollectionViewSection(AppSession.SearchedRecipes);
            AppSession.searchResultsCollection.Add(searchResultsGroup);
            action();
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
                            Text = "No recipes found.",
                            FontSize = Units.FontSizeL,
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Color.White,
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                    }
            };

            return emptyCont;
        }

        public StackLayout BuildContentHeader()
        {
            StackLayout ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING, 0),
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            StaticLabel Info = new StaticLabel(AppData.AppText.SEARCH_RESULTS_INFO);
            Info.Content.TextColor = Color.White;
            Info.Content.FontSize = Units.FontSizeM;
            Info.Content.Padding = new Thickness(Dimensions.GENERAL_COMPONENT_SPACING, 8);
            Info.LeftAlign();

            ContentContainer.Children.Add(Info.Content);

            return ContentContainer;
        }

        public StackLayout BuildContentHeaderLarge()
        {
            StackLayout ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                HeightRequest = 120,
                VerticalOptions = LayoutOptions.StartAndExpand

            };

            StaticLabel Title = new StaticLabel(AppData.AppText.SEARCH_RESULTS + ".");
            Title.Content.TextColor = Color.White;
            Title.Content.FontSize = Units.FontSizeXXL;
            Title.Content.Padding = Dimensions.GENERAL_COMPONENT_SPACING;
            Title.CenterAlign();

            StaticLabel Info = new StaticLabel(AppData.AppText.SEARCH_RESULTS_INFO);
            Info.Content.TextColor = Color.White;
            Info.Content.FontSize = Units.FontSizeM;
            Info.Content.Padding = new Thickness(Dimensions.GENERAL_COMPONENT_SPACING, 8);
            Info.LeftAlign();

            Grid Seperator = new Grid { WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) };

            StackLayout headerSection = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 4
            };

            headerSection.Children.Add(Title.Content);
            headerSection.Children.Add(Info.Content);
            headerSection.Children.Add(Seperator);

            ContentContainer.Children.Add(headerSection);

            return ContentContainer;
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
