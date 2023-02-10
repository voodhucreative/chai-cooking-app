using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Composites;
using ChaiCooking.Components.Fields;
using ChaiCooking.Components.Fields.Custom;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using ChaiCooking.Tools;
using ChaiCooking.Views.CollectionViews;
using Xamarin.Forms;
using System.Linq;
using Xamarin.CommunityToolkit.Effects;
using ChaiCooking.Views.CollectionViews.RecipeEditor;
using ChaiCooking.Views.Custom;

namespace ChaiCooking.Pages.Custom
{
    public class RecipeEditor : Page
    {

        StackLayout ListHeaderContainer;
        StackLayout ListContainer;
        StackLayout EditorContainer;

        UserRecipeTile RecipePreviewTile;

        //Layouts.Custom.Lists.UserRecipeList RecipeList;
        CollectionView RecipesCollectionView;
        ObservableCollection<RecipesCollectionViewSection> RecipesCollection;

        //StaticLabel Title;
        //StaticLabel Info;
        //ActiveLabel SortBy;

        IconLabel TitleLabel;
        StaticLabel InfoLabel;
        StaticLabel YourRecipesLabel;

        //ColourButton NewRecipeButton;
        Components.Buttons.ImageButton NewRecipeButton;
        //ColourButton DeleteRecipesButton;

        //ColourButton DebugClearButton;

        Grid Seperator;

        RecipeEditorCollectionView recipeEditorCollectionView;
        CollectionView recipeEditorLayout;

        bool ShowEditor;
        Recipe CurrentRecipe;
        List<Ingredient> CurrentIngredients;
        int row;
        int col;
        int itemCount;
        StackLayout IngredientsContainer;
        Ingredient MainIngredient;
        StackLayout RecipeContainer;

        Components.Composites.CheckBox ReadyToPublish;

        List<Components.Composites.CheckBox> IngredientOptions;

        StaticLabel RecipeSummaryLabel;
        StaticLabel NameEntryLabel;
        StaticLabel ServingsLabel;
        StaticLabel DishTypeLabel;
        StaticLabel MealTypeLabel;
        StaticLabel TimingsLabel;
        StaticLabel MealCategoryLabel;
        StaticLabel MainIngredientLabel;
        StaticLabel AllIngredientsLabel;
        StaticLabel MethodLabel;
        StaticLabel CloseLabel;

        // info labels
        StaticLabel NameInfoLabel;
        StaticLabel AllIngredientsInfoLabel;

        InputField NameInput;
        InputField ServingsInput;
        InputField NewIngredientInput;

        ValueAdjuster PrepTimeAdjuster;
        ValueAdjuster CookingTimeAdjuster;

        StaticLabel PrepTimeLabel;
        InputField PrepTimeInput;

        StaticLabel CookingTimeLabel;
        InputField CookingTimeInput;

        ScrollView MethodScroll;
        CustomEditor MethodEntry;
        //Label MethodText;

        ColourButton SaveButton;
        ColourButton AddAnotherIngredientButton;

        ScrollView RecipeDetailsScroller;

        List<Recipe> RecipesList;
        List<Ingredient> FoundIngredients;

        IngredientPicker IngrdntPicker;

        StackLayout recipeInfoContainer;

        bool mainIngredientAdded = false;

        bool EditingExisting;
        bool methodFocused;

        //double methodEntrySize = 3200;
        int MethodEntrySize = 2400;
        bool bottomOfScroll = true;

        readonly string emptyRecipeName = "New Recipe";

        public RecipeEditor()
        {
            this.IsScrollable = false;
            this.IsRefreshable = true;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;
            this.Id = (int)AppSettings.PageNames.RecipeEditor;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.FadeIn;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.FadeOut;

            ShowEditor = false;
            EditingExisting = false;

            CurrentRecipe = new Recipe();

            FoundIngredients = new List<Ingredient>();
            IngrdntPicker = new IngredientPicker();

            IngrdntPicker.FoundIngredientsPicker.Unfocused += FoundChanged;

            PageContent = new Grid
            {
                //BackgroundColor = Color.Transparent,
                BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY)
            };

            ListContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            EditorContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            RecipeSummaryLabel = new StaticLabel("Recipe summary");
            RecipeSummaryLabel.Content.FontSize = Units.FontSizeM;
            RecipeSummaryLabel.Content.FontFamily = Fonts.GetRegularAppFont();
            RecipeSummaryLabel.Content.TextColor = Color.White;
            RecipeSummaryLabel.LeftAlign();

            ReadyToPublish = new Components.Composites.CheckBox("Ready to Publish", "tick.png", "tickbg.png", 280, 32, false);
            ReadyToPublish.Title.Content.TextColor = Color.White;
            ReadyToPublish.Title.Content.FontFamily = Fonts.GetBoldAppFont();
            ReadyToPublish.Title.RightAlign();

            CloseLabel = new StaticLabel("Close");
            CloseLabel.Content.FontSize = Units.FontSizeM;
            CloseLabel.Content.FontFamily = Fonts.GetRegularAppFont();
            CloseLabel.Content.TextColor = Color.White;
            CloseLabel.RightAlign();

            TouchEffect.SetNativeAnimation(CloseLabel.Content, true);
            TouchEffect.SetCommand(CloseLabel.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await ShowRecipeList();
                });
            }));

            NameEntryLabel = new StaticLabel("Title");
            NameEntryLabel.Content.FontSize = Units.FontSizeL;
            NameEntryLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            NameEntryLabel.Content.TextColor = Color.White;
            NameEntryLabel.LeftAlign();

            DishTypeLabel = new StaticLabel("Dish type");
            DishTypeLabel.Content.FontSize = Units.FontSizeL;
            DishTypeLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            DishTypeLabel.Content.TextColor = Color.White;
            DishTypeLabel.LeftAlign();

            MealTypeLabel = new StaticLabel("Meal type");
            MealTypeLabel.Content.FontSize = Units.FontSizeL;
            MealTypeLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            MealTypeLabel.Content.TextColor = Color.White;
            MealTypeLabel.LeftAlign();

            TimingsLabel = new StaticLabel("Timings");
            TimingsLabel.Content.FontSize = Units.FontSizeL;
            TimingsLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            TimingsLabel.Content.TextColor = Color.White;
            TimingsLabel.LeftAlign();

            ServingsLabel = new StaticLabel("Serves");
            ServingsLabel.Content.FontSize = Units.FontSizeL;
            ServingsLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            ServingsLabel.Content.TextColor = Color.White;
            ServingsLabel.LeftAlign();

            PrepTimeLabel = new StaticLabel("Prep time (mins)");
            PrepTimeLabel.Content.FontSize = Units.FontSizeM;
            PrepTimeLabel.Content.FontFamily = Fonts.GetRegularAppFont();
            PrepTimeLabel.Content.TextColor = Color.White;
            PrepTimeLabel.CenterAlign();

            PrepTimeInput = new InputField("minutes", "minutes", Keyboard.Numeric, true);
            PrepTimeInput.TextEntry.MaxLength = 3;
            PrepTimeInput.TextEntry.TextChanged += PrepInputChanged;

            CookingTimeLabel = new StaticLabel("Cook time (mins)");
            CookingTimeLabel.Content.FontSize = Units.FontSizeM;
            CookingTimeLabel.Content.FontFamily = Fonts.GetRegularAppFont();
            CookingTimeLabel.Content.TextColor = Color.White;
            CookingTimeLabel.CenterAlign();

            CookingTimeInput = new InputField("minutes", "minutes", Keyboard.Numeric, true);
            CookingTimeInput.TextEntry.MaxLength = 3;
            CookingTimeInput.TextEntry.TextChanged += CookInputChanged;

            MealCategoryLabel = new StaticLabel("Meal category");
            MealCategoryLabel.Content.FontSize = Units.FontSizeL;
            MealCategoryLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            MealCategoryLabel.Content.TextColor = Color.White;
            MealCategoryLabel.LeftAlign();

            MainIngredientLabel = new StaticLabel("Main ingredient");
            MainIngredientLabel.Content.FontSize = Units.FontSizeL;
            MainIngredientLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            MainIngredientLabel.Content.TextColor = Color.White;
            MainIngredientLabel.LeftAlign();

            AllIngredientsLabel = new StaticLabel("Add ingredients");
            AllIngredientsLabel.Content.FontSize = Units.FontSizeL;
            AllIngredientsLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            AllIngredientsLabel.Content.TextColor = Color.White;
            AllIngredientsLabel.LeftAlign();

            MethodLabel = new StaticLabel("Method");
            MethodLabel.Content.FontSize = Units.FontSizeL;
            MethodLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            MethodLabel.Content.TextColor = Color.White;
            MethodLabel.LeftAlign();

            NameInfoLabel = new StaticLabel("You can enter a longer title than one which fits in this box, but this gives you an indication of what will actually be displayed in the summary. Your title will be displayed fully in the recipe instructions page.");
            NameInfoLabel.Content.FontSize = Units.FontSizeM;
            NameInfoLabel.Content.FontFamily = Fonts.GetRegularAppFont();
            NameInfoLabel.Content.TextColor = Color.White;
            NameInfoLabel.LeftAlign();

            AllIngredientsInfoLabel = new StaticLabel("This is where you add all the ingredients for your recipe including the core ingredient. Once added, simply tap on an ingredient to make it the main recipe ingredient.");
            AllIngredientsInfoLabel.Content.FontSize = Units.FontSizeM;
            AllIngredientsInfoLabel.Content.FontFamily = Fonts.GetRegularAppFont();
            AllIngredientsInfoLabel.Content.TextColor = Color.White;
            AllIngredientsInfoLabel.LeftAlign();

            MethodEntry = new CustomEditor
            {
                Keyboard = Keyboard.Text,
                TextColor = Color.Black,
                MaxLength = MethodEntrySize,
                //MinimumHeightRequest = 250,
                AutoSize = EditorAutoSizeOption.TextChanges,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.White,
                FontFamily = Fonts.GetRegularAppFont()
            };

            //MethodText = new Label
            //{
            //    MinimumHeightRequest = 240,
            //    VerticalOptions = LayoutOptions.FillAndExpand,
            //    HorizontalOptions = LayoutOptions.FillAndExpand,
            //    VerticalTextAlignment = TextAlignment.Start,
            //    HorizontalTextAlignment = TextAlignment.Start,
            //    TextColor = Color.Black,
            //    FontFamily = Fonts.GetRegularAppFont()
            //};

            //MethodText.GestureRecognizers.Add(new TapGestureRecognizer()
            //{
            //Command = new Command(() =>
            //    {
            //        DirtyTextSelect(MethodEntry.Text.Length);
            //    })
            //});

            MethodScroll = new ScrollView
            {
                BackgroundColor = Color.Transparent,
                HeightRequest = 240,
                WidthRequest = Units.ScreenWidth,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Content = new StackLayout
                {
                    BackgroundColor = Color.Transparent,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Children =
                    {
                        MethodEntry
                    }
                }
            };
            //MethodScroll.Scrolled += MethodScroll_Scrolled;
            //MethodEntry.TextChanged += MethodTextChanged;
            //MethodEntry.Focused += MethodEntry_Focused;
            //MethodEntry.Unfocused += MethodEntry_Unfocused;

            NameInput = new InputField("Recipe Name", "Recipe name", Keyboard.Text, true);
            NameInput.TextEntry.MaxLength = 80;
            NameInput.TextEntry.TextChanged += NameInputChanged;

            NewIngredientInput = new InputField("New Ingredinet", "ingredient name", Keyboard.Text, true);

            ServingsInput = new InputField("Servings", "Servings", Keyboard.Text, true);

            SaveButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.SAVE_CHANGES, null);
            TouchEffect.SetNativeAnimation(SaveButton.Content, true);

            //Dirty stop multiple presses.
            bool isSaving = false;

            TouchEffect.SetCommand(SaveButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (!isSaving)
                    {
                        isSaving = true;
                        await SaveCurrentRecipe();
                        isSaving = false;
                    }
                });
            }));

            AddAnotherIngredientButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.ADD_BUTTON_TEXT, null);
            TouchEffect.SetNativeAnimation(AddAnotherIngredientButton.Content, true);
            TouchEffect.SetCommand(AddAnotherIngredientButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await AddAnotherIngredient();
                });
            }));

            RecipesCollection = new ObservableCollection<RecipesCollectionViewSection>();

            RecipesCollectionView = new CollectionView
            {
                IsGrouped = true,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                ItemsSource = RecipesCollection,
                WidthRequest = Units.ScreenWidth,
                Header = BuildListHeader(),
                ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                BackgroundColor = Color.Transparent,
                ItemTemplate = new UserRecipeDataTemplateSelector(),
                ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical)
                {
                    VerticalItemSpacing = 10
                },
                RemainingItemsThreshold = 40,
            };

            RecipeContainer = new StackLayout
            {
                Spacing = 0,
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            ResetCurrentRecipe();
            if (CurrentRecipe.chai.Status == "draft")
            {
                ReadyToPublish.IsChecked = false;
                ReadyToPublish.UnSelect();
            }
            else
            {
                ReadyToPublish.IsChecked = true;
                ReadyToPublish.Select();
            }

            RecipesCollection.Clear();

            ListContainer.Children.Add(BuildCollectionView());

            PageContent.Children.Add(ListContainer, 0, 0);
            PageContent.Children.Add(EditorContainer, 0, 0);

            Device.BeginInvokeOnMainThread(async () =>
            {
                await ShowRecipeList();
            });
        }

        //private void MethodScroll_Scrolled(object sender, ScrolledEventArgs e)
        //{
        //    var scrollView = sender as ScrollView;

        //    //this is bad. need to make the scrollview ignore the keyboard.
        //    var scrollingSpace = MethodEntry.Height - scrollView.Height;

        //    if (scrollingSpace > e.ScrollY + 100)
        //    {
        //        bottomOfScroll = false;
        //    }
        //    else
        //    {
        //        bottomOfScroll = true;
        //    }
        //}

        // Dirty way of scrolling past bottom of method box and adding a caret when selected
        //private async void MethodEntry_Focused(object sender, FocusEventArgs e)
        //private async void DirtyTextSelect(int CurPos)
        //{
        //    if (!methodFocused)
        //    {
        //        if (Device.RuntimePlatform == Device.Android)
        //        {
        //            recipeInfoContainer.Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING, 0, Dimensions.GENERAL_COMPONENT_PADDING, recipeInfoContainer.Height + Units.ThirdScreenHeight);
        //            RecipeDetailsScroller.ScrollToAsync(MethodScroll.Content, ScrollToPosition.Start, true);
        //        }
        //        else
        //        {
        //            RecipeDetailsScroller.ScrollToAsync(MethodLabel.Content, ScrollToPosition.Start, true);
        //        }
        //        MethodEntry.Focus();
        //        methodFocused = true;
        //    }
        //    MethodEntry.SetCursorToPosition(CurPos);

        //    //This breaks the label if we dont do this as we become unable to delete this text with regular user actions.
        //    if (MethodEntry.Text == "Method incomplete")
        //    {
        //        MethodEntry.Text = "";
        //    }

        //    bool caret = true;
        //    while (methodFocused)
        //    {
        //        MethodText.Text = caret? MethodEntry.Text.Insert(MethodEntry.GetCursorPosition(), "𝙸") : MethodEntry.Text.Replace("𝙸", "");
        //        caret = !caret;
        //        await Task.Delay(500);
        //    }
        //}

        //private void MethodEntry_Unfocused(object sender, FocusEventArgs e)
        //{
        //    if (Device.RuntimePlatform == Device.Android)
        //    {
        //        recipeInfoContainer.Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING, 0);
        //    }
        //    MethodScroll.ScrollToAsync(MethodEntry, ScrollToPosition.End, true);
        //    methodFocused = false;
        //    FormatMethodText(sender as Editor);
        //}

        void FoundChanged(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("Do stuff");
                Ingredient selected = FoundIngredients[IngrdntPicker.FoundIngredientsPicker.SelectedIndex];

                if (CurrentIngredients.Find(x => x.Name == selected.Name) == null)
                {
                    selected.Amount = "1";
                    selected.BaseIngredient = null;
                    selected.ID = selected.Id;
                    selected.LongDescription = "";
                    if (selected.MainImage == null)
                    {
                        selected.MainImage = "chaismallbag.png";
                    }
                    selected.Text = NewIngredientInput.TextEntry.Text;
                    selected.ShortDescription = "";
                    selected.Unit = AppDataContent.IngredientUnits[0];

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await AddIngredient(selected, false);
                    });

                    UpdateIngredients();

                    IngrdntPicker.GetContent().IsVisible = false;
                    NewIngredientInput.TextEntry.Text = "";
                    IngrdntPicker.FoundIngredientsPicker.Items.Clear();
                }
                else
                {
                    App.ShowAlert("You have already added this ingredient, you can change the quantity below");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Oh no!");
            }
        }

        void ResetCurrentRecipe()
        {
            CurrentRecipe.chai = new Models.Custom.Feed.Chai();
            CurrentRecipe.chai.Method = "";
            CurrentRecipe.Name = emptyRecipeName;
            CurrentRecipe.Durations = new Durations();
            CurrentRecipe.Durations.PrepTime = 10;
            CurrentRecipe.Durations.CookTime = 20;
            CurrentRecipe.Durations.TotalTime = 30;
            CurrentRecipe.PrepTime = "10 mins";
            CurrentRecipe.CookingTime = "20 mins";
            CurrentRecipe.DishType = "Main";
            CurrentRecipe.MealType = "dinner";
            CurrentRecipe.Servings = 2;
            CurrentRecipe.chai.Status = "draft";
            CurrentRecipe.MainImageSource = "chailogo.png";
            CurrentRecipe.MainIngredient = new Ingredient
            {
                Name = "",
            };
            CurrentIngredients = new List<Ingredient>();

            MainIngredient = new Ingredient
            {
                Name = ""
            };

            UpdateContent();
            RecipePreviewTile = new UserRecipeTile(CurrentRecipe);

            RecipePreviewTile.titleLbl.SetBinding(Label.TextProperty, new Binding(nameof(CurrentRecipe.Name), source: CurrentRecipe));


            RecipePreviewTile.prepSubDetail.SetBinding(Label.TextProperty, new Binding(nameof(CurrentRecipe.PrepTime), source: CurrentRecipe));
            RecipePreviewTile.totalSubDetail.SetBinding(Label.TextProperty, new Binding(nameof(CurrentRecipe.CookingTime), source: CurrentRecipe));

            RecipePreviewTile.SetStatus("draft");
        }

        void NameInputChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = sender as Entry;
            CurrentRecipe.Name = $"{entry.Text}";
        }

        void PrepInputChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = sender as Entry;

            try
            {
                CurrentRecipe.Durations.PrepTime = long.Parse(entry.Text);
                CurrentRecipe.PrepTime = $"{CurrentRecipe.Durations.PrepTime} mins";
            }
            catch (Exception e1) { }
        }

        void CookInputChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = sender as Entry;

            try
            {
                CurrentRecipe.Durations.CookTime = long.Parse(entry.Text);
                CurrentRecipe.CookingTime = $"{CurrentRecipe.Durations.CookTime} mins";
            }
            catch (Exception e2) { }
        }


        //void MethodTextChanged(object sender, TextChangedEventArgs e)
        //{
        //    var entry = sender as Editor;
        //    FormatMethodText(entry);

        //    //scrolls to bottom if the amount of lines is increased.
        //    if(entry.Height > methodEntrySize && bottomOfScroll)
        //    {
        //        methodEntrySize = entry.Height;
        //        MethodScroll.ScrollToAsync(MethodEntry, ScrollToPosition.End, true);
        //    }
        //    //MethodText.Text = $"{entry.Text}";
        //}
        
        //void MethodEntry_Focused(object sender, FocusEventArgs e)
        //{
        //    //Makes sure the view is in the right place.
        //    RecipeDetailsScroller.ScrollToAsync(MethodLabel.Content, ScrollToPosition.Start, true);
        //    MethodScroll.ScrollToAsync(MethodEntry, ScrollToPosition.End, false);
        //    //MethodText.Text = $"{entry.Text}";
        //}

        //void FormatMethodText(Editor entry)
        //{
            //MethodScroll.ScrollToAsync(MethodEntry, ScrollToPosition.End, true);

            //CurrentRecipe.chai.Method = $"{entry.Text}";

            //FormattedString tst = "";

            //for (int i = 0; i < entry.Text.Length; i++)
            //{
            //    Span span = new Span
            //    {
            //        Text = entry.Text[i].ToString(),
            //        StyleId = (i+1).ToString()
            //    };
            //    span.GestureRecognizers.Add(new TapGestureRecognizer()
            //    {
            //        Command = new Command(() =>
            //        {
            //            Device.BeginInvokeOnMainThread(async () =>
            //            {
            //                DirtyTextSelect(int.Parse(span.StyleId));
            //            });
            //        })
            //    });
            //    tst.Spans.Add(span);
            //}

            //MethodText.FormattedText = tst;
        //}

        private async Task<bool> ToggleViews()
        {
            ShowEditor = !ShowEditor;

            if (ShowEditor)
            {
                await ShowRecipeEditor();
            }
            else
            {
                await ShowRecipeList();
            }
            return true;
        }


        public async Task<bool> EditCurrentRecipe()
        {
            try
            {
                EditingExisting = true;

                CurrentRecipe = AppSession.SelectedRecipe;
                //CurrentIngredients.Clear();
                CurrentIngredients = CurrentRecipe.Ingredients.ToList<Ingredient>();

                UpdateContent();
                //UpdateIngredients();
                await ShowRecipeEditor();
            }
            catch (Exception e)
            {

            }
            return true;
        }

        private async Task<bool> ShowRecipeList()
        {
            ShowEditor = false;
            await RefreshCollectionView();
            await EditorContainer.TranslateTo(-Units.ScreenWidth, 0, 150, Easing.CubicInOut);
            await ListContainer.TranslateTo(0, 0, 150, Easing.CubicInOut);
            //No longer needed.
            //try
            //{
            //    await contentScrollView.ScrollToAsync(0, 0, false);
            //}
            //catch
            //{
            //    //Null
            //}
            return true;
        }

        private async Task<bool> ShowRecipeEditor()
        {
            ShowEditor = true;
            await RebuildEditorView();

            await ListContainer.TranslateTo(Units.ScreenWidth, 0, 150, Easing.CubicInOut);
            await EditorContainer.TranslateTo(0, 0, 150, Easing.CubicInOut);
            return true;
        }

        public StackLayout BuildListHeader()
        {
            ListHeaderContainer = new StackLayout
            {
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Spacing = Dimensions.GENERAL_COMPONENT_SPACING,
                WidthRequest = Units.ScreenWidth,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            StackLayout TopContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            TitleLabel = new IconLabel("editproperty.png", "Recipe Editor", Dimensions.ICON_LABEL_WIDTH, Dimensions.ICON_LABEL_HEIGHT);
            TitleLabel.TextContent.Content.TextColor = Color.White;
            TitleLabel.Content.WidthRequest = Units.ScreenWidth;
            TitleLabel.TextContent.Content.FontSize = Units.FontSizeL;
            TitleLabel.TextContent.Content.FontFamily = Fonts.GetBoldAppFont();
            TitleLabel.SetIconSize(Dimensions.ICON_LABEL_ICON_SIZE, Dimensions.ICON_LABEL_ICON_SIZE);

            InfoLabel = new StaticLabel("Start a new recipe, or you can edit an existing by finding it in the list and tapping on it");
            InfoLabel.Content.TextColor = Color.White;
            InfoLabel.Content.FontSize = Units.FontSizeL;
            InfoLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            InfoLabel.Content.Padding = new Thickness(Dimensions.GENERAL_COMPONENT_SPACING, 8);
            InfoLabel.LeftAlign();

            StackLayout buttonsContainer = new StackLayout
            {
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                WidthRequest = Units.ScreenWidth,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center
            };

            NewRecipeButton = new Components.Buttons.ImageButton("plus.png", "plus.png", "", Color.White, null);
            NewRecipeButton.Content.WidthRequest = 32;
            NewRecipeButton.Content.HorizontalOptions = LayoutOptions.EndAndExpand;
            NewRecipeButton.RightAlign();
            NewRecipeButton.ActiveStateImage.Content.HorizontalOptions = LayoutOptions.EndAndExpand;
            NewRecipeButton.InactiveStateImage.Content.HorizontalOptions = LayoutOptions.EndAndExpand;

            TouchEffect.SetNativeAnimation(NewRecipeButton.Content, true);
            TouchEffect.SetCommand(NewRecipeButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    ResetCurrentRecipe();
                    EditingExisting = false;
                    await ShowRecipeEditor();
                    //CurrentRecipe.Id = await DataManager.SaveUserRecipe(AppSession.CurrentUser, CurrentRecipe);
                    UpdateContent();
                });
            }));

            Seperator = new Grid { WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) };

            YourRecipesLabel = new StaticLabel("The recipes you've created");
            YourRecipesLabel.Content.TextColor = Color.White;
            YourRecipesLabel.Content.FontSize = Units.FontSizeM;
            YourRecipesLabel.Content.FontFamily = Fonts.GetRegularAppFont();
            YourRecipesLabel.Content.Padding = new Thickness(Dimensions.GENERAL_COMPONENT_SPACING, 8);
            YourRecipesLabel.LeftAlign();

            buttonsContainer.Children.Add(NewRecipeButton.Content);
            TopContainer.Children.Add(TitleLabel.Content);
            TopContainer.Children.Add(NewRecipeButton.Content);

            ListHeaderContainer.Children.Add(TopContainer);
            ListHeaderContainer.Children.Add(Seperator);
            ListHeaderContainer.Children.Add(YourRecipesLabel.Content);

            return ListHeaderContainer;
        }

        public async Task RebuildEditorView()
        {
            await Task.Delay(10);
            EditorContainer.Children.Clear();

            recipeInfoContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING, 0),
                Spacing = Dimensions.GENERAL_COMPONENT_SPACING,
                BackgroundColor = Color.Transparent
            };

            RecipeDetailsScroller = new ScrollView
            {
                Content = recipeInfoContainer
            };

            recipeInfoContainer.Children.Add(RecipeContainer);
            recipeInfoContainer.Children.Add(NameEntryLabel.Content);
            recipeInfoContainer.Children.Add(NameInput.Content);

            StackLayout timingsContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,
                        Children =
                        {
                            PrepTimeInput.Content,
                            PrepTimeLabel.Content
                        }
                    },

                    new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,
                        Children =
                        {
                            CookingTimeInput.Content,
                            CookingTimeLabel.Content
                        }
                    },
                }
            };

            row = 0;
            col = 0;
            itemCount = 0;
            IngredientsContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical
            };

            UpdateIngredients();

            StackLayout addAnotherContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Margin = new Thickness(0, 0, Dimensions.GENERAL_COMPONENT_SPACING, 0),
                Children =
                {
                    NewIngredientInput.Content,
                    AddAnotherIngredientButton.Content
                }
            };

            if (!AppSettings.SimplifiedRecipeEditor)
            {
                recipeInfoContainer.Children.Add(MainIngredientLabel.Content);
            }

            recipeInfoContainer.Children.Add(AllIngredientsLabel.Content);
            recipeInfoContainer.Children.Add(AllIngredientsInfoLabel.Content);
            recipeInfoContainer.Children.Add(addAnotherContainer);
            recipeInfoContainer.Children.Add(IngrdntPicker.GetContent());
            recipeInfoContainer.Children.Add(IngredientsContainer);
            recipeInfoContainer.Children.Add(TimingsLabel.Content);
            recipeInfoContainer.Children.Add(timingsContainer);
            recipeInfoContainer.Children.Add(ServingsLabel.Content);
            recipeInfoContainer.Children.Add(ServingsInput.Content);
            recipeInfoContainer.Children.Add(MethodLabel.Content);
            //recipeInfoContainer.Children.Add(MethodScroll);
            recipeInfoContainer.Children.Add(MethodEntry);
            recipeInfoContainer.Children.Add(ReadyToPublish.Content);
            recipeInfoContainer.Children.Add(SaveButton.Content);


            StackLayout tpBit = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Margin = new Thickness(0, 0, 0, Dimensions.GENERAL_COMPONENT_SPACING)
            };

            // NEEDS A SMALL SCREEN FIX... 
            //HeightRequest = Units.HalfScreenHeight,
            StackLayout tpBit2 = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING
            };

            tpBit2.Children.Add(RecipeSummaryLabel.Content);
            tpBit2.Children.Add(CloseLabel.Content);

            tpBit.Children.Add(tpBit2);

            EditorContainer.Children.Add(tpBit);

            EditorContainer.Children.Add(RecipeDetailsScroller);

            IngrdntPicker.GetContent().IsVisible = false;
            UpdateCurrentRecipe();

        }

        private StackLayout BuildTypeOptions(List<string> optionNames, string groupName, int selectedItem)
        {
            StackLayout optionsContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            List<RadioButton> options = new List<RadioButton>();
            int optionCount = 0;

            foreach (string name in optionNames)
            {
                options.Add(new RadioButton
                {
                    Content = name,
                    BackgroundColor = Color.Transparent,
                    TextColor = Color.White,
                    GroupName = groupName,
                    FontSize = Units.FontSizeM,
                    FontFamily = Fonts.GetRegularAppFont()
                });
            }

            foreach (RadioButton optionsButton in options)
            {
                optionsButton.IsChecked = false;

                if (groupName == "dishType")
                {
                    optionsButton.CheckedChanged += OnDishTypeRadioButtonCheckedChanged;
                }

                if (groupName == "mealType")
                {
                    optionsButton.CheckedChanged += OnMealTypeRadioButtonCheckedChanged;
                }

                if (selectedItem == optionCount)
                {
                    optionsButton.IsChecked = true;
                }
                optionsContainer.Children.Add(optionsButton);
                optionCount++;
            }

            return optionsContainer;
        }

        void OnDishTypeRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            CurrentRecipe.DishType = $"{button.Content}";
            UpdateCurrentRecipe();
        }

        void OnMealTypeRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            CurrentRecipe.MealType = $"{button.Content}";
            UpdateCurrentRecipe();
        }

        public void UpdateContent()
        {
            NameInput.TextEntry.Text = CurrentRecipe.Name;
            ServingsInput.TextEntry.Text = "" + CurrentRecipe.Servings;
            try
            {
                PrepTimeInput.TextEntry.Text = "" + CurrentRecipe.Durations.PrepTime;
                CookingTimeInput.TextEntry.Text = "" + CurrentRecipe.Durations.CookTime;
            }
            catch
            {
                PrepTimeInput.TextEntry.Text = "" + CurrentRecipe.chai.PrepTime;
                CookingTimeInput.TextEntry.Text = "" + CurrentRecipe.chai.CookTime;
            }
            //MainIngredientInput.TextEntry.Text = "" + CurrentRecipe.MainIngredient.Name;

            //Cleans up any instances where new lines ended up broken.
            string cleanedString = CurrentRecipe.chai.Method.Replace(@"\n", "\n");

            MethodEntry.Text = cleanedString;

            if (CurrentRecipe.chai.Status == "draft")
            {
                ReadyToPublish.IsChecked = false;
                ReadyToPublish.UnSelect();
            }
            else
            {
                ReadyToPublish.IsChecked = true;
                ReadyToPublish.Select();
            }

            //MainIngredientInput.TextEntry.Text = CurrentRecipe.MainIngredient.Name;

            UpdateIngredients();
        }

        public void UpdateIngredients()
        {
            // if the recipe hasn't yet been saved, do it now so we have a recipe id
            //if (ValidationPassed())
            //{
            //if (!EditingExisting)
            //{
            //Device.BeginInvokeOnMainThread(async () =>
            //{
            //    CurrentRecipe.Id = await DataManager.SaveUserRecipe(AppSession.CurrentUser, CurrentRecipe);
            //await SaveCurrentRecipe();
            //});

            //EditingExisting = true;
            //}

            if (IngredientsContainer != null)
            {
                IngredientsContainer.Children.Clear();

                foreach (Ingredient ingredient in CurrentIngredients)
                {
                    IngredientTile ingredientTile = new IngredientTile(CurrentRecipe/*.Id*/, ingredient);
                    IngredientsContainer.Children.Add(ingredientTile.Content);
                    ingredientTile.SetAsRegularIngredient();

                    if (AppSettings.SimplifiedRecipeEditor)
                    {
                        ingredientTile.NameLabelContainer.GestureRecognizers.Add(
                           new TapGestureRecognizer()
                           {
                               Command = new Command(() =>
                               {
                                   Device.BeginInvokeOnMainThread(async () =>
                                   {
                                       MainIngredient = ingredient;

                                       ingredientTile.SetAsMainIngredient();

                                       await AddAnotherAsMain();

                                       UpdateIngredients();
                                       //await SaveCurrentRecipe();
                                   });
                               })
                           }
                       );
                    }

                    ingredientTile.SetAsRegularIngredient();

                    if (CurrentRecipe.MainIngredient.Name == ingredient.Name)
                    {
                        MainIngredient = CurrentRecipe.MainIngredient;
                    }
                    Console.WriteLine("Main Ingredient " + MainIngredient.Name);
                    Console.WriteLine("Ingredient " + ingredient.Name);

                    if (ingredient.Name == MainIngredient.Name)
                    {
                        ingredientTile.SetAsMainIngredient();
                    }
                }
                CurrentRecipe.Ingredients = CurrentIngredients.ToArray();

                Console.WriteLine("test");
            }
            //}
        }

        public void UpdateCurrentRecipe()
        {
            RecipeContainer.Children.Clear();

            // ingredients
            if (CurrentRecipe.Ingredients == null)
            {
                CurrentRecipe.Ingredients = new List<Ingredient>().ToArray();
            }

            CurrentRecipe.Ingredients = CurrentIngredients.ToArray();

            // method
            if (CurrentRecipe.chai == null)
            {
                CurrentRecipe.chai = new Models.Custom.Feed.Chai();
            }

            // author details
            CurrentRecipe.Author = AppSession.CurrentUser.FirstName + " " + AppSession.CurrentUser.LastName;
            CurrentRecipe.Creator = AppSession.CurrentUser;

            RecipePreviewTile = new UserRecipeTile(CurrentRecipe);

            RecipePreviewTile.titleLbl.SetBinding(Label.TextProperty, new Binding(nameof(CurrentRecipe.Name), source: CurrentRecipe));
            RecipePreviewTile.prepSubDetail.SetBinding(Label.TextProperty, new Binding(nameof(CurrentRecipe.PrepTime), source: CurrentRecipe));
            RecipePreviewTile.totalSubDetail.SetBinding(Label.TextProperty, new Binding(nameof(CurrentRecipe.CookingTime), source: CurrentRecipe));

            RecipeContainer.Children.Add(RecipePreviewTile.Content);

        }

        public async Task RemoveAllRecipes()
        {
            foreach (Recipe recipe in RecipesList)
            {
                await DataManager.DeleteUserRecipe(recipe.Id);
            }
            await Update();
        }


        public bool ValidationPassed()
        {
            int validationFails = 0;

            if (!string.IsNullOrWhiteSpace(NameInput.TextEntry.Text))
            {
                if (NameInput.TextEntry.Text == emptyRecipeName)
                {
                    App.ShowAlert("Dont forget to give this recipe a name!");
                    validationFails++;
                }
            }
            else
            {
                App.ShowAlert("Please add a valid name");
                validationFails++;
            }

            if (string.IsNullOrWhiteSpace(PrepTimeInput.TextEntry.Text))
            {
                App.ShowAlert("Please add a valid prep time");
                validationFails++;
            }

            if (string.IsNullOrWhiteSpace(CookingTimeInput.TextEntry.Text))
            {
                    App.ShowAlert("Please add a valid cooking time");
                    validationFails++;
            }

            if (string.IsNullOrWhiteSpace(ServingsInput.TextEntry.Text))
            {
                    App.ShowAlert("Please add a number of servings");
                    validationFails++;
            }

            if (string.IsNullOrWhiteSpace(MethodEntry.Text))
            {
                MethodEntry.Text = "Method incomplete";
            }

            long servings = 2;

            try { servings = long.Parse(ServingsInput.TextEntry.Text); } catch (Exception e) { }

            CurrentRecipe.Servings = servings;

            if (ReadyToPublish.IsChecked)
            {
                CurrentRecipe.chai.Status = "complete";
                RecipePreviewTile.SetStatus(CurrentRecipe.chai.Status);
            }
            else
            {
                CurrentRecipe.chai.Status = "draft";
                RecipePreviewTile.SetStatus(CurrentRecipe.chai.Status);
            }

            if (validationFails > 0)
            {
                return false;
            }
            return true;
        }


        public async Task SaveCurrentRecipe2()
        {
            if (ValidationPassed())
            {
                if (EditingExisting) { mainIngredientAdded = true; }

                if (mainIngredientAdded)
                {
                    string recipeId = CurrentRecipe.Id;

                    if (!EditingExisting)
                    {
                        recipeId = await DataManager.SaveUserRecipe(AppSession.CurrentUser, CurrentRecipe);
                    }
                }
            }
            else
            {
                // incomplete, save as draft
                CurrentRecipe.chai.Status = "draft";
                ReadyToPublish.IsChecked = false;
                ReadyToPublish.UnSelect();
            }
        }

        public async Task SaveCurrentRecipe()
        {
            if (ValidationPassed())
            {
                if (EditingExisting)
                {
                    mainIngredientAdded = true;
                }

                if (mainIngredientAdded)
                {
                    string recipeId = CurrentRecipe.Id;

                    if (!EditingExisting)
                    {
                        if (MethodEntry.Text != null)
                        {
                            if (MethodEntry.Text.Length > 0)
                            {
                                CurrentRecipe.chai.Method = MethodEntry.Text;
                            }
                        }

                        if (PrepTimeInput.TextEntry.Text != null)
                        {
                            string time = PrepTimeInput.TextEntry.Text;
                            CurrentRecipe.PrepTime = time;
                            CurrentRecipe.chai.PrepTime = long.Parse(time);
                        }

                        if (CookingTimeInput.TextEntry.Text != null)
                        {
                            string time = CookingTimeInput.TextEntry.Text;
                            CurrentRecipe.CookingTime = time;
                            CurrentRecipe.chai.CookTime = long.Parse(time);
                        }

                        if (PrepTimeInput.TextEntry.Text != null && CookingTimeInput.TextEntry.Text != null)
                        {
                            int result = TextTools.MakeStringNumeric(CookingTimeInput.TextEntry.Text) + TextTools.MakeStringNumeric(PrepTimeInput.TextEntry.Text);
                            CurrentRecipe.TotalTime = result.ToString();
                            CurrentRecipe.chai.TotalTime = result;
                        }

                        recipeId = await DataManager.SaveUserRecipe(AppSession.CurrentUser, CurrentRecipe);

                        foreach (Ingredient ingredient in CurrentIngredients)
                        {
                            ingredient.RecipeId = Int32.Parse(recipeId);
                        }
                    }

                    if (recipeId != null)
                    {
                        CurrentRecipe.Id = recipeId;
                        // add / update ingredients
                        if (EditingExisting)
                        {
                            int ingredientsAdded = 0;
                            int ingredientsFailed = 0;
                            int ingredientsUpdated = 0;

                            Recipe r = CurrentRecipe;

                            // created, so update the ingredients
                            foreach (Ingredient ingredient in CurrentIngredients)
                            {
                                // if the ingredient already has an id, it just needs updating
                                if (ingredient.Id > 0)
                                {
                                    if (await DataManager.UpdateRecipeIngredient(AppSession.CurrentUser, CurrentRecipe.chai.Id.ToString(), ingredient))
                                    //if (await DataManager.UpdateRecipeIngredient(AppSession.CurrentUser, recipeId, ingredient))
                                    {
                                        Console.WriteLine("Updated " + ingredient.Name);
                                        ingredientsUpdated++;
                                    }
                                    else
                                    {
                                        //await DataManager.DeleteUserRecipeIngredient(AppSession.CurrentUser, CurrentRecipe.chai.Id.ToString(), ingredient);
                                        if (await DataManager.AddIngredientToRecipe(AppSession.CurrentUser, CurrentRecipe.chai.Id.ToString(), ingredient))
                                        {
                                            Console.WriteLine("Added " + ingredient.Name);
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failed to update " + ingredient.Name);
                                            ingredientsFailed++;
                                        }
                                    }
                                }
                            }
                            Console.WriteLine("Added " + ingredientsAdded + " ingredients");
                            Console.WriteLine("Updated " + ingredientsUpdated + " ingredients");
                            Console.WriteLine("Failed to add " + ingredientsFailed + " ingredients");
                        }
                        else
                        {
                            int ingredientsAdded = 0;
                            int ingredientsFailed = 0;
                            int ingredientsUpdated = 0;

                            Recipe r = CurrentRecipe;

                            // created, so update the ingredients
                            foreach (Ingredient ingredient in CurrentIngredients)
                            {
                                // if the ingredient already has an id, it just needs updating
                                if (ingredient.Id > 0)
                                {

                                    if (await DataManager.AddIngredientToRecipe(AppSession.CurrentUser, recipeId, ingredient))
                                    {
                                        await DataManager.UpdateRecipeIngredient(AppSession.CurrentUser, recipeId, ingredient);
                                        Console.WriteLine("Updated " + ingredient.Name);
                                        ingredientsUpdated++;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failed to update " + ingredient.Name);
                                        ingredientsFailed++;
                                    }

                                }
                            }
                            Console.WriteLine("Added " + ingredientsAdded + " ingredients");
                            Console.WriteLine("Updated " + ingredientsUpdated + " ingredients");
                            Console.WriteLine("Failed to add " + ingredientsFailed + " ingredients");
                        }

                        if (MethodEntry.Text != null)
                        {
                            if (MethodEntry.Text.Length > 0)
                            {
                                CurrentRecipe.chai.Method = MethodEntry.Text;
                            }
                        }

                        if (PrepTimeInput.TextEntry.Text != null)
                        {
                            string time = PrepTimeInput.TextEntry.Text;
                            CurrentRecipe.PrepTime = time;
                            CurrentRecipe.chai.PrepTime = long.Parse(time);
                        }

                        if (CookingTimeInput.TextEntry.Text != null)
                        {
                            string time = CookingTimeInput.TextEntry.Text;
                            CurrentRecipe.CookingTime = time;
                            CurrentRecipe.chai.CookTime = long.Parse(time);
                        }

                        if (PrepTimeInput.TextEntry.Text != null && CookingTimeInput.TextEntry.Text != null)
                        {
                            int result = TextTools.MakeStringNumeric(CookingTimeInput.TextEntry.Text) + TextTools.MakeStringNumeric(PrepTimeInput.TextEntry.Text);
                            CurrentRecipe.TotalTime = result.ToString();
                            CurrentRecipe.chai.TotalTime = result;
                        }

                        if (await DataManager.UpdateUserRecipe(AppSession.CurrentUser, CurrentRecipe))
                        {
                            App.ShowAlert("Your recipe has been submitted!");
                        }
                    }

                    /*
                    EditingExisting = false;

                    NameInput.TextEntry.Text = "";
                    PrepTimeInput.TextEntry.Text = "";
                    CookingTimeInput.TextEntry.Text = "";
                    NewIngredientInput.TextEntry.Text = "";
                    MethodText.Text = "";

                    CurrentIngredients.Clear();
                    */
                    AppSession.SelectedRecipe = CurrentRecipe;
                    await Update();
                    await ShowRecipeList();

                    await RefreshCollectionView();
                    //await EditCurrentRecipe();
                }
                else
                {
                    App.ShowAlert("Please add a main ingredient. Tap on the ingredient you wish to make your core ingredient");
                }
            }
            else
            {
                // incomplete, save as draft
                CurrentRecipe.chai.Status = "draft";
                ReadyToPublish.IsChecked = false;
                ReadyToPublish.UnSelect();
            }
        }

        public async Task<bool> CreateRecipe(Ingredient ingredient)
        {
            //ResetCurrentRecipe();
            CurrentRecipe.MainIngredient.Id = ingredient.Id;
            //CurrentRecipe.Id = await DataManager.SaveUserRecipe(AppSession.CurrentUser, CurrentRecipe);
            return true;
        }

        public async Task<bool> AddIngredient(Ingredient ingredient, bool isMain)
        {
            bool recipeCreated = false;

            if (CurrentRecipe.Id != null)
            {
                recipeCreated = true;
            }
            else
            {
                if (await CreateRecipe(ingredient))
                {
                    recipeCreated = true;
                }
            }

            if (recipeCreated)
            {
                
                //string currentRecipeId = CurrentRecipe.Id;

                //if (currentRecipeId.Length == 0 && CurrentRecipe.chai.Id > 0)
                //{
                //    currentRecipeId = "" + CurrentRecipe.chai.Id;
                //}

                //if (await DataManager.AddIngredientToRecipe(AppSession.CurrentUser, currentRecipeId, ingredient))
                //{
                    CurrentIngredients.Add(ingredient);

                    //await DataManager.UpdateRecipeIngredient(AppSession.CurrentUser, currentRecipeId, ingredient);

                    Console.WriteLine("Added " + ingredient.Name);
                    UpdateIngredients();
                    return true;
                //}
                //else
                //{
                //    Console.WriteLine("Failed to add " + ingredient.Name);

                //}
            }

            return false;
        }

        private CollectionView BuildCollectionView()
        {
            recipeEditorCollectionView = new RecipeEditorCollectionView(BuildListHeader());
            recipeEditorLayout = recipeEditorCollectionView.GetCollectionView();
            recipeEditorCollectionView.ShowBasket();
            return recipeEditorLayout;
        }

        private async Task RefreshCollectionView()
        {
            await Task.Delay(10);
            AppSession.recipeEditorRecipes = DataManager.GetUserRecipes().Result;
            var recipeGroup = new RecipeEditorViewSection(AppSession.recipeEditorRecipes);
            AppSession.recipeEditorCollection.Add(recipeGroup);
            if (AppSession.recipeEditorCollection.Count > 1)
            {
                AppSession.recipeEditorCollection.RemoveAt(0);
            }
        }

        public async Task AddAnotherAsMain()
        {
            await Task.Delay(10);

            Ingredient found = await DataManager.SearchIngredients(MainIngredient.Name);

            if (found.Id > -1)
            {
                MainIngredient = found;

                CurrentRecipe.MainIngredient = MainIngredient;

                if (MainIngredient.MainImage != null)
                {
                    App.ShowAlert(CurrentRecipe.MainIngredient.Name + " added as the main ingredient.");
                    CurrentRecipe.MainIngredient.MainImage = MainIngredient.MainImage;
                }
                else
                {
                    App.ShowAlert(CurrentRecipe.MainIngredient.Name + " added as the main ingredient.\n\nImage not yet available, so we'll use a temporary one for now :)");
                    CurrentRecipe.MainIngredient.MainImage = "chailogo.png";
                }

                mainIngredientAdded = true;

                RecipePreviewTile.Recipe = CurrentRecipe;
                RecipePreviewTile.Recipe.MainImageSource = MainIngredient.MainImage;

                RecipePreviewTile.recipeImage.Content.Source = MainIngredient.MainImage;

                RecipePreviewTile.UpdateContents();


            }
            else
            {
                App.ShowAlert("Cannot find " + MainIngredient.Name);
            }
        }

        public async Task<bool> DeleteUnsavedIngredient(Ingredient ingredient)
        {
            
            if (CurrentIngredients.Find(x => x.Name == ingredient.Name) != null)
            {
                CurrentIngredients.Remove(ingredient);
                UpdateIngredients();
                await Task.Delay(500);
                //await this.Update();
                return true;
            }
            return false;
        }

        public async Task AddMainAsAnotherIngredient(string ingredientName)
        {
            await Task.Delay(10);

            try
            {
                if (ingredientName.Length > 0)
                {
                    Ingredient found = await DataManager.SearchIngredients(ingredientName);

                    if (found.Id > -1)
                    {
                        if (CurrentIngredients.Find(x => x.Name == ingredientName) == null)
                        {
                            CurrentIngredients.Add(new Ingredient
                            {
                                Name = ingredientName,
                                Amount = "2",
                                BaseIngredient = null,
                                ID = -1,
                                Id = -1,
                                LongDescription = "",
                                MainImage = "chaismallbag.png",
                                Text = ingredientName,
                                ShortDescription = ingredientName,
                                Unit = AppDataContent.IngredientUnits[0]
                            });
                            NewIngredientInput.TextEntry.Text = "";
                            UpdateIngredients();
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        App.ShowAlert("We cannot find that ingredient in our database.");
                    }
                }
                else
                {
                    App.ShowAlert("Please enter a vaid ingredient");
                }
            }
            catch (Exception e)
            {
                App.ShowAlert("Please enter a vaid ingredient");
            }
        }


        private void PopulateFoundIngredients(List<Ingredient> foundList)
        {
            Console.WriteLine("t0");
            IngrdntPicker.GetContent().IsVisible = false;
            IngrdntPicker.FoundIngredientsPicker.Items.Clear();
            Console.WriteLine("t1");
            foreach (Ingredient ingredient in foundList)
            {
                IngrdntPicker.FoundIngredientsPicker.Items.Add(ingredient.Name);
            }

            if (IngrdntPicker.FoundIngredientsPicker.Items.Count > 0)
            {
                IngrdntPicker.GetContent().IsVisible = true;
                IngrdntPicker.ShowText(true);
                //FoundIngredientsPicker.SelectedIndex = 0;
            }
            else
            {

            }
            Console.WriteLine("t2");
        }

        public async Task AddAnotherIngredient()
        {
            await Task.Delay(10);

            try
            {
                if (NewIngredientInput.TextEntry.Text.Length > 0)
                {
                    FoundIngredients = await DataManager.SearchIngredients(NewIngredientInput.TextEntry.Text, false);

                    if (FoundIngredients.Count > 0)
                    {
                        PopulateFoundIngredients(FoundIngredients);
                        //await SaveCurrentRecipe();
                    }
                    else
                    {
                        App.ShowAlert("We cannot find that ingredient in our database.");
                    }
                }
                else
                {
                    App.ShowAlert("Please enter a vaid ingredient");
                }
            }
            catch (Exception e)
            {
                App.ShowAlert("Please enter a vaid ingredient");
            }
        }

        public async Task<bool> RemoveIngredient(Ingredient ingredient)
        {
            await Task.Delay(10);
            if (CurrentIngredients.Find(x => x.Name == ingredient.Name) == null)
            {
                //App.ShowAlert(ingredient.Name + " not found");
                return false;
            }
            else
            {
                CurrentIngredients.Remove(ingredient);
                UpdateIngredients();
                App.ShowAlert("Removed " + ingredient.Name);
            }

            return true;
        }

        public override async Task Update()
        {
            await base.Update();
            if (StaticData.recipeEditorClicked)
            {
                await RefreshCollectionView();

                if (AppDataContent.IngredientUnits.Count <= 0)
                {
                    AppDataContent.IngredientUnits = await DataManager.GetUnits();
                }
            }
            App.SetSubHeaderTitle("", null);

            double totalRecipes = AppSession.TotalRecipes;
            double itemsPerPage = ApiBridge.ITEMS_PER_REQUEST;
            AppSession.TotalPages = (int)Math.Ceiling(totalRecipes / itemsPerPage);

            App.SetSubHeaderTitle(AppText.RECOMMENDED_RECIPES, new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.RecommendedRecipes));
        }
    }
}