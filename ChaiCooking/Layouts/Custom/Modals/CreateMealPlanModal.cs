using System;
using System.Collections.Generic;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom.Feed;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Modals
{
    public class CreateMealPlanModal : StandardLayout
    {
        Dictionary<string, int> weekDict;
        Entry nameEntry;
        Components.Composites.CheckBox AutoFillCheckBox;
        StackLayout nameInputContainer, pickerContainer;
        public CreateMealPlanModal()
        {
            Container = new Grid { }; Content = new Grid { };

            weekDict = new Dictionary<string, int>
            {
                {"1 Week", 1 },
                {"2 Weeks", 2 },
                {"3 Weeks", 3 },
                {"4 Weeks", 4 }
            };

            Label createMealPlanLabel = new Label
            {
                Text = "Create a Meal Plan",
                FontAttributes = FontAttributes.Bold,
                FontSize = Units.FontSizeXXL,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Start
            };

            Label descriptionLabel = new Label
            {
                //Text = "Your current Meal Plan is empty - choose a name for it, select a start date, and the length of your meal plan, then press create.",
                Text = "Choose a name, select a start date, and the length of your meal plan, then press create.",
                FontAttributes = FontAttributes.Bold,
                FontSize = Units.FontSizeM,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Center
            };

            StackLayout createMealPlanLabelContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    createMealPlanLabel,
                    descriptionLabel
                }
            };

            Label nameEntryLabel = new Label
            {
                Text = "Choose a name for your Meal Plan",
                FontSize = Units.FontSizeM,
                FontFamily = Fonts.GetRegularAppFont(),
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Center,
            };

            nameEntry = new Entry
            {
                Placeholder = "Type Text Here",
                PlaceholderColor = Color.LightGray,
                HorizontalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.White,
                FontSize = Units.FontSizeM,
                WidthRequest = Units.HalfScreenWidth,
                HeightRequest = Units.TapSizeS,
                HorizontalOptions = LayoutOptions.Center
            };

            nameInputContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Margin = 5,
                Children =
                {
                    nameEntryLabel,
                    nameEntry
                }
            };

            Label startLabel = new Label
            {
                Text = "Start Date",
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White
            };

            DatePicker startDatePicker = new DatePicker
            {
                Date = DateTime.Now,
                MinimumDate = DateTime.UtcNow,
                TextColor = Color.FromHex(Colors.CC_ORANGE),
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                Format = "dd/MM/yyyy",
                WidthRequest = Units.ScreenWidth30Percent,
                HorizontalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold
            };

            StackLayout datePickerContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                HorizontalOptions = LayoutOptions.Center,
                Margin = 5,
                Children =
                {
                    startLabel,
                    startDatePicker
                }
            };

            Label numberOfWeeksLabel = new Label
            {
                Text = "Number of Weeks",
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Start
            };

            Picker picker = new Picker
            {
                Title = "Meal Plan Duration",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = Units.ScreenWidth45Percent,
                FontSize = Units.FontSizeL,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromHex(Colors.CC_ORANGE),
                TitleColor = Color.White
            };

            foreach (string week in weekDict.Keys)
            {
                picker.Items.Add(week);
            }
            picker.SelectedIndex = 0;

            picker.SelectedIndexChanged += (sender, args) =>
            {
                if (picker.SelectedIndex == -1)
                {
                    // Do nothing
                }
                else
                {
                    string weekID = picker.Items[picker.SelectedIndex];
                    StaticData.chosenWeeks = weekDict[weekID];
                    picker.Title = "";
                }
            };

            pickerContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = 5,
                Children =
                {
                    numberOfWeeksLabel,
                    picker
                }
            };

            StackLayout errorContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                }
            };

            ActiveLabel CloseLabel = new ActiveLabel(AppText.CLOSE, Units.FontSizeS, Color.Transparent, Color.White, null);
            CloseLabel.RightAlign();
            TouchEffect.SetNativeAnimation(CloseLabel.Content, true);
            TouchEffect.SetCommand(CloseLabel.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.HideStartDateMenu();
                });
            }));

            StackLayout closeBtnCont = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.End,
                Children =
                {
                    CloseLabel.Content
                }
            };

            ColourButton createButton = new ColourButton(Color.FromHex(Colors.CC_GREEN), Color.White, AppText.CREATE_BUTTON_TEXT, null);
            createButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            createButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            createButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(createButton.Content, true);
            TouchEffect.SetCommand(createButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (nameEntry.Text == "" || nameEntry.Text == null || picker.SelectedIndex < 0)
                    {
                        App.ShowAlert("Please check if details are correct.");
                    }
                    else
                    {
                        StaticData.startDate = startDatePicker.Date.ToString("M-d-yyyy");
                        StaticData.hypenedDate = startDatePicker.Date.ToString("yyyy-MM-dd");
                        string createdMealPlanId = await App.ApiBridge.CreateMealPlan(AppSession.CurrentUser, nameEntry.Text, StaticData.hypenedDate, (StaticData.chosenWeeks * 7));

                        if (createdMealPlanId != null)
                        {
                            if (AutoFillCheckBox.IsChecked)
                            {
                                Console.WriteLine("Autofill meal plan: " + createdMealPlanId);

                                var result = await App.ApiBridge.AutocompleteMealPlan(AppSession.CurrentUser, createdMealPlanId);
                            }
                            await App.HideRecipeSummary();
                        }
                        else
                        {
                            App.ShowAlert("Could not create Meal Plan.");
                        }
                    }
                });
            }));

            ColourButton createAndAutofillButton = new ColourButton(Color.FromHex(Colors.CC_GREEN), Color.White, AppText.CREATE_BUTTON_TEXT + " AND FILL", null);
            createAndAutofillButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            createAndAutofillButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            createAndAutofillButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            StackLayout buttonContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                WidthRequest = Units.ScreenWidth,//30Percent,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    createButton.Content,
                    //createAndAutofillButton.Content
                }
            };

            AutoFillCheckBox = new Components.Composites.CheckBox("Auto-Fill", "tick.png", "tickbg.png", 320, 32, true);

            AutoFillCheckBox.SetCheckboxLeft();
            AutoFillCheckBox.SetIconSize(Dimensions.CHECKBOX_ICON_SIZE, Dimensions.CHECKBOX_ICON_SIZE);
            AutoFillCheckBox.Title.Content.FontSize = Units.FontSizeM; //Dimensions.CHECKBOX_FONT_SIZE;
            AutoFillCheckBox.Title.Content.FontFamily = Fonts.GetBoldAppFont();
            AutoFillCheckBox.Title.Content.TextColor = Color.White;

            StackLayout ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                WidthRequest = Units.ScreenWidth - Units.ScreenWidth20Percent,
                Children =
                {
                    closeBtnCont,
                    createMealPlanLabelContainer,
                    nameInputContainer,
                    datePickerContainer,
                    pickerContainer,
                    errorContainer,
                    buttonContainer,
                    AutoFillCheckBox.Content
                }
            };

            Container.Children.Add(ContentContainer);
            Content.Children.Add(Container);
        }
    }
}
