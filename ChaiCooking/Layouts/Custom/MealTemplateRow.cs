using System;
using System.Collections.Generic;
using System.Linq;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using ChaiCooking.Views.CollectionViews.MealPlanner;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom
{
    public class MealTemplateRow : ActiveComponent
    {
        Frame dateFrame;
        Grid masterGrid, mealGrid;
        int tileWidth = 50;
        int tileHeight = 50;
        int id { get; set; }
        int day_number { get; set; }

        public MealTemplateRow(MealPlanModel.Datum item)
        {
            id = item.id;
            day_number = (int)item.day_Number;
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
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
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
            if (item.mealTemplates.Count == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    MealPlannerTile mealPlannerTile = new MealPlannerTile(false, tileWidth, tileHeight);
                    mealPlannerTile.SetIsTemplate(true);
                    mealPlannerTile.SetDayTemplateID(id);
                    mealPlannerTile.SetMealType(i);
                    mealPlannerTile.SetTemplateID(StaticData.currentTemplateID);
                    mealGrid.Children.Add(mealPlannerTile.GetContent(), i, 0);
                }
            }
            else
            {
                //Reducing masive code duplication.
                //Creating a list of each mealtype to itterate through
                List<string> mealStrings = new List<string> { "breakfast", "lunch", "dinner", "snacks"};

                foreach (string mealString in mealStrings)
                {
                    var meal = item.mealTemplates.Find(x => x.mealType.ToLower() == mealString);
                    if (meal != null)
                    {
                        MealPlannerTile mealPlannerTile = new MealPlannerTile(true, tileWidth, tileHeight);
                        mealPlannerTile.SetIsTemplate(true);
                        mealPlannerTile.SetDayTemplateID(id);
                        mealPlannerTile.SetTemplateID(StaticData.currentTemplateID);
                        mealPlannerTile.SetMealTemplateID(meal.id);
                        if (meal.mealType != null)
                        {
                            mealPlannerTile.SetMealPeriod(meal.mealType);
                        }

                        if (meal.recipe != null)
                        {
                            if(meal.recipe.Ingredients == null)
                            {
                                System.Diagnostics.Debugger.Break();
                            }
                            mealPlannerTile.SetRecipe(meal.recipe);
                        }

                        mealGrid.Children.Add(mealPlannerTile.GetContent(), mealStrings.IndexOf(mealString), 0);
                    }
                    else
                    {
                        createBlankTile(mealStrings.IndexOf(mealString));
                    }
                }
            }
                

            Content.Children.Add(masterGrid);
        }

        public void createBlankTile(int colPos)
        {
            MealPlannerTile mealPlannerTile = new MealPlannerTile(false, tileWidth, tileHeight);
            mealPlannerTile.SetIsTemplate(true);
            mealPlannerTile.SetDayTemplateID(id); mealPlannerTile.SetMealType(colPos);
            mealPlannerTile.SetTemplateID(StaticData.currentTemplateID);
            mealGrid.Children.Add(mealPlannerTile.GetContent(), colPos, 0);
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

            TouchEffect.SetNativeAnimation(dayLabel.Content, true);
            TouchEffect.SetCommand(dayLabel.Content, new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.ShowTemplateDayModal(true); // We're deleting
                });
            }));

            return dateFrame;
        }
    }
}
