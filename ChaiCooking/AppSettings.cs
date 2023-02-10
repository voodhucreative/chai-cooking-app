using System;
using ChaiCooking.Helpers;
using Plugin.DeviceInfo;
using Xamarin.Forms;
using static ChaiCooking.Helpers.Custom.Accounts;

namespace ChaiCooking
{
    public class AppSettings
    {
        // App specific settings
        public static bool IsPublicBuild=false;
        public static string VersionID = "V0.35";
        public static bool Debug = false;
        public static bool UseFakeData = true;

        public static string TwitterUrl = "https://twitter.com/";
        public static string LinkedInUrl = "https://www.linkedin.com/";
        public static string FacebookUrl = "https://www.facebook.com/";

        public static string PrivacyPolicyUrl = "https://chai.cooking/wp-content/uploads/2021/09/app-privacy-policy.pdf";
        public static string TermAndConditionsUrl = "https://chai.cooking/wp-content/uploads/2021/09/app-terms-and-conditions.pdf";

        // enabled /  disabled areas
        public static bool CollaborationEnabled = false;
        public static bool MealPlannerEnabled = true;
        public static bool CalendarEnabled = true;
        public static bool TrackerEnabled = false;
        public static bool AlbumsEnabled = false;
        public static bool FavouritesEnabled = false;
        public static bool CookItCornerEnabled = false;
        public static bool MenuFilterSectionActive = false;
        public static bool CommunityOptionsEnabled = false;
        public static bool ShoppingBasketEnabled = true; // for first release
        public static bool AccountPreferencesEnabled = false;
        public static bool CutDownQuestionnaire = true;
        public static bool MyRecipesEnabled = false;
        public static bool RecipeOptionsEnabled = true;
        public static bool UsePlaceHolderAuthor = false;
        public static bool InfluencerBioEnabled = true;

        public static bool RecipeEditorEnabled = true;
        public static bool CollectionsEnabled = true; //WIP

        // cutdown version
        public static bool RequiresWhiskSignUp = false;

        public static bool FreeVersion = false;

        public static bool VimeoEnabled = true;
        public static string VIMEO_ACCESS_TOKEN = "742002e466c062727b5c56a6266346c4";
        public static string APP_ACCESS_TOKEN = "c8db3ba2269111ec96210242ac130002";

        public static bool AutoChaiSignupAndLogin = true;
        public static string DEFAULT_CHAI_IDPASSWORD = "00000000";


        // Page identifiers
        public enum PageNames
        {
            Landing,
            AboutUs,
            Menu,
            LogIn,
            Register,
            Questionnaire,
            RecommendedRecipes,
            Albums,
            AccountPreferences,
            Leaderboard,
            Legals,
            MealPlan,
            ShoppingBasket,
            Tracker,
            YourCharacter,
            WasteLess,
            CookItCorner,
            HealthyLiving,
            Videos,
            UpgradePlan,
            LoginAndRegistration,
            Favourites,
            SingleInfluencer,
            MPCalendar,
            InfluencerBrowse,
            SearchResults,
            Calendar,
            MyRecipes,
            RecipeEditor,
            Collections,
            InfluencerBio
        };



        // General app settings
        public static int TransitionSlow = 1500;
        public static int TransitionMedium = 1000;
        public static int TransitionFast = 500;
        public static int TransitionVeryFast = 250;

        public static bool HasStatusBar { get; set; }
        public static bool HasHeader { get; set; }
        public static bool HasSubHeader { get; set; }
        public static bool HasNavHeader { get; set; }
        public static bool HasFooter { get; set; }

        public int FullScreenHeight { get; set; }
        public int StatusBarHeight { get; set; }
        public int HeaderHeight { get; set; }
        public int FooterHeight { get; set; }

        // modal
        public float ModalOpacity { get; set; }

        // menu options
        public static int MenuWidth { get; set; }
        public int MenuHeight { get; set; }
        public int MenuCoverageHorizontal { get; set; }
        public int MenuCoverageVertical { get; set; }
        public static int MenuYPosition { get; set; }
        public bool MenuShownOverContent { get; set; }

        // decoration
        public static bool ShadowsOnTiles;

        public enum MenuPositions
        {
            Left,
            Right,
            Top,
            Bottom
        }

        public enum TransitionDirections
        {
            SlideOutTop,
            SlideOutBottom,
            SlideOutLeft,
            SlideOutRight,
            SlideInFromTop,
            SlideInFromBottom,
            SlideInFromLeft,
            SlideInFromRight
        }

        public enum TransitionTypes
        {
            SlideOutLeft,
            SlideOutRight,
            SlideOutTop,
            SlideOutBottom,
            FadeOut,
            ScaleOut
        }

        public enum RequestIds
        {
            RegisterUser,
            LoginUser,
            LogoutUser,
            GetSomething, // etc
        }

        public enum VideoCategories
        {
            None,
            Plants,
            WasteTips,
            DessertsAndBaking
        }

        public bool OverlayMenu { get; set; }
        public int MenuPosition { get; set; }

        public static string URL_FACEBOOK = "https://www.facebook.com/";
        public static string URL_TWITTER = "https://twitter.com/";

        public static bool GoDirectlyToRecipeFromMealPlan; // don't show intermediate preview
        public static bool HideMetrics; // hide us / uk option
        public static bool ShowAuthor; // show the actual author (it will just show "ChaiCooking" if false)
        public static bool UseStatusBarLoading;
        public static bool SimplifiedLoginAndRegister;
        public static bool SimplifiedRecipeEditor;
        public static bool EnableBilling;
        public static bool InfoModeEnabled;

        public static bool DebugSubsEnabled;
        public static AccountType DebugAccountType;
        public static bool FreeSubsForInfluencers;
        public static bool FreeSubsForAll;
        public static bool IncludeMeatEater = true;

        public AppSettings()
        {
            HeaderHeight = Units.HeaderHeight;
            FooterHeight = Units.FooterHeight;
            ModalOpacity = 0.75f;
            HasStatusBar = true;
            HasHeader = true;
            HasSubHeader = true;
            HasNavHeader = false;
            HasFooter = false;

            OverlayMenu = true;
            MenuWidth = (int)(Units.ScreenWidth * 0.75);
            MenuHeight = Units.ScreenHeight;
            MenuPosition = (int)MenuPositions.Left;
            MenuCoverageHorizontal = MenuWidth;// (int)(Units.ScreenWidth*0.6);
            MenuCoverageVertical = MenuHeight;// (int)(Units.ScreenHeight * 0.6);
            MenuShownOverContent = true;

            ShadowsOnTiles = true;

            // Android
            StatusBarHeight = 0;
            MenuYPosition = 68;

            GoDirectlyToRecipeFromMealPlan = true;
            HideMetrics = true;
            ShowAuthor = true;
            UseStatusBarLoading = true;

            SimplifiedRecipeEditor = true;

            SimplifiedLoginAndRegister = true;
            //SimplifiedLoginAndRegister = false; // for Danielle builds only

            EnableBilling = true;
            //EnableBilling = false; // for Dev testing only

            DebugSubsEnabled = false;

            InfoModeEnabled = true;

            DebugAccountType = Helpers.Custom.Accounts.AccountType.ChaiPremiumVegan;
          
            //Influencers should have free subscriptions?
            FreeSubsForInfluencers = true;
            FreeSubsForAll = false;

            IncludeMeatEater = true;

            // iOS
            if (Device.RuntimePlatform == Device.iOS)
            {
                if (CrossDeviceInfo.Current.VersionNumber.Major <= 10)
                {
                    StatusBarHeight = 24;
                }
                else
                {
                    StatusBarHeight = 44;
                }


                MenuYPosition = 68;
            }

            StatusBarHeight = Units.DeviceStatusBarHeight;
            Console.WriteLine("Status height: " + StatusBarHeight);
        }
    }
}