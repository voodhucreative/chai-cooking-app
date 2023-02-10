using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChaiCooking.Models;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.AlbumAPI;
using ChaiCooking.Models.Custom.Feed;
using ChaiCooking.Models.Custom.InfluencerAPI;
using ChaiCooking.Models.Custom.MealPlanAPI;
using ChaiCooking.Models.Custom.ShoppingBasket;
using static ChaiCooking.Helpers.Custom.Accounts;

namespace ChaiCooking.Services.Api
{
    public interface IApiBridge
    {
        Task<bool> Init();

        Task<User> LogIn(string email, string password);
        Task<bool> LogIn(User user);

        Task<bool> LoginUser(User user);
        Task<bool> LogOut(User user, bool forceLogout);

        Task<bool> SwapWhiskToken(User user);

        Task<bool> AddFavourite(User user, string id);
        Task<bool> RemoveFavourite(User user, string id);

        Task<bool> AddWeek(User user, string date);

        Task<string> CreateMealPlan(User user, string name, string date, int days);
        Task<bool> CreateChaiSubscriptionPlan(User user);
        Task<List<string>> GetSubscriptions(User user);
        Task<List<AccountType>> GetSubscriptionPlans(User user);
        Task<bool> DeleteSubscriptionPlan(User user, string planType);
        Task<AccountType> GetCurrentSubscriptionPlan(User user);
        Task<bool> SendAppleTransactionCode(User user, string code, string purchaseDate, string purchaseDateMillis);

        Task<UserMealPlans> GetUserMealPlans(User user);

        Task<bool> DeleteUserMealPlan(User user, string mealPlanID);

        Task<InfluencerMealPlans> GetInfluencerMealPlans(User user);
        Task<InfluencerMealPlans> BrowseMealPlans(User user);
        Task<MealPlanModel> GetInfluencerMealPlanWeek(User user, string week);

        Task<UserMealTemplate> GetWeek(User user, string mealPlanID, string week, CancellationToken token);

        Task<UserMealTemplate> AutocompleteMealPlan(User user, string mealPlanID);

        Task<UserMealTemplate> GetCalendar(User user, string date, CancellationToken token);

        Task<Influencer> GetInfluencers(User user, int page);

        Task<Meal> AddMealToMealPlan(User user, string mealType, string mealplanID, string date, string chaiID, string whiskID, bool isCalendar);

        Task<Meal> UpdateMealOnMealPlan(User user, string mealID, string mealplanID, string chaiID, string whiskID);

        Task<string> RemoveMealOnMealPlan(User user, string mealID, string mealplanID);

        Task<bool> CreateUser(User user);

        Task<bool> UpdatePreferences(User user, bool updatePrefs);
        Task<UserPreferences> GetPreferences(User user);

        Task<bool> DeleteUser(User User);

        Task<bool> Authorize(string WhiskCode);

        Task<bool> ChangePassword(string currentPassword, string newPassword, string newPasswordConfirmation);

        // albums

        Task<ColourTileModel> ListAlbumColours();

        Task<List<Album>> ListAlbums(User user);

        Task<Album> CreateAlbum(User user, string name, string color);

        Task<Album> ViewAlbum(User user, string albumID);

        Task<bool> EditAlbum(User user, string albumID, string name, string color);

        Task<bool> RemoveAlbum(User user, string albumID);

        Task<bool> AddRecipeToAlbum(User user, Recipe recipe, Album album);

        Task<bool> RemoveRecipeFromAlbum(User user, string albumID, string recipeId);

        // shopping basket

        Task<ShoppingList> GetShoppingList(User user);
        Task<bool> AddRecipeToShoppingList(User user, Recipe recipe);
        Task<bool> RemoveRecipeFromShoppingList(User user, Recipe recipe);
        Task<Uri> ConvertToBasket(User user);
        Task<bool> ClearShoppingBasket(User user);

        // allergens
        Task<List<Preference>> ListAllergens();
        Task<List<Preference>> GetAllergens();

        // diets
        Task<List<Preference>> ListDiets();
        Task<List<Preference>> GetDiets();

        Task<List<IngredientUnit>> GetUnits();

        Task<RecipeFeed> GetRecipes(User user); // search

        Task<Recipe> GetRecipe(string id); // single
        //Task<List<Recipe>> SearchRecipes(User user, bool usePrefs, bool useKeyWords, bool useIncludes);
        Task<List<Recipe>> SearchRecipes(User user, bool usePrefs, bool useKeyWords, bool useIncludes, bool forceVegan, string route = "/recipe-search");

        Task<List<Recipe>> GetFavouriteRecipes(User user);
        Task<List<Recipe>> GetWasteLessRecipes(User user);
        Task<List<Recipe>> GetRecommendedRecipes(User user);
        Task<List<Recipe>> GetRecommendedVeganRecipes(User user);
        Task<VideoFeed> GetVideos(int category);
        Task<bool> ForgotPassword(string email);
        /// <summary>
        ///  Recipe Editor
        /// </summary>
        /// <returns></returns>

        // List recipes by the current user
        Task<List<Recipe>> GetUserRecipes(User user);

        Task<string> SaveUserRecipe(User user, Recipe recipe);
        Task<bool> UpdateUserRecipe(User user, Recipe recipe);

        Task<Ingredient> SearchIngredients(string ingredientName);
        Task<List<Ingredient>> SearchIngredients(string ingredientName, bool matchExact);

        Task<bool> AddIngredientToRecipe(User user, string recipeId, Ingredient ingredient);
        Task<bool> DeleteUserRecipeIngredient(User user, string recipeID, Ingredient ingredient);
        Task<bool> UpdateUserRecipeIngredient(User user, string recipeID, Ingredient ingredient);
        /*
        // Create recipe
        Task<bool> CreateUserRecipe(User user, int coreIngredientID,
            int mealCategoryID, string mealType, string name, string yield,
            string cuisineType = null, string dishType = null);

        // Update recipe
        Task<bool> UpdateUserRecipe(User user, string recipeID,
            int coreIngredientID, int mealCategoryID, string mealType, string method,
            string name, string yield, string cuisineType = null,
            string dishType = null, int cookTime = 0,
            int prepTime = 0, int totalTime = 0);
        */
        // Delete recipe
        Task<bool> DeleteUserRecipe(User user, string recipeID);
        Task<bool> RegenerateMeals(User currentUser);
        Task<string> GetMealPlanFromDate(User user, string date);

        /*
// View a recipe's ingredients
Task<List<Ingredient>> ViewUserRecipeIngredients(User user, string recipeID);

// Add ingredient to recipe
Task<bool> AddUserRecipeIngredient(User user,
string recipeID, int amount, int baseIngredientID, string recipeComponent, int unitID);

// Updating a recipe ingredient
Task<bool> UpdateUserRecipeIngredient(User user,
string recipeID, string ingredientID, int amount,
int baseIngredientID, string recipeComponent, int unitID);

// Deleting a recipe ingredient
Task<bool> DeleteUserRecipeIngredient(User user, string recipeID, string ingredientID);
*/

        Task<InfluencerMealPlans> GetUserMealPlanTemplates(User user);
        Task<string> CreateUserMealPlanTemplates(User user, string name, int days);
        Task<bool> UpdateUserMealPlanTemplates(User user, int templateId, string name, string publishedAt);
        Task<bool> DeleteUserMealPlanTemplates(User user, int templateId);
        Task<MealPlanModel> GetUserMealPlanDayTemplates(User user, int planId);
        Task<MealPlanModel.Datum> CreateDayTemplateOnMealPlan(User user, int templateId, string day);
        Task<bool> DeleteDayTemplateOnMealPlan(User user, int templateId, string day);
        Task<MealTemplate> AddMealToDayTemplate(User user, int dayTemplateId, string mealType, int recipeId, string whiskId, string image);
        Task<MealTemplate> UpdateMealOnDayTemplate(User user, int dayTemplateId, int mealTemplateId, string mealType, int recipeId, string whiskId, string image);
        Task<bool> DeleteMealOnDayTemplate(User user, int dayTemplateId, int mealTemplateId);
        
    }
}
