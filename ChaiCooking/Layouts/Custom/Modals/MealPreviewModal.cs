using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.MealPlanAPI;
using ChaiCooking.Services;
using ChaiCooking.Views.CollectionViews.InfluencerMealPreview;
using FFImageLoading.Transformations;
using Newtonsoft.Json;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Modals
{
    public class MealPreviewModal : StandardLayout
    {
        int numOfWeeks { get; set; }
        int week;
        string name;
        string storePlanLength;
        CollectionView previewLayout;
        PreviewCollectionView previewCollectionView;
        bool isBusy;
        bool isInfluencerMealPreview;

        public MealPreviewModal(string planLength, string mealPlanName, bool influencerMealPreview)
        {
            isBusy = false;
            isInfluencerMealPreview = influencerMealPreview;
            storePlanLength = planLength;
            string substring = planLength.Substring(0, 1);
            numOfWeeks = int.Parse(substring);
            this.name = mealPlanName;
            BuildGrid();
            AppSession.UserCollectionRecipes = DataManager.GetFavouriteRecipes();
        }

        public void BuildGrid()
        {
            StaticLabel subjectLabel = new StaticLabel(AppText.MEAL_PLAN);
            subjectLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            subjectLabel.Content.FontSize = Units.FontSizeXL;
            subjectLabel.Content.TextColor = Color.White;
            subjectLabel.CenterAlign();

            StaticLabel mealPlanLabel = new StaticLabel(name);
            mealPlanLabel.Content.FontFamily = Fonts.GetRegularAppFont();
            mealPlanLabel.Content.FontSize = Units.FontSizeL;
            mealPlanLabel.Content.TextColor = Color.White;
            mealPlanLabel.CenterAlign();

            ActiveLabel CloseLabel = new ActiveLabel(AppText.CLOSE, Units.FontSizeM, Color.Transparent, Color.White, null);
            CloseLabel.RightAlign();

            TouchEffect.SetNativeAnimation(CloseLabel.Content, true);
            TouchEffect.SetCommand(CloseLabel.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (isInfluencerMealPreview)
                    {
                        StaticData.storedInfluencerMealPlan = null;
                        StaticData.isTemplateView = false;
                        //await Update(); //
                        await App.UpdatePage(); // update current page
                    }
                    else
                    {
                        StaticData.storedInfluencerMealPlan = null;
                        await App.HidePreviewMealPlan();
                    }
                });
            }));

            Color inactiveColor = Color.White;

            if (numOfWeeks == 1)
            {
                inactiveColor = Color.DarkGray;
            }

            Color tintColour = Color.LightGray;
            TintTransformation colorTint = new TintTransformation
            {
                HexColor = (string)tintColour.ToHex(),
                EnableSolidColor = true

            };
            var tint = new List<FFImageLoading.Work.ITransformation>();
            tint.Add(colorTint);

            StaticImage LastArrow = new StaticImage("chevronleftbold.png", 48, tint);
            LastArrow.Content.HorizontalOptions = LayoutOptions.Start;
            LastArrow.Content.VerticalOptions = LayoutOptions.Center;
            LastArrow.Content.HeightRequest = 20;
            StaticImage NextArrow = new StaticImage("chevronrightbold.png", 48, tint);
            NextArrow.Content.HorizontalOptions = LayoutOptions.End;
            NextArrow.Content.VerticalOptions = LayoutOptions.Center;
            NextArrow.Content.HeightRequest = 20;

            StaticLabel weekLabel = new StaticLabel("");

            switch (StaticData.week)
            {
                case StaticData.WeekeNum.week1:
                    weekLabel.Content.Text = "WK1";
                    break;
                case StaticData.WeekeNum.week2:
                    weekLabel.Content.Text = "WK2";
                    break;
                case StaticData.WeekeNum.week3:
                    weekLabel.Content.Text = "WK3";
                    break;
                case StaticData.WeekeNum.week4:
                    weekLabel.Content.Text = "WK4";
                    break;
            }

            weekLabel.Content.TextColor = inactiveColor;
            weekLabel.Content.FontAttributes = FontAttributes.Bold;
            weekLabel.Content.HorizontalTextAlignment = TextAlignment.Center;
            weekLabel.Content.VerticalTextAlignment = TextAlignment.Center;
            weekLabel.Content.FontSize = Units.FontSizeL;

            if (numOfWeeks != 1)
            {
                LastArrow.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                StaticData.previewPlanRefresh = true;
                                switch (StaticData.weekPrev)
                                {
                                    case StaticData.WeekeNum.week1:
                                        if (StaticData.chosenWeeks == 2)
                                        {
                                            StaticData.weekPrev = StaticData.WeekeNum.week2;
                                            UpdatePreview(2);
                                            weekLabel.Content.Text = "WK2";
                                        }
                                        else if (StaticData.chosenWeeks == 3)
                                        {
                                            StaticData.weekPrev = StaticData.WeekeNum.week3;
                                            UpdatePreview(3);
                                            weekLabel.Content.Text = "WK3";
                                        }
                                        else if (StaticData.chosenWeeks == 4)
                                        {
                                            StaticData.weekPrev = StaticData.WeekeNum.week4;
                                            UpdatePreview(4);
                                            weekLabel.Content.Text = "WK4";
                                        }
                                        break;
                                    case StaticData.WeekeNum.week2:
                                        StaticData.weekPrev = StaticData.WeekeNum.week1;
                                        UpdatePreview(1);
                                        weekLabel.Content.Text = "WK1";
                                        break;
                                    case StaticData.WeekeNum.week3:
                                        StaticData.weekPrev = StaticData.WeekeNum.week2;
                                        UpdatePreview(2);
                                        weekLabel.Content.Text = "WK2";
                                        break;
                                    case StaticData.WeekeNum.week4:
                                        StaticData.weekPrev = StaticData.WeekeNum.week3;
                                        UpdatePreview(3);
                                        weekLabel.Content.Text = "WK3";
                                        break;
                                }
                            });
                        })
                    });

                NextArrow.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                StaticData.previewPlanRefresh = true;
                                switch (StaticData.weekPrev)
                                {
                                    case StaticData.WeekeNum.week1:
                                        StaticData.weekPrev = StaticData.WeekeNum.week2;
                                        UpdatePreview(2);
                                        weekLabel.Content.Text = "WK2";
                                        break;
                                    case StaticData.WeekeNum.week2:
                                        if (StaticData.chosenWeeks > 2)
                                        {
                                            StaticData.weekPrev = StaticData.WeekeNum.week3;
                                            UpdatePreview(3);
                                            weekLabel.Content.Text = "WK3";
                                        }
                                        else
                                        {
                                            StaticData.weekPrev = StaticData.WeekeNum.week1;
                                            UpdatePreview(1);
                                            weekLabel.Content.Text = "WK1";
                                        }
                                        break;
                                    case StaticData.WeekeNum.week3:
                                        if (StaticData.chosenWeeks > 3)
                                        {
                                            StaticData.weekPrev = StaticData.WeekeNum.week4;
                                            UpdatePreview(4);
                                            weekLabel.Content.Text = "WK4";
                                        }
                                        else
                                        {
                                            StaticData.weekPrev = StaticData.WeekeNum.week1;
                                            UpdatePreview(1);
                                            weekLabel.Content.Text = "WK1";
                                        }
                                        break;
                                    case StaticData.WeekeNum.week4:
                                        if (StaticData.chosenWeeks == 4)
                                        {
                                            StaticData.weekPrev = StaticData.WeekeNum.week1;
                                            UpdatePreview(1);
                                            weekLabel.Content.Text = "WK1";
                                        }
                                        break;
                                }
                            });
                        })
                    });
            }

            Grid weekGrid = new Grid
            {
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                WidthRequest = Units.ScreenWidth - Units.ScreenWidth10Percent,
                //BackgroundColor = Color.Red,
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto)},
                },
                Children =
                {
                    { LastArrow.Content, 0, 0},
                    { weekLabel.Content, 1, 0 },
                    { NextArrow.Content, 2, 0 }
                }
            };

            Grid seperator = new Grid { WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) };
            StackLayout spacer = new StackLayout
            {
                HeightRequest = 10
            };

            Grid seperator2 = new Grid { WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) };
            StackLayout spacer2 = new StackLayout
            {
                HeightRequest = 10
            };

            StackLayout masterCont = new StackLayout
            {
                HeightRequest = Units.ScreenHeight - Units.ScreenHeight10Percent,
                WidthRequest = Units.ScreenWidth - Units.ScreenWidth10Percent,
                Spacing = 0,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING * 2,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    CloseLabel.Content,
                    subjectLabel.Content,
                    mealPlanLabel.Content,
                    seperator,
                    spacer,
                    BuildCollectionView(1, name),
                    spacer2,
                    seperator2,
                    weekGrid,
                }
            };

            Frame frame = new Frame
            {
                Content = masterCont,
                CornerRadius = 5,
                IsClippedToBounds = true,
                Padding = 0,
            };

            Content.Children.Add(frame);
        }

        public CollectionView BuildCollectionView(int week, string mealPlanName)
        {
            try
            {
                previewCollectionView = new PreviewCollectionView();
                previewLayout = previewCollectionView.GetCollectionView();
                previewCollectionView.ShowCalendar(week.ToString(), mealPlanName);
                return previewLayout;
            }
            catch
            {
                return previewLayout;
            }
        }

        public async void UpdatePreview(int chosenWeek)
        {
            if (!isBusy)
            {
                isBusy = true;
                var result = await App.ApiBridge.GetInfluencerMealPlanWeek(AppSession.CurrentUser, week.ToString());
                StaticData.storedInfluencerMealPlan = new StoredInfluencerMealPlan();
                StaticData.storedInfluencerMealPlan.mealPlanModel = result;
                StaticData.storedInfluencerMealPlan.week = chosenWeek.ToString();
                StaticData.storedInfluencerMealPlan.mealPlanName = name;
                var previewGroup = new PreviewCollectionViewSection(StaticData.storedInfluencerMealPlan.mealPlanModel.Data);
                AppSession.influencerMealPreviewCollection.Add(previewGroup);
                AppSession.influencerMealPreviewCollection.RemoveAt(0);
                isBusy = false;
            }
        }
    }
}
