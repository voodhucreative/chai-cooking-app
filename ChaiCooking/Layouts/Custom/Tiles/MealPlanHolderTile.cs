using System;
using ChaiCooking.AppData;
using ChaiCooking.Components;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.Feed;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Tiles
{
    public class MealPlanHolderTile : ActiveComponent
    {
        StackLayout imageContainer, nameContainer; // Containers will help us create the parts of the tile

        Grid masterGrid; // Master Grid will define the base layout

        public StaticLabel nameLabel { get; set; }
        public StaticImage recipeImage { get; set; }
        public Frame newFrame { get; set; }
        public StackLayout outerFrame { get; set; }
        public bool isSearched { get; set; }
        private bool IsSelected { get; set; }
        private Recipe recipe { get; set; }
        private int tileHeight { get; set; }
        private DragGestureRecognizer dragGesture { get; set; }
        public MealPlanHolderTile(Recipe recipe)
        {
            this.recipe = recipe;
            tileHeight = 46;

            dragGesture = new DragGestureRecognizer()
            {
                CanDrag = true,
                DragStartingCommand = new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async ()
                     =>
                    {
                        AppSession.SelectedRecipe = recipe;
                    });
                }),
            };

            newFrame = new Frame
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                CornerRadius = 5,
                IsClippedToBounds = true,
                Padding = 0,
                Margin = 2,
            };

            outerFrame = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                IsClippedToBounds = true,
                Padding = 0,
                Children = { newFrame },
                BackgroundColor = Color.Transparent
            };

            if (recipe.IsSelected)
            {
                IsSelected = true;
                dragGesture.CanDrag = true;
                outerFrame.BackgroundColor = Color.Orange;
            }
            else
            {
                IsSelected = false;
                dragGesture.CanDrag = false;
                outerFrame.BackgroundColor = Color.Transparent;
            }

            imageContainer = new StackLayout
            {
                BackgroundColor = Color.White,
                WidthRequest = 100,
                HeightRequest = 40,
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
                WidthRequest = 100,
                HeightRequest = 40,
                ColumnDefinitions =
                {
                    { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) } },
                },
                Children =
                {
                    {imageContainer,0,0},
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
            nameLabel.Content.Padding = 2;

            nameContainer = new StackLayout
            {
                BackgroundColor = Color.Black,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = 80,
                HeightRequest = 40,
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
                    { new RowDefinition { Height = new GridLength(40) } },
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Star) } }
                },
                Children =
                {
                    {topContainer,0,0},
                    {nameContainer,0,1}
                }
            };

            masterGrid.GestureRecognizers.Add(dragGesture);

            if (recipe.Images != null)
            {
                SetImageSearched(recipe.Images[0]);
            }
            else
            {
                SetImageSearched(null);
            }

            SetName(recipe.Name);

            masterGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async ()
                     =>
                    {
                        if (IsSelected)
                        {
                            this.IsSelected = false;
                            AppSession.SelectedRecipe = null;
                            recipe.IsSelected = false;
                            AppSession.mealPlanHolderCollectionView.SelectedItems.Remove(recipe);
                            dragGesture.CanDrag = false;
                            outerFrame.BackgroundColor = Color.Transparent;
                        }
                        else
                        {
                            this.IsSelected = true;
                            recipe.IsSelected = true;
                            AppSession.mealPlanHolderCollectionView.SelectedItems.Add(recipe);
                            dragGesture.CanDrag = true;
                            outerFrame.BackgroundColor = Color.Orange;
                        }
                        //await App.ShowFullRecipe(recipe, AppSession.CurrentUser.recipeHolder);
                    });
                })
            });

            newFrame.Content = masterGrid;

            Content.Children.Add(outerFrame);
        }

        public void SetImageSearched(Models.Custom.Image uri)
        {
            if (uri != null)
            {
                recipeImage = new StaticImage(uri.Url.AbsoluteUri, tileHeight - (tileHeight / 4), tileHeight - (tileHeight / 4), null);
                recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                recipe.MainImageSource = uri.Url.AbsoluteUri;
                imageContainer.Children.Add(recipeImage.Content);
            }
            else
            {
                recipeImage = new StaticImage("chaismallbag.png", tileHeight - (tileHeight / 4), tileHeight - (tileHeight / 4), null);
                recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                imageContainer.Children.Add(recipeImage.Content);
            }
        }

        public void SetImage(ImageElement uri)
        {
            if (uri != null)
            {
                recipeImage = new StaticImage(uri.Url.AbsoluteUri, tileHeight - (tileHeight / 4), tileHeight - (tileHeight / 4), null);
                recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                recipe.MainImageSource = uri.Url.AbsoluteUri;
                imageContainer.Children.Add(recipeImage.Content);
            }
            else
            {

                recipeImage = new StaticImage("chaismallbag.png", tileHeight - (tileHeight / 4), tileHeight - (tileHeight / 4), null);
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