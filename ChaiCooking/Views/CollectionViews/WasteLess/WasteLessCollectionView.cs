using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ChaiCooking.Branding;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Services;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.WasteLess
{
    public class WasteLessCollectionView
    {
        public WasteLessCollectionView()
        {
            AppSession.wasteLessCollection = new ObservableCollection<RecipesCollectionViewSection>();
            AppSession.wasteLessCollectionView = new CollectionView
            {
                IsGrouped = true,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight,
                ItemsSource = AppSession.wasteLessCollection,
                ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                BackgroundColor = Color.Transparent,
                SelectionMode = SelectionMode.None,
                ItemTemplate = new RecipeDataTemplateSelector(),
                ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    VerticalItemSpacing = 5,
                },
                EmptyView = BuildEmpty(),
                Header = BuildContentHeader(),
                Footer = BuildFooter(),
            };

            AppSession.wasteLessCollection.Clear();
        }

        public async void ShowRecipes(Action action)
        {
            await Task.Delay(10);
            AppSession.wasteLessCollection.Clear();
            AppSession.WasteLessRecipes = DataManager.GetWasteLessRecipes(AppSession.CurrentUser, true);
            var wasteLessGroup = new RecipesCollectionViewSection(AppSession.WasteLessRecipes);
            AppSession.wasteLessCollection.Add(wasteLessGroup);
            //action();
        }

        public CollectionView GetCollectionView()
        {
            return AppSession.wasteLessCollectionView;
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
                VerticalOptions = LayoutOptions.FillAndExpand

            };

            StaticLabel Title = new StaticLabel("Matched Recipes:");
            Title.Content.TextColor = Color.White;
            Title.Content.FontSize = Units.FontSizeM;
            Title.Content.Padding = Dimensions.GENERAL_COMPONENT_PADDING;
            Title.CenterAlign();

            StackLayout headerSection = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 4
            };

            ContentContainer.Children.Add(Title.Content);

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
    }
}
