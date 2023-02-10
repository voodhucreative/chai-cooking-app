using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using ChaiCooking.Branding;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Services;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.RecommendedRecipes
{
    public class RecommendedRecipesCollectionView : BindableObject
    {
        public Thickness collectionViewMargin
        {
            set { SetValue(marginProperty, value); }
            get { return (Thickness)GetValue(marginProperty); }
        }

        CollectionView collectionView;
        StaticLabel Title;
        StaticLabel Info;

        bool ShowInfo;

        public static readonly BindableProperty marginProperty =
       BindableProperty.Create("recommendedRecipesCollectionViewMargin", typeof(Thickness), typeof(RecommendedRecipesCollectionView));

        public double ScrollYPosition;

        CancellationTokenSource _tokenSource = null;

        public RecommendedRecipesCollectionView()
        {
            AppSession.recommendedRecipesCollection = new ObservableCollection<RecipesCollectionViewSection>();
            ShowInfo = false;

            collectionView = new CollectionView
            {
                IsGrouped = true,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight,
                ItemsSource = AppSession.recommendedRecipesCollection,
                ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                SelectionMode = SelectionMode.None,
                ItemTemplate = new RecipeDataTemplateSelector(),
                ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    VerticalItemSpacing = 10
                },
                EmptyView = BuildEmpty(),
                Header = BuildContentHeader(),
                Footer = BuildFooter(),
            };
            _tokenSource = new CancellationTokenSource();
            AppSession.recommendedRecipesCollection.Clear();
        }

        public async void ShowRecipes(Action action)
        {
            _tokenSource.Cancel();
            _tokenSource = new CancellationTokenSource();
            await Task.Delay(1);
            AppSession.recommendedRecipesCollection.Clear();
            AppSession.RecommendedRecipes = DataManager.GetRecommendedRecipes(AppSession.CurrentUser, true);
            var recommendedRecipesGroup = new RecipesCollectionViewSection(AppSession.RecommendedRecipes);
            AppSession.recommendedRecipesCollection.Add(recommendedRecipesGroup);
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
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                //HeightRequest = 120,
                VerticalOptions = LayoutOptions.StartAndExpand

            };

            Title = new StaticLabel(AppData.AppText.RECOMMENDED_RECIPES + ".");
            Title.Content.TextColor = Color.White;
            Title.Content.FontSize = Units.FontSizeXL;
            Title.Content.Padding = Dimensions.GENERAL_COMPONENT_PADDING;
            Title.CenterAlign();

            Info = new StaticLabel(AppData.AppText.RECOMMENDED_RECIPES_INFO);
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

            TouchEffect.SetNativeAnimation(Title.Content, true);
            TouchEffect.SetCommand(Title.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        ToggleInfo();
                    });
                }));

            Info.Content.IsVisible = ShowInfo;
            headerSection.Children.Add(Title.Content);
            headerSection.Children.Add(Info.Content);
            headerSection.Children.Add(Seperator);

            ContentContainer.Children.Add(headerSection);

            return ContentContainer;
        }

        private StackLayout BuildFooter()
        {
            StackLayout emptyCont = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = Units.ScreenHeight5Percent,
            };

            return emptyCont;
        }

        private void ToggleInfo()
        {
            ShowInfo = !ShowInfo;
            Info.Content.IsVisible = ShowInfo;
        }

    }
}
