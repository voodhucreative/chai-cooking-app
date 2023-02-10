
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Components;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom.Panels.Recipe;
using ChaiCooking.Models.Custom;
using ChaiCooking.Tools;
using FFImageLoading.Transformations;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using XFShapeView;

namespace ChaiCooking.Layouts.Custom.Tiles
{
    public class WideRecipeTile : ActiveComponent
    {

        // New Recipe Tile
        int topContainerWidth = Units.ScreenWidth - Units.ScreenWidth10Percent;
        int bottomContainerWidth = Units.ScreenWidth - Units.ScreenWidth10Percent;

        Frame frame;

        public StackLayout masterContainer, heartContainer;
        public StaticImage heartIcon { get; set; }

        public Label titleLbl;
        public Label totalSubDetail;
        public Label prepSubDetail;

        public StaticImage recipeImage;

        //TODO do this correctly so tap gestures do not overlap.
        //quick and dirty tap blocker
        bool heartTapped;

        public WideRecipeTile(Recipe recipe)
        {
            Container = new Grid { }; Content = new Grid { };

            StaticImage starIcon = new StaticImage("star.png", 25, 25, null);

            StackLayout starContainer = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    starIcon.Content
                }
            };

            starContainer.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        Console.WriteLine("Favourite Icon has been pressed.");
                    });
                })
            });

            StaticImage typeIcon = new StaticImage("pizza.png", 25, 25, null);

            try
            {
                typeIcon = new StaticImage(recipe.MealCategoryImageSource, 20, 20, null);
            }
            catch (Exception e) { }

            StackLayout typeContainer = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    typeIcon.Content
                }
            };

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
                           
                                heartTapped = true;
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
                            heartTapped = true;
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
                        heartTapped = true;
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
                    //starContainer,
                    typeContainer,
                },
            };

            recipeImage = new StaticImage("chaismallbag.png", 100, 100, null);

            try
            {
                recipeImage = new StaticImage(recipe.MainImageSource, 100, 100, null);
            }
            catch (Exception e)
            {

            }

            try
            {
                recipeImage = new StaticImage(recipe.Images[0].Url.ToString(), 100, 100, null);
            }
            catch (Exception e)
            {

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
                        if (AppSession.InfoModeOn)
                        {
                            App.ShowInfoBubble(new Paragraph("Recipe Summary", "These give you an overview of a recipe. Tap in the middle of the Recipe Summary to view the full recipe. They also have an options button where you’ll find the following functions:\n\nShare - share the recipe with friends via Facebook\nAdd to Meal Plan(paid feature) - add the recipe/s to a Meal Plan\nAdd to Collection(album) - add the recipe/s to a Collection(album)\nQuick shop - add the recipe/s to a trolley for convenient shopping.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                        }
                        else
                        {
                            //Small delay to ensure heart is updated first.
                            await Task.Delay(10);
                            if (!heartTapped)
                            {
                                AppSession.SelectedRecipe = recipe;
                                await App.ShowFullRecipe(recipe, UpdateHeart, "");
                            }
                            else
                            {
                                heartTapped = false;
                            }
                        }
                    });
                })
            });

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
            //infoBtn.GestureRecognizers.Add(new TapGestureRecognizer()
            //{
            //    Command = new Command(() =>
            //    {
            //        Device.BeginInvokeOnMainThread(async () =>
            //        {
            //            //App.ShowAlert("Coming Soon.");
            //            await App.ShowRecipeOptionsModal(recipe);
            //        });
            //    })
            //});

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
                    new RecipeCreatedBy(recipe).GetContent(),
                    //createdByStack,
                }
            };

            if (AppSettings.RecipeOptionsEnabled)
            {
                bottomContainer.Children.Add(infoBtn);
            }

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
        }

        // Ends here

        public string IconUncheckedImageSource { get; set; }
        public string IconCheckedImageSource { get; set; }

        bool IsSelected { get; set; }


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
            if (AppSession.InfoModeOn)
            {
                double x = Tools.Screen.GetScreenCoordinates(masterContainer).X;
                double y = Tools.Screen.GetScreenCoordinates(masterContainer).Y;
                //App.ShowInfoBubble(new Label { Text = "Tap ‘Favourite’ to store the recipe for later. You’ll find it again in the ‘Favourites’ section of Your Menu under Collections." }, (int)x, (int)y);
                App.ShowInfoBubble(new Paragraph("Favourite", "Tap ‘Favourite’ to store the recipe for later. You’ll find it again in the ‘Favourites’ section of Your Menu under Collections.", null).Content, (int)Units.HalfScreenWidth, (int)Units.HalfScreenHeight);

            }
            else
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

        //public void Toggle()
        //{
        //    IsSelected = !IsSelected;

        //    if (IsSelected)
        //    {
        //        //BackgroundSquare.Opacity = 1;
        //        Icon.Content.Source = IconCheckedImageSource;
        //    }
        //    else
        //    {
        //        //BackgroundSquare.Opacity = 0;
        //        Icon.Content.Source = IconUncheckedImageSource;
        //    }
        //}
    }
}