using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.AlbumAPI;
using ChaiCooking.Models.Custom.Feed;
using ChaiCooking.Models.Custom.InfluencerAPI;
using ChaiCooking.Models.Custom.MealPlanAPI;
using ChaiCooking.Models.Custom.ShoppingBasket;
using ChaiCooking.Services.Api;
using ChaiCooking.Services.Storage;
using ChaiCooking.Tools;
using ChaiCooking.Views.CollectionViews.ShoppingBasket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Xamarin.Essentials;
using static ChaiCooking.Helpers.Custom.Accounts;

namespace ChaiCooking.Services
{
    public class ApiBridge : IApiBridge
    {
        static HttpClient HttpClient;

        private static string ApiRootUrl = "https://app.chai.cooking/api/v1";
        private static string Request;

        public static int ITEMS_PER_REQUEST = 15;//40;
        public static int ITEMS_PER_CHUNK = 5;


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

        //This is being added to allow us to keep all bad responses in one place in the event of shared actions
        //Added optional bool to prevent the app loading up with the login window
        async void BadResponseHandler(HttpResponseMessage httpResponse, bool ignoreUnath = false)
        {
            var code = httpResponse.StatusCode;

            var json = await httpResponse.Content.ReadAsStringAsync();
            var jResponse = JObject.Parse(json);

            if (code == System.Net.HttpStatusCode.BadRequest)
            {
                //App.ShowAlert($"{jResponse.SelectToken("error")}");
            }
            else if (code == System.Net.HttpStatusCode.Unauthorized )
            {
                //This prevents certain unauth calls during boot up
                if (!ignoreUnath)
                {
                    UnauthorizedCheck(httpResponse);
                }
            }
            else if (code == System.Net.HttpStatusCode.Forbidden)
            {
                await App.ShowSubscribeModal();
            }
            else if (code == System.Net.HttpStatusCode.UnprocessableEntity)
            {
                //Not the nicest looking but pulls this together rather than keeping split for the sake of one difference.
                try
                {
                    StaticData.errorText = jResponse.SelectToken("errors.start_date[0]").ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine("The given data was invalid, the colour field is required.");
                }
            }
            else
            {
                Console.WriteLine($"Unhandled Response : {httpResponse.StatusCode} from {httpResponse.RequestMessage.RequestUri}");
                System.Diagnostics.Debugger.Break();
            }
        }

        public async void UnauthorizedCheck(HttpResponseMessage response)
        {
            try
            {
                Console.WriteLine(Helpers.Pages.GetCurrentPageId());
                var json = await response.Content.ReadAsStringAsync();
                var jResponse = JObject.Parse(json);
                string message = jResponse.GetValue("message").ToString();
 
                if (message.ToLower().Contains("unauthenticated"))
                {
                    StaticData.WhiskAuthenticate();
                }
                else
                {
                    //await LogOut(AppSession.CurrentUser, true);
                    /*
                    await Task.Run(() =>
                    {
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                        {
                            Console.WriteLine("BeginInvokeOnMainThread");
                            await App.GoToLoginOrRegister();
                            //await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.LoginAndRegistration);
                        });
                    });*/
                }
            }
            catch
            {

                /*
                await Task.Run(() =>
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                    {
                        Console.WriteLine("BeginInvokeOnMainThread");
                        await App.GoToLoginOrRegister();
                        //await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.LoginAndRegistration);
                    });
                });*/
            }
        }

        public async Task<bool> Authorize(string whiskCode)
        {
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                List<KeyValuePair<string, string>> keyValues = new List<KeyValuePair<string, string>>();

                keyValues.Add(new KeyValuePair<string, string>("code", whiskCode));

                var content = new FormUrlEncodedContent(keyValues);

                //var response = HttpClient.PostAsync(ApiRootUrl + "/whisk-tokens", content);
                //var responseString = await response.Result.Content.ReadAsStringAsync();
                //var jRep = JObject.Parse(responseString);

                string uri = ApiRootUrl + "/whisk-tokens";

                var result = HttpClient.PostAsync(uri, content).Result;

                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);
                    Console.WriteLine(json);

                    JObject data = (JObject)jResponse.GetValue("data");
                    JObject UserObj = (JObject)data.GetValue("user");
                    JObject UserSettingObj = (JObject)UserObj.GetValue("user_settings");
                    JObject UserDetailsObj = null;

                    try
                    {
                        UserDetailsObj = (JObject)UserSettingObj.GetValue("personal_details");
                    }
                    catch (Exception dError)
                    {

                    }

                    JObject ChaiData = (JObject)data.GetValue("chai");

                    string UserId = (string)UserObj.GetValue("id");
                    string UserEmail = (string)UserObj.GetValue("email");

                    // apple does not provide these
                    string FirstName = "Chai";
                    string LastName = "User";
                    string PhotoUrl = "chaismallbag.png";

                    if (UserDetailsObj != null)
                    {
                        if ((string)UserDetailsObj.GetValue("first_name") != null)
                        {
                            FirstName = (string)UserDetailsObj.GetValue("first_name");
                        }

                        if ((string)UserDetailsObj.GetValue("last_name") != null)
                        {
                            LastName = (string)UserDetailsObj.GetValue("last_name");
                        }

                        if ((string)UserDetailsObj.GetValue("photo_url") != null)
                        {
                            PhotoUrl = (string)UserDetailsObj.GetValue("photo_url");
                        }
                    }

                    string WhiskTokenId = (string)ChaiData.GetValue("whisk_token_id");
                    bool HasAccount = (bool)ChaiData.GetValue("has_account");
                    //App.Current.Properties["whiskTokenID"] = WhiskTokenId;
                    //await App.Current.SavePropertiesAsync();

                    AppSession.CurrentUser.EmailAddress = UserEmail;
                    AppSession.CurrentUser.AvatarImageUrl = PhotoUrl;
                    AppSession.CurrentUser.FirstName = FirstName;
                    AppSession.CurrentUser.LastName = LastName;
                    AppSession.CurrentUser.IsRegistered = HasAccount;
                    AppSession.CurrentUser.WhiskTokenId = Int32.Parse(WhiskTokenId);
                    //AppSession.CurrentUser.Id = UserId;
                    LocalDataStore.SaveAll();

                    if (!AppSession.CurrentUser.IsRegistered)
                    {
                        await CreateUser(AppSession.CurrentUser);
                    }

                    return true;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

        public async Task<bool> ReAuthorize(string whiskCode)
        {
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                List<KeyValuePair<string, string>> keyValues = new List<KeyValuePair<string, string>>();

                keyValues.Add(new KeyValuePair<string, string>("code", whiskCode));

                var content = new FormUrlEncodedContent(keyValues);

                //var response = HttpClient.PostAsync(ApiRootUrl + "/whisk-tokens", content);
                //var responseString = await response.Result.Content.ReadAsStringAsync();
                //var jRep = JObject.Parse(responseString);

                string uri = ApiRootUrl + "/whisk-tokens";

                var result = HttpClient.PostAsync(uri, content).Result;

                if (result.IsSuccessStatusCode)
                {

                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);
                    Console.WriteLine(json);

                    JObject data = (JObject)jResponse.GetValue("data");


                    JObject UserObj = (JObject)data.GetValue("user");

                    JObject ChaiData = (JObject)data.GetValue("chai");

                    string WhiskTokenId = (string)ChaiData.GetValue("whisk_token_id");
                    //App.Current.Properties["whiskTokenID"] = WhiskTokenId;
                    //await App.Current.SavePropertiesAsync();

                    AppSession.CurrentUser.WhiskTokenId = Int32.Parse(WhiskTokenId);
                    LocalDataStore.SaveAll();

                    return true;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

        public async Task<bool> LogIn(User user)
        {
            //await Task.Delay(1000).ConfigureAwait(false);

            //User loggedInUser = new User();

            //await CreateUser(user);

            //loggedInUser = await LoginUser(user);

            return true;
        }

        public async Task<bool> SwapWhiskToken(User user)
        {
            
            await Task.Delay(10).ConfigureAwait(false);
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + user.AuthToken);
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                Uri uri = new Uri(string.Format($"{ApiRootUrl}/user/swap-whisk-token", string.Empty));
                JObject jObject = new JObject();

                jObject.Add("whisk_token_id", user.WhiskTokenId);
                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = HttpClient.PostAsync(uri, content).Result;
                if (result.IsSuccessStatusCode)
                {
                    //App.Current.Properties["whiskTokenID"] = user.WhiskTokenId;
                    //await App.Current.SavePropertiesAsync();
                    AppSession.CurrentUser.WhiskTokenId = user.WhiskTokenId;
                    LocalDataStore.SaveAll();
                    Console.WriteLine("Swapped Token");
                    return true;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
            return false;
        }


        public async Task<bool> LoginUser(User user)
        {
            await Task.Delay(200).ConfigureAwait(false); // why?!

            string loginRoute = "/login";

            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (AppSettings.AutoChaiSignupAndLogin)
                {
                    loginRoute = "/app-login";
                }

                string uri = ApiRootUrl + loginRoute;

                JObject jObject = new JObject();

                if (AppSettings.AutoChaiSignupAndLogin)
                {
                    if (user.Password == null)
                    {
                        user.Password = Helpers.Custom.Accounts.GenerateChaiIdPassword(user);
                    }
                }

                jObject.Add("email", user.EmailAddress);

                if (AppSettings.AutoChaiSignupAndLogin)
                {
                    jObject.Add("app_access_token", AppSettings.APP_ACCESS_TOKEN);
                }
                else
                {
                    jObject.Add("password", user.Password);
                }

                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);

                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
               
                var response = HttpClient.PostAsync(uri, content);
                var responseString = await response.Result.Content.ReadAsStringAsync();

                //Was checking if ran to completion, never hit anything after this for some reason?
                if (response.Result.IsSuccessStatusCode)
                {
                    var jResponse = JObject.Parse(responseString);

                    JObject data = (JObject)jResponse.GetValue("data");
                    JObject userData = (JObject)data.GetValue("user");
                    string ApiToken = (string)data.GetValue("api_token");

                    AppSession.CurrentUser.AuthToken = ApiToken;
                    AppSession.CurrentUser.FirstName = (string)userData.GetValue("forename");
                    AppSession.CurrentUser.LastName = (string)userData.GetValue("surname");
                    AppSession.CurrentUser.Username = (string)userData.GetValue("username");
                    AppSession.CurrentUser.Id = "" + (int)userData.GetValue("id");

                    AppSession.CurrentUser.Role = (string)userData.GetValue("role");

                    if ((string)userData.GetValue("bio") != null)
                    {
                        AppSession.CurrentUser.Bio = (string)userData.GetValue("bio");
                    }

                    if ((string)userData.GetValue("image_url") != null)
                    {
                        AppSession.CurrentUser.AvatarImageUrl = (string)userData.GetValue("image_url");
                    }

                    StaticData.loginSuccess = true;

                    try
                    {
                        // get saved user preferences here
                        UserPreferences userPrefs = await App.ApiBridge.GetPreferences(AppSession.CurrentUser);

                        if (userPrefs != null)
                        {
                            AppSession.CurrentUser.Preferences = userPrefs;
                        }

                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Failed to update prefs");
                    }

                    LocalDataStore.SaveAll();

                    return true;
                }
                else
                {

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        // Code to run on the main thread
                        App.ShowAlert("ERROR", "Login failed, please check your details or create a Chai password.");
                        StaticData.loginSuccess = false;
                    });
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e);

                AppSession.CurrentUser.EmailAddress = "";
                AppSession.CurrentUser.AvatarImageUrl = "";
                AppSession.CurrentUser.FirstName = "";
                AppSession.CurrentUser.LastName = "";
                AppSession.CurrentUser.IsRegistered = false;
                AppSession.CurrentUser.AuthToken = null;
                AppSession.CurrentUser.WhiskTokenId = -1;

                LocalDataStore.SaveAll();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    // Code to run on the main thread
                    App.ShowAlert("ERROR", "Login failed, please check your details or create a Chai password.");
                });
                return false;
            }
        }

        public async Task<User> LogIn(string email, string password)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            User loggedInUser = new User();

            return loggedInUser;
        }

        public async Task<bool> LogOut(User user, bool forceLogout)
        {
            await Task.Delay(10).ConfigureAwait(false);
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);

            Uri uri = new Uri(string.Format($"{ApiRootUrl}/logout", string.Empty));
            var result = HttpClient.PostAsync(uri, null).Result;
            try
            {
                if (result.IsSuccessStatusCode)
                {
                    Console.WriteLine("Logged out successfully.");
                    AppSession.CurrentUser.AuthToken = null;
                    //string tempEmail = AppSession.CurrentUser.EmailAddress;
                    //bool tempRemember = AppSession.CurrentUser.RememberMe;
                    AppSession.CurrentUser = new User();
                    //AppSession.CurrentUser.EmailAddress = tempEmail;
                    //AppSession.CurrentUser.RememberMe = tempRemember;
                    LocalDataStore.SaveAll();
                    return true;
                }
                else
                {
                    BadResponseHandler(result, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
            LocalDataStore.SaveAll();
            return false;
        }

        public async Task<bool> LogOutOld(User user, bool forceLogout)
        {
            await Task.Delay(10).ConfigureAwait(false);

            if (forceLogout)
            {
                AppSession.CurrentUser.IsRegistered = false;
                AppSession.CurrentUser.AuthToken = null;
                AppSession.CurrentUser.WhiskTokenId = -1;

                string tempEmail = AppSession.CurrentUser.EmailAddress;
                bool tempRemember = AppSession.CurrentUser.RememberMe;
                AppSession.CurrentUser = new User();
                AppSession.CurrentUser.EmailAddress = tempEmail;
                AppSession.CurrentUser.RememberMe = tempRemember;

                LocalDataStore.SaveAll();
                return true;// false;
            }
            else
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);

                Uri uri = new Uri(string.Format($"{ApiRootUrl}/logout", string.Empty));
                var result = HttpClient.PostAsync(uri, null).Result;
                try
                {
                    if (result.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Logged out successfully.");
                        AppSession.CurrentUser.AuthToken = null;
                        string tempEmail = AppSession.CurrentUser.EmailAddress;
                        bool tempRemember = AppSession.CurrentUser.RememberMe;
                        AppSession.CurrentUser = new User();
                        AppSession.CurrentUser.EmailAddress = tempEmail;
                        AppSession.CurrentUser.RememberMe = tempRemember;
                        LocalDataStore.SaveAll();
                        return true;
                    }
                    else
                    {
                        BadResponseHandler(result);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e}");
                }
                return false;
            }
        }

        public async Task<bool> CreateUserPreferences(User user)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            return true;
        }

        public async Task<bool> GetUserPreferences(User user)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            return true;
        }

        public async Task<bool> UpdateUserPreferences(User user)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            return true;
        }

        public async Task<UserPreferences> GetPreferences(User user)
        {

            UserPreferences userPrefs = new UserPreferences();
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + user.AuthToken);

            try
            {
                string uri = ApiRootUrl + "/preferences";

                var result = HttpClient.GetAsync(uri).Result;
                var json = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    var jResponse = JObject.Parse(json);

                    JObject prefsData = (JObject)jResponse.GetValue("data");

                    if (prefsData != null)
                    {
                        JObject chaiData = (JObject)prefsData.GetValue("chai");
                        JObject userData = (JObject)prefsData.GetValue("user");

                        JObject userSettingsData = (JObject)userData.GetValue("user_settings");
                        JObject foodPrefsData = (JObject)userSettingsData.GetValue("food_preferences");

                        JObject dislikesData = null;
                        JArray dislikesList = null;
                        try
                        {
                            dislikesData = (JObject)foodPrefsData.GetValue("disliked_ingredients");
                            dislikesList = (JArray)dislikesData.GetValue("list");
                        }
                        catch (Exception dlEx)
                        {

                        }

                        if (chaiData != null)
                        {
                            JObject preferencesData = (JObject)chaiData.GetValue("preferences");
                            JArray allergensData = (JArray)chaiData.GetValue("allergens");
                            JArray dietsData = (JArray)chaiData.GetValue("diets");

                            userPrefs.HouseholdSize = "" + (int)preferencesData.GetValue("household_adults") + (int)preferencesData.GetValue("household_children");
                            userPrefs.WeeklyBudget = "" + (int)preferencesData.GetValue("weekly_shopping_budget");
                            userPrefs.WeeklyShop = (string)preferencesData.GetValue("does_weekly_shop");
                            userPrefs.ConvenienceStores = (string)preferencesData.GetValue("uses_convenience_stores");
                            userPrefs.OnlineShopping = (string)preferencesData.GetValue("prefers_online_shopping");
                            userPrefs.Plants = (string)preferencesData.GetValue("likes_plants");
                            userPrefs.Alcohol = (string)preferencesData.GetValue("likes_alcohol");


                            foreach (JObject allergen in allergensData.Children<JObject>())
                            {
                                userPrefs.Allergens.Add(new Preference((string)allergen.GetValue("name"), (string)allergen.GetValue("whisk_name"), true));
                            }

                            foreach (JObject diet in dietsData.Children<JObject>())
                            {
                                userPrefs.DietTypes.Add(new Preference((string)diet.GetValue("name"), (string)diet.GetValue("whisk_name"), true));
                            }

                            if (dislikesList != null && dislikesData != null)
                            {
                                foreach (JObject dislike in dislikesList.Children<JObject>())
                                {
                                    userPrefs.Avoids.Add(new Preference((string)dislike.GetValue("name"), (string)dislike.GetValue("canonical_name"), false));
                                }
                            }

                            
                            if (userPrefs.DietTypes.Count == 0)
                            {
                                if (AppSettings.IncludeMeatEater)
                                {
                                    userPrefs.DietTypes.Add(new Preference("No Preference"/*"Meat Eater"*/, "DIET_NONE", true));
                                }
                                else
                                {
                                    userPrefs.DietTypes.Add(new Preference("Vegan", "DIET_VEGAN", true));
                                }
                            }

                            //await GetSubscriptions(user);
                            //AccountType at = await GetCurrentSubscriptionPlan(user);
                            //Console.WriteLine("GOT SUB");
                        }
                    }
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR");
            }
            userPrefs.AccountType = user.Preferences.AccountType;
            return userPrefs;
        }

        public async Task<bool> DeleteSubscriptionPlan(User user, string planType)
        {
            await Task.Delay(10).ConfigureAwait(false);
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                string uri = $"{ApiRootUrl}/chai-plans/" + planType;


                Console.WriteLine(uri);

                var result = HttpClient.DeleteAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {

                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);
                    return true;
                }
                else
                {
                    BadResponseHandler(result, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
            return false;
        }

        public async Task<List<AccountType>> GetSubscriptionPlans(User user)
        {
            List<AccountType> subs = new List<AccountType>();

            await Task.Delay(10).ConfigureAwait(false);
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                string uri = $"{ApiRootUrl}/chai-plans";

                Console.WriteLine(uri);

                var result = HttpClient.GetAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {

                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);

                    //JObject prefsData = (JObject)jResponse.GetValue("data");
                    JArray plans = (JArray)jResponse.GetValue("data");

                    foreach (JObject plan in plans)
                    {
                        subs.Add(new AccountType()); // doesn't matter
                    }
                }
                else
                {
                    BadResponseHandler(result, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
        
            return subs;
        }

        public async Task<AccountType> GetCurrentSubscriptionPlan(User user)
        {
            await Task.Delay(10).ConfigureAwait(false);
            string planName = "";
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                string uri = $"{ApiRootUrl}/chai-plans";

                Console.WriteLine(uri);

                var result = HttpClient.GetAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {

                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);

                    //JObject prefsData = (JObject)jResponse.GetValue("data");
                    JArray plans = (JArray)jResponse.GetValue("data");

                    int planId = 0;

                    foreach (JObject plan in plans)
                    {
                        if ((int)plan.GetValue("id") > planId)
                        {
                            planId = (int)plan.GetValue("id");
                            planName = (string)plan.GetValue("recipe_preference");

                            AccountType subType;
                            switch (planName)
                            {
                                case "flexitarian":
                                    subType = AccountType.ChaiPremiumFlex;
                                    break;
                                case "transitioning":
                                    subType = AccountType.ChaiPremiumTrans;
                                    break;
                                case "vegan":
                                    subType = AccountType.ChaiPremiumVegan;
                                    break;
                                default:
                                    subType = AccountType.ChaiFree;
                                    break;
                            }

                            return subType;
                        }
                    }
                }
                else
                {
                    BadResponseHandler(result, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }

            return AccountType.ChaiFree;

        }

        public async Task<bool> SendAppleTransactionCode(User user, string code, string purchaseDate, string purchaseDateMillis)
        {
            await Task.Delay(10).ConfigureAwait(false);
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                string uri = $"{ApiRootUrl}/apple-transaction-ids";

                Console.WriteLine(user.GetPlanName());

                JObject jObject = new JObject();
                jObject.Add("transaction_id", code);
                //jObject.Add("user_id", "" + AppSession.CurrentUser.Id);
                //jObject.Add("purchase_date", purchaseDate);
                //jObject.Add("purchase_date_millis", purchaseDateMillis);
              
                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);
                Console.WriteLine("JSON DATA : " + jsonString);
                StringContent content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = HttpClient.PostAsync(uri, content).Result;
                if (result.IsSuccessStatusCode)
                {
                    //var json = await result.Content.ReadAsStringAsync();
                    //var jResponse = JObject.Parse(json);
                    return true;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
            return false;
        }

        public async Task<bool> CreateChaiSubscriptionPlan(User user)
        {
            await Task.Delay(10).ConfigureAwait(false);
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                string uri = $"{ApiRootUrl}/chai-plans";

                Console.WriteLine(user.GetPlanName());

                JObject jObject = new JObject();

                string planName = user.GetPlanName();

                jObject.Add("recipe_preference", planName);

                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);
                Console.WriteLine("JSON DATA : " + jsonString);
                StringContent content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = HttpClient.PostAsync(uri, content).Result;
                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);
                    return true;
                }
                else
                {
                    BadResponseHandler(result, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
            return false;
        }

        public async Task<List<string>> GetSubscriptions(User user)
        {
            List<string> subs = new List<string>();

            //await Task.Delay(10).ConfigureAwait(false);
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                string uri = $"{ApiRootUrl}/subscriptions";

                Console.WriteLine(uri);

                var result = HttpClient.GetAsync(uri).Result;
                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);

                    //JObject prefsData = (JObject)jResponse.GetValue("data");

                    JArray subscriptions = (JArray)jResponse.GetValue("data");

                    foreach (JObject sub in subscriptions)
                    {
                        //DateTime expiryTime = DateTime.Parse(sub.GetValue("expiry_time").ToString());
                        string status = sub.GetValue("status").ToString();
                        if (status == "Active")
                        {
                            subs.Add(sub.GetValue("product_id").ToString());
                        }
                    }
                }
                else
                {
                    BadResponseHandler(result, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
            return subs;
        }

        public async Task<bool> UpdateUser(User user)
        {
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + user.AuthToken);

                string uri = ApiRootUrl + "/users";

                JObject jObject = new JObject();

                string bioText = user.Bio;

                jObject.Add("forename", user.FirstName);
                jObject.Add("surname", user.LastName);
                jObject.Add("username", user.EmailAddress); // !!!!!!!!!!!!
                jObject.Add("date_of_birth", user.DateOfBirth.ToString("1990-08-01")); // !!!!!!!!!!!!
                jObject.Add("gender", "Prefer not to share"); // !!!!!!!!!!!!
                jObject.Add("email", user.EmailAddress);

                jObject.Add("country", "GB"); // !!!!!!!!!!!!

                jObject.Add("bio", bioText);

                try
                {
                    await UploadProfilePhoto(user, user.AvatarImageUrl);
                }
                catch (Exception e) { }

                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);
                Console.WriteLine("JSON DATA : " + jsonString);
                StringContent content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = HttpClient.PutAsync(uri, content).Result;

                if (!result.IsSuccessStatusCode)
                {
                    BadResponseHandler(result);
                    return false;
                }

                var json = await result.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                if (!ApiConnectionAvailable())
                {
                    App.ShowAlert("There was an error connecting to the server.");
                }

                Console.WriteLine("CONNECTION ERROR");
                throw e;
            }

            return true;
        }

        public async Task<bool> UpdatePreferences(User user, bool updatePrefs)
        {

            await UpdateUser(user);

            if (updatePrefs)
            {
                List<int> allergens = new List<int>();
                List<int> diets = new List<int>();

                List<string> dislikes = new List<string>();

                //foreach (Preference pref in user.Preferences.Avoids)
                //{
                //    exclude_ingredients.Add(pref.Name);
                //}

                foreach (Preference pref in user.Preferences.Allergens)
                {
                    Preference p = AppDataContent.Allergens.Find(x => x.Name == pref.Name);// != null)
                    if (p != null)
                    {
                        allergens.Add(p.Id);
                    }
                }

                bool meatEater = false;

                foreach (Preference pref in user.Preferences.DietTypes)
                {
                    Preference p = AppDataContent.DietTypes.Find(x => x.Name == pref.Name);// != null)
                    if (p != null)
                    {
                        if (p.WhiskName != "DIET_NONE") // ignore this
                        {
                            diets.Add(p.Id);
                        }
                        else
                        {
                            meatEater = true;
                            //diets.Add(1);// temp set to something valid to make sure the other prefs update
                        }
                    }
                }



                foreach (Preference pref in user.Preferences.Avoids)
                {
                    dislikes.Add(pref.Name);
                }

                try
                {
                    HttpClient = new HttpClient();
                    HttpClient.DefaultRequestHeaders.Clear();
                    HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + user.AuthToken);

                    string uri = ApiRootUrl + "/preferences";

                    JObject jObject = new JObject();

                    int houseSize = 4;
                    int children = 0;
                    int weeklyBudget = 100;
                    string doesWeeklyShop = "agree";
                    string likesPlants = "agree";
                    string likesAlcohol = "agree";
                    string prefersOnlineShopping = "agree";
                    string usesConvenienceStores = "agree";
                    //string bioText = user.Bio;

                    //if (!meatEater)
                    //{
                        jObject.Add("diet_ids", JArray.FromObject(diets));
                    //}
                    jObject.Add("allergen_ids", JArray.FromObject(allergens));
                    jObject.Add("disliked_ingredients", JArray.FromObject(dislikes));
                    jObject.Add("household_adults", houseSize);
                    jObject.Add("weekly_shopping_budget", weeklyBudget);
                    jObject.Add("household_children", children);

                    jObject.Add("does_weekly_shop", doesWeeklyShop);// user.Preferences.WeeklyShop);
                    jObject.Add("likes_plants", likesPlants);// user.Preferences.Plants);
                    jObject.Add("likes_alcohol", likesAlcohol);// user.Preferences.Alcohol);
                    jObject.Add("prefers_online_shopping", prefersOnlineShopping);// user.Preferences.OnlineShopping);
                    jObject.Add("uses_convenience_stores", usesConvenienceStores);// user.Preferences.ConvenienceStores);
                    //jObject.Add("bio", "whatever");

                    string jsonString = jObject.ToString();
                    jsonString = TextTools.CleanUpJson(jsonString);
                    Console.WriteLine("JSON DATA : " + jsonString);
                    StringContent content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var result = HttpClient.PutAsync(uri, content).Result;

                    if (!result.IsSuccessStatusCode)
                    {
                        BadResponseHandler(result);
                        return false;
                    }

                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);


                    if (meatEater) // re-assign meat eater status
                    {
                        //AppSession.CurrentUser.Preferences.DietTypes.Clear();
                        //AppSession.CurrentUser.Preferences.DietTypes.Add(new Preference("Meat Eater", "DIET_NONE", true));
                    }

                }
                catch (Exception e)
                {
                    if (!ApiConnectionAvailable())
                    {
                        App.ShowAlert("There was an error connecting to the server.");
                    }

                    Console.WriteLine("CONNECTION ERROR");
                    throw e;
                }

            }

            return true;
        }

        public static async Task<string> UploadProfilePhoto(User user, string photoPath)
        {
            var tcs = new TaskCompletionSource<string>();
            await Task.Run(async () =>
            {
                try
                {
                    if (!ApiConnectionAvailable())
                    {
                        App.ShowAlert("There is an error connecting to the internet");
                        tcs.TrySetResult(null);
                    }
                    HttpClient = new HttpClient();
                    HttpClient.DefaultRequestHeaders.Clear();
                    HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + user.AuthToken);

                    var photoFile = new FileResult(photoPath);
                    var stream = await photoFile.OpenReadAsync();

                    var bytes = new byte[stream.Length];

                    //check if image is too big
                    double ImageSizeInMB = ((double)(bytes.Length) / (1024 * 1024));
                    //check if image size is bigger than 5 MB
                    if (ImageSizeInMB > 5)
                    {

                    }

                    await stream.ReadAsync(bytes, 0, (int)stream.Length);
                    string base64 = System.Convert.ToBase64String(bytes);

                    Console.WriteLine("Got image data");

                    string uri = ApiRootUrl + "/user/profile-pictures";

                    List<KeyValuePair<string, string>> dataPairs = new List<KeyValuePair<string, string>>();

                    dataPairs.Add(new KeyValuePair<string, string>("file", "data:image/jpeg;base64," + base64));

                    using (var content = new MultipartFormDataContent())
                    {
                        foreach (var keyValuePair in dataPairs)
                        {
                            content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                        }

                        var result = HttpClient.PostAsync(uri, content).GetAwaiter().GetResult();

                        if (result.IsSuccessStatusCode)
                        {
                            var json = await result.Content.ReadAsStringAsync();
                            var jResponse = JObject.Parse(json);
                            JObject data = (JObject)jResponse.GetValue("data");
                            string imageUrl = (string)data.GetValue("image");
                            tcs.TrySetResult(imageUrl);
                        }
                        else
                        {
                            //as this is a static method we need to create an object reference
                            ApiBridge b = (ApiBridge)App.ApiBridge;
                            b.BadResponseHandler(result);
                        }
                    }
                }
                catch (Exception e)
                {
                    if (!ApiConnectionAvailable())
                    {
                        App.ShowAlert("There was an error connecting to the server.");
                    }

                    Console.WriteLine("CONNECTION ERROR");
                    throw e;
                }
            });
            return await tcs.Task;
        }


        public async Task<List<Recipe>> GetWasteLessRecipes(User user)
        {
            return SearchRecipes(user, true, false, true, false).Result;
        }

        public async Task<List<Recipe>> GetRecommendedRecipes(User user)
        {
            return SearchRecipes(user, true, false, false, false/*, "/recipe-feed"*/).Result;
        }

        public async Task<List<Recipe>> GetRecommendedVeganRecipes(User user)
        {
            return SearchRecipes(user, true, false, false, true).Result;
        }



        public async Task<List<Recipe>> SearchRecipes(User user, bool usePrefs, bool useKeyWords, bool useIncludes, bool forceVegan, string route = "/recipe-search")
        {
            int succeeded = 0;
            int failed = 0;
            List<Recipe> matchedRecipes = new List<Recipe>();

            await Task.Delay(10).ConfigureAwait(false);

            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + user.AuthToken);
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

            List<string> allergens = new List<string>();
            List<string> diets = new List<string>();
            List<string> exclude_ingredients = new List<string>();
            List<string> include_ingredients = new List<string>();

            string keywords = "";

            foreach (string keyword in AppSession.SearchKeywords)
            {
                keywords += keyword + " ";
            }

            foreach (Ingredient ingredient in AppDataContent.AvailableIngredients)
            {
                include_ingredients.Add(ingredient.Name);
            }

            foreach (Preference pref in user.Preferences.Avoids)
            {
                exclude_ingredients.Add(pref.Name);
            }

            foreach (Preference pref in user.Preferences.Allergens)
            {
                Preference p = AppDataContent.Allergens.Find(x => x.Name == pref.Name);// != null)
                if (p != null)
                {
                    allergens.Add(p.WhiskName);
                }
            }

            foreach (Preference pref in user.Preferences.DietTypes)
            {
                Preference p = AppDataContent.DietTypes.Find(x => x.Name == pref.Name);// != null)

                if (p != null)
                {
                    if (p.WhiskName != "DIET_NONE") // ignore this
                    {
                        diets.Add(p.WhiskName);
                    }
                }
            }

            if (forceVegan || user.IsInfluencer())
            {
                diets.Add("DIET_VEGAN");
            }

            try
            {
                string uri = ApiRootUrl + route;

                JObject jObject = new JObject();

                if (usePrefs)
                {
                    if (allergens.Count > 0)
                    {
                        jObject.Add("allergens", JToken.FromObject(allergens));
                    }

                    if (diets.Count > 0)
                    {
                        jObject.Add("diets", JToken.FromObject(diets));
                    }

                    if (exclude_ingredients.Count > 0)
                    {
                        jObject.Add("exclude_ingredients", JToken.FromObject(exclude_ingredients));
                    }
                }

                if (useIncludes)
                {
                    if (include_ingredients.Count > 0)
                    {
                        jObject.Add("include_ingredients", JToken.FromObject(include_ingredients));
                    }
                }

                if (useKeyWords)
                {
                    if (AppSession.SearchKeywords.Count > 0)
                    {
                        jObject.Add("keywords", keywords);
                    }
                }

                JObject pagingObj = new JObject();

                JObject cursorsObj = new JObject();

                if (AppSession.GetNextPage)
                {
                    if (AppSession.AfterCursor != null)
                    {
                        cursorsObj.Add("after", AppSession.AfterCursor);
                        pagingObj.Add("cursors", cursorsObj);
                    }
                }
                else if (AppSession.GetLastPage)
                {
                    if (AppSession.BeforeCursor != null)
                    {
                        cursorsObj.Add("before", AppSession.BeforeCursor);
                        pagingObj.Add("cursors", cursorsObj);
                    }
                }

                pagingObj.Add("limit", ITEMS_PER_REQUEST);

                jObject.Add("paging", pagingObj);


                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);


                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = HttpClient.PostAsync(uri, content).Result;

                if (result.IsSuccessStatusCode)
                {

                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);

                    Console.WriteLine(json);

                    JArray recipeData = (JArray)jResponse.GetValue("data");
                    string error = (string)jResponse.GetValue("error");

                    AppSession.TotalRecipes = 0;

                    try
                    {
                        JObject metaData = (JObject)jResponse.GetValue("meta");
                        JObject pagingData = (JObject)metaData.GetValue("paging");
                        int totalData = (int)pagingData.GetValue("total");

                        if (totalData > 0)
                        {
                            AppSession.TotalRecipes = totalData;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error");
                    }

                    try
                    {
                        JObject metaData = (JObject)jResponse.GetValue("meta");
                        JObject pagingData = (JObject)metaData.GetValue("paging");
                        JObject cursorData = (JObject)pagingData.GetValue("cursors");

                        string afterKey = (string)cursorData.GetValue("after");
                        string beforeKey = (string)cursorData.GetValue("before");


                        if (afterKey != null)
                        {
                            AppSession.AfterCursor = afterKey;
                        }

                        if (beforeKey != null)
                        {
                            AppSession.BeforeCursor = beforeKey;
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error");
                    }

                    foreach (JObject recipe in recipeData.Children<JObject>())
                    {

                        try
                        {
                            Recipe singleRecipe = JsonConvert.DeserializeObject<Recipe>(recipe.ToString());


                            JObject chaiData = (JObject)recipe.GetValue("chai");


                            singleRecipe.MainImageSource = "cheese.jpg";
                            singleRecipe.Creator = new Models.User();

                            try
                            {
                                singleRecipe.Author = (string)chaiData.GetValue("author");
                            }
                            catch (Exception e)
                            {
                                singleRecipe.Author = "Chai Cooking";
                            }

                            if (AppSettings.UsePlaceHolderAuthor)
                            {
                                singleRecipe.Author = "Chai Cooking";
                            }

                            singleRecipe.IsFavourite = false;
                            singleRecipe.IsSelected = false;

                            singleRecipe.StarRating = 4;
                            singleRecipe.FavouriteRating = 3;

                            singleRecipe.MealType = "Dinner";

                            try
                            {
                                singleRecipe.MealType = singleRecipe.Labels.MealType[0].Name;
                            }
                            catch (Exception ee)
                            {

                            }

                            singleRecipe.DishType = singleRecipe.MealType;

                            singleRecipe.PrepTime = "10";
                            singleRecipe.CookingTime = "15";

                            if (chaiData != null)
                            {
                                singleRecipe.PrepTime = "" + chaiData.GetValue("prep_time");
                                singleRecipe.CookingTime = "" + chaiData.GetValue("cook_time");
                                singleRecipe.TotalTime = "" + chaiData.GetValue("total_time");

                                singleRecipe.DishType = "" + chaiData.GetValue("dish_type");
                                singleRecipe.MealType = "" + chaiData.GetValue("meal_type");
                                singleRecipe.CuisineType = "" + chaiData.GetValue("cuisine_type");
                            }


                            matchedRecipes.Add(singleRecipe);
                            succeeded++;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("recipe ERROR" + ex.StackTrace);
                            failed++;
                        }
                    }

                    if (error != null)
                    {
                        //App.ShowAlert("ERROR", error);
                    }
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR" + e.StackTrace);
            }

            if (matchedRecipes.Count == 0)
            {
                //App.ShowAlert("ERROR", "No recipes match your preferences and search parameters.");
            }

            //AppSession.TotalRecipes = matchedRecipes.Count;
            Console.WriteLine("SUCCEEDED: " + succeeded);
            Console.WriteLine("FAILED: " + failed);
            return matchedRecipes;
        }

        public async Task<Recipe> GetRecipe(string id)
        {
            Recipe recipe = new Recipe();
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                string uri = ApiRootUrl + "/recipes/" + id;

                Console.WriteLine(uri);

                var result = HttpClient.GetAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {

                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);

                    JObject recipeData = (JObject)jResponse.GetValue("data");
                    JObject rData = (JObject)recipeData.GetValue("recipe");

                    Console.WriteLine(json.ToString());

                    Recipe singleRecipe = new Recipe();
                    singleRecipe.Id = (string)rData.GetValue("id");
                    singleRecipe.Name = (string)rData.GetValue("name");

                    singleRecipe.Instructions = JsonConvert.DeserializeObject<Instructions>(rData.GetValue("instructions").ToString());
                    singleRecipe.Ingredients = JsonConvert.DeserializeObject<Ingredient[]>(rData.GetValue("ingredients").ToString());

                    singleRecipe.Images = new Image[1];

                    // fudge in a default image
                    singleRecipe.Images[0] = new Image
                    {
                        Responsive = new Responsive
                        {
                            Width = 80,
                            Height = 80,
                            Url = new Uri("https://chai.cooking/wp-content/uploads/2020/08/logo.png")

                        },
                        Url = new Uri("https://chai.cooking/wp-content/uploads/2020/08/logo.png")
                    };


                    try
                    {
                        singleRecipe.Images = JsonConvert.DeserializeObject<Image[]>(rData.GetValue("images").ToString());
                    }
                    catch (Exception e1)
                    {

                    }

                    Console.WriteLine(json.ToString());
                    recipe = singleRecipe;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR\n\n" + e.StackTrace);
            }
            return recipe;
        }


        public async Task<bool> CreateUser(User user)
        {
            //await Task.Delay(1000).ConfigureAwait(false);
            User createdUser = null;
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                string uri = ApiRootUrl + "/users";

                JObject jObject = new JObject();

                user.Password = Helpers.Custom.Accounts.GenerateChaiIdPassword(user);

                jObject.Add("forename", user.FirstName);
                jObject.Add("surname", user.LastName);
                jObject.Add("username", user.EmailAddress); // !!!!!!!!!!!!
                jObject.Add("date_of_birth", user.DateOfBirth.ToString("1990-08-01")); // !!!!!!!!!!!!
                jObject.Add("gender", "Prefer not to share"); // !!!!!!!!!!!!
                jObject.Add("email", user.EmailAddress);
                jObject.Add("password", user.Password);
                jObject.Add("password_confirmation", user.Password);
                jObject.Add("country", "GB"); // !!!!!!!!!!!!
                jObject.Add("whisk_token_id", AppSession.CurrentUser.WhiskTokenId.ToString());
                //jObject.Add("whisk_token_id", AppSession.CurrentUser.AuthToken);



                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);

                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = HttpClient.PostAsync(uri, content).Result;

                if (result.IsSuccessStatusCode)
                {

                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);

                    JObject data = (JObject)jResponse.GetValue("data");
                    string ApiToken = (string)data.GetValue("api_token");

                    //AppSession.CurrentUser.ApiToken = ApiToken;
                    //createdUser.ApiToken = ApiToken;

                    AppSession.CurrentUser.AuthToken = ApiToken;
                    //createdUser.AuthToken = ApiToken;

                    Console.WriteLine(json);

                    LocalDataStore.SaveAll();


                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        App.ShowAlert("Account created."); 
                    });
                }
                else
                {
                    BadResponseHandler(result);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR" + e);
                return false;
            }

            try
            {
                bool logged = await LoginUser(user);
            }
            catch(Exception ef)
            {

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

        public async Task<bool> ForgotPassword(string email)
        {
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                string uri = $"{ApiRootUrl}/forgot-password";

                JObject jObject = new JObject();

                jObject.Add("email", email);

                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);

                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = HttpClient.PostAsync(uri, content).Result;

                if (!result.IsSuccessStatusCode)
                {
                    BadResponseHandler(result);
                }

                var json = await result.Content.ReadAsStringAsync();
                //var jResponse = JObject.Parse(json);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<ColourTileModel> ListAlbumColours()
        {
            ColourTileModel colourTileModel = new ColourTileModel();
            colourTileModel.colourList = new List<TileColour>();

            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                string uri = $"{ApiRootUrl}/album-colours";

                var result = HttpClient.GetAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {

                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);
                    JArray jArray = (JArray)jResponse.GetValue("hex_colour_codes");

                    foreach (string j in jArray)
                    {
                        TileColour tileColour = new TileColour();
                        tileColour.colour = j.ToString();
                        colourTileModel.colourList.Add(tileColour);
                    }

                    Console.WriteLine(json.ToString());
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"CONNECTION ERROR: {e}");
            }
            return colourTileModel;
        }

        public async Task<List<Album>> ListAlbums(User user)
        {
            await Task.Delay(10).ConfigureAwait(false);
            List<Album> albums = new List<Album>();
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);

                Uri uri = new Uri(string.Format($"{ApiRootUrl}/albums", string.Empty));
                var result = HttpClient.GetAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {

                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);

                    JArray albumData = (JArray)jResponse.GetValue("data");

                    albums = albumData.ToObject<List<Album>>();

                    Console.WriteLine("Obtained Albums.");

                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
            return albums;
        }

        public async Task<Album> CreateAlbum(User user, string name, string color)
        {
            await Task.Delay(10).ConfigureAwait(false);
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                Uri uri = new Uri(string.Format($"{ApiRootUrl}/albums", string.Empty));
                JObject jObject = new JObject();

                jObject.Add("colour", color);
                jObject.Add("name", name);
                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = HttpClient.PostAsync(uri, content).Result;

                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);

                    JObject data = (JObject)jResponse.GetValue("data");
                    string id = (string)data.GetValue("id");
                    string colour = (string)data.GetValue("colour");
                    string albumName = (string)data.GetValue("name");
                    Album returnedAlbum = new Album()
                    {
                        Id = id,
                        Name = albumName,
                        FolderColor = colour
                    };
                    Console.WriteLine("Successfully created album.");
                    return returnedAlbum;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
            return null;
        }

        public async Task<Album> ViewAlbum(User user, string albumID)
        {
            await Task.Delay(10).ConfigureAwait(false);
            Album album = new Album();
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                Uri uri = new Uri(string.Format($"{ApiRootUrl}/albums/{albumID}", string.Empty));
                var result = HttpClient.GetAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);

                    JObject data = (JObject)jResponse.GetValue("data");
                    album.Id = data.GetValue("id").ToString();
                    album.Name = data.GetValue("name").ToString();
                    album.FolderColor = data.GetValue("colour").ToString();
                    JArray recipes = (JArray)data.GetValue("recipes");
                    List<Recipe> recipeList = new List<Recipe>();
                    album.Recipes = recipeList;
                    foreach (JObject r in recipes.Children<JObject>())
                    {
                        Recipe recipe = new Recipe();
                        try
                        {
                            Chai chai = new Chai();
                            recipe.Id = r.GetValue("id").ToString();
                            recipe.Name = r.GetValue("name").ToString();
                            recipe.chai = r.GetValue("chai").ToObject<Chai>();
                            recipe.Ingredients = r.GetValue("ingredients").ToObject<Ingredient[]>();
                            recipe.Images = r.GetValue("images").ToObject<Image[]>();
                            recipe.Source = r.GetValue("source").ToObject<Models.Custom.Source>();
                            recipe.Servings = (long)r.GetValue("servings");
                            User creator = new User();
                            recipe.Creator = creator;
                            try
                            {
                                recipe.Creator = r.GetValue("author").ToObject<User>();
                            }
                            catch
                            {
                                recipe.Creator = creator;
                            }
                            recipe.IsFavourite = true;
                            try
                            {
                                recipe.Durations = r.GetValue("durations").ToObject<Models.Custom.Durations>();
                            }
                            catch
                            {
                                recipe.Durations = new Durations();
                                recipe.Durations.PrepTime = 0;
                                recipe.Durations.CookTime = 0;
                                recipe.Durations.TotalTime = 0;
                            }

                            if (recipe.Durations.PrepTime > 0)
                            {
                                recipe.PrepTime = "" + recipe.Durations.PrepTime;
                            }

                            if (recipe.Durations.CookTime > 0)
                            {
                                recipe.CookingTime = "" + recipe.Durations.CookTime;
                            }

                            if (recipe.Durations.TotalTime > 0)
                            {
                                recipe.TotalTime = "" + recipe.Durations.TotalTime;
                            }

                            recipe.Labels = r.GetValue("labels").ToObject<Labels>();
                            recipe.Language = (string)r.GetValue("langauge");
                            try
                            {
                                //var a = recipeArray.GetValue("constraints").ToObject<Constraints>();
                                recipe.Constraints = r.GetValue("constraints").ToObject<Constraints>();
                                //meal.Recipe.Constraints = new Constraints();
                            }
                            catch
                            {
                                recipe.Constraints = new Constraints();
                                Console.WriteLine();
                            }

                            Console.WriteLine();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        album.Recipes.Add(recipe);
                    }

                    Console.WriteLine("Obtained Albums.");

                    return album;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
            return album;
        }

        public async Task<bool> EditAlbum(User user, string albumID, string name, string color)
        {
            await Task.Delay(10).ConfigureAwait(false);
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                Uri uri = new Uri(string.Format($"{ApiRootUrl}/albums/{albumID}", string.Empty));
                JObject jObject = new JObject();

                jObject.Add("colour", color);
                jObject.Add("name", name);
                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = HttpClient.PutAsync(uri, content).Result;

                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);

                    JObject data = (JObject)jResponse.GetValue("data");
                    Console.WriteLine("Successfully created album.");
                    return true;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
            return false;
        }

        public async Task<bool> AddRecipeToAlbum(User user, Recipe recipe, Album album)
        {
            await Task.Delay(10).ConfigureAwait(false);
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                Uri uri = new Uri(string.Format($"{ApiRootUrl}/albums/{album.Id}/{recipe.Id}", string.Empty));
                var result = HttpClient.PostAsync(uri, null).Result;

                if (result.IsSuccessStatusCode)
                {
                    Console.WriteLine("Successfully created album.");
                    return true;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
            return false;
        }

        public async Task<bool> RemoveAlbum(User user, string albumID)
        {
            await Task.Delay(10).ConfigureAwait(false);
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);

                Uri uri = new Uri(string.Format($"{ApiRootUrl}/albums/{albumID}", string.Empty));
                var result = HttpClient.DeleteAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {
                    Console.WriteLine("Successfully deleted album.");
                    return true;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
            return false;
        }

        public async Task<bool> RemoveRecipeFromAlbum(User user, string albumID, string recipeId)
        {
            await Task.Delay(10).ConfigureAwait(false);
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);

                Uri uri = new Uri(string.Format($"{ApiRootUrl}/albums/{albumID}/{recipeId}", string.Empty));
                var result = HttpClient.DeleteAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {
                    Console.WriteLine("Successfully removed recipe.");
                    //Clears the selected recipies so the removed recipes are not selected after removing them.
                    AppSession.favouritesCollectionView.SelectedItems.Clear();
                    return true;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
            return false;
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

        public async Task<List<IngredientUnit>> GetUnits()
        {
            List<IngredientUnit> units = new List<IngredientUnit>();

            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                string uri = ApiRootUrl + "/units";

                var result = HttpClient.GetAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {

                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);

                    JArray unitsData = (JArray)jResponse.GetValue("data");

                    units = unitsData.ToObject<List<IngredientUnit>>();

                    Console.WriteLine(json.ToString());
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR");
            }
            return units;
        }

        public async Task<List<Preference>> GetAllergens()
        {
            List<Preference> allergens = new List<Preference>();

            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                string uri = ApiRootUrl + "/allergens";

                var result = HttpClient.GetAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {

                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);

                    JArray allergensData = (JArray)jResponse.GetValue("data");

                    allergens = allergensData.ToObject<List<Preference>>();

                    Console.WriteLine(json.ToString());
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR");
            }
            return allergens;
        }

        public async Task<List<Preference>> GetDiets()
        {
            List<Preference> diets = new List<Preference>();

            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                string uri = ApiRootUrl + "/diets";

                var result = HttpClient.GetAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {

                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);

                    JArray dietsData = (JArray)jResponse.GetValue("data");


                    diets = dietsData.ToObject<List<Preference>>();


                    // if we include meat eater
                    if (AppSettings.IncludeMeatEater)
                    {
                        diets.Add(new Preference
                        {
                            Id = 0,
                            Name = "No Preference",//Meat Eater",
                            WhiskName = "DIET_NONE",
                            IsSelected = false
                        });
                    }

                    Console.WriteLine(json.ToString());
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR");
            }
            return diets;
        }

        public async Task<RecipeFeed> GetRecipes(User user)
        {
            RecipeFeed recipes = new RecipeFeed();

            await Task.Delay(1000).ConfigureAwait(false);

            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);

            try
            {
                string uri = ApiRootUrl + "/recipe-feed";

                var result = HttpClient.PostAsync(uri, null).Result;

                if (!result.IsSuccessStatusCode)
                {
                    App.ShowAlert("API ERROR : " + uri);
                }

                var json = await result.Content.ReadAsStringAsync();
                var jResponse = JObject.Parse(json);

                if (result.IsSuccessStatusCode)
                {
                    recipes = JsonConvert.DeserializeObject<RecipeFeed>(json);
                    JArray recipeData = (JArray)jResponse.GetValue("data");
                    return recipes;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occurred: {e}");
            }
            return null;
        }

        public async Task<List<Recipe>> GetFavouriteRecipes(User user)
        {
            List<Recipe> favourites = new List<Recipe>();
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + user.AuthToken);
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                string uri = ApiRootUrl + "/favourites";

                var result = HttpClient.GetAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {

                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);
                    Console.WriteLine(json.ToString());

                    JArray recipeData = (JArray)jResponse.GetValue("data");
                    string error = (string)jResponse.GetValue("error");
                    foreach (JObject r in recipeData.Children<JObject>())
                    {
                        Recipe recipe = new Recipe();
                        try
                        {
                            Chai chai = new Chai();
                            recipe.Id = r.GetValue("id").ToString();
                            recipe.Name = r.GetValue("name").ToString();
                            recipe.chai = r.GetValue("chai").ToObject<Chai>();
                            recipe.Ingredients = r.GetValue("ingredients").ToObject<Ingredient[]>();
                            recipe.Images = r.GetValue("images").ToObject<Image[]>();
                            recipe.Source = r.GetValue("source").ToObject<Models.Custom.Source>();
                            recipe.Servings = (long)r.GetValue("servings");
                            User creator = new User();
                            recipe.Creator = creator;
                            try
                            {
                                recipe.Creator = r.GetValue("author").ToObject<User>();
                            }
                            catch
                            {
                                recipe.Creator = creator;
                            }
                            recipe.IsFavourite = true;
                            try
                            {
                                recipe.Durations = r.GetValue("durations").ToObject<Models.Custom.Durations>();
                            }
                            catch
                            {
                                recipe.Durations = new Durations();
                                recipe.Durations.PrepTime = 0;
                                recipe.Durations.CookTime = 0;
                                recipe.Durations.TotalTime = 0;
                            }

                            if (recipe.Durations.PrepTime > 0)
                            {
                                recipe.PrepTime = "" + recipe.Durations.PrepTime;
                            }
                            if (recipe.Durations.CookTime > 0)
                            {
                                recipe.CookingTime = "" + recipe.Durations.CookTime;
                            }

                            if (recipe.Durations.TotalTime > 0)
                            {
                                recipe.TotalTime = "" + recipe.Durations.TotalTime;
                            }

                            recipe.Labels = r.GetValue("labels").ToObject<Labels>();
                            recipe.Language = (string)r.GetValue("langauge");
                            try
                            {
                                //var a = recipeArray.GetValue("constraints").ToObject<Constraints>();
                                recipe.Constraints = r.GetValue("constraints").ToObject<Constraints>();
                                //meal.Recipe.Constraints = new Constraints();
                            }
                            catch
                            {
                                recipe.Constraints = new Constraints();
                                Console.WriteLine();
                            }

                            Console.WriteLine();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        favourites.Add(recipe);
                    }
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR");
            }
            return favourites;
        }

        public async Task<bool> AddFavourite(User user, string id)
        {
            await Task.Delay(10).ConfigureAwait(false);
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + user.AuthToken);
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                string uri = ApiRootUrl + "/favourites/" + id;

                StringContent content = new StringContent("", Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = HttpClient.PostAsync(uri, content).Result;

                Console.WriteLine(result.StatusCode);

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR");
            }
            return false;
        }

        public async Task<bool> RemoveFavourite(User user, string id)
        {
            await Task.Delay(10).ConfigureAwait(false);

            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + user.AuthToken);
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                string uri = ApiRootUrl + "/favourites/" + id;

                StringContent content = new StringContent("", Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = HttpClient.DeleteAsync(uri).Result;
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: {e}");
            }
            return false;
        }

        public async Task<InfluencerMealPlans> GetInfluencerMealPlans(User user)
        {
            await Task.Delay(10).ConfigureAwait(false);
            InfluencerMealPlans influencerMealPlans = new InfluencerMealPlans();

            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + user.AuthToken);
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                Uri uri = new Uri(string.Format(ApiRootUrl + "/influencers/" + StaticData.selectedInfluencerId + "/meal-plan-templates", string.Empty));
                HttpResponseMessage response = await HttpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<InfluencerMealPlans>(content);
                    influencerMealPlans = json;
                }
                else
                {
                    BadResponseHandler(response, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR: " + e);
            }


            return influencerMealPlans;
        }

        public async Task<InfluencerMealPlans> BrowseMealPlans(User user)
        {
            await Task.Delay(10).ConfigureAwait(false);
            InfluencerMealPlans influencerMealPlans = new InfluencerMealPlans();

            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + user.AuthToken);
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                Uri uri = new Uri(string.Format(ApiRootUrl + "/meal-plan-templates", string.Empty));
                HttpResponseMessage response = await HttpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<InfluencerMealPlans>(content);
                    influencerMealPlans = json;
                }
                else
                {
                    BadResponseHandler(response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR: " + e);
            }


            return influencerMealPlans;
        }

        // TODO Method does not work, needs adjusting to be similar to GetWeek
        public async Task<UserMealTemplate> GetCalendar(User user, string date, CancellationToken cancellationToken)
        {
            await Task.Delay(100).ConfigureAwait(false);
            UserMealTemplate userMealTemplate = new UserMealTemplate();
            userMealTemplate.Data = new List<UserMealTemplate.Datum>();

            userMealTemplate.Data.Clear();

            if (cancellationToken.IsCancellationRequested)
            {
                return AppSession.calendar;
            }
            else
            {
                try
                {
                    HttpClient = new HttpClient();
                    HttpClient.DefaultRequestHeaders.Clear();
                    HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + user.AuthToken);
                    HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                    string uri = ApiRootUrl + "/calendar/week/" + date;

                    var response = HttpClient.GetAsync(uri).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();

                        var jResponse = JObject.Parse(json);

                        JArray data = (JArray)jResponse.GetValue("data");

                        foreach (JObject item in data)
                        {
                            UserMealTemplate.Datum datum = new UserMealTemplate.Datum();
                            List<Meal> mealsList = new List<Meal>();
                            datum.Meals = mealsList;
                            string datumDate = (string)item.GetValue("date");
                            datum.date = datumDate;
                            JArray meals = (JArray)item.GetValue("meals");
                            bool error = (bool)item.GetValue("error");
                            if (!error)
                            {
                                foreach (JObject mealItem in meals)
                                {
                                    Meal meal = new Meal();
                                    meal.Id = (int)mealItem.GetValue("id");
                                    meal.MealType = (string)mealItem.GetValue("meal_type");

                                    Recipe recipe = new Recipe();
                                    meal.Recipe = recipe;
                                    Chai chai = new Chai();
                                    try
                                    {
                                        JObject recipeArray = (JObject)mealItem.GetValue("recipe");
                                        meal.Recipe.Id = recipeArray.GetValue("id").ToString();
                                        meal.Recipe.Name = recipeArray.GetValue("name").ToString();
                                        meal.Recipe.Ingredients = recipeArray.GetValue("ingredients").ToObject<Ingredient[]>();
                                        meal.Recipe.Images = recipeArray.GetValue("images").ToObject<Image[]>();
                                        meal.Recipe.Source = recipeArray.GetValue("source").ToObject<Models.Custom.Source>();
                                        meal.Recipe.Servings = (long)recipeArray.GetValue("servings");
                                        try
                                        {
                                            meal.Recipe.Durations = recipeArray.GetValue("durations").ToObject<Models.Custom.Durations>();
                                        }
                                        catch
                                        {
                                            meal.Recipe.Durations = new Durations();
                                            meal.Recipe.Durations.PrepTime = 0;
                                            meal.Recipe.Durations.CookTime = 0;
                                            meal.Recipe.Durations.TotalTime = 0;
                                        }

                                        if (recipe.Durations.PrepTime > 0)
                                        {
                                            recipe.PrepTime = "" + recipe.Durations.PrepTime;
                                        }
                                        if (recipe.Durations.CookTime > 0)
                                        {
                                            recipe.CookingTime = "" + recipe.Durations.CookTime;
                                        }

                                        if (recipe.Durations.TotalTime > 0)
                                        {
                                            recipe.TotalTime = "" + recipe.Durations.TotalTime;
                                        }

                                        meal.Recipe.Labels = recipeArray.GetValue("labels").ToObject<Labels>();
                                        meal.Recipe.Language = (string)recipeArray.GetValue("langauge");
                                        try
                                        {
                                            //var a = recipeArray.GetValue("constraints").ToObject<Constraints>();
                                            meal.Recipe.Constraints = recipeArray.GetValue("constraints").ToObject<Constraints>();
                                            //meal.Recipe.Constraints = new Constraints();
                                        }
                                        catch
                                        {
                                            meal.Recipe.Constraints = new Constraints();
                                            Console.WriteLine();
                                        }

                                        try
                                        {
                                            meal.Recipe.chai = recipeArray.GetValue("chai").ToObject<Chai>();
                                        }
                                        catch
                                        {
                                            meal.Recipe.chai = chai;
                                        }

                                        Console.WriteLine();
                                    }
                                    catch (Exception e)
                                    {
                                        JObject recipeArray = (JObject)mealItem.GetValue("recipe");
                                        Chai chaiObj = recipeArray.GetValue("chai").ToObject<Chai>();
                                        meal.Recipe.chai = chaiObj;
                                        Console.WriteLine(e);
                                    }
                                    Console.WriteLine();
                                    datum.Meals.Add(meal);
                                }
                            }
                            userMealTemplate.Data.Add(datum);
                            Console.WriteLine();
                        }
                        Console.WriteLine();
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        UnauthorizedCheck(response);
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        var parsedDate = DateTime.Parse(date, CultureInfo.CurrentCulture);
                        DateTime monday = parsedDate.AddDays(-(int)parsedDate.DayOfWeek + (int)DayOfWeek.Monday);
                        for (int i = 0; i < 7; i++)
                        {
                            UserMealTemplate.Datum datum = new UserMealTemplate.Datum();
                            string datumDate = monday.ToString("yyyy-MM-dd");
                            datum.date = datumDate;
                            monday = monday.AddDays(1);
                            userMealTemplate.Data.Add(datum);
                        }
                        return userMealTemplate;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("CONNECTION ERROR: " + e);
                }

                return userMealTemplate;
            }
        }

        public async Task<UserMealTemplate> GetWeek(User user, string mealPlanID, string week, CancellationToken cancellationToken)
        {

            await Task.Delay(10).ConfigureAwait(false);
            UserMealTemplate userMealTemplate = new UserMealTemplate();
            userMealTemplate.Data = new List<UserMealTemplate.Datum>();

            userMealTemplate.Data.Clear();
            if (cancellationToken.IsCancellationRequested)
            {
                return userMealTemplate;
            }
            else
            {
                try
                {
                    HttpClient = new HttpClient();
                    HttpClient.DefaultRequestHeaders.Clear();
                    HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);

                    Console.WriteLine("Start : "+ DateTime.UtcNow);

                    Uri uri = new Uri(string.Format(ApiRootUrl + "/meal-plans/" + mealPlanID + "/weeks/" + week, string.Empty));
                    HttpResponseMessage response = await HttpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();

                        var jResponse = JObject.Parse(json);

                        JArray data = (JArray)jResponse.GetValue("data");
                        int i = 0;
                        foreach (JObject item in data)
                        {
                            UserMealTemplate.Datum datum = new UserMealTemplate.Datum();
                            datum.rowIndex = i;
                            List<Meal> mealsList = new List<Meal>();
                            datum.Meals = mealsList;
                            string date = (string)item.GetValue("date");
                            datum.date = date;
                            JArray meals = (JArray)item.GetValue("meals");
                            bool error = (bool)item.GetValue("error");
                            if (!error)
                            {
                                foreach (JObject mealItem in meals)
                                {
                                    Meal meal = new Meal();
                                    meal.Id = (int)mealItem.GetValue("id");
                                    meal.MealType = (string)mealItem.GetValue("meal_type");
                                    Recipe recipe = new Recipe();
                                    meal.Recipe = recipe;
                                    Chai chai = new Chai();
                                    try
                                    {
                                        JObject recipeArray = (JObject)mealItem.GetValue("recipe");
                                        meal.Recipe.Id = recipeArray.GetValue("id").ToString();
                                        meal.Recipe.Name = recipeArray.GetValue("name").ToString();
                                        meal.Recipe.Ingredients = recipeArray.GetValue("ingredients").ToObject<Ingredient[]>();
                                        meal.Recipe.Images = recipeArray.GetValue("images").ToObject<Image[]>();
                                        meal.Recipe.Source = recipeArray.GetValue("source").ToObject<Models.Custom.Source>();
                                        meal.Recipe.Servings = (long)recipeArray.GetValue("servings");
                                        try
                                        {
                                            meal.Recipe.Durations = recipeArray.GetValue("durations").ToObject<Models.Custom.Durations>();
                                        }
                                        catch
                                        {
                                            meal.Recipe.Durations = new Durations();
                                            meal.Recipe.Durations.PrepTime = 0;
                                        meal.Recipe.Durations.CookTime = 0;
                                            meal.Recipe.Durations.TotalTime = 0;
                                    }

                                        if (recipe.Durations.PrepTime > 0)
                                        {
                                            recipe.PrepTime = "" + recipe.Durations.PrepTime;
                                        }
                                        if (recipe.Durations.CookTime > 0)
                                        {
                                            recipe.CookingTime = "" + recipe.Durations.CookTime;
                                        }

                                        if (recipe.Durations.TotalTime > 0)
                                        {
                                            recipe.TotalTime = "" + recipe.Durations.TotalTime;
                                        }

                                        meal.Recipe.Labels = recipeArray.GetValue("labels").ToObject<Labels>();
                                        meal.Recipe.Language = (string)recipeArray.GetValue("langauge");
                                        try
                                        {
                                            var a = recipeArray.GetValue("constraints").ToObject<Constraints>();
                                            meal.Recipe.Constraints = recipeArray.GetValue("constraints").ToObject<Constraints>();
                                            meal.Recipe.Constraints = new Constraints();
                                        }
                                        catch
                                        {
                                            meal.Recipe.Constraints = new Constraints();
                                            Console.WriteLine();
                                        }

                                        try
                                        {
                                            meal.Recipe.chai = recipeArray.GetValue("chai").ToObject<Chai>();
                                        }
                                        catch
                                        {
                                            meal.Recipe.chai = chai;
                                        }

                                        Console.WriteLine();
                                    }
                                    catch (Exception e)
                                    {
                                        JObject recipeArray = (JObject)mealItem.GetValue("recipe");
                                        Chai chaiObj = recipeArray.GetValue("chai").ToObject<Chai>();
                                        meal.Recipe.chai = chaiObj;
                                        Console.WriteLine(e);
                                    }
                                    Console.WriteLine();
                                    datum.Meals.Add(meal);
                                }
                            }
                            userMealTemplate.Data.Add(datum);
                            i++;
                            Console.WriteLine();
                        }
                        Console.WriteLine();
                    }
                    else
                    {
                        BadResponseHandler(response);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR: " + e);
                }
                Console.WriteLine("End : " + DateTime.UtcNow);
                return userMealTemplate;
            }
        }

        public async Task<UserMealTemplate> AutocompleteMealPlan(User user, string mealPlanID)
        {

            await Task.Delay(10).ConfigureAwait(false);
            UserMealTemplate userMealTemplate = new UserMealTemplate();
            userMealTemplate.Data = new List<UserMealTemplate.Datum>();

            userMealTemplate.Data.Clear();

            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);

                Uri uri = new Uri(string.Format(ApiRootUrl + "/meal-plans/" + mealPlanID + "/auto-complete"));

                JObject jObject = new JObject();
                jObject.Add("replace_existing", true);


                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = HttpClient.PostAsync(uri, content).Result;

                if (result.IsSuccessStatusCode)
                {

                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);
                    Console.WriteLine(json);


                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e);
            }
            return userMealTemplate;
        }

        public async Task<bool> AddWeek(User user, string date)
        {
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + user.AuthToken);
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                string uri = ApiRootUrl + "/meal-plan-templates/" + StaticData.selectedPreviewMealPlanID + "/convert-to-meal-plan";
                JObject jObject = new JObject();
                jObject.Add("start_date", date);
                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = HttpClient.PostAsync(uri, content).Result;
                var json = await result.Content.ReadAsStringAsync();
                var jResponse = JObject.Parse(json);


                if (result.IsSuccessStatusCode)
                {
                    await App.HideStartDateMenu();
                    return true;
                }
                else 
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR: " + e);
            }
            return false;
        }

        public async Task<string> CreateMealPlan(User user, string name, string startDate, int days)
        {
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + user.AuthToken);
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                string uri = ApiRootUrl + "/meal-plans/";
                JObject jObject = new JObject();
                jObject.Add("name", name);
                jObject.Add("start_date", startDate);
                jObject.Add("number_of_days", days);

                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = HttpClient.PostAsync(uri, content).Result;
                var json = await result.Content.ReadAsStringAsync();
                var jResponse = JObject.Parse(json);
                Console.WriteLine(json);

                if (result.IsSuccessStatusCode)
                {
                    App.ShowAlert("Successfully Created Meal Plan");
                    JObject mealplanData = (JObject)jResponse.GetValue("data");
                    return (string)mealplanData.GetValue("id");
                }
                else
                {
                    try
                    {
                        var error = jResponse.GetValue("errors");
                        var errorString = error.ToString();
                        string cleaned = errorString.Replace("\n", "").Replace("]", "").Replace(" }", "").Replace("\"", "");
                        string[] splitString = cleaned.Split(": ");
                        string dates = splitString[2];
                        StaticData.createErrorText = dates;
                        App.ShowAlert("Could not create meal plan, dates conflicted.");
                    }
                    catch
                    {
                        App.ShowAlert("Could not create meal plan, dates conflicted.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR: " + e);
            }
            return null;
        }

        public async Task<MealPlanModel> GetInfluencerMealPlanWeek(User user, string week)
        {
            await Task.Delay(10).ConfigureAwait(false);
            MealPlanModel weeklyMealPlans = new MealPlanModel();
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + user.AuthToken);
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                Uri uri = new Uri(string.Format(ApiRootUrl + "/meal-plan-templates/" + StaticData.selectedPreviewMealPlanID + "/weeks/" + week, string.Empty));
                HttpResponseMessage response = await HttpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<MealPlanModel>(content);
                    weeklyMealPlans = json;
                }
                else
                {
                    BadResponseHandler(response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR: " + e);
            }
            return weeklyMealPlans;
        }

        public async Task<Influencer> GetInfluencers(User user, int page)
        {
            await Task.Delay(10).ConfigureAwait(false);
            var influencer = new Influencer();
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + user.AuthToken);
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                Uri uri = new Uri(string.Format("{0}/influencers/?page={1}", ApiRootUrl, page));
                HttpResponseMessage response = await HttpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    influencer = JsonConvert.DeserializeObject<Influencer>(content);

                    var jResponse = JObject.Parse(content);

                    JObject metaData = (JObject)jResponse.GetValue("meta");

                    double perPage = int.Parse(metaData.GetValue("per_page").ToString());
                    double total = int.Parse(metaData.GetValue("total").ToString());
                    try
                    {
                        AppSession.TotalInfluencerPages = int.Parse(Math.Ceiling(total / perPage).ToString());
                        Console.WriteLine(AppSession.TotalInfluencerPages);
                    }
                    catch
                    {
                        //Multiplied by 0 probably somewhere
                        AppSession.TotalInfluencerPages = 1;
                    }
                }
                else
                {
                    BadResponseHandler(response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR: " + e);
            }
            return influencer;
        }

        public async Task<UserMealPlans> GetUserMealPlans(User user)
        {
            await Task.Delay(10).ConfigureAwait(false);
            var mealPlan = new UserMealPlans();
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + user.AuthToken);
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                Uri uri = new Uri(ApiRootUrl + "/meal-plans");
                HttpResponseMessage response = await HttpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    mealPlan = JsonConvert.DeserializeObject<UserMealPlans>(content);
                    user.MealPlans = mealPlan;
                    Console.WriteLine();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    //BadResponseHandler(response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR: " + e);
            }

            return mealPlan;
        }

        public async Task<bool> DeleteUserMealPlan(User user, string mealPlanID)
        {
            await Task.Delay(10).ConfigureAwait(false);

            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);

            try
            {
                Uri uri = new Uri(string.Format($"{ApiRootUrl}/meal-plans/{mealPlanID}", string.Empty));
                HttpResponseMessage response = await HttpClient.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Successfully deleted meal plan.");
                    return true;
                }
                else
                {
                    BadResponseHandler(response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR: " + e);
            }
            return false;
        }

        public async Task<ShoppingList> GetShoppingList(User user)
        {
            await Task.Delay(10).ConfigureAwait(false);
            var shoppingList = new ShoppingList();
            ShoppingList.CustomList customList = new ShoppingList.CustomList();
            ShoppingList.Content content = new ShoppingList.Content();
            shoppingList.customList = customList;
            shoppingList.content = content;
            shoppingList.content.items = new List<ShoppingList.Items>();
            shoppingList.content.combinedItems = new List<ShoppingList.CombinedItems>();
            shoppingList.content.recipes = new List<Recipe>();

            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + user.AuthToken);
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

            string uri = ApiRootUrl + "/shopping-list";

            try
            {
                var response = HttpClient.GetAsync(uri).Result;
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);

                    shoppingList.customList = jResponse.GetValue("list").ToObject<ShoppingList.CustomList>();
                    JObject jContent = (JObject)jResponse.GetValue("content");
                    JArray items = (JArray)jContent.GetValue("items");
                    JArray combinedItems = (JArray)jContent.GetValue("combined_items");
                    JArray recipes = (JArray)jContent.GetValue("recipes");

                    if (items != null)
                    {
                        foreach (JObject i in items.Children<JObject>())
                        {
                            try
                            {
                                ShoppingList.Items shoppingListItems = new ShoppingList.Items();
                                // Get the id
                                try { shoppingListItems.id = i.GetValue("id").ToString(); }
                                catch { shoppingListItems.id = ""; }
                                // Create the item base obj
                                ShoppingList.Item item = new ShoppingList.Item();
                                // Fetch the values
                                JObject jItem = (JObject)i.GetValue("item");
                                try { item.name = jItem.GetValue("name").ToString(); }
                                catch { item.name = "undefined"; }
                                try { item.brand = jItem.GetValue("brand").ToString(); }
                                catch { item.brand = ""; }
                                try { item.comment = jItem.GetValue("comment").ToString(); }
                                catch { item.comment = ""; }
                                try { item.quantity = (double)jItem.GetValue("quantity"); }
                                catch { item.quantity = 0; }
                                try { item.unit = jItem.GetValue("unit").ToString(); }
                                catch { item.unit = ""; }
                                try { shoppingListItems.isChecked = (bool)i.GetValue("checked"); }
                                catch { shoppingListItems.isChecked = false; }
                                try { shoppingListItems.image_url = i.GetValue("image_url").ToString(); }
                                catch { shoppingListItems.image_url = ""; }
                                // Creating the parent item means we will not get null errors when trying to
                                // access its variables.
                                shoppingListItems.analysis = new ShoppingList.Analysis();
                                shoppingListItems.analysis.product = new ShoppingList.Product();
                                JObject jAnalysis = (JObject)i.GetValue("analysis");
                                JObject jProduct = (JObject)jAnalysis.GetValue("product");
                                JObject jCategory = (JObject)jAnalysis.GetValue("category");
                                JObject jBrand = (JObject)jAnalysis.GetValue("category");
                                shoppingListItems.analysis.product.canonical_name = jProduct.GetValue("canonical_name").ToString();
                                try { shoppingListItems.analysis.product.original_name = jProduct.GetValue("original_name").ToString(); }
                                catch { shoppingListItems.analysis.product.original_name = ""; }
                                shoppingListItems.analysis.category = new ShoppingList.Category();
                                try { shoppingListItems.analysis.category.canonical_name = jCategory.GetValue("canonical_name").ToString(); }
                                catch { shoppingListItems.analysis.category.canonical_name = ""; }
                                shoppingListItems.analysis.brand = new ShoppingList.Brand();
                                try { shoppingListItems.analysis.brand.canonical_name = jBrand.GetValue("canonical_name").ToString(); }
                                catch { shoppingListItems.analysis.brand.canonical_name = ""; }


                                shoppingListItems.recipe = new ShoppingList.ShoppingRecipe();
                                JObject jRecipe = (JObject)i.GetValue("recipe");
                                try { shoppingListItems.recipe.recipe_id = jRecipe.GetValue("recipe_id").ToString(); }
                                catch { shoppingListItems.recipe.recipe_id = ""; }
                                try { shoppingListItems.recipe.position = (int)jRecipe.GetValue("position"); }
                                catch { shoppingListItems.recipe.position = -1; }
                                shoppingListItems.combined = new ShoppingList.Combined();
                                JObject jCombined = (JObject)i.GetValue("combined");
                                try { shoppingListItems.combined.combined_item_id = jCombined.GetValue("combined_item_id").ToString(); }
                                catch { shoppingListItems.combined.combined_item_id = ""; }
                                try { shoppingListItems.combined.quantity = (double)jCombined.GetValue("quantity"); }
                                catch { shoppingListItems.combined.quantity = 0; }
                                try { shoppingListItems.created_time = i.GetValue("created_time").ToString(); }
                                catch { shoppingListItems.created_time = ""; }
                                try { shoppingListItems.updated_at = i.GetValue("updated_at").ToString(); }
                                catch { shoppingListItems.updated_at = ""; }
                                shoppingListItems.matchingProperties = new ShoppingList.MatchingProperties();
                                JObject jMatchingProperties = (JObject)i.GetValue("combined");
                                try { shoppingListItems.matchingProperties.gtin = jMatchingProperties.GetValue("gtin").ToString(); }
                                catch { shoppingListItems.matchingProperties.gtin = ""; }
                                try { shoppingListItems.matchingProperties.custom_product_id = jMatchingProperties.GetValue("custom_product_id").ToString(); }
                                catch { shoppingListItems.matchingProperties.custom_product_id = ""; }
                                try { shoppingListItems.custom_product_url = i.GetValue("custom_product_url").ToString(); }
                                catch { shoppingListItems.custom_product_url = ""; }
                                shoppingListItems.item = item;
                                shoppingList.content.items.Add(shoppingListItems);
                            }
                            catch (Exception itemException)
                            {
                                Console.WriteLine("Error with item " + itemException.StackTrace);
                            }
                        }
                    }
                    if (combinedItems != null)
                    {
                        foreach (JObject ci in combinedItems.Children<JObject>())
                        {
                            try
                            {
                                ShoppingList.CombinedItems shoppingListCombinedItems = new ShoppingList.CombinedItems();
                                // Get the id
                                try { shoppingListCombinedItems.id = ci.GetValue("id").ToString(); }
                                catch { shoppingListCombinedItems.id = ""; }
                                // Create the item base obj
                                ShoppingList.Item item = new ShoppingList.Item();
                                // Fetch the values
                                JObject jItem = (JObject)ci.GetValue("item");
                                try { item.name = jItem.GetValue("name").ToString(); }
                                catch { item.name = "undefined"; }
                                try { item.brand = jItem.GetValue("brand").ToString(); }
                                catch { item.brand = ""; }
                                try { item.comment = jItem.GetValue("comment").ToString(); }
                                catch { item.comment = ""; }
                                try { item.quantity = (double)jItem.GetValue("quantity"); }
                                catch { item.quantity = 0; }
                                try { item.unit = jItem.GetValue("unit").ToString(); }
                                catch { item.unit = ""; }
                                try { shoppingListCombinedItems.isChecked = (bool)ci.GetValue("checked"); }
                                catch { shoppingListCombinedItems.isChecked = false; }
                                try { shoppingListCombinedItems.image_url = ci.GetValue("image_url").ToString(); }
                                catch { shoppingListCombinedItems.image_url = ""; }
                                // Creating the parent item means we will not get null errors when trying to
                                // access its variables.
                                shoppingListCombinedItems.analysis = new ShoppingList.Analysis();
                                shoppingListCombinedItems.analysis.product = new ShoppingList.Product();
                                JObject jAnalysis = (JObject)ci.GetValue("analysis");
                                JObject jProduct = (JObject)jAnalysis.GetValue("product");
                                JObject jCategory = (JObject)jAnalysis.GetValue("category");
                                JObject jBrand = (JObject)jAnalysis.GetValue("category");
                                shoppingListCombinedItems.analysis.product.canonical_name = jProduct.GetValue("canonical_name").ToString();
                                try { shoppingListCombinedItems.analysis.product.original_name = jProduct.GetValue("original_name").ToString(); }
                                catch { shoppingListCombinedItems.analysis.product.original_name = ""; }
                                shoppingListCombinedItems.analysis.category = new ShoppingList.Category();
                                try { shoppingListCombinedItems.analysis.category.canonical_name = jCategory.GetValue("canonical_name").ToString(); }
                                catch { shoppingListCombinedItems.analysis.category.canonical_name = ""; }
                                shoppingListCombinedItems.analysis.brand = new ShoppingList.Brand();
                                try { shoppingListCombinedItems.analysis.brand.canonical_name = jBrand.GetValue("canonical_name").ToString(); }
                                catch { shoppingListCombinedItems.analysis.brand.canonical_name = ""; }
                                try { shoppingListCombinedItems.created_time = ci.GetValue("created_time").ToString(); }
                                catch { shoppingListCombinedItems.created_time = ""; }
                                try { shoppingListCombinedItems.updated_at = ci.GetValue("updated_at").ToString(); }
                                catch { shoppingListCombinedItems.updated_at = ""; }
                                shoppingListCombinedItems.matchingProperties = new ShoppingList.MatchingProperties();
                                JObject jMatchingProperties = (JObject)ci.GetValue("combined");
                                try { shoppingListCombinedItems.matchingProperties.gtin = jMatchingProperties.GetValue("gtin").ToString(); }
                                catch { shoppingListCombinedItems.matchingProperties.gtin = ""; }
                                try { shoppingListCombinedItems.matchingProperties.custom_product_id = jMatchingProperties.GetValue("custom_product_id").ToString(); }
                                catch { shoppingListCombinedItems.matchingProperties.custom_product_id = ""; }
                                try { shoppingListCombinedItems.custom_product_url = ci.GetValue("custom_product_url").ToString(); }
                                catch { shoppingListCombinedItems.custom_product_url = ""; }
                                shoppingListCombinedItems.item = item;
                                shoppingList.content.combinedItems.Add(shoppingListCombinedItems);
                            }
                            catch (Exception combinedItemException)
                            {
                                Console.WriteLine("Error with combined item " + combinedItemException.StackTrace);
                            }
                        }
                    }

                    if (recipes != null)
                    {
                        foreach (JObject r in recipes.Children<JObject>())
                        {
                            try
                            {
                                Recipe recipe = new Recipe();
                                // Get the id
                                try { recipe.Id = r.GetValue("id").ToString(); }
                                catch { recipe.Id = ""; }
                                try { recipe.Name = r.GetValue("name").ToString(); }
                                catch { recipe.Name = ""; }
                                JArray jImages = (JArray)r.GetValue("images");
                                if (jImages != null)
                                {
                                    recipe.Images = new Image[jImages.Count];
                                    int i = 0;
                                    foreach (JObject im in jImages.Children<JObject>())
                                    {
                                        JObject jResponsive = (JObject)im.GetValue("responsive");
                                        recipe.Images[i] = new Image();
                                        try { recipe.Images[i].Url = new Uri(im.GetValue("url").ToString()); }
                                        catch { recipe.Images[i] = null; }
                                        recipe.Images[i].Responsive = new Responsive();
                                        if (jResponsive != null)
                                        {
                                            try { recipe.Images[i].Responsive.Url = new Uri(jResponsive.GetValue("url").ToString()); }
                                            catch { }
                                            try { recipe.Images[i].Responsive.Width = (long)jResponsive.GetValue("width"); }
                                            catch { }
                                            try { recipe.Images[i].Responsive.Height = (long)jResponsive.GetValue("height"); }
                                            catch { }
                                        }
                                        i++;
                                    }
                                }
                                else { recipe.Images = null; }

                                JObject jSource = (JObject)r.GetValue("source");
                                recipe.Source = new Source();
                                try { recipe.Source.Name = jSource.GetValue("name").ToString(); }
                                catch { recipe.Source.Name = ""; }
                                try { recipe.Source.DisplayName = jSource.GetValue("display_name").ToString(); }
                                catch { recipe.Source.DisplayName = ""; }
                                try { recipe.Source.SourceRecipeUrl = new Uri(jSource.GetValue("source_recipe_url").ToString()); }
                                catch { }
                                JObject jSourceImage = (JObject)jSource.GetValue("image");
                                recipe.Source.SourceImage = new Image();
                                recipe.Source.SourceImage.Responsive = new Responsive();
                                if (jSourceImage != null)
                                {
                                    try { recipe.Source.SourceImage.Url = new Uri(jSourceImage.GetValue("url").ToString()); }
                                    catch { }
                                    JObject jSourceImageResponsive = (JObject)jSourceImage.GetValue("responsive").ToString();
                                    if (jSourceImageResponsive != null)
                                    {
                                        try { recipe.Source.SourceImage.Responsive.Url = new Uri(jSourceImageResponsive.GetValue("url").ToString()); }
                                        catch { }
                                        try { recipe.Source.SourceImage.Responsive.Width = (long)jSourceImageResponsive.GetValue("width"); }
                                        catch { }
                                        try { recipe.Source.SourceImage.Responsive.Height = (long)jSourceImageResponsive.GetValue("height"); }
                                        catch { }
                                    }
                                }
                                try { recipe.Source.License = jSource.GetValue("license").ToString(); }
                                catch { recipe.Source.License = ""; }
                                shoppingList.content.recipes.Add(recipe);
                            }
                            catch (Exception recipeException)
                            {
                                Console.WriteLine("Error with recipe " + recipeException.StackTrace);
                            }
                        }

                        List<Recipe> recipesList = shoppingList.content.recipes;
                        try
                        {
                            IEnumerable<Recipe> noDuplicates = recipesList.Distinct();
                            shoppingList.content.recipes = noDuplicates.ToList<Recipe>();
                        }
                        catch { }
                    }
                }
                else
                {
                    BadResponseHandler(response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR: " + e.StackTrace);
            }

            return shoppingList;
        }
        public async Task<bool> AddRecipeToShoppingList(User user, Recipe recipe)
        {
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                string uri = ApiRootUrl + "/shopping-list/recipes";
                JObject jObject = new JObject();
                jObject.Add("whisk_recipe_id", recipe.Id);
                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = HttpClient.PostAsync(uri, content).Result;
                var json = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR: " + e);
            }
            return false;
        }
        public async Task<bool> RemoveRecipeFromShoppingList(User user, Recipe recipe)
        {
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                string uri = $"{ApiRootUrl}/shopping-list/recipes/{recipe.Id}";
                var result = HttpClient.DeleteAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {
                    try
                    {
                        return true;
                    }
                    catch (Exception e)
                    {

                    }
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR: " + e);
            }
            return false;
        }

        public async Task<Uri> ConvertToBasket(User user)
        {
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                string uri = $"{ApiRootUrl}/shopping-list/convert";
                var result = HttpClient.PostAsync(uri, null).Result;

                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);
                    JObject data = (JObject)jResponse.GetValue("data");
                    Uri url = (Uri)data.GetValue("url");

                    return url;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR: " + e);
            }
            return null;
        }

        public async Task<bool> ClearShoppingBasket(User user)
        {
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                string uri = $"{ApiRootUrl}/shopping-list/clear";
                var result = HttpClient.PutAsync(uri, null).Result;

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR: " + e);
            }
            return false;
        }

        public async Task<VideoFeed> GetVideos(int category)
        {
            VideoFeed videos = new VideoFeed();


            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

            try
            {
                string uri = ApiRootUrl + "/video-categories/" + category;

                var result = HttpClient.GetAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {

                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);

                    JObject videoData = (JObject)jResponse.GetValue("data");

                    videos = videoData.ToObject<VideoFeed>();

                    Console.WriteLine(json.ToString());
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR");
            }

            return videos;
        }
    

        public async Task<Meal> AddMealToMealPlan(User user, string mealType, string mealplanID, string date, string chaiID, string whiskID, bool isCalendar)
        {
            //await Task.Delay(10).ConfigureAwait(false);
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);


                string uri = ApiRootUrl + $"/meal-plans/{mealplanID}/meals";
                JObject jObject = new JObject();
                jObject.Add("date", date);
                jObject.Add("meal_type", mealType);
                jObject.Add("recipe_id", chaiID);
                jObject.Add("whisk_recipe_id", whiskID);
                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = HttpClient.PostAsync(uri, content).Result;
                var json = await result.Content.ReadAsStringAsync();
                var jResponse = JObject.Parse(json);

                if (result.IsSuccessStatusCode)
                {
                    Meal newMeal = new Meal();
                    newMeal.Recipe = new Recipe();
                    newMeal.Recipe.Source = new Source();
                    newMeal.Recipe.chai = new Chai();
                    JObject data, mealData, recipeData, innerRecipeData, recipeSource, chaiData;
                    try { data = (JObject)jResponse.GetValue("data"); } catch { data = null; }
                    if (data != null)
                    {
                        //This was missing images and the ingredients and was causing a couple issues
                        newMeal.violatesMealPlan = (bool)data.GetValue("violates_plan");
                        mealData = (JObject)data.GetValue("meal");
                        newMeal.Id = (int)mealData.GetValue("id");
                        newMeal.MealType = mealData.GetValue("meal_type").ToString();
                        recipeData = (JObject)mealData.GetValue("recipe");
                        innerRecipeData = (JObject)recipeData.GetValue("recipe");
                        newMeal.Recipe.Id = innerRecipeData.GetValue("id").ToString();
                        newMeal.Recipe.name = innerRecipeData.GetValue("name").ToString();
                        newMeal.Recipe.Ingredients = innerRecipeData.GetValue("ingredients").ToObject<Ingredient[]>();
                        newMeal.Recipe.Images = innerRecipeData.GetValue("images").ToObject<Image[]>();
                        recipeSource = (JObject)innerRecipeData.GetValue("source");
                        newMeal.Recipe.Source.Name = recipeSource.GetValue("name").ToString();
                        newMeal.Recipe.Source.DisplayName = recipeSource.GetValue("display_name").ToString();
                        newMeal.Recipe.Source.SourceRecipeUrl = (Uri)recipeSource.GetValue("source_recipe_url");
                        chaiData = (JObject)recipeData.GetValue("chai");
                        newMeal.Recipe.chai.Id = (int)chaiData.GetValue("id");
                        newMeal.Recipe.chai.Name = chaiData.GetValue("name").ToString();
                        newMeal.Recipe.chai.PrepTime = (long)chaiData.GetValue("prep_time");
                        newMeal.Recipe.chai.CookTime = (long)chaiData.GetValue("cook_time");
                        newMeal.Recipe.chai.TotalTime = (long)chaiData.GetValue("total_time");
                        newMeal.Recipe.chai.Method = chaiData.GetValue("method").ToString();
                        newMeal.Recipe.chai.Author = chaiData.GetValue("author").ToString();
                        return newMeal;
                    }
                    else
                    {
                        // Failed to get the data
                    }
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR: " + e);
            }
            return null;
        }

        public async Task<Meal> UpdateMealOnMealPlan(User user, string mealID, string mealplanID, string chaiID, string whiskID)
        {
            await Task.Delay(10).ConfigureAwait(false);
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                string uri = $"{ApiRootUrl}/meal-plans/{mealplanID}/meals/{mealID}";
                JObject jObject = new JObject();
                jObject.Add("recipe_id", chaiID);
                jObject.Add("whisk_recipe_id", whiskID);
                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = HttpClient.PutAsync(uri, content).Result;
                var json = await result.Content.ReadAsStringAsync();
                var jResponse = JObject.Parse(json);

                if (result.IsSuccessStatusCode)
                {
                    Meal newMeal = new Meal();
                    newMeal.Recipe = new Recipe();
                    newMeal.Recipe.Source = new Source();
                    newMeal.Recipe.chai = new Chai();
                    JObject data, mealData, recipeData, innerRecipeData, recipeSource, chaiData;
                    try { data = (JObject)jResponse.GetValue("data"); } catch { data = null; }
                    if (data != null)
                    {
                        //This was missing images and the ingredients and was causing a couple issues
                        newMeal.violatesMealPlan = (bool)data.GetValue("violates_plan");
                        mealData = (JObject)data.GetValue("meal");
                        newMeal.Id = (int)mealData.GetValue("id");
                        newMeal.MealType = mealData.GetValue("meal_type").ToString();
                        recipeData = (JObject)mealData.GetValue("recipe");
                        innerRecipeData = (JObject)recipeData.GetValue("recipe");
                        newMeal.Recipe.Id = innerRecipeData.GetValue("id").ToString();
                        newMeal.Recipe.name = innerRecipeData.GetValue("name").ToString();
                        newMeal.Recipe.Ingredients = innerRecipeData.GetValue("ingredients").ToObject<Ingredient[]>();
                        newMeal.Recipe.Images = innerRecipeData.GetValue("images").ToObject<Image[]>();
                        recipeSource = (JObject)innerRecipeData.GetValue("source");
                        newMeal.Recipe.Source.Name = recipeSource.GetValue("name").ToString();
                        newMeal.Recipe.Source.DisplayName = recipeSource.GetValue("display_name").ToString();
                        newMeal.Recipe.Source.SourceRecipeUrl = (Uri)recipeSource.GetValue("source_recipe_url");
                        chaiData = (JObject)recipeData.GetValue("chai");
                        newMeal.Recipe.chai.Id = (int)chaiData.GetValue("id");
                        newMeal.Recipe.chai.Name = chaiData.GetValue("name").ToString();
                        newMeal.Recipe.chai.PrepTime = (long)chaiData.GetValue("prep_time");
                        newMeal.Recipe.chai.CookTime = (long)chaiData.GetValue("cook_time");
                        newMeal.Recipe.chai.TotalTime = (long)chaiData.GetValue("total_time");
                        newMeal.Recipe.chai.Method = chaiData.GetValue("method").ToString();
                        newMeal.Recipe.chai.Author = chaiData.GetValue("author").ToString();
                        return newMeal;
                    }
                    else
                    {
                        // Failed to get the data
                    }
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR: " + e);
            }
            return null;
        }

        public async Task<string> RemoveMealOnMealPlan(User user, string mealID, string mealplanID)
        {
            await Task.Delay(10).ConfigureAwait(false);
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                string uri = $"{ApiRootUrl}/meal-plans/{mealplanID}/meals/{mealID}";
                var result = HttpClient.DeleteAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {
                    return mealID;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR: " + e);
            }
            return null;
        }

        /// <summary>
        ///  Recipe Editor
        /// </summary>
        /// <returns></returns>

        // List recipes by the current user
        public async Task<List<Recipe>> GetUserRecipes(User user)
        {
            List<Recipe> recipesList = new List<Recipe>();

            await Task.Delay(10).ConfigureAwait(false);
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                string uri = $"{ApiRootUrl}/user/recipes";
                var result = HttpClient.GetAsync(uri).Result;

                //var test = HttpClient.GetAsync($"{ApiRootUrl}/shopping-list").Result;
                //var json2 = await test.Content.ReadAsStringAsync();
                //var jResponse2 = JObject.Parse(json2);

                if (result.IsSuccessStatusCode)
                {
                    try
                    {
                        var json = await result.Content.ReadAsStringAsync();
                        var jResponse = JObject.Parse(json);
                        JArray jArray = (JArray)jResponse.GetValue("data");

                        foreach (JObject r in jArray.Children<JObject>())
                        {
                            Recipe recipe = new Recipe();
                            try
                            {
                                Chai chai = new Chai();
                                recipe.Id = r.GetValue("whisk_recipe_id").ToString();
                                recipe.WhiskRecipeId = r.GetValue("whisk_recipe_id").ToString();
                                recipe.Name = r.GetValue("name").ToString();

                                string methodString = r.GetValue("method").ToString();

                                recipe.chai = new Chai
                                {

                                };
                                recipe.chai.Id = (long)r.GetValue("id");
                                recipe.chai.Method = methodString;

                                try
                                {
                                    recipe.chai.PrepTime = (long)r.GetValue("prep_time");
                                }
                                catch (Exception es)
                                {
                                    recipe.chai.PrepTime = 0;
                                }

                                try
                                {
                                    recipe.chai.CookTime = (long)r.GetValue("cook_time");
                                }
                                catch (Exception es)
                                {
                                    recipe.chai.CookTime = 0;
                                }

                                try
                                {
                                    recipe.chai.TotalTime = (long)r.GetValue("total_time");
                                }
                                catch (Exception es)
                                {
                                    recipe.chai.TotalTime = 0;
                                }

                                recipe.chai.Status = "draft";

                                try
                                {
                                    recipe.chai.Status = r.GetValue("status").ToString();
                                }
                                catch (Exception es)
                                {
                                    recipe.chai.Status = "draft";
                                }

                                if ((bool)r.GetValue("published"))
                                {
                                    recipe.chai.Status = "published";
                                }

                                recipe.Servings = (int)r.GetValue("yield");
                                recipe.CookingTime = recipe.chai.CookTime + " mins";
                                recipe.PrepTime = recipe.chai.PrepTime + " mins";
                                recipe.TotalTime = recipe.chai.TotalTime + " mins";

                                recipe.MealType = r.GetValue("meal_type").ToString();
                                recipe.DishType = r.GetValue("dish_type").ToString();
                                recipe.CuisineType = r.GetValue("cuisine_type").ToString();

                                JObject userData = (JObject)r.GetValue("creator");
                                JObject coreIngredientData = (JObject)r.GetValue("core_ingredient");
                                JArray ingredients = (JArray)r.GetValue("ingredients");
                                //JObject chaiData = 

                                List<Ingredient> recipeIngredients = new List<Ingredient>();

                                JObject baseIngredient = null;
                                foreach (JObject ingredient in ingredients)
                                {
                                    baseIngredient = (JObject)ingredient.GetValue("base_ingredient");
                                    JObject ingredientUnit = (JObject)ingredient.GetValue("unit");

                                    Ingredient newIngredient = new Ingredient
                                    {
                                        RecipeId = (int)ingredient.GetValue("id"),
                                        Id = (int)baseIngredient.GetValue("id"),
                                        Name = (string)baseIngredient.GetValue("name"),
                                        Text = (string)baseIngredient.GetValue("name"),
                                        Amount = (string)ingredient.GetValue("amount"),
                                        Unit = new IngredientUnit
                                        {
                                            ID = (int)ingredientUnit.GetValue("id"),
                                            Name = (string)ingredientUnit.GetValue("name"),
                                            Abbreviation = (string)ingredientUnit.GetValue("abbreviation")
                                        },
                                    };



                                    //recipeIngredients.Add(newIngredient);

                                    Ingredient found = recipeIngredients.Find(x => x.Name == newIngredient.Name);
                                    if (found == null)
                                    {
                                        Console.WriteLine("Adding new ingredient " + newIngredient.Name);
                                        recipeIngredients.Add(newIngredient);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Found existing ingredient " + newIngredient.Name);
                                        if (float.Parse(found.Amount) < float.Parse(newIngredient.Amount))
                                        {
                                            Console.WriteLine("Found amount < new amount ");
                                            recipeIngredients.Remove(found);
                                            Console.WriteLine("Remove " + found.Name);
                                            recipeIngredients.Add(newIngredient);
                                            Console.WriteLine("Adding " + newIngredient.Name);
                                        }
                                        else
                                        {
                                            Console.WriteLine("Found amount >= new amount ");
                                        }

                                    }
                                }

                                recipe.MainIngredient = new Ingredient
                                {

                                    Name = (string)coreIngredientData.GetValue("name"),
                                    Text = (string)coreIngredientData.GetValue("name"),
                                    Amount = "0",
                                    Id = (int)coreIngredientData.GetValue("id"),
                                    Unit = new IngredientUnit
                                    {
                                        ID = (int)coreIngredientData.GetValue("id"),
                                        Name = "Cups",
                                        Abbreviation = "Cups"
                                    },
                                };


                                //recipeIngredients.Add(recipe.MainIngredient);


                                recipe.Ingredients = SanitiseIngredientList(recipeIngredients);

                                User creator = new User();
                                creator.FirstName = (string)userData.GetValue("forename");
                                creator.LastName = (string)userData.GetValue("surname");

                                recipe.Creator = creator;
                                recipe.Author = creator.FirstName + " " + creator.LastName;

                                recipe.MainImageSource = coreIngredientData.GetValue("image_url").ToString();

                                recipe.Images = new Image[1];
                                recipe.Images[0] = new Image
                                {
                                    Url = new Uri(recipe.MainImageSource)
                                };

                                recipe.IsFavourite = true;

                                recipe.Labels = r.GetValue("labels").ToObject<Labels>();
                                recipe.Language = (string)r.GetValue("langauge");



                                try
                                {
                                    recipe.Constraints = r.GetValue("constraints").ToObject<Constraints>();
                                }
                                catch
                                {
                                    recipe.Constraints = new Constraints();
                                }

                            }
                            catch (Exception e)
                            {

                            }

                            recipesList.Add(recipe);
                        }
                        Console.WriteLine(json.ToString());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error: {e}");
                    }
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }

            return recipesList;
        }

        public Ingredient [] SanitiseIngredientList(List<Ingredient> ingredients)
        {

            return ingredients.ToArray();
        }

        public async Task<bool> UpdateUserRecipe(User user, Recipe recipe)
        {
            await Task.Delay(10).ConfigureAwait(false);
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                string uri = $"{ApiRootUrl}/user/recipes/" + recipe.chai.Id;

                JObject jObject = new JObject();
                jObject.Add("core_ingredient_id", recipe.MainIngredient.Id);
                jObject.Add("cuisine_type", "pizza");
                jObject.Add("dish_type", recipe.DishType);
                jObject.Add("meal_category_id", 1);
                jObject.Add("meal_type", recipe.MealType.ToLower());
                jObject.Add("name", recipe.Name);
                jObject.Add("status", recipe.chai.Status);

                try
                {
                    jObject.Add("cook_time", TextTools.MakeStringNumeric(recipe.chai.CookTime.ToString()));
                    jObject.Add("prep_time", TextTools.MakeStringNumeric(recipe.chai.PrepTime.ToString()));
                    jObject.Add("total_time", recipe.chai.TotalTime);
                }
                catch (Exception e1)
                {
                    jObject.Add("cook_time", 5);
                    jObject.Add("prep_time", 5);
                    jObject.Add("total_time", 10);
                }

                jObject.Add("yield", "" + recipe.Servings);

                string insaneMethod = recipe.chai.Method;
                string saneMethod = TextTools.SanitiseForJson(recipe.chai.Method);

                jObject.Add("method", saneMethod);

                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = HttpClient.PutAsync(uri, content).Result;

                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);
                    //JObject recipeData = (JObject)jResponse.GetValue("data");

                    // string recipeId = recipeData.GetValue("id").ToString();
                    return true;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
            return false;
        }


        public async Task<string> SaveUserRecipe(User user, Recipe recipe)
        {
            await Task.Delay(10).ConfigureAwait(false);

            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                string uri = $"{ApiRootUrl}/user/recipes";


                JObject jObject = new JObject();

                if (recipe.chai.CookTime == null)
                {
                    recipe.chai.PrepTime = 0;
                    recipe.chai.CookTime = 0;
                    recipe.chai.TotalTime = 0;

                }

                if (recipe.MainIngredient.Id < 1)
                {
                    recipe.MainIngredient.Id=1;
                }


                jObject.Add("cook_time", recipe.chai.CookTime);
                jObject.Add("core_ingredient_id", recipe.MainIngredient.Id);
                //jObject.Add("cuisine_type", "pizza");
                jObject.Add("dish_type", recipe.DishType);
                jObject.Add("meal_category_id", 1);
                jObject.Add("meal_type", recipe.MealType.ToLower());

                string insaneMethod = recipe.chai.Method;
                string saneMethod = TextTools.SanitiseForJson(recipe.chai.Method);

                jObject.Add("method", saneMethod);

                //jObject.Add("method", recipe.chai.Method);
                jObject.Add("name", recipe.Name);
                jObject.Add("prep_time", recipe.chai.PrepTime);
                jObject.Add("status", recipe.chai.Status);
                jObject.Add("total_time", recipe.chai.TotalTime);
                jObject.Add("yield", "" + recipe.Servings);
                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = HttpClient.PostAsync(uri, content).Result;

                if (result.IsSuccessStatusCode)
                {

                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);
                    JObject recipeData = (JObject)jResponse.GetValue("data");

                    string recipeId = recipeData.GetValue("id").ToString();

                    Console.WriteLine("tis");
                    return recipeId;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
            return null;
        }

        public async Task<bool> AddIngredientToRecipe(User user, string recipeId, Ingredient ingredient)
        {
            string RecipeID = recipeId;
            await Task.Delay(10).ConfigureAwait(false);
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);

                string uri = $"{ApiRootUrl}/user/recipe/{RecipeID}/ingredients";

                JObject jObject = new JObject();

                jObject.Add("amount", Convert.ToDecimal(ingredient.Amount));
                jObject.Add("base_ingredient_id", ingredient.Id);
                jObject.Add("recipe_component", ingredient.RecipeComponent);
                jObject.Add("unit_id", ingredient.Unit.ID);

                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = HttpClient.PostAsync(uri, content).Result;

                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);
                    //JObject recipeData = (JObject)jResponse.GetValue("data");

                    //string recipeId = recipeData.GetValue("id").ToString();

                    return true;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
            return false;
        }

        // Updating a recipe ingredient
        public async Task<bool> UpdateUserRecipeIngredient(User user, string recipeID, Ingredient ingredient)
        {
            await Task.Delay(10).ConfigureAwait(false);
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                //string uri = $"{ApiRootUrl}/user/recipe/{recipeID}/ingredients/{ingredient.RecipeId}";
                string uri = $"{ApiRootUrl}/user/recipe/{recipeID}/ingredients/{ingredient.Id}";

                JObject jObject = new JObject();
                jObject.Add("amount", Convert.ToDecimal(ingredient.Amount));
                jObject.Add("base_ingredient_id", ingredient.Id);
                jObject.Add("recipe_component", ingredient.RecipeComponent);
                jObject.Add("unit_id", ingredient.Unit.ID);

                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = HttpClient.PutAsync(uri, content).Result;

                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);

                    return true;
                    
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
            return false;
        }

        public async Task<bool> DeleteUserRecipeIngredient(User user, string recipeID, Ingredient ingredient)
        {
            await Task.Delay(10).ConfigureAwait(false);
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                string uri = $"{ApiRootUrl}/user/recipe/{recipeID}/ingredients/{ingredient.RecipeId}";

                Console.WriteLine("uri " + uri);
                //string uri = $"{ApiRootUrl}/user/recipe/{recipeID}/ingredients/{ingredient.Id}";
                var result = HttpClient.DeleteAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
            return false;
        }

        public async Task<List<Ingredient>> SearchIngredients(string ingredientName, bool matchExact)
        {
            List<Ingredient> foundIngredients = new List<Ingredient>();

            await Task.Delay(10).ConfigureAwait(false);

            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string uri = $"{ApiRootUrl}/ingredient-search?keywords=" + ingredientName.Trim();
                var result = HttpClient.GetAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {

                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);
                    JArray jArray = (JArray)jResponse.GetValue("data");

                    foreach (JObject ingredient in jArray.Children<JObject>())
                    {
                        Ingredient foundIngredientForList = new Ingredient
                        {

                        };

                        try
                        {
                            if (ingredient.GetValue("name").ToString().Trim().ToLower().Contains(ingredientName.Trim().ToLower()))
                            {
                                foundIngredientForList.Id = (int)ingredient.GetValue("id");
                                foundIngredientForList.Name = ingredient.GetValue("name").ToString();
                                foundIngredients.Add(foundIngredientForList);
                            }
                            foundIngredientForList.MainImage = ingredient.GetValue("image_url").ToString();
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
            return foundIngredients;
        }


        public async Task<Ingredient> SearchIngredients(string ingredientName)
        {
            await Task.Delay(10).ConfigureAwait(false);
            Ingredient foundIngredient = null;
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string uri = $"{ApiRootUrl}/ingredient-search?keywords=" + ingredientName.Trim();
                var result = HttpClient.GetAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {

                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);
                    JArray jArray = (JArray)jResponse.GetValue("data");

                    foundIngredient = new Ingredient
                    {
                        Id = -1,
                        MainImage = null,
                        Name = "No Ingredient"
                    };

                    foreach (JObject ingredient in jArray.Children<JObject>())
                    {

                        //if (ingredient.GetValue("name").ToString().Contains(ingredientName.Trim()))
                        if (ingredient.GetValue("name").ToString() == ingredientName.Trim())
                        {
                            foundIngredient.Id = (int)ingredient.GetValue("id");
                            foundIngredient.Name = ingredient.GetValue("name").ToString();
                            try
                            {
                                foundIngredient.MainImage = ingredient.GetValue("image_url").ToString();
                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
            return foundIngredient;
        }

        
        public async Task<bool> DeleteUserRecipe(User user, string recipeID)
        {
            await Task.Delay(10).ConfigureAwait(false);
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                string uri = $"{ApiRootUrl}/user/recipes/{recipeID}";
                var result = HttpClient.DeleteAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
            return false;
        }

        public async Task<InfluencerMealPlans> GetUserMealPlanTemplates(User user)
        {
            //List<MealTemplate> mealPlanTemplates = new List<MealTemplate>();
            InfluencerMealPlans mealPlans = new InfluencerMealPlans();

            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);

            try
            {
                string uri = ApiRootUrl + "/user/meal-plan-templates";

                var result = HttpClient.GetAsync(uri).Result;
                var json = await result.Content.ReadAsStringAsync();
                var jResponse = JObject.Parse(json);


                JArray mealPlanTemplateData = (JArray)jResponse.GetValue("data");
                var convertedJson = JsonConvert.DeserializeObject<InfluencerMealPlans>(json);
                mealPlans = convertedJson;

                if (result.IsSuccessStatusCode)
                {
                    return mealPlans;
                }
                else
                {
                    BadResponseHandler(result, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR");
            }
            return null;
        }


        public async Task<string> CreateUserMealPlanTemplates(User user, string name, int days)
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);

            try
            {
                string uri = ApiRootUrl + "/user/meal-plan-templates";

                JObject jObject = new JObject();
                jObject.Add("name", name);
                jObject.Add("days", days);

                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = HttpClient.PostAsync(uri, content).Result;

                var json = await result.Content.ReadAsStringAsync();
                var jResponse = JObject.Parse(json);

                if (result.IsSuccessStatusCode)
                {
                    JObject data = (JObject)jResponse.GetValue("data");
                    if (data == null)
                    {
                        JObject error = (JObject)jResponse.GetValue("errors");
                        JArray nameError = (JArray)error.GetValue("name");
                        return nameError[0].ToString();
                    }
                    string idValue = data.GetValue("id").ToString();
                    return idValue;
                }
                else
                {
                    BadResponseHandler(result);

                    if (result.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        return "Forbidden";
                    }
                    return "Error";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR");
                return "crash";
            }
        }

        public async Task<bool> UpdateUserMealPlanTemplates(User user, int templateId, string name, string publishedAt)
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);

            try
            {
                string uri = ApiRootUrl + "/user/meal-plan-templates/" + templateId;

                JObject jObject = new JObject();
                jObject.Add("name", name);
                jObject.Add("published_at", publishedAt);


                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = HttpClient.PutAsync(uri, content).Result;

                //var json = await result.Content.ReadAsStringAsync();
                //var jResponse = JObject.Parse(json);

                //Console.WriteLine(json.ToString());

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR");
            }
            return false;
        }

        public async Task<bool> DeleteUserMealPlanTemplates(User user, int templateId)
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);

            try
            {
                string uri = ApiRootUrl + "/user/meal-plan-templates/" + templateId;
                var result = HttpClient.DeleteAsync(uri).Result;

                var json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR");
            }
            return false;
        }

        public async Task<MealPlanModel> GetUserMealPlanDayTemplates(User user, int planId)
        {
            await Task.Delay(10).ConfigureAwait(false);

            MealPlanModel mealPlans = new MealPlanModel();
            mealPlans.Data = new List<MealPlanModel.Datum>();
            mealPlans.Data.Clear();

            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);

            try
            {
                string uri = ApiRootUrl + "/user/meal-plan-templates/" + planId + "/day-templates";

                var result = HttpClient.GetAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);
                    JArray data = (JArray)jResponse.GetValue("data");

                    foreach (JObject item in data)
                    {
                        MealPlanModel.Datum datum = new MealPlanModel.Datum();
                        List<MealTemplate> mealsList = new List<MealTemplate>();
                        datum.mealTemplates = mealsList;
                        int mealID = (int)item.GetValue("id");
                        datum.id = mealID;
                        int day = (int)item.GetValue("day_number");
                        datum.day_Number = day;
                        JArray meals = (JArray)item.GetValue("meal_templates");
                        foreach (JObject mealItem in meals)
                        {
                            // id
                            // mealType
                            // recipe
                            // whisk_recipe_id
                            MealTemplate meal = new MealTemplate();
                            meal.id = (int)mealItem.GetValue("id");
                            meal.mealType = (string)mealItem.GetValue("meal_type");
                            Recipe recipe = new Recipe();
                            meal.recipe = recipe;
                            Chai chai = new Chai();
                            meal.whiskID = (string)mealItem.GetValue("whisk_recipe_id");
                            try
                            {
                                JObject recipeArray = (JObject)mealItem.GetValue("recipe");
                                meal.recipe.Id = recipeArray.GetValue("id").ToString();
                                meal.recipe.Name = recipeArray.GetValue("name").ToString();
                                meal.recipe.Durations = new Durations();
                                meal.recipe.Durations.PrepTime = (long)recipeArray.GetValue("prep_time");
                                meal.recipe.Durations.CookTime = (long)recipeArray.GetValue("cook_time");
                                meal.recipe.Durations.TotalTime = (long)recipeArray.GetValue("total_time");
                                meal.recipe.WhiskRecipeId = (string)recipeArray.GetValue("whisk_recipe_id");
                                if (recipe.Durations.PrepTime > 0)
                                {
                                    recipe.PrepTime = "" + recipe.Durations.PrepTime;
                                }
                                if (recipe.Durations.CookTime > 0)
                                {
                                    recipe.CookingTime = "" + recipe.Durations.CookTime;
                                }

                                if (recipe.Durations.TotalTime > 0)
                                {
                                    recipe.TotalTime = "" + recipe.Durations.TotalTime;
                                }

                                meal.recipe.MainIngredient = new Ingredient();

                                try
                                {
                                    JObject mainIngredient = (JObject)recipeArray.GetValue("core_ingredient");
                                    meal.recipe.MainIngredient.ID = (int)mainIngredient.GetValue("id");
                                    meal.recipe.MainIngredient.Id = meal.recipe.MainIngredient.ID;
                                    meal.recipe.MainIngredient.Name = mainIngredient.GetValue("name").ToString();
                                    meal.recipe.MainImageSource = mainIngredient.GetValue("image_url").ToString();

                                }
                                catch
                                {

                                }

                                Console.WriteLine();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                            Console.WriteLine();
                            datum.mealTemplates.Add(meal);
                        }
                        mealPlans.Data.Add(datum);
                    }
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR");
            }
            return mealPlans;
        }

        public async Task<MealPlanModel.Datum> CreateDayTemplateOnMealPlan(User user, int templateId, string day)
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);

            try
            {
                string uri = ApiRootUrl + "/user/meal-plan-templates/" + templateId + "/day-templates";

                JObject jObject = new JObject();
                jObject.Add("day_number", day);

                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = HttpClient.PostAsync(uri, content).Result;
                MealPlanModel.Datum mealPlanModel = new MealPlanModel.Datum();

                var json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    var jResponse = JObject.Parse(json);
                    JObject data = (JObject)jResponse.GetValue("data");
                    int id = (int)data.GetValue("id");
                    int dayNumber = (int)data.GetValue("day_number");
                    mealPlanModel.id = id;
                    mealPlanModel.day_Number = dayNumber;
                    mealPlanModel.mealTemplates = new List<MealTemplate>();

                    return mealPlanModel;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR");
            }
            return null;
        }

        public async Task<bool> DeleteDayTemplateOnMealPlan(User user, int templateId, string day)
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);

            try
            {
                string uri = ApiRootUrl + "/user/meal-plan-templates/" + templateId + "/day-templates/" + day;
                var result = HttpClient.DeleteAsync(uri).Result;

                var json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR");
            }
            return false;
        }

        public async Task<MealTemplate> AddMealToDayTemplate(User user, int dayTemplateId, string mealType, int recipeId, string whiskId, string image)
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);

            try
            {
                string uri = ApiRootUrl + "/user/day-templates/" + dayTemplateId + "/meal-templates";

                JObject jObject = new JObject();
                jObject.Add("meal_type", mealType);
                jObject.Add("recipe_id", recipeId);
                jObject.Add("whisk_recipe_id", whiskId);

                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = HttpClient.PostAsync(uri, content).Result;
                var json = await result.Content.ReadAsStringAsync();
                var jResponse = JObject.Parse(json);
                if (result.IsSuccessStatusCode)
                {
                    MealTemplate newMeal = new MealTemplate();
                    newMeal.recipe = new Recipe();
                    newMeal.recipe.Source = new Source();
                    newMeal.recipe.chai = new Chai();
                    JObject data, recipeData;
                    try { data = (JObject)jResponse.GetValue("data"); } catch { data = null; }
                    if (data != null)
                    {
                        newMeal.id = (int)data.GetValue("id");
                        newMeal.mealType = data.GetValue("meal_type").ToString();
                        recipeData = (JObject)data.GetValue("recipe");
                        newMeal.recipe.Id = recipeData.GetValue("whisk_recipe_id").ToString();
                        newMeal.recipe.WhiskRecipeId = recipeData.GetValue("whisk_recipe_id").ToString();
                        newMeal.whiskID = recipeData.GetValue("whisk_recipe_id").ToString();
                        newMeal.recipe.name = recipeData.GetValue("name").ToString();
                        //newMeal.Recipe.Images = recipeData.GetValue("images").ToObject<Image[]>();
                        //recipeSource = (JObject)recipeData.GetValue("source");
                        //newMeal.Recipe.Source.Name = recipeSource.GetValue("name").ToString();
                        //newMeal.Recipe.Source.DisplayName = recipeSource.GetValue("display_name").ToString();
                        //newMeal.Recipe.Source.SourceRecipeUrl = (Uri)recipeSource.GetValue("source_recipe_url");
                        newMeal.recipe.chai.Id = (int)recipeData.GetValue("id");
                        newMeal.recipe.chai.Name = recipeData.GetValue("name").ToString();
                        newMeal.recipe.chai.PrepTime = (long)recipeData.GetValue("prep_time");
                        newMeal.recipe.chai.CookTime = (long)recipeData.GetValue("cook_time");
                        newMeal.recipe.chai.TotalTime = (long)recipeData.GetValue("total_time");
                        newMeal.recipe.chai.Method = recipeData.GetValue("method").ToString();
                        newMeal.recipe.MainImageSource = image;
                        return newMeal;
                    }
                    else
                    {
                        // Failed to get the data
                        return null;
                    }
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR");
            }
            return null;
        }

        public async Task<MealTemplate> UpdateMealOnDayTemplate(User user, int dayTemplateId, int mealTemplateId, string mealType, int recipeId, string whiskId, string image)
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);

            try
            {
                string uri = ApiRootUrl + "/user/day-templates/" + dayTemplateId + "/meal-templates/" + mealTemplateId;

                JObject jObject = new JObject();
                jObject.Add("meal_type", mealType);
                jObject.Add("recipe_id", recipeId);
                jObject.Add("whisk_recipe_id", whiskId);

                string jsonString = jObject.ToString();
                jsonString = TextTools.CleanUpJson(jsonString);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = HttpClient.PutAsync(uri, content).Result;
                var json = await result.Content.ReadAsStringAsync();
                var jResponse = JObject.Parse(json);

                if (result.IsSuccessStatusCode)
                {
                    MealTemplate newMeal = new MealTemplate();
                    newMeal.recipe = new Recipe();
                    newMeal.recipe.Source = new Source();
                    newMeal.recipe.chai = new Chai();
                    JObject data, recipeData;
                    try { data = (JObject)jResponse.GetValue("data"); } catch { data = null; }
                    if (data != null)
                    {
                        newMeal.id = (int)data.GetValue("id");
                        newMeal.mealType = data.GetValue("meal_type").ToString();
                        recipeData = (JObject)data.GetValue("recipe");
                        newMeal.recipe.Id = recipeData.GetValue("whisk_recipe_id").ToString();
                        newMeal.whiskID = recipeData.GetValue("whisk_recipe_id").ToString();
                        newMeal.recipe.WhiskRecipeId = recipeData.GetValue("whisk_recipe_id").ToString();
                        newMeal.recipe.name = recipeData.GetValue("name").ToString();
                        //newMeal.Recipe.Images = recipeData.GetValue("images").ToObject<Image[]>();
                        //recipeSource = (JObject)recipeData.GetValue("source");
                        //newMeal.Recipe.Source.Name = recipeSource.GetValue("name").ToString();
                        //newMeal.Recipe.Source.DisplayName = recipeSource.GetValue("display_name").ToString();
                        //newMeal.Recipe.Source.SourceRecipeUrl = (Uri)recipeSource.GetValue("source_recipe_url");
                        newMeal.recipe.chai.Id = (int)recipeData.GetValue("id");
                        newMeal.recipe.chai.Name = recipeData.GetValue("name").ToString();
                        newMeal.recipe.chai.PrepTime = (long)recipeData.GetValue("prep_time");
                        newMeal.recipe.chai.CookTime = (long)recipeData.GetValue("cook_time");
                        newMeal.recipe.chai.TotalTime = (long)recipeData.GetValue("total_time");
                        newMeal.recipe.chai.Method = recipeData.GetValue("method").ToString();
                        newMeal.recipe.MainImageSource = image;
                        return newMeal;
                    }
                    else
                    {
                        // Failed to get the data
                    }
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR");
            }
            return null;
        }

        public async Task<bool> DeleteMealOnDayTemplate(User user, int dayTemplateId, int mealTemplateId)
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);

            try
            {
                string uri = ApiRootUrl + "/user/day-templates/" + dayTemplateId + "/meal-templates/" + mealTemplateId;

                var result = HttpClient.DeleteAsync(uri).Result;
                var json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    BadResponseHandler(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CONNECTION ERROR");
            }
            return false;
        }

        public async Task<bool> RegenerateMeals(User user)
        {
            await Task.Delay(10).ConfigureAwait(false);
            try
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                string uri = $"{ApiRootUrl}/meal-plans/regenerate";

                Console.WriteLine(uri);

                var result = HttpClient.PostAsync(uri, null).Result;
                //var result = HttpClient.GetAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {

                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);
                    return true;
                }
                else
                {
                    BadResponseHandler(result, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
            return false;
        }

        public async Task<string> GetMealPlanFromDate(User user, string date)
        {
            try
            { 
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthToken);
                string uri = $"{ApiRootUrl}/calendar/meal-plan/{date}";

                var result = HttpClient.GetAsync(uri).Result;

                if (result.IsSuccessStatusCode)
                {

                    var json = await result.Content.ReadAsStringAsync();
                    var jResponse = JObject.Parse(json);

                    JObject data = (JObject)jResponse.GetValue("data");
                    int id = (int)data.GetValue("id");

                    return id.ToString();
                }
                else
                {
                    BadResponseHandler(result, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
            return null;
        }
    }
}
