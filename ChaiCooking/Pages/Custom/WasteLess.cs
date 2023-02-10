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
using ChaiCooking.DebugData.Custom;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using ChaiCooking.Views.CollectionViews;
using ChaiCooking.Views.CollectionViews.IngredientFilter;
using ChaiCooking.Views.CollectionViews.WasteLess;
using FFImageLoading.Transformations;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class WasteLess : Page
    {
        StackLayout ContentContainer, pageNavBar, showHideContainer;
        StaticLabel Title, Info, PageInfo;
        StaticImage collapseArrow, expandArrow, LastArrow, NextArrow;
        Grid seperator, masterGrid, secondaryGrid;
        CustomEntry availableInput;
        ScrollView scrollContainer;
        CollectionView ingredientsLayout, recipesLayout;
        IngredientsFilterCollectionView ingredientsCollectionView;
        WasteLessCollectionView wasteLessCollectionView;

        public WasteLess()
        {
            this.IsScrollable = false;
            this.IsRefreshable = false;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;
            AppSession.CurrentPageWaste = 1;
            AppSession.TotalPages = 1;
            //bool isShowing = true;
            this.Id = (int)AppSettings.PageNames.WasteLess;
            this.Name = AppData.AppText.WASTE_LESS;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.SlideInFromRight;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.SlideOutToRight;

            PageContent = new Grid
            {
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING)
            };

            #region - Tint Colour
            Color tintColour = Color.White;

            TintTransformation colorTint = new TintTransformation
            {
                HexColor = (string)tintColour.ToHex(),
                EnableSolidColor = true

            };
            #endregion

            #region - NavBar
            pageNavBar = new StackLayout
            {
                WidthRequest = Units.ScreenWidth,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                //BackgroundColor = Color.Blue
            };

            PageInfo = new StaticLabel("");
            PageInfo.Content.TextColor = Color.White;
            PageInfo.Content.FontSize = Units.FontSizeM;
            PageInfo.Content.Padding = new Thickness(Dimensions.GENERAL_COMPONENT_SPACING, 8);
            PageInfo.CenterAlign();

            Color arrowTintColor = Color.LightGray;
            TintTransformation arrowTint = new TintTransformation
            {
                HexColor = (string)arrowTintColor.ToHex(),
                EnableSolidColor = true

            };
            var tint = new List<FFImageLoading.Work.ITransformation>();
            tint.Add(arrowTint);

            LastArrow = new StaticImage("chevronleftbold.png", 48, tint);
            LastArrow.Content.HorizontalOptions = LayoutOptions.Start;
            LastArrow.Content.VerticalOptions = LayoutOptions.Center;
            LastArrow.Content.HeightRequest = 20;
            NextArrow = new StaticImage("chevronrightbold.png", 48, tint);
            NextArrow.Content.HorizontalOptions = LayoutOptions.End;
            NextArrow.Content.VerticalOptions = LayoutOptions.Center;
            NextArrow.Content.HeightRequest = 20;

            if (AppSession.CurrentPageWaste <= 1)
            {
                LastArrow.Content.Opacity = 0;
            }

            if (AppSession.TotalPages <= 1)
            {
                NextArrow.Content.Opacity = 0;
            }

            TouchEffect.SetNativeAnimation(LastArrow.Content, true);
            TouchEffect.SetCommand(LastArrow.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (LastArrow.Content.Opacity == 0)
                        {

                        }
                        else
                        {
                            GetLast();
                        }
                    });
                }));

            TouchEffect.SetNativeAnimation(NextArrow.Content, true);
            TouchEffect.SetCommand(NextArrow.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        GetNext();
                    });
                }));

            pageNavBar.Children.Add(LastArrow.Content);
            pageNavBar.Children.Add(PageInfo.Content);
            pageNavBar.Children.Add(NextArrow.Content);
            #endregion

            #region - Title and Info
            Title = new StaticLabel(AppData.AppText.WASTE_LESS + ".");
            Title.Content.TextColor = Color.White;
            Title.Content.FontSize = Units.FontSizeXXL;
            Title.Content.Padding = Dimensions.GENERAL_COMPONENT_SPACING;
            Title.CenterAlign();

            Info = new StaticLabel(AppData.AppText.WASTE_LESS_INFO);
            Info.Content.TextColor = Color.White;
            Info.Content.FontSize = Units.FontSizeM;
            Info.Content.Padding = Dimensions.GENERAL_COMPONENT_SPACING;
            Info.Content.FontFamily = Fonts.GetRegularAppFont();
            Info.CenterAlign();
            #endregion

            #region - Search And Drop Bar

            IconButton searchButton = new IconButton(52, 32, Color.FromHex(Colors.CC_ORANGE), Color.White, "", "searchicon.png", null);
            searchButton.SetContentCenter();
            searchButton.ContentContainer.Padding = 4;
            searchButton.SetIconSize(32, 32);
            searchButton.SetPositionLeft();
            searchButton.Icon.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
            searchButton.Icon.Content.Transformations.Add(colorTint);

            TouchEffect.SetNativeAnimation(searchButton.Content, true);
            TouchEffect.SetCommand(searchButton.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (AppSession.InfoModeOn)
                        {
                            //App.ShowInfoBubble(searchButton.Content, "Tap the search button to open the search menu where you can create ingredient filters.");
                            App.ShowInfoBubble(new Paragraph("Search", "Tap the search button to open the search menu where you can create ingredient filters.", null).Content, (int)Units.HalfScreenWidth, (int)Units.HalfScreenHeight);

                        }
                        else
                        {
                            await App.ShowIngredientFilterModal();
                        }
                    });
                }));

            StackLayout searchContainer = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    searchButton.Content
                }
            };

            //Grid searchAndDropCont = new Grid
            //{
            //    VerticalOptions = LayoutOptions.FillAndExpand,
            //    HorizontalOptions = LayoutOptions.FillAndExpand,
            //    WidthRequest = Units.ScreenWidth,
            //    //BackgroundColor = Color.FromHex(Colors.CC_GREEN),
            //    RowDefinitions =
            //    {
            //        new RowDefinition { Height = new GridLength(34)}
            //    },
            //    ColumnDefinitions =
            //    {
            //        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto)},
            //        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)},
            //        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto)},

            //    },
            //    Children =
            //    {
            //        { searchContainer, 0,0 },
            //        //{ ingredientsLabel, 1,0 },
            //        { showHideContainer, 2,0 },
            //    }
            //};

            #endregion

            #region - Seperator 1

            Grid seperator = new Grid
            {
                WidthRequest = Units.ScreenWidth,
                Margin = 3,
                HeightRequest = 1,
                BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY)
            };

            #endregion

            #region - Seperator 2

            Grid seperator2 = new Grid
            {
                WidthRequest = Units.ScreenWidth,
                Margin = 3,
                HeightRequest = 1,
                BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY)
            };

            #endregion

            StackLayout SearchBox = new StackLayout
            {
                WidthRequest = Units.ScreenWidth,
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };


            StaticLabel SearchBoxDesc1 = new StaticLabel("Add your available ingredients.");
            SearchBoxDesc1.Content.TextColor = Color.White;
            SearchBoxDesc1.Content.FontSize = Units.FontSizeM;
            SearchBoxDesc1.Content.Padding = new Thickness(Dimensions.GENERAL_COMPONENT_SPACING, 8);
            SearchBoxDesc1.CenterAlign();

           
            SearchBoxDesc1.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                if (AppSession.InfoModeOn)
                                {
                                    double x = Tools.Screen.GetScreenCoordinates(SearchBoxDesc1.Content).X;
                                    double y = Tools.Screen.GetScreenCoordinates(SearchBoxDesc1.Content).Y;
                                    //App.ShowInfoBubble(new Label { Text = "Type your ingredients here. You can add multiple ingredients and we will return content to match your ingredients." }, (int)x, (int)y);

                                    App.ShowInfoBubble(new Paragraph("Search", "Type your ingredients here. You can add multiple ingredients and we will return content to match your ingredients.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);

                                }
                            });
                        })
                    }
                );

            availableInput = new CustomEntry
            {
                Placeholder = "Add an Ingredient",
                PlaceholderColor = Color.LightGray,
                HorizontalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.White,
                WidthRequest = Units.HalfScreenWidth,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = Units.TapSizeS,
                FontSize = Units.FontSizeL,
            };

            IconButton addButton = new IconButton(52, 32, Color.FromHex(Colors.CC_ORANGE), Color.White, "", "plus.png", null);
            addButton.SetContentCenter();
            addButton.ContentContainer.Padding = 4;
            addButton.SetIconSize(32, 32);

            TouchEffect.SetNativeAnimation(addButton.Content, true);
            TouchEffect.SetCommand(addButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        double x = Tools.Screen.GetScreenCoordinates(addButton.Content).X;
                        double y = Tools.Screen.GetScreenCoordinates(addButton.Content).Y;
                        //App.ShowInfoBubble(new Label { Text = "Tap the + button, when you’ve typed in your ingredients, to add them to the list of filters on this page. The recipe finder will update appropriately." }, (int)x, (int)y);

                        App.ShowInfoBubble(new Paragraph("Add Ingredient", "Tap the + button, when you’ve typed in your ingredients, to add them to the list of filters on this page. The recipe finder will update appropriately.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);

                    }
                    else
                    {
                        await AddIngredient();
                    }
                });
            }));

            StackLayout SearchBoxMid = new StackLayout
            {
                WidthRequest = Units.ScreenWidth,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                Children =
                {
                    availableInput,
                    addButton.Content
                }
            };

            StaticLabel SearchBoxDesc2 = new StaticLabel("Your ingredients.");
            SearchBoxDesc2.Content.TextColor = Color.White;
            SearchBoxDesc2.Content.FontSize = Units.FontSizeM;
            SearchBoxDesc2.Content.Padding = new Thickness(Dimensions.GENERAL_COMPONENT_SPACING, 8);
            SearchBoxDesc2.CenterAlign();


            SearchBox.Children.Add(SearchBoxDesc1.Content);
            SearchBox.Children.Add(SearchBoxMid);
            SearchBox.Children.Add(SearchBoxDesc2.Content);

            secondaryGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0,
                RowDefinitions =
                    {
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition { Height = new GridLength(10) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    },
                Children =
                {
                    { Title.Content, 0, 0},
                    { Info.Content, 0, 1},
                    { SearchBox, 0, 2},
                    { seperator, 0, 3 },
                    { BuildIngredientSection(), 0, 5 },
                    { seperator2, 0, 6},
                    { BuildRecipesSection(), 0, 7 },
                }
            };
            scrollContainer = new ScrollView()
            {
                Content = secondaryGrid
            };

            #region - Master Grid
            masterGrid = new Grid
            {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                RowSpacing = 0,
                RowDefinitions =
                    {
                        new RowDefinition { Height = new GridLength(50) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    },
                Children =
                {
                    { pageNavBar, 0, 0 },
                    { scrollContainer, 0, 1 },
                }
            };

            PageContent.Children.Add(masterGrid);

            #endregion

            AppSession.wasteLessUpdate = CallUpdate;
            AppSession.UserCollectionRecipes = DataManager.GetFavouriteRecipes();
            AppSession.shoppingList = DataManager.GetShoppingList().Result;
        }

        private CollectionView BuildIngredientSection()
        {
            ingredientsCollectionView = new IngredientsFilterCollectionView();
            ingredientsLayout = ingredientsCollectionView.GetCollectionView();
            ingredientsCollectionView.ShowIngredients();
            return ingredientsLayout;
        }

        private CollectionView BuildRecipesSection()
        {
            wasteLessCollectionView = new WasteLessCollectionView();
            recipesLayout = wasteLessCollectionView.GetCollectionView();
            wasteLessCollectionView.ShowRecipes(null);
            return recipesLayout;
        }
        bool allowUpdate = false;
        public void GetNext()
        {
            if (AppSession.CurrentPageWaste < AppSession.TotalPages)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    AppSession.GetNextPage = true;
                    AppSession.GetLastPage = false;
                    AppSession.CurrentPageWaste++;
                    if (!allowUpdate)
                    {
                        allowUpdate = true;
                        await UpdateData();
                        RefreshCollectionView();
                        allowUpdate = false;
                    }
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
                if (AppSession.CurrentPageWaste > 1)
                {
                    AppSession.CurrentPageWaste--;
                }
                if (!allowUpdate)
                {
                    allowUpdate = true;
                    await UpdateData();
                    RefreshCollectionView();
                    allowUpdate = false;
                }
                AppSession.GetNextPage = false;
                AppSession.GetLastPage = false;
                SetPageTotal();
            });
        }


        private async Task UpdateData()
        {
            await App.ShowLoading();
            await Task.Delay(10);
            AppSession.WasteLessRecipes = DataManager.GetWasteLessRecipes(AppSession.CurrentUser, true);
            await App.HideLoading();
        }

        public void RefreshCollectionView()
        {
            var wasteLessGroup = new RecipesCollectionViewSection(AppSession.WasteLessRecipes);
            AppSession.wasteLessCollection.Add(wasteLessGroup);
            AppSession.wasteLessCollection.RemoveAt(0);
        }

        private async void CallUpdate()
        {
            await Update();
        }

        public override async Task Update()
        {
            await base.Update();
            SetPageTotal();
        }

        private void SetPageTotal()
        {
            AppSession.TotalPages = (AppSession.TotalRecipes + ApiBridge.ITEMS_PER_REQUEST - 1) / ApiBridge.ITEMS_PER_REQUEST;

            PageInfo.Content.WidthRequest = Units.HalfScreenWidth;
            PageInfo.Content.Text = "PAGE " + AppSession.CurrentPageWaste + "/" + AppSession.TotalPages + "\n(of " + AppSession.TotalRecipes + " recipes)";

            if (AppSession.CurrentPageWaste <= 1)
            {
                LastArrow.Content.Opacity = 0;
            }
            else
            {
                LastArrow.Content.Opacity = 1;
            }

            if (AppSession.CurrentPageWaste < AppSession.TotalPages)
            {
                NextArrow.Content.Opacity = 1;
            }
            else
            {
                NextArrow.Content.Opacity = 0;
            }

            if (AppSession.TotalRecipes < 1)
            {
                PageInfo.Content.Text = "1/1";
                NextArrow.Content.Opacity = 0;
                LastArrow.Content.Opacity = 0;
            }
            if (AppSession.TotalRecipes <= ApiBridge.ITEMS_PER_REQUEST)
            {
                PageInfo.Content.Text = "1/1";
                NextArrow.Content.Opacity = 0;
                LastArrow.Content.Opacity = 0;
            }
        }

        private async Task<bool> AddIngredient()
        {
            if (availableInput.Text != null)
            {
                if (availableInput.Text.Count() > 0)
                {
                    bool containsItem = false;
                    await Task.Delay(50);
                    try
                    {
                        containsItem = AppDataContent.AvailableIngredients.Any(item => item.Name.ToLower() == availableInput.Text.ToLower());
                    }
                    catch
                    {
                        Console.WriteLine("ERROR");
                    }

                    if (containsItem)
                    {
                        //AvailableInput.Text = "";
                        App.ShowAlert("Ingredient already listed.");
                        return false;
                    }
                    else
                    {
                        //AvailableInput.Text = "";
                        AppDataContent.AvailableIngredients.Add(new Ingredient { Id = 0, Name = availableInput.Text, ShortDescription = "", LongDescription = "", MainImage = "" });
                        var ingredientsFilterGroup = new IngredientsCollectionViewSection(AppDataContent.AvailableIngredients, BuildEmpty());
                        AppSession.ingredientsCollection.Add(ingredientsFilterGroup);
                        AppSession.ingredientsCollection.RemoveAt(0);
                        DataManager.FilterRecipesByIngredients(AppDataContent.AvailableIngredients, AppDataContent.AvoidedIngredients);
                        AppSession.WasteLessRecipes = DataManager.GetWasteLessRecipes(AppSession.CurrentUser, true);
                        var wasteLessGroup = new RecipesCollectionViewSection(AppSession.WasteLessRecipes);
                        AppSession.wasteLessCollection.Add(wasteLessGroup);
                        AppSession.wasteLessCollection.RemoveAt(0);
                        AppSession.wasteLessUpdate();
                        availableInput.Text = "";

                        return true;
                    }
                }
                App.ShowAlert("Enter a valid ingredient");
                availableInput.Text = "";
                return false;
            }
            App.ShowAlert("Enter a valid ingredient");
            availableInput.Text = "";
            return false;
        }

        private StackLayout BuildEmpty()
        {
            StackLayout emptyCont = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                Children =
                    {
                        new Label
                        {
                            Text = "No ingredients added.",
                            FontSize = Units.FontSizeL,
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Color.White,
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                    }
            };

            return emptyCont;
        }
    }
}