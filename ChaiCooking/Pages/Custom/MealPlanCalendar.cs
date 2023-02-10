using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom;
using ChaiCooking.Layouts.Custom.Lists;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.Feed;
using ChaiCooking.Models.Custom.MealPlanAPI;
using ChaiCooking.Services;
using ChaiCooking.Services.Storage;
using ChaiCooking.Views.CollectionViews.CreateOrSelect;
using ChaiCooking.Views.CollectionViews.MealPlanHolder;
using ChaiCooking.Views.CollectionViews.MealPlanner;
using FFImageLoading.Transformations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    class MealPlanCalendar : Page
    {
        MealPlanHolderCollectionView mealPlanHolderCollectionView;
        CollectionView mealPlanHolderLayout;
        Grid weekContainer, mealPlanHolderGrid;
        string week;
        Frame binFrame, toggleRecipeHolderFrame;
        StaticImage editImage, binImage, NextArrow, LastArrow;
        Grid topGrid;
        StackLayout dayButtonCont, weekCont;
        CollectionView mealPlannerLayout;
        MealPlannerCollectionView mealPlannerCollectionView;
        StaticLabel titleLabel, toggleLabel, weekLabel;
        TapGestureRecognizer prevWeekGesture, nextWeekGesture;
        VisualStateGroup stateGroup;
        VisualState visible, notVisible;
        bool isUpdating = false;

        public MealPlanCalendar()
        {
            this.IsScrollable = false;
            this.IsRefreshable = false;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;

            this.Id = (int)AppSettings.PageNames.MPCalendar;
            this.Name = AppData.AppText.MEAL_PLAN;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.SlideInFromRight;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.SlideOutToRight;

            AppSession.SetMealPlanner = SetMealPlanner;
            stateGroup = new VisualStateGroup();
            visible = new VisualState()
            {
                Name = "Visible"
            };
            visible.Setters.Add(new Setter
            {
                Property = Grid.IsVisibleProperty,
                Value = true
            });

            notVisible = new VisualState()
            {
                Name = "NotVisible"
            };

            notVisible.Setters.Add(new Setter
            {
                Property = Grid.IsVisibleProperty,
                Value = false
            });

            stateGroup.States.Add(visible);
            stateGroup.States.Add(notVisible);

            PageContent = new Grid
            {
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY)
            };

            titleLabel = new StaticLabel("");
            titleLabel.Content.TextColor = Color.White;
            titleLabel.Content.FontAttributes = FontAttributes.Bold;
            titleLabel.Content.HorizontalTextAlignment = TextAlignment.Center;
            titleLabel.Content.VerticalTextAlignment = TextAlignment.Center;

            StackLayout titleLabelCont = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children = { titleLabel.Content }
            };

            StaticImage dayButton = new StaticImage("plus.png", 24, 24, null);

            dayButtonCont = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End,
                Children = { dayButton.Content },
                IsVisible = false
            };

            TouchEffect.SetNativeAnimation(dayButtonCont, true);
            TouchEffect.SetCommand(dayButtonCont,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.ShowTemplateDayModal(false); // We're adding!
                });
            }));

            topGrid = new Grid
            {
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                RowSpacing = 0,
                ColumnSpacing = 0,
                BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY),
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                },
                ColumnDefinitions =
                {
                    { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)}},
                    { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto)}},
                },
                Children =
                {
                    { titleLabelCont, 0, 0},
                    { dayButtonCont, 1, 0 },
                }
            };

            weekLabel = new StaticLabel("");

            switch (StaticData.week)
            {
                case StaticData.WeekeNum.week1:
                    weekLabel.Content.Text = "WK1";
                    week = "1";
                    break;
                case StaticData.WeekeNum.week2:
                    weekLabel.Content.Text = "WK2";
                    week = "2";
                    break;
                case StaticData.WeekeNum.week3:
                    weekLabel.Content.Text = "WK3";
                    week = "3";
                    break;
                case StaticData.WeekeNum.week4:
                    weekLabel.Content.Text = "WK4";
                    week = "4";
                    break;
            }

            Color inactiveColor = Color.White;

            if (StaticData.chosenWeeks == 1)
            {
                inactiveColor = Color.DarkGray;
            }

            weekLabel.Content.TextColor = inactiveColor;
            weekLabel.Content.FontAttributes = FontAttributes.Bold;
            weekLabel.Content.FontSize = Units.FontSizeL;
            weekLabel.RightAlign();

            Color tintColour = Color.LightGray;
            TintTransformation colorTint = new TintTransformation
            {
                HexColor = (string)tintColour.ToHex(),
                EnableSolidColor = true

            };
            var tint = new List<FFImageLoading.Work.ITransformation>();
            tint.Add(colorTint);

            LastArrow = new StaticImage("chevronleftbold.png", 48, tint);
            LastArrow.Content.HorizontalOptions = LayoutOptions.Start;
            LastArrow.Content.VerticalOptions = LayoutOptions.Center;
            LastArrow.Content.HeightRequest = 20;
            NextArrow = new StaticImage("chevronrightbold.png", 48, tint);
            NextArrow.Content.HorizontalOptions = LayoutOptions.End;
            NextArrow.Content.VerticalOptions = LayoutOptions.Center;
            NextArrow.Content.HeightRequest = 20;

            toggleLabel = new StaticLabel("Tap here to view the recipe holder.");
            toggleLabel.Content.FontSize = Units.FontSizeM;
            toggleLabel.Content.TextColor = Color.White;
            toggleLabel.Content.HorizontalTextAlignment = TextAlignment.Center;
            toggleLabel.Content.VerticalTextAlignment = TextAlignment.Center;
            toggleLabel.Content.FontFamily = Fonts.GetBoldAppFont();

            TouchEffect.SetNativeAnimation(toggleLabel.Content, true);

            weekCont = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    { LastArrow.Content},
                        { weekLabel.Content},
                        { NextArrow.Content}
                }
            };

            LastArrow.Content.GestureRecognizers.Clear();
            LastArrow.Content.GestureRecognizers.Add(GetPrevWeekGesture());
            NextArrow.Content.GestureRecognizers.Clear();
            NextArrow.Content.GestureRecognizers.Add(GetNextWeekGesture());

            toggleRecipeHolderFrame = new Frame
            {
                Padding = 0,
                HasShadow = false,
                BorderColor = Color.White,
                BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY),
                Content = toggleLabel.Content,
            };

            weekContainer = new Grid
            {
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                VerticalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = Units.ScreenWidth,
                BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY),
                ColumnSpacing = 2,
                ColumnDefinitions =
                {
                        { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)}},
                        { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto)}},
                },
                Children =
                    {
                        { toggleRecipeHolderFrame, 0, 0 },
                        { weekCont, 1, 0 },
                    },
            };

            Grid masterGrid = new Grid
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight,
                ColumnSpacing = 0,
                RowSpacing = 0,
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Star)}},
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                },
                ColumnDefinitions =
                {
                     { new ColumnDefinition { Width = Units.ScreenWidth}},
                },
                Children =
                {
                    { topGrid, 0, 0 },
                    { BuildMealPlannerContent(week), 0, 1 },
                    { weekContainer, 0, 2 }
                }
            };

            Label mealPlanHolderTitle = new Label
            {
                Text = "Recipe Holder",
                TextColor = Color.White,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = Units.FontSizeM,
                FontFamily = Fonts.GetBoldAppFont(),
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            Label leftArrow = new Label
            {
                Text = "<",
                TextColor = Color.White,
                VerticalTextAlignment = TextAlignment.Center,
                FontFamily = Fonts.GetBoldAppFont(),
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            Label rightArrow = new Label
            {
                Text = ">",
                TextColor = Color.White,
                VerticalTextAlignment = TextAlignment.Center,
                FontFamily = Fonts.GetBoldAppFont(),
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            if (AppSession.CurrentUser.recipeHolder == null)
            {
                AppSession.CurrentUser.recipeHolder = new List<Recipe>();
            }

            binImage = new StaticImage("trash.png", 24, 24, null);

            StackLayout binCont = new StackLayout
            {
                Children =
                {
                    binImage.Content
                }
            };

            binFrame = new Frame
            {
                CornerRadius = 4,
                Content = binCont,
                WidthRequest = 40,
                HeightRequest = 40,
                Padding = 2,
                BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY),
                Margin = 1,
            };

            TouchEffect.SetNativeAnimation(binFrame, true);
            TouchEffect.SetCommand(binFrame,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // Remove All
                    if (AppSession.CurrentUser.recipeHolder.Count > 0)
                    {
                        await App.ShowMealHolderRemove();
                    }
                    else
                    {
                        App.ShowAlert("No recipes to remove.");
                    }
                });
            }));

            binFrame.GestureRecognizers.Add(new DropGestureRecognizer()
            {
                AllowDrop = true,
                DragOverCommand = new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async ()
                     =>
                    {
                        binFrame.Content.BackgroundColor = Color.Orange;
                        binImage.Content.BackgroundColor = Color.Orange;
                        binImage.Content.Scale = 1.5;
                    });
                }),
                DragLeaveCommand = new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async ()
                     =>
                    {
                        binFrame.Content.BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY);
                        binImage.Content.BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY);
                        binImage.Content.Scale = 1;
                    });
                }),
                DropCommand = new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async ()
                     =>
                    {
                        binFrame.Content.BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY);
                        binImage.Content.BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY);
                        binImage.Content.Scale = 1;
                        if (AppSession.SelectedRecipe != null)
                        {
                            if (AppSession.mealPlanHolderCollectionView.SelectedItems.Count == 1)
                            {
                                await App.ShowMealHolderRemove(AppSession.SelectedRecipe);
                            }
                            else
                            {
                                await App.ShowMealHolderRemove(AppSession.mealPlanHolderCollectionView.SelectedItems);
                            }
                        }
                    });
                })
                //DropCommand = command,
            });
            StackLayout removalContent = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Spacing = 2,
                Children =
                {
                    binFrame,
                }
            };

            mealPlanHolderGrid = new Grid
            {
                ColumnSpacing = 2,
                RowSpacing = 0,
                HeightRequest = 100,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                },
                ColumnDefinitions =
                {
                    { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto)}},
                    { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto)}},
                    { new ColumnDefinition { Width = new GridLength(10)}},
                    { new ColumnDefinition { Width = 50}},
                },
                Children =
                {
                    { mealPlanHolderTitle, 0, 0 },
                    { leftArrow, 0, 1 },
                    { BuildMealPlanHolderContent(), 1, 1 },
                    { rightArrow, 2, 1 },
                    { removalContent, 3, 1 }
                }
            };

            VisualStateManager.GetVisualStateGroups(mealPlanHolderGrid).Add(stateGroup);

            TouchEffect.SetCommand(toggleRecipeHolderFrame.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("Recipe Waiting Bar", "Tap once to open the bar, drag recipes out onto the meal plan and replace existing recipes, and tap the bar again to close it.Tap to highlight a single or multiple recipes and then drag and drop one onto the trash icon.It then deletes all recipes selected.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                    }
                    else
                    {
                        if (stateGroup.CurrentState == visible)
                        {
                            VisualStateManager.GoToState(mealPlanHolderGrid, "NotVisible");
                            toggleRecipeHolderFrame.BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY);
                            toggleLabel.Content.Text = "Tap here to view the recipe holder.";
                        }
                        else
                        {
                            VisualStateManager.GoToState(mealPlanHolderGrid, "Visible");
                            toggleRecipeHolderFrame.BackgroundColor = Color.Orange;
                            toggleLabel.Content.Text = "Tap here to close the recipe holder.";
                        }
                    }
                });
            }));

            Grid.SetColumnSpan(mealPlanHolderTitle, 3);

            masterGrid.Children.Add(mealPlanHolderGrid, 0, 3);
            VisualStateManager.GoToState(mealPlanHolderGrid, "NotVisible");
            PageContent.Children.Add(masterGrid);
        }

        public override Task Update()
        {
            if (StaticData.mealPlanClicked)
            {
                GetMealPlans();
                if (AppSession.mealPlanCheck)
                {
                    UpdateCalendar();
                    ShowPopup();
                    AppSession.UserCollectionRecipes = DataManager.GetFavouriteRecipes();
                }
                StaticData.mealPlanClicked = false;
            }
            //if (StaticData.isEditing == true)
            //{
            //    StaticData.isEditing = false;
            //    editImageCont.Children.Clear();
            //    editImage = new StaticImage("editproperty.png", 24, 24, null);
            //    Color tintColour = Color.White;

            //    TintTransformation colorTint = new TintTransformation
            //    {
            //        HexColor = (string)tintColour.ToHex(),
            //        EnableSolidColor = true

            //    };
            //    editImage.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
            //    editImage.Content.Transformations.Add(colorTint);
            //    editImageCont.Children.Add(editImage.Content);
            //    StaticData.isEditing = false;
            //}
            App.SetSubHeaderTitle(AppText.RECOMMENDED_RECIPES, new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.RecommendedRecipes));
            return base.Update();
        }

        public async void UpdateCalendar()
        {
            await App.ShowLoading();
            
            if (BusyCheck())
            {
            }
            else
            {
                try
                {
                    if (!AppSession.mealPlanTokenSource.IsCancellationRequested)
                    {
                        if (AppSession.mealPlannerCalendar != null)
                        {
                            await Task.Delay(100);
                            isUpdating = true;
                            switch (StaticData.week)
                            {
                                case StaticData.WeekeNum.week1:
                                    week = "1";
                                    break;
                                case StaticData.WeekeNum.week2:
                                    week = "2";
                                    break;
                                case StaticData.WeekeNum.week3:
                                    week = "3";
                                    break;
                                case StaticData.WeekeNum.week4:
                                    week = "4";
                                    break;
                            }
                            AppSession.mealPlannerCalendar = await App.ApiBridge.GetWeek(AppSession.CurrentUser, AppSession.CurrentUser.defaultMealPlanID.ToString(), week, AppSession.mealPlanTokenSource.Token);
                            var mealPlannerGroup = new MealPlannerCollectionViewSection(AppSession.mealPlannerCalendar.Data);
                            AppSession.mealPlannerCollection.Add(mealPlannerGroup);
                            AppSession.mealPlannerCollection.RemoveAt(0);
                            AppSession.SetMealPlanner(AppSession.CurrentUser.defaultMealPlanName, true, StaticData.chosenWeeks, false);
                        }

                        if (AppSession.mealPlanHolderCollection != null)
                        {
                            foreach (Recipe recipe in AppSession.CurrentUser.recipeHolder)
                            {
                                recipe.IsSelected = false;
                            }
                            AppSession.mealPlanHolderCollectionView.SelectedItems.Clear();
                            LocalDataStore.SaveAll();
                            var mealPlanHolderGroup = new MealPlanHolderCollectionViewSection(AppSession.CurrentUser.recipeHolder);
                            AppSession.mealPlanHolderCollection.Add(mealPlanHolderGroup);
                            AppSession.mealPlanHolderCollection.RemoveAt(0);
                        }
                        isUpdating = false;
                    }
                    else
                    {
                        AppSession.mealPlanTokenSource = new CancellationTokenSource();
                    }
                }
                catch(Exception e)
                {

                }
                await App.HideLoading();
            }
        }

        public async void ShowPopup()
        {
            await App.ShowSelectOrCreateModal();
        }

        //public void ToggleEditActive()
        //{
        //    editImageCont.Children.Clear();
        //    editImage = new StaticImage("editproperty.png", 24, 24, null);

        //    if (StaticData.isEditing)
        //    {
        //        Color tintColour = Color.White;

        //        TintTransformation colorTint = new TintTransformation
        //        {
        //            HexColor = (string)tintColour.ToHex(),
        //            EnableSolidColor = true

        //        };
        //        editImage.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
        //        editImage.Content.Transformations.Add(colorTint);
        //        StaticData.isEditing = false;
        //        App.ShowAlert("Editing meals disabled.");
        //    }
        //    else
        //    {
        //        Color tintColour = Color.Orange;

        //        TintTransformation colorTint = new TintTransformation
        //        {
        //            HexColor = (string)tintColour.ToHex(),
        //            EnableSolidColor = true

        //        };
        //        editImage.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
        //        editImage.Content.Transformations.Add(colorTint);
        //        StaticData.isEditing = true;
        //        App.ShowAlert("Editing meals enabled.");
        //    }
        //    editImageCont.Children.Add(editImage.Content);
        //}

        private CollectionView BuildMealPlanHolderContent()
        {
            try
            {
                mealPlanHolderCollectionView = new MealPlanHolderCollectionView();
                mealPlanHolderLayout = mealPlanHolderCollectionView.GetCollectionView();
                mealPlanHolderCollectionView.ShowRecipes();
                return mealPlanHolderLayout;
            }
            catch
            {
                return mealPlanHolderLayout;
            }
        }

        private CollectionView BuildMealPlannerContent(string week)
        {
            try
            {
                mealPlannerCollectionView = new MealPlannerCollectionView();
                mealPlannerLayout = mealPlannerCollectionView.GetCollectionView();
                mealPlannerCollectionView.ShowCalendar(week);
                return mealPlannerLayout;
            }
            catch
            {
                return mealPlannerLayout;
            }
        }

        private void SetMealPlanner(string input, bool showUI, int numWeeks, bool publish = false)
        {
            dayButtonCont.IsVisible = false;
            titleLabel.Content.Text = input;
            if (!showUI)
            {
                topGrid.IsVisible = false;
                weekContainer.IsVisible = false;
                weekCont.Children.Clear();
                VisualStateManager.GoToState(mealPlanHolderGrid, "NotVisible");
                toggleRecipeHolderFrame.BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY);
                toggleLabel.Content.Text = "Tap here to view the recipe holder.";
            }
            else
            {
                topGrid.IsVisible = true;
                weekContainer.IsVisible = true;
                weekCont.Children.Clear();
                weekCont.Children.Add(LastArrow.Content);
                weekCont.Children.Add(weekLabel.Content);
                weekCont.Children.Add(NextArrow.Content);
                VisualStateManager.GoToState(mealPlanHolderGrid, "NotVisible");
                toggleRecipeHolderFrame.BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY);
                toggleLabel.Content.Text = "Tap here to view the recipe holder.";
            }
            if (!publish)
            {
                if (numWeeks > 1)
                {
                    LastArrow.Content.Opacity = 1;
                    NextArrow.Content.Opacity = 1;
                }
                else
                {
                    LastArrow.Content.Opacity = 0;
                    NextArrow.Content.Opacity = 0;
                }
            }
            else
            {
                dayButtonCont.IsVisible = true;

                ColourButton createButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.PUBLISH_TEMPLATE, null);
                createButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
                createButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
                createButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

                TouchEffect.SetNativeAnimation(createButton.Content, true);
                TouchEffect.SetCommand(createButton.Content,
                    new Command(() =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            var result = await App.ApiBridge.UpdateUserMealPlanTemplates(AppSession.CurrentUser, StaticData.currentTemplateID, StaticData.currentTemplateName, DateTime.Now.ToString("yyyy-MM-dd"));
                            if (result)
                            {
                                await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.HealthyLiving);
                            }
                            else
                            {
                                App.ShowAlert("Failed to publish.");
                            }
                        });
                    }));

                weekCont.Children.Clear();
                weekCont.Children.Add(createButton.Content);

                TouchEffect.SetNativeAnimation(titleLabel.Content, true);
                TouchEffect.SetCommand(titleLabel.Content, new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async ()
                     =>
                    {
                        await App.ShowEditTitle(StaticData.currentTemplateID, StaticData.currentTemplateName, null);
                    });
                }));
            }
        }

        private void SetWeeks(StaticData.WeekeNum weekInput, string weekName)
        {
            if (BusyCheck())
            {
                App.ShowAlert("Please wait...");
            }
            else
            {
                StaticData.week = weekInput;
                weekLabel.Content.Text = weekName;
                UpdateCalendar();
            }
        }

        private TapGestureRecognizer GetPrevWeekGesture()
        {
            prevWeekGesture = new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (StaticData.chosenWeeks > 1)
                        {
                            switch (StaticData.week)
                            {
                                case StaticData.WeekeNum.week1:
                                    if (StaticData.chosenWeeks == 2)
                                    {
                                        SetWeeks(StaticData.WeekeNum.week2, "WK2");
                                        break;
                                    }
                                    else if (StaticData.chosenWeeks == 3)
                                    {
                                        SetWeeks(StaticData.WeekeNum.week3, "WK3");
                                        break;
                                    }
                                    else if (StaticData.chosenWeeks == 4)
                                    {
                                        SetWeeks(StaticData.WeekeNum.week4, "WK4");
                                        break;
                                    }
                                    break;
                                case StaticData.WeekeNum.week2:
                                    SetWeeks(StaticData.WeekeNum.week1, "WK1");
                                    break;
                                case StaticData.WeekeNum.week3:
                                    SetWeeks(StaticData.WeekeNum.week2, "WK2");
                                    break;
                                case StaticData.WeekeNum.week4:
                                    SetWeeks(StaticData.WeekeNum.week3, "WK3");
                                    break;
                            }
                        }
                    });
                })
            };
            return prevWeekGesture;
        }

        private TapGestureRecognizer GetNextWeekGesture()
        {
            nextWeekGesture = new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (StaticData.chosenWeeks > 1)
                        {
                            switch (StaticData.week)
                            {
                                case StaticData.WeekeNum.week1:
                                    SetWeeks(StaticData.WeekeNum.week2, "WK2");
                                    break;
                                case StaticData.WeekeNum.week2:
                                    if (StaticData.chosenWeeks > 2)
                                    {
                                        SetWeeks(StaticData.WeekeNum.week3, "WK3");
                                        break;
                                    }
                                    else
                                    {
                                        SetWeeks(StaticData.WeekeNum.week1, "WK1");
                                        break;
                                    }
                                case StaticData.WeekeNum.week3:
                                    if (StaticData.chosenWeeks > 3)
                                    {
                                        SetWeeks(StaticData.WeekeNum.week4, "WK4");
                                        break;
                                    }
                                    else
                                    {
                                        SetWeeks(StaticData.WeekeNum.week1, "WK1");
                                        break;
                                    }

                                case StaticData.WeekeNum.week4:
                                    if (StaticData.chosenWeeks == 4)
                                    {
                                        SetWeeks(StaticData.WeekeNum.week1, "WK1");
                                    }
                                    break;
                            }
                        }
                    });

                })
            };

            return nextWeekGesture;
        }

        private bool BusyCheck()
        {
            if (isUpdating)
            {
                AppSession.mealPlanTokenSource.Cancel();
                return true;
            }
            else
            {
                return false;
            }
        }

        private async void GetMealPlans()
        {
            StaticData.userMealPlans = await App.ApiBridge.GetUserMealPlans(AppSession.CurrentUser);
            if (StaticData.userMealPlans != null)
            {
                AppSession.mealPlanCheck = true;
            }
            else
            {
                AppSession.mealPlanCheck = false;
            }
        }
    }
}
