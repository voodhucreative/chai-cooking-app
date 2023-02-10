using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ChaiCooking.DebugData.Custom;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.AlbumAPI;
using ChaiCooking.Models.Custom.Feed;
using ChaiCooking.Models.Custom.InfluencerAPI;
using ChaiCooking.Models.Custom.MealPlanAPI;
using ChaiCooking.Services;
using ChaiCooking.Services.Storage;
using ChaiCooking.Views.CollectionViews;
using ChaiCooking.Views.CollectionViews.IngredientFilter;
using ChaiCooking.Views.CollectionViews.MealPlanner;
using CsvHelper;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using static ChaiCooking.Models.Custom.Week;

namespace ChaiCooking.AppData
{
    public class StaticData
    {
        public static bool forceLogin = true;

        public string authResult;
        public static int selectedInfluencerId { get; set; }
        public static Month monthlyMealPlan { get; set; }
        public static MealPlanModel previewMealPlan { get; set; }
        public static int selectedPreviewMealPlanID { get; set; }
        public static int chosenWeeks { get; set; }
        public static int chosenDays { get; set; }
        public static int chosenPrevWeeks { get; set; }
        public static int currentTemplateID { get; set; }
        public static string currentTemplateName { get; set; }
        //public static List<Recipe> favouriteRecipes { get; set; }
        public static List<Recipe> holdingAreaRecipes { get; set; }
        public static Recipe selectedRecipe { get; set; }
        public static bool authKeyExists { get; set; }
        public static bool loginSuccess { get; set; }
        public static int calendarMealPlanID { get; set; }
        public static int calendarMealPlanWeeks { get; set; }
        public static string calendarMealPlanName { get; set; }
        public static bool isCreating { get; set; }
        public static bool isTemplate { get; set; }
        public static string startDate { get; set; }
        public static string hypenedDate { get; set; }
        public static string errorText { get; set; }
        public static string createErrorText { get; set; }
        public static RecipeFeed recipeFeed { get; set; }
        public static string whiskRecipeID { get; set; }
        public static string chaiRecipeID { get; set; }
        public static Action buildPage { get; set; }
        public static Action<Recipe> setPreviewRecipe { get; set; }
        public static Action<string, int> callTemplateView { get; set; }
        public static bool mealPlanClicked { get; set; }
        public static bool calendarClicked { get; set; }
        public static bool recipeEditorClicked { get; set; }
        public static bool isTemplateView { get; set; }
        public static int basketClickedCount { get; set; }
        public static int mealPlanToDelete { get; set; }
        public static int templateToDelete { get; set; }
        public static UserMealPlans userMealPlans { get; set; }
        public static InfluencerMealPlans mealTemplates { get; set; }
        public static bool favouritesClicked { get; set; }
        public static bool albumsClicked { get; set; }
        public static bool collectionsClicked { get; set; }
        public static ColourTileModel colourModel { get; set; }
        public static string selectedColour { get; set; }
        public static Album selectedAlbum { get; set; }
        public static bool isEditing { get; set; }
        public static bool tilePressed { get; set; }
        public static StoredInfluencerMealPlan storedInfluencerMealPlan { get; set; }
        public static bool previewPlanRefresh { get; set; }
        public static List<TemplateDays> daysList { get; set; }

        public enum WeekeNum
        {
            week1,
            week2,
            week3,
            week4
        }
        public static WeekeNum week = WeekeNum.week1;
        public static WeekeNum weekPrev = WeekeNum.week1;

        public static async void Init()
        {
            selectedAlbum = new Album();

            monthlyMealPlan = JsonConvert.DeserializeObject<Month>(ReadJsonFile("mealplan.json"));
            monthlyMealPlan.week1 = JsonConvert.DeserializeObject<Week>(ReadJsonFile("mealplanweek1.json"));
            monthlyMealPlan.week2 = JsonConvert.DeserializeObject<Week>(ReadJsonFile("mealplanweek2.json"));
            monthlyMealPlan.week3 = JsonConvert.DeserializeObject<Week>(ReadJsonFile("mealplanweek3.json"));
            monthlyMealPlan.week4 = JsonConvert.DeserializeObject<Week>(ReadJsonFile("mealplanweek4.json"));

            previewMealPlan = FakeData.previewMealPlan;
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

        public static async void WhiskAuthenticate()
        {
            try
            {
                await App.DisplayAlert("CHAI is powered by Whisk", "Please create a Whisk / CHAI user for the nutritional benefits of meal planning.", "Ok");

                WebAuthenticatorResult authResult = await WebAuthenticator.AuthenticateAsync(new Uri("https://app.chai.cooking/whisk/authorize"), new Uri("chai://app.chai.cooking/get-access-token"));

                var accessToken = authResult?.Properties["code"];

                if (accessToken.Contains("#_=_"))
                {
                    accessToken = accessToken.Replace("#_=_", "");
                }

                if (accessToken.Contains("#"))
                {
                    accessToken = accessToken.Replace("#", "");
                }

                bool tokenSent = false;
                if (accessToken.Length > 0)
                {
                    AppSession.CurrentWhiskToken = accessToken;
                }

                tokenSent = await DataManager.Authorize(AppSession.CurrentWhiskToken);


                if (tokenSent)
                {
                    Console.WriteLine("Sent code to API");
                    AppSession.AuthorizeTimerRunning = false;
                    //App.Current.Properties["whiskToken"] = "authorized";
                    AppSession.CurrentUser.IsAuthorised = true;
                    //App.Current.Properties["whiskTokenID"] = AppSession.CurrentUser.WhiskTokenId;
                    //await App.Current.SavePropertiesAsync();

                    AppSession.CurrentUser.AuthToken = AppSession.CurrentWhiskToken;
                    //AppSession.CurrentUser.IsRegistered = true;
                    LocalDataStore.SaveAll();
                    await Task.Run(() =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            Console.WriteLine("BeginInvokeOnMainThread");
                            await App.GoToLoginOrRegister();

                            //await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.LoginAndRegistration);
                        });
                    });
                }
                else
                {
                    Console.WriteLine("Code not sent to API");
                    App.Current.Properties["whiskToken"] = "unauthorized";
                    await App.Current.SavePropertiesAsync();
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        // Code to run on the main thread
                        App.ShowAlert($"Successfully extracted the Whisk access token: {accessToken} but not sent to our API!");
                    });

                    AppSession.AuthorizeTimerRunning = false;
                    AppSession.CurrentUser.AuthToken = null;
                    //AppSession.CurrentUser.IsRegistered = false;
                    LocalDataStore.SaveAll();
                }
            }
            catch (Exception e)
            {
                //AppSession.CurrentUser.
                //App.Current.Properties["whiskToken"] = null;// "unauthorized";

                await App.ApiBridge.LogOut(AppSession.CurrentUser, true);
                await App.Current.SavePropertiesAsync();
                Console.WriteLine($"Failed: {e.Message}");
                // Prevent crash if user declines to authenticate
                try
                {
                    //App.ShowAlert("Failed to authenticate.\n\n" + e.Message);
                }
                catch (Exception e2)
                {
                    Console.WriteLine($"Error: {e2}");
                }

                //await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.LoginAndRegistration);

            }
        }

        /*
        public static StackLayout BuildEmpty()
        {
            StackLayout emptyCont = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                Children =
                    {
                        new Label
                        {
                            Text = "No recipes in basket.",
                            FontSize = Units.FontSizeL,
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Color.White,
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                    }
            };

            return emptyCont;
        }
        */

        public static async Task<bool> UpdateMealTemplate()
        {
            //var temp = await App.ApiBridge.GetUserMealPlanDayTemplates(AppSession.CurrentUser, StaticData.currentTemplateID);
            //AppSession.mealTemplate = new MealPlanModel();
            //AppSession.mealTemplate.Data = new List<MealPlanModel.Datum>();

            //for (int i = 0; i < 28; i++)
            //{
            //    MealPlanModel.Datum datum = new MealPlanModel.Datum();
            //    datum.day_Number = i + 1;
            //    datum.id = -1;
            //    datum.mealTemplates = new List<MealTemplate>();
            //    AppSession.mealTemplate.Data.Add(datum);
            //}

            //foreach (MealPlanModel.Datum d in AppSession.mealTemplate.Data)
            //{
            //    var match = temp.Data.FirstOrDefault(x => x.day_Number == d.day_Number);
            //    try
            //    {
            //        d.day_Number = match.day_Number;
            //        d.id = match.id;
            //        d.mealTemplates = match.mealTemplates;
            //    }
            //    catch
            //    {
            //    }
            //}

            AppSession.mealTemplate = await App.ApiBridge.GetUserMealPlanDayTemplates(AppSession.CurrentUser, StaticData.currentTemplateID);
            var mealPlannerGroup = new MealPlannerCollectionViewSection(AppSession.mealTemplate.Data);
            AppSession.mealPlannerCollection.Add(mealPlannerGroup);
            AppSession.mealPlannerCollection.RemoveAt(0);

            return true;
        }

    }
}
