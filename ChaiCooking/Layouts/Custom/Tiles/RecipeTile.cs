using System;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.Feed;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Tiles
{
    public class RecipeTile : ActiveComponent
    {
        StackLayout imageContainer, nameContainer; // Containers will help us create the parts of the tile

        Grid masterGrid; // Master Grid will define the base layout

        Color highlightedColour { get; set; }

        public StaticImage Icon { get; set; }

        bool IsSelected { get; set; }
        public StaticLabel nameLabel { get; set; }
        public StaticLabel timeLabel { get; set; }
        public StaticImage recipeImage { get; set; }
        public Frame outerFrame { get; set; }
        public bool isHighlighted { get; set; }
        public Action UpdateData;
        public bool isSearched { get; set; }
        Datum recipe;
        Recipe searchedRecipe;

        public RecipeTile()
        {
            highlightedColour = Color.Transparent;

            if (IsSelected)
            {
                highlightedColour = Color.FromHex(Colors.CC_ORANGE);
            }


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
            nameLabel.Content.LineBreakMode = LineBreakMode.TailTruncation;

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

            TouchEffect.SetNativeAnimation(masterGrid, true);
            TouchEffect.SetCommand(masterGrid,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (!isSearched)
                    {
                        ToggleHighlight();
                    }
                    else
                    {
                        StaticData.tilePressed = true;
                        ToggleHighlight(isSearched);
                    }
                });
            }));

            Frame newframe = new Frame
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Content = masterGrid,
                CornerRadius = 5,
                IsClippedToBounds = true,
                Padding = 0,
            };

            outerFrame = new Frame
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Content = newframe,
                CornerRadius = 5,
                IsClippedToBounds = true,
                Padding = 5,
                BorderColor = Color.Transparent,
                Margin = 0,
                BackgroundColor = Color.Transparent,
            };

            Content.Children.Add(outerFrame);
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

        public void SetHighlighted(bool isHighlighted)
        {
            if (!isHighlighted)
            {
                outerFrame.BackgroundColor = Color.Transparent;
                outerFrame.BorderColor = Color.Transparent;
            }
            else
            {
                outerFrame.BackgroundColor = Color.FromHex(Colors.CC_ORANGE);
                outerFrame.BorderColor = Color.FromHex(Colors.CC_ORANGE);
            }
        }

        //public void SetListID(int input)
        //{
        //    recipe.listID = input;
        //}

        public void ToggleHighlight()
        {
            if (StaticData.recipeFeed.Data != null)
            {
                foreach (Datum r in StaticData.recipeFeed.Data)
                {
                    r.Content.recipe.isHighlighted = false;
                }

                if (StaticData.whiskRecipeID != recipe.Content.recipe.Id)
                {
                    recipe.Content.recipe.isHighlighted = true;
                    StaticData.whiskRecipeID = recipe.Content.recipe.Id;
                    StaticData.chaiRecipeID = recipe.Chai.Id.ToString();
                }
                else
                {
                    recipe.Content.recipe.isHighlighted = false;
                    StaticData.whiskRecipeID = null;
                    StaticData.chaiRecipeID = null;
                }
            }

            this.UpdateData();
        }

        public void SetRecipe(Datum input)
        {
            recipe = input;

            if (recipe.Display.Images != null)
            {
                SetImage(recipe.Display.Images[0]);
            }
            else
            {
                SetImage(null);
            }

            SetName(recipe.Chai.Name);

            SetTime(recipe.Chai.CookTime);
        }

        // Special Recipe Specific

        public void SetSearchedRecipe(Recipe input)
        {
            searchedRecipe = input;

            if (searchedRecipe.Images != null)
            {
                SetImageSearched(searchedRecipe.Images[0]);
            }
            else
            {
                SetImageSearched(null);
            }

            SetName(searchedRecipe.Name);

            SetTime(searchedRecipe.CookingTime);
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

        public void ToggleHighlight(bool isSearched)
        {
            if (AppSession.SearchedRecipes != null)
            {
                foreach (Recipe r in AppSession.SearchedRecipes)
                {
                    r.isHighlighted = false;
                }

                if (StaticData.whiskRecipeID != searchedRecipe.Id)
                {
                    searchedRecipe.isHighlighted = true;
                    StaticData.whiskRecipeID = searchedRecipe.Id;
                    StaticData.chaiRecipeID = searchedRecipe.chai.Id.ToString();
                }
                else
                {
                    searchedRecipe.isHighlighted = false;
                    StaticData.whiskRecipeID = null;
                    StaticData.chaiRecipeID = null;
                }
            }

            this.UpdateData();
        }

        public void SetIsSearched(bool input)
        {
            this.isSearched = input;
        }
    }
}
