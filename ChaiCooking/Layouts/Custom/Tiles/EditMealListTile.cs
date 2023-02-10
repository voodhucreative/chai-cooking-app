using System;
using ChaiCooking.AppData;
using ChaiCooking.Components;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.Feed;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
namespace ChaiCooking.Layouts.Custom.Tiles
{
    public class EditMealListTile : ActiveComponent
    {
        StackLayout imageContainer, nameContainer; // Containers will help us create the parts of the tile

        Grid masterGrid; // Master Grid will define the base layout

        public StaticLabel nameLabel { get; set; }
        public StaticImage recipeImage { get; set; }
        public Frame outerFrame { get; set; }
        public bool isSearched { get; set; }
        public Recipe tileRecipe { get; set; }
        public EditMealListTile(Recipe recipe)
        {
            this.tileRecipe = recipe;
            imageContainer = new StackLayout
            {
                BackgroundColor = Color.White,
                WidthRequest = 80,
                HeightRequest = 50,
                Children =
                {
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
                WidthRequest = 80,
                HeightRequest = 50,
                Children =
                {
                    imageContainer
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
                WidthRequest = 80,
                HeightRequest = 30,
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
                WidthRequest = 80,
                HeightRequest = 80,
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
                    try { StaticData.whiskRecipeID = recipe.Id; } catch { }
                    try { StaticData.chaiRecipeID = recipe.chai.Id.ToString(); } catch { }
                    try { AppSession.SelectedRecipe = recipe; } catch { }
                    StaticData.setPreviewRecipe(recipe);
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

            if (recipe.Images != null)
            {
                SetImageSearched(recipe.Images[0]);
            }
            else
            {
                SetImageSearched(null);
            }

            SetName(recipe.Name);

            Content.Children.Add(newframe);
        }

        public void SetImageSearched(Models.Custom.Image uri)
        {
            if (uri != null)
            {
                recipeImage = new StaticImage(uri.Url.AbsoluteUri, 80, 50, null);
                recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                tileRecipe.MainImageSource = uri.Url.AbsoluteUri;
                imageContainer.Children.Add(recipeImage.Content);
            }
            else
            {
                recipeImage = new StaticImage("chaismallbag.png", 80, 50, null);
                recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                imageContainer.Children.Add(recipeImage.Content);
            }
        }

        public void SetImage(ImageElement uri)
        {
            if (uri != null)
            {
                recipeImage = new StaticImage(uri.Url.AbsoluteUri, 80, 50, null);
                recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                tileRecipe.MainImageSource = uri.Url.AbsoluteUri;
                imageContainer.Children.Add(recipeImage.Content);
            }
            else
            {
                recipeImage = new StaticImage("chaismallbag.png", 100, 50, null);
                recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                imageContainer.Children.Add(recipeImage.Content);
            }
        }

        public void SetName(string input)
        {
            this.nameLabel.Content.Text = input;
        }
    }
}
