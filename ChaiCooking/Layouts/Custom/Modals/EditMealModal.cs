using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.Feed;
using ChaiCooking.Models.Custom.MealPlanAPI;
using ChaiCooking.Services;
using ChaiCooking.Tools;
using ChaiCooking.Views.CollectionViews.AddEdit;
using ChaiCooking.Views.CollectionViews.Calendar;
using ChaiCooking.Views.CollectionViews.MealPlanner;
using FFImageLoading.Transformations;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Modals
{
    public class EditMealModal : StandardLayout
    {
        EditMealCollectionView editMealCollectionView;
        CollectionView recipesLayout;
        StackLayout pageContainer, searchInputContainer, selectRecipeCont;
        //Grid rightCont;
        StackLayout rightCont;
        Action updatePageAction;
        CustomEntry searchEntry;
        Grid masterGrid;
        Recipe selectedRecipe;
        StaticLabel pageNum;
        StaticImage DownArrow, UpArrow;
        bool isSearched;
        bool isCalendar;
        bool isEditing { get; set; }
        bool isFavourites { get; set; }
        int mealID { get; set; }
        int mealPlanId { get; set; }
        Recipe recipe { get; set; }
        string mealPeriod { get; set; }
        string date { get; set; }
        string filter;
        int mealTemplateID { get; set; }
        int dayTemplateID { get; set; }
        int rowIndex { get; set; }
        RecipePreviewLayout recipePreviewLayout;
        ActiveImage infoImage;
        StaticImage removeIcon;

        ActiveImage InfoIcon;

        Label closeLabel, sortLabel;

        // Populated Meal Plan
        public EditMealModal(int index, int mealID, int mealPlanId, Recipe recipe, string mealPeriod, bool isEditing, string date)
        {
            Container = new Grid { }; Content = new Grid { };
            isSearched = false;
            this.mealID = mealID;
            this.mealPlanId = mealPlanId;
            this.recipe = recipe;
            this.mealPeriod = mealPeriod;
            this.isEditing = isEditing;
            this.date = date;
            this.rowIndex = index;
            recipePreviewLayout = new RecipePreviewLayout();
            BuildEditModalNew();
        }
        // Empty Meal Plan
        public EditMealModal(int index, int mealPeriod, string date, int mealPlanId, bool isEditing, bool isCalendar = false)
        {
            recipePreviewLayout = new RecipePreviewLayout();
            StaticData.chaiRecipeID = null;
            StaticData.whiskRecipeID = null;
            this.recipe = null;
            Container = new Grid { }; Content = new Grid { };
            isFavourites = false;
            isSearched = false;
            this.isCalendar = isCalendar;
            this.mealPlanId = mealPlanId;
            this.rowIndex = index;
            switch (mealPeriod)
            {
                case 0:
                    this.mealPeriod = "breakfast";
                    break;
                case 1:
                    this.mealPeriod = "lunch";
                    break;
                case 2:
                    this.mealPeriod = "dinner";
                    break;
                case 3:
                    this.mealPeriod = "snacks";
                    break;
            }
            this.isEditing = isEditing;
            this.date = date;
            BuildEditModalNew();
        }

        // Meal Templates Constructor
        public EditMealModal(int mealTemplate, int dayTemplateID, string mealType, Recipe recipe, bool isEditing, bool isTemplate)
        {
            recipePreviewLayout = new RecipePreviewLayout();
            Container = new Grid { }; Content = new Grid { };
            StaticData.chaiRecipeID = recipe.Id;
            StaticData.whiskRecipeID = recipe.WhiskRecipeId;
            this.recipe = null;
            isSearched = false;
            this.recipe = recipe;
            this.mealPeriod = mealType;
            this.isEditing = isEditing;
            this.mealTemplateID = mealTemplate;
            this.dayTemplateID = dayTemplateID;
            BuildEditModalNew();
        }
        // Meal Templates Constructor Empty Tile
        public EditMealModal(int dayTemplateID, int mealType, bool isEditing, bool isTemplate)
        {
            recipePreviewLayout = new RecipePreviewLayout();
            Container = new Grid { }; Content = new Grid { };
            isSearched = false;
            this.recipe = recipe;
            switch (mealType)
            {
                case 0:
                    this.mealPeriod = "breakfast";
                    break;
                case 1:
                    this.mealPeriod = "lunch";
                    break;
                case 2:
                    this.mealPeriod = "dinner";
                    break;
                case 3:
                    this.mealPeriod = "snacks";
                    break;
            }
            this.isEditing = isEditing;
            this.dayTemplateID = dayTemplateID;
            BuildEditModalNew();
        }

        public void BuildEditModalNew()
        {
            Action<Recipe> selectedAction = (r) =>
            {
                if (AppSession.InfoModeOn)
                {
                    App.ShowInfoBubble(new Paragraph("Swap", "Tap to swap them in the plan. CHAI will recalculate the overall Meal Plan for you if you edit too many options and we will prompt you if you go over your transition or flexitarian plans.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                }
                else
                {
                    SwitchSelectedRecipe(r);
                }
            };

            StaticData.setPreviewRecipe = selectedAction;

            AppSession.editMealRecipesCollection = new ObservableCollection<MealsCollectionViewSection>();

            updatePageAction = StaticData.buildPage;

            closeLabel = new Label
            {
                Text = AppText.CLOSE,
                FontSize = Units.FontSizeL,
                TextColor = Color.White,
                FontFamily = Fonts.GetBoldAppFont(),
                VerticalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.End,
                HorizontalTextAlignment = TextAlignment.End
            };

            TouchEffect.SetNativeAnimation(closeLabel, true);
            TouchEffect.SetCommand(closeLabel,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    AppSession.InfoModeOn = false;
                    recipe = null;
                    StaticData.selectedRecipe = null;
                    StaticData.whiskRecipeID = null;
                    StaticData.chaiRecipeID = null;
                    ResetPages();
                    await App.HideAddRecipe();
                    await App.HideLoading();
                });
            }));

            sortLabel = new Label
            {
                Text = "Sort..",
                FontSize = Units.FontSizeM,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.White,
                Margin = new Thickness(8, 0),
                VerticalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.End,
                HorizontalTextAlignment = TextAlignment.End
            };

            TouchEffect.SetNativeAnimation(sortLabel, true);
            TouchEffect.SetCommand(sortLabel,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    CheckResult();
                });
            }));

            /*StackLayout closeBtnCont = new StackLayout
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    closeLabel
                }
            };*/

            removeIcon = new StaticImage("trash.png", 24, 24, null);
            removeIcon.Content.HorizontalOptions = LayoutOptions.End;
            removeIcon.Content.VerticalOptions = LayoutOptions.Start;
            removeIcon.Content.Margin = 4;

            TouchEffect.SetNativeAnimation(removeIcon.Content, true);
            TouchEffect.SetCommand(removeIcon.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("Remove", "Tap to remove the current recipe.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                    }
                    else
                    {
                        Content.Children.Clear();
                        BuildRemoveMealConfirm(mealID, mealPlanId, recipe, mealPeriod);
                    }
                });
            }));

            string title = "";


            
            //if (isEditing)
            //{
            //    title = "Edit Meal (" + DateTime.Parse(this.date).ToShortDateString() + ")";
            //}
            //else
            //{
            //    title = "Add Meal - (" + DateTime.Parse(this.date).ToShortDateString() + ")";
            //}

            Label editMealLabel = new Label
            {
                Text = "",//title,
                TextColor = Color.White,
                FontSize = Units.FontSizeL,
                WidthRequest = Units.ScreenWidth - 180,
                FontFamily = Fonts.GetBoldAppFont(),
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center,
                
            };

            editMealLabel.FormattedText = new FormattedString();
            //Duplication.
            if (isEditing)
            {
                editMealLabel.FormattedText.Spans.Add(new Span { Text = "Edit Meal", TextColor = Color.White, FontFamily = Fonts.GetRegularAppFont(), FontSize = Units.FontSizeL });
            }
            else
            {
                editMealLabel.FormattedText.Spans.Add(new Span { Text = "Add Meal", TextColor = Color.White, FontFamily = Fonts.GetBoldAppFont(), FontSize = Units.FontSizeXL });
            }
            if (date != null)
            {
                editMealLabel.FormattedText.Spans[0].Text += " on:   ";
                editMealLabel.FormattedText.Spans.Add(new Span { Text = DateTime.Parse(this.date).ToShortDateString(), TextColor = Color.FromHex(Colors.CC_ORANGE), FontFamily = Fonts.GetBoldAppFont(), FontSize = Units.FontSizeL });
            }

            InfoIcon = new ActiveImage("infoicon.png", Dimensions.STANDARD_ICON_WIDTH, Dimensions.STANDARD_ICON_HEIGHT, null, null);
            InfoIcon.Image.HorizontalOptions = LayoutOptions.EndAndExpand;
            InfoIcon.Image.VerticalOptions = LayoutOptions.CenterAndExpand;
            InfoIcon.Content.Margin = Dimensions.GENERAL_COMPONENT_PADDING;

            TouchEffect.SetNativeAnimation(InfoIcon.Content, true);
            TouchEffect.SetCommand(InfoIcon.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        AppSession.InfoModeOn = !AppSession.InfoModeOn;
                        if (AppSession.InfoModeOn)
                        {
                            App.ShowInfoBubble(new Paragraph("Manage Meal Plans", "Manage Meal Plans Manage which Meal Plan to use, delete unwanted ones. Tap centrally on the name of the Meal Plan to open a Meal Plan you've downloaded or created, and you can switch to a different one.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                            InfoIcon.Image.Source = "infoiconon.png";
                        }
                        else
                        {
                            InfoIcon.Image.Source = "infoicon.png";
                        }
                    });
                }));



            StackLayout topGrid = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Children =
                {
                    editMealLabel,
                    
                    InfoIcon.Content,
                    closeLabel,
                }
            };
            /*
            Grid topGrid = new Grid
            {
                RowSpacing = 0,
                ColumnSpacing = 0,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                ColumnDefinitions =
                {
                    //{ new ColumnDefinition { Width = new GridLength(32)}},
                    { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)}},
                    { new ColumnDefinition { Width = new GridLength(32)}},
                    { new ColumnDefinition { Width = new GridLength(64)}},
                },
                Children =
                {
                    //{ removeIcon.Content, 0, 0 },
                    { editMealLabel, 0 , 0 },
                    { InfoIcon.Content, 1 , 0 },
                    { closeLabel, 2 , 0 }
                }
            };*/

            /*StackLayout topGrid = new StackLayout
            {
                WidthRequest = Units.ScreenWidth,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    removeIcon.Content,
                    editMealLabel,
                    closeLabel
                }
            };*/

            Label mealTypeLabel = new Label
            {
                Text = TextTools.FirstCharToUpper(mealPeriod),
                TextColor = Color.White,
                FontFamily = Fonts.GetBoldAppFont(),
                FontSize = Units.FontSizeL,
                HorizontalTextAlignment = TextAlignment.Center,
            };

            Label mealTypeSmallLabel = new Label
            {
                Text = "Meal Type:",
                TextColor = Color.White,
                FontSize = Units.FontSizeL,
                HorizontalTextAlignment = TextAlignment.Start,
            };

            StackLayout mealTypeCont = new StackLayout
            {
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    mealTypeSmallLabel,
                    mealTypeLabel
                }
            };

            Label currentRecipeLabel = new Label
            {
                Text = "Current Recipe:",
                TextColor = Color.White,
                FontSize = Units.FontSizeL,
                HorizontalTextAlignment = TextAlignment.Start,
            };

            StackLayout currentRecipeLabelCont = new StackLayout
            {
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                Orientation = StackOrientation.Horizontal,
                WidthRequest = 320,
                Children =
                {
                    currentRecipeLabel,
                    removeIcon.Content
                }
            };

            PreviewTile previewTile = new PreviewTile();

            if (recipe != null)
            {
                previewTile.SetRecipe(recipe);
                Action<Recipe> showRecipeView = (r) =>
                {
                    BuildRecipeView(recipe);
                };
                previewTile.SetAction(showRecipeView);
                if (recipe.Name != null)
                {
                    previewTile.SetName(recipe.Name);
                }
                else
                {
                    previewTile.SetName(recipe.chai.Name);
                }
                previewTile.SetTime(recipe.CookingTime);

                if (recipe.Images != null)
                {
                    previewTile.SetImageSearched(recipe.Images[0]);
                }
                else if (recipe.MainImageSource != null)
                {
                    previewTile.SetImage(recipe.MainImageSource);
                }
                else if (recipe.Images == null && recipe.chai != null && recipe.chai.CoreIngredient != null)
                {
                    try
                    {
                        previewTile.SetImage(recipe.chai.CoreIngredient.ImageUrl);
                    }
                    catch
                    {
                        previewTile.SetImageSearched(null);
                    }
                }
                else
                {
                    previewTile.SetImageSearched(null);
                }

                this.selectedRecipe = recipe;
            }

            try
            {
                StaticData.whiskRecipeID = recipe.Id;
            }
            catch { }
            try
            {
                StaticData.chaiRecipeID = recipe.chai.Id.ToString();
            }
            catch { }

            selectRecipeCont = new StackLayout
            {
               // WidthRequest = 105,
               // HeightRequest = 105,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = 1,
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                Children =
                {
                }
            };

            selectRecipeCont.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (recipe == null)
                        {
                            App.ShowAlert("Please select a recipe.");
                        }
                    });
                })
            });

            if (recipe != null)
            {
                selectRecipeCont.Children.Add(previewTile.Content);
                selectRecipeCont.Children.Add(removeIcon.Content); 
            }

            Label descriptionLabel = new Label
            {
                Text = "Enter a keyword into the search box below to find the best matching recipe(s).",
                TextColor = Color.White,
                FontSize = Units.FontSizeL,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
            };

            StackLayout descriptionLabelCont = new StackLayout
            {
                WidthRequest = Units.ScreenWidth - 145,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                //BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY),
                Children =
                {
                    descriptionLabel,
                }
            };

            Label searchEntryLabel = new Label
            {
                Text = "Search a recipe for your Meal Plan",
                FontSize = Units.FontSizeM,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Center,
            };

            searchEntry = new CustomEntry
            {
                Placeholder = "Type Text Here",
                PlaceholderColor = Color.LightGray,
                HorizontalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.White,
                WidthRequest = Units.HalfScreenWidth,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = Units.TapSizeS,
                FontSize = Units.FontSizeL,
            };

            ColourButton searchButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE),
                 Color.White, "Search", null);
            searchButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            searchButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            searchButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            TouchEffect.SetNativeAnimation(searchButton.Content, true);
            TouchEffect.SetCommand(searchButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("Search", "Search for a recipe or enter a keyword in the search box, then tap Search and your options will appear in the sidebar.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                    }
                    else
                    {
                        if (searchEntry.Text == "" || searchEntry.Text == null)
                        {
                            App.ShowAlert("Please check if details are correct.");
                        }
                        else
                        {
                            await App.PerformRecipeSearch(searchEntry.Text);
                            isSearched = true;
                            isFavourites = false;
                            ResetPages();
                            await UpdateData();
                            RefreshCollectionView();
                            SetPageTotal();
                        }
                    }
                });
            }));

            ColourButton resetButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE),
                 Color.White, "Reset", null);
            resetButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            resetButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            resetButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            TouchEffect.SetNativeAnimation(resetButton.Content, true);
            TouchEffect.SetCommand(resetButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("Reset", "Tap Reset and it will undo your filters.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                    }
                    else
                    {
                        await App.ShowLoading();
                        isSearched = false;
                        searchEntry.Text = "";
                        StaticData.chaiRecipeID = null;
                        StaticData.whiskRecipeID = null;
                        selectRecipeCont.Children.Clear();
                        isFavourites = false;
                        filter = "";
                        ResetPages();
                        await UpdateData();
                        RefreshCollectionView();
                        SetPageTotal();
                        await App.HideLoading();
                    }
                });
            }));

            StackLayout searchBtnCont = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH,
                Orientation = StackOrientation.Horizontal,
                Margin = 2,
                Children =
                {
                    searchButton.Content,
                    resetButton.Content
                }
            };

            searchInputContainer = new StackLayout
            {
                Margin = 5,
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    searchEntryLabel,
                    searchEntry,
                    searchBtnCont
                }
            };

            ColourButton confirmButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE),
            Color.White, AppText.CONFIRM, null);
            confirmButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            confirmButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            confirmButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(confirmButton.Content, true);
            TouchEffect.SetCommand(confirmButton.Content,
            new Command(() =>
            {
                string planId = GetPlanFromCalendar.GetCalendarPlan(date);

                if (isCalendar)
                {
                    if (DateTime.Parse(date) < DateTime.Today)
                    {
                        //Editing a day in the past, show an error message
                        App.DisplayAlert("Oh!", "You've tried to edit a meal in the past", "Ok");
                        return;
                    }

                    //mealplanID = GetPlanFromCalendar.GetCalendarPlan(date);
                }

                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("Confirm", "Tap and confirm when you’ve finished editing the content and it will update the chosen meal with your new one.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                    }
                    else
                    {
                        if (AppSession.notifiedCalendarChange != true)
                        {
                            App.DisplayAlert("Oh!", "If you want to edit your calendar thats ok, but please be aware it could reduce the effectivness of your plan.", "Ok");
                            AppSession.notifiedCalendarChange = true;

                        }

                        if (StaticData.isEditing && isEditing)
                        {
                            // Add Recipe API
                            if (StaticData.chaiRecipeID == null || StaticData.whiskRecipeID == null)
                            {
                                App.ShowAlert("Recipe not selected.");
                            }
                            else
                            {
                                if (!StaticData.isTemplate)
                                {
                                    var result = await App.ApiBridge.UpdateMealOnMealPlan(
                                    AppSession.CurrentUser,
                                    mealID.ToString(),
                                    mealPlanId.ToString(),
                                    StaticData.chaiRecipeID,
                                    StaticData.whiskRecipeID);

                                    if (result != null)
                                    {
                                        if (AppSession.mealPlannerCalendar != null)
                                        {
                                            foreach (Meal meal in AppSession.mealPlannerCalendar.Data[rowIndex].Meals.Where(x => x.Id == result.Id))
                                            {
                                                meal.Id = result.Id;
                                                meal.MealType = result.MealType;
                                                meal.Recipe = result.Recipe;
                                                meal.violatesMealPlan = result.violatesMealPlan;
                                            }
                                            await UpdateMealPlan();
                                            await App.HideAddRecipe();
                                            recipe = null;
                                            StaticData.selectedRecipe = null;
                                            StaticData.whiskRecipeID = null;
                                            StaticData.chaiRecipeID = null;
                                        }
                                        else
                                        {
                                            try
                                            {
                                                await App.HideAddRecipe();
                                                recipe = null;
                                                StaticData.selectedRecipe = null;
                                                StaticData.whiskRecipeID = null;
                                                StaticData.chaiRecipeID = null;
                                                ResetPages();
                                                await App.UpdatePage((int)AppSettings.PageNames.Calendar);
                                            }
                                            catch (Exception e)
                                            {
                                                App.ShowAlert("Failed to update meal.");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            await App.HideAddRecipe();
                                            recipe = null;
                                            StaticData.selectedRecipe = null;
                                            StaticData.whiskRecipeID = null;
                                            StaticData.chaiRecipeID = null;
                                            ResetPages();
                                            await App.UpdatePage((int)AppSettings.PageNames.Calendar);
                                        }
                                        catch (Exception e)
                                        {
                                            App.ShowAlert("Failed to update meal.");
                                        }
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        var result = await App.ApiBridge.UpdateMealOnDayTemplate(AppSession.CurrentUser,
                                            dayTemplateID, mealTemplateID, mealPeriod, int.Parse(StaticData.chaiRecipeID), StaticData.whiskRecipeID, AppSession.SelectedRecipe.MainImageSource);
                                        if (result != null)
                                        {
                                            var match = AppSession.mealTemplate.Data.FirstOrDefault(x => x.id == dayTemplateID);
                                            if (match != null)
                                            {
                                                foreach (MealTemplate meal in match.mealTemplates)
                                                {
                                                    if (meal.mealType == result.mealType)
                                                    {
                                                        meal.id = result.id;
                                                        meal.recipe = result.recipe;
                                                        meal.whiskID = result.recipe.WhiskRecipeId;
                                                    }

                                                }
                                                var mealPlannerGroup = new MealPlannerCollectionViewSection(AppSession.mealTemplate.Data);
                                                AppSession.mealPlannerCollection.Add(mealPlannerGroup);
                                                AppSession.mealPlannerCollection.RemoveAt(0);
                                                await App.HideModalAsync();
                                                recipe = null;
                                                StaticData.selectedRecipe = null;
                                                StaticData.whiskRecipeID = null;
                                                StaticData.chaiRecipeID = null;
                                            }
                                            else
                                            {
                                                App.ShowAlert("Failed to update meal.");
                                            }
                                        }
                                        else
                                        {
                                            App.ShowAlert("Failed to update meal.");
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e);
                                    }
                                }
                            }
                        }
                        else if (StaticData.isEditing && !isEditing || !StaticData.isEditing)
                        {
                            if (StaticData.chaiRecipeID == null || StaticData.whiskRecipeID == null)
                            {
                                App.ShowAlert("Recipe not selected.");
                            }
                            else
                            {
                                if (!StaticData.isTemplate)
                                {
                                  
                                    var result = await App.ApiBridge.AddMealToMealPlan(
                                    AppSession.CurrentUser,
                                    mealPeriod,
                                    planId,
                                    date,
                                    StaticData.chaiRecipeID,
                                    StaticData.whiskRecipeID,
                                    isCalendar);
                                    if (result != null)
                                    {
                                        //Ths causes the app to crash on the calendar page but for some reason if it is skipped it fails to update the calendar.
                                        try
                                        {
                                            AppSession.mealPlannerCalendar.Data[rowIndex].Meals.Add(result);
                                        }
                                        catch { }
                                        await UpdateMealPlan();
                                        await App.HideAddRecipe();
                                        recipe = null;
                                        StaticData.selectedRecipe = null;
                                        StaticData.whiskRecipeID = null;
                                        StaticData.chaiRecipeID = null;
                                    }
                                    else
                                    {
                                        App.ShowAlert("Failed to add meal.");
                                    }
                                }
                                else
                                {
                                    var result = await App.ApiBridge.AddMealToDayTemplate(AppSession.CurrentUser,
                                        dayTemplateID, mealPeriod, int.Parse(StaticData.chaiRecipeID), StaticData.whiskRecipeID, AppSession.SelectedRecipe.MainImageSource);
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
                                        recipe = null;
                                        StaticData.selectedRecipe = null;
                                        StaticData.whiskRecipeID = null;
                                        StaticData.chaiRecipeID = null;
                                    }
                                    else
                                    {
                                        App.ShowAlert("Failed to update meal.");
                                    }
                                }
                            }
                        }
                    }
                });
            }));

            StackLayout btnCont = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH,
                Children =
                {
                    confirmButton.Content
                }
            };

            StackLayout emptyCont = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = 10,
                Children =
                {
                }
            };

            StaticLabel selectRecipeDesc = new StaticLabel("Tap the above recipe to view.");
            selectRecipeDesc.Content.FontFamily = Fonts.GetBoldAppFont();
            selectRecipeDesc.Content.FontSize = Units.FontSizeM;
            selectRecipeDesc.Content.TextColor = Color.White;
            selectRecipeDesc.CenterAlign();

            StackLayout leftCont = new StackLayout
            {
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                Orientation = StackOrientation.Vertical,
                Children =
                {
                }
            };
            //leftCont.Children.Add(mealTypeTitleCont);
            leftCont.Children.Add(mealTypeCont);

            if (isEditing)
            {
                //leftCont.Children.Add(removeIcon.Content);
            }

            leftCont.Children.Add(currentRecipeLabelCont);
            leftCont.Children.Add(selectRecipeCont);
            leftCont.Children.Add(selectRecipeDesc.Content);
            leftCont.Children.Add(descriptionLabelCont);
            leftCont.Children.Add(searchInputContainer);
            leftCont.Children.Add(btnCont);
            leftCont.Children.Add(emptyCont);

            /*
            rightCont = new Grid
            {
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                HeightRequest = 300,
                WidthRequest = 80,
                RowSpacing = 2,
                BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY),
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                    { new RowDefinition { Height = new GridLength(18)}},
                }
            };*/

            rightCont = new StackLayout
            {
                //HeightRequest = 300,
                //WidthRequest = 80,
                Spacing = 8,
                HeightRequest = 480,//Units.ScreenHeight - 240,
                Padding = new Thickness(0, 8),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,

            };

            /*pageTextCont = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = 80,
                IsClippedToBounds = true,
                Children =
                {
                }
            };*/

            pageNum = new StaticLabel($"0/{AppSession.TotalPages}");
            pageNum.Content.TextColor = Color.White;
            pageNum.Content.FontSize = Units.FontSizeM;
            pageNum.Content.Padding = new Thickness(0, 4);
            pageNum.CenterAlign();


            

            //pageTextCont.Children.Add(pageNum.Content);
            /*
            infoImage = new ActiveImage("info.png", 20, 20, null, null);
            TouchEffect.SetNativeAnimation(infoImage.Content, true);
            TouchEffect.SetCommand(infoImage.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    CheckResult();
                });
            }));*/

            /*StackLayout infoBtn = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 0,
                WidthRequest = 20,
                HeightRequest = 20,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    infoImage.Content
                }
            };*/

            Color tintColour = Color.LightGray;
            TintTransformation colorTint = new TintTransformation
            {
                HexColor = (string)tintColour.ToHex(),
                EnableSolidColor = true

            };
            var tint = new List<FFImageLoading.Work.ITransformation>();
            tint.Add(colorTint);

            UpArrow = new StaticImage("chevronupbold.png", 24, tint);
            UpArrow.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            UpArrow.Content.VerticalOptions = LayoutOptions.Center;
            UpArrow.Content.HeightRequest = 24;
            
            DownArrow = new StaticImage("chevrondownbold.png", 24, tint);
            DownArrow.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            DownArrow.Content.VerticalOptions = LayoutOptions.Center;
            DownArrow.Content.HeightRequest = 24;

            TouchEffect.SetNativeAnimation(UpArrow.Content, true);
            TouchEffect.SetCommand(UpArrow.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (UpArrow.Content.Opacity == 0) { }
                    else { GetLast(); }
                });
            }));

            TouchEffect.SetNativeAnimation(DownArrow.Content, true);
            TouchEffect.SetCommand(DownArrow.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    GetNext();
                });
            }));

            if (AppSession.CurrentPage <= 0)
            {
                UpArrow.Content.Opacity = 0;
            }

            /*
            StackLayout navBtnCont = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Spacing = 8,
                Padding = 4,
                Margin = 4,
                Children =
                {
                    UpArrow.Content,
                    DownArrow.Content
                }
            };*/


            rightCont.Children.Add(UpArrow.Content);//, 0, 0);
            rightCont.Children.Add(BuildRecipeCollection());//, 0, 1);
            rightCont.Children.Add(DownArrow.Content);//, 0, 2);
            rightCont.Children.Add(pageNum.Content);//, 0, 3);

            //rightCont.Children.Add(infoImage.Content);//, 0, 4);

            rightCont.Children.Add(sortLabel);//

            StackLayout midCont = new StackLayout
            {
                WidthRequest = Units.ScreenWidth,
                BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY),
                Orientation = StackOrientation.Horizontal,
                IsClippedToBounds = true,
                Spacing = 0,
                Children =
                {
                    leftCont,
                    rightCont,
                }
            };
            StackLayout seperator = new StackLayout { WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) };
            masterGrid = new Grid
            {
                ColumnSpacing = 0,
                RowSpacing = 0,
                WidthRequest = Units.ScreenWidth,
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                },
                Children = {
                    {topGrid, 0, 0},
                    {seperator, 0, 1},
                    {midCont, 0, 2},
                },
            };

            StackLayout masterContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                Spacing = 0,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                Children =
                {
                    masterGrid
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

            pageContainer = new StackLayout
            {
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    frame
                }
            };
            //Update();
            Content.Children.Add(pageContainer);
            GetLast();
        }

        public void BuildRemoveMealConfirm(int mealID, int mealPlanId, Recipe recipe, string mealPeriod)
        {
            pageContainer.Children.Clear();

            StackLayout displayedTitleCont = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
                        Text = "Remove Meal",
                        FontSize = Units.FontSizeXL,
                        TextColor = Color.White
                    }
                }
            };
            string descText = "";
            if (!StaticData.isTemplate)
            {
                descText = "Do you wish to remove the meal from the meal plan?";
            }
            else
            {
                descText = "Do you wish to remove the meal from the meal template?";
            }

            StackLayout descCont = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
                        Text = descText,
                        FontSize = Units.FontSizeL,
                        TextColor = Color.White,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center
                    }
                }
            };

            ColourButton confirmButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE),
                 Color.White, AppText.CONFIRM, null);
            confirmButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            confirmButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            confirmButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            TouchEffect.SetNativeAnimation(confirmButton.Content, true);
            TouchEffect.SetCommand(confirmButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("Confirm", "Tap and confirm when you’ve finished editing the content and it will update the chosen meal with your new one.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                    }
                    else
                    {
                        if (!StaticData.isTemplate)
                        {
                            var result = App.ApiBridge.RemoveMealOnMealPlan(AppSession.CurrentUser, mealID.ToString(), mealPlanId.ToString()).Result;

                            if (result != null)
                            {
                                int resultID = int.Parse(result);
                                try
                                {
                                    AppSession.mealPlannerCalendar.Data[rowIndex].Meals.RemoveAll(x => x.Id == resultID);
                                    await UpdateMealPlan();
                                }
                                catch (Exception e)
                                {


                                }

                                await App.UpdatePage();
                                await App.HideAddRecipe();
                                recipe = null;
                                StaticData.selectedRecipe = null;
                                StaticData.whiskRecipeID = null;
                                StaticData.chaiRecipeID = null;
                            }
                            else
                            {
                                App.ShowAlert("Failed to remove meal.");
                            }
                        }
                        else
                        {
                            var result = await App.ApiBridge.DeleteMealOnDayTemplate(AppSession.CurrentUser,
                                dayTemplateID, mealTemplateID);
                            if (result)
                            {
                                var match = AppSession.mealTemplate.Data.FirstOrDefault(x => x.id == dayTemplateID);
                                if (match != null)
                                {
                                    match.mealTemplates.RemoveAll(x => x.id == mealTemplateID);
                                    var mealPlannerGroup = new MealPlannerCollectionViewSection(AppSession.mealTemplate.Data);
                                    AppSession.mealPlannerCollection.Add(mealPlannerGroup);
                                    AppSession.mealPlannerCollection.RemoveAt(0);
                                }
                                else
                                {
                                    App.ShowAlert("Failed to remove meal.");
                                }
                                await App.HideModalAsync();
                                recipe = null;
                                StaticData.selectedRecipe = null;
                                StaticData.whiskRecipeID = null;
                                StaticData.chaiRecipeID = null;
                            }
                            else
                            {
                                App.ShowAlert("Failed to remove meal.");
                            }
                        }
                    }
                });
            }));

            ColourButton cancelButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE),
            Color.White, AppText.CANCEL, null);
            cancelButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            cancelButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            cancelButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            TouchEffect.SetNativeAnimation(cancelButton.Content, true);
            TouchEffect.SetCommand(cancelButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    RebuildEditModal();
                });
            }));

            StackLayout btnCont = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH * 2,
                Children =
                {
                    confirmButton.Content,
                    cancelButton.Content
                }
            };

            Grid contentGrid = new Grid
            {
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                ColumnSpacing = 0,
                RowSpacing = 4,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(Units.ScreenHeight5Percent)}},
                    { new RowDefinition { Height = new GridLength(Units.ScreenHeight5Percent)}},
                    { new RowDefinition { Height = new GridLength(Units.ScreenHeight5Percent)}},
                },
                Children = {
                    {displayedTitleCont, 0, 0},
                     {descCont, 0, 1},
                    {btnCont,0,2}
                },
            };

            pageContainer = new StackLayout
            {
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            pageContainer.Children.Add(contentGrid);

            Content.Children.Add(pageContainer);
        }

        public CollectionView BuildRecipeCollection()
        {
            editMealCollectionView = new EditMealCollectionView();
            recipesLayout = editMealCollectionView.GetCollectionView();
            editMealCollectionView.ShowRecipes(SetPageTotal);
            return recipesLayout;
        }

        public void SetPageTotal()
        {
            AppSession.TotalPages = (AppSession.TotalRecipes + ApiBridge.ITEMS_PER_REQUEST - 1) / ApiBridge.ITEMS_PER_REQUEST;
            pageNum.Content.Text = $"PAGE {AppSession.CurrentPage}/{AppSession.TotalPages}";

            if (AppSession.CurrentPage <= 1)
            {
                UpArrow.Content.Opacity = 0;
            }
            else
            {
                UpArrow.Content.Opacity = 1;
            }

            if (AppSession.CurrentPage < AppSession.TotalPages)
            {
                DownArrow.Content.Opacity = 1;
            }
            else
            {
                DownArrow.Content.Opacity = 0;
            }

            if (AppSession.TotalRecipes <= ApiBridge.ITEMS_PER_REQUEST)
            {
                pageNum.Content.Text = "PAGE 1/1";
                DownArrow.Content.Opacity = 0;
                UpArrow.Content.Opacity = 0;
            }
            else if (AppSession.TotalRecipes < 1)
            {
                pageNum.Content.Text = "PAGE 1/1";
                DownArrow.Content.Opacity = 0;
                UpArrow.Content.Opacity = 0;
            }
        }

        public async Task UpdateData()
        {
            await App.ShowLoading();
            if (!isFavourites && !isSearched)
            {
                AppSession.EditMealRecipes = DataManager.GetRecommendedRecipes(AppSession.CurrentUser, AppSession.UpdateSearch);
            }
            else if (!isFavourites && isSearched)
            {
                AppSession.EditMealRecipes = DataManager.SearchRecipes(AppSession.CurrentUser, AppSession.UpdateSearch);
            }
            else
            {
                AppSession.EditMealRecipes = DataManager.GetFavouriteRecipes();
            }
            await Task.Delay(10);
            await App.HideLoading();
        }

        public async void RefreshCollectionView()
        {
            var mealsGroup = new MealsCollectionViewSection(AppSession.EditMealRecipes);
            //Surrounded in try catch as this could fail...
            try
            {
                AppSession.editMealRecipesCollection.Clear();
                //Adding a delay to try and prevent collection modified crash
                await Task.Delay(250);
            }
            catch
            {
                Console.WriteLine("No recipes to remove!");
            }
            AppSession.editMealRecipesCollection.Add(mealsGroup);
        }

        public void GetNext()
        {
            if (AppSession.CurrentPage < AppSession.TotalPages)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    AppSession.GetNextPage = true;
                    AppSession.GetLastPage = false;
                    AppSession.CurrentPage++;
                    await UpdateData();
                    RefreshCollectionView();
                    AppSession.GetNextPage = false;
                    AppSession.GetLastPage = false;
                    SetPageTotal();
                });
            }
        }

        public void GetLast()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                AppSession.GetNextPage = false;
                AppSession.GetLastPage = true;
                if (AppSession.CurrentPage > 1)
                {
                    AppSession.CurrentPage--;
                }
                await UpdateData();
                RefreshCollectionView();
                AppSession.GetNextPage = false;
                AppSession.GetLastPage = false;
                SetPageTotal();
            });
        }

        public void SwitchSelectedRecipe(Recipe input)
        {
            selectRecipeCont.Children.Clear();
            PreviewTile previewTile = new PreviewTile();
            if (input.Name != null)
            {
                previewTile.SetName(input.Name);
            }
            else
            {
                previewTile.SetName(input.chai.Name);
            }
            previewTile.SetTime(input.CookingTime);
            if (input.Images != null)
            {
                previewTile.SetImageSearched(input.Images[0]);
            }
            else
            {
                previewTile.SetImageSearched(null);
            }
            previewTile.SetRecipe(input);
            Action<Recipe> showRecipeView = (r) =>
            {
                BuildRecipeView(input);
            };
            previewTile.SetAction(showRecipeView);
            selectRecipeCont.Children.Add(previewTile.Content);

            selectRecipeCont.Children.Add(removeIcon.Content);

            this.recipe = input;
        }

        public async void CheckResult()
        {
            filter = await App.ShowActionSheet("Sort Recipes By", "Cancel", null, new string[] { "Ascending", "Descending", "Dish Type", "Favourites" });
            if (filter == "Favourites")
            {
                isFavourites = true;
            }
            if (filter != "Cancel")
            {
                await UpdateData();
                SetFilter();
                RefreshCollectionView();
                if (!isFavourites)
                {
                    SetPageTotal();
                }
                else
                {
                    pageNum.Content.Text = "PAGE 1/1";
                    DownArrow.Content.Opacity = 0;
                    UpArrow.Content.Opacity = 0;
                }
            }
        }

        public void SetFilter()
        {
            switch (filter)
            {
                case "Ascending":
                    AppSession.EditMealRecipes = AppSession.EditMealRecipes.OrderBy(x => x.Name).ToList();
                    break;
                case "Descending":
                    AppSession.EditMealRecipes = AppSession.EditMealRecipes.OrderByDescending(x => x.Name).ToList();
                    break;
                case "Dish Type":
                    AppSession.EditMealRecipes = AppSession.EditMealRecipes.OrderBy(x => x.DishType).ToList();
                    break;
                case "Favourites":
                    break;
                case "":
                    break;
                case null:
                    break;
            }
        }

        public void ResetPages()
        {
            AppSession.CurrentPage = 1;
            AppSession.TotalPages = 1;
        }

        private async Task<bool> UpdateMealPlan()
        {
            string week = "";
            switch (StaticData.week)
            {
                case StaticData.WeekeNum.week1:
                    week = "1";
                    break;
                case StaticData.WeekeNum.week2:
                    week = "2";
                    break;
                case StaticData.WeekeNum.week3:
                    week = "3";
                    break;
                case StaticData.WeekeNum.week4:
                    week = "4";
                    break;
            }
            //AppSession.mealPlannerCalendar = await App.ApiBridge.GetWeek(AppSession.CurrentUser, AppSession.CurrentUser.defaultMealPlanID.ToString(), week, AppSession.mealPlanTokenSource.Token);
            try
            {
                var mealPlannerGroup = new MealPlannerCollectionViewSection(AppSession.mealPlannerCalendar.Data);
                AppSession.mealPlannerCollection.RemoveAt(0);
                AppSession.mealPlannerCollection.Add(mealPlannerGroup);
                try
                {
                    //Try to update calendar page
                    Pages.Custom.Calendar.ForceCalendarRefresh();
                }
                catch
                {
                    Console.WriteLine("Error");
                }
            }
            catch(Exception e)
            {
                try
                {
                    //Try to update calendar page
                    Pages.Custom.Calendar.ForceCalendarRefresh();
                }
                catch
                {
                    Console.WriteLine("Error");
                }
            }

            return true;
        }

        public async void RebuildEditModal()
        {
            Content.Children.Clear();
            ResetPages();
            await App.ShowLoading();
            await App.HideAddRecipe();
            if (!StaticData.isTemplate)
            {
                await App.ShowEditMeal(this.rowIndex, this.mealID, this.mealPlanId, this.recipe, this.mealPeriod, this.isEditing, this.date);
            }
            else
            {
                await App.ShowEditMeal(this.mealTemplateID, this.dayTemplateID, this.mealPeriod, this.recipe, this.isEditing, true);
            }

            await App.HideLoading();
        }

        public void BuildRecipeView(Recipe recipe)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                //App.ScaleUpBackground();
                //await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Landing);
                await App.ShowFullRecipe(recipe, true, true);
               
            });
            
            /*
            Content.Children.Clear();

            recipePreviewLayout.CheckBasketItemExistsForColour(recipe);

            StaticLabel closeLabel = recipePreviewLayout.CreateCloseLabel(new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    Content.Children.Clear();
                    BuildEditModalNew();
                });
            }));
            StaticLabel titleLabel = recipePreviewLayout.CreateTitleLabel(recipe.Name);
            StackLayout imgCont = recipePreviewLayout.CreateRecipeImageContainer(recipe);
            StackLayout ingredientsContainer = recipePreviewLayout.CreateIngredientsContainer(recipe);
            StackLayout methodContainer = recipePreviewLayout.CreateMethodContainer(recipe);

            recipePreviewLayout.CreateFavouriteHeart(recipe);

            StaticLabel favLabel = recipePreviewLayout.CreateFavouriteLabel();

            List<View> favGridViews = new List<View>() { recipePreviewLayout.favouriteHeart.Content, favLabel.Content };
            recipePreviewLayout.favGrid = recipePreviewLayout.CreateFavouriteGrid(favGridViews);

            recipePreviewLayout.AddToGesture(recipe);

            IconLabel ShareThis = recipePreviewLayout.CreateShareLabel();

            recipePreviewLayout.bottomGrid = recipePreviewLayout.CreateBottomGrid(recipePreviewLayout.favGrid);

            if (Helpers.Pages.CurrentPage != (int)AppSettings.PageNames.ShoppingBasket)
            {
                recipePreviewLayout.bottomGrid.Children.Add(recipePreviewLayout.AddTo.Content, 1, 0);
            }
            recipePreviewLayout.bottomGrid.Children.Add(ShareThis.Content, 2, 0);

            List<View> scrollContViews = new List<View>() { ingredientsContainer, methodContainer, recipePreviewLayout.bottomGrid };
            ScrollView scrollView = recipePreviewLayout.CreateScrollView(scrollContViews);

            List<View> masterContViews = new List<View>() { closeLabel.Content, titleLabel.Content, imgCont, scrollView };
            StackLayout masterContainer = recipePreviewLayout.CreateMasterContainer(masterContViews);

            Frame modalFrame = recipePreviewLayout.CreateModalFrame(masterContainer);

            Content.Children.Add(modalFrame);
            Content.Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING);
            */
        }
    }
}