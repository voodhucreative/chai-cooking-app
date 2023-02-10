using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
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
using ChaiCooking.Views.CollectionViews.RecommendedRecipes;
using FFImageLoading.Transformations;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class RecommendedRecipes : Page
    {
        StackLayout TopNavBox;
        StaticLabel PageInfo;
        StaticImage LastArrow, NextArrow;

        RecommendedRecipesCollectionView recommendedRecipesCollectionView;
        CollectionView recipesLayout;

        StackLayout pageContainer;

        public RecommendedRecipes()
        {
            this.IsScrollable = false;
            this.IsRefreshable = false;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;
            AppSession.CurrentPageRec = 1;
            AppSession.TotalPages = 1;

            this.Id = (int)AppSettings.PageNames.RecommendedRecipes;
            this.Name = AppData.AppText.RECOMMENDED_RECIPES; ;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.SlideInFromRight;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.SlideOutToRight;

            pageContainer = new StackLayout
            {
                Spacing = 0
            };

            PageContent = new Grid
            {
                Padding=0,
                ColumnSpacing=0,
                RowSpacing=0
            };

            TopNavBox = new StackLayout
            {
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                WidthRequest = Units.ScreenWidth,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_SPACING, 0)
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

            if (AppSession.CurrentPageRec <= 1)
            {
                LastArrow.Content.Opacity = 0;
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

            pageContainer.Children.Add(TopNavBox);
            pageContainer.Children.Add(BuildRecommendedRecipeCollection());
            PageContent.Children.Add(pageContainer);


            AppSession.UserCollectionRecipes = DataManager.GetFavouriteRecipes();
            AppSession.shoppingList = DataManager.GetShoppingList().Result;
        }

        public CollectionView BuildRecommendedRecipeCollection()
        {
            recommendedRecipesCollectionView = new RecommendedRecipesCollectionView();
            recipesLayout = recommendedRecipesCollectionView.GetCollectionView();
            recommendedRecipesCollectionView.ShowRecipes(SetPageTotal);
            return recipesLayout;
        }

        public void SetPageTotal()
        {
            AppSession.TotalPages = (AppSession.TotalRecipes + ApiBridge.ITEMS_PER_REQUEST - 1) / ApiBridge.ITEMS_PER_REQUEST;
            PageInfo.Content.WidthRequest = Units.HalfScreenWidth;
            PageInfo.Content.Text = "PAGE " + AppSession.CurrentPageRec + "/" + AppSession.TotalPages + "\n(of " + AppSession.TotalRecipes + " recipes)";

            if (AppSession.CurrentPageRec <= 1)
            {
                LastArrow.Content.Opacity = 0;
            }
            else
            {
                LastArrow.Content.Opacity = 1;
            }

            if (AppSession.CurrentPageRec < AppSession.TotalPages)
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

        public async Task UpdateData()
        {
            await App.ShowLoading();
            await Task.Delay(10);
            AppSession.RecommendedRecipes = DataManager.GetRecommendedRecipes(AppSession.CurrentUser, true);
            await App.HideLoading();
        }

        public void RefreshCollectionView()
        {
            try
            {
                var recommendedRecipesGroup = new RecipesCollectionViewSection(AppSession.RecommendedRecipes);
                AppSession.recommendedRecipesCollection.Add(recommendedRecipesGroup);
                AppSession.recommendedRecipesCollection.RemoveAt(0);
                recipesLayout.ScrollTo(0, animate: true);
            }
            catch(Exception e)
            {

            }
        }

        bool allowUpdate = false;
        public void GetNext()
        {
            if (AppSession.CurrentPageRec < AppSession.TotalPages)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    AppSession.GetNextPage = true;
                    AppSession.GetLastPage = false;
                    AppSession.CurrentPageRec++;
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

        public void GetLast()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                AppSession.GetNextPage = false;
                AppSession.GetLastPage = true;
                if (AppSession.CurrentPageRec > 1)
                {
                    AppSession.CurrentPageRec--;
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

        //Reload the recipe collection, quick but dirty.
        public override Task TransitionIn()
        {
            pageContainer.Children.Remove(recipesLayout);

            pageContainer.Children.Add(BuildRecommendedRecipeCollection());

            return base.TransitionIn();
        }

        public override async Task Update()
        {
            await base.Update();

            if (AppSession.CurrentUser.Preferences.DietTypes.Count < 1)
            {
                UserPreferences userPrefs = await App.ApiBridge.GetPreferences(AppSession.CurrentUser);

                if (userPrefs != null)
                {
                    AppSession.CurrentUser.Preferences = userPrefs;
                }
            }

            if (AppSession.CurrentPageRec > AppSession.TotalPages)
            {
                AppSession.CurrentPageRec = 1;
            }

            if (!allowUpdate)
            {
                allowUpdate = true;
                await UpdateData();
                RefreshCollectionView();
                allowUpdate = false;
            }
            SetPageTotal();
            App.SetSubHeaderTitle("", null);
        }
    }
}