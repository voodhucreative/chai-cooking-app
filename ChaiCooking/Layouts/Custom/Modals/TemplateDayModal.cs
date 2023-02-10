using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using ChaiCooking.Views.CollectionViews.MealPlanner;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Modals
{
    public class TemplateDayModal : StandardLayout
    {
        Grid numberGrid { get; set; }
        bool isDeleting { get; set; }
        // Add Days
        public TemplateDayModal(bool isDeleting)
        {
            this.isDeleting = isDeleting;
            StaticData.daysList = new List<TemplateDays>();
            string titleString = isDeleting ? "Remove Days" : "Add Days";
            // If a recipe is present we can add it after making the album I guess.
            Label titleLabel = new Label
            {
                Text = titleString,
                FontSize = Units.FontSizeXL,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold
            };

            StackLayout titleContainer = new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                //BackgroundColor = Color.Orange,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    titleLabel
                }
            };

            Label closeLabel = new Label
            {
                Text = AppText.CLOSE,
                FontSize = Units.FontSizeL,
                TextColor = Color.White,
            };

            TouchEffect.SetNativeAnimation(closeLabel, true);
            TouchEffect.SetCommand(closeLabel,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.HideRecipeSummary();
                });
            }));

            StackLayout closeLabelContainer = new StackLayout
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                //BackgroundColor = Color.Blue,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    closeLabel
                }
            };

            StackLayout titleContent = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                //BackgroundColor = Color.Red,
                Orientation = StackOrientation.Horizontal,

                Children =
                {
                    titleContainer,
                    closeLabelContainer
                }
            };

            StackLayout seperator = new StackLayout { WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) };

            string descLabelText = isDeleting ? "Select the days you want to remove." : "Select the days you wish to add.";

            Label descLabel = new Label
            {
                Text = descLabelText,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White,
                FontSize = Units.FontSizeL
            };

            Label desc2Label = new Label
            {
                Text = "Select days:",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White
            };

            numberGrid = new Grid
            {
                ColumnSpacing = 2,
                RowSpacing = 2,
                ColumnDefinitions =
                {
                    { new ColumnDefinition { Width = new GridLength(50)}},
                    { new ColumnDefinition { Width = new GridLength(50)}},
                    { new ColumnDefinition { Width = new GridLength(50)}},
                    { new ColumnDefinition { Width = new GridLength(50)}},
                },
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(50)}},
                    { new RowDefinition { Height = new GridLength(50)}},
                    { new RowDefinition { Height = new GridLength(50)}},
                    { new RowDefinition { Height = new GridLength(50)}},
                    { new RowDefinition { Height = new GridLength(50)}},
                    { new RowDefinition { Height = new GridLength(50)}},
                    { new RowDefinition { Height = new GridLength(50)}},
                }
            };


            StackLayout numberCont = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Spacing = 0,
                Children =
                {
                    numberGrid
                }
            };

            CreateNumberGrid();

            ColourButton confirmBtn = new ColourButton
(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CONFIRM, null);
            confirmBtn.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            confirmBtn.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            confirmBtn.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(confirmBtn.Content, true);
            TouchEffect.SetCommand(confirmBtn.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (StaticData.daysList.Count == 0) { App.ShowAlert("Please select a day."); }
                    else
                    {
                        foreach (TemplateDays s in StaticData.daysList)
                        {
                            if (isDeleting)
                            {
                                await Task.Delay(100);
                                var dayObj = await App.ApiBridge.DeleteDayTemplateOnMealPlan(AppSession.CurrentUser, StaticData.currentTemplateID, s.templateID.ToString());
                                var selectedIndex = AppSession.mealTemplate.Data.Select((item, index) => (item, index)).First(x => x.item.day_Number == int.Parse(s.dayNumber)).index;
                                AppSession.mealTemplate.Data.RemoveAt(selectedIndex);
                                var mealPlannerGroup = new MealPlannerCollectionViewSection(AppSession.mealTemplate.Data);
                                AppSession.mealPlannerCollection.Add(mealPlannerGroup);
                                AppSession.mealPlannerCollection.RemoveAt(0);

                            }
                            else
                            {
                                await Task.Delay(100);
                                //Surrounding in try catch as the bug is not replicable anymore.
                                try
                                {
                                    //CRASH HERE - Null reference 
                                    MealPlanModel.Datum createdDay = await App.ApiBridge.CreateDayTemplateOnMealPlan(AppSession.CurrentUser, StaticData.currentTemplateID, s.dayNumber);
                                    int selectedIndex = AppSession.mealTemplate.Data.FindIndex(x => x.day_Number > createdDay.day_Number);
                                    //END CRASH

                                    if (selectedIndex == -1) { selectedIndex = AppSession.mealTemplate.Data.Count; }

                                    AppSession.mealTemplate.Data.Insert(selectedIndex, createdDay);
                                    var mealPlannerGroup = new MealPlannerCollectionViewSection(AppSession.mealTemplate.Data);
                                    AppSession.mealPlannerCollection.Add(mealPlannerGroup);
                                    AppSession.mealPlannerCollection.RemoveAt(0);
                                }
                                catch(Exception e)
                                {
                                    //Force a break if in debug mode
                                    Console.WriteLine(e);
                                    System.Diagnostics.Debugger.Break();
                                }
                            }
                        }
                        string alert = isDeleting ? "Days removed successfully." : "Days added successfully";
                        App.ShowAlert(alert);
                        await App.HideModalAsync();
                    }
                });
            }));

            StackLayout btnCont = new StackLayout
            {
                WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Children =
                {
                    confirmBtn.Content
                }
            };

            StackLayout masterContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                WidthRequest = 300,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                Spacing = 0,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                Children =
                {
                    titleContent,
                    seperator,
                    descLabel,
                    desc2Label,
                    numberCont,
                    btnCont
                }
            };

            Frame frame = new Frame
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Content = masterContainer,
                CornerRadius = 5,
                IsClippedToBounds = true,
                Padding = 0,
            };

            Content.Children.Add(frame);
        }

        public async void CreateNumberGrid()
        {
            var temp = AppSession.mealTemplate;
            // Create the empty model
            MealPlanModel mealPlanModel = new MealPlanModel();
            mealPlanModel.Data = new List<MealPlanModel.Datum>();
            for (int i = 0; i < 28; i++)
            {
                MealPlanModel.Datum datum = new MealPlanModel.Datum();
                datum.day_Number = i + 1;
                mealPlanModel.Data.Add(datum);
            }
            // Populate the empty model
            int col = 0;
            int row = 0;
            foreach (MealPlanModel.Datum d in mealPlanModel.Data)
            {
                TemplateDayNumberTile numberTile = new TemplateDayNumberTile();
                var match = temp.Data.FirstOrDefault(x => x.day_Number == d.day_Number);
                if (match != null)
                {
                    numberTile.SetTile(dayTemplateID: match.id, input: match.day_Number.ToString(), dayExists: false, isDeleting: isDeleting);
                }
                else
                {
                    numberTile.SetTile(dayTemplateID: -1, input: d.day_Number.ToString(), dayExists: true, isDeleting: isDeleting);
                }

                numberGrid.Children.Add(numberTile.Content, col, row);
                col++;
                if (col == 4) { row++; col = 0; }
            }

        }
    }
}
