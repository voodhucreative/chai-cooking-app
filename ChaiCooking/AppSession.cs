using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using ChaiCooking.AppData;
using ChaiCooking.DebugData.Custom;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.InfluencerAPI;
using ChaiCooking.Models.Custom.MealPlanAPI;
using ChaiCooking.Models.Custom.ShoppingBasket;
using ChaiCooking.Services;
using ChaiCooking.Views.CollectionViews;
using ChaiCooking.Views.CollectionViews.AddEdit;
using ChaiCooking.Views.CollectionViews.Calendar;
using ChaiCooking.Views.CollectionViews.Collections;
using ChaiCooking.Views.CollectionViews.CreateOrSelect;
using ChaiCooking.Views.CollectionViews.Favourites;
using ChaiCooking.Views.CollectionViews.InfluencerMealPreview;
using ChaiCooking.Views.CollectionViews.IngredientFilter;
using ChaiCooking.Views.CollectionViews.MealPlanHolder;
using ChaiCooking.Views.CollectionViews.MealPlanner;
using ChaiCooking.Views.CollectionViews.RecipeEditor;
using ChaiCooking.Views.CollectionViews.ShoppingBasket;
using ChaiCooking.Views.CollectionViews.SingleInfluencer;
using Xamarin.Forms;
using static ChaiCooking.Helpers.Custom.Accounts;

namespace ChaiCooking
{
    public static class AppSession
    {
        // The AppSession holds temporary data used by the app
        public static User CurrentUser;
        public static List<Item> TestItems;

        public static bool InfoModeOn;
        public static bool SingleRecipeMode;
        public static int CurrentPage, CurrentPageRec, CurrentPageWaste, CurrentPageSearch;
        public static int TotalPages;

        public static int LastPageId { get; set; }

        public static Influencer.Datum SelectedInfluencer;
        public static Recipe SelectedRecipe;
        public static Recipe CurrentRecipe;
        public static int TotalRecipes;

        public static int TotalInfluencerPages;

        public static DateTime StartTime;
        public static DateTime EndTime;

        public static List<Recipe> SelectedFavourites;

        public static string CurrentWhiskToken;
        public static bool AuthorizeTimerRunning;

        public static List<string> SearchKeywords;
        public static int day = 2;
        public static List<Recipe> SearchedRecipes;
        public static List<Recipe> RecommendedRecipes;
        public static List<Recipe> WasteLessRecipes;
        public static List<Recipe> UserRecipes;
        public static List<Recipe> UserCollectionRecipes;
        public static List<Recipe> EditMealRecipes;
        public static List<Album> UserCollections;
        public static List<Ingredient> ingredientsFilterModalList;
        public static MealPlanModel mealTemplate;
        public static InfluencerMealPlans singleInfluencerMealPlans;
        public static UserMealTemplate mealPlannerCalendar, calendar;
        public static ObservableCollection<InfluencerMealPlanViewSection> influencerMealPlanCollection;
        public static ObservableCollection<CollectionsCollectionViewSection> collectionsCollection;
        public static ObservableCollection<FavouritesCollectionViewSection> favouritesCollection;
        public static ObservableCollection<MealsCollectionViewSection> editMealRecipesCollection;
        public static ObservableCollection<RecipesCollectionViewSection> recommendedRecipesCollection;
        public static ObservableCollection<RecipesCollectionViewSection> searchResultsCollection;
        public static ObservableCollection<MealPlanHolderCollectionViewSection> mealPlanHolderCollection;
        public static ObservableCollection<MealPlannerCollectionViewSection> mealPlannerCollection;
        public static ObservableCollection<CalendarCollectionViewSection> calendarCollection;
        public static ObservableCollection<RecipesCollectionViewSection> wasteLessCollection;
        public static ObservableCollection<IngredientsCollectionViewSection> ingredientsCollection;
        public static ObservableCollection<IngredientsCollectionViewSection> ingredientsFilterModalCollection;
        public static ObservableCollection<ShoppingBasketViewSection> shoppingBasketCollection;
        public static ObservableCollection<PreviewCollectionViewSection> influencerMealPreviewCollection;
        public static ObservableCollection<CreateOrSelectViewSection> createOrSelectCollection;
        public static ObservableCollection<RecipeEditorViewSection> recipeEditorCollection;
        public static ShoppingList shoppingList;
        public static List<Recipe> recipeEditorRecipes;
        public static Action<string, string> SetNavHeader;
        public static Action<string, bool, int, bool> SetMealPlanner;
        public static System.Action SetViewText;
        public static System.Action SetClearText;
        public static System.Action basketClearVisible;
        public static System.Action updateBasketShortcut;
        public static System.Action wasteLessUpdate { get; set; }
        public static CollectionView collectionsCollectionView, favouritesCollectionView,
            mealPlanHolderCollectionView, mealPlannerCollectionView, calendarCollectionView, wasteLessCollectionView,
            ingredientsCollectionView, ingredientsFilterModalCollectionView, shoppingBasketCollectionView,
            influencerMealPlanCollectionView, influencerMealPreviewCollectionView, createOrSelectCollectionView,
            recipeEditorCollectionView;
        public static CancellationTokenSource calendarTokenSource, mealPlanTokenSource;
        public static Album collectionsSelectedItem;
        public static string AfterCursor;
        public static string BeforeCursor;
        public static bool GetNextPage;
        public static bool GetLastPage;
        public static bool IsDraggable;
        public static bool mealPlanCheck;
        public static int CurrentVideoFeed;
        public static int CurrentVideo;

        public static bool UpdateSearch;
        public static bool BrowsePlans;

        public static string EmailFromWhiskAuth;
        public static string PasswordFromWhiskAuth;

        public static int SubscriptionTargetPage;

        public static AccountType SelectedAccountType;

        public static bool settingUpAccount;
        public static bool notifiedCalendarChange;

        public static int CurrentMealIndex;
        public static int CurrentMealId;
        public static int CurrentMealPlanId;
        public static string CurrentMealPeriod;
        public static bool CurrentIsEditingMeal;
        public static string CurrentMealDate;

        public static void Init()
        {
            StartTime = DateTime.Now;

            TestItems = new List<Item>();
            InfoModeOn = false;
            SingleRecipeMode = false;
            CurrentPage = 1;
            CurrentPageRec = 1;
            CurrentPageWaste = 1;
            CurrentPageSearch = 1;
            TotalPages = 1;

            CurrentUser = null;

            SelectedInfluencer = null;
            SelectedRecipe = null;
            CurrentRecipe = null;
            TotalRecipes = 0;

            CurrentWhiskToken = null;
            EmailFromWhiskAuth = null;
            PasswordFromWhiskAuth = null;
            AuthorizeTimerRunning = false;

            LastPageId = (int)AppSettings.PageNames.Landing;

            SelectedFavourites = new List<Recipe>();

            SearchKeywords = new List<string>();

            SearchedRecipes = null;
            WasteLessRecipes = null;
            RecommendedRecipes = null;
            UserRecipes = null;

            AfterCursor = null;
            BeforeCursor = null;

            GetNextPage = false;
            GetLastPage = false;

            SelectedRecipe = DataManager.GetSingleRecipe("101dd2ba5098a29bd1e122c6a6b7c978449177d4c76");
            CurrentVideoFeed = 1;
            CurrentVideo = 0;

            SubscriptionTargetPage = (int)AppSettings.PageNames.MealPlan;
            SelectedAccountType = AccountType.ChaiFree;
            UpdateSearch = true;
            BrowsePlans = false;

            CurrentMealIndex = -1;
            CurrentMealId = -1;
            CurrentMealPlanId = -1;
            CurrentMealPeriod = "";
            CurrentIsEditingMeal = false;
            CurrentMealDate = "";

            notifiedCalendarChange = false;
        }
    }
}
