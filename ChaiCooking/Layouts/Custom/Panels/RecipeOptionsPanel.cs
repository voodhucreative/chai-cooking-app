using System;
using System.Collections.Generic;
using System.Linq;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using ChaiCooking.Services.Storage;
using ChaiCooking.Views.CollectionViews.Collections;
using ChaiCooking.Views.CollectionViews.ShoppingBasket;
using FFImageLoading.Transformations;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Panels
{
    public class RecipeOptionsPanel : StandardLayout
    {
        StaticLabel recipeOptionsLabel, shareLabel, addToMealPlanLabel,
            addToCollectionLabel, shoppingBasketLabel, closeLabel;

        StackLayout blackFrameCont, innerContainer,
            masterContainer;

        CollectionView collectionsLayout;
        RecipeOptionsCollectionView recipeOptionsCollectionView;

        Frame addFrame;

        ChaiCooking.Models.Custom.Recipe optionsRecipe { get; set; }

        enum Choice
        {
            Share,
            MealPlan,
            Collection,
            ShoppingBasket,
            Close
        }

        public RecipeOptionsPanel(ChaiCooking.Models.Custom.Recipe recipe)
        {
            this.optionsRecipe = recipe;

            Content = new Grid
            {
                WidthRequest = Units.HalfScreenWidth + Units.ScreenWidth40Percent,
                //HeightRequest = Units.ScreenHeight,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                //BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            recipeOptionsLabel = new StaticLabel("Recipe Options");
            recipeOptionsLabel.Content.TextColor = Color.White;
            recipeOptionsLabel.Content.FontSize = Units.FontSizeXXL;
            recipeOptionsLabel.Content.FontFamily = Fonts.GetRegularAppFont();
            recipeOptionsLabel.LeftAlign();

            shareLabel = BuildOptionsLabel("Share", Color.White, Units.FontSizeL, Fonts.GetBoldAppFont(), new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (AppSession.InfoModeOn)
                        {
                            double x = ChaiCooking.Tools.Screen.GetScreenCoordinates(shareLabel.Content).X;
                            double y = ChaiCooking.Tools.Screen.GetScreenCoordinates(shareLabel.Content).Y;
                            //App.ShowInfoBubble(new Label { Text = "Tap ‘Share’ to share the recipe on your Facebook page." }, (int)x, (int)y);
                            App.ShowInfoBubble(new Paragraph("Share", "Tap ‘Share’ to share the recipe on your Facebook page.", null).Content, (int)Units.HalfScreenWidth, (int)Units.HalfScreenHeight);

                        }
                        else
                        {
                            SetLabelColours(Choice.Share);
                            BuildShareContainer();
                        }
                    });
                }));

            addToMealPlanLabel = BuildOptionsLabel("Add to Meal Plan", Color.White, Units.FontSizeL, Fonts.GetBoldAppFont(), new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (AppSession.InfoModeOn)
                        {
                            double x = ChaiCooking.Tools.Screen.GetScreenCoordinates(addToMealPlanLabel.Content).X;
                            double y = ChaiCooking.Tools.Screen.GetScreenCoordinates(addToMealPlanLabel.Content).Y;
                            //App.ShowInfoBubble(new Label { Text = "Add the recipe/s to a Meal Plan " }, (int)x, (int)y);
                            App.ShowInfoBubble(new Paragraph("Add to Meal Plan", "Add the recipe/s to a Meal Plan.", null).Content, (int)Units.HalfScreenWidth, (int)Units.HalfScreenHeight);

                        }
                        else
                        {
                            SetLabelColours(Choice.MealPlan);
                            BuildMealPlanContainer();
                        }
                    });
                }));

            addToCollectionLabel = BuildOptionsLabel("Add to Collection", Color.White, Units.FontSizeL, Fonts.GetBoldAppFont(), new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (AppSession.InfoModeOn)
                        {
                            double x = ChaiCooking.Tools.Screen.GetScreenCoordinates(addToCollectionLabel.Content).X;
                            double y = ChaiCooking.Tools.Screen.GetScreenCoordinates(addToCollectionLabel.Content).Y;
                            //App.ShowInfoBubble(new Label { Text = "Add the recipe/s to a Collection (album)" }, (int)x, (int)y);
                            App.ShowInfoBubble(new Paragraph("Add to Collection", "Add the recipe/s to a Collection (album)", null).Content, (int)Units.HalfScreenWidth, (int)Units.HalfScreenHeight);

                        }
                        else
                        {
                            SetLabelColours(Choice.Collection);
                            BuildCollectionContainer();
                        }
                    });
                }));

            shoppingBasketLabel = BuildOptionsLabel("Quick Shop", Color.White, Units.FontSizeL, Fonts.GetBoldAppFont(), new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        double x = ChaiCooking.Tools.Screen.GetScreenCoordinates(shoppingBasketLabel.Content).X;
                        double y = ChaiCooking.Tools.Screen.GetScreenCoordinates(shoppingBasketLabel.Content).Y;
                        //App.ShowInfoBubble(new Label { Text = "Add the recipe/s to a trolley for convenient shopping." }, (int)x, (int)y);
                        App.ShowInfoBubble(new Paragraph("Quick Shop", "Add the recipe/s to a trolley for convenient shopping.", null).Content, (int)Units.HalfScreenWidth, (int)Units.HalfScreenHeight);

                    }
                    else
                    {
                        SetLabelColours(Choice.ShoppingBasket);
                        BuildShoppingBasketContainer();
                    }
                });
            }));

            innerContainer = new StackLayout
            {
                BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY),
                IsClippedToBounds = true,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
            };

            blackFrameCont = new StackLayout
            {
                Padding = 1,
                BackgroundColor = Color.Black,
                Children =
                {
                    innerContainer
                }
            };

            closeLabel = new StaticLabel(AppText.CLOSE);
            closeLabel.Content.TextColor = Color.White;
            closeLabel.Content.FontSize = Units.FontSizeL;
            closeLabel.Content.FontFamily = Fonts.GetRegularAppFont();
            closeLabel.RightAlign();

            TouchEffect.SetNativeAnimation(closeLabel.Content, true);
            TouchEffect.SetCommand(closeLabel.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        SetLabelColours(Choice.Close);
                        BuildEmptyContainer();
                        AppSession.SelectedRecipe = null;
                        await App.HideRecipeSummary();
                    });
                }));

            masterContainer = new StackLayout
            {
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                Children =
                {
                    closeLabel.Content,
                    recipeOptionsLabel.Content,
                    shareLabel.Content,
                    addToMealPlanLabel.Content,
                    addToCollectionLabel.Content,
                    shoppingBasketLabel.Content,
                    blackFrameCont,
                }
            };

            if (!SubscriptionManager.CanAccessPaidContent(AppSession.CurrentUser).Result)
            {
                masterContainer.Children.Remove(addToMealPlanLabel.Content);
            }

            if (Helpers.Pages.CurrentPage == (int)AppSettings.PageNames.ShoppingBasket)
            {
                masterContainer.Children.Remove(shoppingBasketLabel.Content);
            }

            BuildEmptyContainer();

            Content.Children.Add(masterContainer);
        }

        private void BuildEmptyContainer()
        {
            innerContainer.Children.Clear();

            Label selectOptionsLabel = new Label
            {
                Text = "Select an option",
                TextColor = Color.White,
                FontFamily = Fonts.GetBoldAppFont(),
                FontSize = Units.FontSizeL,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };

            StackLayout selectOptionsContent = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    selectOptionsLabel
                }
            };

            Grid selectOptionsGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                RowSpacing = 10,
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                },
                Children = {
                    { selectOptionsContent, 0, 0 },
                },
            };

            innerContainer.Children.Add(selectOptionsGrid);
        }

        private void BuildMealPlanContainer()
        {
            innerContainer.Children.Clear();

            Label addContentLabel = new Label
            {
                Text = "Add to Meal Plan",
                TextColor = Color.White,
                FontFamily = Fonts.GetBoldAppFont(),
                FontSize = Units.FontSizeL,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center
            };


            StackLayout topAddContent = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                Children =
                {
                    addContentLabel
                }
            };

            Label addDescLabel = new Label
            {
                Text = "Add selected\nto Meal Plan",
                TextColor = Color.White,
                FontFamily = Fonts.GetBoldAppFont(),
                FontSize = Units.FontSizeL,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center
            };

            StaticImage addIcon = new StaticImage("plus.png", 10, 10, null);

            StackLayout addIconContent = new StackLayout
            {
                Children =
                {
                    addIcon.Content
                }
            };

            addFrame = new Frame
            {
                Content = addIconContent,
                CornerRadius = 20,
                Padding = 10,
                HeightRequest = 20,
                WidthRequest = 60,
                BackgroundColor = Color.Orange,
                HasShadow = false,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
            };

            TouchEffect.SetNativeAnimation(addFrame, true);
            TouchEffect.SetCommand(addFrame,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            var search = AppSession.CurrentUser.recipeHolder.Find(x => x.Id == optionsRecipe.Id);
                            if (search != null)
                            {
                                App.ShowAlert("Recipe already exists.");
                                addFrame.BackgroundColor = Color.LightGray;
                                addFrame.Content.BackgroundColor = Color.LightGray;
                            }
                            else
                            {
                                AppSession.CurrentUser.recipeHolder.Add(optionsRecipe);
                                App.ShowAlert($"Successfully added {optionsRecipe.Name}.");
                                addFrame.BackgroundColor = Color.LightGray;
                                addFrame.Content.BackgroundColor = Color.LightGray;
                                LocalDataStore.SaveAll();
                            }
                            search = null;
                        }
                        catch
                        {
                            App.ShowAlert($"Failed to add {optionsRecipe.Name}.");
                        }
                    });
                }));

            if (AppSession.CurrentUser.recipeHolder == null)
            {
                AppSession.CurrentUser.recipeHolder = new List<Models.Custom.Recipe>();
            }
            var search = AppSession.CurrentUser.recipeHolder.Find(x => x.Id == optionsRecipe.Id);
            if (search != null)
            {
                addFrame.BackgroundColor = Color.LightGray;
                addFrame.Content.BackgroundColor = Color.LightGray;
            }

            StackLayout midAddContent = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 4,
                Children = {
                    addDescLabel,
                    addFrame
                },
            };

            Label infoDescLabel = new Label
            {
                Text = "The recipes you add will be available from the holding area at the bottom.",
                TextColor = Color.White,
                FontFamily = Fonts.GetBoldAppFont(),
                FontSize = Units.FontSizeM,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.WordWrap,
            };

            StackLayout bottomAddContent = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    infoDescLabel
                }
            };

            Grid addContent = new Grid
            {
                IsClippedToBounds = true,
                RowSpacing = 4,
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                    { new RowDefinition { Height = 90}},
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                },
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    { topAddContent, 0, 0 },
                    { midAddContent, 0, 1 },
                    { bottomAddContent, 0, 2 }
                },
            };


            ScrollView scrollView = new ScrollView()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Content = addContent
            };


            innerContainer.Children.Add(scrollView);
        }

        private void BuildCollectionContainer()
        {
            innerContainer.Children.Clear();

            Label addLabel = new Label
            {
                Text = "Add to Collection",
                TextColor = Color.White,
                FontFamily = Fonts.GetBoldAppFont(),
                FontSize = Units.FontSizeL,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };

            StackLayout addContent = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    addLabel
                }
            };

            Grid addGrid = new Grid
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = 2,
                RowSpacing = 10,
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                    { new RowDefinition { Height = 130}},
                },
                Children = {
                    { addContent, 0, 0 },
                    { BuildCollectionView(), 0, 1 },
                },
            };

            AppSession.SelectedRecipe = optionsRecipe;

            innerContainer.Children.Add(addGrid);
        }

        private void BuildShareContainer()
        {
            innerContainer.Children.Clear();

            Label shareContentLabel = new Label
            {
                Text = "Share",
                TextColor = Color.White,
                FontFamily = Fonts.GetBoldAppFont(),
                FontSize = Units.FontSizeL,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center
            };

            StackLayout topShareContent = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                Children =
                {
                    shareContentLabel
                }
            };

            Label shareDescLabel = new Label
            {
                Text = "Create Post\non Facebook",
                TextColor = Color.White,
                FontFamily = Fonts.GetBoldAppFont(),
                FontSize = Units.FontSizeL,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center
            };

            Color tintColour = Color.FromHex("#4267B2");

            TintTransformation colorTint = new TintTransformation
            {
                HexColor = (string)tintColour.ToHex(),
                EnableSolidColor = true

            };

            StaticImage facebookIcon = new StaticImage("facebookblack.png", 20, 20, null);
            facebookIcon.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
            facebookIcon.Content.Transformations.Add(colorTint);

            Frame fbFrame = new Frame
            {
                Content = facebookIcon.Content,
                CornerRadius = 31,
                Padding = 0,
                HeightRequest = 40,
                WidthRequest = 40,
                BackgroundColor = Color.White,
                HasShadow = false,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
            };

            StackLayout midShareContent = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children = {
                    shareDescLabel,
                    fbFrame
                },
            };

            ColourButton createButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Create", null);
            createButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            createButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;

            TouchEffect.SetNativeAnimation(createButton.Content, true);
            TouchEffect.SetCommand(createButton.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Share.RequestAsync(new ShareTextRequest
                        {
                            Uri = "https://chai.cooking/",
                            Title = "Chai Cooking"
                        });
                    });
                }));

            StackLayout bottomShareContent = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Children =
                {
                    createButton.Content
                }
            };

            Grid shareContent = new Grid
            {
                IsClippedToBounds = true,
                RowSpacing = 10,
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                    { new RowDefinition { Height = 90}},
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                },
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    { topShareContent, 0, 0 },
                    { midShareContent, 0, 1 },
                    { bottomShareContent, 0, 2 }
                },
            };

            innerContainer.Children.Add(shareContent);
        }

        private void BuildShoppingBasketContainer()
        {
            innerContainer.Children.Clear();

            Label addContentLabel = new Label
            {
                Text = "Quick Shop",
                TextColor = Color.White,
                FontFamily = Fonts.GetBoldAppFont(),
                FontSize = Units.FontSizeL,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center
            };


            StackLayout topAddContent = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                Children =
                {
                    addContentLabel
                }
            };

            Label addDescLabel = new Label
            {
                Text = "Add selected\nto trolley",
                TextColor = Color.White,
                FontFamily = Fonts.GetBoldAppFont(),
                FontSize = Units.FontSizeL,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center
            };

            StaticImage addIcon = new StaticImage("plus.png", 10, 10, null);

            StackLayout addIconContent = new StackLayout
            {
                Children =
                {
                    addIcon.Content
                }
            };

            addFrame = new Frame
            {
                Content = addIconContent,
                CornerRadius = 20,
                Padding = 10,
                HeightRequest = 20,
                WidthRequest = 60,
                BackgroundColor = Color.Orange,
                HasShadow = false,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
            };

            TouchEffect.SetNativeAnimation(addFrame, true);
            TouchEffect.SetCommand(addFrame,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            var search = AppSession.shoppingList.content.recipes.Find(x => x.Id == optionsRecipe.Id);
                            if (search != null)
                            {
                                App.ShowAlert("Recipe already exists.");
                                addFrame.BackgroundColor = Color.LightGray;
                                addFrame.Content.BackgroundColor = Color.LightGray;
                            }
                            else
                            {
                                await DataManager.AddRecipeToShoppingList(optionsRecipe);
                                App.ShowAlert($"Successfully added {optionsRecipe.Name}.");
                                addFrame.BackgroundColor = Color.LightGray;
                                addFrame.Content.BackgroundColor = Color.LightGray;
                                AppSession.shoppingList.content.recipes.Add(optionsRecipe);

                                var shoppingBasketGroup = new ShoppingBasketViewSection(AppSession.shoppingList.content.recipes);//, StaticData.BuildEmpty());

                                AppSession.shoppingBasketCollection.Add(shoppingBasketGroup);
                                if (AppSession.shoppingBasketCollection.Count > 1)
                                {
                                    AppSession.shoppingBasketCollection.RemoveAt(0);
                                }
                            }
                            search = null;
                        }
                        catch
                        {
                            App.ShowAlert($"Failed to add {optionsRecipe.Name}.");
                        }
                    });
                }));

            if (AppSession.shoppingList == null)
            {
                AppSession.shoppingList = new Models.Custom.ShoppingBasket.ShoppingList();
                AppSession.shoppingList = DataManager.GetShoppingList().Result;
            }

            try
            {
                var search = AppSession.shoppingList.content.recipes.FirstOrDefault(x => x.Id == optionsRecipe.Id);
                if (search != null)
                {
                    addFrame.BackgroundColor = Color.LightGray;
                    addFrame.Content.BackgroundColor = Color.LightGray;
                }
            }
            catch
            {

            }


            StackLayout midAddContent = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 4,
                Children = {
                    addDescLabel,
                    addFrame
                },
            };

            ColourButton confirmBtn = new ColourButton
                (Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CHECKOUT, null);
            confirmBtn.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            confirmBtn.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            confirmBtn.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(confirmBtn.Content, true);
            TouchEffect.SetCommand(confirmBtn.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.HideModalAsync();
                        await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.ShoppingBasket);
                    });
                }));

            StackLayout bottomAddContent = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Children =
                {
                    confirmBtn.Content
                }
            };

            Grid addContent = new Grid
            {
                IsClippedToBounds = true,
                RowSpacing = 10,
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                    { new RowDefinition { Height = 90}},
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                },
                Children = {
                    { topAddContent, 0, 0 },
                    { midAddContent, 0, 1 },
                    { bottomAddContent, 0, 2 }
                },
            };


            ScrollView scrollView = new ScrollView()
            {
                Content = addContent
            };


            innerContainer.Children.Add(scrollView);
        }

        private void SetLabelColours(Choice input)
        {
            switch (input)
            {
                case Choice.Share:
                    shareLabel.Content.TextColor = Color.Orange;
                    addToCollectionLabel.Content.TextColor = Color.White;
                    addToMealPlanLabel.Content.TextColor = Color.White;
                    shoppingBasketLabel.Content.TextColor = Color.White;
                    break;
                case Choice.MealPlan:
                    shareLabel.Content.TextColor = Color.White;
                    addToCollectionLabel.Content.TextColor = Color.White;
                    addToMealPlanLabel.Content.TextColor = Color.Orange;
                    shoppingBasketLabel.Content.TextColor = Color.White;
                    break;
                case Choice.Collection:
                    shareLabel.Content.TextColor = Color.White;
                    addToCollectionLabel.Content.TextColor = Color.Orange;
                    addToMealPlanLabel.Content.TextColor = Color.White;
                    shoppingBasketLabel.Content.TextColor = Color.White;
                    break;
                case Choice.ShoppingBasket:
                    shareLabel.Content.TextColor = Color.White;
                    addToCollectionLabel.Content.TextColor = Color.White;
                    addToMealPlanLabel.Content.TextColor = Color.White;
                    shoppingBasketLabel.Content.TextColor = Color.Orange;
                    break;
                case Choice.Close:
                    shareLabel.Content.TextColor = Color.White;
                    addToCollectionLabel.Content.TextColor = Color.White;
                    addToMealPlanLabel.Content.TextColor = Color.White;
                    shoppingBasketLabel.Content.TextColor = Color.White;
                    break;
            }
        }

        private CollectionView BuildCollectionView()
        {
            recipeOptionsCollectionView = new RecipeOptionsCollectionView();
            collectionsLayout = recipeOptionsCollectionView.GetCollectionView();
            recipeOptionsCollectionView.ShowRecipeOptionsCollection();
            return collectionsLayout;
        }

        private StaticLabel BuildOptionsLabel(string input, Color fontColour, double fontSize, string fontFamily, Command command)
        {
            StaticLabel staticLabel = new StaticLabel(input);
            staticLabel.Content.TextColor = fontColour;
            staticLabel.Content.FontSize = fontSize;
            staticLabel.Content.FontFamily = fontFamily;
            staticLabel.LeftAlign();
            TouchEffect.SetNativeAnimation(staticLabel.Content, true);
            TouchEffect.SetCommand(staticLabel.Content, command);

            return staticLabel;
        }
    }
}
