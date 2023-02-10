using System;
using System.Collections.Generic;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Helpers;
using ChaiCooking.Models;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Types;
using ChaiCooking.Services;
using ChaiCooking.Tools;
using Xamarin.Forms;

namespace ChaiCooking
{
    public static class AppDataContent
    {
        // The AppData holds and manages data used by the app
        public static List<Preference> DietTypes;

        public static List<Preference> Allergens;

        public static List<Preference> OtherPrefs;

        public static List<Preference> DeleteAccountPrefs;

        public static List<Ingredient> AvailableIngredients;

        public static List<Ingredient> AvoidedIngredients;

        public static List<MenuLink> CommunityMenuLinks;

        public static List<MenuLink> ProfileAndSettingsMenuLinks;

        //public static List<List<string>> WeeklyBudgetValues;

        //public static List<List<string>> HouseholdSizeValues;

        //public static List<List<string>> WeeklyShop;

        //public static List<List<string>> ConvenienceStores;

        //public static List<List<string>> OnlineShopping;

        //public static List<List<string>> Plants;

        //public static List<List<string>> Alcohol;

        public static Dictionary<int, string> WeeklyBudgetValues;

        public static Dictionary<int, string> HouseholdSizeValues;

        public static Dictionary<int, string> WeeklyShop;

        public static Dictionary<int, string> ConvenienceStores;

        public static Dictionary<int, string> OnlineShopping;

        public static Dictionary<int, string> Plants;

        public static Dictionary<int, string> Alcohol;

        public static List<CookItCornerSection> CookItCornerSections;

        public static List<List<Video>> CookItCornerVideos;

        public static List<AttributedText> PrivacyPolicyText;
        public static List<AttributedText> SubscriptionTypeText;

        public static List<SocialReward> SocialRewards;

        public static List<string> FavouriteSortChoices;

        public static List<VideoFeed> Videos;

        public static List<IngredientUnit> IngredientUnits;

        public static void Init()
        {
            PopulateAllergens();

            PopulateDietTypes();

            PopulateAvailableIngredients();

            PopulateAvoidedIngredients();

            PopulateCommunityLinks();

            PopulateProfileAndSettingsMenuLinks();

            PopulateWeeklyBudgetValues();

            PopulateHouseholdSizeValues();

            PopulateHouseholdWeeklyShopValues();

            PopulateHouseholdConvenienceStoresValues();

            PopulateHouseholdOnlineShoppingValues();

            PopulateHouseholdPlantsValues();

            PopulateHouseholdAlcoholValues();

            PopulateCookItCornerSections();

            PopulateCookItCornerVideos();

            PopulateSubscriptionTypeText();

            PopulateDeleteAccountPrefs();

            PopulateSocialRewards();

            PopulateFavouriteSortChoices();

            PopulateCookItCornerVideos();

            PopulateUnits();
        }

        public static void PopulateDietTypes()
        {
            // get the list from API...
            /*DietTypes = new List<Preference>();
            DietTypes.Add(new Preference("vegan", false));
            DietTypes.Add(new Preference("pescatarian", false));
            //DietTypes.Add(new Preference("Meat Eater", false));
            DietTypes.Add(new Preference("vegetarian", false));
            DietTypes.Add(new Preference("lacto-vegetarian", false));
            DietTypes.Add(new Preference("ovo-vegetarian", false));
            DietTypes.Add(new Preference("ovo-lacto-vegetarian", false));

            DietTypes.Add(new Preference("DIET_VEGAN", false));*/

            DietTypes = new List<Preference>();

            try
            {
                DietTypes = DataManager.GetDiets().Result;
            }
            catch (Exception e) { }
        }

        public static void PopulateAllergens()
        {
            Allergens = new List<Preference>();
            try
            {
                Allergens = DataManager.GetAllergens().Result;
            }
            catch (Exception e)
            {

            }
        }

        public static void PopulateUnits()
        {
            IngredientUnits = new List<IngredientUnit>();
            try
            {
                IngredientUnits = DataManager.GetUnits().Result;
            }
            catch (Exception e)
            {

            }
        }


        public static IngredientUnit GetUnitByName(string name)
        {
            IngredientUnit unit = new IngredientUnit
            {
                Name = "Bag",
                Abbreviation = "Bag",
                ID = 25
            };

            unit = IngredientUnits.Find(x => x.Name == name);

            foreach (IngredientUnit iu in IngredientUnits)
            {
                if (iu.Name == name)
                {
                    unit = iu;
                }
            }


            return unit;

        }

        public static void PopulateAvailableIngredients()
        {
            AvailableIngredients = new List<Ingredient>();
            //AvailableIngredients.Add(new Ingredient { Id = 0, Name = "Asparagus", ShortDescription = "", LongDescription = "", MainImage = "" });
            //AvailableIngredients.Add(new Ingredient { Id = 1, Name = "Cauliflower", ShortDescription = "", LongDescription = "", MainImage = "" });
            //AvailableIngredients.Add(new Ingredient { Id = 2, Name = "Mushrooms", ShortDescription = "", LongDescription = "", MainImage = "" });
            //AvailableIngredients.Add(new Ingredient { Id = 3, Name = "Stilton", ShortDescription = "", LongDescription = "", MainImage = "" });
        }

        public static void PopulateAvoidedIngredients()
        {
            AvoidedIngredients = new List<Ingredient>();
            //AvoidedIngredients.Add(new Ingredient { Id = 0, Name = "Eggs", ShortDescription = "", LongDescription = "", MainImage = "" });
            //AvoidedIngredients.Add(new Ingredient { Id = 1, Name = "Milk", ShortDescription = "", LongDescription = "", MainImage = "" });
            //AvoidedIngredients.Add(new Ingredient { Id = 2, Name = "Elk", ShortDescription = "", LongDescription = "", MainImage = "" });
            //AvoidedIngredients.Add(new Ingredient { Id = 3, Name = "Worms", ShortDescription = "", LongDescription = "", MainImage = "" });
        }

        public static void PopulateCommunityLinks()
        {
            CommunityMenuLinks = new List<MenuLink>();
            CommunityMenuLinks.Add(new MenuLink("Your Character", "", new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.YourCharacter), "charactericon.png", true));
            CommunityMenuLinks.Add(new MenuLink("Leaderboard", "", new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Leaderboard), "trophyicon.png", true));
        }

        public static void PopulateProfileAndSettingsMenuLinks()
        {
            ProfileAndSettingsMenuLinks = new List<MenuLink>();
            //ProfileAndSettingsMenuLinks.Add(new MenuLink("Shopping Basket", "", new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.ShoppingBasket), "carticon.png", true));
            if (!AppSettings.ShoppingBasketEnabled)
            {
                //ProfileAndSettingsMenuLinks.Add(new MenuLink("Shopping Basket", "", new Models.Action((int)Actions.ActionName.ShowComingSoon), "carticon.png", true));
            }
            else
            {
                if (Connection.IsConnected())
                {
                    ProfileAndSettingsMenuLinks.Add(new MenuLink("Shopping Basket", "", new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.ShoppingBasket), "carticon.png", true));
                }
                else
                {
                    App.ShowAlert("Please connect to the internet.");
                }
            }

            ProfileAndSettingsMenuLinks.Add(new MenuLink("Your Preferences", "", new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Questionnaire), "questioncircleicon.png", true));

            if (AppSettings.AccountPreferencesEnabled)
            {
                ProfileAndSettingsMenuLinks.Add(new MenuLink("Account Preferences", "", new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.AccountPreferences), "userprefsicon.png", true));
            }

            //ProfileAndSettingsMenuLinks.Add(new MenuLink("Privacy Policy", "", new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Legals), "privacyicon.png", true));
            //ProfileAndSettingsMenuLinks.Add(new MenuLink("Terms & Conditions", "", new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Legals), "termsicon.png", true));
        }

        public static void PopulateWeeklyBudgetValues()
        {
            WeeklyBudgetValues = new Dictionary<int, string>
            {
                { 0, "£10" },
                { 1, "£20" },
                { 2, "£30" },
                { 3, "£40" },
                { 4, "£50" },
                { 5, "£60" },
                { 6, "£70" },
                { 7, "£80" },
                { 8, "£90" },
                { 9, "£100" },
                { 10, "No limit" }
            };
        }

        public static void PopulateHouseholdSizeValues()
        {
            HouseholdSizeValues = new Dictionary<int, string>
            {
                { 0, "1" },
                { 1, "2" },
                { 2, "3" },
                { 3, "4" },
                { 4, "5" },
                { 5, "6" },
                { 6, "7" },
                { 7, "8" },
                { 8, "9" },
                { 9, "10" }
            };
        }

        public static void PopulateHouseholdWeeklyShopValues()
        {
            WeeklyShop = new Dictionary<int, string>
            {
                { 0, "Strongly disagree" },
                { 1, "Disagree" },
                { 2, "Neither agree nor disagree" },
                { 3, "Agree" },
                { 4, "Strongly agree" }
            };
        }

        public static void PopulateHouseholdConvenienceStoresValues()
        {
            ConvenienceStores = new Dictionary<int, string>
            {
                { 0, "Strongly disagree" },
                { 1, "Disagree" },
                { 2, "Neither agree nor disagree" },
                { 3, "Agree" },
                { 4, "Strongly agree" }
            };
        }

        public static void PopulateHouseholdOnlineShoppingValues()
        {
            OnlineShopping = new Dictionary<int, string>
            {
                { 0, "Strongly disagree" },
                { 1, "Disagree" },
                { 2, "Neither agree nor disagree" },
                { 3, "Agree" },
                { 4, "Strongly agree" }
            };
        }

        public static void PopulateHouseholdPlantsValues()
        {
            Plants = new Dictionary<int, string>
            {
                { 0, "Strongly disagree" },
                { 1, "Disagree" },
                { 2, "Neither agree nor disagree" },
                { 3, "Agree" },
                { 4, "Strongly agree" }
            };
        }

        public static void PopulateHouseholdAlcoholValues()
        {
            Alcohol = new Dictionary<int, string>
            {
                { 0, "Strongly disagree" },
                { 1, "Disagree" },
                { 2, "Neither agree nor disagree" },
                { 3, "Agree" },
                { 4, "Strongly agree" }
            };
        }

        public static void PopulateCookItCornerSections()
        {
            CookItCornerSections = new List<CookItCornerSection>();
            CookItCornerSections.Add(new CookItCornerSection { Id = 0, Name = "Plants", ComponentInfo = "Plant-based cooking videos.", ImageSource = "vgn1.jpg", LinkAction = null });
            CookItCornerSections.Add(new CookItCornerSection { Id = 1, Name = "Waste Tips", ComponentInfo = "Waste-reduction tips and videos.", ImageSource = "vgn2.jpg", LinkAction = null });
            CookItCornerSections.Add(new CookItCornerSection { Id = 2, Name = "Desserts and Baking", ComponentInfo = "Plant-based cooking videos.", ImageSource = "vgn3.jpg", LinkAction = null });
        }

        public static void PopulateCookItCornerVideos()
        {
            CookItCornerVideos = new List<List<Video>>();



            Videos = new List<VideoFeed>();
            VideoFeed PlantsFeed = new VideoFeed();
            VideoFeed WasteTips = new VideoFeed();
            VideoFeed DessertsAndBaking = new VideoFeed();

            try
            {
                PlantsFeed = DataManager.GetVideos((int)AppSettings.VideoCategories.Plants).Result;
                WasteTips = DataManager.GetVideos((int)AppSettings.VideoCategories.WasteTips).Result;
                DessertsAndBaking = DataManager.GetVideos((int)AppSettings.VideoCategories.DessertsAndBaking).Result;

                Videos.Add(PlantsFeed);
                Videos.Add(WasteTips);
                Videos.Add(DessertsAndBaking);
                Console.WriteLine("Videos populated");
            }
            catch (Exception e) { }
        }

        public static void PopulateSubscriptionTypeText()
        {
            SubscriptionTypeText = new List<AttributedText>();
            SubscriptionTypeText.Add(new AttributedText { TextContent = "Flexitarian Diet\n", TextColor = Color.White, TextFont = Fonts.GetBoldAppFont(), TextFontSize = Units.FontSizeL });
            SubscriptionTypeText.Add(new AttributedText { TextContent = "Cut down the amount of meat you eat\n\n", TextColor = Color.White, TextFont = Fonts.GetRegularAppFont(), TextFontSize = Units.FontSizeML });
            SubscriptionTypeText.Add(new AttributedText { TextContent = "Vegan Diet\n", TextColor = Color.White, TextFont = Fonts.GetBoldAppFont(), TextFontSize = Units.FontSizeL });
            SubscriptionTypeText.Add(new AttributedText { TextContent = "Enjoy a fully vegan diet and lifestyle\n\n", TextColor = Color.White, TextFont = Fonts.GetRegularAppFont(), TextFontSize = Units.FontSizeML });
            SubscriptionTypeText.Add(new AttributedText { TextContent = "Transition to a Vegan Diet\n", TextColor = Color.White, TextFont = Fonts.GetBoldAppFont(), TextFontSize = Units.FontSizeL });
            SubscriptionTypeText.Add(new AttributedText { TextContent = "Gradual transition to a vegan diet over 12 months\n\n", TextColor = Color.White, TextFont = Fonts.GetRegularAppFont(), TextFontSize = Units.FontSizeML });
        }

        public static void PopulateDeleteAccountPrefs()
        {
            DeleteAccountPrefs = new List<Preference>();
            DeleteAccountPrefs.Add(new Preference("Created a second account", false));
            DeleteAccountPrefs.Add(new Preference("Data concerns", false));
            DeleteAccountPrefs.Add(new Preference("Too busy / too distracting", false));
            DeleteAccountPrefs.Add(new Preference("Too many ads", false));
            DeleteAccountPrefs.Add(new Preference("Want to remove something", false));
            DeleteAccountPrefs.Add(new Preference("Privacy concerns", false));
            DeleteAccountPrefs.Add(new Preference("Something else", false));
        }

        public static void PopulateSocialRewards()
        {
            SocialRewards = new List<SocialReward>();
            SocialRewards.Add(new SocialReward { Name = "Chai Bag", ImageUrl = "social_reward_chai.png" });
            SocialRewards.Add(new SocialReward { Name = "Chicken", ImageUrl = "social_reward_chicken.png" });
            SocialRewards.Add(new SocialReward { Name = "Fish", ImageUrl = "social_reward_fish.png" });
            SocialRewards.Add(new SocialReward { Name = "Plant", ImageUrl = "social_reward_plant.png" });
            SocialRewards.Add(new SocialReward { Name = "Sheep", ImageUrl = "social_reward_sheep.png" });
            SocialRewards.Add(new SocialReward { Name = "Maggie", ImageUrl = "social_reward_puss.png" });
        }

        public static void PopulateFavouriteSortChoices()
        {
            FavouriteSortChoices = new List<string>();
            FavouriteSortChoices.Add("Name");
            FavouriteSortChoices.Add("Recent");
            FavouriteSortChoices.Add("Ingredient");
            FavouriteSortChoices.Add("Cuisine");
            FavouriteSortChoices.Add("Starters");
            FavouriteSortChoices.Add("Desserts");
            FavouriteSortChoices.Add("Cook time");
            FavouriteSortChoices.Add("Rating");
            FavouriteSortChoices.Add("Cal PP");
            FavouriteSortChoices.Add("Dish Type");
            FavouriteSortChoices.Add("My Rating");
            FavouriteSortChoices.Add("Mains");
            FavouriteSortChoices.Add("Snacks");
        }
    }
}