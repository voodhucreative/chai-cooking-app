using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ChaiCooking.AppData;
using ChaiCooking.Models;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.Feed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChaiCooking.DebugData.Custom
{
    public static class FakeData
    {
        public static Address TestAddress;

        public static User TestUser;

        public static Location TestLocation;

        public static List<Recipe> FavouriteRecipes;

        public static List<Recipe> MyPublishedRecipes;

        public static List<Recipe> MyUnfinishedRecipes;

        public static List<Recipe> MyPendingRecipes;

        public static Recipe SingleRecipe;

        public static RecipeFeed UserRecipeFeed;

        public static RecipeFeed WasteLessFeed;

        public static RecipeFeed FavouritesFeed;
        //public static RecipeFeed UserRecipeFeed;

        public static List<Album> UserAlbums;

        public static List<Influencer> Influencers;

        public static List<List<Influencer>> Leaderboard;

        public static MealPlanModel previewMealPlan;

        public static List<string> FolderColors;

        public static void Init()
        {
            // from local json
            PopulateSingleRecipe();

            PopulateUser();

            PopulateFavouriteRecipes();

            PopulateMyPublishedRecipes();

            PopulateMyUnfinishedRecipes();

            PopulateMyPendingRecipes();

            PopulateUserAlbums();

            PopulateInfluencers();

            PopulateLeaderboard();

            PopulateFeed();

            PopulateWasteLessFeed();

            PopulateFavouritesFeed();

            PopulateFolderColors();

            PopulateMealPlan();
        }

        public static void PopulateFeed()
        {
            string testJson = "";

            string jsonFileName = "recipefeed.json";

            System.Reflection.Assembly Assembly = IntrospectionExtensions.GetTypeInfo(typeof(FakeData)).Assembly;

            Stream stream = Assembly.GetManifestResourceStream($"ChaiCooking.{jsonFileName}");

            using (var reader = new System.IO.StreamReader(stream))
            {
                testJson = reader.ReadToEnd();
            }

            JObject parsedJson = JObject.Parse(testJson);

            UserRecipeFeed = JsonConvert.DeserializeObject<RecipeFeed>(testJson);

            Console.WriteLine("Feed data read");
        }

        public static void PopulateWasteLessFeed()
        {
            string testJson = "";

            string jsonFileName = "wastelessfeed.json";

            System.Reflection.Assembly Assembly = IntrospectionExtensions.GetTypeInfo(typeof(FakeData)).Assembly;

            Stream stream = Assembly.GetManifestResourceStream($"ChaiCooking.{jsonFileName}");

            using (var reader = new System.IO.StreamReader(stream))
            {
                testJson = reader.ReadToEnd();
            }

            JObject parsedJson = JObject.Parse(testJson);

            WasteLessFeed = JsonConvert.DeserializeObject<RecipeFeed>(testJson);

            Console.WriteLine("Feed data read");
        }

        public static void PopulateMealPlan()
        {
            previewMealPlan = JsonConvert.DeserializeObject<MealPlanModel>(ReadJsonFile("mealplantemplate.json"));

            Console.WriteLine("Feed data read");
        }

        public static void PopulateFavouritesFeed()
        {
            FavouritesFeed = JsonConvert.DeserializeObject<RecipeFeed>(ReadJsonFile("favouritesfeed.json"));

            Console.WriteLine("Feed data read");
        }

        public static void PopulateSingleRecipe()
        {
       /*
            SingleRecipe = JsonConvert.DeserializeObject<Recipe>(ReadJsonFile("recipe1.json"));

            // fill any extra Chia specific info
            SingleRecipe.MainImageSource = "cheese.jpg";
            SingleRecipe.CookingTime = "30 mins";
            SingleRecipe.Creator = new Models.User();
            SingleRecipe.FavouriteRating = 3;
            SingleRecipe.IsFavourite = false;
            SingleRecipe.IsSelected = false;
            SingleRecipe.PrepTime = "10 mins";
            SingleRecipe.StarRating = 4;
            SingleRecipe.Type = "Lunch";*/
        }


        public static void PopulateUser()
        {
            TestUser = new User
            {
                Id = "00001",
                Username = "chaiuser",
                AuthToken = null, // random
                FirstName = "Chai",
                MiddleName = "",
                LastName = "User",
                FullName = "Chai User", // do we need this? probably not
                Gender = "Male",
                EmailAddress = "chai@testuser.co.uk",
                Password = AppSettings.DEFAULT_CHAI_IDPASSWORD,
                DateOfBirth = new DateTime(1975, 3, 14),
                IsRegistered = false,
                MobileNumber = "+4401234567890",
                AvatarImageUrl = "drnow.jpg",

                Address = new Address
                {
                    Id = "",
                    AddressLine1 = "8 Market Street",
                    AddressLine2 = "Whilmsford",
                    AddressLine3 = "",
                    AddressLine4 = "",
                    County = "Cheshire",
                    Country = "UK",
                    AreaCode = "WH1 0RD",

                    Location = new Location
                    {
                        Id = "",
                        Longitude = "-1.998852",
                        Latitude = "53.251114"
                    }
                },

                Preferences = new UserPreferences
                {
                    //AccountType = Helpers.Custom.Accounts.AccountType.ChaiFree,


                    Allergens = new System.Collections.Generic.List<Models.Custom.Preference>
                    {

                    },

                    Avoids = new System.Collections.Generic.List<Models.Custom.Preference>
                    {

                    },

                    DietTypes = new System.Collections.Generic.List<Models.Custom.Preference>
                    {

                    },

                    OtherPrefs = new System.Collections.Generic.List<Models.Custom.Preference>
                    {

                    }
                }
            };

            TestUser.Password = Helpers.Custom.Accounts.GenerateChaiIdPassword(TestUser);
            TestUser.Update();
        }

        public static void PopulateFavouriteRecipes()
        {
            FavouriteRecipes = new List<Recipe>();

            int recipeSlotsForFavourites = 10;

            for (int i = 0; i < recipeSlotsForFavourites; i++)
            {
                //Recipe r = (Recipe)SingleRecipe.Clone();

                //r.Name = "Fave: " + (i + 1);

                //FavouriteRecipes.Add(r);
            }
        }

        public static void PopulateMyPublishedRecipes()
        {
            MyPublishedRecipes = new List<Recipe>();     
        }

        public static void PopulateMyUnfinishedRecipes()
        {
            MyUnfinishedRecipes = new List<Recipe>();
        }

        public static void PopulateMyPendingRecipes()
        {
            MyPublishedRecipes = new List<Recipe>();


        }

        public static void PopulateUserAlbums()
        {
            UserAlbums = new List<Album>();

            Album UserAlbum1 = new Album { Id = "0", Name = "Great Soups", FolderColor = "ff00ff" };
            Album UserAlbum2 = new Album { Id = "1", Name = "Quick Pasta Dishes", FolderColor = "0fee44" };
            Album UserAlbum3 = new Album { Id = "2", Name = "Best Breads" };
            Album UserAlbum4 = new Album { Id = "3", Name = "Fruity Desserts" };
            Album UserAlbum5 = new Album { Id = "4", Name = "Gourmet Salads" };
            Album UserAlbum6 = new Album { Id = "5", Name = "Hot and Spicy Mexican" };
            Album UserAlbum7 = new Album { Id = "6", Name = "Dinner Party Ideas" };
            Album UserAlbum8 = new Album { Id = "7", Name = "Italian" };
            Album UserAlbum9 = new Album { Id = "8", Name = "Starters" };
            Album UserAlbum10 = new Album { Id = "9", Name = "Show Stoppers" };
            Album UserAlbum11 = new Album { Id = "10", Name = "Favourites" };
            Album UserAlbum12 = new Album { Id = "11", Name = "Recipes in January" };
            Album UserAlbum13 = new Album { Id = "12", Name = "Gorgeous Cakes" };
            Album UserAlbum14 = new Album { Id = "13", Name = "Mains" };
            Album UserAlbum15 = new Album { Id = "14", Name = "Desserts" };
            Album UserAlbum16 = new Album { Id = "15", Name = "Snacks" };
            Album UserAlbum17 = new Album { Id = "16", Name = "Indian and Thai" };
            Album UserAlbum18 = new Album { Id = "17", Name = "Veg Dishess" };
            Album UserAlbum19 = new Album { Id = "18", Name = "Take Time But Worth It" };
            Album UserAlbum20 = new Album { Id = "19", Name = "Stuff that Mat Likes" };


            UserAlbum1.AddRecipe(FakeData.SingleRecipe);
            UserAlbum1.AddRecipe(FakeData.SingleRecipe);




            UserAlbum1.AddRecipe(FakeData.SingleRecipe);

            UserAlbum2.AddRecipe(FakeData.SingleRecipe);
            UserAlbum2.AddRecipe(FakeData.SingleRecipe);
            UserAlbum2.AddRecipe(FakeData.SingleRecipe);
            UserAlbum2.AddRecipe(FakeData.SingleRecipe);

            UserAlbum3.AddRecipe(FakeData.SingleRecipe);
            UserAlbum3.AddRecipe(FakeData.SingleRecipe);
            UserAlbum3.AddRecipe(FakeData.SingleRecipe);
            UserAlbum3.AddRecipe(FakeData.SingleRecipe);
            UserAlbum3.AddRecipe(FakeData.SingleRecipe);

            UserAlbums.Add(UserAlbum1);
            UserAlbums.Add(UserAlbum2);
            UserAlbums.Add(UserAlbum3);
            UserAlbums.Add(UserAlbum4);
            UserAlbums.Add(UserAlbum5);
            UserAlbums.Add(UserAlbum6);
            UserAlbums.Add(UserAlbum7);
            UserAlbums.Add(UserAlbum8);
            UserAlbums.Add(UserAlbum9);
            UserAlbums.Add(UserAlbum10);
            UserAlbums.Add(UserAlbum11);
            UserAlbums.Add(UserAlbum12);
            UserAlbums.Add(UserAlbum13);
            UserAlbums.Add(UserAlbum14);
            UserAlbums.Add(UserAlbum15);
            UserAlbums.Add(UserAlbum16);
            UserAlbums.Add(UserAlbum17);
            UserAlbums.Add(UserAlbum18);
            UserAlbums.Add(UserAlbum19);
            UserAlbums.Add(UserAlbum20);



        }

        public static void PopulateInfluencers()
        {
            Influencers = new List<Influencer>();
            Influencers.Add(new Influencer {
                //Username = "Dr Now",
                //BioText = "Dr Now is a leading bariatric surgeon, helping morbidly obese people to lose weight. His famous 'Twelve Hundred Calorie, Low Carb, High Protein Diet' has helps hundreds of people lose literally thousands of pounds of weight. Add this plan to join in.. but only if you already weight 600lb or more. No need otherwise.",
                //AvatarImageUrl = "drnow.jpg"
                Username = "Yumna Jawad",
                //BioText = "With more than 2 million followers on Instagram and an engaging blog to match, Yumna Jawad shares healthful comfort food recipes with her adoring fans. Jawad got her start on the photo-sharing platform and later transitioned into blogging. This allowed her to provide more in-depth information about her recipes and share details about the Middle Eastern ingredients she uses that reflect her heritage.",
                AvatarImageUrl = "https://static.parade.com/wp-content/uploads/2020/06/Yumna.jpg"
            });

            Influencers.Add(new Influencer {
                Username = "Jamie Oliver",
                //BioText = "Although he got his start working in his family’s restaurant kitchen, Jamie Oliver began his transition into celebrity chef with a successful BBC show in 1999. Twenty years later, he’s used his platform to expose the darker side of food-supply chains — with sometimes-controversial results. Still, fans love this affable chef’s enthusiasm for healthy ingredients and recipes. He’s active on Twitter and Instagram, with more than 6.5 and 7 million followers on each platform, respectively.",
                AvatarImageUrl = "https://cdn.jamieoliver.com/library/images/Jamie-Social.jpg",
                PublicMealPlans = JsonConvert.DeserializeObject<MealPlanModel>(ReadJsonFile("jamieolivermealplan.json"))
            });

            Influencers.Add(new Influencer
            {
                //Username = "James Doyley",
                //BioText = "James specialises in cooking from up a tree. His 'Dinner In The Branches' meal plan has become legendary amongst tree frogs, parrots and spiders. Why not give it a try yourself!",
                //AvatarImageUrl = "james2.jpg"
                Username = "Rosanna Pansino",
                //BioText = "Attracting loyal followers on both Instagram and YouTube — where she has over 10 million subscribers — Rosanna Pansino creates baking-tutorial videos with decidedly nerdy twists. Pineapple-flavored Pikachu cake, anyone? How about some Infinity Stone cupcakes? Pansino has released two cookbooks and has also won several awards for her content. She also frequently creates gluten-free and vegan recipes to appeal to a wider audience, making her one of the top foodie influencers for 2019.",
                AvatarImageUrl = "https://influencermatchmaker.co.uk/sites/default/files/2020-01/rosanna.JPG"
            });

            Influencers.Add(new Influencer
            {
                //Username = "Rusty Lee",
                //BioText = "Rusty has been creating healthy stuff for centuries now and really knows her stuff! Why not spend a week in Rustyland?",
                //AvatarImageUrl = "rusty.jpg"
                Username = "Ignatius Gorby",
                //BioText = "If shots of artfully plated foods set your heart aflutter, Ignatius Gorby is one to follow. And yes, “bad plating” is actually the opposite of what Gorby does. This Indonesian chef showcases his stunning presentation skills on his Instagram account, @badplating, where he has just under 14,000 followers. Although that number puts him close to micro-influencer territory, Gorby successfully reaches a niche that values his strikingly vivid and beautifully balanced meals.",
                AvatarImageUrl = "https://data.thefeedfeed.com/static/profiles/15295136735b2a86c942c93.jpg"

            });


        }

        public static void PopulateLeaderboard()
        {
            Leaderboard = new List<List<Influencer>>();

            List<Influencer> MostPoints = new List<Influencer>();
            List<Influencer> LeastWaste = new List<Influencer>();
            List<Influencer> TopCreators = new List<Influencer>();

            MostPoints.Add(new Influencer { Username = "Most Points 1", AvatarImageUrl = "social_reward_chai.png" });
            MostPoints.Add(new Influencer { Username = "Most Points 2", AvatarImageUrl = "social_reward_chai.png" });
            MostPoints.Add(new Influencer { Username = "Most Points 3", AvatarImageUrl = "social_reward_chai.png" });
            MostPoints.Add(new Influencer { Username = "Most Points 4", AvatarImageUrl = "social_reward_chai.png" });
            MostPoints.Add(new Influencer { Username = "Most Points 5", AvatarImageUrl = "social_reward_chai.png" });
            MostPoints.Add(new Influencer { Username = "Most Points 6", AvatarImageUrl = "social_reward_chai.png" });
            MostPoints.Add(new Influencer { Username = "Most Points 7", AvatarImageUrl = "social_reward_chai.png" });
            MostPoints.Add(new Influencer { Username = "Most Points 8", AvatarImageUrl = "social_reward_chai.png" });
            MostPoints.Add(new Influencer { Username = "Most Points 9", AvatarImageUrl = "social_reward_chai.png" });
            MostPoints.Add(new Influencer { Username = "Most Points 10", AvatarImageUrl = "social_reward_chai.png" });

            LeastWaste.Add(new Influencer { Username = "Least Waste 1", AvatarImageUrl = "social_reward_chai.png" });
            LeastWaste.Add(new Influencer { Username = "Least Waste 2", AvatarImageUrl = "social_reward_chai.png" });
            LeastWaste.Add(new Influencer { Username = "Least Waste 3", AvatarImageUrl = "social_reward_chai.png" });
            LeastWaste.Add(new Influencer { Username = "Least Waste 4", AvatarImageUrl = "social_reward_chai.png" });
            LeastWaste.Add(new Influencer { Username = "Least Waste 5", AvatarImageUrl = "social_reward_chai.png" });
            LeastWaste.Add(new Influencer { Username = "Least Waste 6", AvatarImageUrl = "social_reward_chai.png" });
            LeastWaste.Add(new Influencer { Username = "Least Waste 7", AvatarImageUrl = "social_reward_chai.png" });
            LeastWaste.Add(new Influencer { Username = "Least Waste 8", AvatarImageUrl = "social_reward_chai.png" });
            LeastWaste.Add(new Influencer { Username = "Least Waste 9", AvatarImageUrl = "social_reward_chai.png" });
            LeastWaste.Add(new Influencer { Username = "Least Waste 10", AvatarImageUrl = "social_reward_chai.png" });

            TopCreators.Add(new Influencer { Username = "Top Creator 1", AvatarImageUrl = "social_reward_chai.png" });
            TopCreators.Add(new Influencer { Username = "Top Creator 2", AvatarImageUrl = "social_reward_chai.png" });
            TopCreators.Add(new Influencer { Username = "Top Creator 3", AvatarImageUrl = "social_reward_chai.png" });
            TopCreators.Add(new Influencer { Username = "Top Creator 4", AvatarImageUrl = "social_reward_chai.png" });
            TopCreators.Add(new Influencer { Username = "Top Creator 5", AvatarImageUrl = "social_reward_chai.png" });
            TopCreators.Add(new Influencer { Username = "Top Creator 6", AvatarImageUrl = "social_reward_chai.png" });
            TopCreators.Add(new Influencer { Username = "Top Creator 7", AvatarImageUrl = "social_reward_chai.png" });
            TopCreators.Add(new Influencer { Username = "Top Creator 8", AvatarImageUrl = "social_reward_chai.png" });
            TopCreators.Add(new Influencer { Username = "Top Creator 9", AvatarImageUrl = "social_reward_chai.png" });
            TopCreators.Add(new Influencer { Username = "Top Creator 10", AvatarImageUrl = "social_reward_chai.png" });

            Leaderboard.Add(MostPoints);
            Leaderboard.Add(LeastWaste);
            Leaderboard.Add(TopCreators);

            foreach(List<Influencer> sublist in Leaderboard)
            {
                foreach(Influencer influencer in sublist)
                {
                    Random rnd = new Random();
                    influencer.CreatorPoints = rnd.Next(500, 25000);
                }
            }

        }

        public static void PopulateFolderColors()
        {
            FolderColors = new List<string>();

            JObject parsedJson = JObject.Parse(ReadJsonFile("folder_colors.json"));

            JArray colorsData = (JArray)parsedJson.GetValue("hex_colour_codes");

            FolderColors = colorsData.ToObject<List<string>>();

            Console.WriteLine("Colors loaded");
        }


        public static string ReadJsonFile(string filename)
        {
            string readJson = "";

            string jsonFileName = filename;

            System.Reflection.Assembly Assembly = IntrospectionExtensions.GetTypeInfo(typeof(FakeData)).Assembly;

            Stream stream = Assembly.GetManifestResourceStream($"ChaiCooking.{jsonFileName}");

            using (var reader = new System.IO.StreamReader(stream))
            {
                readJson = reader.ReadToEnd();
            }

            return readJson;

        }

    }
}