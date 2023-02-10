using System;
using System.Collections.Generic;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Helpers;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.MealPlanAPI;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom
{
    // Temporary class to be replaced in the future.
    public class MealPlannerUserRow : ActiveComponent
    {
        string day;
        Frame dateFrame;
        Grid masterGrid, mealGrid;
        int tileWidth = 50;
        int tileHeight = 50;
        int mealCounter = 0;
        int index { get; set; }

        public MealPlannerUserRow(int index, List<Meal> userMealTemplate = null, string date = "", int id = -1, bool isCalendarTile = false)
        {
            this.index = index;
            masterGrid = new Grid
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                ColumnSpacing = 0,
                RowSpacing = 0,
                HeightRequest = 90,
            };

            masterGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(90) });
            masterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = Units.ScreenWidth10Percent });
            masterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

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
                HeightRequest = 90,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Never
            };

            dateFrame = new Frame
            {
                BorderColor = Color.White,
                Padding = 0,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                Margin = 2
            };

            DateTime dateTime = DateTime.ParseExact(date, "yyyy-MM-dd", null);
            string newDate = dateTime.ToString("ddd\ndd\nMMM");
            day = newDate;

            var parseDate = dateTime.ToString("dd/MM/yyyy");
            var currentDate = DateTime.Now.ToString("dd/MM/yyyy");
            if (parseDate == currentDate)
            {
                dateFrame.BackgroundColor = Color.FromHex(Colors.CC_ORANGE);
                //scrollView.BackgroundColor = Color.FromHex(Colors.CC_ORANGE);
                mealGrid.BackgroundColor = Color.FromHex(Colors.CC_ORANGE);
            }


            dateFrame.Content = new Label
            {
                Text = day,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = Units.FontSizeM
            };

            if (userMealTemplate == null)
            {
                for (int i = 0; i < 4; i++)
                {
                    MealPlannerTile mealPlannerTile = new MealPlannerTile(false, tileWidth, tileHeight);

                    mealPlannerTile.SetMealType(i);
                    mealPlannerTile.SetInternalDate(date);
                    mealPlannerTile.SetMealPlanID(id);
                    mealPlannerTile.SetRowIndex(index);
                    if (!isCalendarTile)
                    {
                        mealPlannerTile.SetCalendarTile(false);
                    }
                    else
                    {
                        mealPlannerTile.SetCalendarTile(true);
                    }
                    mealGrid.Children.Add(mealPlannerTile.GetContent(), i, 0);
                }
            }
            else
            {
                //Reducing masive code duplication.
                //Creating a list of each mealtype to itterate through
                List<string> mealStrings = new List<string> { "breakfast", "lunch", "dinner", "snacks" };

                foreach(string mealString in mealStrings)
                {
                    var meal = userMealTemplate.Find(x => x.MealType.ToLower() == mealString);
                    if (meal != null)
                    {
                        MealPlannerTile mealPlannerTile = new MealPlannerTile(true, tileWidth, tileHeight);
                        mealPlannerTile.SetRowIndex(index);
                        if (meal.MealType != null)
                        {
                            mealPlannerTile.SetMealPeriod(meal.MealType);
                        }

                        string hypenedDate = dateTime.ToString("yyyy-MM-dd");
                        mealPlannerTile.SetInternalDate(hypenedDate);
                        mealPlannerTile.SetMealPlanID(id);
                        mealPlannerTile.SetMealID(meal.Id);
                        mealPlannerTile.SetMealType(mealCounter);
                        if (!isCalendarTile)
                        {
                            mealPlannerTile.SetCalendarTile(false);
                        }
                        else
                        {
                            mealPlannerTile.SetCalendarTile(true);
                        }
                        mealCounter++;
                        if (meal.Recipe != null)
                        {
                            if (meal.Recipe.Ingredients == null)
                            {
                                //System.Diagnostics.Debugger.Break();
                            }
                            mealPlannerTile.SetRecipe(meal.Recipe);
                        }

                        mealGrid.Children.Add(mealPlannerTile.GetContent(), mealStrings.IndexOf(mealString), 0);
                    }
                    else
                    {
                        createBlankTile(dateTime, id, mealStrings.IndexOf(mealString), isCalendarTile);
                    }
                }
            }
                
            masterGrid.Children.Add(dateFrame, 0, 0);
            masterGrid.Children.Add(scrollView, 1, 0);
            Content.Children.Add(masterGrid);
        }

        public void createBlankTile(DateTime d, int inputID, int colPos, bool isCalendarTile)
        {
            MealPlannerTile mealPlannerTile = new MealPlannerTile(false, tileWidth, tileHeight);
            mealPlannerTile.SetRowIndex(index);
            mealPlannerTile.SetMealType(mealCounter);
            string hypenedDate = d.ToString("yyyy-MM-dd");
            mealPlannerTile.SetInternalDate(hypenedDate);
            mealPlannerTile.SetMealPlanID(inputID);
            if (!isCalendarTile)
            {
                mealPlannerTile.SetCalendarTile(false);
            }
            else
            {
                mealPlannerTile.SetCalendarTile(true);
            }
            mealCounter++;
            mealGrid.Children.Add(mealPlannerTile.GetContent(), colPos, 0);
        }
    }
}
