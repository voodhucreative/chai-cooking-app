using System;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Helpers;
using ChaiCooking.Layouts.Custom;
using ChaiCooking.Layouts.Custom.Panels;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models;
using ChaiCooking.Services;
using ChaiCooking.Tools;
using Plugin.DeviceInfo.Abstractions;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class Landing : Page
    {
        StackLayout ContentContainer;

        HomePageTile CookItCornerTile;
        HomePageTile WasteLessTile;
        HomePageTile HealthyLiving;

        //WhiskSignupPanel WhiskSignup;

        public Landing()
        {
            this.IsScrollable = true;
            this.IsRefreshable = true;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;

            this.Id = (int)AppSettings.PageNames.Landing;
            this.Name = "Home";
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.FadeIn;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.FadeOut;

            PageContent = new Grid
            {
                BackgroundColor = Color.Transparent
            };

            // add a background?
            //AddBackgroundImage("pagebg.jpg");
            //WhiskSignup = new WhiskSignupPanel();

            if (Application.Current.Properties.ContainsKey("whiskToken"))
            {
                StaticData.authKeyExists = true;
                // For testing uncomment necessary properties to remove the property value.
                //App.Current.Properties["whiskToken"] = "authorized";
                //App.Current.Properties["whiskToken"] = "unauthorized"; /// Will deauthorize the user
                //App.Current.Properties.Remove("defaultMealPlan"); /// Will remove the default meal plan
                //App.Current.SavePropertiesAsync();

            }
            else
            {
                if (StaticData.forceLogin && !AppSession.CurrentUser.IsRegistered)
                {
                    StaticData.WhiskAuthenticate();
                }
                StaticData.forceLogin = false;
                StaticData.authKeyExists = false;
            }

            ContentContainer = BuildContent();
            PageContent.Children.Add(ContentContainer);
        }

        public StackLayout BuildContent()
        {

            // build labels
            StackLayout mainLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.End,
                BackgroundColor = Color.Transparent,// FromHex("eeeeee"),
                WidthRequest = Units.ScreenWidth,
                Spacing = 0
            };

            //TouchEffect.SetNativeAnimation(mainLayout, true);
            //TouchEffect.SetCommand(mainLayout,
            //new Command(() =>
            //{
            //    Device.BeginInvokeOnMainThread(async () =>
            //    {
            //        await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Questionnaire);
            //    });
            //}));

            CookItCornerTile = new HomePageTile(AppText.COOK_IT_CORNER, "Step by step video guidance");
            CookItCornerTile.BackgroundImage.Content.Source = "cookitcornerbg.jpg";
            CookItCornerTile.BackgroundImage.Content.Aspect = Aspect.AspectFill;
            TouchEffect.SetNativeAnimation(CookItCornerTile.Content, true);
            TouchEffect.SetCommand(CookItCornerTile.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        /*App.ShowInfoBubble(new Label
                        {
                            Text = "Watch plant-based instructional cooking videos."
                        }, Units.HalfScreenWidth, Units.HalfScreenHeight);*/

                        App.ShowInfoBubble(new Paragraph("Cook It Corner", "Watch plant-based instructional cooking videos.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);

                    }
                    else
                    {
                        await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.CookItCorner);

                        //AuthCheck(1);
                    }
                });
            }));

            WasteLessTile = new HomePageTile(AppText.WASTE_LESS, "Shop wisely");
            WasteLessTile.BackgroundImage.Content.Source = "wastelessbg.jpg";
            WasteLessTile.BackgroundImage.Content.Aspect = Aspect.AspectFill;
            TouchEffect.SetNativeAnimation(WasteLessTile.Content, true);
            TouchEffect.SetCommand(WasteLessTile.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        /*
                        App.ShowInfoBubble(new Label
                        {
                            Text = "This is our standard recipe page - here you can shop, share and find more recipes to choose from."
                        }, Units.HalfScreenWidth, Units.HalfScreenHeight);
                        */
                        App.ShowInfoBubble(new Paragraph("Waste Less", "Here, you’ll be able to discover new recipes to help you use up any leftover ingredients, and make the most of what you buy.You can enter multiple ingredients into the Waste Less input and find recipes from our vast database which provide the best match.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);

                    }
                    else
                    {
                        await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.WasteLess);

                        //AuthCheck(2);
                    }
                });
            }));

            HealthyLiving = new HomePageTile(AppText.HEALTHY_LIVING, "Influencers");
            HealthyLiving.BackgroundImage.Content.Source = "healthyeatingbg.jpg";
            HealthyLiving.BackgroundImage.Content.Aspect = Aspect.AspectFill;
            TouchEffect.SetNativeAnimation(HealthyLiving.Content, true);
            TouchEffect.SetCommand(HealthyLiving.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        /*
                        App.ShowInfoBubble(new Label
                        {
                            Text = "This is where you can browse and select meal plans uploaded by influencers, browse all meal plans, or create your own content using CHAI’s recipe editor and make it visible for everyone to see here."
                        }, Units.HalfScreenWidth, Units.HalfScreenHeight);
                        */
                        App.ShowInfoBubble(new Paragraph("Healthy Living", "This is where you can browse and select meal plans uploaded by influencers, browse all meal plans, or create your own content using CHAI’s recipe editor and make it visible for everyone to see here.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);

                    }
                    else
                    {
                        if (Connection.IsConnected())
                        {
                            if (await App.AuthCheck((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.HealthyLiving, false))
                            {
                                if (AppSettings.EnableBilling) // billing enabled
                                {
                                    if (await SubscriptionManager.CanAccessPaidContent(AppSession.CurrentUser))
                                    {
                                        await App.GoToHealthyLiving();
                                    }
                                    else
                                    {
                                        AppSession.SubscriptionTargetPage = (int)AppSettings.PageNames.HealthyLiving; // ask to subscrive
                                        await App.ShowSubscribeModal();
                                    }
                                }
                                else
                                {
                                    await App.GoToHealthyLiving(); // free play!
                                }
                            }
                        }
                        else
                        {
                            App.ShowAlert("Please connect to the internet.");
                        }
                    }
                });
            }));

            if (AppSettings.CookItCornerEnabled)
            {
                mainLayout.Children.Add(CookItCornerTile.GetContent());
            }
            mainLayout.Children.Add(WasteLessTile.GetContent());
            mainLayout.Children.Add(HealthyLiving.GetContent());
            return mainLayout;
        }

        public override async Task Update()
        {
            App.HideBackGroundImage();

            if (this.NeedsRefreshing)
            {
                await DebugUpdate(AppSettings.TransitionVeryFast);
                await base.Update();
                PageContent.Children.Remove(ContentContainer);
                ContentContainer = BuildContent();
                PageContent.Children.Add(ContentContainer);
                App.ShowMenuButton();
                App.SetSubHeaderTitle(AppText.RECOMMENDED_RECIPES, new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.RecommendedRecipes));
                //App.SetSubHeaderDescription("Find recipes based on your filter choices and stored favourites. The content will update regularly so keep checking back in.");
            }
        }

        public async Task<bool> AuthCheckOld(int actionId, int pageId)
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
                        await App.GoToLoginOrRegister();
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
    }
}