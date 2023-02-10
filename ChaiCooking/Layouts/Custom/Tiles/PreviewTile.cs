using System;
using ChaiCooking.Components;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.Feed;
using ChaiCooking.Services;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Tiles
{
    public class PreviewTile : ActiveComponent
    {
        StackLayout imageContainer, nameContainer; // Containers will help us create the parts of the tile

        Grid masterGrid; // Master Grid will define the base layout

        public StaticImage Icon { get; set; }

        public StaticLabel nameLabel { get; set; }
        public StaticLabel timeLabel { get; set; }
        public StaticImage recipeImage { get; set; }
        public Frame outerFrame { get; set; }
        Datum recipeDatum { get; set; }
        Recipe recipe { get; set; }
        Action<Recipe> showRecipeView { get; set; }

        public PreviewTile()
        {
            StaticImage timeIcon = new StaticImage("timer.png", 15, 15, null);

            timeIcon.Content.HorizontalOptions = LayoutOptions.Center;
            timeIcon.Content.VerticalOptions = LayoutOptions.Center;

            timeLabel = new StaticLabel("");
            timeLabel.Content.TextColor = Color.DarkOrange;
            timeLabel.Content.FontAttributes = FontAttributes.Bold;
            timeLabel.Content.HorizontalTextAlignment = TextAlignment.Center;
            timeLabel.Content.VerticalTextAlignment = TextAlignment.Center;
            timeLabel.Content.FontSize = Units.FontSizeM;

            StackLayout timeCont = new StackLayout
            {
                BackgroundColor = Color.Transparent,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = 1,
                IsClippedToBounds = false,
                Children =
                {
                    timeIcon.Content,
                    timeLabel.Content
                }
            };

            Grid detailsGrid = new Grid
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                ColumnSpacing = 0,
                RowSpacing = 0,
                Padding = 0,
                //BackgroundColor = Color.Green,
                WidthRequest = 50,
                HeightRequest = 50,
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(50) } },
                },
                Children =
                {
                    {timeCont,0,0},
                }
            };

            StackLayout leftCont = new StackLayout
            {
                //BackgroundColor = Color.Blue,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = 1,
                IsClippedToBounds = false,
                Children =
                {
                    detailsGrid
                }
            };

            imageContainer = new StackLayout
            {
                BackgroundColor = Color.White,
                WidthRequest = 50,
                HeightRequest = 50,
                Children =
                {
                }
            };

            StackLayout rightCont = new StackLayout
            {
                //BackgroundColor = Color.Red,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = 1,
                IsClippedToBounds = true,
                Children =
                {
                    imageContainer
                }
            };

            Grid topGrid = new Grid
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                ColumnSpacing = 0,
                RowSpacing = 0,
                Padding = 0,
                //BackgroundColor = Color.Green,
                WidthRequest = 100,
                HeightRequest = 50,
                ColumnDefinitions =
                {
                    { new ColumnDefinition { Width = new GridLength(50) } },
                    { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) } }
                },
                Children =
                {
                    {leftCont,0,0},
                    {rightCont,1,0}
                }
            };

            StackLayout topContainer = new StackLayout
            {
                BackgroundColor = Color.White,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = 1,
                IsClippedToBounds = true,
                Children =
                {
                    topGrid
                }
            };

            nameLabel = new StaticLabel("");
            nameLabel.Content.TextColor = Color.White;
            nameLabel.Content.FontAttributes = FontAttributes.Bold;
            nameLabel.Content.HorizontalTextAlignment = TextAlignment.Center;
            nameLabel.Content.VerticalTextAlignment = TextAlignment.Center;
            nameLabel.Content.FontSize = Units.FontSizeS;
            nameLabel.Content.LineBreakMode = LineBreakMode.CharacterWrap;

            nameContainer = new StackLayout
            {
                BackgroundColor = Color.Black,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = 100,
                HeightRequest = 50,
                Padding = 1,
                IsClippedToBounds = true,
                Children =
                {
                    nameLabel.Content
                }
            };

            masterGrid = new Grid
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                ColumnSpacing = 0,
                RowSpacing = 0,
                Padding = 0,
                //BackgroundColor = Color.Green,
                WidthRequest = 100,
                HeightRequest = 100,
                IsClippedToBounds = true,
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(50) } },
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Star) } }
                },
                Children =
                {
                    {topContainer,0,0},
                    {nameContainer,0,1}
                }
            };

            Frame newframe = new Frame
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Content = masterGrid,
                CornerRadius = 5,
                IsClippedToBounds = true,
                Padding = 0,
            };

            Command command = new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("Recipe Preview", "Tap to see enlarged details of the recipe within it and tap the close option to close them.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                    }
                    else
                    {
                        if (recipe != null)
                        {
                            string id = "";
                            if (recipe.WhiskRecipeId == null)
                            {
                                id = recipe.Id;
                            }
                            else
                            {
                                id = recipe.WhiskRecipeId;
                            }
                            if (AppSettings.GoDirectlyToRecipeFromMealPlan)
                            {
                                Recipe newRecipe = await DataManager.GetRecipe(id);
                                if (newRecipe != null)
                                {
                                    showRecipeView(newRecipe);
                                }
                                else
                                {
                                    App.ShowAlert("An error occured");
                                }
                            }
                            else
                            {
                                Recipe newRecipe = await DataManager.GetRecipe(id);
                                if (newRecipe != null)
                                {
                                    showRecipeView(newRecipe);
                                }
                                else
                                {
                                    App.ShowAlert("An error occured");
                                }
                            }
                        }
                    }
                });
            });

            TouchEffect.SetNativeAnimation(newframe, true);
            TouchEffect.SetCommand(newframe, command);

            Content.Children.Add(newframe);
        }

        public void SetImageSearched(Models.Custom.Image uri)
        {
            if (uri != null)
            {
                recipeImage = new StaticImage(uri.Url.AbsoluteUri, 50, 50, null);
                recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                imageContainer.Children.Add(recipeImage.Content);
            }
            else
            {
                recipeImage = new StaticImage("chaismallbag.png", 50, 50, null);
                recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                imageContainer.Children.Add(recipeImage.Content);
            }
        }

        public void SetImage(ImageElement uri)
        {
            if (uri != null)
            {
                recipeImage = new StaticImage(uri.Url.AbsoluteUri, 50, 50, null);
                recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                imageContainer.Children.Add(recipeImage.Content);
            }
            else
            {
                recipeImage = new StaticImage("chaismallbag.png", 50, 50, null);
                recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                imageContainer.Children.Add(recipeImage.Content);
            }
        }

        public void SetImage(Uri uri)
        {
            if (uri != null)
            {
                recipeImage = new StaticImage(uri.AbsoluteUri, 50, 50, null);
                recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                imageContainer.Children.Add(recipeImage.Content);
            }
            else
            {
                recipeImage = new StaticImage("chaismallbag.png", 50, 50, null);
                recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                imageContainer.Children.Add(recipeImage.Content);
            }
        }

        public void SetImage(string url)
        {
            recipeImage = new StaticImage(url, 50, 35, null);
            recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
            recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
            imageContainer.Children.Add(recipeImage.Content);
        }

        public void SetName(string input)
        {
            this.nameLabel.Content.Text = input;
        }

        public void SetTime(long? time)
        {
            if (time == null)
            {
                time = 0;
            }
            this.timeLabel.Content.Text = time + "m";
        }

        public void SetTime(string? time)
        {
            if (time == null)
            {
                time = "0";
            }
            this.timeLabel.Content.Text = time + "m";
        }

        public void SetDatumRecipe(Datum input)
        {
            this.recipeDatum = input;
        }

        public void SetRecipe(Recipe input)
        {
            this.recipe = input;
        }

        public void SetAction(Action<Recipe> input)
        {
            this.showRecipeView = input;
        }
    }
}
