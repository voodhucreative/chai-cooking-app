using ChaiCooking.AppData;
using ChaiCooking.Components;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.MealPlanAPI;
using ChaiCooking.Services;
using ChaiCooking.Views.CollectionViews.MealPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Tiles
{
    class MealPlannerTile : ActiveComponent
    {
        StackLayout masterContainer, mealPeriodContainer, imgCont;

        public Label mealPeriod { get; set; }

        public Label duration { get; set; }

        public Label calories { get; set; }

        public Label nameLabel { get; set; }

        public Label totalTimeLabel { get; set; }

        public StaticImage recipeImage { get; set; }

        public int mealTypeID { get; set; }

        public int mealPlanID { get; set; }

        public int mealID { get; set; }

        public string date { get; set; }

        public Recipe recipe { get; set; }

        bool hasRecipe = false;

        bool isCalendarTile { get; set; }

        bool isTemplate { get; set; }

        public int templateID { get; set; }

        public int mealTemplateID { get; set; }

        public int dayTemplateID { get; set; }

        public int rowIndex { get; set; }

        StaticImage timeIcon;
        Frame frame;
        Label iconLabel;

        public MealPlannerTile(bool isSnack, int width, int height)
        {
            hasRecipe = isSnack;
            StaticData.isEditing = true;
            if (hasRecipe)
            {
                // New and Improved Tile Code - really!? MH
                mealPeriodContainer = new StackLayout
                {
                    BackgroundColor = Color.White,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
                    WidthRequest = 100,
                    HeightRequest = 20,
                    IsClippedToBounds = true
                };

                mealPeriod = getMealPeriodLabel();

                mealPeriodContainer.Children.Add(mealPeriod);

                timeIcon = new StaticImage("timer.png", 10, 10, null);

                timeIcon.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                timeIcon.Content.VerticalOptions = LayoutOptions.FillAndExpand;

                totalTimeLabel = getTotalTimeLabel();

                StackLayout timeDetCont = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    WidthRequest = 50,
                    HeightRequest = 35,
                    Orientation = StackOrientation.Horizontal,
                    BackgroundColor = Color.White,
                    Padding = 1,
                    Spacing = 1,
                    Children =
                    {
                        timeIcon.Content,
                        totalTimeLabel
                    }
                };

                recipeImage = getImage();

                imgCont = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start,
                    WidthRequest = 50,
                    HeightRequest = 35,
                    Orientation = StackOrientation.Vertical,
                    Margin = 1,
                };

                //imgCont.Children.Add(recipeImage.Content);

                Grid splitGrid = new Grid
                {
                    ColumnDefinitions = {
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                    },
                    RowDefinitions =
                    {
                        new RowDefinition { Height = new GridLength(35) }
                    },
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
                    BackgroundColor = Color.White,
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                    IsClippedToBounds = true,
                    Children =
                    {
                        { timeDetCont, 0, 0 },
                        { imgCont, 1, 0 }
                    }
                };
                Grid.SetRowSpan(imgCont, 2);

                nameLabel = getMealNameLabel();

                StackLayout titleCont = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start,
                    WidthRequest = 100,
                    HeightRequest = 35,
                    Orientation = StackOrientation.Vertical,
                    BackgroundColor = Color.Black,
                    IsClippedToBounds = true,
                    Padding = 1,
                    Children =
                    {
                        nameLabel
                    }
                };

                masterContainer = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    BackgroundColor = Color.White,
                    IsClippedToBounds = true,
                    Spacing = 0,
                    Children =
                {
                        mealPeriodContainer,
                        splitGrid,
                        titleCont
                }
                };

                frame = new Frame
                {
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start,
                    Content = masterContainer,
                    CornerRadius = 5,
                    IsClippedToBounds = true,
                    Padding = 0,
                };

                Container.GestureRecognizers.Add(new DropGestureRecognizer()
                {
                    AllowDrop = true,
                    DropCommand = new Command(() =>
                    {
                        Device.BeginInvokeOnMainThread(async ()
                         =>
                        {
                            App.SetLoadingMessage(AppText.UPDATING);
                            await App.ShowLoading();
                            await Task.Delay(500);
                            await App.HideLoading();

                            if (isCalendarTile)
                            {
                                if (DateTime.Parse(date) < DateTime.Today)
                                {
                                    //Editing a day in the past, show an error message
                                    App.DisplayAlert("Oh!", "You've tried to edit a meal in the past", "Ok");
                                    return;
                                }
                            }

                            if (!isTemplate)
                            {
                                var editResult = await App.ApiBridge.UpdateMealOnMealPlan(
                                                                    AppSession.CurrentUser,
                                                                    mealID.ToString(),
                                                                    mealPlanID.ToString(),
                                                                    AppSession.SelectedRecipe.chai.Id.ToString(),
                                                                    AppSession.SelectedRecipe.Id);
                                if (editResult != null)
                                {
                                    AppSession.SelectedRecipe = null;
                                    foreach (Meal meal in AppSession.mealPlannerCalendar.Data[rowIndex].Meals.Where(x => x.Id == editResult.Id))
                                    {
                                        meal.Id = editResult.Id;
                                        meal.MealType = editResult.MealType;
                                        meal.Recipe = editResult.Recipe;
                                        meal.violatesMealPlan = editResult.violatesMealPlan;
                                    }
                                    await UpdateMealPlan();
                                }
                                else
                                {
                                    App.SetLoadingMessage(AppText.FAILED_TO_ADD_MEAL);
                                    App.ShowAlert(AppText.FAILED_TO_ADD_MEAL);
                                }
                                
                            }
                            else
                            {
                                var result = await App.ApiBridge.UpdateMealOnDayTemplate(AppSession.CurrentUser,
                                dayTemplateID, mealTemplateID, mealPeriod.Text, (int)AppSession.SelectedRecipe.chai.Id, AppSession.SelectedRecipe.Id, AppSession.SelectedRecipe.MainImageSource);
                                if (result != null)
                                {
                                    AppSession.SelectedRecipe = null;

                                    var match = AppSession.mealTemplate.Data.FirstOrDefault(x => x.id == dayTemplateID);
                                    if (match != null)
                                    {
                                        foreach (MealTemplate meal in match.mealTemplates.Where(y => y.mealType == result.mealType))
                                        {
                                            meal.id = result.id;
                                            meal.recipe = result.recipe;
                                            meal.whiskID = result.recipe.WhiskRecipeId;
                                        }
                                        var mealPlannerGroup = new MealPlannerCollectionViewSection(AppSession.mealTemplate.Data);
                                        AppSession.mealPlannerCollection.Add(mealPlannerGroup);
                                        AppSession.mealPlannerCollection.RemoveAt(0);
                                    }
                                    else
                                    {
                                        App.SetLoadingMessage(AppText.FAILED_TO_ADD_MEAL);
                                        App.ShowAlert(AppText.FAILED_TO_ADD_MEAL);
                                    }
                                }
                                else
                                {
                                    App.SetLoadingMessage(AppText.FAILED_TO_ADD_MEAL);
                                    App.ShowAlert(AppText.FAILED_TO_ADD_MEAL);
                                }
                            }   
                        });
                    })
                });

                TouchEffect.SetNativeAnimation(Container, true);
                TouchEffect.SetCommand(Container,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {

                        if (isCalendarTile)
                        {
                            if (DateTime.Parse(date) < DateTime.Today)
                            {

                                if (recipe != null)
                                {
                                    await App.ShowFullRecipe(recipe, false, false);
                                }
                                return;
                            }
                        }

                        StaticData.isEditing = true;
                        if (!isTemplate)
                        {
                            await App.ShowEditMeal(rowIndex, mealID, mealPlanID, recipe, mealPeriod: mealPeriod.Text, true, date);
                        }
                        else
                        {
                            Recipe newRecipe = await App.ApiBridge.GetRecipe(recipe.WhiskRecipeId);
                            Console.WriteLine(newRecipe);
                            if (newRecipe.Id != null)
                            {
                                await App.ShowEditMeal(mealTemplateID, dayTemplateID, mealPeriod.Text, newRecipe, true, isTemplate);
                            }
                            else
                            {
                                await App.ShowEditMeal(mealTemplateID, dayTemplateID, mealPeriod.Text, recipe, true, isTemplate);
                            }
                        }
                    });
                }));

                Content.Children.Add(frame);
                Content.WidthRequest = 100;
            }
            else
            {
                StackLayout iconContainer = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    BackgroundColor = Color.Transparent,
                    WidthRequest = 100,
                    HeightRequest = 90,
                    Children =
                    {
                    }
                };

                TouchEffect.SetNativeAnimation(Container, true);
                TouchEffect.SetCommand(Container,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        // Open Recipe Menu Here.
                        //Shows the pop up when you tap into your meal in the calendar
                        //if (isCalendarTile)
                        //{
                        //    if (AppSession.notifiedCalendarChange != true)
                        //    {
                        //        App.DisplayAlert("Oh!", "If you want to edit your calendar thats ok, but please be aware it could reduce the effectivness of your plan.", "Ok");
                        //        AppSession.notifiedCalendarChange = true;

                        //    }
                        //}
                        if (isCalendarTile)
                        {
                            if (DateTime.Parse(date) < DateTime.Today)
                            {
                                //Editing a day in the past, show an error message
                                App.DisplayAlert("Oh!", "You've tried to edit a meal in the past", "Ok");
                                return;
                            }
                        }

                        StaticData.isEditing = false;
                            if (!isTemplate)
                            {
                                await App.ShowEditMeal(rowIndex, GetMealType(), date, mealPlanID, false, isCalendarTile);
                            }
                            else
                            {
                                await App.ShowEditMeal(dayTemplateID, mealTypeID, false, isTemplate);
                            }
                        //}
                        //else
                        //{
                        //    App.ShowAlert("Sorry!", "Not yet available");
                        //    await App.ShowEditMeal(dayTemplateID, mealTypeID, false, true);
                        //}
                    });
                }));

                Container.GestureRecognizers.Add(new DropGestureRecognizer()
                {
                    AllowDrop = true,
                    DropCommand = new Command(() =>
                    {
                        Device.BeginInvokeOnMainThread(async ()
                         =>
                        {
                            App.SetLoadingMessage(AppText.UPDATING);
                            await App.ShowLoading();
                            await Task.Delay(500);
                            await App.HideLoading();

                            if (isCalendarTile)
                            {
                                if (DateTime.Parse(date) < DateTime.Today)
                                {
                                    //Editing a day in the past, show an error message
                                    App.DisplayAlert("Oh!", "You've tried to edit a meal in the past", "Ok");
                                    return;
                                }
                            }

                            StaticData.isEditing = false;
                            if (isCalendarTile && AppSession.notifiedCalendarChange != true)
                            {
                                App.DisplayAlert("Oh!","If you want to edit your calendar thats ok, but please be aware it could reduce the effectivness of your plan.","Ok");
                                AppSession.notifiedCalendarChange = true;
                                
                            }
                                if (!isTemplate)
                                {
                                    var result = await App.ApiBridge.AddMealToMealPlan(
                                                                                    AppSession.CurrentUser,
                                                                                    GetMealPeriod(mealTypeID),
                                                                                    mealPlanID.ToString(),
                                                                                    date,
                                                                                    AppSession.SelectedRecipe.chai.Id.ToString(),
                                                                                    AppSession.SelectedRecipe.Id,
                                                                                    isCalendarTile);
                                    if (result != null)
                                    {
                                        AppSession.SelectedRecipe = null;
                                        AppSession.mealPlannerCalendar.Data[rowIndex].Meals.Add(result);
                                        await UpdateMealPlan();
                                    }
                                    else
                                    {
                                        App.SetLoadingMessage(AppText.FAILED_TO_ADD_MEAL);
                                        App.ShowAlert(AppText.FAILED_TO_ADD_MEAL);
                                    }
                                }
                                else
                                {
                                    var result = await App.ApiBridge.AddMealToDayTemplate(
                                                                            AppSession.CurrentUser,
                                                                            dayTemplateID,
                                                                            GetMealPeriod(mealTypeID),
                                                                            (int)AppSession.SelectedRecipe.chai.Id,
                                                                            AppSession.SelectedRecipe.Id,
                                                                            AppSession.SelectedRecipe.MainImageSource);
                                    if (result != null)
                                    {
                                        foreach (MealPlanModel.Datum datum in AppSession.mealTemplate.Data.Where(x => x.id == dayTemplateID))
                                        {
                                            datum.mealTemplates.Add(result);
                                        }
                                        var mealPlannerGroup = new MealPlannerCollectionViewSection(AppSession.mealTemplate.Data);
                                        AppSession.mealPlannerCollection.Add(mealPlannerGroup);
                                        AppSession.mealPlannerCollection.RemoveAt(0);
                                        await App.HideModalAsync();
                                    }
                                    else
                                    {
                                        App.SetLoadingMessage(AppText.FAILED_TO_ADD_MEAL);
                                        App.ShowAlert(AppText.FAILED_TO_ADD_MEAL);
                                    }
                                }
                            
                        });
                    })
                });

                Frame newframe = new Frame
                {
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start,
                    Content = iconContainer,
                    CornerRadius = 5,
                    IsClippedToBounds = true,
                    Padding = 0,
                    BackgroundColor = Color.White.MultiplyAlpha(0.5),
                };

                Content.Children.Add(newframe);
            }
        }

        public Label getMealPeriodLabel()
        {
            return mealPeriod = new Label
            {
                Text = "",
                TextColor = Color.Orange,
                FontSize = Units.FontSizeM,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                WidthRequest = 100
            };
        }

        public Label getMealNameLabel()
        {
            return nameLabel = new Label
            {
                Text = "",
                TextColor = Color.White,
                FontSize = Units.FontSizeS,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
            };
        }

        public Label getTotalTimeLabel()
        {
            return totalTimeLabel = new Label
            {
                Text = "",
                TextColor = Color.Orange,
                FontSize = Units.FontSizeS,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
            };
        }

        public StaticImage getImage()
        {
            return recipeImage;
        }

        public void SetInternalDate(string input)
        {
            this.date = input;
        }

        public void SetPrevIngredients(Ingredient[] input)
        {
            List<Ingredient> ingredientsList = new List<Ingredient>();

            foreach (Ingredient r in input)
            {
                Ingredient newIngredient = new Ingredient();
                newIngredient.Text = r.Text;
                ingredientsList.Add(newIngredient);
            }
        }

        public int GetMealType()
        {
            return this.mealTypeID;
        }

        public void SetMealPlanID(int input)
        {
            this.mealPlanID = input;
        }

        public void SetMealID(int input)
        {
            this.mealID = input;
        }

        public void SetMealType(int input)
        {
            this.mealTypeID = input;
        }

        public void SetMealPeriod(string input)
        {
            this.mealPeriod.Text = input;
        }

        public void SetRecipe(Recipe r)
        {
            if (r.Images != null)
            {
                recipeImage = new StaticImage(r.Images[0].Url.AbsoluteUri, 50, 35, null);
                recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                imgCont.Children.Add(recipeImage.Content);
            }

            if (r.Images == null && r.chai != null && r.chai.CoreIngredient != null)
            {
                try
                {
                    recipeImage = new StaticImage(r.chai.CoreIngredient.ImageUrl.AbsoluteUri, 50, 35, null);
                    recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                    recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                    imgCont.Children.Add(recipeImage.Content);
                }
                catch
                {
                    recipeImage = new StaticImage("chaismallbag.png", 50, 35, null);
                    recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                    recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                    imgCont.Children.Add(recipeImage.Content);
                }
            }

            if (r.MainImageSource != null)
            {
                recipeImage = new StaticImage(r.MainImageSource, 50, 35, null);
                recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                imgCont.Children.Add(recipeImage.Content);
            }
            else
            {
                recipeImage = new StaticImage("chaismallbag.png", 50, 35, null);
                recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                imgCont.Children.Add(recipeImage.Content);
            }

            if (r.Durations != null)
            {
                this.totalTimeLabel.Text = $"{r.Durations.TotalTime}m";
            }
            else
            {
                this.totalTimeLabel.Text = "0m";
            }

            if (r.Name == null || r.Name == "")
            {
                this.nameLabel.Text = r.chai.Name;
            }
            else
            {
                this.nameLabel.Text = r.Name;
            }

            this.recipe = r;
            this.recipe.WhiskRecipeId = r.WhiskRecipeId;
        }

        public void SetCalendarTile(bool input)
        {
            if (!hasRecipe)
            {
                if (iconLabel != null)
                {
                    iconLabel.IsVisible = false;
                }
                this.Content.Opacity = 0.5;
            }
            isCalendarTile = input;
        }

        public void SetIsTemplate(bool input)
        {
            this.isTemplate = input;
        }

        public void SetTemplateID(int input)
        {
            this.templateID = input;
        }

        public void SetMealTemplateID(int input)
        {
            this.mealTemplateID = input;
        }

        public void SetDayTemplateID(int input)
        {
            this.dayTemplateID = input;
        }

        public void SetRowIndex(int input)
        {
            this.rowIndex = input;
        }

        private string GetMealPeriod(int input)
        {
            switch (input)
            {
                case 0:
                    return "breakfast";
                case 1:
                    return "lunch";
                case 2:
                    return "dinner";
                case 3:
                    return "snacks";
                default:
                    return null;
            }
        }


        private async Task<bool> UpdateMealPlan()
        {
            var mealPlannerGroup = new MealPlannerCollectionViewSection(AppSession.mealPlannerCalendar.Data);
            AppSession.mealPlannerCollection.RemoveAt(0);
            AppSession.mealPlannerCollection.Add(mealPlannerGroup);
            return true;
        }
    }
}
