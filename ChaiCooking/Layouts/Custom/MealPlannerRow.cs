using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.Feed;
using ChaiCooking.Models.Custom.MealPlanAPI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom
{
    class MealPlannerRow : ActiveComponent
    {
        Frame dateFrame;
        Grid masterGrid, mealGrid;
        int tileWidth = 50;
        int tileHeight = 50;

        public MealPlannerRow(MealPlanModel.Datum item)
        {
            mealGrid = new Grid
            {
                ColumnSpacing = 2,
                RowSpacing = 0,
                IsClippedToBounds = true,
                RowDefinitions =
                {
                    {new RowDefinition { Height = new GridLength(90) } }
                },
                ColumnDefinitions =
                {
                    {new ColumnDefinition { Width = new GridLength(100) } },
                    {new ColumnDefinition { Width = new GridLength(100) } },
                    {new ColumnDefinition { Width = new GridLength(100) } },
                    {new ColumnDefinition { Width = new GridLength(100) } },
                }
            };

            if (item.mealTemplates.Count == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    PreviewMealPlanTile PreviewMealPlanTile = new PreviewMealPlanTile(false, tileWidth, tileHeight);
                    mealGrid.Children.Add(PreviewMealPlanTile.GetContent(), i, 0);
                }
            }
            else
            {
                var breakfast = item.mealTemplates.Find(x => x.mealType == "breakfast");
                if (breakfast != null)
                {
                    PreviewMealPlanTile PreviewMealPlanTile = new PreviewMealPlanTile(true, tileWidth, tileHeight);

                    if (breakfast.mealType != null)
                    {
                        PreviewMealPlanTile.SetMealPeriod(breakfast.mealType);
                    }

                    if (breakfast.recipe != null)
                    {
                        PreviewMealPlanTile.SetRecipe(breakfast.recipe);
                    }

                    mealGrid.Children.Add(PreviewMealPlanTile.GetContent(), 0, 0);
                }
                else
                {
                    createBlankTile(0);
                }

                var lunch = item.mealTemplates.Find(x => x.mealType == "lunch");
                if (lunch != null)
                {
                    PreviewMealPlanTile PreviewMealPlanTile = new PreviewMealPlanTile(true, tileWidth, tileHeight);
                    if (lunch.mealType != null)
                    {
                        PreviewMealPlanTile.SetMealPeriod(lunch.mealType);
                    }

                    if (lunch.recipe != null)
                    {
                        PreviewMealPlanTile.SetRecipe(lunch.recipe);
                    }

                    mealGrid.Children.Add(PreviewMealPlanTile.GetContent(), 1, 0);
                }
                else
                {
                    createBlankTile(1);
                }

                var dinner = item.mealTemplates.Find(x => x.mealType == "dinner");
                if (dinner != null)
                {
                    PreviewMealPlanTile PreviewMealPlanTile = new PreviewMealPlanTile(true, tileWidth, tileHeight);
                    if (dinner.mealType != null)
                    {
                        PreviewMealPlanTile.SetMealPeriod(dinner.mealType);
                    }

                    if (dinner.recipe != null)
                    {
                        PreviewMealPlanTile.SetRecipe(dinner.recipe);
                    }

                    mealGrid.Children.Add(PreviewMealPlanTile.GetContent(), 2, 0);
                }
                else
                {
                    createBlankTile(2);
                }

                var snacks = item.mealTemplates.Find(x => x.mealType == "snacks");
                if (snacks != null)
                {
                    PreviewMealPlanTile PreviewMealPlanTile = new PreviewMealPlanTile(true, tileWidth, tileHeight);
                    if (snacks.mealType != null)
                    {
                        PreviewMealPlanTile.SetMealPeriod(snacks.mealType);
                    }

                    if (snacks.recipe != null)
                    {
                        PreviewMealPlanTile.SetRecipe(snacks.recipe);
                    }

                    mealGrid.Children.Add(PreviewMealPlanTile.GetContent(), 3, 0);
                }
                else
                {
                    createBlankTile(3);
                }
            }

            ScrollView scrollView = new ScrollView
            {
                Orientation = ScrollOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                WidthRequest = Units.ScreenWidth,
                Content = mealGrid,
                HeightRequest = 100,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Never
            };
            masterGrid = new Grid
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                ColumnSpacing = 0,
                RowSpacing = 0,
                BackgroundColor = Color.Transparent,
                HeightRequest = 90,
                WidthRequest = Units.ScreenWidth,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(90) },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = Units.ScreenWidth10Percent },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
                },
                Children =
                {
                    { BuildDaySection(item), 0, 0},
                    { scrollView, 1, 0},
                }
            };
            Content.Children.Add(masterGrid);
        }

        public void createBlankTile(int colPos)
        {
            PreviewMealPlanTile PreviewMealPlanTile = new PreviewMealPlanTile(false, tileWidth, tileHeight);
            mealGrid.Children.Add(PreviewMealPlanTile.GetContent(), colPos, 0);
        }

        public Frame BuildDaySection(MealPlanModel.Datum item)
        {
            StaticLabel dayLabel = new StaticLabel($"Day\n{item.day_Number}");
            dayLabel.Content.HorizontalTextAlignment = TextAlignment.Center;
            dayLabel.Content.VerticalTextAlignment = TextAlignment.Center;
            dayLabel.Content.TextColor = Color.White;
            dayLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            dayLabel.Content.FontSize = Units.FontSizeM;

            dateFrame = new Frame
            {
                BorderColor = Color.White,
                Padding = 0,
                BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY),
                Margin = 1,
                Content = dayLabel.Content
            };

            return dateFrame;
        }
    }
}
