using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.DebugData.Custom;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using FFImageLoading.Transformations;
using Newtonsoft.Json;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class HealthyLiving : Page
    {
        StackLayout ContentContainer, SectionContainer, NavContent,
            CreateInfluencerBioContainer, ViewInfluencersContainer,
            ViewInfluencerMealPlansContainer;

        StaticLabel Title;
        Influencer newInfluencer;

        private const int CREATE_INFLUENCER_BIO = 0, VIEW_INFLUENCERS = 1,
            VIEW_INFLUENCER_MEAL_PLANS = 2;

        private int CurrentSection;
        private int influencerPage = 1;

        bool busy = false;

        public HealthyLiving()
        {
            this.IsScrollable = true;
            this.IsRefreshable = true;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;


            this.Id = (int)AppSettings.PageNames.HealthyLiving;
            this.Name = AppData.AppText.HEALTHY_LIVING;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.SlideInFromRight;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.SlideOutToRight;

            PageContent = new Grid
            {
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY)
            };

            //BuildNav();

            ContentContainer = BuildContent();
        }

        public void BuildNav()
        {
            NavContent = new StackLayout { BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY), WidthRequest = Units.ScreenWidth, HeightRequest = Dimensions.NAVHEADER_HEIGHT, Orientation = StackOrientation.Horizontal, Padding = Dimensions.GENERAL_COMPONENT_PADDING };

            IconLabel NavTitle = new IconLabel("hearticonsmall.png", AppText.MEAL_PLANNER, 200, Dimensions.HEADER_HEIGHT);
            NavTitle.TextContent.Content.TextColor = Color.White;
            TouchEffect.SetNativeAnimation(NavTitle.Content, true);
            TouchEffect.SetCommand(NavTitle.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    StaticData.mealPlanClicked = true;
                    StaticData.isCreating = false;

                    if (await App.AuthCheck((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.MPCalendar))
                    {
                        Console.WriteLine("Cool, we're authorized");
                    }
                });
            }));

            ActiveLabel CloseLabel = new ActiveLabel(AppText.CLOSE, Units.FontSizeS, Color.Transparent, Color.White, null);
            TouchEffect.SetNativeAnimation(CloseLabel.Content, true);
            TouchEffect.SetCommand(CloseLabel.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.ShowMenu();
                    await App.PerformActionAsync((int)Actions.ActionName.GoToPage, AppSession.LastPageId);
                });
            }));

            NavContent.Children.Add(NavTitle.Content);
            NavContent.Children.Add(CloseLabel.Content);

            App.SetNavHeaderContent(NavContent);
        }


        public StackLayout BuildContent()
        {
            ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING)

            };

            SectionContainer = new StackLayout
            {

            };

            CurrentSection = VIEW_INFLUENCERS;

            RefreshViews();

            SectionContainer.Children.Add(GetSection(CurrentSection));

            ContentContainer.Children.Add(SectionContainer);

            PageContent.Children.Add(ContentContainer);

            return ContentContainer;
        }


        public void SetSection(int sectionToShow)
        {
            RefreshViews();

            SectionContainer.Children.Clear();
            SectionContainer.Children.Add(GetSection(sectionToShow));
        }

        private StackLayout GetSection(int currentSection)
        {
            StackLayout currentContainer = ViewInfluencersContainer;
            switch (currentSection)
            {
                case VIEW_INFLUENCERS:
                    currentContainer = ViewInfluencersContainer;
                    break;
                case VIEW_INFLUENCER_MEAL_PLANS:
                    currentContainer = ViewInfluencerMealPlansContainer;
                    break;
                case CREATE_INFLUENCER_BIO:
                    currentContainer = CreateInfluencerBioContainer;
                    break;
            }
            return currentContainer;
        }

        private StackLayout BuildCreateInfluencerBio()
        {
            CreateInfluencerBioContainer = new StackLayout
            {
                BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY)
            };

            return CreateInfluencerBioContainer;
        }

        private StackLayout BuildViewInfluencers()
        {
            ViewInfluencersContainer = new StackLayout
            {
                BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY)
            };

            UpdateInfluencers();

            #region Pagination
            StackLayout NavBar = new StackLayout
            {
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.MediumButtonHeight,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_SPACING, 8)
            };

            StaticLabel pageInfo = new StaticLabel(string.Format("{0}/{1}", influencerPage, AppSession.TotalInfluencerPages));
            pageInfo.Content.TextColor = Color.White;
            pageInfo.Content.FontSize = Units.FontSizeL;
            pageInfo.CenterAlign();

            Color tintColour = Color.LightGray;
            TintTransformation colorTint = new TintTransformation
            {
                HexColor = (string)tintColour.ToHex(),
                EnableSolidColor = true

            };
            var tint = new List<FFImageLoading.Work.ITransformation>();
            tint.Add(colorTint);

            StaticImage prevArrow = new StaticImage("chevronleftbold.png", 48, tint);
            prevArrow.Content.HorizontalOptions = LayoutOptions.Start;
            prevArrow.Content.VerticalOptions = LayoutOptions.Center;
            prevArrow.Content.HeightRequest = 20;
            StaticImage nextArrow = new StaticImage("chevronrightbold.png", 48, tint);
            nextArrow.Content.HorizontalOptions = LayoutOptions.End;
            nextArrow.Content.VerticalOptions = LayoutOptions.Center;
            nextArrow.Content.HeightRequest = 20;

            if (influencerPage <= 1)
            {
                prevArrow.Content.Opacity = 0.5;
            }

            if (influencerPage >= AppSession.TotalInfluencerPages)
            {
                nextArrow.Content.Opacity = 0.5;
            }

            TouchEffect.SetNativeAnimation(prevArrow.Content, true);
            TouchEffect.SetCommand(prevArrow.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (prevArrow.Content.Opacity > 0.9)
                        {
                            influencerPage--;
                            SetSection(CurrentSection);
                        }
                    });
                }));

            TouchEffect.SetNativeAnimation(nextArrow.Content, true);
            TouchEffect.SetCommand(nextArrow.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (nextArrow.Content.Opacity > 0.9)
                        {
                            influencerPage++;
                            SetSection(CurrentSection);
                        }
                    });
                }));

            NavBar.Children.Add(prevArrow.Content);
            NavBar.Children.Add(pageInfo.Content);
            NavBar.Children.Add(nextArrow.Content);

            ViewInfluencersContainer.Children.Add(NavBar);
            #endregion

            Grid influencersListContainer = new Grid { ColumnSpacing = 0, RowSpacing = 0 };

            int row = 0;
            int col = 0;

            foreach (Influencer.Datum influencer in newInfluencer.data)
            {
                Grid inf = new Grid { WidthRequest = Units.ThirdScreenWidth, HeightRequest = Units.ThirdScreenWidth, BackgroundColor = Color.Transparent };

                // create influencer tile a
                InfluencerTile itile = new InfluencerTile(influencer);
                TouchEffect.SetNativeAnimation(itile.Content, true);
                TouchEffect.SetCommand(itile.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (AppSession.InfoModeOn)
                        {
                            App.ShowInfoBubble(new Paragraph("Influencer", "Tap on an influencer / user to see more details about them and options to use their Meal Plan.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                        }
                        else
                        {
                            //If a user taps several influencers at the same time the data getting pulled seems to get messed up and was causing a crash.
                            if (!busy)
                            {
                                busy = true;
                                AppSession.SelectedInfluencer = influencer;
                                StaticData.selectedInfluencerId = influencer.id;
                                AppSession.BrowsePlans = false;
                                await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.SingleInfluencer);
                                busy = false;
                            }
                        }
                    });
                }));

                inf.Children.Add(itile.Content);
                influencersListContainer.Children.Add(inf, col, row);

                col++;
                if (col >= 3) { col = 0; row++; }

            }
            InfluencerTile browseTile = new InfluencerTile(new Models.Custom.Influencer.Datum { display_name = AppText.BROWSE_BUTTON_TEXT, image_url = "browseplans.jpg" });
            TouchEffect.SetNativeAnimation(browseTile.Content, true);
            TouchEffect.SetCommand(browseTile.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("Browse", "Tap the 'Browse' button to look through all the Meal Plans uploaded by influencers and other users.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                    }
                    else
                    {
                        AppSession.BrowsePlans = true;
                        await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.InfluencerBrowse);
                    }
                    //App.ShowAlert("Coming soon...");
                });
            }));

            influencersListContainer.Children.Add(browseTile.Content, col, row);

            ViewInfluencersContainer.Children.Add(influencersListContainer);

            return ViewInfluencersContainer;
        }

        private StackLayout BuildViewInfluencerMealPlans()
        {
            ViewInfluencerMealPlansContainer = new StackLayout
            {
                BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY)
            };

            return ViewInfluencerMealPlansContainer;
        }

        private void RefreshViews()
        {
            CreateInfluencerBioContainer = BuildCreateInfluencerBio();
            ViewInfluencersContainer = BuildViewInfluencers();
            ViewInfluencerMealPlansContainer = BuildViewInfluencerMealPlans();
        }

        public override async Task Update()
        {
            await base.Update();
            App.SetSubHeaderTitle(AppText.RECOMMENDED_RECIPES, new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.RecommendedRecipes));
        }

        public void UpdateInfluencers()
        {
            Task<Influencer> task = Task.Run(async () => newInfluencer = await App.ApiBridge.GetInfluencers(AppSession.CurrentUser, influencerPage));
            task.Wait();
        }
    }
}
