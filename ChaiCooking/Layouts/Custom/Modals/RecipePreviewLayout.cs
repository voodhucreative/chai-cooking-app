using System;
using System.Collections.Generic;
using System.Linq;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using ChaiCooking.Views.CollectionViews.ShoppingBasket;
using FFImageLoading.Transformations;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Modals
{
    public class RecipePreviewLayout
    {
        // View Recipe Variables
        public StaticImage favouriteHeart { get; set; }
        public Grid favGrid, bottomGrid;
        public IconLabel AddTo;
        public StaticImage recipeImageAlbum;
        public StackLayout topContainerAlbum, albumImgcont, ingredientsContainerAlbum, methodContainerAlbum;
        public Recipe albumRecipe;
        public List<Recipe> collection;
        public Color closeLabelColour = Color.Black;
        public Color titleLabelColour = Color.Orange;

        public async void CheckBasketItemExists(Recipe recipe)
        {
            try
            {
                ShoppingBasketNullCheck();
                var search = AppSession.shoppingList.content.recipes.FirstOrDefault(x => x.Id == recipe.Id);
                if (search != null)
                {
                    //App.ShowAlert("Recipe already exists.");
                }
                else
                {
                    var result = await DataManager.AddRecipeToShoppingList(recipe);
                    if (result)
                    {
                        App.ShowAlert($"Successfully added {recipe.Name}.");
                        AppSession.shoppingList.content.recipes.Add(recipe);
                        bottomGrid.Children.Remove(AddTo.Content);
                        AddTo = GetIconLabel("cartinactive.png");
                        bottomGrid.Children.Add(AddTo.Content, 1, 0);
                        var shoppingBasketGroup = new ShoppingBasketViewSection(AppSession.shoppingList.content.recipes);//, StaticData.BuildEmpty());
                        AppSession.shoppingBasketCollection.Add(shoppingBasketGroup);
                        if (AppSession.shoppingBasketCollection.Count > 1)
                        {
                            AppSession.shoppingBasketCollection.RemoveAt(0);
                        }
                    }
                    else
                    {
                        App.ShowAlert($"Failed to add {recipe.Name}.");
                    }
                }
                search = null;
            }
            catch
            {
                App.ShowAlert($"Failed to add {recipe.Name}.");
            }
        }

        public IconLabel GetIconLabel(string image)
        {
            IconLabel iconLabel = new IconLabel(image, "Add to", 100, 24, null);
            return iconLabel;
        }

        public void CheckBasketItemExistsForColour(Recipe recipe)
        {
            try
            {
                ShoppingBasketNullCheck();
                var search = AppSession.shoppingList.content.recipes.FirstOrDefault(x => x.Id == recipe.Id);
                if (search != null)
                {
                    //App.ShowAlert("Recipe already exists.");
                    AddTo = GetIconLabel("cartinactive.png");
                }
                else
                {
                    AddTo = GetIconLabel("cartblack.png");
                }
                search = null;
            }
            catch
            {
            }
        }

        public StaticLabel CreateCloseLabel(Command command)
        {
            StaticLabel closeLabel = new StaticLabel(AppText.CLOSE);
            closeLabel.Content.FontFamily = Fonts.GetRegularAppFont();
            closeLabel.Content.TextColor = closeLabelColour;
            closeLabel.Content.FontSize = Units.FontSizeL;
            closeLabel.RightAlign();
            TouchEffect.SetNativeAnimation(closeLabel.Content, true);
            TouchEffect.SetCommand(closeLabel.Content, command);
            return closeLabel;
        }

        public StaticLabel CreateTitleLabel(string recipeName)
        {
            StaticLabel titleLabel = new StaticLabel(recipeName);
            titleLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            titleLabel.Content.TextColor = titleLabelColour;
            titleLabel.Content.FontSize = Units.FontSizeL;
            titleLabel.LeftAlign();
            return titleLabel;
        }

        public StaticImage CreateRecipeImage(Recipe recipe)
        {
            StaticImage recipeImage = null;
            try
            {
                recipeImage = new StaticImage(recipe.Images[0].Url.AbsoluteUri, 100, 100, null);
                recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
            }
            catch
            {
                recipeImage = new StaticImage("chaismallbag.png", 100, 100, null);
            }

            if (recipe.Creator != null)
            {
                if (recipe.Creator.Username == AppSession.CurrentUser.Username)
                {
                    try
                    {
                        recipeImage = new StaticImage(recipe.MainImageSource, 128, 128, null);
                    }
                    catch (Exception e)
                    {
                        recipeImage = new StaticImage("chaismallbag.png", 100, 100, null);
                    }
                }
            }
            return recipeImage;
        }

        public StackLayout CreateRecipeImageContainer(Recipe recipe)
        {
            StackLayout imageContainer = new StackLayout
            {
                WidthRequest = 100,
                HeightRequest = 100,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                Padding = 5,
                Children =
                {
                  CreateRecipeImage(recipe).Content,
                }
            };

            return imageContainer;
        }

        public StackLayout CreateSeperator()
        {
            StackLayout stackLayout = new StackLayout
            {
                WidthRequest = Units.ScreenWidth,
                HeightRequest = 1,
                BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY)
            };
            return stackLayout;
        }

        public StackLayout CreateIngredientsContainer(Recipe recipe)
        {
            StaticLabel titleLabel = new StaticLabel("Ingredients");
            titleLabel.Content.TextColor = Color.Black;
            titleLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            titleLabel.Content.FontSize = Units.FontSizeM;

            StackLayout seperator = CreateSeperator();

            StackLayout ingredientsContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    titleLabel.Content,
                    seperator,
                }
                // Put ingredients here
            };

            if (recipe.Ingredients != null)
            {
                for (int i = 0; i < recipe.Ingredients.Length; i++)
                {
                    Label ingredientLabel = new Label
                    {
                        Text = recipe.Ingredients[i].Text.Replace(".00", ""),
                        TextColor = Color.Black,
                    };

                    try
                    {
                        string amountStripped = recipe.Ingredients[i].Amount.Replace(".00", "");
                        ingredientLabel.Text = amountStripped + " " + recipe.Ingredients[i].Unit.Name + " " + recipe.Ingredients[i].Text;
                    }
                    catch (Exception e)
                    {

                    }
                    ingredientsContainer.Children.Add(ingredientLabel);
                }
            }
            else
            {
                Label ingredientLabel = new Label
                {
                    Text = "Ingredients could not be found.",
                    TextColor = Color.Black
                };

                ingredientsContainer.Children.Add(ingredientLabel);
            }

            return ingredientsContainer;
        }

        public StackLayout CreateMethodContainer(Recipe recipe)
        {
            StaticLabel titleLabel = new StaticLabel("Method");
            titleLabel.Content.TextColor = Color.Black;
            titleLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            titleLabel.Content.FontSize = Units.FontSizeM;
            titleLabel.Content.Margin = new Thickness(0, Dimensions.GENERAL_COMPONENT_SPACING, 0, 0);

            StackLayout seperator = CreateSeperator();

            StackLayout methodContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    titleLabel.Content,
                    seperator,
                }
                // Put method / instructions? here
            };

            if (recipe.chai != null && recipe.chai.Method != null)
            {
                Label methodTextLabel = new Label
                {
                    Text = recipe.chai.Method,
                    TextColor = Color.Black,
                };

                methodContainer.Children.Add(methodTextLabel);
            }
            else if (recipe.Instructions != null)
            {
                foreach (Ingredient steps in recipe.Instructions.Steps)
                {
                    methodContainer.Children.Add(new Label
                    {
                        Text = steps.Text,
                        TextColor = Color.Black,
                    });
                }
            }
            else
            {
                Label methodTextLabel = new Label
                {
                    Text = "Method could not be found.",
                    TextColor = Color.Black,
                };

                methodContainer.Children.Add(methodTextLabel);
            }

            return methodContainer;
        }

        public void CreateFavouriteHeart(Recipe recipe)
        {
            try
            {
                GetFavs();
                if (AppSession.UserCollectionRecipes != null)
                {
                    var exists = AppSession.UserCollectionRecipes.Contains(recipe);
                    if (exists)
                    {
                        Color tintColour = Color.Black;

                        TintTransformation colorTint = new TintTransformation
                        {
                            HexColor = (string)tintColour.ToHex(),
                            EnableSolidColor = true

                        };
                        favouriteHeart = new StaticImage("hearticonsmall.png", 24, 24, null);
                        favouriteHeart.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
                        favouriteHeart.Content.Transformations.Add(colorTint);

                        favouriteHeart.Content.GestureRecognizers.Add(
                        new TapGestureRecognizer()
                        {
                            Command = new Command(() =>
                            {
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    App.ShowAlert($"{recipe.Name} has already been added to your favourites.");
                                });
                            })

                        });
                    }
                    else
                    {
                        favouriteHeart = new StaticImage("faveblack.png", 24, 24, null);
                        favouriteHeart.Content.GestureRecognizers.Add(
                        new TapGestureRecognizer()
                        {
                            Command = new Command(() =>
                            {
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    if (AppSession.InfoModeOn)
                                    {
                                        double x = Tools.Screen.GetScreenCoordinates(favouriteHeart.Content).X;
                                        double y = Tools.Screen.GetScreenCoordinates(favouriteHeart.Content).Y;
                                        //App.ShowInfoBubble(new Label { Text = " Tap ‘Favourite’ to store the recipe for later. You’ll find it again in the ‘Favourites’ section of Your Menu under Collections." }, (int)x + 240, (int)y);
                                        App.ShowInfoBubble(new Paragraph("Favourite", "Tap ‘Favourite’ to store the recipe for later. You’ll find it again in the ‘Favourites’ section of Your Menu under Collections.", null).Content, (int)Units.HalfScreenWidth, (int)Units.HalfScreenHeight);


                                    }
                                    else
                                    {
                                        if (AppSession.UserCollectionRecipes.Count < 50)
                                        {
                                            var result = await App.ApiBridge.AddFavourite(AppSession.CurrentUser, recipe.Id);
                                            if (result)
                                            {
                                                App.ShowAlert(recipe.Name + " added to your favourites.");
                                                Console.WriteLine("Fave Added");
                                                Color tintColour = Color.Black;

                                                TintTransformation colorTint = new TintTransformation
                                                {
                                                    HexColor = (string)tintColour.ToHex(),
                                                    EnableSolidColor = true

                                                };
                                                AppSession.UserCollectionRecipes.Add(recipe);
                                                favGrid.Children.Remove(favouriteHeart.Content);
                                                favouriteHeart = new StaticImage("hearticonsmall.png", 24, 24, null);
                                                favouriteHeart.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
                                                favouriteHeart.Content.Transformations.Add(colorTint);
                                                favGrid.Children.Add(favouriteHeart.Content, 0, 0);
                                            }
                                            else
                                            {
                                                App.ShowAlert($"Failed to add {recipe.Name} to your favourites.");
                                                await App.HideRecipeSummary();
                                            }
                                        }
                                        else
                                        {
                                            App.ShowAlert("Failed to add to your favourites. Favourite limit (50) has been reached.");
                                        }
                                    }
                                });
                            })
                        }
                    );
                    }
                }
                else
                {
                    favouriteHeart = new StaticImage("faveblack.png", 24, 24, null);
                    favouriteHeart.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                if (AppSession.InfoModeOn)
                                {
                                    double x = Tools.Screen.GetScreenCoordinates(favouriteHeart.Content).X;
                                    double y = Tools.Screen.GetScreenCoordinates(favouriteHeart.Content).Y;
                                    // App.ShowInfoBubble(new Label { Text = " Tap ‘Favourite’ to store the recipe for later. You’ll find it again in the ‘Favourites’ section of Your Menu under Collections." }, (int)x + 240, (int)y);
                                    App.ShowInfoBubble(new Paragraph("Favourite", "Tap ‘Favourite’ to store the recipe for later. You’ll find it again in the ‘Favourites’ section of Your Menu under Collections.", null).Content, (int)Units.HalfScreenWidth, (int)Units.HalfScreenHeight);

                                }
                                else
                                {
                                    if (AppSession.UserCollectionRecipes.Count < 50)
                                    {
                                        var result = await App.ApiBridge.AddFavourite(AppSession.CurrentUser, recipe.Id);
                                        if (result)
                                        {
                                            App.ShowAlert(recipe.Name + " added to your favourites.");
                                            Console.WriteLine("Fave Added");
                                            Color tintColour = Color.Black;

                                            TintTransformation colorTint = new TintTransformation
                                            {
                                                HexColor = (string)tintColour.ToHex(),
                                                EnableSolidColor = true

                                            };
                                            AppSession.UserCollectionRecipes.Add(recipe);
                                            favGrid.Children.Remove(favouriteHeart.Content);
                                            favouriteHeart = new StaticImage("hearticonsmall.png", 24, 24, null);
                                            favouriteHeart.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
                                            favouriteHeart.Content.Transformations.Add(colorTint);
                                            favGrid.Children.Add(favouriteHeart.Content, 0, 0);
                                        }
                                        else
                                        {
                                            App.ShowAlert($"Failed to add {recipe.Name} to your favourites.");
                                            await App.HideRecipeSummary();
                                        }
                                    }
                                    else
                                    {
                                        App.ShowAlert("Failed to add to your favourites. Favourite limit (50) has been reached.");
                                    }
                                }
                            });
                        })
                    }
                );
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public async void GetFavs()
        {
            if (AppSession.UserCollectionRecipes == null)
            {
                AppSession.UserCollectionRecipes = await App.ApiBridge.GetFavouriteRecipes(AppSession.CurrentUser);
            }
        }

        public StaticLabel CreateFavouriteLabel()
        {
            StaticLabel favLabel = new StaticLabel(AppText.FAVOURITE);
            favLabel.Content.HorizontalTextAlignment = TextAlignment.Start;
            favLabel.Content.Margin = new Thickness(4, 0, 8, 0);
            return favLabel;
        }

        public IconLabel CreateShareLabel()
        {
            IconLabel shareThis = new IconLabel("facebookblack.png", "Share this", 120, 24);

            shareThis.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await Share.RequestAsync(new ShareTextRequest
                                {
                                    Uri = "https://chai.cooking/",
                                    Title = "Chai Cooking"
                                });
                            });
                        })
                    }
                );

            return shareThis;
        }

        public Frame CreateModalFrame(View view)
        {
            Frame modalFrame = new Frame
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Content = view,
                CornerRadius = 5,
                IsClippedToBounds = true,
                Padding = 0,
            };
            return modalFrame;
        }

        public void AddToGesture(Recipe recipe)
        {
            AddTo.Content.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        CheckBasketItemExists(recipe);
                    });
                })
            });
        }

        public Grid CreateFavouriteGrid(List<View> views)
        {
            int i = 0;
            Grid grid = new Grid
            {
                RowSpacing = 0,
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) } },
                },
                ColumnDefinitions =
                {
                    { new ColumnDefinition { Width = new GridLength(25) } },
                    { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) } },
                },
            };

            foreach (View v in views)
            {
                grid.Children.Add(v, i, 0);
                i++;
            }

            return grid;
        }

        public StackLayout CreateMasterContainer(List<View> views)
        {
            StackLayout masterContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.White,
                WidthRequest = Units.ScreenWidth,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Spacing = 0,
                Padding = Dimensions.RECIPE_LAYOUT_PADDING,
            };

            foreach (View v in views)
            {
                masterContainer.Children.Add(v);
            }

            return masterContainer;
        }

        public Grid CreateBottomGrid(View view)
        {
            bottomGrid = new Grid
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 24, 0, 24),
                Padding = 0,
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) } },
                },
                ColumnDefinitions =
                {
                    { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) } },
                    { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) } },
                    { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) } },
                },
            };

            if (view != null)
            {
                bottomGrid.Children.Add(view, 0, 0);
            }

            return bottomGrid;
        }

        public ScrollView CreateScrollView(List<View> views)
        {
            int i = 0;
            StackLayout scrollContainer = new StackLayout
            {
            };

            foreach (View v in views)
            {
                scrollContainer.Children.Add(v);
                i++;
            }

            ScrollView scrollView = new ScrollView
            {
                Content = scrollContainer
            };

            return scrollView;
        }

        public void ShoppingBasketNullCheck()
        {
            if (AppSession.shoppingList == null)
            {
                AppSession.shoppingList = new Models.Custom.ShoppingBasket.ShoppingList();
                AppSession.shoppingList.content = new Models.Custom.ShoppingBasket.ShoppingList.Content();
                AppSession.shoppingList.content.recipes = new List<Recipe>();
            }
        }
    }
}
