using ChaiCooking.AppData;
using ChaiCooking.Components;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.MealPlanAPI;
using ChaiCooking.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Tiles
{
    public class PreviewMealPlanTile : ActiveComponent
    {
        StackLayout masterContainer, mealPeriodContainer, imgCont;

        public Label mealPeriod { get; set; }

        public Label duration { get; set; }

        public Label calories { get; set; }

        public Label nameLabel { get; set; }

        public Label totalTimeLabel { get; set; }

        public StaticImage recipeImage { get; set; }

        public Ingredient[] ingredients { get; set; } // For Meal Preview

        public int mealTypeID { get; set; }

        public int mealPlanID { get; set; }

        public string date { get; set; }

        public Recipe recipe { get; set; }

        bool hasRecipe = false;

        StaticImage timeIcon;

        public PreviewMealPlanTile(bool isSnack, int width, int height)
        {
            hasRecipe = isSnack;

            if (hasRecipe)
            {
                // New and Improved Tile Code

                mealPeriodContainer = new StackLayout
                {
                    BackgroundColor = Color.White,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
                    WidthRequest = 100,
                    HeightRequest = 20,
                };

                mealPeriod = getMealPeriodLabel();

                mealPeriodContainer.Children.Add(mealPeriod);

                timeIcon = new StaticImage("timer.png", 10, 10, null);

                timeIcon.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                timeIcon.Content.VerticalOptions = LayoutOptions.FillAndExpand;

                totalTimeLabel = getTotalTimeLabel();

                StackLayout timeDetCont = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    WidthRequest = 50,
                    HeightRequest = 35,
                    Orientation = StackOrientation.Horizontal,
                    BackgroundColor = Color.White,
                    Padding = 1,
                    Spacing = 1,
                    Children =
                    {
                        timeIcon.Content,
                        totalTimeLabel
                    }
                };

                recipeImage = getImage();

                imgCont = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start,
                    WidthRequest = 50,
                    HeightRequest = 35,
                    Orientation = StackOrientation.Vertical,
                    Padding = 1
                };

                //imgCont.Children.Add(recipeImage.Content);

                Grid splitGrid = new Grid
                {
                    ColumnDefinitions = {
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                    },
                    RowDefinitions =
                    {
                        new RowDefinition { Height = new GridLength(35) }
                    },
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
                    BackgroundColor = Color.White,
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                    Children =
                    {
                        { timeDetCont, 0, 0 },
                        { imgCont, 1, 0 }
                    }
                };
                Grid.SetRowSpan(imgCont, 2);

                nameLabel = getMealNameLabel();

                StackLayout titleCont = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start,
                    WidthRequest = 100,
                    HeightRequest = 35,
                    Orientation = StackOrientation.Vertical,
                    BackgroundColor = Color.Black,
                    IsClippedToBounds = true,
                    Padding = 1,
                    Children =
                    {
                        nameLabel
                    }
                };

                masterContainer = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    BackgroundColor = Color.White,
                    Spacing = 0,
                    Children =
                {
                        mealPeriodContainer,
                        splitGrid,
                        titleCont
                }
                };

                Frame frame = new Frame
                {
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start,
                    Content = masterContainer,
                    CornerRadius = 5,
                    IsClippedToBounds = true,
                    Padding = 0,
                };
                TouchEffect.SetNativeAnimation(Container, true);
                TouchEffect.SetCommand(Container,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        Recipe newRecipe = await DataManager.GetRecipe(recipe.Id);
                        if (newRecipe != null)
                        {
                            await App.ShowFullRecipe(newRecipe, false, false);
                        }
                        else
                        {
                            App.ShowAlert("An error occured");
                        }

                    });
                }));

                Content.Children.Add(frame);
                Content.WidthRequest = 100;
            }
            else
            {
                StackLayout iconContainer = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    BackgroundColor = Color.Transparent,
                    WidthRequest = 100,
                    HeightRequest = 90,
                    Children =
                    {
                    }
                };

                Container.GestureRecognizers.Add(
                new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            // Open Recipe Menu Here.
                            //await App.ShowAddRecipe(GetMealType(), date, mealPlanID);
                        });
                    })
                });

                Frame newframe = new Frame
                {
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start,
                    Content = iconContainer,
                    CornerRadius = 5,
                    IsClippedToBounds = true,
                    Padding = 0,
                    BackgroundColor = Color.White.MultiplyAlpha(0.5),
                    //BorderColor = Color.Green
                };

                Content.Children.Add(newframe);
            }
        }

        public Label getMealPeriodLabel()
        {
            return mealPeriod = new Label
            {
                Text = "",
                TextColor = Color.Orange,
                FontSize = Units.FontSizeM,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                WidthRequest = 100
            };
        }

        public void SetMealPeriod(string name)
        {
            mealPeriod.Text = name;
        }

        public Label getMealNameLabel()
        {
            return nameLabel = new Label
            {
                Text = "",
                TextColor = Color.White,
                FontSize = Units.FontSizeS,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold
            };
        }

        public Label getTotalTimeLabel()
        {
            return totalTimeLabel = new Label
            {
                Text = "",
                TextColor = Color.Orange,
                FontSize = Units.FontSizeS,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
            };
        }

        public void setTotalTime(long time)
        {
            totalTimeLabel.Text = time.ToString() + "m";
        }

        public StaticImage getImage()
        {

            return recipeImage;
        }

        public void SetIngredients(Ingredient[] input)
        {
            ingredients = input;
        }

        public void SetPrevIngredients(Ingredient[] input)
        {

        }

        public void SetMealType(int input)
        {
            this.mealTypeID = input;
        }

        public int GetMealType()
        {
            return this.mealTypeID;
        }

        public void SetMealPlanID(int input)
        {
            this.mealPlanID = input;
        }

        public void SetInternalDate(string input)
        {
            this.date = input;
        }

        public void SetRecipe(Recipe r)
        {
            this.recipe = r;

            if (r.Images != null)
            {
                recipeImage = new StaticImage(r.Images[0].Url.AbsoluteUri, 50, 35, null);
                recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                recipe.MainImageSource = r.Images[0].Url.AbsoluteUri;
                imgCont.Children.Add(recipeImage.Content);
            }
            else
            {
                recipeImage = new StaticImage("chaismallbag.png", 50, 35, null);
                recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                imgCont.Children.Add(recipeImage.Content);
            }

            if (r.Durations != null)
            {
                this.totalTimeLabel.Text = $"{r.Durations.TotalTime}m";
            }
            else
            {
                this.totalTimeLabel.Text = "0m";
            }

            List<Ingredient> ingredientsList = new List<Ingredient>();

            foreach (Ingredient i in r.Ingredients)
            {
                Ingredient newIngredient = new Ingredient();
                newIngredient.Text = i.Text;
                ingredientsList.Add(newIngredient);
            }

            ingredients = ingredientsList.ToArray();

            this.nameLabel.Text = r.Name;
        }
    }
}
