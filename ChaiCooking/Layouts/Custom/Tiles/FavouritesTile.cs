using System;
using System.Collections.Generic;
using System.Windows.Input;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using FFImageLoading.Transformations;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Tiles
{
    public class FavouritesTile : ActiveComponent
    {
        StackLayout imageContainer, sideItemsContainer,
            iconContainer, nameContainer; // Containers will help us create the parts of the tile

        Grid ContentLayer,
            masterGrid, heartContainer; // Master Grid will define the base layout

        Color highlightedColour { get; set; }

        public string IconUncheckedImageSource { get; set; }
        public string IconCheckedImageSource { get; set; }
        public StaticImage Icon { get; set; }

        bool IsSelected { get; set; }
        int tileWidth, tileHeight;
        public StaticLabel nameLabel { get; set; }
        public StaticLabel rating { get; set; }
        public StaticImage recipeImage { get; set; }
        public StackLayout outerFrame { get; set; }
        private DragGestureRecognizer dragGesture { get; set; }
        public Frame newFrame { get; set; }

        Recipe recipe;
        StackLayout RecipeContainer;
        public Action UpdateData;

        public FavouritesTile(Recipe recipe)
        {
            this.recipe = recipe;
            tileWidth = tileHeight;
            tileHeight = tileHeight;
            //IconCheckedImageSource = "tick.png";
            //IconUncheckedImageSource = "tickbg.png";
            highlightedColour = Color.Transparent;
            //Icon = new StaticImage(IconUncheckedImageSource, tileHeight / 4, tileHeight / 4, null);
            Color tintColour = Color.Red;

            TintTransformation colorTint = new TintTransformation
            {
                HexColor = (string)tintColour.ToHex(),
                EnableSolidColor = true

            };
            StaticImage heartIcon = new StaticImage("hearticonsmall.png", 20, 20, null);
            heartIcon.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
            heartIcon.Content.Transformations.Add(colorTint);

            //heartIcon = new StaticImage("heart.png", height / 3, height / 3, null);
            StaticImage starIcon = new StaticImage("star.png", tileHeight / 3, tileHeight / 3, null);
            StaticImage TypeIcon = new StaticImage("pizza.png", tileHeight / 6, tileHeight / 6, null);

            try
            {
                TypeIcon = new StaticImage(recipe.MealCategoryImageSource, 20, 20, null);
            }
            catch (Exception e)
            {

            }

            dragGesture = new DragGestureRecognizer()
            {
                CanDrag = true,
                DragStartingCommand = new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async ()
                     =>
                    {
                        AppSession.IsDraggable = true;
                        AppSession.SelectedRecipe = recipe;
                    });
                }),
                DropCompletedCommand = new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async ()
                        =>
                    {
                    });
                })
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
                //Icon.Content.Source = IconCheckedImageSource;
                dragGesture.CanDrag = true;
                outerFrame.BackgroundColor = Color.Orange;
            }
            else
            {
                IsSelected = false;
                //Icon.Content.Source = IconUncheckedImageSource;
                dragGesture.CanDrag = false;
                outerFrame.BackgroundColor = Color.Transparent;
            }

            iconContainer = new StackLayout
            {
                BackgroundColor = Color.White,
                HeightRequest = tileHeight / 4,
                WidthRequest = tileHeight / 4,
                Padding = 2,
                Children =
                {
                    heartIcon.Content,
                    //Icon.Content
                }
            };

            recipeImage = new StaticImage("chaismallbag.png", tileHeight - (tileHeight / 4), tileHeight - (tileHeight / 4), null);
            recipeImage.Content.HorizontalOptions = LayoutOptions.StartAndExpand;
            recipeImage.Content.VerticalOptions = LayoutOptions.StartAndExpand;

            Grid detailsGrid = new Grid
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                ColumnSpacing = 0,
                RowSpacing = 0,
                Padding = 0,
                //BackgroundColor = Color.Green,
                WidthRequest = 20,
                HeightRequest = 50,
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(50) } },
                },
                Children =
                {
                   {iconContainer,0,0}
                }
            };

            StackLayout rightCont = new StackLayout
            {
                //BackgroundColor = Color.Blue,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = 0,
                IsClippedToBounds = true,
                Children =
                {
                    detailsGrid
                }
            };

            imageContainer = new StackLayout
            {
                BackgroundColor = Color.White,
                WidthRequest = 60,
                HeightRequest = 60,
                Children =
                {
                }
            };

            StackLayout leftCont = new StackLayout
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
                HeightRequest = 60,
                ColumnDefinitions =
                {
                    { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) } },
                    { new ColumnDefinition { Width = new GridLength(20) } }
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
            nameLabel.Content.MaxLines = 2;
            nameLabel.Content.Text = recipe.Name;
            nameContainer = new StackLayout
            {
                BackgroundColor = Color.Black,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = 100,
                HeightRequest = 40,
                Padding = 2,
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
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(60) } },
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Star) } }
                },
                Children =
                {
                    {topContainer,0,0},
                    {nameContainer,0,1}
                }
            };

            masterGrid.GestureRecognizers.Add(dragGesture);
            TouchEffect.SetNativeAnimation(masterGrid, true);
            TouchEffect.SetCommand(masterGrid,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    Toggle();
                });
            }));

            newFrame.Content = masterGrid;

            try
            {
                if (recipe.MainImageSource != null)
                {
                    try
                    {
                        recipeImage = new StaticImage(recipe.MainImageSource, tileHeight - (tileHeight / 4), tileHeight - (tileHeight / 4), null);
                    }
                    catch (Exception e)
                    {
                        recipeImage = new StaticImage("chaismallbag.png", 50, 35, null);
                        recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                        recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                    }
                }
                else
                {

                    if (recipe.Images != null)
                    {
                        recipeImage = new StaticImage(recipe.Images[0].Url.AbsoluteUri, tileHeight - (tileHeight / 4), tileHeight - (tileHeight / 4), null);
                        recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                        recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                    }
                    else
                    {
                        recipeImage = new StaticImage("chaismallbag.png", 50, 35, null);
                        recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                        recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                    }
                }
            }
            catch
            {
                recipeImage = new StaticImage("chaismallbag.png", 50, 35, null);
                recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
            }



            if (imageContainer != null)
            {
                imageContainer.Children.Clear();
                imageContainer.Children.Add(recipeImage.Content);
            }

            Content.Children.Add(outerFrame);
        }

        private Recipe GetRecipe()
        {
            return this.recipe;
        }

        private void Toggle()
        {
            if (IsSelected)
            {
                this.IsSelected = false;
                //Icon.Content.Source = IconUncheckedImageSource;
                recipe.IsSelected = false;
                AppSession.favouritesCollectionView.SelectedItems.Remove(recipe);
                dragGesture.CanDrag = false;
                outerFrame.BackgroundColor = Color.Transparent;
                AppSession.SetViewText();
            }
            else
            {
                this.IsSelected = true;
                recipe.IsSelected = true;
                //Icon.Content.Source = IconCheckedImageSource;
                AppSession.favouritesCollectionView.SelectedItems.Add(recipe);
                dragGesture.CanDrag = true;
                outerFrame.BackgroundColor = Color.Orange;
                AppSession.SetViewText();
            }
        }
    }
}
