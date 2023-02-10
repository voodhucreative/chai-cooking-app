using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Composites;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts;
using ChaiCooking.Layouts.Custom;
using ChaiCooking.Layouts.Custom.Modals;
using ChaiCooking.Layouts.Custom.Panels;
using ChaiCooking.Layouts.Custom.Panels.Account;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.MealPlanAPI;
using ChaiCooking.Pages;
using ChaiCooking.Pages.Custom;
using ChaiCooking.Services;
using Plugin.DeviceInfo;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace ChaiCooking
{

    // The AppContainer is the controlling view that handles the loading,
    // unloading, transitioning and overall presentation of all other pages.

    public class AppContainter : ContentPage
    {
        public new int Width { get; set; }
        public new int Height { get; set; }

        private AppSettings AppSettings;

        // the top level container for all other content, including overlays and slide on menus
        private Layer ParentContainer;

        // holds header, main content layers and footer
        private Layer MainLayoutContainer;

        private StackLayout MainLayoutStack;

        // holds all over layers below
        private Layer MainLayout;

        // main background layer
        private Layer BackgroundLayer;

        // effects, parallax, etc
        private Layer BackgroundOverlayLayer;

        // main page layer
        private Layer ContentLayer;

        // panels layer
        private Layer PanelLayer;

        // menu layer
        private Layer MenuLayer;

        // menu layer
        private Layer MenuForegroundLayer;

        // anything presented on top of main content
        private Layer ForegroundLayer;

        // effects, parallax, etc
        private Layer ForegroundOverlayLayer;

        // full screen modal layer
        private Layer ModalBackgroundLayer;

        // full screen modal layer
        private Layer ModalContentLayer;

        // loading spinner
        private ActivityIndicator Spinner;
        private StackLayout AppStatusBar;

        private Layouts.Header Header;
        private Layouts.Footer Footer;

        //private Layouts.Menu Menu;

        private int ContentHeight;

        private MainHeader MainHeader;
        private SubHeader SubHeader;
        private NavHeader NavHeader;

        Pages.Custom.Menu MenuPage;

        // Pages
        Landing LandingPage;
        AboutUs AboutUsPage;
        //Menu MenuPage;
        Questionnaire QuestionnairePage;
        RecommendedRecipes RecommendedRecipesPage;
        AccountPreferences AccountPreferencesPage;
        Leaderboard LeaderboardPage;
        Legals LegalsPage;
        ShoppingBasket ShoppingBasketPage;
        Tracker TrackerPage;
        YourCharacter YourCharacterPage;
        //MealPlanPreview MealPlanPage;
        MealPlanCalendar MealPlanCalendar;
        Calendar Calendar;
        CookItCorner CookItCornerPage;
        WasteLess WasteLessPage;
        HealthyLiving HealthyLivingPage;
        Videos VideosPage;
        UpgradePlan UpgradePlanPage;
        LoginAndRegistration LoginAndRegistrationPage;
        SingleInfluencer SingleInfluencerPage;
        Browse InfluencerBrowsePage;
        SearchResults SearchResults;
        InfluencerBio InfluencerBio;
        MyRecipes MyRecipes;
        RecipeEditor RecipeEditor;

        Collections collections;

        // panels
        //CreateMealPlanPanel CreateMealPlanPanel;
        RecipeOptionsPanel RecipeOptionsPanel;
        RecommendedRecipeFilterPanel RecommendedRecipeFilterPanel;

        SubscribeModal SubscribeModal;

        Label LoadingLabel;

        public AppContainter(int width, int height)
        {
            // full screen height
            Width = width;
            Height = height;
            ContentHeight = height;
            BackgroundColor = Color.Black;//FromHex(Colors.CC_ORANGE);

            AppSettings = new AppSettings();
            AppSettings.FullScreenHeight = height;
            Content = BuildAppLayout(width, height).Layout;

            if (AppSettings.HasStatusBar)
            {
                On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(false);
                this.Padding = new Thickness(0, AppSettings.StatusBarHeight, 0, 0);
                //this.Padding = new Thickness(0, 8, 0, 0);

            }

            Populate();
        }

        private Layer BuildAppLayout(int width, int height)
        {
            // create all layers
            ParentContainer = new Layer(width, height);


            // stack to hold header, main layout and footer (header and footer are optional)
            MainLayoutStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                WidthRequest = width,
                HeightRequest = height,
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Green//FromHex(Colors.CC_DARK_BLUE_GREY)
            };

            // stack to hold header, main layout and footer (header and footer are optional)
            BackgroundLayer = new Layer(width, height);
            BackgroundLayer.Deactivate();

            BackgroundOverlayLayer = new Layer(width, height);
            BackgroundOverlayLayer.Deactivate();

            ContentLayer = new Layer(width, height);
            ContentLayer.Deactivate();

            MenuLayer = new Layer(width, height);
            MenuLayer.Layout.BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY);
            // add the menu to the content layer

            MenuPage = new Pages.Custom.Menu();

            MenuLayer.Layout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            MenuLayer.Layout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            SearchField MenuSearch = new SearchField("search");
            MenuSearch.Content.WidthRequest = Dimensions.MAIN_MENU_WIDTH;
            MenuSearch.Content.Margin = new Thickness((Units.ScreenWidth - AppSettings.MenuCoverageHorizontal) + Dimensions.GENERAL_COMPONENT_SPACING, Dimensions.GENERAL_COMPONENT_PADDING, Dimensions.GENERAL_COMPONENT_SPACING, Dimensions.GENERAL_COMPONENT_PADDING);
            MenuLayer.Layout.Children.Add(MenuSearch.Content, 0, 0);
            MenuLayer.Layout.Children.Add(MenuPage.GetContent(), 0, 1);

            MenuLayer.Layout.TranslateTo(0, Dimensions.HEADER_HEIGHT, 0, Easing.Linear);
            MenuLayer.Deactivate();

            PanelLayer = new Layer(width, height);
            PanelLayer.Layout.BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY);
            PanelLayer.Layout.Opacity = 1;
            PanelLayer.Layout.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                //await Update();
                                await TogglePanel();
                            });
                        })
                    }
                );

            ForegroundLayer = new Layer(width, height);
            ForegroundLayer.Layout.BackgroundColor = Color.Transparent;
            ForegroundLayer.Layout.Opacity = 1;
            ForegroundLayer.Layout.HorizontalOptions = LayoutOptions.StartAndExpand;
            ForegroundLayer.Layout.VerticalOptions = LayoutOptions.StartAndExpand;
            //ForegroundLayer.Layout.Margin = new Thickness(0, Units.ScreenHeight25Percent, 0, Units.ThirdScreenHeight);



            Spinner = new ActivityIndicator
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = Units.ScreenWidth25Percent,
                WidthRequest = Units.ScreenWidth25Percent,
                Color = Color.White,
                IsRunning = false,
            };

            LoadingLabel = new Label
            {
                Text = "Loading...",
                FontFamily = Fonts.GetRegularAppFont(),
                FontSize = Units.FontSizeM,
                TextColor = Color.Gray,
                VerticalOptions = LayoutOptions.StartAndExpand,
            };

            AppStatusBar = new StackLayout
            {
                WidthRequest = Units.ScreenWidth,
                //HeightRequest = 40,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.White,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING, 0, 0, 0),
                Opacity = 0,

                Children =
                {
                    LoadingLabel
                }
            };

            ForegroundOverlayLayer = new Layer(width, height);
            ForegroundOverlayLayer.Layout.BackgroundColor = Color.Transparent;
            ForegroundOverlayLayer.Layout.Opacity = 0;
            //ForegroundOverlayLayer.Layout.Children.Add(Spinner);
            ForegroundOverlayLayer.Deactivate();

            ModalBackgroundLayer = new Layer(width + 4, height + 4);
            ModalBackgroundLayer.Layout.HorizontalOptions = LayoutOptions.CenterAndExpand;
            ModalBackgroundLayer.Layout.VerticalOptions = LayoutOptions.CenterAndExpand;
            ModalBackgroundLayer.Layout.BackgroundColor = Color.Black;
            ModalBackgroundLayer.Layout.Opacity = AppSettings.ModalOpacity;
            ModalBackgroundLayer.Deactivate();

            ModalContentLayer = new Layer(width + 4, height + 4);
            ModalContentLayer.Layout.HorizontalOptions = LayoutOptions.CenterAndExpand;
            ModalContentLayer.Layout.VerticalOptions = LayoutOptions.CenterAndExpand;
            ModalContentLayer.Layout.BackgroundColor = Color.Transparent;
            ModalContentLayer.Layout.Opacity = 1;
            ModalContentLayer.Deactivate();

            // create header, if required
            if (AppSettings.HasHeader)
            {
                Header = new Layouts.Header();
                Header.SetHeight(AppSettings.HeaderHeight); // test
                MainHeader = new MainHeader();
                Header.Content = MainHeader.Content;
                Header.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            //ToggleMenu();// LayoutTest();
                        })
                    }
                );
            }
            SubHeader = new SubHeader();
            NavHeader = new NavHeader();

            // create footer, if required
            if (AppSettings.HasFooter)
            {
                Footer = new Layouts.Footer();
                Footer.Content.HeightRequest = AppSettings.FooterHeight; // test
            }

            // add the header to the main layout stack
            if (AppSettings.HasHeader)
            {
                MainLayoutStack.Children.Add(Header.Content);
            }

            MainLayoutStack.Children.Add(AppStatusBar);

            if (AppSettings.HasSubHeader)
            {
                MainLayoutStack.Children.Add(SubHeader.Content);
            }

            MainLayoutStack.Children.Add(NavHeader.Content);


            // create main content layer, always
            MainLayout = new Layer(width, ContentHeight);
            MainLayout.Layout.Children.Add(ContentLayer.Layout);
            MainLayoutStack.Children.Add(MainLayout.Layout);

            // add the footer to the main layout stack
            if (AppSettings.HasFooter)
            {
                MainLayoutStack.Children.Add(Footer.Content);
            }


            // replace standard background with a background image
            MainLayoutStack.BackgroundColor = Color.Transparent;// Purple;// FromHex(Colors.CC_BLUE_GREY);

            //BackgroundLayer.AddBackgroundImage("sep2.jpg");
            SetBackgroundImage("sep2.jpg");
            SetBackgroundColor(Color.FromHex(Colors.CC_BLUE_GREY));
            BackgroundLayer.SetBackGroundImageAspect(Aspect.AspectFill);
            BackgroundLayer.BackgroundImage.Content.Margin = new Thickness(0, 0, 0, 0);

            ShowBackgroundImage();

            ParentContainer.Layout.Children.Add(BackgroundLayer.Layout, 0, 0);
            ParentContainer.Layout.Children.Add(BackgroundOverlayLayer.Layout, 0, 0);
            ParentContainer.Layout.Children.Add(MainLayoutStack, 0, 0);
            ParentContainer.Layout.Children.Add(MenuLayer.Layout, 0, 0);

            //ParentContainer.Layout.Children.Add(PanelLayer.Layout, 0, 0);

            ParentContainer.Layout.Children.Add(ForegroundLayer.Layout, 0, 0);
            
            ParentContainer.Layout.Children.Add(ModalBackgroundLayer.Layout, 0, 0);
            ParentContainer.Layout.Children.Add(ModalContentLayer.Layout, 0, 0);

            ParentContainer.Layout.Children.Add(PanelLayer.Layout, 0, 0);
            ParentContainer.Layout.Children.Add(ForegroundOverlayLayer.Layout, 0, 0);

            PanelLayer.Deactivate();
            ForegroundLayer.Deactivate();
            ForegroundOverlayLayer.Deactivate();
            ModalContentLayer.Deactivate();


            // test
            ParentContainer.Layout.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            //ToggleMenu();
                        })
                    }
                );

            Device.BeginInvokeOnMainThread(async () =>
            {
                await HideFooter();
                await HideHeader();
                await HideSubHeader();
                await HideNavHeader();
                await HideMenu();
            });

            AppStatusBar.IsVisible = false;

            return ParentContainer;
        }


        bool LoadAll = true;

        private void Populate()
        {
            if (App.IsBusy) return;
            App.IsBusy = true;

            Console.WriteLine("1) Time now: " + DateTime.Now);

            int updateDelay = 10;
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(updateDelay);


                // Pages
                LandingPage = new Landing();
                Helpers.Pages.AddPage(LandingPage);

                // we can do this elsewhere, but for now, add it here
                await Helpers.Pages.GetCurrent().Update();
                ContentLayer.Layout.Children.Add(Helpers.Pages.GetCurrent().GetContent());

                if (Helpers.Pages.GetCurrent().HasFooter)
                {
                    await ShowFooter();
                }
                else
                {
                    await HideFooter();
                }

                if (Helpers.Pages.GetCurrent().HasHeader)
                {
                    await ShowHeader();
                }
                else
                {
                    await HideHeader();
                }

                if (Helpers.Pages.GetCurrent().HasSubHeader)
                {
                    await ShowSubHeader();
                }
                else
                {
                    await HideSubHeader();
                }

                if (Helpers.Pages.GetCurrent().HasNavHeader)
                {
                    await ShowNavHeader();
                }
                else
                {
                    await HideNavHeader();
                }

                ////////// END POPULATE CONTENT LAYER //////////////////////////

                // activate the content layer
                ContentLayer.Activate();
            });

            // misc panels
            //CreateMealPlanPanel = new CreateMealPlanPanel();
            RecommendedRecipeFilterPanel = new RecommendedRecipeFilterPanel();

            Console.WriteLine("2) Session start: " + AppSession.StartTime);
            Console.WriteLine("3) Time now: " + DateTime.Now);

            App.IsBusy = false;
        }

        public async Task<bool> LoadPage(int pageId)
        {
            await Task.Delay(1);
            switch (pageId)
            {
                case (int)AppSettings.PageNames.Landing:
                    if (LandingPage == null)
                    {
                        LandingPage = new Landing();
                        Helpers.Pages.AddPage(LandingPage);
                    }
                    break;
                case (int)AppSettings.PageNames.LoginAndRegistration:
                    if (LoginAndRegistrationPage == null)
                    {
                        LoginAndRegistrationPage = new LoginAndRegistration();
                        Helpers.Pages.AddPage(LoginAndRegistrationPage);
                    }
                    break;
                case (int)AppSettings.PageNames.Questionnaire:
                    if (QuestionnairePage == null)
                    {
                        QuestionnairePage = new Questionnaire();
                        Helpers.Pages.AddPage(QuestionnairePage);
                    }
                    break;
                case (int)AppSettings.PageNames.RecommendedRecipes:
                    if (RecommendedRecipesPage == null)
                    {
                        RecommendedRecipesPage = new RecommendedRecipes();
                        Helpers.Pages.AddPage(RecommendedRecipesPage);
                    }
                    break;
                case (int)AppSettings.PageNames.AccountPreferences:
                    if (AccountPreferencesPage == null)
                    {
                        AccountPreferencesPage = new AccountPreferences();
                        Helpers.Pages.AddPage(AccountPreferencesPage);
                    }
                    break;
                case (int)AppSettings.PageNames.Leaderboard:
                    if (LeaderboardPage == null)
                    {
                        LeaderboardPage = new Leaderboard();
                        Helpers.Pages.AddPage(LeaderboardPage);
                    }
                    break;
                case (int)AppSettings.PageNames.Legals:
                    if (LegalsPage == null)
                    {
                        LegalsPage = new Legals();
                        Helpers.Pages.AddPage(LegalsPage);
                    }
                    break;
                case (int)AppSettings.PageNames.ShoppingBasket:
                    if (ShoppingBasketPage == null)
                    {
                        ShoppingBasketPage = new ShoppingBasket();
                        Helpers.Pages.AddPage(ShoppingBasketPage);
                    }
                    break;
                case (int)AppSettings.PageNames.Tracker:
                    if (TrackerPage == null)
                    {
                        TrackerPage = new Tracker();
                        Helpers.Pages.AddPage(TrackerPage);
                    }
                    break;
                case (int)AppSettings.PageNames.YourCharacter:
                    if (YourCharacterPage == null)
                    {
                        YourCharacterPage = new YourCharacter();
                        Helpers.Pages.AddPage(YourCharacterPage);
                    }
                    break;
                case (int)AppSettings.PageNames.Calendar:
                    if (Calendar == null)
                    {
                        Calendar = new Calendar();
                        Helpers.Pages.AddPage(Calendar);
                    }
                    break;
                case (int)AppSettings.PageNames.MPCalendar:
                    if (MealPlanCalendar == null)
                    {
                        MealPlanCalendar = new MealPlanCalendar();
                        Helpers.Pages.AddPage(MealPlanCalendar);
                    }
                    break;
                case (int)AppSettings.PageNames.CookItCorner:
                    if (CookItCornerPage == null)
                    {
                        CookItCornerPage = new CookItCorner();
                        Helpers.Pages.AddPage(CookItCornerPage);
                    }
                    break;
                case (int)AppSettings.PageNames.WasteLess:
                    if (WasteLessPage == null)
                    {
                        WasteLessPage = new WasteLess();
                        Helpers.Pages.AddPage(WasteLessPage);
                    }
                    break;
                case (int)AppSettings.PageNames.HealthyLiving:
                    if (HealthyLivingPage == null)
                    {
                        HealthyLivingPage = new HealthyLiving();
                        Helpers.Pages.AddPage(HealthyLivingPage);
                    }
                    break;
                case (int)AppSettings.PageNames.Videos:
                    if (VideosPage == null)
                    {
                        VideosPage = new Videos();
                        Helpers.Pages.AddPage(VideosPage);
                    }
                    break;
                case (int)AppSettings.PageNames.UpgradePlan:
                    if (UpgradePlanPage == null)
                    {
                        UpgradePlanPage = new UpgradePlan();
                        Helpers.Pages.AddPage(UpgradePlanPage);
                    }
                    break;
                case (int)AppSettings.PageNames.SingleInfluencer:
                    if (SingleInfluencerPage == null)
                    {
                        SingleInfluencerPage = new SingleInfluencer();
                        Helpers.Pages.AddPage(SingleInfluencerPage);
                    }
                    break;
                case (int)AppSettings.PageNames.InfluencerBrowse:
                    if (InfluencerBrowsePage == null)
                    {
                        InfluencerBrowsePage = new Browse();
                        Helpers.Pages.AddPage(InfluencerBrowsePage);
                    }
                    break;

                case (int)AppSettings.PageNames.SearchResults:
                    if (SearchResults == null)
                    {
                        SearchResults = new SearchResults();
                        Helpers.Pages.AddPage(SearchResults);
                    }
                    break;
                case (int)AppSettings.PageNames.MyRecipes:
                    if (MyRecipes == null)
                    {
                        MyRecipes = new MyRecipes();
                        Helpers.Pages.AddPage(MyRecipes);
                    }
                    break;
                case (int)AppSettings.PageNames.RecipeEditor:
                    if (RecipeEditor == null)
                    {
                        RecipeEditor = new RecipeEditor();
                        Helpers.Pages.AddPage(RecipeEditor);
                    }
                    break;
                case (int)AppSettings.PageNames.Collections:
                    if (collections == null)
                    {
                        collections = new Collections();
                        Helpers.Pages.AddPage(collections);
                    }
                    break;
                case (int)AppSettings.PageNames.InfluencerBio:
                    if (InfluencerBio == null)
                    {
                        InfluencerBio = new InfluencerBio();
                        Helpers.Pages.AddPage(InfluencerBio);
                    }
                    break;
            }
            return true;
        }

        // header
        public async Task<bool> ToggleHeader()
        {
            if (Header.Content.IsVisible)
            {
                await HideHeader();
            }
            else
            {
                await ShowHeader();
            }
            return true;
        }

        public async Task<bool> HideHeader()
        {
            await Header.Hide();
            UpdateContentSizeAndPosition();
            return true;
        }

        public async Task<bool> ShowHeader()
        {
            await Header.Show();
            UpdateContentSizeAndPosition();
            return true;
        }

        // sub header
        public async Task<bool> ToggleSubHeader()
        {
            if (SubHeader.Content.IsVisible)
            {
                await HideSubHeader();
            }
            else
            {
                await ShowSubHeader();
            }
            return true;
        }

        public async Task<bool> HideSubHeader()
        {
            await SubHeader.Hide();
            UpdateContentSizeAndPosition();
            return true;
        }

        public async Task<bool> ShowSubHeader()
        {
            await SubHeader.Show();
            UpdateContentSizeAndPosition();
            return true;
        }

        // nav header
        public async Task<bool> ToggleNavHeader()
        {
            if (NavHeader.Content.IsVisible)
            {
                await HideNavHeader();
            }
            else
            {
                await ShowNavHeader();
            }
            return true;
        }

        public async Task<bool> HideNavHeader()
        {
            NavHeader.TransitionType = (int)AppSettings.TransitionTypes.SlideOutTop;
            NavHeader.TransitionTime = 50;
            await NavHeader.Hide();
            UpdateContentSizeAndPosition();
            return true;
        }

        public async Task<bool> ShowNavHeader()
        {
            await NavHeader.Show();
            UpdateContentSizeAndPosition();
            return true;
        }

        public void ClearNavHeader()
        {
            NavHeader.ClearContent();
        }

        public void SetNavHeaderContent(View view)
        {
            NavHeader.SetContent(view);
        }

        // modal
        public async Task<bool> ToggleModal()
        {
            if (ModalContentLayer.IsActive())
            {
                await HideModal();
            }
            else
            {
                await ShowModal(null);
            }
            return true;
        }

        public async Task<bool> HideModal()
        {
            await ModalContentLayer.Layout.FadeTo(0, 0, Easing.Linear);
            await ModalBackgroundLayer.Layout.FadeTo(0, 0, Easing.Linear);

            ModalContentLayer.Deactivate();
            ModalBackgroundLayer.Deactivate();
            ModalContentLayer.Layout.Children.Clear();
            ParentContainer.Layout.Children.Remove(ModalContentLayer.Layout);
            return true;
        }

        // foreground
        public async Task<bool> ToggleForeground()
        {
            if (ForegroundLayer.IsActive())
            {
                await HideForeground();
            }
            else
            {
                await ShowForeground();
            }
            return true;
        }

        public async Task<bool> SetForegroundContent(View view)
        {
            ForegroundLayer.Layout.Children.Clear();

            ForegroundLayer.Layout.Children.Add(view, 0, 0);
            await ShowForeground();
            return true;

        }




        public async Task<bool> ShowForeground()
        {
            ForegroundLayer.Activate();
            ParentContainer.Layout.Children.Add(ForegroundLayer.Layout, 0, 0);

            await ForegroundLayer.Layout.FadeTo(1, 250, Easing.Linear);

            return true;
        }

        public async Task<bool> SetForegroundOverlayContent(View view)
        {
            ForegroundOverlayLayer.Layout.Children.Clear();

            ForegroundOverlayLayer.Layout.Children.Add(view, 0, 0);
            await ShowForegroundOverlay();
            return true;

        }

        public async Task<bool> ShowForegroundOverlay()
        {
            ForegroundOverlayLayer.Activate();
            ParentContainer.Layout.Children.Add(ForegroundOverlayLayer.Layout, 0, 0);
            ParentContainer.Layout.RaiseChild(ForegroundOverlayLayer.Layout);
            await ForegroundOverlayLayer.Layout.FadeTo(1, 250, Easing.Linear);

            

            return true;
        }
        /*public async Task<bool> SetForegroundOverlayContent(View view)
        {
            ForegroundOverlayLayer.Layout.Children.Clear();

            ForegroundOverlayLayer.Layout.Children.Add(view, 0, 0);
            await ShowForegroundOverlay();
            return true;

        }




        public async Task<bool> ForegroundOverlayLayer()
        {
            ForegroundOverlayLayer.Activate();
            ParentContainer.Layout.Children.Add(ForegroundOverlayLayer.Layout, 0, 0);

            await ForegroundLayer.Layout.FadeTo(1, 250, Easing.Linear);

            return true;
        }*/

        public async Task<bool> HideForeground()
        {
            await ForegroundLayer.Layout.FadeTo(0, 250, Easing.Linear);
            ForegroundLayer.Deactivate();
            ForegroundLayer.Layout.Children.Clear();
            ParentContainer.Layout.Children.Remove(ForegroundLayer.Layout);
            return true;
        }

        public async Task<bool> HideForegroundOverlay()
        {
            await ForegroundOverlayLayer.Layout.FadeTo(0, 250, Easing.Linear);
            ForegroundOverlayLayer.Deactivate();
            ForegroundOverlayLayer.Layout.Children.Clear();
            ParentContainer.Layout.Children.Remove(ForegroundOverlayLayer.Layout);
            return true;
        }

        // panel
        public async Task<bool> TogglePanel()
        {
            if (!PanelLayer.DefaultEventDisabled)
            {
                if (PanelLayer.IsActive())
                {
                    await HidePanel();
                }
                else
                {
                    await ShowPanel();
                }
            }
            return true;
        }

        public async Task<bool> ShowPanel()
        {
            PanelLayer.DefaultEventDisabled = true;
            PanelLayer.Layout.Children.Clear();

            // add content to modal here
            await Task.Delay((int)AppSettings.TransitionVeryFast);

            //ActionResultPanel modalPanel = new ActionResultPanel(false);
            //modalPanel.SetBackground(Color.Green, AppSettings.ModalOpacity);
            //PanelLayer.Layout.Children.Add(modalPanel.Content);

            GeneralInfoPanel generalInfoPanel = new GeneralInfoPanel("Dialog Box", Themes.PANEL_THEME_GREY, Dimensions.INFO_PANEL_WIDTH);
            generalInfoPanel.AddContent(new Label { Text = "This is a panel", TextColor = Color.Gray });
            generalInfoPanel.AddContent(new Label { Text = "With some arbirary information in it", TextColor = Color.Green });
            generalInfoPanel.AddContent(new Label { Text = "Click me to close" });
            generalInfoPanel.AddButton("goodbye", 0, Actions.GetAction((int)Actions.ActionName.HidePanel), true);
            generalInfoPanel.Content.Padding = Dimensions.PANEL_BORDER_WIDTH;
            generalInfoPanel.Content.BackgroundColor = Color.FromHex(Colors.CC_DARK_GREY);
            View modalContent = generalInfoPanel.GetContent(); // get this from modal manager class, once implemented
            PanelLayer.Layout.Children.Add(modalContent);



            int MarginLeft = Units.ScreenWidth - (Dimensions.RIGHT_MENU_WIDTH + Units.TapSizeXXS);
            int MarginRight = Units.TapSizeXXS;
            int MarginTop = Dimensions.HEADER_HEIGHT + Dimensions.SUBHEADER_HEIGHT + Dimensions.NAVHEADER_HEIGHT;
            PanelLayer.Layout.Margin = new Thickness(MarginLeft, MarginTop, MarginRight, Units.TapSizeXXS);

            PanelLayer.Activate();
            await PanelLayer.Layout.TranslateTo(0, 0, (uint)AppSettings.TransitionVeryFast, Easing.Linear);
            //await PanelLayer.Layout.FadeTo(1, 250, Easing.Linear);

            //ParentContainer.Layout.Children.Add(PanelLayer.Layout, 0, 0);
            return true;

        }

        public async Task<bool> HidePanel()
        {
            await PanelLayer.Layout.TranslateTo((int)(Dimensions.RIGHT_MENU_WIDTH + Units.TapSizeXXS), 0, (uint)AppSettings.TransitionVeryFast, Easing.Linear);
            //await PanelLayer.Layout.FadeTo(0, 250, Easing.Linear);
            PanelLayer.Deactivate();
            //PanelLayer.Layout.Children.Clear();
            return true;
        }

        public async Task<bool> ToggleFooter()
        {
            if (AppSettings.HasFooter)
            {
                if (Footer.Content.IsEnabled)
                {
                    await HideFooter();
                }
                else
                {
                    await ShowFooter();
                }
            }
            return true;
        }

        public async Task<bool> HideFooter()
        {
            if (AppSettings.HasFooter)
            {
                await Footer.Content.TranslateTo(0, Units.FooterHeight, 150, Easing.Linear);
                Footer.Content.IsEnabled = false;
                UpdateContentSizeAndPosition();
            }
            return true;
        }

        public async Task<bool> ShowFooter()
        {
            if (AppSettings.HasFooter)
            {
                await Footer.Content.TranslateTo(0, 0, 150, Easing.Linear);
                Footer.Content.IsEnabled = true;
                UpdateContentSizeAndPosition();
            }
            return true;
        }

        public async Task<bool> ToggleMenu()
        {
            // menu is onscreen, so we're turning it off
            if (MenuLayer.Layout.IsVisible)
            {
                await HideMenu();
                MainHeader.ShowMenuClosed();
            }
            else
            {

                await ShowMenu();
                MainHeader.ShowMenuOpen();
            }

            return true;
        }

        public async Task<bool> ShowMenu()
        {
            //Commented these delays out as they were causing a delay between the button press and them menu sliding out.
            //await Task.Delay(25);
            /*await*/ MenuPage.Update();
            Device.BeginInvokeOnMainThread(async () =>
            {
                MenuLayer.Activate();
                switch (AppSettings.MenuPosition)
                {
                    case (int)AppSettings.MenuPositions.Top:
                        if (AppSettings.MenuShownOverContent)
                        {
                            await Task.WhenAll(
                                MenuLayer.Layout.TranslateTo(0, (0 - (Units.ScreenHeight - AppSettings.MenuCoverageVertical)), 250, Easing.CubicIn)
                            );
                        }
                        else
                        {
                            await Task.WhenAll(
                                MainLayoutStack.TranslateTo(0, Units.HalfScreenHeight, 350, Easing.CubicIn),
                                MenuLayer.Layout.TranslateTo(0, (0 - (Units.ScreenHeight - AppSettings.MenuCoverageVertical)), 250, Easing.CubicIn)
                            );
                        }
                        break;
                    case (int)AppSettings.MenuPositions.Bottom:
                        if (AppSettings.MenuShownOverContent)
                        {
                            await Task.WhenAll(
                                MenuLayer.Layout.TranslateTo(0, (Units.ScreenHeight - AppSettings.MenuCoverageVertical), 250, Easing.CubicIn)
                            );
                        }
                        else
                        {
                            await Task.WhenAll(
                                MainLayoutStack.TranslateTo(0, -Units.HalfScreenHeight, 350, Easing.CubicIn),
                                MenuLayer.Layout.TranslateTo(0, (Units.ScreenHeight - AppSettings.MenuCoverageVertical), 250, Easing.CubicIn)
                            );
                        }
                        break;
                    case (int)AppSettings.MenuPositions.Left:
                        if (AppSettings.MenuShownOverContent)
                        {
                            await Task.WhenAll(
                                MenuLayer.Layout.TranslateTo(0 - (Units.ScreenWidth - AppSettings.MenuCoverageHorizontal), AppSettings.MenuYPosition, 250, Easing.CubicIn)
                            );
                        }
                        else
                        {
                            await Task.WhenAll(
                                MainLayoutStack.TranslateTo(Units.ScreenWidth, 0, 350, Easing.CubicIn),
                                MenuLayer.Layout.TranslateTo(0 - (Units.ScreenWidth - AppSettings.MenuCoverageHorizontal), AppSettings.MenuYPosition, 250, Easing.CubicIn)
                            );
                        }
                        break;
                    case (int)AppSettings.MenuPositions.Right:
                        if (AppSettings.MenuShownOverContent)
                        {
                            await Task.WhenAll(
                               MenuLayer.Layout.TranslateTo(Units.ScreenWidth - AppSettings.MenuCoverageHorizontal, AppSettings.MenuYPosition, 250, Easing.CubicIn)
                           );
                        }
                        else
                        {
                            await Task.WhenAll(
                                MainLayoutStack.TranslateTo(-AppSettings.MenuCoverageHorizontal, 0, 350, Easing.CubicIn),
                                MenuLayer.Layout.TranslateTo(Units.ScreenWidth - AppSettings.MenuCoverageHorizontal, AppSettings.MenuYPosition, 250, Easing.CubicIn)
                            );
                        }
                        break;
                }
            });
            return true;
        }

        public async Task<bool> HideMenu()
        {
            //await Task.Delay(25);
            Device.BeginInvokeOnMainThread(async () =>
            {
                switch (AppSettings.MenuPosition)
                {
                    case (int)AppSettings.MenuPositions.Top:
                        if (AppSettings.MenuShownOverContent)
                        {
                            await Task.WhenAll(
                                MenuLayer.Layout.TranslateTo(0, -Units.ScreenHeight, 50, Easing.CubicOut)
                            );
                        }
                        else
                        {
                            await Task.WhenAll(
                                MainLayoutStack.TranslateTo(0, 0, 50, Easing.CubicOut),
                                MenuLayer.Layout.TranslateTo(0, -Units.ScreenHeight, 50, Easing.CubicOut)
                            );
                        }
                        break;
                    case (int)AppSettings.MenuPositions.Bottom:
                        if (AppSettings.MenuShownOverContent)
                        {
                            await Task.WhenAll(
                                MenuLayer.Layout.TranslateTo(0, Units.ScreenHeight, 50, Easing.CubicOut)
                            );
                        }
                        else
                        {
                            await Task.WhenAll(
                                MainLayoutStack.TranslateTo(0, 0, 50, Easing.CubicOut),
                                MenuLayer.Layout.TranslateTo(0, Units.ScreenHeight, 50, Easing.CubicOut)
                            );
                        }
                        break;
                    case (int)AppSettings.MenuPositions.Left:
                        if (AppSettings.MenuShownOverContent)
                        {
                            await Task.WhenAll(
                                MenuLayer.Layout.TranslateTo(-Units.ScreenWidth, AppSettings.MenuYPosition, 50, Easing.CubicOut)
                            );
                        }
                        else
                        {
                            await Task.WhenAll(
                                MainLayoutStack.TranslateTo(0, 0, 50, Easing.CubicOut),
                                MenuLayer.Layout.TranslateTo(-Units.ScreenWidth, AppSettings.MenuYPosition, 50, Easing.CubicOut)
                            );
                        }
                        break;
                    case (int)AppSettings.MenuPositions.Right:
                        if (AppSettings.MenuShownOverContent)
                        {
                            await Task.WhenAll(
                                MenuLayer.Layout.TranslateTo(Units.ScreenWidth, AppSettings.MenuYPosition, 50, Easing.CubicOut)
                            );
                        }
                        else
                        {
                            await Task.WhenAll(
                                MainLayoutStack.TranslateTo(0, 0, 50, Easing.CubicOut),
                                MenuLayer.Layout.TranslateTo(Units.ScreenWidth, AppSettings.MenuYPosition, 50, Easing.CubicOut)
                            );
                        }
                        break;

                }
                MenuLayer.Deactivate();
            });
            return true;
        }

        public void ForceSubHeaderUpdate()
        {
            SubHeader.Update();
        }

        public void SetSubHeaderTitle(string title, Models.Action action)
        {
            SubHeader.SetTitle(title, action);
        }

        public void SetSubHeaderTitleWithAction(string title, Action action)
        {
            SubHeader.SetTitle(title, action);
        }

        public void SetSubHeaderDescription(string description)
        {
            SubHeader.SetDescription(description);
        }

        public void ClearSubHeaderTitle()
        {
            SubHeader.ClearTitle();
        }

        public void SwitchSubHeaderToCloseMode()
        {
            SubHeader.SwitchToCloseMode();
        }

        public void SwitchSubHeaderToMainMode()
        {
            SubHeader.SwitchToMainMode();
        }

        public void ScaleUpBackground()
        {
            BackgroundLayer.BackgroundImage.Content.ScaleTo(1.25f, 250, Easing.Linear);
        }

        public void ScaleDownBackground()
        {
            BackgroundLayer.BackgroundImage.Content.ScaleTo(1.0f, 250, Easing.Linear);
        }

        public void ResetScaleBackground()
        {
            //BackgroundLayer.BackgroundImage.Content.ScaleTo(1.0f, 0, Easing.Linear);
        }

        private void UpdateContentSizeAndPosition()
        {
            // not currently used
        }

        public void HardwareBack()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await App.CloseMenu();

                switch (Helpers.Pages.GetCurrentPageId())
                {
                    case (int)AppSettings.PageNames.Landing:
                        break;

                    case (int)AppSettings.PageNames.Menu:
                        await App.CloseMenu();
                        break;

                    case (int)AppSettings.PageNames.Questionnaire:
                        await GoToPage((int)AppSettings.PageNames.Landing);
                        break;

                    case (int)AppSettings.PageNames.RecommendedRecipes:
                        await GoToPage((int)AppSettings.PageNames.Landing);
                        break;

                    case (int)AppSettings.PageNames.Legals:
                        await GoToPage((int)AppSettings.PageNames.Landing);
                        break;

                    case (int)AppSettings.PageNames.MealPlan:
                        await GoToPage((int)AppSettings.PageNames.Landing);
                        break;

                    case (int)AppSettings.PageNames.WasteLess:
                        await GoToPage((int)AppSettings.PageNames.Landing);
                        break;

                    case (int)AppSettings.PageNames.CookItCorner:
                        await GoToPage((int)AppSettings.PageNames.Landing);
                        break;

                    case (int)AppSettings.PageNames.HealthyLiving:
                        await GoToPage((int)AppSettings.PageNames.Landing);
                        break;

                    case (int)AppSettings.PageNames.Videos:
                        await GoToPage((int)AppSettings.PageNames.CookItCorner);
                        break;

                    case (int)AppSettings.PageNames.MPCalendar:
                        await GoToPage((int)AppSettings.PageNames.Landing);
                        break;

                    case (int)AppSettings.PageNames.SearchResults:
                        await GoToPage((int)AppSettings.PageNames.Landing);
                        break;
                    case (int)AppSettings.PageNames.SingleInfluencer:
                        await GoToPage((int)AppSettings.PageNames.HealthyLiving);
                        break;
                    case (int)AppSettings.PageNames.Calendar:
                        await GoToPage((int)AppSettings.PageNames.Landing);
                        break;
                    case (int)AppSettings.PageNames.Collections:
                        await GoToPage((int)AppSettings.PageNames.Landing);
                        break;
                    case (int)AppSettings.PageNames.RecipeEditor:
                        await GoToPage((int)AppSettings.PageNames.Landing);
                        break;
                    case (int)AppSettings.PageNames.InfluencerBrowse:
                        await GoToPage((int)AppSettings.PageNames.HealthyLiving);
                        break;
                    case (int)AppSettings.PageNames.InfluencerBio:
                        await GoToPage((int)AppSettings.PageNames.Landing);
                        break;
                    case (int)AppSettings.PageNames.ShoppingBasket:
                        await GoToPage((int)AppSettings.PageNames.Landing);
                        break;
                }
            });
        }

        public void ShowMenuButton()
        {
            MainHeader.ShowMenuButton();
        }

        public void HideMenuButton()
        {
            MainHeader.HideMenuButton();
        }

        public void ShowCurrentPageInfo()
        {
            string title = "";
            string content = "";
            int currentPageId = Helpers.Pages.GetCurrentPageId();
            switch (currentPageId)
            {
                case (int)AppSettings.PageNames.RecipeEditor:
                    title = "Recipe Editor";
                    content = "Build your recipes and share them with others by publishing. Build your recipes and share them with others by publishing. As you create recipes, the Recipe Summaries will appear here in a list. In the top left corner of each one is a letter and colour to give you a quick visual reference:\n\nP = Published\nC = Complete and ready to be published\nU = Unfinished and unpublishable.";
                    break;

                case (int)AppSettings.PageNames.RecommendedRecipes:
                    title = "Recommended Recipes";
                    content = "The list of recipes will be updated regularly based on any changes to your filters and other recipes that you’ve favourited. You can select a recipe(by tapping the Recipe Options in the lower right of the Recipe Summary) and choose to share them, add them to a Meal Plan(paid feature), add them to a Collection(album) or Quick Shop them.";
                    break;

                case (int)AppSettings.PageNames.MealPlan:
                case (int)AppSettings.PageNames.MPCalendar:
                    if (StaticData.isCreating)
                    {
                        title = "Meal Plan Editor";
                        content = "This may look daunting, but it has all the tools you need to make detailed changes to your plan, and options for publishing or keep it for yourself and your private viewing only.";
                    }
                    else
                    {
                        title = "Meal Plans";
                        content = "Meal Planner / Calendar This is a convenient way to plan out and shop for your meals over a week or more. You can fill it with content from an influencer or other users, or have CHAI autocomplete one for you. You can edit the meals in any plan(other than Influencers) or start from scratch but please try to stick to your transition or flexitarian plan (available to view in your Calendar for easy access) as much as possible!";

                    }
                    break;

                case (int)AppSettings.PageNames.Calendar:
                    title = "Calendar";
                    content = "Here you can view your transition / flexitarian / vegan subscription meal plans. They will automatically update for you each week. You can edit your meal plan if you do not like the allocated meals and we will prompt you if you go off track and include too many non-vegan recipes. We base all of our above plans on a health score of 5 and above to ensure that we make your changes as healthy as possible. For those that edit their plans we cannot guarantee that it will still be a health score of 5 and above.";
                    break;

                case (int)AppSettings.PageNames.WasteLess:
                    title = "Waste Less";
                    content = "Here, you’ll be able to discover new recipes to help you use up any leftover ingredients, and make the most of what you buy.You can enter multiple ingredients into the Waste Less input and find recipes from our vast database which provide the best match.";
                    break;

                case (int)AppSettings.PageNames.HealthyLiving:
                    title = "Healthy Living";
                    content = "This is where you can browse and select meal plans uploaded by influencers, browse all meal plans, or create your own content using CHAI’s recipe editor and make it visible for everyone to see here.";
                    break;

                case (int)AppSettings.PageNames.Landing:
                    title = "Home Page";
                    content = "Scroll and tap on an image to enter one of the two main app sections:\n\nWaste Less -find recipes to match the ingredients in your kitchen\nHealthy Living -choose from a selection of meal plans.";
                    break;

                case (int)AppSettings.PageNames.Collections:
                    title = "Collections";
                    content = "Favourites - organise your favourite recipes\nAlbums - organise and edit recipe albums";
                    break;
                case (int)AppSettings.PageNames.ShoppingBasket:
                    title = "Shopping Basket";
                    content = "See what's in your shopping basket and checkout when you're ready";
                    break;

                case (int)AppSettings.PageNames.Questionnaire:
                    title = "Your Preferences";
                    content = "Use these to adjust the recipe content that CHAI displays. All the recipes will be updated in immediate response to any changes made to the filters. They will apply to any list of recipes in the app e.g., Recommended Recipes, Favourites, Waste Less etc";
                    break;

                case (int)AppSettings.PageNames.InfluencerBio:
                    title = "Influencer Bio";
                    content = "If you are invited to be an Influencer, then you can write your bio here and upload an image to go with it.";
                    break;
                    

            }
            ShowInfoBubble(new Paragraph(title, content, null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
        }


        public void ShowInfoBubble(View view, int x, int y)
        {
            InfoBubblePanel info = new InfoBubblePanel("", "", false);
            //info.Content.HeightRequest = 200;
            info.SetBubbleContent(view);

            //info.SetPosition(x, y);

            info.SetPosition(Units.HalfScreenWidth, 0);// (int)Units.HalfScreenHeight - (int)(info.Content.Height));
            info.HidePointer();
           

            Device.BeginInvokeOnMainThread(async () =>
            {
                //await SetForegroundContent(info.Content);
                //await ShowForeground();
                //await HideModal();
                await SetForegroundOverlayContent(info.Content);
                await ShowForegroundOverlay();


            });
        }

        public void HideInfoBubble()
        {
            //AppContainer.HideInfoBubble();
            Device.BeginInvokeOnMainThread(async () =>
            {
                //await HideForeground();
                await HideForegroundOverlay();
            });
        }

        public void ShowMenuInfoBubble(View view, int x, int y)
        {
            InfoBubblePanel info = new InfoBubblePanel("", "", true);
            info.SetBubbleContent(view);
            info.SetPosition(x, y);

            Device.BeginInvokeOnMainThread(async () =>
            {
                //await MenuPage.(info.Content);
                //await ShowMenuForeground();
                MenuPage.AddOverlayContent(info.GetContent());

            });
        }

        public void HideMenuInfoBubble()
        {
            //AppContainer.HideInfoBubble();
            Device.BeginInvokeOnMainThread(async () =>
            {
                //await HideMenuForeground();
                MenuPage.HideOverlayContent();
            });
        }

        public void ShowBackgroundImage()
        {
            BackgroundLayer.Layout.BackgroundColor = Color.Transparent;
            BackgroundLayer.BackgroundImage.Content.IsVisible = true;
            BackgroundLayer.Activate();
        }

        public void HideBackGroundImage()
        {
            BackgroundLayer.Layout.BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY);
            BackgroundLayer.BackgroundImage.Content.IsVisible = false;
            BackgroundLayer.Activate();
        }

        public void SetBackgroundImage(string imageSource)
        {
            BackgroundLayer.AddBackgroundImage(imageSource);
        }

        public void SetBackgroundColor(Color color)
        {
            BackgroundLayer.Layout.BackgroundColor = color;
            //MainLayoutStack.BackgroundColor = Color.Transparent;
        }

        public async Task ShowLoginAndRegistration(bool isRegistered)
        {
            await LoginAndRegistrationPage.ShowLoginAndCreateAccountPanel(isRegistered);
        }

        public async Task ShowVerifyAccount()
        {
            await ModalContentLayer.Layout.FadeTo(0, 0, Easing.Linear);
            await ModalBackgroundLayer.Layout.FadeTo(0, 0, Easing.Linear);

            ModalContentLayer.Deactivate();
            ModalBackgroundLayer.Deactivate();
            ModalContentLayer.Layout.Children.Clear();
            ParentContainer.Layout.Children.Remove(ModalContentLayer.Layout);
            await LoginAndRegistrationPage.ShowVerifyAccountPanel();
        }

        public async Task HideVerifyAccount()
        {
            await LoginAndRegistrationPage.HideVerifyAccountPanel();
        }

        public async Task<bool> ShowRecipeOptionsModal(Recipe recipe)
        {
            return await ShowModal(new RecipeOptionsPanel(recipe).GetContent());
        }

        public async Task<bool> DeleteUnsavedIngredient(Ingredient ingredient)
        {
            return await RecipeEditor.DeleteUnsavedIngredient(ingredient);
        }

        public async Task<bool> ShowTemplateDayModal(bool isDeleting)
        {
            return await ShowModal(new TemplateDayModal(isDeleting).GetContent());
        }

        public async Task ShowRecommendedRecipeFilterPanel()
        {
            SubHeader.SetTitleActivated();
            PanelLayer.Layout.Children.Clear();
            PanelLayer.DefaultEventDisabled = true; // disable default toggle

            PanelLayer.Layout.Children.Add(RecommendedRecipeFilterPanel.Content);

            await PanelLayer.Layout.FadeTo(1, 50, null);
            await PanelLayer.Layout.TranslateTo(0, 0, 0, Easing.Linear);

            PanelLayer.Layout.Margin = 0;// new Thickness(Dimensions.GENERAL_COMPONENT_PADDING, Dimensions.HEADER_HEIGHT + Dimensions.SUBHEADER_HEIGHT, Dimensions.GENERAL_COMPONENT_PADDING, Units.ScreenWidth);
            //ModalBackgroundLayer.Activate();
            PanelLayer.Activate();
        }

        public async Task ShowCreateMealPlan()
        {
            SubHeader.SetTitleActivated();
            PanelLayer.Layout.Children.Clear();
            PanelLayer.DefaultEventDisabled = true; // disable default toggle

            //PanelLayer.Layout.Children.Add(CreateMealPlanPanel.Content);

            await PanelLayer.Layout.FadeTo(1, 50, null);
            await PanelLayer.Layout.TranslateTo(0, 0, 0, Easing.Linear);

            PanelLayer.Layout.Margin = 0;// new Thickness(Dimensions.GENERAL_COMPONENT_PADDING, Dimensions.HEADER_HEIGHT + Dimensions.SUBHEADER_HEIGHT, Dimensions.GENERAL_COMPONENT_PADDING, Units.ScreenWidth);
            //ModalBackgroundLayer.Activate();
            PanelLayer.Activate();
        }

        public async Task ShowRemoveRecipeEditorIngredientModal(Ingredient ingredient)
        {
            await RecipeEditor.RemoveIngredient(ingredient);
        }

        public async Task ShowRecipeEditor()
        {
            await RecipeEditor.EditCurrentRecipe();
        }


        public async Task<bool> ShowForgotPassword()
        {
            return await ShowModal(new ForgotPasswordModal().GetContent());
        }

        public async Task<bool> ShowStartDateMenu(Influencer.Datum influencer, string planLength, string planName)
        {
            return await ShowModal(new StartDateModal(influencer, planLength, planName).GetContent());
        }

        public async Task<bool> ShowCalendar(Influencer.Datum influencer, string planLength)
        {
            return await ShowModal(new CalendarModal(influencer, planLength).GetContent());
        }

        public async Task<bool> ShowPreviewMealPlan(string name, string planLength)
        {
            return await ShowModal(new MealPreviewModal(planLength, name, false).GetContent());
        }

        public async Task<bool> HidePreviewMealPlan()
        {
            await ModalContentLayer.Layout.FadeTo(0, 0, Easing.Linear);
            await ModalBackgroundLayer.Layout.FadeTo(0, 0, Easing.Linear);

            ModalContentLayer.Deactivate();
            ModalBackgroundLayer.Deactivate();
            ModalContentLayer.Layout.Children.Clear();
            ParentContainer.Layout.Children.Remove(ModalContentLayer.Layout);

            return true;
        }

        public async Task<bool> RateRecipe(Recipe recipe)
        {
            return await ShowModal(new RateModal(recipe).GetContent());
        }

        public async Task<bool> ShowCreateAlbumModal(Action updateAlbums, bool hasColour)
        {
            return await ShowModal(new CreateAlbumModal(updateAlbums, hasColour).GetContent());
        }

        public async Task<bool> ShowMealHolderRemove()
        {
            return await ShowModal(new MealHolderRemoveModal().GetContent());
        }

        public async Task<bool> ShowMealHolderRemove(Recipe recipe)
        {
            return await ShowModal(new MealHolderRemoveModal(recipe).GetContent());
        }

        public async Task<bool> ShowMealHolderRemove(IList<object> recipes)
        {
            return await ShowModal(new MealHolderRemoveModal(recipes).GetContent());
        }

        public async Task<bool> ShowRemoveAlbumModal(Album album)
        {
            return await ShowModal(new RemoveAlbumModal(album).GetContent());
        }

        public async Task<bool> ShowRemoveAlbumModal(Recipe recipe)
        {
            return await ShowModal(new RemoveAlbumModal(recipe).GetContent());
        }

        public async Task<bool> ShowRemoveAlbumModal(IList<object> recipes)
        {
            return await ShowModal(new RemoveAlbumModal(recipes).GetContent());
        }

        public async Task<bool> ShowRemoveAlbumModal()
        {
            return await ShowModal(new RemoveAlbumModal().GetContent());
        }

        public async Task<bool> ShowEditAlbumModal(Action updateAlbums)
        {
            return await ShowModal(new EditAlbumModal(updateAlbums).GetContent());
        }

        public async Task<bool> ShowFullRecipe(Recipe recipe, bool reloadParent, bool reloadMealEditor)
        {
            return await ShowModal(new ViewFullRecipeModal(recipe, reloadParent, reloadMealEditor).GetContent());
        }

        public async Task<bool> ShowFullRecipe(Recipe recipe, List<Recipe> recipeCollection)
        {
            return await ShowModal(new ViewFullRecipeModal(recipe, recipeCollection).GetContent());
        }

        public async Task<bool> ShowFullRecipe(Recipe recipe, Action updateHeart, string specialConstructor)
        {
            return await ShowModal(new ViewFullRecipeModal(recipe, updateHeart, specialConstructor).GetContent());
        }

        public async Task<bool> ShowRecipeSummary(Recipe recipe, string mealPeriod)
        {
            return await ShowModal(new RecipeSummary(recipe, mealPeriod).GetContent());
        }

        public async Task<bool> ShowRemoveFromBasketModal()
        {
            return await ShowModal(new RemoveFromBasketModal().GetContent());
        }

        public async Task<bool> ShowRemoveFromBasketModal(Recipe recipe)
        {
            return await ShowModal(new RemoveFromBasketModal(recipe).GetContent());
        }

        public async Task<bool> ShowCreateMealPlanPopup()
        {
            return await ShowModal(new CreateMealPlanModal().GetContent());
        }

        public async Task<bool> ShowSubscribeModal()
        {
            if (SubscribeModal == null)
            {
                SubscribeModal = new SubscribeModal();
            }
            return await ShowModal(SubscribeModal.GetContent());
        }

        public async void HideSubscribeModal()
        {
            await HideModal();
            await HideMenu();
        }

        public async Task<bool> ShowModal(View modalContent)
        {
            try
            {
                ModalContentLayer.Layout.Children.Clear();

                modalContent.Margin = new Thickness(Units.ScreenWidth2Percent, 0, Units.ScreenWidth2Percent, 0);
                //modalContent.HeightRequest = Units.ScreenHeight;

                modalContent.VerticalOptions = LayoutOptions.CenterAndExpand;

                ModalContentLayer.Layout.WidthRequest = Units.ScreenWidth;
                ModalContentLayer.Layout.HeightRequest = Units.ScreenHeight;
                ModalContentLayer.Layout.Margin = 0;
                ModalContentLayer.Layout.Padding = 0;
                ModalContentLayer.Layout.Children.Add(modalContent, 0, 0);

                // add content to modal here
                ModalContentLayer.Activate();
                ModalBackgroundLayer.Activate();

                await ModalBackgroundLayer.Layout.FadeTo(AppSettings.ModalOpacity, 25, Easing.Linear);
                await ModalContentLayer.Layout.FadeTo(1, 0, Easing.Linear);

                ParentContainer.Layout.Children.Add(ModalContentLayer.Layout, 0, 0);

            }
            catch (Exception e)
            {

            }
            return true;
        }


        public async Task<bool> HideCalendar()
        {
            await ModalContentLayer.Layout.FadeTo(0, 0, Easing.Linear);
            await ModalBackgroundLayer.Layout.FadeTo(0, 0, Easing.Linear);

            ModalContentLayer.Deactivate();
            ModalBackgroundLayer.Deactivate();
            ModalContentLayer.Layout.Children.Clear();
            ParentContainer.Layout.Children.Remove(ModalContentLayer.Layout);

            return true;
        }

        public async Task<bool> HideStartDateMenu()
        {
            await ModalContentLayer.Layout.FadeTo(0, 0, Easing.Linear);
            await ModalBackgroundLayer.Layout.FadeTo(0, 0, Easing.Linear);

            ModalContentLayer.Deactivate();
            ModalBackgroundLayer.Deactivate();
            ModalContentLayer.Layout.Children.Clear();
            ParentContainer.Layout.Children.Remove(ModalContentLayer.Layout);

            return true;
        }

        public async Task<bool> HideRecipeSummary()
        {
            await ModalContentLayer.Layout.FadeTo(0, 0, Easing.Linear);
            await ModalBackgroundLayer.Layout.FadeTo(0, 0, Easing.Linear);

            ModalContentLayer.Deactivate();
            ModalBackgroundLayer.Deactivate();
            ModalContentLayer.Layout.Children.Clear();
            ParentContainer.Layout.Children.Remove(ModalContentLayer.Layout);

            return true;
        }

        public async Task<bool> HideModalAsync()
        {
            await ModalContentLayer.Layout.FadeTo(0, 0, Easing.Linear);
            await ModalBackgroundLayer.Layout.FadeTo(0, 0, Easing.Linear);

            ModalContentLayer.Deactivate();
            ModalBackgroundLayer.Deactivate();
            ModalContentLayer.Layout.Children.Clear();
            ParentContainer.Layout.Children.Remove(ModalContentLayer.Layout);

            return true;
        }

        public async Task<bool> ShowEditMeal(int index, int mealID, int mealPlanID, Recipe recipe, string mealPeriod, bool isEditing, string date)
        {
            if (AppSession.InfoModeOn)
            {
                App.ShowInfoBubble(new Paragraph("Meal Summaries", "Your Meal Plan / Calendar is filled with Meal Summaries - they contain one or more recipes for that meal. Scroll through the sidebar list and select a new recipe to replace the existing one. Meal Summary to see enlarged details of the recipes within it and tap the close option to close them.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                return true;
            }

            AppSession.CurrentMealIndex = index;
            AppSession.CurrentMealId = mealID;
            AppSession.CurrentMealPlanId = mealPlanID;
            AppSession.CurrentMealPeriod = mealPeriod;
            AppSession.CurrentIsEditingMeal = isEditing;
            AppSession.CurrentMealDate = date;

            return await ShowModal(new EditMealModal(index, mealID, mealPlanID, recipe, mealPeriod, isEditing, date).GetContent());
            
        }

        public async Task<bool> ShowEditMeal(Recipe recipe)
        {
            if (AppSession.InfoModeOn)
            {
                App.ShowInfoBubble(new Paragraph("Meal Summaries", "Your Meal Plan / Calendar is filled with Meal Summaries - they contain one or more recipes for that meal. Scroll through the sidebar list and select a new recipe to replace the existing one. Meal Summary to see enlarged details of the recipes within it and tap the close option to close them.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                return true;
            }
            if (AppSession.CurrentMealId > -1)
            {
                return await ShowModal(new EditMealModal(AppSession.CurrentMealIndex, AppSession.CurrentMealId, AppSession.CurrentMealPlanId, recipe, AppSession.CurrentMealPeriod, AppSession.CurrentIsEditingMeal, AppSession.CurrentMealDate).GetContent());
            }
            return false;
        }

        public async Task<bool> ShowEditMeal(int index, int mealPeriod, string date, int mealPlanId, bool isEditing, bool isCalendar = false)
        {
            if (AppSession.InfoModeOn)
            {
                App.ShowInfoBubble(new Paragraph("Meal Summaries", "Your Meal Plan / Calendar is filled with Meal Summaries - they contain one or more recipes for that meal. Scroll through the sidebar list and select a new recipe to replace the existing one. Meal Summary to see enlarged details of the recipes within it and tap the close option to close them.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                return true;
            }
            return await ShowModal(new EditMealModal(index, mealPeriod, date, mealPlanId, isEditing, isCalendar).GetContent());
        }

        // MEAL TEMPLATE
        public async Task<bool> ShowEditMeal(int mealTemplateID, int dayTemplateID, string mealType, Recipe recipe, bool isEditing, bool isTemplate)
        {
            if (AppSession.InfoModeOn)
            {
                App.ShowInfoBubble(new Paragraph("Meal Summaries", "Your Meal Plan / Calendar is filled with Meal Summaries - they contain one or more recipes for that meal. Scroll through the sidebar list and select a new recipe to replace the existing one. Meal Summary to see enlarged details of the recipes within it and tap the close option to close them.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                return true;
            }

            return await ShowModal(new EditMealModal(mealTemplateID, dayTemplateID, mealType, recipe, isEditing, isTemplate).GetContent());
        }

        // Adding to Empty Tile Meal Template
        public async Task<bool> ShowEditMeal(int dayTemplateID, int mealType, bool isEditing, bool isTemplate)
        {
            if (AppSession.InfoModeOn)
            {
                App.ShowInfoBubble(new Paragraph("Meal Summaries", "Your Meal Plan / Calendar is filled with Meal Summaries - they contain one or more recipes for that meal. Scroll through the sidebar list and select a new recipe to replace the existing one. Meal Summary to see enlarged details of the recipes within it and tap the close option to close them.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                return true;
            }
            return await ShowModal(new EditMealModal(dayTemplateID, mealType, isEditing, isTemplate).GetContent());
        }

        public async Task<bool> ShowIngredientFilterModal()
        {
            return await ShowModal(new FilterIngredientsModal().GetContent());
        }

        // Template
        public async Task<bool> ShowEditTitle(int templateId, string name, string publishedAt)
        {
            return await ShowModal(new EditPlanTitleModal(templateId: templateId, name: name, publishedAt: publishedAt).GetContent());
        }


        public async Task<bool> HideAddRecipe()
        {
            await ModalContentLayer.Layout.FadeTo(0, 0, Easing.Linear);
            await ModalBackgroundLayer.Layout.FadeTo(0, 0, Easing.Linear);

            ModalContentLayer.Deactivate();
            ModalBackgroundLayer.Deactivate();
            ModalContentLayer.Layout.Children.Clear();
            ParentContainer.Layout.Children.Remove(ModalContentLayer.Layout);

            return true;
        }

        public async Task<bool> HideForgotPassword()
        {
            await ModalContentLayer.Layout.FadeTo(0, 0, Easing.Linear);
            await ModalBackgroundLayer.Layout.FadeTo(0, 0, Easing.Linear);

            ModalContentLayer.Deactivate();
            ModalBackgroundLayer.Deactivate();
            ModalContentLayer.Layout.Children.Clear();
            ParentContainer.Layout.Children.Remove(ModalContentLayer.Layout);
            return true;
        }

        public async Task<bool> ShowAccountConfirmationModal()
        {
            ModalContentLayer.Layout.Children.Clear();
            AccountConfirmationModal confirmationModal = new AccountConfirmationModal();
            GeneralInfoPanel infoPanel = new GeneralInfoPanel("Account Confirmation",
                Themes.PANEL_THEME_ORANGE_AND_WHITE, Dimensions.INFO_PANEL_WIDTH);
            infoPanel.AddContent(confirmationModal.Content);
            View modalContent = infoPanel.GetContent(); // get this from modal manager class, once implemented

            ModalContentLayer.Layout.WidthRequest = modalContent.Width;
            ModalContentLayer.Layout.HeightRequest = modalContent.Height;
            ModalContentLayer.Layout.Children.Add(modalContent, 0, 0);

            // add content to modal here
            ModalContentLayer.Activate();
            ModalBackgroundLayer.Activate();

            //await Task.Delay((int)AppSettings.TransitionVeryFast);
            await ModalBackgroundLayer.Layout.FadeTo(AppSettings.ModalOpacity, 25, Easing.Linear);
            await ModalContentLayer.Layout.FadeTo(1, 0, Easing.Linear);

            ParentContainer.Layout.Children.Add(ModalContentLayer.Layout, 0, 0);
            return true;
        }

        public async Task<bool> HideAccountConfirmationModal()
        {
            await ModalContentLayer.Layout.FadeTo(0, 0, Easing.Linear);
            await ModalBackgroundLayer.Layout.FadeTo(0, 0, Easing.Linear);

            ModalContentLayer.Deactivate();
            ModalBackgroundLayer.Deactivate();
            ModalContentLayer.Layout.Children.Clear();
            ParentContainer.Layout.Children.Remove(ModalContentLayer.Layout);
            return true;
        }

        public async Task HideCreateMealPlan()
        {
            SubHeader.SetTitleDeactivated();
            await PanelLayer.Layout.FadeTo(0, 50, null);
            PanelLayer.Deactivate();
            PanelLayer.Layout.Children.Clear();
        }

        public async Task<bool> ShowSelectOrCreateModal()
        {
            return await ShowModal(new CreateOrSelectModal().GetContent());
        }

        public async Task<bool> HideSelectOrCreateModal()
        {
            await ModalContentLayer.Layout.FadeTo(0, 0, Easing.Linear);
            await ModalBackgroundLayer.Layout.FadeTo(0, 0, Easing.Linear);

            ModalContentLayer.Deactivate();
            ModalBackgroundLayer.Deactivate();
            ModalContentLayer.Layout.Children.Clear();
            ParentContainer.Layout.Children.Remove(ModalContentLayer.Layout);
            return true;
        }

        public async Task ClearPanels()
        {
            SubHeader.SetTitleDeactivated();
            await PanelLayer.Layout.FadeTo(0, 50, null);
            PanelLayer.Deactivate();
            ModalBackgroundLayer.Deactivate();
            PanelLayer.Layout.Children.Clear();
        }


        private void Update()
        {
            // keep header and footer on top
            if (AppSettings.HasHeader)
            {
                MainLayoutStack.RaiseChild(Header.Content);
            }
            MainLayoutStack.RaiseChild(MainLayout.Layout);
            if (AppSettings.HasFooter)
            {
                MainLayoutStack.RaiseChild(Footer.Content);
            }

        }



        
        public void SetLoadingMessage(string message)
        {
            LoadingLabel.Text = message;
        }

        public async Task<bool> ShowLoading()
        {
            App.IsBusy = true;

            if (AppSettings.UseStatusBarLoading)
            {
                AppStatusBar.IsVisible = true;
                await Device.InvokeOnMainThreadAsync(() =>
                {
                    AppStatusBar.FadeTo(1, 100, Easing.SinIn);
                });
                
            }
            else
            {
                ForegroundOverlayLayer.Layout.BackgroundColor = Color.Black;
                ForegroundOverlayLayer.Activate();
                ParentContainer.Layout.RaiseChild(ForegroundOverlayLayer.Layout);
                await ForegroundOverlayLayer.Layout.FadeTo(0.75, 50, Easing.Linear);
                ForegroundOverlayLayer.Layout.IsVisible = true;
                ForegroundOverlayLayer.Layout.IsEnabled = true;
                Spinner.IsRunning = true;
            }
            return true;
        }

        public async Task<bool> HideLoading()
        {
            if (AppSettings.UseStatusBarLoading)
            {
                await Device.InvokeOnMainThreadAsync(() =>
                {
                    //For some reason anything less than this causes it to skip the fade animation...
                    AppStatusBar.FadeTo(0.01, 1000, easing: Easing.SinOut);
                });
                await Task.Delay(1000);
                AppStatusBar.IsVisible = false;
            }
            else
            {
                Spinner.IsRunning = false;
                await ForegroundOverlayLayer.Layout.FadeTo(0, 50, Easing.Linear);
                ForegroundOverlayLayer.Layout.IsVisible = false;
                ForegroundOverlayLayer.Layout.IsEnabled = false;
                ParentContainer.Layout.LowerChild(ForegroundOverlayLayer.Layout);
                ForegroundOverlayLayer.Deactivate();
                ForegroundOverlayLayer.Layout.BackgroundColor = Color.Transparent;
            }
            App.IsBusy = false;
            SetLoadingMessage("Loading...");
            return true;
        }

        public void SetNextPage(int pageId)
        {
            Helpers.Pages.SetNext(pageId);
        }

        public void SetLastPage(int pageId)
        {
            Helpers.Pages.SetLast(Helpers.Pages.GetCurrentPageId());
        }

        public async Task<bool> UpdatePage(int pageID)
        {
            await Helpers.Pages.GetPageById(pageID).Update();
            return true;
        }

        public async Task<bool> UpdatePage()
        {
            await Helpers.Pages.GetCurrent().Update();
            return true;
        }

        public async Task<bool> ReloadPage(int pageID)
        {
            Helpers.Pages.TransitionAction = (int)Helpers.Pages.TransitionActions.Next;
            await GoToPage(pageID);
            return true;
        }

        public async Task<bool> UpdateLayout(string layoutID)
        {
            await Task.Delay(0);
            foreach (View c1 in ParentContainer.Layout.Children)
            {
                Console.WriteLine("Layer: " + c1.GetType());

            }
            return true;
        }

        public async Task<bool> GoToNextPage()
        {
            //Helpers.Pages.SetLast((int)Helpers.Pages.CurrentPage);
            Helpers.Pages.TransitionAction = (int)Helpers.Pages.TransitionActions.Next;
            await GoToPage(Helpers.Pages.GetNextPageId());
            return true;
        }

        public async Task<bool> GoToLastPage()
        {
            Helpers.Pages.TransitionAction = (int)Helpers.Pages.TransitionActions.Last;
            await GoToPage(Helpers.Pages.GetLastPageId());
            return true;
        }

        public async Task<bool> GoToPage()
        {
            //Helpers.Pages.SetLast((int)Helpers.Pages.CurrentPage);
            await GoToPage(Helpers.Pages.GetNextPageId());
            return true;
        }

        public async Task<bool> GoToPage(int pageIndex)
        {
            // specific to the menu page
            //if (Helpers.Pages.GetCurrent().Id == (int)AppSettings.PageNames.TEMenuPage)
            //{
            //    GoToLastPage();
            //    return;
            //

            //Make sure the menu eye is updated when we start changing page
            MainHeader.ShowMenuClosed();

            if (await LoadPage(pageIndex))
            {
                Console.WriteLine("LOADING " + pageIndex);
            }
            else
            {
                Console.WriteLine("ALREADY LOADED " + pageIndex);
            }


            await HideMenu();
            await HidePanel();
            await HideForeground();
            await HideModal();

            await App.ShowLoading();

            await Helpers.Pages.GetCurrent().Reset();

            // store current page as last page
            AppSession.LastPageId = Helpers.Pages.GetCurrentPageId();

            // test.. always go back to home page -> Is this required to ensure the UI works?
            //AppSession.LastPageId = (int)AppSettings.PageNames.Landing;

            // set the target page
            Pages.Page CurrentPage = Helpers.Pages.GetCurrent();
            Pages.Page NewPage = Helpers.Pages.GetPageById(pageIndex);

            Helpers.Pages.SetLast(CurrentPage.Id);

            //await NewPage.DebugUpdate(5000);
            //Helpers.Pages.TransitionDirection = (int)Helpers.Pages.TransitionDirections.Vertical;

            ClearSubHeaderTitle();

            if (CurrentPage.Id != (int)AppSettings.PageNames.Menu)
            {
                Helpers.Pages.PageBeforeMenu = CurrentPage.Id;
            }

            Console.WriteLine("Current page: " + CurrentPage.Id);
            Console.WriteLine("Next page: " + NewPage.Id);
            Console.WriteLine("Going to page: " + pageIndex);

            if ((CurrentPage.Id == NewPage.Id) && NewPage.Id == (int)AppSettings.PageNames.Menu)
            {
                NewPage = Helpers.Pages.GetPageById((int)Helpers.Pages.PageBeforeMenu);
            }

            CurrentPage.TransitionOutType = (int)Helpers.Pages.TransitionTypes.FadeOut;
            NewPage.TransitionInType = (int)Helpers.Pages.TransitionTypes.FadeIn;

            Console.WriteLine("Current page: " + CurrentPage.Id);
            Console.WriteLine("Next page: " + NewPage.Id);
            Console.WriteLine("Going to page: " + pageIndex);

            if (CurrentPage.Id == NewPage.Id)
            {
                Console.WriteLine("ALREADY ON THIS PAGE - Next");
                ContentLayer.Layout.Children.Remove(CurrentPage.GetContent());
                await CurrentPage.Update();
                ContentLayer.Layout.Children.Add(CurrentPage.GetContent());
                await HideLoading();
                return false;
            }

            App.ResetScaleBackground();

            try
            {
                Helpers.Pages.GoTo(pageIndex);
                //await CurrentPage.Update();
                await NewPage.Update();
                await NewPage.PositionPage();

                // add the page to the layout, for presentation
                ContentLayer.Layout.Children.Add(NewPage.GetContent());
                //ContentLayer.Layout.LowerChild(CurrentPage.GetContent());
                //try
                //{
                //    await Task.WhenAll(
                //            CurrentPage.TransitionOut(),
                //            NewPage.TransitionIn()
                //        );
                //}
                //catch
                //{
                //    Console.WriteLine("Problem with page transitions");
                //}

                ContentLayer.Layout.Children.Remove(CurrentPage.GetContent());

                Helpers.Pages.SetCurrent(pageIndex);
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem going to next page");
                return false;
            }

            if (pageIndex == (int)AppSettings.PageNames.Landing)
            {
                MainHeader.SetLargeIcon();
            }
            MainHeader.Update();
            SubHeader.Update();

            await App.HideLoading();
            return true;
        }

        public async Task<bool> ShowAlert(string title, string message, string cancel, string confirm)
        {
             return await DisplayAlert(title, message, confirm, cancel);
        }

        public async Task ShowAlert(string title, string message, string cancel)
        {
            await DisplayAlert(title, message, cancel);
        }

        public async Task<string> DisplaySheet(string title, string cancel, string? destruction, params string[] buttons)
        {
            try
            {
                string action = await DisplayActionSheet(title, cancel, destruction, buttons);
                return action;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
                return "";
            }
        }

        public async Task<bool> UpdateDietTypes()
        {
            try
            {
                return await QuestionnairePage.UpdateDietTypes();
            }
            catch (Exception e)
            {

            }
            return false;
        }

        public async Task<bool> UpdateQuestionnaire()
        {
            try
            {
                await QuestionnairePage.Update();
                await InfluencerBio.Update();
            }
            catch (Exception e)
            {

            }
            return true;
        }



    }
}
