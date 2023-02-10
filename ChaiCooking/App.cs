using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.DebugData.Custom;
using ChaiCooking.Helpers;
using ChaiCooking.Services;
using ChaiCooking.Services.Api;
using ChaiCooking.Services.Storage;
using Newtonsoft.Json;
using Xamarin.Forms;
using Plugin.DeviceInfo;
using ChaiCooking.Models;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.MealPlanAPI;
using Action = System.Action;
using ChaiCooking.Components.Images;
using static ChaiCooking.Helpers.Custom.Accounts;
using ChaiCooking.Models.Custom.InfluencerAPI;
using Plugin.Connectivity;
using ChaiCooking.Pages;
using ChaiCooking.Models.Custom.AlbumAPI;

namespace ChaiCooking
{
    public class App : Application
    {
        static AppContainter AppContainer;
        public static IApiBridge ApiBridge;

        public static bool IsBusy { get; set; }

        public App(int width, int height, int pixelWidth, int pixelHeight, float scale, int statusBarHeight)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                MainPage = new ErrorPage("Chai requires an internet connection. Please check your connection and try again.");
                return;
            }

            try
            {
                ApiBridge = new ApiBridge(); // use this for live api
                ApiBridge.Init();
                ColourTileModel colourTileModel = ApiBridge.ListAlbumColours().Result;
                Console.WriteLine("Ok");
            }
            catch (Exception e)
            {
                MainPage = new ErrorPage("Failed to initialise API. Please try again later.");
                return;
            }

            ApiBridge = new ApiBridge(); // use this for live api
            ApiBridge.Init();

            Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(2000);
            });

            // set up UI units
            Units.Init(width, height, pixelWidth, pixelHeight, scale, statusBarHeight);


            // initialise accounts
            Accounts.Init();

            // initialise fonts
            Fonts.Init();

            // initialise debug data
            FakeData.Init(); // unused for now

            // initialise static data (data loaded from assets contained within the app - data, text, etc)
            StaticData.Init();

            // initialise static data (data loaded from assets contained within the app - data, text, etc)
            AppDataContent.Init(); // stick this in StaticData and remove..

            // initialise local storage
            LocalDataStore.Init();

            // initialise actions
            Actions.Init();


            Units.Init(width, height, scale, statusBarHeight);

            // initialise pages
            Helpers.Pages.Init();

            // initialise the app session
            AppSession.Init();

            if (LocalDataStore.LoadUser() != null)
            {
                AppSession.CurrentUser = LocalDataStore.LoadUser();

                try
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (await ApiBridge.LoginUser(AppSession.CurrentUser))
                        {
                            
                            /*UserPreferences prefs = null;
                            try
                            {
                                prefs = await App.ApiBridge.GetPreferences(AppSession.CurrentUser);
                                AppSession.CurrentUser.Preferences = prefs;
                            }
                            catch (Exception e)
                            {

                            }*/
                            
                            if (await SubscriptionManager.CanAccessPaidContent(AppSession.CurrentUser))
                            {
                                Console.WriteLine("Has active plan");
                                //AccountType accountType = await App.ApiBridge.GetCurrentSubscriptionPlan(AppSession.CurrentUser).ConfigureAwait(false);
                                //await App.SetUserAccountType(accountType);
                            }
                            else
                            {
                                Console.WriteLine("Does not have active plan");
                                await App.SetUserAccountType(Accounts.AccountType.ChaiFree);

                            }


                            string alertPrefix = "Welcome back ";
                            if (AppSession.CurrentUser.FirstName == "Chai")
                            {
                                alertPrefix += "to ";
                            }
                            try
                            {
                                ShowAlert(alertPrefix + AppSession.CurrentUser.FirstName);
                            }
                            catch (Exception e) { }
                        }
                        else
                        {

                        }
                    });

                }
                catch (Exception e)
                {
                    Console.WriteLine("oh dear");
                }
            }
            else
            {
                //AppSession.CurrentUser = new Models.User();
                AppSession.CurrentUser = DataManager.GetCurrentUser();
            }


            // create app container
            AppContainer = new AppContainter(width, height);

            //Device.StartTimer(TimeSpan.FromMilliseconds(25), Update);

            DependencyService.Get<IStatusBarStyleManager>().SetDarkTheme();

            //AppContainer.BackgroundColor = Color.Red;
            /*
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            if (Device.RuntimePlatform == Device.iOS)
            {
                if (CrossDeviceInfo.Current.VersionNumber.Major <= 10)
                {
                    // SetUseSafeArea does not work on iOS 10 and below: set page padding
                    this.Padding = new Thickness(0, 16, 0, 0);
                }
            }
            */

            // meal plan template tests
            //InfluencerMealPlans templateTest1 = ApiBridge.GetUserMealPlanTemplates(AppSession.CurrentUser).Result;
            //ApiBridge.CreateUserMealPlanTemplates(AppSession.CurrentUser, "test", 7);
            //ApiBridge.UpdateUserMealPlanTemplates(AppSession.CurrentUser, 9, "hello");
            //ApiBridge.DeleteUserMealPlanTemplates(AppSession.CurrentUser, 9);
            //ApiBridge.CreateUserMealPlanTemplates(AppSession.CurrentUser, "great!", 14);
            //templated = ApiBridge.GetUserMealPlanTemplates(AppSession.CurrentUser).Result;
            //InfluencerMealPlans templatetest2 = ApiBridge.GetUserMealPlanDayTemplates(AppSession.CurrentUser, 10).Result;
            //ApiBridge.CreateDayTemplateOnMealPlan(AppSession.CurrentUser, 10, 18);
            //ApiBridge.DeleteDayTemplateOnMealPlan(AppSession.CurrentUser, 10, 65);
            //InfluencerMealPlans templatetest3 = ApiBridge.GetUserMealPlanDayTemplates(AppSession.CurrentUser, 10).Result;
            //ApiBridge.AddMealToDayTemplate(AppSession.CurrentUser, 63, "dinner", 8672, "101821b32e5e3a4cf19ad4679432f2b200eeb421f29");
            //ApiBridge.UpdateMealOnDayTemplate(AppSession.CurrentUser, 63, 105, "dinner", 9772, "10196142213824fe6f29c48e4fb807cd9996539519c");
            //InfluencerMealPlans templatetest4 = ApiBridge.GetUserMealPlanDayTemplates(AppSession.CurrentUser, 10).Result;
            //ApiBridge.DeleteMealOnDayTemplate(AppSession.CurrentUser, 63, 105);
            //InfluencerMealPlans templatetes5 = ApiBridge.GetUserMealPlanDayTemplates(AppSession.CurrentUser, 10).Result;

            MainPage = AppContainer;

            
        }

        public static async Task<bool> PurchaseSubscription(int duration)
        {
            if (await IAPManager.WasItemPurchased(duration))
            {
                return await Subscribe();
            }
            else
            {
                AppContainer.HideSubscribeModal();
            try
            {
                if (await IAPManager.MakePurchase(duration))
                {
                      return await Subscribe();
                }
                else
                {
                    App.HideSubscribeModal();
                }
            }
            catch
            {
                // Crash Prevention
            }
            return false;
            }
        }

        private static async Task<bool> Subscribe()
        {
            if (await App.ApiBridge.CreateChaiSubscriptionPlan(AppSession.CurrentUser))
            {
                await App.ApiBridge.UpdatePreferences(AppSession.CurrentUser, true);


                ShowAlert("Thanks!" + AppSession.CurrentUser.FirstName, "You are now subscribed and on the " + AppSession.CurrentUser.GetLongPlanName() + ". " + SubscriptionManager.GetAccountInfoText(AppSession.CurrentUser));

                //ShowInfoPanel("Thanks " + AppSession.CurrentUser.FirstName + "!\nYou're now on the following plan: " + AppSession.CurrentUser.GetLongPlanName() + "\n" + SubscriptionManager.GetAccountInfoText(AppSession.CurrentUser));

                //Ignore this if transitioning
                //if (AppSession.CurrentUser.GetPlanName() != "transitioning")
                //{
                //    switch (AppSession.SubscriptionTargetPage)
                //    {
                //        case (int)AppSettings.PageNames.HealthyLiving:
                //            await App.GoToHealthyLiving();
                //            break;
                //        case (int)AppSettings.PageNames.MealPlan:
                //            await App.GoToMealPlanner();
                //            break;
                //        case (int)AppSettings.PageNames.MPCalendar:
                //        case (int)AppSettings.PageNames.Calendar:
                //            await App.GoToCalendar();
                //            break;
                //    }
                //}
                //else
                //{
                    await App.GoToCalendar();
                //}
                return true;
            }
            Console.WriteLine("Subscription Error");

            return false;
        }

        public static async Task<bool> SetUserAccountType(AccountType accountType)
        {
            AppSession.CurrentUser.SetAccountType(accountType);
            AppSession.CurrentUser.Preferences.AccountType = accountType;//

            if (await DataManager.UpdateUserPrefs(accountType, false))
            {
                //App.ShowAlert("SUCCESS", "ACCOUNT TYPE UPDATED TO " + Accounts.GetAccountName(AppSession.CurrentUser.Preferences.AccountType));
                await AppContainer.UpdateQuestionnaire();
                //await App.GoToMealPlanner();
                return true;
            }
            else
            {
                await ApiBridge.LogOut(AppSession.CurrentUser, true);
                return false;
            }
        }

        public static async Task<bool> UpdateQuestionnaire()
        {
            //await AppContainer.UpdatePage((int)AppSettings.PageNames.Questionnaire);
            //await AppContainer.UpdateDietTypes();
            await AppContainer.UpdateQuestionnaire();
            return true;
        }

        public static void SetScalables(int scale)
        {
            Units.SetScalableUnits(scale);
        }

        public static void SetPixelSize(int pixelWidth, int pixelHeight)
        {
            Units.SetPixelSize(pixelWidth, pixelHeight);
        }

        public static async Task PerformActionAsync(Models.Action action)
        {
            if (IsBusy)
            {
                return;
            }


            // this action takes us to another page
            if (action.Id == (int)Actions.ActionName.GoToPage && action.TargetPageId > -1)
            {
                AppContainer.SetNextPage(action.TargetPageId);
            }

            await Execute(action.Id).ConfigureAwait(false);
        }

        public static async Task PerformActionAsync(int actionId, int targetPageId)
        {
            if (IsBusy)
            {
                return;
            }

            // this action takes us to another page
            if (actionId == (int)Actions.ActionName.GoToPage && targetPageId > -1)
            {
                AppContainer.SetNextPage(targetPageId);
            }

            if (actionId == (int)Actions.ActionName.GoToNextPage && targetPageId > -1)
            {
                AppContainer.SetNextPage(targetPageId);
            }

            await Execute(actionId).ConfigureAwait(false);
        }


        public static async Task<bool> AuthCheck(Models.Action action)
        {
            if (!AppSettings.FreeVersion)
            {
                if (AppSession.CurrentUser.IsRegistered)
                {
                    if (AppSession.CurrentUser.AuthToken != null)
                    {
                        await App.PerformActionAsync(action);
                    }
                    else
                    {
                        await ApiBridge.LogOut(AppSession.CurrentUser, true);
                        //await GoToLoginOrRegister();
                        //await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.LoginAndRegistration);
                    }
                }
                else
                {
                    // go to whisk to kick off the registration process
                    StaticData.WhiskAuthenticate();
                }
            }
            else
            {
                //not free can't access
                App.ShowAlert("COMING SOON", "COMING SOON!!\n\n");
            }
            return false;
        }

        public static async Task<bool> AuthCheck(int actionId, int pageId)
        {
            if (!AppSettings.FreeVersion)
            {
                if (AppSession.CurrentUser.IsRegistered)
                {
                    if (AppSession.CurrentUser.AuthToken != null)
                    {
                        await App.PerformActionAsync(actionId, pageId);
                    }
                    else
                    {
                        await ApiBridge.LogOut(AppSession.CurrentUser, true);
                        //await GoToLoginOrRegister();
                        //await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.LoginAndRegistration);
                    }
                }
                else
                {
                    // go to whisk to kick off the registration process
                    StaticData.WhiskAuthenticate();
                }
            }
            else
            {
                //not free can't access
                App.ShowAlert("COMING SOON", "COMING SOON!!\n\n");
            }
            return false;
        }

        public static async Task<bool> DeleteUnsavedIngredient(Ingredient ingredient)
        {
            return await AppContainer.DeleteUnsavedIngredient(ingredient);
        }

        public static async Task<bool> AuthCheck(int actionId, int pageId, bool performAction)
        {
            if (!AppSettings.FreeVersion)
            {
                if (AppSession.CurrentUser.IsRegistered)
                {
                    if (AppSession.CurrentUser.AuthToken != null)
                    {
                        if (performAction)
                        {
                            await App.PerformActionAsync(actionId, pageId);
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (performAction)
                        {
                            await ApiBridge.LogOut(AppSession.CurrentUser, true);
                            //await GoToLoginOrRegister();
                            //await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.LoginAndRegistration);
                        }
                        return true;
                    }
                }
                else
                {
                    // go to whisk to kick off the registration process
                    StaticData.WhiskAuthenticate();
                }
            }
            else
            {
                //not free can't access
                App.ShowAlert("COMING SOON", "COMING SOON!!\n\n");
            }
            return false;
        }



        public static async Task<bool> GoToLoginOrRegister()
        {
            string userWhiskToken = ""+AppSession.CurrentUser.WhiskTokenId;

            if (!AppSettings.AutoChaiSignupAndLogin)
            {
                await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.LoginAndRegistration);
            }
            else
            {
                //if (AppSession.CurrentUser.IsRegistered)
                //{
                    if (await App.ApiBridge.LoginUser(AppSession.CurrentUser))
                    {
                        LocalDataStore.SaveAll();
                        AppSession.CurrentUser.IsRegistered = true;

                        //var result = await App.ApiBridge.SwapWhiskToken(AppSession.CurrentUser);

                        // get saved user preferences here
                        /*UserPreferences userPrefs = await App.ApiBridge.GetPreferences(AppSession.CurrentUser);

                        if (userPrefs != null)
                        {
                            AppSession.CurrentUser.Preferences = userPrefs;
                        }
                        */

                        try
                        {
                            var result = await App.ApiBridge.SwapWhiskToken(AppSession.CurrentUser);
                        }
                        catch (Exception e)
                        {
                            await App.ApiBridge.LogOut(AppSession.CurrentUser, true);
                        }

                        LocalDataStore.SaveAll();
                        await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Landing);
                    }
                    else
                    {
                        Console.WriteLine("Login failed");
                        //await App.PerformActionAsync(new Models.Action((int)Actions.ActionName.ShowAccountConfirmationModal));
                    }
                //}
                //else
                //{
                //    await App.PerformActionAsync(new Models.Action((int)Actions.ActionName.ShowAccountConfirmationModal));
                //}
            }
            return true;
        }


        public static async Task<bool> PerformRecipeSearch(string searchString)
        {
            if (searchString != null && searchString != "")
            {
                AppSession.SearchKeywords.Clear();

                string[] words = searchString.Split(' ');

                foreach (string word in words)
                {
                    AppSession.SearchKeywords.Add(word);
                }

                AppSession.CurrentPageSearch = 1;

                //await AppContainer.GoToPage((int)AppSettings.PageNames.SearchResults);
                //await AppContainer.UpdatePage((int)AppSettings.PageNames.SearchResults);

                await Task.Delay(10);
                await AppContainer.HideMenu();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task ShowMenu()
        {
            await Task.Delay(10);
            await AppContainer.ShowMenu();
        }

        public static async Task CloseMenu()
        {
            await Task.Delay(10);
            await AppContainer.HideMenu();
        }

        public static async Task ToggleMenu()
        {
            await Task.Delay(10);
            await AppContainer.ToggleMenu();
        }

        public static void ShowMenuButton()
        {
            AppContainer.ShowMenuButton();
        }



        public static void HideMenuButton()
        {
            AppContainer.HideMenuButton();
        }

        public static async void ShowHeaderAsync()
        {
            await AppContainer.ShowHeader();
        }

        public static async void HideHeaderAsync()
        {
            await AppContainer.HideHeader();
        }

        public static async void ShowSubHeader()
        {
            await AppContainer.ShowSubHeader();
        }

        public static async void HideSubHeader()
        {
            await AppContainer.HideSubHeader();
        }

        public static async void ShowNavHeader()
        {
            await AppContainer.ShowNavHeader();
        }

        public static async void HideNavHeader()
        {
            await AppContainer.HideNavHeader();
        }

        public static void ClearNavHeader()
        {

        }

        public static void SetNavHeaderContent(View view)
        {
            AppContainer.SetNavHeaderContent(view);
        }

        public static async void ShowFooter()
        {
            await AppContainer.ShowFooter();
        }

        public static async void HideFooter()
        {
            await AppContainer.HideFooter();
        }

        public static void ShowBackgroundImage()
        {
            AppContainer.ShowBackgroundImage();
        }

        public static void HideBackGroundImage()
        {
            AppContainer.HideBackGroundImage();
        }

        public static void SetBackgroundImage(string imageSource)
        {
            AppContainer.SetBackgroundImage(imageSource);
        }

        public static void SetBackgroundColor(Color color)
        {
            AppContainer.SetBackgroundColor(color);
        }

        public static async Task GoToMealPlanner()
        {
            AppSession.InfoModeOn = false;
            await App.CloseMenu();
            App.SwitchSubHeaderToMainMode();

            StaticData.mealPlanClicked = true;
            StaticData.isCreating = false;

            if (await App.AuthCheck((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.MPCalendar))
            {
                Console.WriteLine("Cool, we're authorized");
            }
        }


        public static async Task GoToHealthyLiving()
        {
            await App.CloseMenu();
            App.SwitchSubHeaderToMainMode();

            if (await App.AuthCheck((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.HealthyLiving))
            {
                Console.WriteLine("Cool, we're authorized");
            }
        }

        public static async Task GoToCalendar()
        {
            await App.CloseMenu();
            App.SwitchSubHeaderToMainMode();

            if (await App.AuthCheck((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Calendar))
            {
                Console.WriteLine("Cool, we're authorized");
            }
        }

        public static async Task GoToCollections()
        {
            await App.CloseMenu();
            App.SwitchSubHeaderToMainMode();

            if (await App.AuthCheck((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Collections))
            {
                Console.WriteLine("Cool, we're authorized");
            }
        }




        public static async void ShowSingleRecipe()
        {
            AppSession.SingleRecipeMode = true;
            /*new System.Threading.Thread(new System.Threading.ThreadStart(() =>
            {
                _ = AppContainer.UpdatePage((int)AppSettings.PageNames.RecommendedRecipes);
            })).Start();*/
            await AppContainer.UpdatePage();
        }

        public static async void ShowRecipeList()
        {
            AppSession.SingleRecipeMode = false;
            /*new System.Threading.Thread(new System.Threading.ThreadStart(() =>
            {
                _ = AppContainer.UpdatePage((int)AppSettings.PageNames.RecommendedRecipes);
            })).Start();*/
            await AppContainer.UpdatePage();

        }

        public static async void UpdateUserDietType(Preference pref)
        {
            AppSession.CurrentUser.Preferences.DietTypes.Clear();
            AppSession.CurrentUser.Preferences.DietTypes.Add(pref);
            await AppContainer.UpdateDietTypes();
            //await AppContainer.UpdatePage((int)AppSettings.PageNames.Questionnaire);
        }

        public static void SwitchSubHeaderToCloseMode()
        {
            AppContainer.SwitchSubHeaderToCloseMode();
        }

        public static void SwitchSubHeaderToMainMode()
        {
            AppContainer.SwitchSubHeaderToMainMode();
        }

        public static void ForceSubHeaderUpdate()
        {
            AppContainer.ForceSubHeaderUpdate();
        }

        public static void SetSubHeaderTitle(string title, Models.Action action)
        {
            AppContainer.SetSubHeaderTitle(title, action);
        }

        public static void SetSubHeaderTitleWithAction(string title, Action action)
        {
            AppContainer.SetSubHeaderTitleWithAction(title, action);
        }

        public static void SetSubHeaderDescription(string description)
        {
            AppContainer.SetSubHeaderDescription(description);
        }

        public static void ClearSubHeaderTitle()
        {
            AppContainer.ClearSubHeaderTitle();
        }

        public static void ShowInfoBubble(View view, int x, int y)
        {
            AppContainer.ShowInfoBubble(view, x, y);
        }

        public static void ShowInfoBubble(View view, string info)
        {
           ShowInfoBubble(new Label { Text = info }, (int)Tools.Screen.GetScreenCoordinates(view).X, (int)Tools.Screen.GetScreenCoordinates(view).Y);
        }

        public static void ShowCurrentPageInfo()
        {
            AppContainer.ShowCurrentPageInfo();
        }

        public static void HideInfoBubble()
        {
            AppContainer.HideInfoBubble();
        }

        public static void SetCurrentPageTitle(string title, string imageSrc)
        {

        }

        /*
        public static void ShowMenuInfoBubble(View view, int x, int y)
        {
            AppContainer.ShowMenuInfoBubble(view, x, y);
        }*/

        public static void ShowMenuInfoBubble(View info, View parent)
        {
            double x = Tools.Screen.GetScreenCoordinates(parent).X;
            double y = Tools.Screen.GetScreenCoordinates(parent).Y;
            AppContainer.ShowMenuInfoBubble(info, (int)(x + parent.Width / 2), (int)y);
        }

        public static void HideMenuInfoBubble()
        {
            AppContainer.HideMenuInfoBubble();
        }

        public static void ScaleUpBackground()
        {
            AppContainer.ScaleUpBackground();
        }

        public static void ScaleDownBackground()
        {
            AppContainer.ScaleDownBackground();
        }

        public static void ResetScaleBackground()
        {
            AppContainer.ResetScaleBackground();
        }
        public static async Task<bool> RefreshData(int pageId, int dataRequestId)
        {
            await Task.Delay(10);

            return true;
        }


        private static async Task<bool> Execute(int actionId)
        {
            if (IsBusy)
            {
                return false;
            }
            IsBusy = true;

            await Task.Delay(10);

            Console.WriteLine("Performing action: " + actionId);

            switch (actionId)
            {
                case (int)Actions.ActionName.GoToPage:
                    Helpers.Pages.TransitionAction = (int)Helpers.Pages.TransitionActions.Direct;
                    await CloseMenu();
                    await AppContainer.GoToNextPage();
                    //AppContainer.ForceSubHeaderUpdate();
                    break;
                case (int)Actions.ActionName.GoToNextPage:
                    await CloseMenu();
                    await AppContainer.GoToNextPage();
                    break;
                case (int)Actions.ActionName.GoToLastPage:
                    await CloseMenu();
                    await AppContainer.GoToLastPage();
                    break;
                case (int)Actions.ActionName.QuitApp:
                    break;
                case (int)Actions.ActionName.ShowLoading:
                    await ShowLoading();
                    break;
                case (int)Actions.ActionName.HideLoading:
                    await HideLoading();
                    break;
                case (int)Actions.ActionName.ShowMenu:
                    await AppContainer.ShowMenu();
                    break;
                case (int)Actions.ActionName.HideMenu:
                    await AppContainer.HideMenu();
                    break;
                case (int)Actions.ActionName.ToggleMenu:
                    await AppContainer.ToggleMenu();
                    break;
                case (int)Actions.ActionName.ShowHeader:
                    await AppContainer.ShowHeader();
                    break;
                case (int)Actions.ActionName.HideHeader:
                    await AppContainer.HideHeader();
                    break;
                case (int)Actions.ActionName.ToggleHeader:
                    await AppContainer.ToggleHeader();
                    break;
                case (int)Actions.ActionName.ShowFooter:
                    await AppContainer.ShowFooter();
                    break;
                case (int)Actions.ActionName.HideFooter:
                    await AppContainer.HideFooter();
                    break;
                case (int)Actions.ActionName.ToggleFooter:
                    await AppContainer.ToggleFooter();
                    break;
                case (int)Actions.ActionName.ShowSubHeader:
                    await AppContainer.ShowSubHeader();
                    break;
                case (int)Actions.ActionName.HideSubHeader:
                    await AppContainer.HideSubHeader();
                    break;
                case (int)Actions.ActionName.ToggleSubHeader:
                    await AppContainer.ToggleSubHeader();
                    break;
                case (int)Actions.ActionName.ShowNavHeader:
                    await AppContainer.ShowNavHeader();
                    break;
                case (int)Actions.ActionName.HideNavHeader:
                    await AppContainer.HideNavHeader();
                    break;
                case (int)Actions.ActionName.ToggleNavHeader:
                    await AppContainer.ToggleNavHeader();
                    break;
                case (int)Actions.ActionName.ShowModal:
                    await AppContainer.ShowModal(null);
                    break;
                case (int)Actions.ActionName.HideModal:
                    await AppContainer.HideModal();
                    break;
                case (int)Actions.ActionName.ToggleModal:
                    await AppContainer.ToggleModal();
                    break;
                case (int)Actions.ActionName.ShowForeground:
                    await AppContainer.ShowForeground();
                    break;
                case (int)Actions.ActionName.HideForeground:
                    await AppContainer.HideForeground();
                    break;
                case (int)Actions.ActionName.ToggleForeground:
                    await AppContainer.ToggleForeground();
                    break;
                case (int)Actions.ActionName.ShowPanel:
                    await AppContainer.ShowPanel();
                    break;
                case (int)Actions.ActionName.HidePanel:
                    await AppContainer.HidePanel();
                    break;
                case (int)Actions.ActionName.TogglePanel:
                    await AppContainer.TogglePanel();
                    break;
                case (int)Actions.ActionName.LogIn:


                    //AppSession.CurrentUser.FirstName = "Mafyew";
                    //AppSession.CurrentUser.Username = "mathowlett";
                    //AppSession.CurrentUser.AvatarImageUrl = "mat.jpg";

                    if (AccountManager.LogIn(AppSession.CurrentUser.EmailAddress, AppSession.CurrentUser.Password) != null)
                    {
                        AppSession.CurrentUser.IsRegistered = true;
                        await AppContainer.GoToPage((int)AppSettings.PageNames.Landing);
                    }

                    break;
                case (int)Actions.ActionName.ShowLoginAndRegister:


                    await AppContainer.ShowLoginAndRegistration(AppSession.CurrentUser.IsRegistered);
                    break;
                case (int)Actions.ActionName.ShowCreateMealPlan:
                    await AppContainer.ShowCreateMealPlan();
                    break;
                case (int)Actions.ActionName.HideCreateMealPlan:
                    await AppContainer.HideCreateMealPlan();
                    break;
                //case (int)Actions.ActionName.ShowRecipeSummary:
                //    await AppContainer.ShowRecipeSummary(mealPeriod, string nameLabel, string prepTime, string cookingTime, string createdBy);
                //    break;
                case (int)Actions.ActionName.HideRecipeSummary:
                    await AppContainer.HideRecipeSummary();
                    break;
                case (int)Actions.ActionName.ShowForgotPassword:
                    await AppContainer.ShowForgotPassword();
                    break;
                case (int)Actions.ActionName.HideForgotPassword:
                    await AppContainer.HideForgotPassword();
                    break;
                //case (int)Actions.ActionName.ShowCalendar:
                //    await AppContainer.ShowCalendar();
                //    break;
                case (int)Actions.ActionName.ShowAccountConfirmationModal:
                    await AppContainer.ShowAccountConfirmationModal();
                    break;
                case (int)Actions.ActionName.HideAccountConfirmationModal:
                    await AppContainer.HideAccountConfirmationModal();
                    break;
                case (int)Actions.ActionName.ShowVerifyAccount:
                    await AppContainer.ShowVerifyAccount();
                    break;
                case (int)Actions.ActionName.HideVerifyAccount:
                    await AppContainer.HideVerifyAccount();
                    break;
                case (int)Actions.ActionName.ShowErrorPopup:
                    ShowAlert("Incorrect Details", "Please check the empty fields.");
                    break;
                case (int)Actions.ActionName.ShowComingSoon:
                    ShowAlert("COMING SOON");
                    break;
                case (int)Actions.ActionName.ShowRecommendedRecipeFilterPanel:
                    await AppContainer.ShowRecommendedRecipeFilterPanel();
                    break;
                case (int)Actions.ActionName.ClearPanels:
                    await AppContainer.ClearPanels();
                    break;
            }



            Console.WriteLine("Action: " + actionId + " completed");

            IsBusy = false;

            return true;
        }

        public static async Task<bool> ShowSubscribeModal()
        {
            return await AppContainer.ShowSubscribeModal();
        }

        public static async void HideSubscribeModal()
        {
            AppContainer.HideSubscribeModal();
        }

        public static async Task<bool> ShowForgotPassword()
        {
            //new Models.Action((int)Actions.ActionName.ShowCreateMealPlan));
            await AppContainer.ShowForgotPassword();
            return true;
        }

        public static async Task<bool> ShowCalendar(Influencer.Datum influencer, string planLength)
        {
            await AppContainer.ShowCalendar(influencer, planLength);
            return true;
        }

        public static async Task<bool> HideCalendar()
        {
            await AppContainer.HideCalendar();
            return true;
        }

        public static async Task<bool> ShowSelectOrCreateModal()//Action buildEmpty, Action buildPage)
        {
            await AppContainer.ShowSelectOrCreateModal();// buildEmpty, buildPage);
            return true;
        }

        public static async Task<bool> ShowRecipeOptionsModal(Recipe recipe)
        {
            await AppContainer.ShowRecipeOptionsModal(recipe);
            return true;
        }

        public static async Task<bool> ShowTemplateDayModal(bool isDeleting)
        {
            await AppContainer.ShowTemplateDayModal(isDeleting);
            return true;
        }

        public static async Task<bool> HideSelectOrCreateModal()
        {
            await AppContainer.HideSelectOrCreateModal();
            return true;
        }

        public static async Task<bool> ShowPreviewMealPlan(string input, string planLength)
        {
            await AppContainer.ShowPreviewMealPlan(input, planLength);
            return true;
        }

        public static async Task<bool> HidePreviewMealPlan()
        {
            await AppContainer.HidePreviewMealPlan();
            return true;
        }

        public static async Task<bool> ShowStartDateMenu(Influencer.Datum influencer, string planLength, string planName)
        {
            await AppContainer.ShowStartDateMenu(influencer, planLength, planName);
            return true;
        }

        public static async Task<bool> HideStartDateMenu()
        {
            await AppContainer.HideStartDateMenu();
            return true;
        }

        public static async Task<bool> ShowFullRecipe(Recipe recipe, bool reloadParent, bool reloadMealEditor)
        {
            await AppContainer.ShowFullRecipe(recipe, reloadParent, reloadMealEditor);
            return true;
        }

        public static async Task<bool> ShowFullRecipe(Recipe recipe, List<Recipe> recipeCollection)
        {
            await AppContainer.ShowFullRecipe(recipe, recipeCollection);
            return true;
        }

        public static async Task<bool> ShowFullRecipe(Recipe recipe, Action updateHeart,
            string specialConstructor)
        {
            await AppContainer.ShowFullRecipe(recipe, updateHeart: updateHeart, "");
            return true;
        }

        public static async Task<bool> RateRecipe(Recipe recipe)
        {
            await AppContainer.RateRecipe(recipe);
            return true;
        }

        public static async Task<bool> ShowRecipeSummary(Recipe recipe, string mealPeriod)
        {
            await AppContainer.ShowRecipeSummary(recipe, mealPeriod);
            return true;
        }

        public static async Task<bool> HideRecipeSummary()
        {
            await AppContainer.HideRecipeSummary();
            return true;
        }

        public static async Task<bool> HideModalAsync()
        {
            await AppContainer.HideModalAsync();
            return true;
        }

        public static async Task<bool> ShowRemoveRecipeEditorIngredient(Ingredient ingredient)
        {
            await AppContainer.ShowRemoveRecipeEditorIngredientModal(ingredient);
            return true;
        }

        public static async Task<bool> ShowRecipeEditor()
        {
            await AppContainer.ShowRecipeEditor();
            return true;
        }

        public static async Task<bool> ShowIngredientFilterModal()
        {
            await AppContainer.ShowIngredientFilterModal();
            return true;
        }

        public static async Task<bool> ShowRemoveFromBasketModal()
        {
            await AppContainer.ShowRemoveFromBasketModal();
            return true;
        }

        public static async Task<bool> ShowRemoveFromBasketModal(Recipe recipe)
        {
            await AppContainer.ShowRemoveFromBasketModal(recipe);
            return true;
        }

        //public static async Task<bool> ShowCreateAlbumModal(Action updateAlbums, Recipe r = null)
        //{
        //    await AppContainer.ShowCreateAlbumModal(updateAlbums, r);
        //    return true;
        //}

        public static async Task<bool> ShowCreateAlbumModal(Action updateAlbums, bool hasColour)
        {
            await AppContainer.ShowCreateAlbumModal(updateAlbums, hasColour);
            return true;
        }

        public static async Task<bool> ShowRemoveAlbumModal(Album album)
        {
            await AppContainer.ShowRemoveAlbumModal(album);
            return true;
        }

        public static async Task<bool> ShowRemoveAlbumModal(Recipe recipe)
        {
            await AppContainer.ShowRemoveAlbumModal(recipe);
            return true;
        }

        public static async Task<bool> ShowRemoveAlbumModal(IList<object> recipes)
        {
            await AppContainer.ShowRemoveAlbumModal(recipes);
            return true;
        }

        public static async Task<bool> ShowRemoveAlbumModal()
        {
            await AppContainer.ShowRemoveAlbumModal();
            return true;
        }

        public static async Task<bool> ShowMealHolderRemove()
        {
            await AppContainer.ShowMealHolderRemove();
            return true;
        }

        public static async Task<bool> ShowMealHolderRemove(Recipe recipe)
        {
            await AppContainer.ShowMealHolderRemove(recipe);
            return true;
        }

        public static async Task<bool> ShowMealHolderRemove(IList<object> recipes)
        {
            await AppContainer.ShowMealHolderRemove(recipes);
            return true;
        }

        public static async Task<bool> ShowEditAlbumModal(Action updateAlbums)
        {
            await AppContainer.ShowEditAlbumModal(updateAlbums);
            return true;
        }

        // Meal plan
        public static async Task<bool> ShowEditMeal(int index, int mealID, int mealPlanID,
            Recipe recipe, string mealPeriod, bool isEditing, string date)
        {
            await AppContainer.ShowEditMeal(index, mealID, mealPlanID, recipe, mealPeriod, isEditing, date);
            return true;
        }

        // Adding to empty tile Meal Plan
        public static async Task<bool> ShowEditMeal(int index, int mealPeriod, string date, int mealPlanId, bool isEditing, bool isCalendar = false)
        {
            await AppContainer.ShowEditMeal(index, mealPeriod, date, mealPlanId, isEditing, isCalendar);
            return true;
        }

        // Meal Template
        public static async Task<bool> ShowEditMeal(int mealTemplateID, int dayTemplateID, string mealType, Recipe recipe, bool isEditing, bool isTemplate)
        {
            await AppContainer.ShowEditMeal(mealTemplateID, dayTemplateID, mealType, recipe, isEditing, isTemplate);
            return true;
        }

        // Adding to Empty Tile Meal Template
        public static async Task<bool> ShowEditMeal(int dayTemplateID, int mealType, bool isEditing, bool isTemplate)
        {
            await AppContainer.ShowEditMeal(dayTemplateID, mealType, isEditing, isTemplate);
            return true;
        }

        public static async Task<bool> ShowEditMeal(Recipe recipe)
        {
            await AppContainer.ShowEditMeal(recipe);
            return true;
        }

        // Template
        public static async Task<bool> ShowEditTitle(int templateId, string name, string publishedAt)
        {
            await AppContainer.ShowEditTitle(templateId, name, publishedAt);
            return true;
        }

        public static async Task<bool> ShowCreateMealPlanPopup()
        {
            await AppContainer.ShowCreateMealPlanPopup();
            return true;
        }

        public static async Task<bool> HideAddRecipe()
        {
            await AppContainer.HideAddRecipe();
            return true;
        }

        public static async Task<bool> ShowVerifyAccount()
        {
            //new Models.Action((int)Actions.ActionName.ShowCreateMealPlan));
            HideModal();
            await AppContainer.ShowVerifyAccount();
            return true;
        }

        public static async Task DisplayAlert(string title, string message, string cancel)
        {
            await AppContainer.ShowAlert(title, message, cancel);
        }

        public static async Task<bool> DisplayAlert(string title, string message, string confirm, string cancel)
        {
            return await AppContainer.ShowAlert(title, message, cancel, confirm);
        }

        public static async Task<bool> ShowLoading()
        {
            await AppContainer.ShowLoading();
            return true;
        }

        public static void SetLoadingMessage(string message)
        {
            AppContainer.SetLoadingMessage(message);
        }


        public static async Task<bool> HideLoading()
        {
            await AppContainer.HideLoading();
            return true;
        }

        public static bool Update()
        {
            if (IsBusy) { return false; }
            return true;
        }

        public static async Task<bool> UpdatePage(int pageID)
        {
            await AppContainer.UpdatePage(pageID);
            return true;
        }

        public static async Task<bool> UpdatePage()
        {
            await AppContainer.UpdatePage();
            return true;
        }

        public static async Task<bool> ReloadPage(int pageID)
        {
            await AppContainer.ReloadPage(pageID);
            return true;
        }

        public static void UpdateLayout(string id)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(async () =>
            {
                _ = await AppContainer.UpdateLayout(id);
            })).Start();

        }

        public static void LogApiError(string uri)
        {
            ShowAlert("API ERROR: " + uri);
        }

        public static void ShowAlert(string text)
        {
            //appView.ShowAlert(text);
            DependencyService.Get<IMessage>().ShortAlert(text);
        }

        public static void ShowAlert(string title, string text)
        {
            //appView.ShowAlert(title, text);
            try
            {
                DependencyService.Get<IMessage>().LongAlert(text);
            }
            catch(Exception e)
            {
                Console.WriteLine("error showing alert");
            }
        }

        /*(
        public static void ShowInfoPanel(string text)
        {
            AppContainer.ShowInfoPanel(text);
            
        }*/

        /*
        public static void AddModalContent(View modalContent)
        {
            AppContainer.AddModalContent(modalContent);
        }*/

        public static void ShowModal()
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(async () =>
            {
                _ = await AppContainer.ShowModal(null);
            })).Start();

        }

        public static void HideModal()
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(async () =>
            {
                _ = await AppContainer.HideModal();
            })).Start();

        }

        /*
        public static async Task<bool> ShowAlert(string title, string text, string yes, string no)
        {
            return await AppContainer.ShowAlert(title, text, yes, no);
        }*/

        /*
        public async Task<bool> HandleHardwareBack()
        {
            Console.WriteLine("Hardware back pressed");
            if (Helpers.Pages.GetCurrent().Id != (int)AppSettings.PageNames.Landing)
            {
                await AppContainer.HardwareBack();
                return true;
            }
            return false; //;
        }*/

        public bool HandleHardwareBack()
        {
            AppContainer.HardwareBack();
            return true;
        }

        public static void GoBack()
        {
            AppContainer.HardwareBack();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            LocalDataStore.Save("app_start_time", DateTime.Now.ToString());
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            LocalDataStore.Save("app_sleep_time", DateTime.Now.ToString());
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            LocalDataStore.Save("app_resume_time", DateTime.Now.ToString());
        }

        public static async Task<string> ShowActionSheet(string title, string cancel, string? destruction, params string[] buttons)
        {
            var result = await AppContainer.DisplayActionSheet(title, cancel, destruction, buttons);
            return result;
        }

        public static bool IsSmallScreen()
        {
            return Units.SmallScreen;
            //Console.WriteLine("Scale " + Units.Scale);
            //if (Units.ScreenWidth < 400)
            //if (Units.PixelWidth < 400)
            //{
            //    return true;
            //}
            //return false;
        }

    }
}
