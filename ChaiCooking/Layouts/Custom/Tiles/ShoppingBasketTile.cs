using System;
using System.Collections.Generic;
using ChaiCooking.Components;
using ChaiCooking.Components.Images;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom.Panels.Recipe;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using ChaiCooking.Tools;
using FFImageLoading.Transformations;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Tiles
{
    public class ShoppingBasketTile : ActiveComponent
    {
        int topContainerWidth = Units.ScreenWidth - Units.ScreenWidth10Percent;
        int bottomContainerWidth = Units.ScreenWidth - Units.ScreenWidth10Percent;

        Recipe recipe { get; set; }
        StaticImage recipeImage, deleteImage, heartIcon;
        StackLayout heartContainer, masterContainer;
        Label titleLbl;
        Frame frame;

        public ShoppingBasketTile()
        {
        }

        public ShoppingBasketTile(Recipe _recipe)
        {
            recipe = _recipe;

            Container = new Grid { }; Content = new Grid { };

            #region - Recipe Image
            recipeImage = new StaticImage("chaismallbag.png", 100, 100, null);
            if (recipe.MainImageSource != null)
            {
                try
                {
                    recipeImage = new StaticImage(recipe.MainImageSource, 100, 100, null);
                }
                catch
                {
                    recipeImage = new StaticImage("chaismallbag.png", 100, 100, null);
                }
            }
            else
            {
                try
                {
                    recipeImage = new StaticImage(recipe.Images[0].Url.ToString(), 100, 100, null);
                }
                catch (Exception e)
                {
                    recipeImage = new StaticImage("chaismallbag.png", 100, 100, null);
                }
            }

            StackLayout imageContainer = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    recipeImage.Content
                }
            };

            #endregion

            #region - Heart Icon

            heartContainer = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            if (AppSession.UserCollectionRecipes != null)
            {
                var s = AppSession.UserCollectionRecipes.Find(x => x.Id == recipe.Id);
                if (s != null && s.Id == recipe.Id)
                {
                    Color tintColour = Color.Red;

                    TintTransformation colorTint = new TintTransformation
                    {
                        HexColor = (string)tintColour.ToHex(),
                        EnableSolidColor = true
                    };

                    heartIcon = new StaticImage("hearticonsmall.png", 25, 25, null);
                    heartIcon.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
                    heartIcon.Content.Transformations.Add(colorTint);

                    heartContainer.Children.Add(heartIcon.Content);

                    TouchEffect.SetNativeAnimation(heartIcon.Content, true);
                    TouchEffect.SetCommand(heartIcon.Content,
                    new Command(() =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            App.ShowAlert($"{recipe.Name} has already been added to your favourites.");
                        });
                    }));
                }
                else
                {
                    heartIcon = new StaticImage("faveblack.png", 25, 25, null);

                    heartContainer.Children.Add(heartIcon.Content);
                    TouchEffect.SetNativeAnimation(heartIcon.Content, true);
                    TouchEffect.SetCommand(heartIcon.Content,
                    new Command(() =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            
                                favouriteActivate(recipe);
                           
                        });
                    }));
                }
            }
            else
            {
                heartIcon = new StaticImage("faveblack.png", 25, 25, null);

                heartContainer.Children.Add(heartIcon.Content);
                TouchEffect.SetNativeAnimation(heartIcon.Content, true);
                TouchEffect.SetCommand(heartIcon.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        favouriteActivate(recipe);
                    });
                }));
            }

            StackLayout iconStack = new StackLayout // stack em high
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Spacing = 5,
                Children =
                {
                    heartContainer,
                },
            };

            #endregion

            #region - Delete Icon

            deleteImage = new StaticImage("trash.png", 24, 24, null);
            deleteImage.Content.HorizontalOptions = LayoutOptions.EndAndExpand;
            deleteImage.Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            deleteImage.Content.Margin = 8;

            TouchEffect.SetNativeAnimation(deleteImage.Content, true);
            TouchEffect.SetCommand(deleteImage.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    AppSession.SelectedRecipe = recipe;
                    await App.ShowRemoveFromBasketModal(recipe);
                });
            }));
            #endregion

            #region - Top Container
            StackLayout topContainer = new StackLayout
            {
                BackgroundColor = Color.White,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Spacing = 5,
                WidthRequest = topContainerWidth,
                Children =
                {

                    //mainDetailStack,
                    //subDetailStack,
                    new RecipeDetail().GetContent(),
                    new RecipeSubDetail(recipe, recipe.DishType).GetContent(),
                    imageContainer,
                    iconStack
                }
            };

            topContainer.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        AppSession.SelectedRecipe = recipe;
                        Recipe newRecipe = await DataManager.GetRecipe(recipe.Id);
                        if (newRecipe != null)
                        {
                            await App.ShowFullRecipe(newRecipe, UpdateHeart, "");
                        }
                        else
                        {
                            App.ShowAlert("An error occured");
                        }
                    });
                })
            });
            #endregion

            #region - Bottom Container

            titleLbl = new Label
            {
                TextColor = Color.White,
                Text = TextTools.FirstCharToUpper(recipe.Name),
                FontAttributes = FontAttributes.Bold,
                FontSize = Units.FontSizeML,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.TailTruncation
            };


            StackLayout titleStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                Spacing = 0,
                WidthRequest = Units.ScreenWidth40Percent,
                Children =
                {
                    titleLbl
                }
            };

            
            ActiveImage infoImage = new ActiveImage("menu_circle.png", Dimensions.CHECKBOX_ICON_SIZE, Dimensions.CHECKBOX_ICON_SIZE, null, null);// new Models.Action((int)ChaiCooking.Helpers.Actions.ActionName.ShowRecipeOptionsPanel));

            StackLayout infoBtn = new StackLayout
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                Spacing = 0,
                WidthRequest = Units.ScreenWidth10Percent,
                Children =
                {
                    infoImage.Content
                }
            };
            TouchEffect.SetNativeAnimation(infoImage.Content, true);
            TouchEffect.SetCommand(infoImage.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.ShowRecipeOptionsModal(recipe);
                });
            }));

            StackLayout bottomContainer = new StackLayout
            {
                BackgroundColor = Color.Black,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                WidthRequest = bottomContainerWidth,
                Children =
                {
                    titleStack,
                    //createdByStack,
                    new RecipeCreatedBy(recipe).GetContent(),
                }
            };

            if (AppSettings.RecipeOptionsEnabled)
            {
                bottomContainer.Children.Add(infoBtn);
            }

            bottomContainer.Children.Add(deleteImage.Content);

            #endregion

            #region - Master Container

            masterContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.White,
                Spacing = 0,
                Children =
                {
                    topContainer,
                    bottomContainer
                }
            };

            frame = new Frame
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Content = masterContainer,
                CornerRadius = 5,
                IsClippedToBounds = true,
                Padding = 0,
            };

            Content.Children.Add(frame);

            #endregion
        }

        public string EmptyCheck(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "0";// mins";
            }
            else
            {
                return s;// + " mins";
            }
        }

        public string EmptyCreator(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "-";
            }
            else
            {
                return s;
            }
        }

        public async void favouriteActivate(Recipe recipe)
        {
    
                if (AppSession.UserCollectionRecipes == null)
                {
                    App.ShowAlert("Failed to add to your favourites. Favourite not found!");
                }
                else
                {
                    if (AppSession.UserCollectionRecipes.Count < 50)
                    {
                        var result = await App.ApiBridge.AddFavourite(AppSession.CurrentUser, recipe.Id);
                        if (result)
                        {
                            Color tintColour = Color.Red;

                            TintTransformation colorTint = new TintTransformation
                            {
                                HexColor = (string)tintColour.ToHex(),
                                EnableSolidColor = true

                            };
                            var s = AppSession.UserCollectionRecipes.Find(x => x.Id == recipe.Id);
                            if (s != null && s.Id == recipe.Id)
                            {
                                App.ShowAlert($"{recipe.Name} has already been added to your favourites.");
                            }
                            else
                            {
                                App.ShowAlert($"{recipe.Name} added to your favourites.");
                                AppSession.UserCollectionRecipes.Add(recipe);
                                heartContainer.Children.Remove(heartIcon.Content);
                                heartIcon = new StaticImage("hearticonsmall.png", 24, 24, null);
                                heartIcon.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
                                heartIcon.Content.Transformations.Add(colorTint);
                                heartContainer.Children.Add(heartIcon.Content);
                            }
                        }
                        else
                        {
                            App.ShowAlert($"Failed to add {recipe.Name} to favourites.");
                        }
                    }
                    else
                    {
                        App.ShowAlert("Failed to add to your favourites. Favourite limit (50) has been reached.");
                    }
                
            }
        }

        public void UpdateHeart()
        {
            Color tintColour = Color.Red;

            TintTransformation colorTint = new TintTransformation
            {
                HexColor = (string)tintColour.ToHex(),
                EnableSolidColor = true
            };
            heartContainer.Children.Remove(heartIcon.Content);
            heartIcon = new StaticImage("hearticonsmall.png", 24, 24, null);
            heartIcon.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
            heartIcon.Content.Transformations.Add(colorTint);
            heartContainer.Children.Add(heartIcon.Content);
        }
    }
}
