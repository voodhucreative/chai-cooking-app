using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts;
using ChaiCooking.Layouts.Custom.Lists;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.InfluencerAPI;
using ChaiCooking.Views.CollectionViews.SingleInfluencer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    class Browse : Page
    {
        StackLayout NavContent, ContentContainer, listContainer;
        Frame contentFrame;
        InfluencerMealPlanList influencerMealPlanList;
        InfluencerMealPlans influencerMealPlans;
        InfluencerMealPlanCollectionView influencerMealPlanCollectionView;
        CollectionView influencerMealPlanLayout;
        public Browse()
        {
            this.IsScrollable = false;
            this.IsRefreshable = false;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;

            this.Id = (int)AppSettings.PageNames.InfluencerBrowse;
            this.Name = AppData.AppText.BROWSE_BUTTON_TEXT;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.SlideInFromRight;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.SlideOutToRight;

            PageContent = new Grid
            {
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                Padding = new Thickness(12, 0, 12, 12),
            };
        }

        public override Task Update()
        {
            PageContent.Children.Clear();
            App.SetSubHeaderTitle("", null);
            BuildGrid();
            App.SetSubHeaderTitle(AppText.RECOMMENDED_RECIPES, new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.RecommendedRecipes));
            return base.Update();
        }

        public void BuildGrid()
        {
            NavContent = new StackLayout { BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY), WidthRequest = Units.ScreenWidth, HeightRequest = Dimensions.NAVHEADER_HEIGHT, Orientation = StackOrientation.Horizontal, Padding = Dimensions.GENERAL_COMPONENT_PADDING };

            Label browseLabel = new Label
            {
                Text = AppText.BROWSE_BUTTON_TEXT,
                FontAttributes = FontAttributes.Bold,
                FontSize = Units.FontSizeL,
                TextColor = Color.White
            };

            ActiveLabel CloseLabel = new ActiveLabel(AppText.CLOSE, Units.FontSizeS, Color.Transparent, Color.White, null);
            TouchEffect.SetNativeAnimation(CloseLabel.Content, true);
            TouchEffect.SetCommand(CloseLabel.Content, new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.HealthyLiving);
                    // Doesn't go back to the Influencer page for some reason? So it goes back to HealthyLiving for now.
                });
            }));

            CloseLabel.RightAlign();

            NavContent.Children.Add(browseLabel);
            NavContent.Children.Add(CloseLabel.Content);

            StaticLabel browseDescLabel = new StaticLabel(AppText.BROWSE_DESC_TEXT);
            browseDescLabel.Content.TextColor = Color.White;
            browseDescLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            browseDescLabel.Content.FontSize = Units.FontSizeM;
            browseDescLabel.CenterAlign();

            StackLayout innerBorderCont = new StackLayout
            {
                Children =
                {
                    BuildCollectionView()
                }
            };

            StackLayout borderCont = new StackLayout
            {
                BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY),
                Padding = 2,
                Children =
                {
                    innerBorderCont
                }
            };

            StackLayout masterContainer = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Spacing = 0,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight,
                Children =
                {
                    NavContent,
                    browseDescLabel.Content,
                    borderCont
                }
            };

            PageContent.Children.Add(masterContainer);
        }

        private CollectionView BuildCollectionView()
        {
            influencerMealPlanCollectionView = new InfluencerMealPlanCollectionView();
            influencerMealPlanLayout = influencerMealPlanCollectionView.GetCollectionView();
            influencerMealPlanCollectionView.ShowMealPlansAll();
            return influencerMealPlanLayout;
        }
    }
}
