using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom;
using ChaiCooking.Models.Custom.MealPlanAPI;
using ChaiCooking.Services;
using ChaiCooking.Views.CollectionViews.Calendar;
using FFImageLoading.Transformations;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class Calendar : Page
    {
        UserMealTemplate userMealTemplate;

        Grid mealGrid;
        MealPlannerUserRow mealPlannerUserRow;
        DatePicker startDatePicker;
        int mealPlanId;
        Grid topGrid;
        string dateString;
        Color todayColor;
        Label todayLabel;
        //ActiveImage repopulate;
        CollectionView calendarLayout;
        CalendarCollectionView calendarCollectionView;
        bool isUpdating = false;

        bool gettingCalendar = false;

        static string selectedDate = "";

        public Calendar()
        {
            this.IsScrollable = false;
            this.IsRefreshable = false;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;

            this.Id = (int)AppSettings.PageNames.Calendar;
            this.Name = AppData.AppText.CALENDAR;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.SlideInFromRight;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.SlideOutToRight;

            PageContent = new Grid
            {
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY)
            };
            dateString = DateTime.Today.ToString("yyyy-MM-dd");
            selectedDate = dateString;
            BuildPage();
        }

        public StackLayout BuildPage()
        {
            PageContent.Children.Clear();
            StaticData.isEditing = false;

            Color tintColour = Color.LightGray;
            TintTransformation colorTint = new TintTransformation
            {
                HexColor = (string)tintColour.ToHex(),
                EnableSolidColor = true

            };
            var tint = new List<FFImageLoading.Work.ITransformation>();

            StaticImage LastArrow = new StaticImage("chevronleftbold.png", 48, tint);
            LastArrow.Content.HorizontalOptions = LayoutOptions.Start;
            LastArrow.Content.VerticalOptions = LayoutOptions.Center;
            LastArrow.Content.HeightRequest = 20;
            StaticImage NextArrow = new StaticImage("chevronrightbold.png", 48, tint);
            NextArrow.Content.HorizontalOptions = LayoutOptions.End;
            NextArrow.Content.VerticalOptions = LayoutOptions.Center;
            NextArrow.Content.HeightRequest = 20;

            TouchEffect.SetNativeAnimation(LastArrow.Content, true);
            TouchEffect.SetCommand(LastArrow.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    GetLast();
                });
            }));

            TouchEffect.SetNativeAnimation(NextArrow.Content, true);
            TouchEffect.SetCommand(NextArrow.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    GetNext();
                });
            }));

            Label startLabel = new Label
            {
                Text = "Date:",
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                FontFamily = Fonts.GetRegularAppFont(),
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = Units.FontSizeL,
                Padding = 5,
            };

            startDatePicker = new DatePicker
            {
                Date = DateTime.Now,
                TextColor = Color.FromHex(Colors.CC_ORANGE),
                BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY),
                Format = "dd/MM/yyyy",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                WidthRequest = Units.ScreenWidth,
                FontFamily = Fonts.GetBoldAppFont(),
            };

            startDatePicker.DateSelected += OnDateSelected;

            if (startDatePicker.Date != DateTime.Today)
            {
                todayColor = Color.Orange;
            }
            else
            {
                todayColor = Color.Transparent;
            }

            todayLabel = new Label
            {
                Text = "TODAY",
                FontAttributes = FontAttributes.Bold,
                TextColor = todayColor,
                FontFamily = Fonts.GetBoldAppFont(),
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = Units.FontSizeL,
            };

            TouchEffect.SetNativeAnimation(todayLabel, true);
            TouchEffect.SetCommand(todayLabel,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    //Added this so user cannot reload the week when they are already on today
                    if (startDatePicker.Date != DateTime.Today)
                    {
                        GetToday();
                    }
                });
            }));

            StackLayout todayLabelCont = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Padding = 5,
                Children =
                {
                    todayLabel
                }
            };


            //this.repopulate = new ActiveImage("redo.png", Units.TapSizeXS, Units.TapSizeXS, null, null);
            //repopulate.Content.Opacity = 0;
            //repopulate.Content.Margin = new Thickness(Units.MarginXS);

            //TouchEffect.SetNativeAnimation(repopulate.Content, true);
            //TouchEffect.SetCommand(repopulate.Content,
            //new Command(() =>
            //{
            //    Device.BeginInvokeOnMainThread(async () =>
            //    {
            //        await App.DisplayAlert("Re-populate", "Not happy with your meals? If you want to edit your calendar thats ok, but please be aware it could reduce the effectivness of your plan.", "Ok");
            //        bool success = await App.ApiBridge.RegenerateMeals(AppSession.CurrentUser);
            //        if (success)
            //        {
            //            UpdateCalendar();
            //        }
            //    });
            //}));




            topGrid = new Grid
            {
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY),
                RowSpacing = 0,
                ColumnSpacing = 0,
                HorizontalOptions = LayoutOptions.Center,
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                },
                ColumnDefinitions =
                {
                    { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto)}},
                    { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto)}},
                    { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)}},
                    { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto)}},
                    { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto)}},
                    //{ new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto)}},

                },
                Children =
                {
                    { LastArrow.Content, 0, 0 },
                    { startLabel, 1 , 0 },
                    { startDatePicker, 2, 0 },
                    { todayLabelCont, 3, 0 },
                    { NextArrow.Content, 4, 0 },
                    //{ repopulate.Content, 5, 0 }
                }
            };

            if (App.IsSmallScreen())
            {
                topGrid.ColumnDefinitions.Clear();
                topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(32) });
                topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
                topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(32) });
            }


            mealGrid = new Grid
            {
                ColumnSpacing = 0,
                RowSpacing = 0,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Star)}},
                },
                Children =
                {
                    { topGrid, 0, 0 },
                    { BuildCalendarContent(), 0, 1 }
                }
            };

            StackLayout pageContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    mealGrid
                }
            };

            PageContent.Children.Add(pageContainer);
            return pageContainer;
        }

        public override Task Update()
        {
            gettingCalendar = true;
            UpdateCalendar();
            StaticData.calendarClicked = false;
            return base.Update();
        }

        public override Task TransitionIn()
        {
            GetToday();
            //Dirty animation
            Task.Run(async () =>
            {
                while (gettingCalendar)
                {
                    await Task.Delay(50);
                }

                while (calendarLayout.Opacity < 1)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        calendarLayout.Opacity += 0.05f;
                    });
                    await Task.Delay(50);
                }
            });
            AppSession.CurrentUser.ReorderInternalCalendarPlans();
            return base.TransitionIn();
        }

        public override Task TransitionOut()
        {
            calendarLayout.Opacity = 0;
            return base.TransitionOut();
        }

        void OnDateSelected(object sender, DateChangedEventArgs args)
        {
            dateString = startDatePicker.Date.ToString("yyyy-MM-dd");
            selectedDate = dateString;
            UpdateCalendar();
        }

        public void GetLast()
        {
            if (BusyCheck())
            {
                App.ShowAlert("Please wait...");
            }
            else
            {
                DateTime newDate = startDatePicker.Date.AddDays(-7);
                dateString = newDate.ToString("yyyy-MM-dd");
                selectedDate = dateString;
                startDatePicker.Date = newDate;
                //UpdateCalendar();
            }
        }

        public void GetNext()
        {
            if (BusyCheck())
            {
                App.ShowAlert("Please wait...");
            }
            else
            {
                DateTime newDate = startDatePicker.Date.AddDays(7);
                dateString = newDate.ToString("yyyy-MM-dd");
                selectedDate = dateString;
                startDatePicker.Date = newDate;
                //UpdateCalendar();
            }
        }

        public void GetToday()
        {
            if (BusyCheck())
            {
                App.ShowAlert("Please wait...");
            }
            else
            {
                dateString = DateTime.Today.ToString("yyyy-MM-dd");
                selectedDate = dateString;
                startDatePicker.Date = DateTime.Today;
            }
        }
        
        private CollectionView BuildCalendarContent()
        {
            try
            {
                calendarCollectionView = new CalendarCollectionView();
                calendarLayout = calendarCollectionView.GetCollectionView();
                //calendarCollectionView.ShowCalendar();
                return calendarLayout;
            }
            catch
            {
                return calendarLayout;
            }
        }

        private async void UpdateCalendar()
        {
            if (!AppSession.calendarTokenSource.IsCancellationRequested)
            {
                //if (AppSession.calendarCollection != null)
                //{
                await App.ShowLoading();
                mealGrid.Opacity = 0.25f;
                await Task.Delay(100);
                isUpdating = true;
                if (startDatePicker.Date != DateTime.Today)
                {
                    todayColor = Color.Orange;
                }
                else
                {
                    todayColor = Color.Transparent;
                }
                todayLabel.TextColor = todayColor;

                //if (startDatePicker.Date >= DateTime.Now.Date.AddDays(7) && startDatePicker.Date < DateTime.Now.Date.AddDays(14))
                //{
                //    repopulate.Content.Opacity = 1;
                //}
                //else
                //{
                //    repopulate.Content.Opacity = 0;
                //}

                await Task.Delay(10);

                AppSession.calendar = await App.ApiBridge.GetCalendar(AppSession.CurrentUser, dateString, AppSession.calendarTokenSource.Token);
                gettingCalendar = false;
                var calendarGroup = new CalendarCollectionViewSection(AppSession.calendar.Data);
                AppSession.calendarCollection.Clear();
                AppSession.calendarCollection.Add(calendarGroup);
                AppSession.calendarCollectionView.ItemsSource = AppSession.calendarCollection;
                isUpdating = false;
                selectedDate = dateString;
                //}
                await App.HideLoading();
                await mealGrid.FadeTo(1, 100, Easing.SinIn);
            }
            else
            {
                AppSession.calendarTokenSource = new CancellationTokenSource();
            }
        }

        public static async void ForceCalendarRefresh()
        {
            AppSession.calendarTokenSource = new CancellationTokenSource();
            AppSession.calendar = await App.ApiBridge.GetCalendar(AppSession.CurrentUser, selectedDate, AppSession.calendarTokenSource.Token);
            var calendarGroup = new CalendarCollectionViewSection(AppSession.calendar.Data);
            AppSession.calendarCollection.Clear();
            AppSession.calendarCollection.Add(calendarGroup);
            AppSession.calendarCollectionView.ItemsSource = AppSession.calendarCollection;
        }

        private bool BusyCheck()
        {
            if (isUpdating)
            {
                AppSession.calendarTokenSource.Cancel();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
