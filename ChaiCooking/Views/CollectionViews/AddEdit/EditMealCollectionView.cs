using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Services;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.AddEdit
{
    public class EditMealCollectionView : BindableObject
    {
        public Thickness collectionViewMargin
        {
            set { SetValue(marginProperty, value); }
            get { return (Thickness)GetValue(marginProperty); }
        }

        CollectionView collectionView;

        public static readonly BindableProperty marginProperty =
       BindableProperty.Create("editMealCollectionViewMargin", typeof(Thickness), typeof(EditMealCollectionView));

        public double ScrollYPosition;

        CancellationTokenSource _tokenSource = null;

        public EditMealCollectionView()
        {
            AppSession.editMealRecipesCollection = new ObservableCollection<MealsCollectionViewSection>();
            collectionView = new CollectionView
            {
                IsGrouped = true,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight,
                ItemsSource = AppSession.editMealRecipesCollection,
                ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                BackgroundColor = Color.Transparent,
                SelectionMode = SelectionMode.Single,
                ItemTemplate = new MealDataTemplateSelector(),
                ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    VerticalItemSpacing = 10,
                },
                EmptyView = BuildEmpty(),
            };
            _tokenSource = new CancellationTokenSource();
        }

        public async void ShowRecipes(Action action)
        {
            _tokenSource.Cancel();
            _tokenSource = new CancellationTokenSource();
            await Task.Delay(10);
            AppSession.EditMealRecipes = DataManager.GetRecommendedRecipes(AppSession.CurrentUser, AppSession.UpdateSearch);
            //This appears to be unnecessary, it also causes a crash so will leave commented out for now.
            //var mealsGroup = new MealsCollectionViewSection(AppSession.EditMealRecipes);
            //AppSession.editMealRecipesCollection.Add(mealsGroup);
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
