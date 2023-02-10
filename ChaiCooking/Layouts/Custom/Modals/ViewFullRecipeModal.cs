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
using FFImageLoading.Transformations;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Modals
{
    public class ViewFullRecipeModal : StandardLayout
    {
        RecipePreviewLayout recipePreviewLayout = new RecipePreviewLayout();

        public ViewFullRecipeModal(Recipe recipe, bool reloadParent, bool reloadMealEditor)
        {
            recipePreviewLayout.CheckBasketItemExistsForColour(recipe);

            StaticLabel closeLabel = recipePreviewLayout.CreateCloseLabel(new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (reloadParent)
                    {
                        // reload parent page
                        await App.UpdatePage();
                    }

                    if (reloadMealEditor)
                    {
                        await App.ShowEditMeal(recipe);
                        return;
                    }
                    
                    await App.HideModalAsync();

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
        }

        #region Collections
        // For viewing recipes in collections
        public ViewFullRecipeModal(Recipe recipe, List<Recipe> recipeCollection)
        {
            recipePreviewLayout.CheckBasketItemExistsForColour(recipe);
            recipePreviewLayout.albumRecipe = recipe;
            recipePreviewLayout.collection = recipeCollection;

            StaticLabel closeLabel = recipePreviewLayout.CreateCloseLabel(new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.HideAddRecipe();
                });
            }));
            StaticLabel titleLabel = recipePreviewLayout.CreateTitleLabel(recipePreviewLayout.albumRecipe.Name);
            recipePreviewLayout.recipeImageAlbum = recipePreviewLayout.CreateRecipeImage(recipePreviewLayout.albumRecipe);

            recipePreviewLayout.albumImgcont = new StackLayout
            {
                WidthRequest = 100,
                HeightRequest = 100,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Padding = 5,
                Children =
                {
                    recipePreviewLayout.recipeImageAlbum.Content,
                }
            };

            recipePreviewLayout.topContainerAlbum = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                WidthRequest = Units.ScreenWidth,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    titleLabel.Content,
                }
                // Put Title and Image here
            };

            StackLayout firstSeperator = recipePreviewLayout.CreateSeperator();

            recipePreviewLayout.ingredientsContainerAlbum = recipePreviewLayout.CreateIngredientsContainer(recipePreviewLayout.albumRecipe);

            recipePreviewLayout.methodContainerAlbum = recipePreviewLayout.CreateMethodContainer(recipePreviewLayout.albumRecipe);

            recipePreviewLayout.AddToGesture(recipe);

            IconLabel ShareThis = recipePreviewLayout.CreateShareLabel();

            recipePreviewLayout.bottomGrid = recipePreviewLayout.CreateBottomGrid(null);

            if (Helpers.Pages.CurrentPage != (int)AppSettings.PageNames.ShoppingBasket)
            {
                recipePreviewLayout.bottomGrid.Children.Add(recipePreviewLayout.AddTo.Content, 1, 0);
            }
            recipePreviewLayout.bottomGrid.Children.Add(ShareThis.Content, 2, 0);

            List<View> scrollContViews = new List<View>() { recipePreviewLayout.ingredientsContainerAlbum, recipePreviewLayout.methodContainerAlbum, recipePreviewLayout.bottomGrid };
            ScrollView scrollView = recipePreviewLayout.CreateScrollView(scrollContViews);

            StackLayout leftContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                WidthRequest = Units.HalfScreenWidth,
                Children =
                {
                    new Label
                    {
                        Text = "◄ Previous",
                        HorizontalTextAlignment = TextAlignment.Start,
                        VerticalTextAlignment = TextAlignment.Center,
                        FontSize = Units.FontSizeL,
                        TextColor = Color.DarkGray,
                        FontAttributes = FontAttributes.Bold
                    }
                }
            };

            leftContainer.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                // Change Recipe
                                PreviousRecipe(recipePreviewLayout.albumRecipe, recipePreviewLayout.collection);
                            });
                        })
                    }
                );

            StackLayout rightContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                WidthRequest = Units.HalfScreenWidth,
                Children =
                {
                    new Label
                    {
                        Text = "Next ►",
                        HorizontalTextAlignment = TextAlignment.End,
                        VerticalTextAlignment = TextAlignment.Center,
                        FontSize = Units.FontSizeL,
                        TextColor = Color.DarkGray,
                        FontAttributes = FontAttributes.Bold
                    }
                }
            };

            rightContainer.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                // Change recipe
                                NextRecipe(recipePreviewLayout.albumRecipe, recipePreviewLayout.collection);
                            });
                        })
                    }
                );

            StackLayout navCont = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Start,
                WidthRequest = Units.ScreenWidth,
                Spacing = 10,
                Children =
                {
                    leftContainer,
                    rightContainer
                }
            };

            List<View> masterContViews = new List<View>() { closeLabel.Content, recipePreviewLayout.topContainerAlbum, recipePreviewLayout.albumImgcont };
            StackLayout masterContainer = recipePreviewLayout.CreateMasterContainer(masterContViews);

            if (AppSession.favouritesCollectionView.SelectedItems.Count > 1)
            {
                masterContainer.Children.Add(navCont);

                masterContainer.GestureRecognizers.Add(
                        new SwipeGestureRecognizer()
                        {
                            Command = new Command(() =>
                            {
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    // Change Recipe
                                    PreviousRecipe(recipePreviewLayout.albumRecipe, recipePreviewLayout.collection);
                                });
                            }),
                            Direction = SwipeDirection.Right,
                        }
                    );


                masterContainer.GestureRecognizers.Add(
                        new SwipeGestureRecognizer()
                        {
                            Command = new Command(() =>
                            {
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    // Change Recipe
                                    NextRecipe(recipePreviewLayout.albumRecipe, recipePreviewLayout.collection);
                                });
                            }),
                            Direction = SwipeDirection.Left,
                        }
                    );
            }
            masterContainer.Children.Add(scrollView);

            Frame modalFrame = recipePreviewLayout.CreateModalFrame(masterContainer);

            Content.Children.Add(modalFrame);
            Content.Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING);
        }

        // Collection related
        public void NextRecipe(Recipe recipe, List<Recipe> inputCollection)
        {
            int recipeCount = inputCollection.Count;
            var recipeIndex = inputCollection.IndexOf(recipe);

            if (recipeCount == 1)
            {
                App.ShowAlert("Sellection only contains 1 recipe.");
            }
            else
            {
                if (recipeIndex + 1 == recipeCount)
                {
                    recipeIndex = 0;
                }
                else
                {
                    recipeIndex += 1;
                }

                recipePreviewLayout.albumRecipe = inputCollection[recipeIndex];

                try
                {
                    recipePreviewLayout.recipeImageAlbum = new StaticImage(inputCollection[recipeIndex]
                        .Images[0].Url.AbsoluteUri, 100, 100, null);
                    recipePreviewLayout.recipeImageAlbum.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                    recipePreviewLayout.recipeImageAlbum.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                }
                catch
                {
                    recipePreviewLayout.recipeImageAlbum = new StaticImage("chaismallbag.png", 100, 100, null);
                }

                if (recipe.Creator != null)
                {
                    if (recipe.Creator.Username == AppSession.CurrentUser.Username)
                    {
                        try
                        {
                            recipePreviewLayout.recipeImageAlbum = new StaticImage(recipe.MainImageSource, 128, 128, null);
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }

                recipePreviewLayout.topContainerAlbum.Children.Clear();
                recipePreviewLayout.albumImgcont.Children.Clear();

                Label titleLabelAlbum = new Label
                {
                    Text = inputCollection[recipeIndex].Name,
                    TextColor = Color.Orange,
                    FontAttributes = FontAttributes.Bold
                };

                recipePreviewLayout.topContainerAlbum.Children.Add(titleLabelAlbum);
                recipePreviewLayout.albumImgcont.Children.Add(recipePreviewLayout.recipeImageAlbum.Content);

                recipePreviewLayout.ingredientsContainerAlbum.Children.Clear();

                Label ingredientsTitleLabel = new Label
                {
                    Text = "Ingredients",
                    TextColor = Color.Black,
                    FontAttributes = FontAttributes.Bold
                };

                StackLayout firstSeperator = new StackLayout { WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) };

                recipePreviewLayout.ingredientsContainerAlbum.Children.Add(ingredientsTitleLabel);
                recipePreviewLayout.ingredientsContainerAlbum.Children.Add(firstSeperator);

                if (inputCollection[recipeIndex].Ingredients != null)
                {
                    for (int i = 0; i < inputCollection[recipeIndex].Ingredients.Length; i++)
                    {
                        /*
                        Label ingredientLabel = new Label
                        {
                            Text = StaticData.selectedAlbum.Recipes[recipeIndex].Ingredients[i].Text
                        };

                        ingredientsContainerAlbum.Children.Add(ingredientLabel);*/
                        Label ingredientLabel = new Label
                        {
                        };

                        try
                        {
                            string amountStripped = inputCollection[recipeIndex].Ingredients[i].Amount.Replace(".00", "");
                            ingredientLabel.Text = amountStripped + " " + inputCollection[recipeIndex].Ingredients[i].Unit.Name
                                + " " + inputCollection[recipeIndex].Ingredients[i].Text;
                        }
                        catch (Exception e)
                        {
                            ingredientLabel.Text = inputCollection[recipeIndex].Ingredients[i].Text;

                        }
                        recipePreviewLayout.ingredientsContainerAlbum.Children.Add(ingredientLabel);
                    }
                }
                else
                {
                    Label ingredientLabel = new Label
                    {
                        Text = "Ingredients could not be found."
                    };

                    recipePreviewLayout.ingredientsContainerAlbum.Children.Add(ingredientLabel);
                }

                recipePreviewLayout.methodContainerAlbum.Children.Clear();

                Label methodTitleLabel = new Label
                {
                    Text = "Method",
                    TextColor = Color.Black,
                    FontAttributes = FontAttributes.Bold,
                    Margin = new Thickness(0, Dimensions.GENERAL_COMPONENT_SPACING, 0, 0)
                };

                StackLayout secondSeperator = new StackLayout { WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) };

                recipePreviewLayout.methodContainerAlbum.Children.Add(methodTitleLabel);
                recipePreviewLayout.methodContainerAlbum.Children.Add(secondSeperator);

                if (inputCollection[recipeIndex].chai.Method != null)
                {
                    Label methodTextLabel = new Label
                    {
                        Text = inputCollection[recipeIndex].chai.Method,
                        TextColor = Color.Black,
                    };

                    recipePreviewLayout.methodContainerAlbum.Children.Add(methodTextLabel);
                }
                else
                {
                    Label methodTextLabel = new Label
                    {
                        Text = "Method could not be found.",
                        TextColor = Color.Black,
                    };

                    recipePreviewLayout.methodContainerAlbum.Children.Add(methodTextLabel);
                }
            }
        }

        // Collection related
        public void PreviousRecipe(Recipe recipe, List<Recipe> inputCollection)
        {
            int recipeCount = inputCollection.Count;
            var recipeIndex = inputCollection.IndexOf(recipe);

            if (recipeCount == 1)
            {
                App.ShowAlert("Selection only contains 1 recipe.");
            }
            else
            {
                if (recipeIndex - 1 == -1)
                {
                    recipeIndex = recipeCount - 1;
                }
                else
                {
                    recipeIndex -= 1;
                }

                recipePreviewLayout.albumRecipe = inputCollection[recipeIndex];

                try
                {
                    recipePreviewLayout.recipeImageAlbum = new StaticImage(inputCollection[recipeIndex]
                        .Images[0].Url.AbsoluteUri, 100, 100, null);
                    recipePreviewLayout.recipeImageAlbum.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                    recipePreviewLayout.recipeImageAlbum.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                }
                catch
                {
                    recipePreviewLayout.recipeImageAlbum = new StaticImage("chaismallbag.png", 100, 100, null);
                }

                if (recipe.Creator != null)
                {
                    if (recipe.Creator.Username == AppSession.CurrentUser.Username)
                    {
                        try
                        {
                            recipePreviewLayout.recipeImageAlbum = new StaticImage(recipe.MainImageSource, 128, 128, null);
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }

                recipePreviewLayout.topContainerAlbum.Children.Clear();
                recipePreviewLayout.albumImgcont.Children.Clear();

                Label titleLabelAlbum = new Label
                {
                    Text = inputCollection[recipeIndex].Name,
                    TextColor = Color.Orange,
                    FontAttributes = FontAttributes.Bold
                };

                recipePreviewLayout.topContainerAlbum.Children.Add(titleLabelAlbum);
                recipePreviewLayout.albumImgcont.Children.Add(recipePreviewLayout.recipeImageAlbum.Content);

                recipePreviewLayout.ingredientsContainerAlbum.Children.Clear();

                Label ingredientsTitleLabel = new Label
                {
                    Text = "Ingredients",
                    TextColor = Color.Black,
                    FontAttributes = FontAttributes.Bold
                };

                StackLayout firstSeperator = new StackLayout { WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) };

                recipePreviewLayout.ingredientsContainerAlbum.Children.Add(ingredientsTitleLabel);
                recipePreviewLayout.ingredientsContainerAlbum.Children.Add(firstSeperator);

                if (inputCollection[recipeIndex].Ingredients != null)
                {
                    for (int i = 0; i < inputCollection[recipeIndex].Ingredients.Length; i++)
                    {
                        /*
                        Label ingredientLabel = new Label
                        {
                            Text = StaticData.selectedAlbum.Recipes[recipeIndex].Ingredients[i].Text
                        };

                        ingredientsContainerAlbum.Children.Add(ingredientLabel);*/
                        Label ingredientLabel = new Label
                        {
                        };

                        try
                        {
                            string amountStripped = inputCollection[recipeIndex].Ingredients[i].Amount.Replace(".00", "");
                            ingredientLabel.Text = amountStripped + " " + inputCollection[recipeIndex].Ingredients[i].Unit.Name
                                + " " + inputCollection[recipeIndex].Ingredients[i].Text;
                        }
                        catch (Exception e)
                        {
                            ingredientLabel.Text = inputCollection[recipeIndex].Ingredients[i].Text;

                        }
                        recipePreviewLayout.ingredientsContainerAlbum.Children.Add(ingredientLabel);
                    }
                }
                else
                {
                    Label ingredientLabel = new Label
                    {
                        Text = "Ingredients could not be found."
                    };

                    recipePreviewLayout.ingredientsContainerAlbum.Children.Add(ingredientLabel);
                }

                recipePreviewLayout.methodContainerAlbum.Children.Clear();

                Label methodTitleLabel = new Label
                {
                    Text = "Method",
                    TextColor = Color.Black,
                    FontAttributes = FontAttributes.Bold,
                    Margin = new Thickness(0, Dimensions.GENERAL_COMPONENT_SPACING, 0, 0)
                };

                StackLayout secondSeperator = new StackLayout { WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) };

                recipePreviewLayout.methodContainerAlbum.Children.Add(methodTitleLabel);
                recipePreviewLayout.methodContainerAlbum.Children.Add(secondSeperator);

                if (inputCollection[recipeIndex].chai.Method != null)
                {
                    Label methodTextLabel = new Label
                    {
                        Text = inputCollection[recipeIndex].chai.Method,
                        TextColor = Color.Black,
                    };

                    recipePreviewLayout.methodContainerAlbum.Children.Add(methodTextLabel);
                }
                else
                {
                    Label methodTextLabel = new Label
                    {
                        Text = "Method could not be found.",
                        TextColor = Color.Black,
                    };

                    recipePreviewLayout.methodContainerAlbum.Children.Add(methodTextLabel);
                }
            }
        }
        #endregion

        public ViewFullRecipeModal(Recipe recipe, Action updateHeart, string specialConstructor)
        {
            recipePreviewLayout.CheckBasketItemExistsForColour(recipe);
            StaticLabel closeLabel = recipePreviewLayout.CreateCloseLabel(new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.HideRecipeSummary();
                    try
                    {
                        var s = AppSession.UserCollectionRecipes.Find(x => x.Id == recipe.Id);
                        if (s != null && s.Id == recipe.Id)
                        {
                            // Already exists update the heart
                            updateHeart();
                        }
                        else
                        {
                            // Do nothing
                        }
                    }
                    catch { }
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
        }
    }
}
