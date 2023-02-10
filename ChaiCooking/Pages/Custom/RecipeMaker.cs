// WIP
//using System;
//using System;
//namespace ChaiCooking.Pages.Custom
//{
//    public class RecipeMaker
//    {
//        public RecipeMaker()
//        {
//            using ChaiCooking.Branding;
//            using ChaiCooking.Components;
//            using ChaiCooking.Components.Labels;
//            using ChaiCooking.Helpers;
//            using ChaiCooking.Helpers.Custom;
//            using ChaiCooking.Layouts.Custom.Tiles;
//            using ChaiCooking.Models.Custom;
//            using ChaiCooking.Models.Custom.Feed;
//            using System;
//            using System.Collections.Generic;
//            using System.Globalization;
//            using System.Linq;
//            using System.Text;
//            using Xamarin.Forms;

//namespace ChaiCooking.Layouts.Custom
//    {
//        class MealPlannerRow : ActiveComponent
//        {
//            string day;
//            Frame dateFrame;
//            Grid masterGrid, mealGrid;
//            int tileWidth = 50;
//            int tileHeight = 50;

//            public MealPlannerRow(List<MealTemplate> mealTemplate, int mealDay = 0, string date = "")
//            {
//                masterGrid = new Grid
//                {
//                    VerticalOptions = LayoutOptions.Start,
//                    HorizontalOptions = LayoutOptions.FillAndExpand,
//                    ColumnSpacing = 0,
//                    RowSpacing = 0,
//                    //BackgroundColor = Color.Purple,
//                    BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
//                    HeightRequest = 90,
//                    WidthRequest = Units.ScreenWidth
//                };

//                masterGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(90) });
//                masterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = Units.ScreenWidth10Percent });
//                masterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

//                mealGrid = new Grid
//                {
//                    ColumnSpacing = 2,
//                    RowSpacing = 0,
//                    IsClippedToBounds = true,
//                    RowDefinitions =
//                {
//                    {new RowDefinition { Height = new GridLength(90) } }
//                },
//                    ColumnDefinitions =
//                {
//                    {new ColumnDefinition { Width = new GridLength(100) } },
//                    {new ColumnDefinition { Width = new GridLength(100) } },
//                    {new ColumnDefinition { Width = new GridLength(100) } },
//                    {new ColumnDefinition { Width = new GridLength(100) } },
//                }
//                };

//                ScrollView scrollView = new ScrollView
//                {
//                    Orientation = ScrollOrientation.Horizontal,
//                    HorizontalOptions = LayoutOptions.StartAndExpand,
//                    WidthRequest = Units.ScreenWidth,
//                    Content = mealGrid,
//                    HeightRequest = 100,
//                    HorizontalScrollBarVisibility = ScrollBarVisibility.Never
//                };

//                dateFrame = new Frame
//                {
//                    BorderColor = Color.White,
//                    Padding = 0,
//                    BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
//                    Margin = 2
//                };

//                if (mealDay == 99)
//                {
//                    DateTime dateTime = DateTime.Parse(date);
//                    string newDate = dateTime.ToString("ddd\ndd\nMMM");
//                    day = newDate;

//                    var parseDate = dateTime.ToString("dd/MM/yyyy");
//                    var currentDate = DateTime.Now.ToString("dd/MM/yyyy");
//                    if (parseDate == currentDate)
//                    {
//                        dateFrame.BackgroundColor = Color.FromHex(Colors.CC_ORANGE);
//                        //scrollView.BackgroundColor = Color.FromHex(Colors.CC_ORANGE);
//                        mealGrid.BackgroundColor = Color.FromHex(Colors.CC_ORANGE);
//                    }
//                }
//                else
//                {
//                    day = "Day " + mealDay.ToString();
//                }

//                dateFrame.Content = new Label
//                {
//                    Text = day,
//                    HorizontalTextAlignment = TextAlignment.Center,
//                    VerticalTextAlignment = TextAlignment.Center,
//                    TextColor = Color.White,
//                    FontAttributes = FontAttributes.Bold,
//                    FontSize = Units.FontSizeM
//                };

//                if (mealTemplate.Count == 0)
//                {
//                    for (int i = 0; i < 4; i++)
//                    {
//                        PreviewMealPlanTile mealPlannerTile = new PreviewMealPlanTile(false, tileWidth, tileHeight);
//                        mealGrid.Children.Add(mealPlannerTile.GetContent(), i, 0);
//                    }
//                }
//                else
//                {
//                    var breakfast = mealTemplate.Find(x => x.mealType == "breakfast");
//                    if (breakfast != null)
//                    {
//                        PreviewMealPlanTile mealPlannerTile = new PreviewMealPlanTile(true, tileWidth, tileHeight);
//                        if (breakfast.mealType != null)
//                        {
//                            mealPlannerTile.SetMealPeriod(breakfast.mealType);
//                        }

//                        if (breakfast.recipe != null)
//                        {
//                            mealPlannerTile.SetRecipe(breakfast.recipe);
//                        }

//                        mealGrid.Children.Add(mealPlannerTile.GetContent(), 0, 0);
//                    }
//                    else
//                    {
//                        createBlankTile(0);
//                    }

//                    var lunch = mealTemplate.Find(x => x.mealType == "lunch");
//                    if (lunch != null)
//                    {
//                        PreviewMealPlanTile mealPlannerTile = new PreviewMealPlanTile(true, tileWidth, tileHeight);
//                        if (lunch.mealType != null)
//                        {
//                            mealPlannerTile.SetMealPeriod(lunch.mealType);
//                        }

//                        if (lunch.recipe != null)
//                        {
//                            mealPlannerTile.SetRecipe(lunch.recipe);
//                        }

//                        mealGrid.Children.Add(mealPlannerTile.GetContent(), 1, 0);
//                    }
//                    else
//                    {
//                        createBlankTile(1);
//                    }

//                    var dinner = mealTemplate.Find(x => x.mealType == "dinner");
//                    if (dinner != null)
//                    {
//                        PreviewMealPlanTile mealPlannerTile = new PreviewMealPlanTile(true, tileWidth, tileHeight);

//                        if (dinner.mealType != null)
//                        {
//                            mealPlannerTile.SetMealPeriod(dinner.mealType);
//                        }

//                        if (dinner.recipe != null)
//                        {
//                            mealPlannerTile.SetRecipe(dinner.recipe);
//                        }

//                        mealGrid.Children.Add(mealPlannerTile.GetContent(), 2, 0);
//                    }
//                    else
//                    {
//                        createBlankTile(2);
//                    }

//                    var snacks = mealTemplate.Find(x => x.mealType == "snacks");
//                    if (snacks != null)
//                    {
//                        PreviewMealPlanTile mealPlannerTile = new PreviewMealPlanTile(true, tileWidth, tileHeight);

//                        if (snacks.mealType != null)
//                        {
//                            mealPlannerTile.SetMealPeriod(snacks.mealType);
//                        }

//                        if (snacks.recipe != null)
//                        {
//                            mealPlannerTile.SetRecipe(snacks.recipe);
//                        }

//                        mealGrid.Children.Add(mealPlannerTile.GetContent(), 3, 0);
//                    }
//                    else
//                    {
//                        createBlankTile(3);
//                    }
//                }

//                masterGrid.Children.Add(dateFrame, 0, 0);
//                masterGrid.Children.Add(scrollView, 1, 0);
//                Content.Children.Add(masterGrid);
//            }

//            public void createBlankTile(int colPos)
//            {
//                PreviewMealPlanTile mealPlannerTile = new PreviewMealPlanTile(false, tileWidth, tileHeight);
//                mealGrid.Children.Add(mealPlannerTile.GetContent(), colPos, 0);
//            }
//        }
//    }

//}
//    }
//}
