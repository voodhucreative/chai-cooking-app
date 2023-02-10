using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChaiCooking.DebugData.Custom;
using ChaiCooking.Models;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.AlbumAPI;
using ChaiCooking.Models.Custom.ShoppingBasket;
using ChaiCooking.Services.Storage;
using static ChaiCooking.Helpers.Custom.Accounts;

namespace ChaiCooking.Services
{
    // The data manager provides a simple, single point of entry system for data access across the app
    // It handles the flow of data from the various internal and external systems

    // 3 data stores are used

    // 1. API data - either from the live API or FakeData
    // 2. Static data - data either contained in code or as bundled resource
    // 3. Local data - app data and settings stored on the device


    public static class DataManager
    {
        /*
        public static Address TestAddress;

        public static User TestUser;

        public static Location TestLocation;

        public static List<Recipe> FavouriteRecipes;

        public static Recipe SingleRecipe;

        public static RecipeFeed UserRecipeFeed;

        public static List<Album> UserAlbums;

        public static List<Influencer> Influencers;

        public static List<List<Influencer>> Leaderboard;

        */


        public static async Task<bool> Authorize(string whiskCode)
        {
            return await App.ApiBridge.Authorize(whiskCode);
        }

        public static async Task<bool> UpdateUserPrefs(AccountType accountType, bool savePrefs)
        {
            bool updated = await App.ApiBridge.UpdatePreferences(AppSession.CurrentUser, savePrefs);

            if (savePrefs)
            {
                if (updated)
                {
                    UserPreferences prefs = null;

                    try
                    {
                        prefs = await App.ApiBridge.GetPreferences(AppSession.CurrentUser);
                        AppSession.CurrentUser.Preferences = prefs;
                        prefs.AccountType = accountType;
                        LocalDataStore.SaveAll();
                    }
                    catch (Exception e)
                    {

                    }
                }
                else
                {
                    // SPECIAL CASE FOR MEAT EATER!!
                    //if (AppSession.CurrentUser.Preferences.)
                    App.ShowAlert("Failed to update preferences.");
                }
            }
            return updated;
        }



        // Album Recipes
        public static async Task<bool> AddRecipeToAlbum(Recipe recipe, Album album)
        {
            await Task.Delay(50);
            return await App.ApiBridge.AddRecipeToAlbum(AppSession.CurrentUser, recipe, album);
        }

        public static async Task<bool> RemoveRecipeFromAlbum(string albumID, string recipeId)
        {
            await Task.Delay(50);
            return await App.ApiBridge.RemoveRecipeFromAlbum(AppSession.CurrentUser, albumID, recipeId);
        }

        // Albums
        public static async Task<ColourTileModel> GetAlbumColours()
        {
            await Task.Delay(50);
            return await App.ApiBridge.ListAlbumColours();
        }

        public static async Task<List<Album>> GetAlbums()
        {
            await Task.Delay(50).ConfigureAwait(false);
            List<Album> albums = new List<Album>();
            albums = App.ApiBridge.ListAlbums(AppSession.CurrentUser).Result;
            return albums;
        }

        public static async Task<List<Album>> GetAlbums(CancellationToken token, String orderBy = null)
        {
            await Task.Delay(50);

            if (orderBy == null)
            {
                AppSession.CurrentUser.Albums = await App.ApiBridge.ListAlbums(AppSession.CurrentUser);
            }
            else
            {
                // Can be utilised in the future
                AppSession.CurrentUser.Albums = await App.ApiBridge.ListAlbums(AppSession.CurrentUser);
            }

            if (token.IsCancellationRequested)
            {
                return null;
            }
            else
            {
                return AppSession.CurrentUser.Albums;
            }
        }

        public static async Task<Album> CreateAlbum(string name, string color)
        {
            await Task.Delay(50);
            var result = await App.ApiBridge.CreateAlbum(AppSession.CurrentUser, name, color);
            return result;
        }

        public static async Task<Album> ViewAlbum(string id)
        {
            await App.ShowLoading();
            var result = await App.ApiBridge.ViewAlbum(AppSession.CurrentUser, id);
            await App.HideLoading();
            return result;
        }

        public static async Task<bool> EditAlbum(string albumID, string name, string albumColour)
        {
            await Task.Delay(50);
            var result = await App.ApiBridge.EditAlbum(
            AppSession.CurrentUser, albumID, name, albumColour);
            return result;
        }

        public static async Task<bool> RemoveAlbum(string albumID)
        {
            await Task.Delay(50);
            var result = await App.ApiBridge.RemoveAlbum(AppSession.CurrentUser, albumID);
            return result;
        }

        public static async Task<List<Preference>> GetAllergens() // replace string list with list of allergens
        {
            //await Task.Delay(50);
            List<Preference> allergens = new List<Preference>();

            allergens = App.ApiBridge.GetAllergens().Result;
            /*
            if (AppSettings.UseFakeData)
            {
                allergens = FakeData.UserAlbums;
            }
            else
            {
                allergens = App.ApiBridge.ListAlbums().Result;
            }*/

            return allergens;
        }

        public static async Task<List<IngredientUnit>> GetUnits()
        {
            List<IngredientUnit> units = new List<IngredientUnit>();

            units = App.ApiBridge.GetUnits().Result;

            return units;
        }


        public static async Task<List<Preference>> GetDiets() // replace string list with list of allergens
        {
            //await Task.Delay(50);
            List<Preference> diets = new List<Preference>();

            diets = App.ApiBridge.GetDiets().Result;
            return diets;
        }

        //public static async Task<List<Recipe>> GetRecipeFeed()
        //{
        //    await Task.Delay(50);
        //    List<Recipe> recipeFeed = new List<Recipe>();

        //    if (AppSettings.UseFakeData)
        //    {
        //        return GetTestRecipes();// Services.Converters.RecipeConverter.RecipeFeedToRecipeList(FakeData.UserRecipeFeed);
        //    }
        //    else
        //    {
        //        return await App.ApiBridge.GetRecipeFeed();
        //    }
        //    throw new ApiException("API ERROR: Recipe feed not available");
        //}


        public static async Task<Recipe> GetRecipe(string id)
        {
            await Task.Delay(50);
            Recipe recipe = new Recipe();
            AppSettings.UseFakeData = false;
            if (AppSettings.UseFakeData)
            {
                return GetSingleRecipe(id);
            }
            else
            {
                return await App.ApiBridge.GetRecipe(id);
            }
            throw new ApiException("API ERROR: Recipe feed not available");
        }


        public static async Task<List<Recipe>> GetFilteredRecipes(List<string> keywords, List<string> diets, List<string> allergens, List<string> includes, List<string> excludes)
        {
            await Task.Delay(50);
            List<Recipe> recipeList = new List<Recipe>();




            return recipeList;
        }


        public static async Task<VideoFeed> GetVideos(int category)
        {

            VideoFeed videos = new VideoFeed();

            videos = await App.ApiBridge.GetVideos(category);

            //string videoThumb = videos.MainImage;

            /*
            foreach (Video video in videos.Videos)
            {
                video.MainImage = videoThumb;
            }
            */
            return videos;
        }


        /// <summary>
        ///  Recipe Editor
        /// </summary>
        /// <returns></returns>

        // List recipes by the current user
        public static async Task<List<Recipe>> GetUserRecipes()
        {
            await Task.Delay(50).ConfigureAwait(false);
            List<Recipe> recipes = new List<Recipe>();
            recipes = App.ApiBridge.GetUserRecipes(AppSession.CurrentUser).Result;
            return recipes;
        }

        // Delete recipe
        public static async Task<bool> DeleteUserRecipe(string recipeID)
        {
            await Task.Delay(50).ConfigureAwait(false);
            bool result = App.ApiBridge.DeleteUserRecipe(AppSession.CurrentUser, recipeID).Result;
            return result;
        }

        /*
        // Create recipe
        public static async Task<bool> CreateUserRecipe(int coreIngredientID,
            int mealCategoryID, string mealType, string name, string yield,
            string cuisineType = null, string dishType = null)
        {
            await Task.Delay(50).ConfigureAwait(false);
            bool result = App.ApiBridge.CreateUserRecipe(AppSession.CurrentUser,
                coreIngredientID, mealCategoryID, mealType, name, yield, cuisineType, dishType).Result;
            return result;
        }
        
        // Update recipe
        public static async Task<bool> UpdateUserRecipe(string recipeID,
            int coreIngredientID, int mealCategoryID, string mealType, string method,
            string name, string yield, string cuisineType = null,
            string dishType = null, int cookTime = 0,
            int prepTime = 0, int totalTime = 0)
        {
            await Task.Delay(50).ConfigureAwait(false);
            bool result = App.ApiBridge.UpdateUserRecipe(AppSession.CurrentUser,
                recipeID, coreIngredientID, mealCategoryID, mealType, method,
                name, yield, cuisineType, dishType, cookTime, prepTime, totalTime).Result;
            return result;
        }


        
        // Delete recipe
        public static async Task<bool> DeleteUserRecipe(string recipeID)
        {
            await Task.Delay(50).ConfigureAwait(false);
            bool result = App.ApiBridge.DeleteUserRecipe(AppSession.CurrentUser, recipeID).Result;
            return result;
        }

        // View a recipe's ingredients
        public static async Task<List<Ingredient>> ViewUserRecipeIngredients(string recipeID)
        {
            await Task.Delay(50).ConfigureAwait(false);
            List<Ingredient> ingredients = new List<Ingredient>();
            ingredients = App.ApiBridge.ViewUserRecipeIngredients(AppSession.CurrentUser, recipeID).Result;
            return ingredients;
        }

        // Add ingredient to recipe
        public static async Task<bool> AddUserRecipeIngredient(string recipeID,
             int amount, int baseIngredientID, string recipeComponent, int unitID)
        {
            await Task.Delay(50).ConfigureAwait(false);
            bool result = App.ApiBridge.AddUserRecipeIngredient(AppSession.CurrentUser,
                recipeID, amount, baseIngredientID, recipeComponent, unitID).Result;
            return result;
        }

        // Updating a recipe ingredient
        public static async Task<bool> UpdateUserRecipeIngredient(string recipeID,
            string ingredientID, int amount,
            int baseIngredientID, string recipeComponent, int unitID)
        {
            await Task.Delay(50).ConfigureAwait(false);
            bool result = App.ApiBridge.UpdateUserRecipeIngredient(AppSession.CurrentUser,
                recipeID, ingredientID, amount, baseIngredientID, recipeComponent, unitID).Result;
            return result;
        }

        // Deleting a recipe ingredient
        public static async Task<bool> DeleteUserRecipeIngredient(string recipeID, string ingredientID)
        {
            await Task.Delay(50).ConfigureAwait(false);
            bool result = App.ApiBridge.DeleteUserRecipeIngredient(AppSession.CurrentUser,
                recipeID, ingredientID).Result;
            return result;
        }
        */









        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static User GetCurrentUser()
        {
            if (AppSettings.UseFakeData)
            {
                User debugUser = (User)FakeData.TestUser.Clone();
                debugUser.Albums = FakeData.UserAlbums;
                return debugUser;
            }

            throw new ApiException("API ERROR: User not available");

        }

        public static List<Influencer> GetInfluencers()
        {
            if (AppSettings.UseFakeData)
            {
                return FakeData.Influencers;
            }
            throw new ApiException("API ERROR: Influencers not available");
        }

        public static Influencer GetInfluencer(int id)
        {
            if (AppSettings.UseFakeData)
            {
                return FakeData.Influencers[id];
            }
            throw new ApiException("API ERROR: Influencers not available");
        }

        public static List<Recipe> GetTestRecipes()
        {
            if (AppSettings.UseFakeData)
            {
                return Services.Converters.RecipeConverter.RecipeFeedToRecipeList(FakeData.UserRecipeFeed);
            }
            throw new ApiException("API ERROR: Recipe feed not available");
        }

        public static Recipe GetSingleRecipe(string id)
        {
            if (AppSettings.UseFakeData)
            {

            }
            return App.ApiBridge.GetRecipe(id).Result;
        }

        public static List<Recipe> GetAllMyRecipes()
        {
            List<Recipe> allMyRecipes = new List<Recipe>();

            foreach (Recipe recipe in GetMyUnfinishedRecipes())
            {
                allMyRecipes.Add(recipe);
            }

            foreach (Recipe recipe in GetMyPendingRecipes())
            {
                allMyRecipes.Add(recipe);
            }

            foreach (Recipe recipe in GetMyPublishesRecipes())
            {
                allMyRecipes.Add(recipe);
            }

            return allMyRecipes;
        }

        public static async Task<List<Ingredient>> SearchIngredients(string ingredientName, bool exactMatch)
        {
            return await App.ApiBridge.SearchIngredients(ingredientName, exactMatch);
        }

        public static async Task<Ingredient> SearchIngredients(string ingredientName)
        {
            return await App.ApiBridge.SearchIngredients(ingredientName);
        }

        public static async Task<string> SaveUserRecipe(User user, Recipe recipe)
        {
            return await App.ApiBridge.SaveUserRecipe(user, recipe);
        }

        public static async Task<bool> UpdateUserRecipe(User user, Recipe recipe)
        {
            return await App.ApiBridge.UpdateUserRecipe(user, recipe);
        }

        public static async Task<bool> AddIngredientToRecipe(User user, string recipeId, Ingredient ingredient)
        {
            return await App.ApiBridge.AddIngredientToRecipe(user, recipeId, ingredient);
        }

        public static async Task<bool> UpdateRecipeIngredient(User user, string recipeId, Ingredient ingredient)
        {
            return await App.ApiBridge.UpdateUserRecipeIngredient(user, recipeId, ingredient);
        }

        public static async Task<bool> DeleteUserRecipeIngredient(User user, string recipeID, Ingredient ingredient)
        {
            return await App.ApiBridge.DeleteUserRecipeIngredient(user, recipeID, ingredient);
        }

        public static List<Recipe> GetMyPublishesRecipes()
        {
            if (FakeData.MyPublishedRecipes.Count == 0)
            {
                FakeData.MyPublishedRecipes = App.ApiBridge.GetFavouriteRecipes(AppSession.CurrentUser).Result;
            }
            return FakeData.MyPublishedRecipes;
            //return App.ApiBridge.GetFavouriteRecipes(AppSession.CurrentUser).Result;
        }

        public static List<Recipe> GetMyUnfinishedRecipes()
        {
            if (FakeData.MyUnfinishedRecipes.Count == 0)
            {
                FakeData.MyUnfinishedRecipes = App.ApiBridge.GetFavouriteRecipes(AppSession.CurrentUser).Result;
            }
            return FakeData.MyUnfinishedRecipes;
            //return App.ApiBridge.GetFavouriteRecipes(AppSession.CurrentUser).Result;
        }

        public static List<Recipe> GetMyPendingRecipes()
        {
            if (FakeData.MyPendingRecipes.Count == 0)
            {
                FakeData.MyPendingRecipes = App.ApiBridge.GetFavouriteRecipes(AppSession.CurrentUser).Result;
            }
            return FakeData.MyPendingRecipes;
            //return App.ApiBridge.GetFavouriteRecipes(AppSession.CurrentUser).Result;
        }

        public static List<Recipe> GetFavouriteRecipes()
        {
            if (AppSettings.UseFakeData)
            {

            }
            return App.ApiBridge.GetFavouriteRecipes(AppSession.CurrentUser).Result;
        }

        public static async Task<List<Recipe>> GetFavouriteRecipes(CancellationToken token, String orderBy = null)
        {
            await Task.Delay(50);

            if (orderBy == null)
            {
                AppSession.UserCollectionRecipes = await App.ApiBridge.GetFavouriteRecipes(AppSession.CurrentUser);
            }
            else
            {
                // Can be utilised in the future
                AppSession.UserCollectionRecipes = await App.ApiBridge.GetFavouriteRecipes(AppSession.CurrentUser);
            }

            if (token.IsCancellationRequested)
            {
                return null;
            }
            else
            {
                return AppSession.UserCollectionRecipes;
            }
        }

        public static List<Recipe> GetRecommendedRecipes()
        {
            if (AppSettings.UseFakeData)
            {

            }
            return App.ApiBridge.GetRecommendedRecipes(AppSession.CurrentUser).Result;
        }

        public static List<Recipe> GetWasteLessRecipes()
        {
            if (AppSettings.UseFakeData)
            {

            }
            return App.ApiBridge.GetWasteLessRecipes(AppSession.CurrentUser).Result;
        }

        public static bool AddFavourite(string id)
        {
            if (AppSettings.UseFakeData)
            {

            }
            return App.ApiBridge.AddFavourite(AppSession.CurrentUser, id).Result;
        }

        public static bool RemoveFavourite(string id)
        {
            if (AppSettings.UseFakeData)
            {

            }
            return App.ApiBridge.RemoveFavourite(AppSession.CurrentUser, id).Result;
        }

        public static List<Recipe> GetHoldingAreaRecipes()
        {
            if (AppSettings.UseFakeData)
            {
                return Services.Converters.RecipeConverter.RecipeFeedToRecipeList(FakeData.FavouritesFeed);
            }
            throw new ApiException("API ERROR: Favourite not available");
        }

        public static List<Recipe> GetRandomRecipes()
        {
            // test - simulates an altered list being returned, based on filters
            Random rnd = new Random();
            int numItems = rnd.Next(1, 10);

            if (numItems < 5)
            {
                Console.WriteLine("Returning test recipes");
                return GetTestRecipes();
            }
            Console.WriteLine("Returning waste less recipes");
            return GetWasteLessRecipes();
            throw new ApiException("API ERROR: Favourite not available");
        }

        /*
        public static List<Album> GetUserAlbums()
        {
            if (AppSettings.UseFakeData)
            {
                return FakeData.UserAlbums;
            }
            throw new ApiException("API ERROR: User albums not available");
        }*/

        public static List<List<Influencer>> GetLeaderboard()
        {
            if (AppSettings.UseFakeData)
            {
                return FakeData.Leaderboard;
            }
            throw new ApiException("API ERROR: Leaderboard not available");
        }

        public static MealPlanModel GetMealPlans()
        {
            if (AppSettings.UseFakeData)
            {
                return FakeData.previewMealPlan;
            }
            throw new ApiException("API ERROR: Meal Plan not available");
        }

        public static List<Recipe> GetRecommendedVeganRecipes()
        {
            if (AppSettings.UseFakeData)
            {

            }
            return App.ApiBridge.GetRecommendedVeganRecipes(AppSession.CurrentUser).Result;
        }


        public static List<Recipe> GetWasteLessRecipes(User user, bool update)
        {
            if (AppSession.WasteLessRecipes == null || update || AppSession.WasteLessRecipes.Count == 0)
            {
                AppSession.WasteLessRecipes = App.ApiBridge.GetWasteLessRecipes(user).Result;
            }

            return AppSession.WasteLessRecipes;
        }

        public static List<Recipe> GetRecommendedRecipes(User user, bool update)
        {
            if (AppSession.RecommendedRecipes == null || update || AppSession.RecommendedRecipes.Count == 0)
            {
                AppSession.RecommendedRecipes = App.ApiBridge.GetRecommendedRecipes(user).Result;
            }

            return AppSession.RecommendedRecipes;
        }

        public static List<Recipe> SearchRecipes(User user, bool update)
        {
            try
            {
                if (AppSession.SearchedRecipes == null || update || AppSession.SearchedRecipes.Count == 0)
                {
                    AppSession.SearchedRecipes = App.ApiBridge.SearchRecipes(user, true, true, false, false).Result;
                }
            }
            catch (Exception e)
            {
                App.ShowAlert("No results");
            }

            return AppSession.SearchedRecipes;
        }

        /*
        public static List<Recipe>SearchRecipes(User user, bool update)
        {
            if (AppSession.SearchedRecipes == null || update || AppSession.SearchedRecipes.Count == 0)
            {
                AppSession.SearchedRecipes = App.ApiBridge.SearchRecipes(user, false, true, false).Result;
            }

            return AppSession.SearchedRecipes;
        }
        */


        public static void FilterRecipesByIngredients(List<Ingredient> ingredientsToInclude, List<Ingredient> ingredientsToAvoid)
        {
            if (ingredientsToInclude != null)
            {
                foreach (Ingredient ingredientToInclude in ingredientsToInclude)
                {
                    Console.WriteLine("Filter:Include: " + ingredientToInclude.Name);
                }

            }

            if (ingredientsToAvoid != null)
            {
                foreach (Ingredient ingredientToAvoid in ingredientsToAvoid)
                {
                    Console.WriteLine("Filter:Avoid: " + ingredientToAvoid.Name);
                }
            }
        }

        public static bool AddWeek(int mealPlanTemplateId, string date)
        {
            return App.ApiBridge.AddWeek(AppSession.CurrentUser, date).Result;
        }

        // shopping basket

        public static Task<ShoppingList> GetShoppingList()
        {
            return App.ApiBridge.GetShoppingList(AppSession.CurrentUser);
        }
        public static Task<bool> AddRecipeToShoppingList(Recipe recipe)
        {
            return App.ApiBridge.AddRecipeToShoppingList(AppSession.CurrentUser, recipe);
        }
        public static Task<bool> RemoveRecipeFromShoppingList(Recipe recipe)
        {
            return App.ApiBridge.RemoveRecipeFromShoppingList(AppSession.CurrentUser, recipe);
        }
        public static Task<Uri> ConvertToBasket()
        {
            return App.ApiBridge.ConvertToBasket(AppSession.CurrentUser);
        }

        public static Task<bool> ClearShoppingBasket()
        {
            return App.ApiBridge.ClearShoppingBasket(AppSession.CurrentUser);
        }
    }
}
