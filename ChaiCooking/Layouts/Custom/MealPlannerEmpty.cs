using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services.Storage;
using ChaiCooking.Views.CollectionViews.MealPlanner;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using static ChaiCooking.Models.Custom.MealPlanAPI.UserMealTemplate;

namespace ChaiCooking.Layouts.Custom
{
    public class MealPlannerEmpty : ActiveComponent
    {
        Dictionary<string, int> dayDict = new Dictionary<string, int>
            {
                {"1", 1 },
                {"2", 2 },
                {"3", 3 },
                {"4", 4 },
                {"5", 5 },
                {"6", 6 },
                {"7", 7 },
                {"8", 8 },
                {"9", 9 },
                {"10", 10 },
                {"11", 11 },
                {"12", 12 },
                {"13", 13 },
                {"14", 14 },
                {"15", 15 },
                {"16", 16 },
                {"17", 17 },
                {"18", 18 },
                {"19", 19 },
                {"20", 20 },
                {"21", 21 },
                {"22", 22 },
                {"23", 23 },
                {"24", 24 },
                {"25", 25 },
                {"26", 26 },
                {"27", 27 },
                {"28", 28 },
            };

        Dictionary<string, int> weekDict = new Dictionary<string, int>
            {
                {"1 Week", 1 },
                {"2 Weeks", 2 },
                {"3 Weeks", 3 },
                {"4 Weeks", 4 }
            };
        StaticLabel switchLabel, startLabel, numberOfDaysLabel,
            createMealPlanLabel, descriptionLabel, numberOfWeeksLabel;
        StackLayout dateSwapCont, buttonContainer, autoFillCont, numOfWeekCont;
        Picker weeksPicker, daysPicker;
        DatePicker startDatePicker;
        ColourButton createButton, publishButton;
        Components.Composites.CheckBox AutoFillCheckBox;
        bool allowPress;
        public MealPlannerEmpty()
        {
            Content.Children.Add(BuildEmpty());

            
        }

        private StackLayout BuildEmpty()
        {
            

            allowPress = false;
            Switch creationType = new Switch()
            {
                IsToggled = false,
                OnColor = Color.Orange,
                ThumbColor = Color.White,

            };

            creationType.Toggled += (sender, e) =>
            {
                OnToggled(sender, e);
            };

            switchLabel = new StaticLabel(AppText.PRIVATE_MEAL_PLAN);
            switchLabel.Content.FontSize = Units.FontSizeL;
            switchLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            switchLabel.Content.TextColor = Color.White;
            switchLabel.Content.HorizontalTextAlignment = TextAlignment.Center;

            StackLayout switchContainer = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    switchLabel.Content,
                    creationType
                }
            };

            createMealPlanLabel = new StaticLabel("Create a Meal Plan");
            createMealPlanLabel.Content.FontSize = Units.FontSizeXL;
            createMealPlanLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            createMealPlanLabel.Content.TextColor = Color.White;
            createMealPlanLabel.Content.HorizontalTextAlignment = TextAlignment.Start;

            descriptionLabel = new StaticLabel("Your current Meal Plan is empty - choose a name for it, select a start date, and the length of your meal plan, then press create.");
            descriptionLabel.Content.FontSize = Units.FontSizeL;
            descriptionLabel.Content.FontFamily = Fonts.GetRegularAppFont();
            descriptionLabel.Content.TextColor = Color.White;
            descriptionLabel.Content.HorizontalTextAlignment = TextAlignment.Start;

            StaticLabel nameEntryLabel = new StaticLabel("Meal Plan Name");
            nameEntryLabel.Content.FontSize = Units.FontSizeL;
            nameEntryLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            nameEntryLabel.Content.TextColor = Color.White;
            nameEntryLabel.Content.HorizontalTextAlignment = TextAlignment.Center;

            CustomEntry nameEntry = new CustomEntry
            {
                Placeholder = "Type Text Here",
                PlaceholderColor = Color.LightGray,
                HorizontalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.White,
                WidthRequest = Units.HalfScreenWidth + Units.ScreenWidth25Percent,
                HeightRequest = Units.TapSizeS,
                FontSize = Units.FontSizeL,
                HorizontalOptions = LayoutOptions.Center,
            };

            startLabel = new StaticLabel("Start Date");
            startLabel.Content.FontSize = Units.FontSizeL;
            startLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            startLabel.Content.TextColor = Color.White;
            startLabel.Content.HorizontalTextAlignment = TextAlignment.Center;

            startDatePicker = new CustomDatePicker
            {
                Date = DateTime.Now,
                MinimumDate = DateTime.UtcNow,
                TextColor = Color.Gray,
                FontSize = Units.FontSizeL,
                BackgroundColor = Color.White,
                WidthRequest = Units.HalfScreenWidth + Units.ScreenWidth25Percent,
                HeightRequest = Units.TapSizeS,
                HorizontalOptions = LayoutOptions.Center,
                Format = "dd/MM/yyyy",
            };

            dateSwapCont = new StackLayout
            {
                Children =
                {
                    startLabel.Content,
                    startDatePicker,
                }
            };

            numberOfWeeksLabel = new StaticLabel("Number of Weeks");
            numberOfWeeksLabel.Content.FontSize = Units.FontSizeL;
            numberOfWeeksLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            numberOfWeeksLabel.Content.TextColor = Color.White;
            numberOfWeeksLabel.Content.HorizontalTextAlignment = TextAlignment.Center;

            weeksPicker = new Picker
            {
                HorizontalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.White,
                WidthRequest = Units.HalfScreenWidth + Units.ScreenWidth25Percent,
                HeightRequest = Units.TapSizeS,
                FontSize = Units.FontSizeL,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.Gray,
            };

            foreach (string week in weekDict.Keys)
            {
                weeksPicker.Items.Add(week);
            }

            weeksPicker.SelectedIndexChanged += (sender, args) =>
            {
                if (weeksPicker.SelectedIndex == -1)
                {
                    // Do nothing
                }
                else
                {
                    string weekID = weeksPicker.Items[weeksPicker.SelectedIndex];
                    StaticData.chosenWeeks = weekDict[weekID];
                    weeksPicker.Title = "";
                }
            };

            numOfWeekCont = new StackLayout()
            {
                Children =
                {
                    numberOfWeeksLabel.Content,
                    weeksPicker,
                }
            };

            StackLayout errorContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                HorizontalOptions = LayoutOptions.Center,
            };

            AutoFillCheckBox = new Components.Composites.CheckBox("Auto-Fill", "tick.png", "tickbg.png", 320, 10, true);

            AutoFillCheckBox.SetCheckboxLeft();
            AutoFillCheckBox.SetIconSize(Dimensions.CHECKBOX_ICON_SIZE, Dimensions.CHECKBOX_ICON_SIZE);
            AutoFillCheckBox.Title.Content.FontSize = Units.FontSizeM; //Dimensions.CHECKBOX_FONT_SIZE;
            AutoFillCheckBox.Title.Content.FontFamily = Fonts.GetBoldAppFont();
            AutoFillCheckBox.Title.Content.TextColor = Color.White;
            AutoFillCheckBox.Container.VerticalOptions = LayoutOptions.Start;

            autoFillCont = new StackLayout()
            {
                Orientation  = StackOrientation.Vertical,
                Children =
                {
                    AutoFillCheckBox.Content,
                    new Label
                    {
                        Text = "If this box is checked, we will popuplate your meal plan by automatically selecting recipes, based on your diet preferences.",
                        FontFamily = Fonts.GetRegularAppFont(),
                        FontSize = Units.FontSizeS,
                        TextColor = Color.White,
                        Margin = Dimensions.GENERAL_COMPONENT_PADDING

                    }
                }
            };

            createButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CREATE_BUTTON_TEXT, null);
            createButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            createButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            createButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(createButton.Content, true);
            TouchEffect.SetCommand(createButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("Create", "Tap Create if you decide that you’d rather make your own plan and auto populate it using CHAI recipe filters.You can adjust it afterwards using the Meal Plan editor.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                    }
                    else
                    {
                        if (nameEntry.Text == "" || nameEntry.Text == null || weeksPicker.SelectedIndex < 0)
                        {
                            App.ShowAlert("Please check if details are correct.");
                        }
                        else
                        {
                            StaticData.createErrorText = "";
                            StaticData.startDate = startDatePicker.Date.ToString("M-d-yyyy");
                            StaticData.hypenedDate = startDatePicker.Date.ToString("yyyy-MM-dd");
                            string createdMealPlanId = await App.ApiBridge.CreateMealPlan(AppSession.CurrentUser, nameEntry.Text, StaticData.hypenedDate, (StaticData.chosenWeeks * 7));

                            if (createdMealPlanId != null)
                            {
                                if (!allowPress)
                                {
                                    allowPress = true;
                                    if (AutoFillCheckBox.IsChecked)
                                    {
                                        Console.WriteLine("Autofill meal plan: " + createdMealPlanId);
                                        await App.ShowLoading();
                                        var result = await App.ApiBridge.AutocompleteMealPlan(AppSession.CurrentUser, createdMealPlanId);
                                    }
                                    AppSession.mealPlannerCalendar = await App.ApiBridge.GetWeek(AppSession.CurrentUser, createdMealPlanId, "1", AppSession.mealPlanTokenSource.Token);
                                    var mealPlannerGroup = new MealPlannerCollectionViewSection(AppSession.mealPlannerCalendar.Data);
                                    AppSession.mealPlannerCollection.Add(mealPlannerGroup);
                                    AppSession.mealPlannerCollection.RemoveAt(0);
                                    AppSession.CurrentUser.defaultMealPlanID = int.Parse(createdMealPlanId);
                                    AppSession.CurrentUser.defaultMealPlanName = nameEntry.Text;
                                    AppSession.CurrentUser.defaultMealPlanWeeks = StaticData.chosenWeeks;
                                    AppSession.SetMealPlanner(nameEntry.Text, true, StaticData.chosenWeeks, false);
                                    LocalDataStore.SaveAll();
                                    StaticData.userMealPlans = await App.ApiBridge.GetUserMealPlans(AppSession.CurrentUser);
                                    AppSession.mealPlanCheck = true;
                                    await App.HideLoading();
                                    Console.WriteLine();
                                    StaticData.mealPlanClicked = true;
                                    StaticData.isTemplate = false;
                                    allowPress = false;
                                }
                            }
                            else if (StaticData.createErrorText != "")
                            {
                                errorContainer.Children.Clear();
                                errorContainer.Children.Add(new Label
                                {
                                    Text = $"Could not create a meal plan due to the conflicting dates: {StaticData.createErrorText}",
                                    TextColor = Color.Orange,
                                    HorizontalTextAlignment = TextAlignment.Center,
                                });
                            }
                            else
                            {
                                App.ShowAlert("Could not create Meal Plan.");
                            }
                        }
                    }
                });
            }));

            buttonContainer = new StackLayout
            {
                Children =
                {
                    createButton.Content
                }
            };

            publishButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CREATE_BUTTON_TEXT, null);
            publishButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            publishButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            publishButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(publishButton.Content, true);
            TouchEffect.SetCommand(publishButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (nameEntry.Text == "" || nameEntry.Text == null || daysPicker.SelectedIndex < 0)
                    {
                        App.ShowAlert("Please check if details are correct.");
                    }
                    else
                    {
                        var result = await App.ApiBridge.CreateUserMealPlanTemplates(AppSession.CurrentUser, nameEntry.Text, StaticData.chosenDays);
                        switch (result)
                        {
                            case "Forbidden":
                                App.ShowAlert("Could not create Meal Template.");
                                break;
                            case "The name has already been taken.":
                                App.ShowAlert("The name has already been taken.");
                                break;
                            case "Unknown_error":
                                App.ShowAlert("Could not create Meal Template.");
                                break;
                            default:
                                try
                                {
                                    StaticData.currentTemplateID = int.Parse(result);
                                    if (!allowPress)
                                    {
                                        allowPress = true;
                                        await App.ShowLoading();
                                        await StaticData.UpdateMealTemplate();
                                        AppSession.SetMealPlanner(nameEntry.Text, true, 1, true);
                                        await App.HideLoading();
                                        allowPress = false;
                                        StaticData.currentTemplateName = nameEntry.Text;
                                        StaticData.isTemplate = true;
                                    }
                                }
                                catch
                                {
                                    App.ShowAlert("Could not create Meal Template.");
                                }
                                break;
                        }
                    }
                });
            }));

            numberOfDaysLabel = new StaticLabel("Number of Days");
            numberOfDaysLabel.Content.FontSize = Units.FontSizeL;
            numberOfDaysLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            numberOfDaysLabel.Content.TextColor = Color.White;
            numberOfDaysLabel.Content.HorizontalTextAlignment = TextAlignment.Center;

            daysPicker = new Picker
            {
                HorizontalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.White,
                WidthRequest = Units.HalfScreenWidth + Units.ScreenWidth25Percent,
                HeightRequest = Units.TapSizeS,
                FontSize = Units.FontSizeL,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.Gray,
            };

            foreach (string day in dayDict.Keys)
            {
                daysPicker.Items.Add(day);
            }

            daysPicker.SelectedIndexChanged += (sender, args) =>
            {
                if (daysPicker.SelectedIndex == -1)
                {
                    // Do nothing
                }
                else
                {
                    string dayID = daysPicker.Items[daysPicker.SelectedIndex];
                    StaticData.chosenDays = dayDict[dayID];
                    daysPicker.Title = "";
                }
            };

            StackLayout fillerCont = new StackLayout
            {
                HeightRequest = 2,
            };

            StackLayout masterContainer = new StackLayout
            {
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight,
                Spacing = Dimensions.GENERAL_COMPONENT_SPACING,
                Children =
                {
                    createMealPlanLabel.Content,
                    descriptionLabel.Content,
                    nameEntryLabel.Content,
                    nameEntry,
                    dateSwapCont,
                    numOfWeekCont,
                    fillerCont,
                    buttonContainer,
                    autoFillCont,
                    errorContainer
                }
            };

            StackLayout contentContainer = new StackLayout
            {
                Children = {
                    switchContainer,
                    masterContainer },
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING)
            };

            nameEntry.Text = "";
            //Setting pickers to first index so they do not appear null.
            weeksPicker.SelectedIndex = 0;
            daysPicker.SelectedIndex = 0;
            startDatePicker.Date = DateTime.Now;
            errorContainer.Children.Clear();
            return contentContainer;
        }

        void OnToggled(object sender, ToggledEventArgs e)
        {
            if (AppSession.InfoModeOn)
            {
                App.ShowInfoBubble(new Paragraph("Public or Private Option", "This will give you the option to make a created Meal Plan from scratch, either Public for everyone to view or keep it Private for your viewing only!", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
            }
            else
            {
                // Perform an action after examining e.Value
                if (e.Value)
                {
                    // Activated
                    switchLabel.Content.Text = AppText.PUBLIC_MEAL_PLAN;
                    createMealPlanLabel.Content.Text = "Create a " + AppText.PUBLIC_MEAL_PLAN;
                    descriptionLabel.Content.Text = "Create a meal plan that you can publish and share with other users.";
                    dateSwapCont.Children.Clear();
                    dateSwapCont.Children.Add(numberOfDaysLabel.Content);
                    dateSwapCont.Children.Add(daysPicker);
                    numOfWeekCont.Children.Clear();
                    buttonContainer.Children.Clear();
                    buttonContainer.Children.Add(publishButton.Content);
                    autoFillCont.Children.Clear();
                }
                else
                {
                    // Deactivated
                    switchLabel.Content.Text = AppText.PRIVATE_MEAL_PLAN;
                    createMealPlanLabel.Content.Text = "Create a " + AppText.PRIVATE_MEAL_PLAN;
                    descriptionLabel.Content.Text = "Your current meal plan is empty - choose a name for it, select a start date, and the length of your meal plan, then press create.";
                    dateSwapCont.Children.Clear();
                    dateSwapCont.Children.Add(startLabel.Content);
                    dateSwapCont.Children.Add(startDatePicker);
                    numOfWeekCont.Children.Clear();
                    numOfWeekCont.Children.Add(numberOfWeeksLabel.Content);
                    numOfWeekCont.Children.Add(weeksPicker);
                    buttonContainer.Children.Clear();
                    buttonContainer.Children.Add(createButton.Content);
                    autoFillCont.Children.Clear();
                    autoFillCont.Children.Add(AutoFillCheckBox.Content);
                }
            }
        }
    }
}
