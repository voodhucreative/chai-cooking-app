
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom.Panels.Recipe;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using ChaiCooking.Tools;
using ChaiCooking.Views.CollectionViews.RecipeEditor;
using FFImageLoading.Transformations;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using XFShapeView;

namespace ChaiCooking.Layouts.Custom.Tiles
{
    public class UserRecipeTile : ActiveComponent
    {

        // New Recipe Tile
        int topContainerWidth = Units.ScreenWidth - Units.ScreenWidth10Percent;
        int bottomContainerWidth = Units.ScreenWidth - Units.ScreenWidth10Percent;

        Frame frame;

        public StackLayout masterContainer, heartContainer;
        //public StaticImage heartIcon { get; set; }

        public Label titleLbl;
        public Label totalSubDetail;
        public Label prepSubDetail;
        public Label recipeStatusLabel;

        public Label dishSubDetail;
        public Label createdTitleLbl;

        public StaticImage recipeImage;
        public StaticImage editImage;
        public StaticImage optionImage;
        public StaticImage cornerImage;
        public StaticImage eggtimerImage;

        public Recipe Recipe;

        public Grid StatusGrid;

        int detailFontSize;

        public UserRecipeTile(Recipe _recipe)
        {
            Recipe = _recipe;
            Container = new Grid { }; Content = new Grid { };

            recipeImage = new StaticImage("chaismallbag.png", 100, 100, null);
            editImage = new StaticImage("pencil.png", 24, 24, null);
            optionImage = new StaticImage("trash.png", 24, 24, null);
            eggtimerImage = new StaticImage("eggtimer.png", 16, 16, null);

            optionImage.Content.HorizontalOptions = LayoutOptions.EndAndExpand;
            optionImage.Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            optionImage.Content.Margin = 8;



            eggtimerImage.Content.HorizontalOptions = LayoutOptions.StartAndExpand;
            eggtimerImage.Content.VerticalOptions = LayoutOptions.StartAndExpand;
            eggtimerImage.Content.Margin = 8;
            eggtimerImage.Content.Opacity = 0.75;

            cornerImage = new StaticImage("white_corner.png", 54, 54, null);
            cornerImage.Content.HorizontalOptions = LayoutOptions.StartAndExpand;
            cornerImage.Content.VerticalOptions = LayoutOptions.StartAndExpand;

            editImage.Content.HorizontalOptions = LayoutOptions.EndAndExpand;
            editImage.Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            editImage.Content.Margin = 2;

            detailFontSize = Units.FontSizeL;

            if (App.IsSmallScreen())
            {
                detailFontSize = Units.FontSizeM;
            }

            try
            {
                recipeImage = new StaticImage(Recipe.MainImageSource, 100, 100, null);
            }
            catch (Exception e)
            {

            }

            try
            {
                recipeImage = new StaticImage(Recipe.Images[0].Url.ToString(), 100, 100, null);
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

            totalSubDetail = new Label
            {
                TextColor = Color.Gray,
                Text = Recipe.CookingTime,//EmptyCheck(Recipe.CookingTime),
                FontSize = detailFontSize,
                FontAttributes = FontAttributes.Bold,
                LineBreakMode = LineBreakMode.WordWrap
            };

            prepSubDetail = new Label
            {
                TextColor = Color.Gray,
                Text = EmptyCheck(Recipe.PrepTime),
                FontSize = detailFontSize,
                FontAttributes = FontAttributes.Bold,
                LineBreakMode = LineBreakMode.WordWrap
            };
            //////////
            dishSubDetail = new Label
            {
                TextColor = Color.Gray,
                Text = TextTools.FirstCharToUpper(Recipe.DishType),
                FontSize = detailFontSize,
                FontAttributes = FontAttributes.Bold,
                LineBreakMode = LineBreakMode.WordWrap
            };

            StackLayout subDetailStack = new StackLayout // stack em high
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Spacing = 5,
                Children =
                {
                    dishSubDetail,
                    prepSubDetail,
                    totalSubDetail
                },
                MinimumWidthRequest = Units.ScreenWidth25Percent
            };

            Label totalDetail = new Label
            {
                TextColor = Color.Black,
                Text = "Cooking Time",
                FontSize = detailFontSize,
                FontAttributes = FontAttributes.Bold,
                LineBreakMode = LineBreakMode.WordWrap
            };

            Label prepDetail = new Label
            {
                TextColor = Color.Black,
                Text = "Prep Time",
                FontSize = detailFontSize,
                FontAttributes = FontAttributes.Bold,
                LineBreakMode = LineBreakMode.WordWrap
            };

            Label dishDetail = new Label
            {
                TextColor = Color.Black,
                Text = "Dish Type",
                FontSize = detailFontSize,
                FontAttributes = FontAttributes.Bold,
                LineBreakMode = LineBreakMode.WordWrap
            };

            StackLayout mainDetailStack = new StackLayout // stack em high
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Spacing = 5,
                Children =
                {
                    dishDetail,
                    prepDetail,
                    totalDetail
                },
                MinimumWidthRequest = Units.ScreenWidth25Percent
            };



            recipeStatusLabel = new Label
            {
                BackgroundColor = Color.Transparent,
                Text = "U",
                FontFamily = Fonts.GetBoldAppFont(),
                FontSize = Units.FontSizeXL,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White
            };

            StatusGrid = new Grid
            {
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,


            };

            StatusGrid.Children.Add(cornerImage.Content, 0, 0);
            StatusGrid.Children.Add(recipeStatusLabel, 0, 0);
            //statusGrid.Children.Add(eggtimerImage.Content, 0, 0);


            ///cornerImage.Content.TranslateTo(0, 0, 0, Easing.Linear);

            //mainDetailStack.TranslateTo(-28, 0, 0, null);
            //subDetailStack.TranslateTo(-16, 0, 0, null);

            mainDetailStack.TranslateTo(-28, 0, 0, null);
            subDetailStack.TranslateTo(-16, 0, 0, null);

            recipeStatusLabel.TranslateTo(-8, 4, 0, Easing.Linear);


            //Recipe.chai.Status = "draft";
            //Recipe.chai.Status = "published";

            if (Recipe.chai != null)
            {
                if (Recipe.chai.Status != null)
                {
                    UpdateStatus();
                }
            }

            StackLayout topContainer = new StackLayout
            {
                BackgroundColor = Color.White,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                //Padding = new Thickness(0, 0, 0, Dimensions.GENERAL_COMPONENT_PADDING),
                Spacing = 0,
                WidthRequest = topContainerWidth,
                Children =
                {
                    StatusGrid,
                    mainDetailStack,
                    subDetailStack,
                    //new RecipeDetail().GetContent(),
                    //new RecipeSubDetail(Recipe, Recipe.DishType).GetContent(),
                    imageContainer
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
                            AppSession.SelectedRecipe = Recipe;
                            await App.ShowFullRecipe(AppSession.SelectedRecipe, false, false);
                            //await App.ShowRecipeEditor();
                        }
                    });
                })
            });

            titleLbl = new Label
            {
                TextColor = Color.White,
                Text = TextTools.FirstCharToUpper(Recipe.Name),
                FontAttributes = FontAttributes.Bold,
                FontSize = Units.FontSizeL,
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

            createdTitleLbl = new Label
            {
                TextColor = Color.White,
                Text = EmptyCreator("ChaiCooking"),
                FontSize = Units.FontSizeML,
                FontAttributes = FontAttributes.Bold,
                LineBreakMode = LineBreakMode.TailTruncation
            };

            if (AppSettings.ShowAuthor)
            {
                if (Recipe.Creator != null)
                {
                    if (Recipe.Creator.FirstName != null && Recipe.Creator.FirstName.Length > 0)
                    {
                        createdTitleLbl.Text = Recipe.Creator.FirstName;
                    }
                }


                if (Recipe.Author != null && Recipe.Author.Length > 0)
                {
                    createdTitleLbl.Text = Recipe.Author;
                }
            }

            if (createdTitleLbl.Text.Length > 32)
            {
                createdTitleLbl.Text = createdTitleLbl.Text.Substring(0, 32);
            }


            StackLayout createdByTitleStack = new StackLayout // stack em high
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Horizontal,
                Spacing = 0,
                Children =
                {
                    createdTitleLbl
                }
            };

            Label createdByLbl = new Label
            {
                TextColor = Color.White,
                Text = "Created by:",
                FontSize = Units.FontSizeM
            };

            StackLayout createdByStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Spacing = 0,
                Children =
                {
                    createdByLbl,
                    createdByTitleStack
                },
                WidthRequest = Units.ScreenWidth30Percent
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
                    App.ShowAlert("EDIT THIS RECIPE");
                    //await App.PerformActionAsync(new Models.Action((int)Helpers.Actions.ActionName.ShowRecipeOptionsPanel));
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
                    createdByStack,
                    editImage.Content,
                    optionImage.Content
                }
            };

            TouchEffect.SetNativeAnimation(editImage.Content, true);
            TouchEffect.SetCommand(editImage.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    AppSession.SelectedRecipe = Recipe;
                    //await App.ShowFullRecipe(AppSession.SelectedRecipe);
                    await App.ShowRecipeEditor();
                });
            }));

            TouchEffect.SetNativeAnimation(optionImage.Content, true);
            TouchEffect.SetCommand(optionImage.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (Recipe.chai.Status == "published")
                    {
                        if (Recipe.WhiskRecipeId.Length > 0)
                        {
                            if (AppSession.InfoModeOn)
                            {
                                App.ShowInfoBubble(new Paragraph("Options", "Tap the options button on any recipe (lower right) when it has been confirmed as a P = published recipe (keep checking back in as this may take a short while to update), add it to a Meal Plan (paid feature), add it to a Collection (album), or Quick shop it. You can do this by tapping on any of their options buttons.", null).Content, Units.HalfScreenWidth, Units.HeaderHeight);
                            }
                            else
                            {
                                await App.ShowRecipeOptionsModal(Recipe);
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            var result = await DataManager.DeleteUserRecipe(Recipe.chai.Id.ToString());
                            if (result)
                            {
                                var deleteRecipe = AppSession.recipeEditorRecipes.Find(x => x.chai.Id == Recipe.chai.Id);
                                AppSession.recipeEditorRecipes.Remove(deleteRecipe);
                                await Task.Delay(10);
                                var recipeGroup = new RecipeEditorViewSection(AppSession.recipeEditorRecipes);
                                AppSession.recipeEditorCollection.Add(recipeGroup);
                                if (AppSession.recipeEditorCollection.Count > 1)
                                {
                                    AppSession.recipeEditorCollection.RemoveAt(0);
                                }
                            }
                            else
                            {
                                App.ShowAlert("Failed to remove recipe.");
                            }
                        }
                        catch
                        {
                            App.ShowAlert("Failed to remove recipe.");
                        }
                    }
                });
            }));

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

            UpdateContents();
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

        public void UpdateContents()
        {
            try
            {
                recipeImage.Content.Source = Recipe.MainImageSource;

            }
            catch (Exception e) { }


            totalSubDetail.Text = Recipe.CookingTime;

            prepSubDetail.Text = EmptyCheck(Recipe.PrepTime);

            dishSubDetail.Text = TextTools.FirstCharToUpper(Recipe.DishType);

            if (Recipe.chai != null)
            {
                if (Recipe.chai.Status != null)
                {
                    UpdateStatus();
                }
            }

            titleLbl.Text = TextTools.FirstCharToUpper(Recipe.Name);

            if (AppSettings.ShowAuthor)
            {
                if (Recipe.Creator != null)
                {
                    if (Recipe.Creator.FirstName != null && Recipe.Creator.FirstName.Length > 0)
                    {
                        createdTitleLbl.Text = Recipe.Creator.FirstName;
                    }
                }

                if (Recipe.Author != null && Recipe.Author.Length > 0)
                {
                    createdTitleLbl.Text = Recipe.Author;
                }
            }
        }

        public void SetStatus(string status)
        {
            Recipe.chai.Status = status;
            UpdateStatus();
        }

        public void UpdateStatus()
        {
            eggtimerImage.Content.IsVisible = false;
            recipeStatusLabel.Opacity = 1.0;
            cornerImage.Content.Opacity = 1.0;
            editImage.Content.Opacity = 1.0;
            optionImage.Content.Opacity = 1.0;

            if (Recipe.chai.Status == "draft")
            {
                recipeStatusLabel.Text = "U";
                cornerImage = ImageTools.Tint(cornerImage, Color.FromHex(Colors.CC_PALE_GREY));
                cornerImage.Content.HorizontalOptions = LayoutOptions.StartAndExpand;
                cornerImage.Content.VerticalOptions = LayoutOptions.StartAndExpand;
            }

            if (Recipe.chai.Status == "complete")
            {
                recipeStatusLabel.Text = "C";
                cornerImage = ImageTools.Tint(cornerImage, Color.FromHex(Colors.CC_GREEN));
                cornerImage.Content.HorizontalOptions = LayoutOptions.StartAndExpand;
                cornerImage.Content.VerticalOptions = LayoutOptions.StartAndExpand;
            }

            if (Recipe.chai.Status == "published")
            {
                // debug
                // Recipe.WhiskRecipeId = "uidb98gd938cg39g9e7et";
                if (Recipe.WhiskRecipeId.Length > 0)
                {
                    recipeStatusLabel.Text = "P";
                    cornerImage = ImageTools.Tint(cornerImage, Color.FromHex(Colors.CC_ORANGE));
                    optionImage.Content.Source = "menu_circle.png";

                }
                else
                {
                    //eggtimerImage.Content.IsVisible = true;
                    recipeStatusLabel.Text = "P";
                    cornerImage = ImageTools.Tint(cornerImage, Color.FromHex(Colors.CC_DARK_ORANGE));

                    recipeStatusLabel.Opacity = 0.5;
                    cornerImage.Content.Opacity = 0.5;

                }
                cornerImage.Content.HorizontalOptions = LayoutOptions.StartAndExpand;
                cornerImage.Content.VerticalOptions = LayoutOptions.StartAndExpand;
            }

            StatusGrid.Children.Clear();
            StatusGrid.Children.Add(cornerImage.Content, 0, 0);
            StatusGrid.Children.Add(recipeStatusLabel, 0, 0);
        }
    }
}
