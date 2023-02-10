using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.DebugData.Custom;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using ChaiCooking.Views.CollectionViews;
using ChaiCooking.Views.CollectionViews.SearchResults;
using FFImageLoading.Transformations;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class SearchResults : Page
    {
        StaticLabel PageInfo;
        StackLayout TopNavBox;
        StaticImage LastArrow, NextArrow;
        SearchResultsCollectionView searchResultsCollectionView;
        CollectionView recipesLayout;
        bool isFirstTime = true;
        public SearchResults()
        {
            this.IsScrollable = false;
            this.IsRefreshable = false;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;

            AppSession.CurrentPageSearch = 1;
            AppSession.TotalPages = 1;

            this.Id = (int)AppSettings.PageNames.SearchResults;
            this.Name = AppData.AppText.SEARCH_RESULTS; ;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.SlideInFromRight;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.SlideOutToRight;

            PageContent = new Grid
            {
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                RowDefinitions =
                    {
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    },
            };

            TopNavBox = new StackLayout
            {
                WidthRequest = Units.ScreenWidth,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };

            PageInfo = new StaticLabel("");
            PageInfo.Content.TextColor = Color.White;
            PageInfo.Content.FontSize = Units.FontSizeM;
            PageInfo.Content.Padding = new Thickness(Dimensions.GENERAL_COMPONENT_SPACING, 8);
            PageInfo.CenterAlign();

            Color tintColour = Color.LightGray;
            TintTransformation colorTint = new TintTransformation
            {
                HexColor = (string)tintColour.ToHex(),
                EnableSolidColor = true

            };
            var tint = new List<FFImageLoading.Work.ITransformation>();
            tint.Add(colorTint);

            LastArrow = new StaticImage("chevronleftbold.png", 48, tint);
            LastArrow.Content.HorizontalOptions = LayoutOptions.Start;
            LastArrow.Content.VerticalOptions = LayoutOptions.Center;
            LastArrow.Content.HeightRequest = 20;
            NextArrow = new StaticImage("chevronrightbold.png", 48, tint);
            NextArrow.Content.HorizontalOptions = LayoutOptions.End;
            NextArrow.Content.VerticalOptions = LayoutOptions.Center;
            NextArrow.Content.HeightRequest = 20;

            if (AppSession.CurrentPageSearch <= 1)
            {
                LastArrow.Content.Opacity = 0;
            }

            if (AppSession.TotalPages <= 1)
            {
                NextArrow.Content.Opacity = 0;
            }

            TouchEffect.SetNativeAnimation(LastArrow.Content, true);
            TouchEffect.SetCommand(LastArrow.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (LastArrow.Content.Opacity == 0)
                        {

                        }
                        else
                        {
                            GetLast();
                        }
                    });
                }));

            TouchEffect.SetNativeAnimation(NextArrow.Content, true);
            TouchEffect.SetCommand(NextArrow.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        GetNext();
                    });
                }));

            TopNavBox.Children.Add(LastArrow.Content);
            TopNavBox.Children.Add(PageInfo.Content);
            TopNavBox.Children.Add(NextArrow.Content);
            PageContent.Children.Add(TopNavBox, 0, 0);

            PageContent.Children.Add(BuildSearchResultsCollectionView(), 0, 1);
            App.HideNavHeader();
        }

        private CollectionView BuildSearchResultsCollectionView()
        {
            searchResultsCollectionView = new SearchResultsCollectionView();
            recipesLayout = searchResultsCollectionView.GetCollectionView();
            searchResultsCollectionView.ShowRecipes(SetPageTotal);
            return recipesLayout;
        }

        private void RefreshCollectionView()
        {
            var searchResultsGroup = new RecipesCollectionViewSection(AppSession.SearchedRecipes);
            AppSession.searchResultsCollection.RemoveAt(0);
            AppSession.searchResultsCollection.Add(searchResultsGroup);
        }

        private async Task UpdateData()
        {
            await App.ShowLoading();
            AppSession.UpdateSearch = true;
            AppSession.SearchedRecipes = DataManager.SearchRecipes(AppSession.CurrentUser, AppSession.UpdateSearch);
            await Task.Delay(10);
            await App.HideLoading();
        }

        private void SetPageTotal()
        {
            AppSession.TotalPages = (AppSession.TotalRecipes + ApiBridge.ITEMS_PER_REQUEST - 1) / ApiBridge.ITEMS_PER_REQUEST;
            PageInfo.Content.WidthRequest = Units.HalfScreenWidth;
            PageInfo.Content.Text = "PAGE " + AppSession.CurrentPageSearch + "/" + AppSession.TotalPages + "\n(of " + AppSession.TotalRecipes + " recipes)";

            if (AppSession.CurrentPageSearch <= 1)
            {
                LastArrow.Content.Opacity = 0;
            }
            else
            {
                LastArrow.Content.Opacity = 1;
            }

            if (AppSession.CurrentPageSearch < AppSession.TotalPages)
            {
                NextArrow.Content.Opacity = 1;
            }
            else
            {
                NextArrow.Content.Opacity = 0;
            }

            if (AppSession.TotalRecipes <= ApiBridge.ITEMS_PER_REQUEST)
            {
                PageInfo.Content.Text = "1/1";
                NextArrow.Content.Opacity = 0;
                LastArrow.Content.Opacity = 0;
            }
            else if (AppSession.TotalRecipes < 1)
            {
                PageInfo.Content.Text = "1/1";
                NextArrow.Content.Opacity = 0;
                LastArrow.Content.Opacity = 0;
            }
        }
        bool allowUpdate = false;
        private void GetNext()
        {
            if (AppSession.CurrentPageSearch < AppSession.TotalPages)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    AppSession.GetNextPage = true;
                    AppSession.GetLastPage = false;
                    AppSession.CurrentPageSearch++;
                    if (!allowUpdate)
                    {
                        allowUpdate = true;
                        await UpdateData();
                        RefreshCollectionView();
                        allowUpdate = false;
                    }
                    AppSession.GetNextPage = false;
                    AppSession.GetLastPage = false;
                    SetPageTotal();
                });
            }
        }

        private void GetLast()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                AppSession.GetNextPage = false;
                AppSession.GetLastPage = true;
                if (AppSession.CurrentPageSearch > 1)
                {
                    AppSession.CurrentPageSearch--;
                }
                if (!allowUpdate)
                {
                    allowUpdate = true;
                    await UpdateData();
                    RefreshCollectionView();
                    allowUpdate = false;
                }
                AppSession.GetNextPage = false;
                AppSession.GetLastPage = false;
                SetPageTotal();
            });
        }

        public async override Task Update()
        {
            
            await base.Update();
            if (AppSession.CurrentPageSearch > AppSession.TotalPages)
            {
                AppSession.CurrentPageSearch = 1;
            }
            if (!isFirstTime)
            {
                await UpdateData();
                RefreshCollectionView();
                SetPageTotal();
            }
            isFirstTime = false;
            AppSession.UserCollectionRecipes = DataManager.GetFavouriteRecipes();
            AppSession.shoppingList = DataManager.GetShoppingList().Result;
            App.SetSubHeaderTitle("", null);
        }
    }
}