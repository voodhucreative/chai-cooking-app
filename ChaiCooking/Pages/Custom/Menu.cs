using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Composites;
using ChaiCooking.Components.Fields;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts;
using ChaiCooking.Layouts.Custom;
using ChaiCooking.Models;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using ChaiCooking.Tools;
using FFImageLoading.Forms;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Essentials;
using Xamarin.Forms;
using static ChaiCooking.Helpers.Custom.Accounts;
using CheckBox = ChaiCooking.Components.Composites.CheckBox;

namespace ChaiCooking.Pages.Custom
{
    public class Menu : Page
    {
        StackLayout ContentContainer;
        Grid MenuOverlay;
        private MenuHeader MainHeader;

        // search layout

        // library layout
        // - favourites
        // - albums
        // - meal plan (premium)
        // - tracker (premium)

        // filter options
        // - diet
        // - - {vegan, pescatarien, meat eater, vegetarian, lacto vegetarien, ovo vegetarian, ovo lacto vegetarian}
        // - allergens
        // - - 
        // - avoids
        Section MyLibrarySection;
        Section FilterOptionsSection;

        Section DietSection;
        Section AllergensSection;

        Section AvoidsSection;
        Section OthersSection;

        Section CommunitySection;
        Section ProfileAndSettingsSection;

        StackLayout FiltersContainer;
        StackLayout MenuLinksContainer;

        StackLayout CommunityContainer;
        StackLayout CommunityLinksContainer;
        StackLayout LibraryItemsContainer;

        //Grid LibraryItemsContainer;

        Grid DietOptionsContainer;
        Grid AllergensOptionsContainer;
        Grid AvoidsContainer;
        Grid OtherPrefsContainer;

        // library items - change these to custom layouts
        IconLabel Favourites;
        IconLabel Albums;
        IconLabel MealPlan;
        IconLabel Tracker;
        IconLabel Calendar;
        IconLabel Recipes;
        IconLabel RecipeEditor;
        IconLabel CollectionsLabel;
        IconLabel InfluencerBio;
        IconLabel Logout;

        Avatar UserAvatar;

        Grid AvoidsList;


        int GroupFontSize = Units.FontSizeL;
        int ItemFontSize = Units.FontSizeM;


        public Menu()
        {

            GroupFontSize = Units.FontSizeL;
            ItemFontSize = Units.FontSizeM;

            //GroupFontSize = Units.DynamicFontSizeM;
            //ItemFontSize = Units.DynamicFontSizeM;


            this.IsScrollable = true;
            this.Id = (int)AppSettings.PageNames.Menu;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.FadeIn;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.FadeOut;

            MenuOverlay = new Grid
            {
                BackgroundColor = Color.Transparent,

                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Opacity = 1,
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight
            };

            FiltersContainer = new StackLayout { Orientation = StackOrientation.Vertical };

            MenuLinksContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                WidthRequest = Units.ScreenWidth * 0.6,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };


            CommunityContainer = new StackLayout { Orientation = StackOrientation.Horizontal };
            CommunityLinksContainer = new StackLayout { Orientation = StackOrientation.Vertical };

            LibraryItemsContainer = new StackLayout {
                Orientation = StackOrientation.Vertical,
                WidthRequest = Units.ScreenWidth * 0.6,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            DietOptionsContainer = new Grid { };
            AllergensOptionsContainer = new Grid { };
            AvoidsContainer = new Grid { };
            OtherPrefsContainer = new Grid { };

            MyLibrarySection = new Section("My Library", true, true);
            MyLibrarySection.Toggle.LeftAlign();
            MyLibrarySection.SetBorder(1, Color.Transparent);
            MyLibrarySection.SetInfo("My Library", "Here you’ll find four of the key features of CHAI:\n\nMeal Plan - create a meal plan for public or private viewing\nCalendar – view your transition / flexitarian / vegan subscription meal plans\nRecipe Editor – create your own content\nCollections- organise your favourite recipes");

            FilterOptionsSection = new Section("Filter Options", true, true);
            FilterOptionsSection.SetBorder(1, Color.Transparent);
            FilterOptionsSection.Toggle.LeftAlign();

            DietSection = new Section("Diet", true, true);
            DietSection.SetBorder(1, Color.White);

            AllergensSection = new Section("Allergens", true, true);
            AllergensSection.SetBorder(1, Color.White);

            AvoidsSection = new Section("Avoids", true, true);
            AvoidsSection.SetBorder(1, Color.White);

            OthersSection = new Section("Others", true, true);
            OthersSection.SetBorder(1, Color.White);

            CommunitySection = new Section("Community", true, true);
            CommunitySection.SetBorder(1, Color.Transparent);
            CommunitySection.Toggle.LeftAlign();

            ProfileAndSettingsSection = new Section("Profile & Settings", true, true);
            ProfileAndSettingsSection.SetBorder(1, Color.Transparent);
            ProfileAndSettingsSection.Toggle.LeftAlign();
            ProfileAndSettingsSection.SetInfo("Profile & Settings", "Everything you need to configure your account and more");

            int row = 0;
            int col = 0;
            int itemCount = 0;

            List<IconLabel> libraryLabels = new List<IconLabel>();



            // LIBRARY ITEMS - sort these into smarter components
            CollectionsLabel = new IconLabel("folderfg.png", "Collections", Dimensions.ICON_LABEL_WIDTH, Dimensions.ICON_LABEL_HEIGHT);
            CollectionsLabel.TextContent.Content.TextColor = Color.White;
            CollectionsLabel.TextContent.Content.FontSize = GroupFontSize;
            CollectionsLabel.TextContent.Content.FontFamily = Fonts.GetBoldAppFont();
            CollectionsLabel.SetIconSize(Dimensions.ICON_LABEL_ICON_SIZE, Dimensions.ICON_LABEL_ICON_SIZE);

            TouchEffect.SetNativeAnimation(CollectionsLabel.Content, true);
            TouchEffect.SetCommand(CollectionsLabel.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        /*App.ShowInfoBubble(new Label
                        {
                            Text = "Organise your recipe collections."
                        }, Units.HalfScreenWidth, Units.HalfScreenHeight);*/
                        App.ShowInfoBubble(new Paragraph("Collections", "Organise your recipe collections.", null).Content, (int)Units.HalfScreenWidth, (int)Units.HalfScreenHeight);

                    }
                    else
                    { 
                        if (Connection.IsConnected())
                        {
                            App.SetLoadingMessage("Loading your collections...");
                            AppSession.InfoModeOn = false;
                            await App.CloseMenu();
                            App.SwitchSubHeaderToMainMode();
                            StaticData.collectionsClicked = true;
                            if (await App.AuthCheck((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Collections))
                            {
                                Console.WriteLine("Cool, we're authorized");
                            }
                        }
                        else
                        {
                            App.ShowAlert("Please connect to the internet.");
                        }
                    }
                });
            }));

            MealPlan = new IconLabel("calendaricon.png", AppText.MEAL_PLANS, Dimensions.ICON_LABEL_WIDTH, Dimensions.ICON_LABEL_HEIGHT);
            MealPlan.TextContent.Content.TextColor = Color.White;
            MealPlan.TextContent.Content.FontSize = GroupFontSize;
            MealPlan.TextContent.Content.FontFamily = Fonts.GetBoldAppFont();
            MealPlan.SetIconSize(Dimensions.ICON_LABEL_ICON_SIZE, Dimensions.ICON_LABEL_ICON_SIZE);

            TouchEffect.SetNativeAnimation(MealPlan.Content, true);
            TouchEffect.SetCommand(MealPlan.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        /*
                        App.ShowInfoBubble(new Label
                        {
                            Text = "Tap ‘Meal Planner’ to go to the Meal Planner. If you haven’t created or chosen a Meal Plan yet then you’ll be prompted to do so.You can also access the Meal Planner via Your Menu."
                        }, Units.HalfScreenWidth, Units.HalfScreenHeight);
                        */
                        App.ShowInfoBubble(new Paragraph("Meal Plans", "Tap ‘Meal Plans’ to go to the Meal Planner. If you haven’t created or chosen a Meal Plan yet then you’ll be prompted to do so. You can also access the Meal Planner via Your Menu.", null).Content, (int)Units.HalfScreenWidth, (int)Units.HalfScreenHeight);

                    }
                    else
                    {

                        if (Connection.IsConnected())
                        {
                            // check if we have an account and signed in first
                            if (await App.AuthCheck((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.MealPlan, false))
                            {
                                if (AppSettings.EnableBilling) // billing enabled
                                {
                                    App.SetLoadingMessage("Loading your meal plans...");

                                    //causes page loading glitch.
                                    //await App.GoToMealPlanner();
                                    if (await SubscriptionManager.CanAccessPaidContent(AppSession.CurrentUser))
                                    {
                                        App.SetLoadingMessage("Loading your meal plans...");

                                        await App.GoToMealPlanner();
                                    }
                                    else
                                    {
                                        await App.SetUserAccountType(Accounts.AccountType.ChaiFree);
                                        AppSession.SubscriptionTargetPage = (int)AppSettings.PageNames.MealPlan; // ask to subscrive
                                        await App.ShowSubscribeModal();
                                    }
                                }
                                else
                                {
                                    App.SetLoadingMessage("Loading your meal plans...");

                                    await App.GoToMealPlanner(); // free play!
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

            Calendar = new IconLabel("calendaricon.png", "Calendar", Dimensions.ICON_LABEL_WIDTH, Dimensions.ICON_LABEL_HEIGHT);
            Calendar.TextContent.Content.TextColor = Color.White;
            Calendar.TextContent.Content.FontSize = GroupFontSize;
            Calendar.TextContent.Content.FontFamily = Fonts.GetBoldAppFont();
            Calendar.SetIconSize(Dimensions.ICON_LABEL_ICON_SIZE, Dimensions.ICON_LABEL_ICON_SIZE);

            TouchEffect.SetNativeAnimation(Calendar.Content, true);
            TouchEffect.SetCommand(Calendar.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("Calendar", "Tap ‘Calendar’ to go to the Calendar. Here you’ll have easy access to view your Meal Plan subscription content. For example, if you’ve chosen, the Transition Plan then each week you’ll have access to newly created auto-populated content that will cut your meat intake down gradually until you are seeing fully vegan content at the end of your 12 months.", null).Content, (int)Units.HalfScreenWidth, (int)Units.HalfScreenHeight);
                    }
                    else
                    {
                        if (Connection.IsConnected())
                        {
                            // check if we have an account and signed in first
                            if (await App.AuthCheck((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Calendar, false))
                            {
                                if (AppSettings.EnableBilling) // billing enabled
                                {
                                    if (await SubscriptionManager.CanAccessPaidContent(AppSession.CurrentUser))
                                    {
                                        App.SetLoadingMessage("Loading your calendar...");
                                        await App.GoToCalendar();
                                    }
                                    else
                                    {
                                        AppSession.SubscriptionTargetPage = (int)AppSettings.PageNames.Calendar; // ask to subscribe
                                        await App.ShowSubscribeModal();
                                    }
                                }
                                else
                                {
                                    App.SetLoadingMessage("Loading your calendar...");
                                    await App.GoToCalendar(); // free play!
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

            Recipes = new IconLabel("trackericon.png", "My Recipes", Dimensions.ICON_LABEL_WIDTH, Dimensions.ICON_LABEL_HEIGHT);
            Recipes.TextContent.Content.TextColor = Color.White;
            Recipes.TextContent.Content.FontSize = GroupFontSize;
            Recipes.TextContent.Content.FontFamily = Fonts.GetBoldAppFont();
            Recipes.SetIconSize(Dimensions.ICON_LABEL_ICON_SIZE, Dimensions.ICON_LABEL_ICON_SIZE);

            TouchEffect.SetNativeAnimation(Recipes.Content, true);
            TouchEffect.SetCommand(Recipes.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {

                    }
                    else
                    {
                        if (Connection.IsConnected())
                        {
                            App.SetLoadingMessage("Loading your recipes...");
                            AppSession.InfoModeOn = false;
                            await App.CloseMenu();
                            App.SwitchSubHeaderToMainMode();
                            StaticData.collectionsClicked = true;
                            if (await App.AuthCheck((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.MyRecipes))
                            {
                                Console.WriteLine("Cool, we're authorized");
                            }
                        }
                        else
                        {
                            App.ShowAlert("Please connect to the internet.");
                        }
                    }
                });
            }));

            RecipeEditor = new IconLabel("editproperty.png", "Recipe Editor", Dimensions.ICON_LABEL_WIDTH, Dimensions.ICON_LABEL_HEIGHT);
            RecipeEditor.TextContent.Content.TextColor = Color.White;
            RecipeEditor.TextContent.Content.FontSize = GroupFontSize;
            RecipeEditor.TextContent.Content.FontFamily = Fonts.GetBoldAppFont();
            RecipeEditor.SetIconSize(Dimensions.ICON_LABEL_ICON_SIZE, Dimensions.ICON_LABEL_ICON_SIZE);

            TouchEffect.SetNativeAnimation(RecipeEditor.Content, true);
            TouchEffect.SetCommand(RecipeEditor.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("Recipe Editor", "Everything you need to create your own recipes with CHAI. You can publish them and include them in Meal Plans for other users to try. They will apply to any list of recipes in the app e.g., Recommended Recipes, Favourites, Waste Less etc.", null).Content, (int)Units.HalfScreenWidth, (int)Units.HalfScreenHeight);
                    }
                    else
                    { 
                        if (!AppSettings.RecipeEditorEnabled)
                        {
                            await App.PerformActionAsync(new Models.Action((int)Actions.ActionName.ShowComingSoon));
                        }
                        else
                        {
                            if (Connection.IsConnected())
                            {
                                App.SetLoadingMessage("Loading your recipes...");
                                AppSession.InfoModeOn = false;
                                await App.CloseMenu();
                                App.SwitchSubHeaderToMainMode();
                                StaticData.recipeEditorClicked = true;
                                if (await App.AuthCheck((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.RecipeEditor))
                                {
                                    Console.WriteLine("Cool, we're authorized");
                                }
                            }
                            else
                            {
                                App.ShowAlert("Please connect to the internet.");
                            }
                        }
                    }
                });
            }));

            Tracker = new IconLabel("trackericon.png", "Tracker", Dimensions.ICON_LABEL_WIDTH, Dimensions.ICON_LABEL_HEIGHT);
            Tracker.TextContent.Content.TextColor = Color.White;
            Tracker.TextContent.Content.FontSize = ItemFontSize;
            Tracker.TextContent.Content.FontFamily = Fonts.GetBoldAppFont();
            Tracker.SetIconSize(Dimensions.ICON_LABEL_ICON_SIZE, Dimensions.ICON_LABEL_ICON_SIZE);

            TouchEffect.SetNativeAnimation(Tracker.Content, true);
            TouchEffect.SetCommand(Tracker.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (!AppSession.InfoModeOn)
                    {
                        if (!AppSettings.TrackerEnabled)
                        {
                            await App.PerformActionAsync(new Models.Action((int)Actions.ActionName.ShowComingSoon));
                        }
                        else
                        {
                            if (Connection.IsConnected())
                            {
                                AppSession.InfoModeOn = false;
                                await App.CloseMenu();
                                App.SwitchSubHeaderToMainMode();

                                await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Tracker);
                            }
                            else
                            {
                                App.ShowAlert("Please connect to the internet.");
                            }
                        }
                    }
                });
            }));

            if (AppSettings.FavouritesEnabled)
            {
                libraryLabels.Add(Favourites);
            }

            if (AppSettings.AlbumsEnabled)
            {
                libraryLabels.Add(Albums);
            }

            if (AppSettings.MealPlannerEnabled)
            {
                libraryLabels.Add(MealPlan);
            }

            if (AppSettings.CalendarEnabled)
            {
                libraryLabels.Add(Calendar);
            }

            if (AppSettings.TrackerEnabled)
            {
                libraryLabels.Add(Tracker);
            }

            if (AppSettings.MyRecipesEnabled)
            {
                libraryLabels.Add(Recipes);
            }

            if (AppSettings.RecipeEditorEnabled)
            {
                libraryLabels.Add(RecipeEditor);
            }
            if (AppSettings.CollectionsEnabled)
            {
                libraryLabels.Add(CollectionsLabel);
            }

            foreach (IconLabel libraryLabel in libraryLabels)
            {
                //LibraryItemsContainer.Children.Add(libraryLabel.Content, col, row);
                //col++;

                //if (col >= Dimensions.GetNumberOfMenuColumns() - 1) { col = 0; row++; };


                //LibraryItemsContainer.Children.Add(libraryLabel.Content, 0, row);
                LibraryItemsContainer.Children.Add(libraryLabel.Content);
                row++;
                //col++;

                //if (col >= Dimensions.GetNumberOfMenuColumns() - 1) { col = 0; row++; };

                libraryLabel.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                if (AppSession.InfoModeOn)
                                {
                                    //Paragraph itemInfo = new Paragraph(libraryLabel.TextContent.Content.Text, "Some information about " + "This is the " + libraryLabel.TextContent.Content.Text, null);
                                    //App.ShowMenuInfoBubble(itemInfo.Content, libraryLabel.Content);
                                }
                                else
                                {

                                }
                            });
                        })
                    }
                );
                itemCount++;
            }
            
            MyLibrarySection.AddContent(LibraryItemsContainer);

            // DIET TYPES SECTION
            row = 0;
            col = 0;
            itemCount = 0;
            foreach (Preference dietType in AppDataContent.DietTypes)
            {
                if (AppSession.CurrentUser.Preferences.DietTypes.Find(x => x.Name == dietType.Name) != null) { dietType.IsSelected = true; }

                SelectLabel selectLabel = new SelectLabel(dietType.Name, Color.FromHex(Colors.CC_ORANGE), Color.White, 320, 32, dietType.IsSelected, true);
                selectLabel.CenterAlign();
                selectLabel.Title.Content.FontSize = ItemFontSize;
                selectLabel.Title.Content.FontFamily = Fonts.GetBoldAppFont();

                DietOptionsContainer.Children.Add(selectLabel.Content, col, row);

                col++;
                if (col >= Dimensions.GetNumberOfMenuColumns()) { col = 0; row++; };

                itemCount++;
                if (itemCount == AppDataContent.DietTypes.Count && AppDataContent.DietTypes.Count % 2 > 0)
                {
                    Grid.SetColumnSpan(selectLabel.Content, 2);
                }
            }

            /*
            if (!SubscriptionManager.CanAccessPaidContent(AppSession.CurrentUser).Result) // if subbed, we decide :)
            {
                DietSection.AddContent(DietOptionsContainer);
            }*/
            // ALLERGENS SECTION
            row = 0;
            col = 0;
            itemCount = 0;
            foreach (Preference allergen in AppDataContent.Allergens)
            {
                if (AppSession.CurrentUser.Preferences.Allergens.Find(x => x.Name == allergen.Name) != null) { allergen.IsSelected = true; }


                CheckBox checkBox = new CheckBox(allergen.Name, "tick.png", "tickbg.png", 320, 32, allergen.IsSelected);
                checkBox.SetCheckboxLeft();
                checkBox.SetIconSize(Dimensions.CHECKBOX_ICON_SIZE, Dimensions.CHECKBOX_ICON_SIZE);
                checkBox.Title.Content.FontSize = ItemFontSize; //Dimensions.CHECKBOX_FONT_SIZE;
                checkBox.Title.Content.FontFamily = Fonts.GetBoldAppFont();
                checkBox.Title.Content.TextColor = Color.White;
                AllergensOptionsContainer.Children.Add(checkBox.Content, col, row);

                col++;
                if (col >= Dimensions.GetNumberOfMenuColumns()) { col = 0; row++; };

                itemCount++;
                if (itemCount == AppDataContent.Allergens.Count && AppDataContent.Allergens.Count % 2 > 0)
                {
                    Grid.SetColumnSpan(checkBox.Content, 2);
                }
            }
            AllergensSection.AddContent(AllergensOptionsContainer);

            // AVOIDS SECTION

            AvoidsContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            AvoidsContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            row = 1;
            col = 0;
            itemCount = 0;

            StackLayout addAvoidSection = new StackLayout { Spacing = 0, Orientation = StackOrientation.Horizontal };
            CustomEntry avoidInput = new CustomEntry();
            avoidInput.Placeholder = "add an avoid / dislike";
            avoidInput.HorizontalOptions = LayoutOptions.StartAndExpand;

            IconButton addButton = new IconButton(52, 32, Color.FromHex(Colors.CC_ORANGE), Color.White, "", "plus.png", null);
            addButton.SetContentCenter();
            addButton.ContentContainer.Padding = 4;// Dimensions.GENERAL_COMPONENT_PADDING;
            addButton.SetIconSize(32, 32);

            TouchEffect.SetNativeAnimation(addButton.Content, true);
            TouchEffect.SetCommand(addButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (Helpers.Validation.ValidateInput(avoidInput, 32, 2, false))
                    {
                        Preference found = AppSession.CurrentUser.Preferences.Avoids.Find(x => x.Name == avoidInput.Text);

                        if (found != null)
                        {
                            App.ShowAlert(avoidInput.Text + " is already one of your avoids");
                        }
                        else
                        {
                            AppSession.CurrentUser.Preferences.Avoids.Add(new Preference(avoidInput.Text, false));
                            UpdateAvoids();
                        }
                    }
                    else
                    {
                        App.ShowAlert("Please use a valid name");
                    }
                });
            }));

            addAvoidSection.Children.Add(avoidInput);
            addAvoidSection.Children.Add(addButton.Content);


            AvoidsContainer.Children.Add(addAvoidSection, 0, 0);


            AvoidsList = new Grid();

            AvoidsContainer.Children.Add(AvoidsList, 0, 1);

            AvoidsSection.AddContent(AvoidsContainer);

            UpdateAvoids();


            // OTHERS SECTION
            row = 0;
            col = 0;
            itemCount = 0;
            foreach (Preference pref in AppSession.CurrentUser.Preferences.OtherPrefs)
            {
                CheckBox checkBox = new CheckBox(pref.Name, "tick.png", "tickbg.png", 280, 64, pref.IsSelected);
                checkBox.SetCheckboxLeft();
                checkBox.SetIconSize(Dimensions.CHECKBOX_ICON_SIZE, Dimensions.CHECKBOX_ICON_SIZE);
                checkBox.Title.Content.FontSize = Dimensions.CHECKBOX_FONT_SIZE;
                checkBox.Title.Content.TextColor = Color.White;
                OtherPrefsContainer.Children.Add(checkBox.Content, col, row);

                col++;
                if (col >= Dimensions.GetNumberOfMenuColumns()) { col = 0; row++; };

                itemCount++;
                if (itemCount == AppSession.CurrentUser.Preferences.OtherPrefs.Count && AppSession.CurrentUser.Preferences.OtherPrefs.Count % 2 > 0)
                {
                    Grid.SetColumnSpan(checkBox.Content, 2);
                }
            }
            OthersSection.AddContent(OtherPrefsContainer);


            // COMMUNITY SECTION

            foreach (MenuLink menuLink in AppDataContent.CommunityMenuLinks)
            {
                IconLabel iconLabel = new IconLabel(menuLink.ImageSource, menuLink.Name, 240, Dimensions.ICON_LABEL_HEIGHT);
                iconLabel.TextContent.Content.TextColor = Color.White;
                iconLabel.TextContent.Content.FontFamily = Fonts.GetBoldAppFont();
                iconLabel.SetIconSize(Dimensions.ICON_LABEL_ICON_SIZE, Dimensions.ICON_LABEL_ICON_SIZE);
                TouchEffect.SetNativeAnimation(iconLabel.Content, true);
                TouchEffect.SetCommand(iconLabel.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (AppSession.InfoModeOn)
                        {
                            Paragraph itemInfo = new Paragraph(iconLabel.TextContent.Content.Text, "Some information about " + "This is the " + iconLabel.TextContent.Content.Text, null);
                            App.ShowMenuInfoBubble(itemInfo.Content, iconLabel.Content);
                        }
                        else
                        {
                            await App.PerformActionAsync(menuLink.LinkAction);
                        }
                    });
                }));

                CommunityLinksContainer.Children.Add(iconLabel.Content);


            }

            UserAvatar = new Avatar(AppSession.CurrentUser.Username, AppSession.CurrentUser.AvatarImageUrl, Dimensions.AVATAR_SIZE, Dimensions.AVATAR_SIZE);

            UserAvatar.Title.Label.SetBinding(Label.TextProperty, new Binding(nameof(AppSession.CurrentUser.Username), source: AppSession.CurrentUser));
            UserAvatar.Icon.Image.SetBinding(CachedImage.SourceProperty, new Binding(nameof(AppSession.CurrentUser.AvatarImageUrl), source: AppSession.CurrentUser));


            CommunityContainer.Children.Add(CommunityLinksContainer);
            CommunityContainer.Children.Add(UserAvatar.Content);
            CommunitySection.AddContent(CommunityContainer);

            FiltersContainer.Children.Add(DietSection.Content);
            FiltersContainer.Children.Add(AllergensSection.Content);
            FiltersContainer.Children.Add(AvoidsSection.Content);
            FiltersContainer.Children.Add(OthersSection.Content);

            FilterOptionsSection.AddContent(FiltersContainer);





            PageContent = new Grid
            {
                BackgroundColor = Color.Transparent,//FromHex(Colors.CC_DARK_BLUE_GREY),

            };

            // add a background?
            //AddBackgroundImage("no_image.png");

            // build labels
            ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.End,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                WidthRequest = Units.ScreenWidth,
                //HeightRequest = Units.ScreenHeight,
                Spacing = 0,
                Padding = new Thickness(Units.QuarterScreenWidth, 0, 0, 240)
            };

            /*
            foreach (MenuLink menuLink in AppDataContent.ProfileAndSettingsMenuLinks)
            {
                IconLabel iconLabel = new IconLabel(menuLink.ImageSource, menuLink.Name, 240, 40);
                iconLabel.TextContent.Content.TextColor = Color.White;
                iconLabel.TextContent.Content.FontFamily = Fonts.GetBoldAppFont();
                iconLabel.SetIconSize(24, 24);

                TouchEffect.SetNativeAnimation(iconLabel.Content, true);
                TouchEffect.SetCommand(iconLabel.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (AppSession.InfoModeOn)
                        {
                            Paragraph itemInfo = new Paragraph(iconLabel.TextContent.Content.Text, "Some information about " + "This is the " + iconLabel.TextContent.Content.Text, null);
                            //App.ShowMenuInfoBubble(itemInfo.Content, iconLabel.Content);
                            App.ShowInfoBubble(itemInfo.Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                        }
                        else
                        {
                            //await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.LoginAndRegistration);
                            if (await App.AuthCheck(menuLink.LinkAction))
                            {
                                Console.WriteLine("Cool, we're authorized");
                            }
                        }
                    });
                }));

                MenuLinksContainer.Children.Add(iconLabel.Content);
            }
            */

            IconLabel ShoppingBasketLink = new IconLabel("carticon.png", "Shopping Basket", 240, 40);

            ShoppingBasketLink.TextContent.Content.TextColor = Color.White;
            ShoppingBasketLink.TextContent.Content.FontFamily = Fonts.GetBoldAppFont();
            ShoppingBasketLink.SetIconSize(24, 24);
            ShoppingBasketLink.TextContent.Content.FontSize = GroupFontSize;

            TouchEffect.SetNativeAnimation(ShoppingBasketLink.Content, true);
            TouchEffect.SetCommand(ShoppingBasketLink.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("Shopping Basket", "See what's in there and checkout if you're ready.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                    }
                    else
                    {
                        if (await App.AuthCheck((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.ShoppingBasket))
                        {
                            Console.WriteLine("Cool, we're authorized");
                        }
                    }
                });
            }));

            IconLabel YourPreferencesLink = new IconLabel("userprefsicon.png", "Your Preferences", 240, 40);
            YourPreferencesLink.TextContent.Content.TextColor = Color.White;
            YourPreferencesLink.TextContent.Content.FontFamily = Fonts.GetBoldAppFont();
            YourPreferencesLink.SetIconSize(24, 24);
            YourPreferencesLink.TextContent.Content.FontSize = GroupFontSize;
            TouchEffect.SetNativeAnimation(YourPreferencesLink.Content, true);
            TouchEffect.SetCommand(YourPreferencesLink.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("Your Preferences", "Go back and change the settings you applied.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                    }
                    else
                    {
                        if (await App.AuthCheck((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Questionnaire))
                        {
                            Console.WriteLine("Cool, we're authorized");
                        }
                    }
                });
            }));



            IconLabel TermsLink = new IconLabel("termsicon.png", "Terms & Conditions", 240, 40);
            TermsLink.TextContent.Content.TextColor = Color.White;
            TermsLink.TextContent.Content.FontFamily = Fonts.GetBoldAppFont();
            TermsLink.SetIconSize(24, 24);
            TermsLink.TextContent.Content.FontSize = GroupFontSize;
            TouchEffect.SetNativeAnimation(TermsLink.Content, true);
            TouchEffect.SetCommand(TermsLink.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("Terms & Conditions", "Please read these carefully so that you understand what you can expect from us and what we ask from you in return.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                    }
                    else
                    {
                        await Launcher.OpenAsync(new Uri(AppSettings.TermAndConditionsUrl));
                    }
                });
            }));

            IconLabel PrivacyLink = new IconLabel("privacyicon.png", "Privacy Policy", 240, 40);
            PrivacyLink.TextContent.Content.TextColor = Color.White;
            PrivacyLink.TextContent.Content.FontFamily = Fonts.GetBoldAppFont();
            PrivacyLink.SetIconSize(24, 24);
            PrivacyLink.TextContent.Content.FontSize = GroupFontSize;
            TouchEffect.SetNativeAnimation(PrivacyLink.Content, true);
            TouchEffect.SetCommand(PrivacyLink.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("Privacy Policy", "We take it seriously, and your privacy is very important to us.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                    }
                    else
                    {
                        await Launcher.OpenAsync(new Uri(AppSettings.PrivacyPolicyUrl));
                    }
                });
            }));


            MenuLinksContainer.Children.Add(ShoppingBasketLink.Content);
            MenuLinksContainer.Children.Add(YourPreferencesLink.Content);
            MenuLinksContainer.Children.Add(TermsLink.Content);
            MenuLinksContainer.Children.Add(PrivacyLink.Content);

            InfluencerBio = new IconLabel("userprefsicon.png", "Influencer Bio", 240, 40);
            InfluencerBio.TextContent.Content.FontFamily = Fonts.GetBoldAppFont();
            InfluencerBio.TextContent.Content.TextColor = Color.White;
            InfluencerBio.SetIconSize(24, 24);
            TouchEffect.SetNativeAnimation(InfluencerBio.Content, true);
            TouchEffect.SetCommand(InfluencerBio.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (AppSession.InfoModeOn)
                        {
                            App.ShowInfoBubble(new Paragraph("Influencer Bio", "If you are invited to be an Influencer, then you can write your bio here and upload an image to go with it.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                        }
                        else
                        {
                            if (Connection.IsConnected())
                            {
                                AppSession.InfoModeOn = false;
                                await App.CloseMenu();
                                App.SwitchSubHeaderToMainMode();

                                if (await App.AuthCheck(new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.InfluencerBio)))
                                {
                                    Console.WriteLine("Cool, we're authorized");
                                }
                            }
                            else
                            {
                                App.ShowAlert("Please connect to the internet.");
                            }
                        }
                    });
                }));

            if (AppSettings.InfluencerBioEnabled)
            {
                if (AppSession.CurrentUser.IsInfluencer())
                {
                    MenuLinksContainer.Children.Add(InfluencerBio.Content);
                }
            }

            Logout = new IconLabel("closecirclewhite.png", "Logout", 240, 40);
            Logout.TextContent.Content.FontFamily = Fonts.GetBoldAppFont();
            Logout.TextContent.Content.TextColor = Color.White;
            Logout.SetIconSize(24, 24);
            Logout.TextContent.Content.FontSize = GroupFontSize;
            TouchEffect.SetNativeAnimation(Logout.Content, true);
            TouchEffect.SetCommand(Logout.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("Logout", "You’ll be taken to the login page, ready for when you want to sign back into your CHAI account.", null).Content, (int)Units.HalfScreenWidth, (int)Units.HalfScreenHeight);
                    }
                    else
                    {
                        if (Connection.IsConnected())
                        {
                            AppSession.InfoModeOn = false;
                            await App.CloseMenu();
                            App.SwitchSubHeaderToMainMode();
                            var result = await App.ApiBridge.LogOut(AppSession.CurrentUser, false);

                            //If we successfully loggout, show a message.
                            if (result)
                            {
                                App.ShowAlert("You've been logged out, have a nice day.");
                                //Sends us back to the homepage if the user logs out while not on the homepage.
                                if (Helpers.Pages.GetCurrentPageId() != (int)AppSettings.PageNames.Landing)
                                {
                                    await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Landing);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Failed to logout of API, but forcing logout of app.");
                                await App.ApiBridge.LogOut(AppSession.CurrentUser, true);
                            }
                            //Why are we auth checking a logout?
                            //This creates a up a login with chai popup.
                            //if (await App.AuthCheck(new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.LoginAndRegistration)))
                            //{
                            //    Console.WriteLine("Cool, we're authorized");
                            //}
                        }
                        else
                        {
                            App.ShowAlert("Please connect to the internet.");
                        }
                    }
                });
            }));

            //Defaults Logout button to hidden if not logged in
            if (AppSession.CurrentUser.WhiskTokenId > 0)
            {
                MenuLinksContainer.Children.Add(Logout.Content);
            }

            ProfileAndSettingsSection.AddContent(MenuLinksContainer);

            MainHeader = new MenuHeader();

            // ContentContainer.Children.Add(MainHeader.Content);


            if (LibraryItemsContainer.Children.Count > 0)
            {
                ContentContainer.Children.Add(MyLibrarySection.Content);
            }

            if (AppSettings.MenuFilterSectionActive)
            {
                ContentContainer.Children.Add(FilterOptionsSection.Content);
            }

            if (AppSettings.CommunityOptionsEnabled)
            {
                ContentContainer.Children.Add(CommunitySection.Content);
            }

            ContentContainer.Children.Add(ProfileAndSettingsSection.Content);

            StackLayout MenuContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Color.Transparent,

                Spacing = 0
            };

            Grid DropShadow = new Grid
            {
                WidthRequest = 2,
                BackgroundColor = Color.FromHex(Colors.CC_DARK_GREY),
                Opacity = 0.25
            };

            MenuContainer.Children.Add(ContentContainer);
            MenuContainer.Children.Add(DropShadow);
            PageContent.Children.Add(MenuContainer, 0, 0);

        }


        private void UpdateAvoids()
        {
            int itemCount = 0;
            int row = 0;
            int col = 0;
            AvoidsList.Children.Clear();
            foreach (Preference avoid in AppSession.CurrentUser.Preferences.Avoids)
            {
                RemovableLabel removeLabel = new RemovableLabel(Color.FromHex(Colors.CC_ORANGE), Color.White, avoid.Name, null);
                TouchEffect.SetNativeAnimation(removeLabel.Content, true);
                TouchEffect.SetCommand(removeLabel.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //Preference found = AppSession.CurrentUser.Preferences.Avoids.Find(x => x.Name == removeLabel.Name);

                        AppSession.CurrentUser.Preferences.Avoids.Remove(avoid);
                        UpdateAvoids();
                    });
                }));

                AvoidsList.Children.Add(removeLabel.Content, col, row);

                col++;
                if (col >= Dimensions.GetNumberOfMenuColumns()) { col = 0; row++; };

                itemCount++;
            }

        }

        public override async Task Update()
        {
            MenuLinksContainer.Children.Remove(InfluencerBio.Content);
            MenuLinksContainer.Children.Remove(Logout.Content);

            if (AppSettings.InfluencerBioEnabled)
            {
                if (AppSession.CurrentUser.IsInfluencer())
                {
                    MenuLinksContainer.Children.Add(InfluencerBio.Content);
                }
            }
            //This hides the logout button if we're not logged in
            if (AppSession.CurrentUser.WhiskTokenId > 0)
            {
                MenuLinksContainer.Children.Add(Logout.Content);
            }

            await DebugUpdate(AppSettings.TransitionVeryFast);
        }

        public void AddOverlayContent(View view)
        {

            MenuOverlay.Children.Add(view);

            PageContent.Children.Add(MenuOverlay, 0, 0);
        }

        public void HideOverlayContent()
        {

            MenuOverlay.Children.Clear();

            PageContent.Children.Remove(MenuOverlay);
        }
    }
}
