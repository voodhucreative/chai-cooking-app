using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.AlbumAPI;
using ChaiCooking.Models.Custom.Feed;
using ChaiCooking.Models.Custom.InfluencerAPI;
using ChaiCooking.Models.Custom.MealPlanAPI;
using ChaiCooking.Models.Custom.ShoppingBasket;
using ChaiCooking.Services.Api;
using ChaiCooking.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace ChaiCooking.Services
{
    public class MockApiBridge : IApiBridge
    {
        static HttpClient HttpClient;

        private static string ApiRootUrl = Device.RuntimePlatform == Device.Android ? "http://10.0.2.2:3000/" : "http://localhost:3000/";
        private static string Request;

        public async Task<bool> Init()
        {
            await Task.Delay(100).ConfigureAwait(false);
            HttpClient = new HttpClient();
            return true;
        }

        public static bool ApiConnectionAvailable()
        {
            return CrossConnectivity.Current.IsConnected;
        }

        public async Task<User> LogIn(string email, string password)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            User loggedInUser = new User();

            return loggedInUser;
        }

        public async Task<bool> LoginUser(User user)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            return true;
        }

        public async Task<bool> SwapWhiskToken(User user)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            return true;
        }

        public async Task<bool> LogOut(User user, bool forceLogout)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            return true;
        }

        public async Task<RecipeFeed> GetRecipes(User user)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            throw new NotImplementedException();
        }

        public async Task<Recipe> GetRecipe(string id)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            throw new NotImplementedException();
        }

        public async Task<bool> CreateUser(User user)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            User createdUser = null;

            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

            try
            {
                string uri = ApiRootUrl + "users";

                JObject jObject = new JObject();

                jObject.Add("foremname", user.FirstName);
                jObject.Add("surname", user.LastName);
                jObject.Add("username", user.Username);
                jObject.Add("date_of_birth", user.DateOfBirth);
                jObject.Add("username", user.Username);
                jObject.Add("email", user.EmailAddress);
                jObject.Add("password", user.Password);
                jObject.Add("password_confirmation", user.Password);

                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);

                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = HttpClient.PostAsync(uri, content).Result;
                var json = await result.Content.ReadAsStringAsync();
                var jResponse = JObject.Parse(json);

                // and now what? 

            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR");
                return true;
            }

            return true;
        }

        public async Task<bool> DeleteUser(User account)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            throw new NotImplementedException();
        }

        public async Task<bool> ChangePassword(string currentPassword, string newPassword, string newPasswordConfirmation)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            throw new NotImplementedException();
        }

        public async Task<ColourTileModel> ListAlbumColours()
        {
            await Task.Delay(1000).ConfigureAwait(false);

            throw new NotImplementedException();
        }

        public async Task<List<Album>> ListAlbums(User user)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            throw new NotImplementedException();
        }

        public async Task<Album> CreateAlbum(User user, string name, string color)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            throw new NotImplementedException();
        }

        public async Task<Album> ViewAlbum(User user, string albumID)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            throw new NotImplementedException();
        }

        public async Task<bool> EditAlbum(User user, string albumID, string name, string color)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            throw new NotImplementedException();
        }

        public async Task<bool> RemoveAlbum(User user, string albumID)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            throw new NotImplementedException();
        }

        public async Task<bool> AddRecipeToAlbum(User user, Recipe recipe, Album album)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            throw new NotImplementedException();
        }

        public async Task<bool> RemoveRecipeFromAlbum(User user, string albumID, string recipeId)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            throw new NotImplementedException();
        }

        public async Task<List<Preference>> ListAllergens()
        {
            await Task.Delay(1000).ConfigureAwait(false);

            throw new NotImplementedException();
        }

        public async Task<List<Preference>> ListDiets()
        {
            await Task.Delay(1000).ConfigureAwait(false);

            throw new NotImplementedException();
        }

        public async Task<List<Recipe>> GetRecipes()
        {
            await Task.Delay(1000).ConfigureAwait(false);

            throw new NotImplementedException();
        }

        public async Task<Meal> AddMealToMealPlan(User user, string mealType, string mealplanID, string date, string chaiID, string whiskID, bool isCalendar)
        {
            await Task.Delay(1000).ConfigureAwait(false);
            throw new NotImplementedException();
        }

        public async Task<Meal> UpdateMealOnMealPlan(User user, string mealID, string mealplanID, string chaiID, string whiskID)
        {
            await Task.Delay(1000).ConfigureAwait(false);
            throw new NotImplementedException();
        }

        public async Task<string> RemoveMealOnMealPlan(User user, string mealID, string mealplanID)
        {
            await Task.Delay(1000).ConfigureAwait(false);
            throw new NotImplementedException();
        }

        public Task<User> LogIn(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Authorize(string WhiskCode)
        {
            throw new NotImplementedException();
        }

        public Task<List<Recipe>> SearchRecipes(User user)
        {
            throw new NotImplementedException();
        }

        public Task<List<Preference>> GetAllergens()
        {
            throw new NotImplementedException();
        }

        public Task<List<Preference>> GetDiets()
        {
            throw new NotImplementedException();
        }

        Task<bool> IApiBridge.LogIn(User user)
        {
            throw new NotImplementedException();
        }

        Task<bool> IApiBridge.AddWeek(User user, string date)
        {
            throw new NotImplementedException();
        }

        Task<string> IApiBridge.CreateMealPlan(User user, string name, string date, int days)
        {
            throw new NotImplementedException();
        }

        public Task<InfluencerMealPlans> BrowseMealPlans(User user)
        {
            throw new NotImplementedException();
        }

        Task<InfluencerMealPlans> IApiBridge.GetInfluencerMealPlans(User user)
        {
            throw new NotImplementedException();
        }

        Task<MealPlanModel> IApiBridge.GetInfluencerMealPlanWeek(User user, string week)
        {
            throw new NotImplementedException();
        }

        Task<Influencer> IApiBridge.GetInfluencers(User user, int page)
        {
            throw new NotImplementedException();
        }

        Task<UserMealPlans> IApiBridge.GetUserMealPlans(User user)
        {
            throw new NotImplementedException();
        }

        Task<bool> IApiBridge.DeleteUserMealPlan(User user, string mealPlanID)
        {
            throw new NotImplementedException();
        }

        Task<UserMealTemplate> IApiBridge.GetWeek(User user, string mealPlanID, string week, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        Task<UserMealTemplate> IApiBridge.GetCalendar(User user, string date, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddFavourite(User user, string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveFavourite(User user, string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Recipe>> GetWasteLessRecipes(User user)
        {
            throw new NotImplementedException();
        }

        public Task<List<Recipe>> GetFavouriteRecipes(User user)
        {
            throw new NotImplementedException();
        }

        public Task<List<Recipe>> GetRecommendedRecipes(User user)
        {
            throw new NotImplementedException();
        }

        public Task<List<Recipe>> SearchRecipes(User user, bool usePrefs, bool useKeyWords, bool useIncludes)
        {
            throw new NotImplementedException();
        }

        public Task<VideoFeed> GetVideos(int category)
        {
            throw new NotImplementedException();
        }

        public Task<List<Recipe>> GetRecommendedVeganRecipes(User user)
        {
            throw new NotImplementedException();
        }

        public Task<List<Recipe>> SearchRecipes(User user, bool usePrefs, bool useKeyWords, bool useIncludes, bool forceVegan, string route)
        {
            throw new NotImplementedException();
        }

        public Task<UserMealTemplate> AutocompleteMealPlan(User user, string mealPlanID)
        {
            throw new NotImplementedException();
        }

        public Task<ShoppingList> GetShoppingList(User user)
        {
            throw new NotImplementedException();
        }
        public Task<bool> AddRecipeToShoppingList(User user, Recipe recipe)
        {
            throw new NotImplementedException();
        }
        public Task<bool> RemoveRecipeFromShoppingList(User user, Recipe recipe)
        {
            throw new NotImplementedException();
        }
        public Task<Uri> ConvertToBasket(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ClearShoppingBasket(User user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  Recipe Editor
        /// </summary>
        /// <returns></returns>

        // List recipes by the current user
        public Task<List<Recipe>> GetUserRecipes(User user)
        {
            throw new NotImplementedException();
        }
        /*
        // Create recipe
        public Task<bool> CreateUserRecipe(User user, int coreIngredientID,
            int mealCategoryID, string mealType, string name, string yield,
            string cuisineType = null, string dishType = null)
        {
            throw new NotImplementedException();
        }

        // Update recipe
        public Task<bool> UpdateUserRecipe(User user, string recipeID,
            int coreIngredientID, int mealCategoryID, string mealType, string method,
            string name, string yield, string cuisineType = null,
            string dishType = null, int cookTime = 0, int prepTime = 0,
            int totalTime = 0)
        {
            throw new NotImplementedException();
        }
        */
        // Delete recipe
        public Task<bool> DeleteUserRecipe(User user, string recipeID)
        {
            throw new NotImplementedException();
        }
        /*
        // View a recipe's ingredients
        public Task<List<Ingredient>> ViewUserRecipeIngredients(User user, string recipeID)
        {
            throw new NotImplementedException();
        }

        // Add ingredient to recipe
        public Task<bool> AddUserRecipeIngredient(User user,
            string recipeID, int amount, int baseIngredientID, string recipeComponent, int unitID)
        {
            throw new NotImplementedException();
        }

        // Updating a recipe ingredient
        public Task<bool> UpdateUserRecipeIngredient(User user,
            string recipeID, string ingredientID, int amount,
            int baseIngredientID, string recipeComponent, int unitID)
        {
            throw new NotImplementedException();
        }

        // Deleting a recipe ingredient
        public Task<bool> DeleteUserRecipeIngredient(User user, string recipeID,
            string ingredientID)
        {
            throw new NotImplementedException();
        }
        */
        public Task<bool> UpdatePreferences(User user)
        {
            throw new NotImplementedException();
        }

        public Task<UserPreferences> GetPreferences(User user)
        {
            throw new NotImplementedException();
        }

        public Task<string> SaveUserRecipe(User user, Recipe recipe)
        {
            throw new NotImplementedException();
        }

        public Task<Ingredient> SearchIngredients(string ingredientName)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ingredient>> SearchIngredients(string ingredientName, bool matchExact)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddIngredientToRecipe(User user, string recipeId, Ingredient ingredient)
        {
            throw new NotImplementedException();
        }

        public Task<List<IngredientUnit>> GetUnits()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserRecipe(User user, Recipe recipe)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserRecipeIngredient(User user, string recipeID, Ingredient ingredient)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserRecipeIngredient(User user, string recipeID, Ingredient ingredient)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ForgotPassword(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateChaiSubscriptionPlan(User user)
        {
            throw new NotImplementedException();
        }

        public Task<List<Accounts.AccountType>> GetSubscriptions(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePlan(User user, string planType)
        {
            throw new NotImplementedException();
        }

        public Task<Accounts.AccountType> GetCurrentSubscription(User user)
        {
            throw new NotImplementedException();
        }

        public Task<List<Accounts.AccountType>> GetSubscriptionPlans(User user)
        {
            throw new NotImplementedException();
        }

        public Task<Accounts.AccountType> GetCurrentSubscriptionPlan(User user)
        {
            throw new NotImplementedException();
        }

        Task<List<string>> IApiBridge.GetSubscriptions(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteSubscriptionPlan(User user, string planType)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendAppleTransactionCode(User user, string code)
        {
            throw new NotImplementedException();
        }

        public Task<InfluencerMealPlans> GetUserMealPlanTemplates(User user)
        {
            throw new NotImplementedException();
        }

        public Task<string> CreateUserMealPlanTemplates(User user, string name, int days)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserMealPlanTemplates(User user, int templateId, string name, string publishedAt)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserMealPlanTemplates(User user, int templateId)
        {
            throw new NotImplementedException();
        }

        public Task<MealPlanModel> GetUserMealPlanDayTemplates(User user, int planId)
        {
            throw new NotImplementedException();
        }

        public Task<MealPlanModel.Datum> CreateDayTemplateOnMealPlan(User user, int templateId, string day)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteDayTemplateOnMealPlan(User user, int templateId, string day)
        {
            throw new NotImplementedException();
        }

        public Task<MealTemplate> AddMealToDayTemplate(User user, int dayTemplateId, string mealType, int recipeId, string whiskId, string image)
        {
            throw new NotImplementedException();
        }

        public Task<MealTemplate> UpdateMealOnDayTemplate(User user, int dayTemplateId, string mealType, int recipeId, string whiskId, string image)
        {
            throw new NotImplementedException();
        }

        public Task<MealTemplate> UpdateMealOnDayTemplate(User user, int dayTemplateId, int mealTemplateId, string mealType, int recipeId, string whiskId, string image)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMealOnDayTemplate(User user, int dayTemplateId, int mealTemplateId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePreferences(User user, bool updatePrefs)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendAppleTransactionCode(User user, string code, string purchaseDate, string purchaseDateMillis)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegenerateMeals(User currentUser)
        {
            throw new NotImplementedException();
        }
        public Task<string> GetMealPlanFromDate(User user, string date)
        {
            throw new NotImplementedException();
        }
    }
}

