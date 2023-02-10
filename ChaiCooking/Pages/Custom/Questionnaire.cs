using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Composites;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using ChaiCooking.Services.Storage;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class Questionnaire : Page
    {
        StackLayout ContentContainer;
        StackLayout DietryNeedsContainer;

        Grid DietOptionsContainer;
        List<Components.Composites.DietOption> DietTypeOptions;

        Grid AllergensOptionsContainer;
        List<Components.Composites.CheckBox> AllergensOptions;

        Grid AvoidsContainer;
        List<Components.Composites.CheckBox> AvoidsOptions;

        ActiveLabel SkipThisStep;
        bool isBusy;
        StaticLabel UserName;
        StaticLabel IntroInfo;
        StaticLabel ChooseOptions;
        StaticLabel IAmA;
        StaticLabel AllergyInfo;
        StaticLabel IAmAllergicTo;
        StaticLabel AvoidInfo;
        StaticLabel IWantToAvoid;
        StaticLabel QuestionsInfo;
        //StaticLabel IWantToUse;


        DragSlider WeeklyBudget;
        DragSlider HouseholdSize;
        DragSlider WeeklyShop;
        DragSlider ConvenienceStores;
        DragSlider OnlineShopping;
        DragSlider Plants;
        DragSlider Alcohol;

        SearchField AvoidSearch;

        StaticLabel PrefsTitle;
        //StaticLabel BioTitle;
        //StaticLabel PlanTitle;
        //Editor BioEditor;

        //Components.Composites.CheckBox UKMetrics;
        //Components.Composites.CheckBox USMetrics;

        Components.Buttons.ImageButton SubmitButton;
        //Components.Buttons.ImageButton SaveProfileButton;

        public Questionnaire()
        {
            this.IsScrollable = true;
            this.IsRefreshable = false;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;

            this.Id = (int)AppSettings.PageNames.Questionnaire;
            this.Name = "Your Preferences";
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.FadeIn;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.FadeOut;
            isBusy = false;
            PageContent = new Grid
            {
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING)
            };

            ContentContainer = BuildContent();
            PageContent.Children.Add(ContentContainer);
        }

        public StackLayout BuildContent()
        {
            int row = 0;
            int col = 0;
            int itemCount = 0;
            // build labels
            StackLayout mainLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.End,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),// FromHex("eeeeee"),
                WidthRequest = Units.ScreenWidth,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING)
            };

            mainLayout.Children.Clear();

            SkipThisStep = new ActiveLabel("Close", Units.FontSizeM, Color.Transparent, Color.White, null);

            //SkipThisStep = new ActiveLabel("Skip this step", Units.FontSizeM, Color.Transparent, Color.White, null);
            SkipThisStep.RightAlign();
            SkipThisStep.Label.Padding = Dimensions.GENERAL_COMPONENT_SPACING;

            string greeting = "Hi";



            if(AppSession.CurrentUser.FirstName != "Chai")
            {
                if (AppSession.CurrentUser.FirstName.Length > 0)
                {
                    greeting += " " + AppSession.CurrentUser.FirstName + ".";
                }
                else
                {
                    AppSession.CurrentUser.FirstName = "Chai";
                }
                
            }
            else
            {
                greeting += ".";
            }

            
            UserName = new StaticLabel(greeting);
            UserName.Content.TextColor = Color.White;
            UserName.Content.FontSize = Units.FontSizeXXL;
            UserName.LeftAlign();



            //BioTitle = new StaticLabel("About You");
            //BioTitle.Content.TextColor = Color.White;
            //BioTitle.Content.FontSize = Units.FontSizeL;
            //BioTitle.LeftAlign();

            //PlanTitle = new StaticLabel("Current plan: " + Accounts.GetAccountName(AppSession.CurrentUser.Preferences.AccountType));
            //PlanTitle.Content.TextColor = Color.White;
            //PlanTitle.Content.FontSize = Units.FontSizeL;
            //PlanTitle.LeftAlign();

            PrefsTitle = new StaticLabel("Your Preferences");
            PrefsTitle.Content.TextColor = Color.White;
            PrefsTitle.Content.FontSize = Units.FontSizeL;
            PrefsTitle.LeftAlign();

            /*BioEditor = new Editor
            {
                Keyboard = Keyboard.Text,
                WidthRequest = Units.ScreenWidth,
                HeightRequest = 240,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.White,
                FontFamily = Fonts.GetRegularAppFont(),
                Placeholder = "Just a little bit about yourself! If you want to publish your own meal plans, this is a great place to describe the kind of person you are and the kind of diets you love!"
            };

            if (AppSession.CurrentUser.Bio != null)
            {
                BioEditor.Text = AppSession.CurrentUser.Bio;
            }
            */

            IntroInfo = new StaticLabel(AppText.QUESTIONNAIRE_INTRO);
            IntroInfo.Content.TextColor = Color.White;
            IntroInfo.Content.FontSize = Units.FontSizeM;
            IntroInfo.Content.Padding = Dimensions.GENERAL_COMPONENT_SPACING;
            IntroInfo.CenterAlign();



            WeeklyBudget = new DragSlider(AppText.MY_WEEKLY_BUDGET, AppDataContent.WeeklyBudgetValues, Dimensions.SLIDER_WIDTH, Dimensions.SLIDER_HEIGHT);

            HouseholdSize = new DragSlider(AppText.MY_HOUSEHOLD_SIZE, AppDataContent.HouseholdSizeValues, Dimensions.SLIDER_WIDTH, Dimensions.SLIDER_HEIGHT);

            DietryNeedsContainer = new StackLayout
            {
                BackgroundColor = Color.White,
                WidthRequest = Units.ScreenWidth
            };

            ChooseOptions = new StaticLabel(AppText.CHOOSE_OPTIONS);
            ChooseOptions.Content.TextColor = Color.Black;
            ChooseOptions.Content.FontSize = Units.FontSizeM;
            ChooseOptions.Content.Padding = Dimensions.GENERAL_COMPONENT_SPACING;
            ChooseOptions.LeftAlign();

            IAmA = new StaticLabel("I am a...");
            IAmA.Content.TextColor = Color.Black;
            IAmA.Content.FontSize = Units.FontSizeL;
            IAmA.Content.FontFamily = Fonts.GetBoldAppFont();
            IAmA.Content.Padding = Dimensions.GENERAL_COMPONENT_SPACING;
            IAmA.LeftAlign();

            // DIET TYPES SECTION
            row = 0;
            col = 0;
            itemCount = 0;
            DietOptionsContainer = new Grid { };
            DietTypeOptions = new List<Components.Composites.DietOption>();

            foreach (Preference dietType in AppDataContent.DietTypes)
            {
                dietType.IsSelected = false;
                if (AppSession.CurrentUser.Preferences.DietTypes.Find(x => x.Name == dietType.Name) != null)
                {
                    dietType.IsSelected = true;
                }

                DietTypeOptions.Add(new Components.Composites.DietOption(dietType, "tick.png", "tickbg.png", 280, 32, dietType.IsSelected));
            }

            foreach (Components.Composites.DietOption checkBox in DietTypeOptions)
            {
                checkBox.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;
                checkBox.SetCheckboxLeft();
                checkBox.SetIconSize(Dimensions.CHECKBOX_ICON_SIZE, Dimensions.CHECKBOX_ICON_SIZE);
                checkBox.Title.Content.FontSize = Dimensions.CHECKBOX_FONT_SIZE;
                checkBox.Title.Content.TextColor = Color.Black;

                DietOptionsContainer.Children.Add(checkBox.Content, col, row);

                if (App.IsSmallScreen())
                {
                    row++;
                    col = 0;
                    Grid.SetColumnSpan(checkBox.Content, 2);
                }
                else
                {
                    col++;
                    if (col >= Dimensions.GetNumberOfMenuColumns()) { col = 0; row++; };
                }

                itemCount++;
                if (itemCount == AppDataContent.DietTypes.Count && AppDataContent.DietTypes.Count % 2 > 0)
                {
                    Grid.SetColumnSpan(checkBox.Content, 2);
                }
            }

            row = 0;
            col = 0;
            itemCount = 0;
            AllergensOptionsContainer = new Grid { };
            AllergensOptions = new List<Components.Composites.CheckBox>();

            foreach (Preference allergen in AppDataContent.Allergens)
            {
                allergen.IsSelected = false;
                if (AppSession.CurrentUser.Preferences.Allergens.Find(x => x.Name == allergen.Name) != null)
                {
                    allergen.IsSelected = true;
                    Console.WriteLine("Setting allergen: " + allergen.Name);
                }

                AllergensOptions.Add(new Components.Composites.CheckBox(allergen.Name, "tick.png", "tickbg.png", 280, 32, allergen.IsSelected));
            }

            foreach (Components.Composites.CheckBox checkBox in AllergensOptions)
            {
                checkBox.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;
                checkBox.SetCheckboxLeft();
                checkBox.SetIconSize(Dimensions.LARGE_CHECKBOX_ICON_SIZE, Dimensions.LARGE_CHECKBOX_ICON_SIZE);
                checkBox.Title.Content.FontSize = Dimensions.CHECKBOX_FONT_SIZE;
                checkBox.Title.Content.TextColor = Color.Black;
                AllergensOptionsContainer.Children.Add(checkBox.Content, col, row);

                if (App.IsSmallScreen())
                {
                    row++;
                    col = 0;
                    Grid.SetColumnSpan(checkBox.Content, 2);
                }
                else
                {
                    col++;
                    if (col >= Dimensions.GetNumberOfMenuColumns()) { col = 0; row++; };
                }

                itemCount++;
                if (itemCount == AppDataContent.Allergens.Count && AppDataContent.Allergens.Count % 2 > 0)
                {
                    Grid.SetColumnSpan(checkBox.Content, 2);
                }
            }


            AllergyInfo = new StaticLabel(AppText.ALLERGY_INFO);
            AllergyInfo.Content.TextColor = Color.Black;
            AllergyInfo.Content.FontSize = Units.FontSizeM;
            AllergyInfo.Content.Padding = Dimensions.GENERAL_COMPONENT_SPACING;
            AllergyInfo.LeftAlign();

            IAmAllergicTo = new StaticLabel("I am allergic to...");
            IAmAllergicTo.Content.TextColor = Color.Black;
            IAmAllergicTo.Content.FontSize = Units.FontSizeL;
            IAmAllergicTo.Content.FontFamily = Fonts.GetBoldAppFont();
            IAmAllergicTo.Content.Padding = Dimensions.GENERAL_COMPONENT_SPACING;
            IAmAllergicTo.LeftAlign();

            AvoidInfo = new StaticLabel(AppText.AVOID_INFO);
            AvoidInfo.Content.TextColor = Color.Black;
            AvoidInfo.Content.FontSize = Units.FontSizeM;
            AvoidInfo.Content.Padding = Dimensions.GENERAL_COMPONENT_SPACING;
            AvoidInfo.LeftAlign();

            IWantToAvoid = new StaticLabel("I want to avoid...");
            IWantToAvoid.Content.TextColor = Color.Black;
            IWantToAvoid.Content.FontSize = Units.FontSizeL;
            IWantToAvoid.Content.FontFamily = Fonts.GetBoldAppFont();
            IWantToAvoid.Content.Padding = Dimensions.GENERAL_COMPONENT_SPACING;
            IWantToAvoid.LeftAlign();

            StackLayout addAvoidSection = new StackLayout { Spacing = 0, Orientation = StackOrientation.Horizontal, Padding = new Thickness(Dimensions.GENERAL_COMPONENT_SPACING) };
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
                        avoidInput.Text = "";
                    }
                    else
                    {
                        App.ShowAlert("Please use a valid name");
                    }
                });
            }));

            row = 0;
            col = 0;
            itemCount = 0;
            AvoidsContainer = new Grid { };
            foreach (Preference avoid in AppSession.CurrentUser.Preferences.Avoids)
            {
                RemovableLabel removeLabel = new RemovableLabel(Color.FromHex(Colors.CC_ORANGE), Color.White, avoid.Name, null);
                TouchEffect.SetNativeAnimation(removeLabel.Content, true);
                TouchEffect.SetCommand(removeLabel.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        AppSession.CurrentUser.Preferences.Avoids.Remove(avoid);
                        UpdateAvoids();
                    });
                }));

                AvoidsContainer.Children.Add(removeLabel.Content, col, row);

                col++;
                if (col >= Dimensions.GetNumberOfMenuColumns()) { col = 0; row++; };

                itemCount++;
            }
            addAvoidSection.Children.Add(avoidInput);
            addAvoidSection.Children.Add(addButton.Content);

            QuestionsInfo = new StaticLabel(AppText.QUESTIONS_INFO);
            QuestionsInfo.Content.TextColor = Color.Black;
            QuestionsInfo.Content.FontSize = Units.FontSizeM;
            QuestionsInfo.Content.Padding = Dimensions.GENERAL_COMPONENT_SPACING;
            QuestionsInfo.LeftAlign();

            WeeklyShop = new DragSlider(AppText.WEEKLY_SHOP, AppDataContent.WeeklyShop, Dimensions.SLIDER_WIDTH, Dimensions.SLIDER_HEIGHT);
            WeeklyShop.Title.Content.TextColor = Color.Black;
            WeeklyShop.BarPointValueLabel.Content.TextColor = Color.FromHex(Colors.CC_ORANGE);

            ConvenienceStores = new DragSlider(AppText.CONVENIENCE_STORES, AppDataContent.ConvenienceStores, Dimensions.SLIDER_WIDTH, Dimensions.SLIDER_HEIGHT);
            ConvenienceStores.Title.Content.TextColor = Color.Black;
            ConvenienceStores.BarPointValueLabel.Content.TextColor = Color.FromHex(Colors.CC_ORANGE);

            OnlineShopping = new DragSlider(AppText.ONLINE_SHOPPING, AppDataContent.OnlineShopping, Dimensions.SLIDER_WIDTH, Dimensions.SLIDER_HEIGHT);
            OnlineShopping.Title.Content.TextColor = Color.Black;
            OnlineShopping.BarPointValueLabel.Content.TextColor = Color.FromHex(Colors.CC_ORANGE);

            Plants = new DragSlider(AppText.PLANTS, AppDataContent.Plants, Dimensions.SLIDER_WIDTH, Dimensions.SLIDER_HEIGHT);
            Plants.Title.Content.TextColor = Color.Black;
            Plants.BarPointValueLabel.Content.TextColor = Color.FromHex(Colors.CC_ORANGE);

            Alcohol = new DragSlider(AppText.ALCOHOL, AppDataContent.Alcohol, Dimensions.SLIDER_WIDTH, Dimensions.SLIDER_HEIGHT);
            Alcohol.Title.Content.TextColor = Color.Black;
            Alcohol.BarPointValueLabel.Content.TextColor = Color.FromHex(Colors.CC_ORANGE);

            //IWantToUse = new StaticLabel("I want to use...");
            //IWantToUse.Content.TextColor = Color.Black;
            //IWantToUse.Content.FontSize = Units.FontSizeL;
            //IWantToUse.Content.FontFamily = Fonts.GetBoldAppFont();
            //IWantToUse.Content.Padding = Dimensions.GENERAL_COMPONENT_SPACING;
            //IWantToUse.LeftAlign();

            Grid metricsContainer = new Grid { };

            //UKMetrics = new Components.Composites.CheckBox("UK Metrics", "tick.png", "tickbg.png", 280, 32, true);
            //UKMetrics.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;
            //UKMetrics.SetCheckboxLeft();
            //UKMetrics.SetIconSize(Dimensions.CHECKBOX_ICON_SIZE, Dimensions.CHECKBOX_ICON_SIZE);
            //UKMetrics.Title.Content.FontSize = Dimensions.CHECKBOX_FONT_SIZE;
            //UKMetrics.Title.Content.TextColor = Color.Black;

            //USMetrics = new Components.Composites.CheckBox("US Metrics", "tick.png", "tickbg.png", 280, 32, false);
            //USMetrics.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;
            //USMetrics.SetCheckboxLeft();
            //USMetrics.SetIconSize(Dimensions.CHECKBOX_ICON_SIZE, Dimensions.CHECKBOX_ICON_SIZE);
            //USMetrics.Title.Content.FontSize = Dimensions.CHECKBOX_FONT_SIZE;
            //USMetrics.Title.Content.TextColor = Color.Black;

            //UKMetrics.Content.GestureRecognizers.Add(
            //        new TapGestureRecognizer()
            //        {
            //            Command = new Command(() =>
            //            {
            //                Device.BeginInvokeOnMainThread(async () =>
            //                {
            //                    UKMetrics.Toggle();
            //                    USMetrics.Toggle();
            //                });
            //            })
            //        }
            //   );


            //USMetrics.Content.GestureRecognizers.Add(
            //        new TapGestureRecognizer()
            //        {
            //            Command = new Command(() =>
            //            {
            //                Device.BeginInvokeOnMainThread(async () =>
            //                {
            //                    UKMetrics.Toggle();
            //                    USMetrics.Toggle();
            //                });
            //            })
            //        }
            //   );

            SubmitButton = new Components.Buttons.ImageButton("arrow_right_green_chevron.png", "arrow_right_green_chevron.png", AppText.SAVE_CHANGES, Color.Black, null);
            SubmitButton.RightAlign();
            SubmitButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            SubmitButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;

            
            //SaveProfileButton = new Components.Buttons.ImageButton("arrow_right_green_chevron.png", "arrow_right_green_chevron.png", AppText.SAVE_CHANGES, Color.Black, null);
            //SaveProfileButton.RightAlign();
            //SaveProfileButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            //SaveProfileButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;

            TouchEffect.SetNativeAnimation(SubmitButton.Content, true);
            TouchEffect.SetCommand(SubmitButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (!isBusy)
                    {
                        isBusy = true;
                        PerformSubmit();
                        //App.ShowAlert("NOT BUSY");
                    }
                    else
                    {
                        //App.ShowAlert("BUSY");
                    }
                });
            }));

            /*
            TouchEffect.SetNativeAnimation(SaveProfileButton.Content, true);
            TouchEffect.SetCommand(SaveProfileButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (!isBusy)
                    {
                        isBusy = true;
                        PerformSubmitPrefs();
                        //App.ShowAlert("NOT BUSY");
                    }
                    else
                    {
                        //App.ShowAlert("BUSY");
                    }
                });
            }));
            */
            //metricsContainer.Children.Add(UKMetrics.Content, 0, 0);
            //metricsContainer.Children.Add(USMetrics.Content, 1, 0);
            metricsContainer.Children.Add(SubmitButton.Content, 1, 1);


            DietryNeedsContainer.Children.Clear();





            StaticLabel AccountType = new StaticLabel("Account: " + AppSession.CurrentUser.EmailAddress +
            "\nChai Plan: " + Accounts.GetAccountName(AppSession.CurrentUser.Preferences.AccountType));



            AccountType.Content.TextColor = Color.Gray;
            AccountType.Content.FontSize = Units.FontSizeM;
            AccountType.Content.FontFamily = Fonts.GetRegularAppFont();
            AccountType.Content.Padding = Dimensions.GENERAL_COMPONENT_SPACING;
            AccountType.LeftAlign();

            DietryNeedsContainer.Children.Add(AccountType.Content);

            if (AppSession.CurrentUser.Preferences.AccountType == (int)Accounts.AccountType.ChaiFree) // if subbed, we decide, otherwise.. :)
            {
                if (!AppSession.CurrentUser.IsInfluencer())
                {
                    DietryNeedsContainer.Children.Add(ChooseOptions.Content);
                    DietryNeedsContainer.Children.Add(IAmA.Content);
                    DietryNeedsContainer.Children.Add(DietOptionsContainer);
                }
            }

            DietryNeedsContainer.Children.Add(AllergyInfo.Content);
            DietryNeedsContainer.Children.Add(IAmAllergicTo.Content);

            DietryNeedsContainer.Children.Add(AllergensOptionsContainer);

            DietryNeedsContainer.Children.Add(AvoidInfo.Content);
            DietryNeedsContainer.Children.Add(IWantToAvoid.Content);

            DietryNeedsContainer.Children.Add(addAvoidSection);

            DietryNeedsContainer.Children.Add(AvoidsContainer);


            if (!AppSettings.CutDownQuestionnaire)
            {
                DietryNeedsContainer.Children.Add(QuestionsInfo.Content);
                DietryNeedsContainer.Children.Add(WeeklyShop.GetContent());
                DietryNeedsContainer.Children.Add(ConvenienceStores.GetContent());
                DietryNeedsContainer.Children.Add(OnlineShopping.GetContent());
                DietryNeedsContainer.Children.Add(Plants.GetContent());
                DietryNeedsContainer.Children.Add(Alcohol.GetContent());
            }

            //if (!AppSettings.HideMetrics)
            //{
            //DietryNeedsContainer.Children.Add(IWantToUse.Content);


            //DietryNeedsContainer.Children.Add(metricsContainer);
            //}
            //else
            //{
            DietryNeedsContainer.Children.Add(SubmitButton.Content);
            //}



            //DietryNeedsContainer.Children.Add(SubmitButton.Content);

            Grid.SetColumnSpan(addAvoidSection, 2);


            //mainLayout.Children.Add(SkipThisStep.Content);
            mainLayout.Children.Add(UserName.Content);
            //mainLayout.Children.Add(PlanTitle.Content);
            //mainLayout.Children.Add(BioTitle.Content);
            //mainLayout.Children.Add(BioEditor);

            //mainLayout.Children.Add(SaveProfileButton.Content);

            mainLayout.Children.Add(PrefsTitle.Content);

            if (!AppSettings.CutDownQuestionnaire)
            {
                mainLayout.Children.Add(IntroInfo.Content);
                mainLayout.Children.Add(WeeklyBudget.GetContent());
                mainLayout.Children.Add(HouseholdSize.GetContent());
            }


            mainLayout.Children.Add(DietryNeedsContainer);


            return mainLayout;
        }

        private string PreparePrefChoices(string input)
        {
            string stripped = input.ToLower().Replace(' ', '_');
            string removeCurrency = stripped.Replace("£", "");
            return removeCurrency;
        }


        private async void PerformSubmit()
        {
            //App.ShowAlert("YOU ARE NOW LOGGED IN");
            // await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Landing);

            App.SetLoadingMessage("Updating your preferences...");


            Console.WriteLine("Data collected");

            Console.WriteLine("Selected WeeklyBudget: " + PreparePrefChoices(WeeklyBudget.GetSelectedValue()));
            Console.WriteLine("Selected HouseholdSize: " + PreparePrefChoices(HouseholdSize.GetSelectedValue()));
            Console.WriteLine("Selected WeeklyShop: " + PreparePrefChoices(WeeklyShop.GetSelectedValue()));
            Console.WriteLine("Selected ConvenienceStores: " + PreparePrefChoices(ConvenienceStores.GetSelectedValue()));
            Console.WriteLine("Selected OnlineShopping: " + PreparePrefChoices(OnlineShopping.GetSelectedValue()));
            Console.WriteLine("Selected Plants: " + PreparePrefChoices(Plants.GetSelectedValue()));
            Console.WriteLine("Selected Alcohol: " + PreparePrefChoices(Alcohol.GetSelectedValue()));

            AppSession.CurrentUser.Preferences.DietTypes.Clear();

            foreach (Components.Composites.DietOption checkBox in DietTypeOptions)
            {
                if (checkBox.IsChecked)
                {
                    AppSession.CurrentUser.Preferences.DietTypes.Add(new Preference(checkBox.Title.Content.Text, true));
                }
            }

            AppSession.CurrentUser.Preferences.Allergens.Clear();

            foreach (Components.Composites.CheckBox checkBox in AllergensOptions)
            {
                if (checkBox.IsChecked)
                {
                    AppSession.CurrentUser.Preferences.Allergens.Add(new Preference(checkBox.Title.Content.Text, true));
                }
            }

            //Console.WriteLine("DONE");


            //List<Recipe> r = DataManager.SearchRecipes(AppSession.CurrentUser, true);

            //Console.WriteLine("DONE");

            /*
            if (BioEditor.Text != null)
            {
                AppSession.CurrentUser.Bio = BioEditor.Text;
                LocalDataStore.SaveAll();
            }
            */

            if (await DataManager.UpdateUserPrefs(AppSession.CurrentUser.Preferences.AccountType, true))
            {
                await App.ShowLoading();
                await Update();
                await App.HideLoading();
                if (AppSession.CurrentUser.Preferences.AccountType == Accounts.AccountType.ChaiPremiumTrans && AppSession.settingUpAccount)
                {
                    AppSession.settingUpAccount = false;
                    await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Calendar);
                }
                else
                {
                    await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.RecommendedRecipes);
                }
            }
            isBusy = false;
        }

        private async void PerformSubmitPrefs()
        {
            /*
            if (BioEditor.Text != null)
            {
                AppSession.CurrentUser.Bio = BioEditor.Text;
                LocalDataStore.SaveAll();
            }
            */
            if (await DataManager.UpdateUserPrefs(AppSession.CurrentUser.Preferences.AccountType, false))
            {
                App.ShowAlert("Your information has been saved");
            }

            /*
            if (await DataManager.UpdateUserPrefs(AppSession.CurrentUser.Preferences.AccountType))
            {

                await App.ShowLoading();
                await Update();
                await App.HideLoading();
                await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.RecommendedRecipes);
            }*/
            isBusy = false;
        }



        private void UpdateAvoids()
        {
            int itemCount = 0;
            int row = 0;
            int col = 0;
            AvoidsContainer.Children.Clear();
            foreach (Preference avoid in AppSession.CurrentUser.Preferences.Avoids)
            {
                RemovableLabel removeLabel = new RemovableLabel(Color.FromHex(Colors.CC_ORANGE), Color.White, avoid.Name, null);

                removeLabel.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                //Preference found = AppSession.CurrentUser.Preferences.Avoids.Find(x => x.Name == removeLabel.Name);

                                AppSession.CurrentUser.Preferences.Avoids.Remove(avoid);
                                UpdateAvoids();
                            });
                        })
                    }
                );

                AvoidsContainer.Children.Add(removeLabel.Content, col, row);

                col++;
                if (col >= Dimensions.GetNumberOfMenuColumns()) { col = 0; row++; };

                itemCount++;
            }

        }

        public async Task<bool> UpdateDietTypes()
        {
            DietOptionsContainer.Children.Clear();
            int col = 0;
            int row = 0;
            int itemCount = 0;

            DietTypeOptions.Clear();

            foreach (Preference dietType in AppDataContent.DietTypes)
            {
                dietType.IsSelected = false;
                if (AppSession.CurrentUser.Preferences.DietTypes.Find(x => x.Name == dietType.Name) != null)
                {
                    dietType.IsSelected = true;
                }

                DietTypeOptions.Add(new Components.Composites.DietOption(dietType, "tick.png", "tickbg.png", 280, 32, dietType.IsSelected));
            }

            foreach (Components.Composites.DietOption checkBox in DietTypeOptions)
            {
                checkBox.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;
                checkBox.SetCheckboxLeft();
                checkBox.SetIconSize(Dimensions.CHECKBOX_ICON_SIZE, Dimensions.CHECKBOX_ICON_SIZE);
                checkBox.Title.Content.FontSize = Dimensions.CHECKBOX_FONT_SIZE;
                checkBox.Title.Content.TextColor = Color.Black;

                DietOptionsContainer.Children.Add(checkBox.Content, col, row);

                if (App.IsSmallScreen())
                {
                    row++;
                    col = 0;
                    Grid.SetColumnSpan(checkBox.Content, 2);
                }
                else
                {
                    col++;
                    if (col >= Dimensions.GetNumberOfMenuColumns()) { col = 0; row++; };
                }

                //col++;

                //if (col >= Dimensions.GetNumberOfMenuColumns()) { col = 0; row++; };

                itemCount++;
                if (itemCount == AppDataContent.DietTypes.Count && AppDataContent.DietTypes.Count % 2 > 0)
                {
                    Grid.SetColumnSpan(checkBox.Content, 2);
                }
            }
            return true;
        }

        public override async Task Update()
        {
            if(AppDataContent.DietTypes.Count == 0)
            {
                AppDataContent.Init();
            }
            

            //if (this.NeedsRefreshing)
            //{
            UserPreferences prefs = null;
            try
            {
                prefs = await App.ApiBridge.GetPreferences(AppSession.CurrentUser);
                AppSession.CurrentUser.Preferences = prefs;
                LocalDataStore.SaveAll();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to update prefs - Questionnaire.cs");
            }
            //Null exception stopping page from loading on registration...
            //Console.WriteLine("Prefs: " + AppSession.CurrentUser.Preferences.DietTypes[0].Name);


            await DebugUpdate(AppSettings.TransitionVeryFast);
            await base.Update();
            try
            {
                PageContent.Children.Remove(ContentContainer);
                ContentContainer = BuildContent();
                PageContent.Children.Add(ContentContainer);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to update prefs - Questionnaire.cs");
            }
            App.SetSubHeaderTitle(AppText.RECOMMENDED_RECIPES, new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.RecommendedRecipes));
        }
    }
}
