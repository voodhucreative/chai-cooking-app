using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using XamForms.Controls;

namespace ChaiCooking.Layouts.Custom.Modals
{
    class CalendarModal : StandardLayout
    {
        Influencer.Datum influencer;
        StackLayout masterContainer, calendarContainer;
        Calendar calendar;
        public CalendarModal(Influencer.Datum influencerInput, string planLength)
        {
            Container = new Grid { }; Content = new Grid { };

            masterContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Spacing = 0
            };

            influencer = influencerInput;

            calendar = new Calendar
            {
                BorderColor = Color.Orange,
                OuterBorderWidth = 0,
                BorderWidth = 0,
                BackgroundColor = Color.FromHex(Colors.CC_ORANGE),
                StartDay = DayOfWeek.Sunday,
                StartDate = DateTime.Now,
                SelectedBorderWidth = 1,
                SelectedBorderColor = Color.Black,
                DatesBackgroundColor = Color.White,
                DatesBackgroundColorOutsideMonth = Color.LightGray,
                DatesFontAttributesOutsideMonth = FontAttributes.Bold,
                WeekdaysBackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                DatesTextColor = Color.Black,
                DisabledBackgroundColor = Color.White,
                DisabledFontSize = Units.FontSizeM,
                DisabledTextColor = Color.DarkGray,
                DisabledBorderColor = Color.White,
                TitleLabelTextColor = Color.White,
                DatesTextColorOutsideMonth = Color.Black,
                DatesFontAttributes = FontAttributes.Bold,
                DatesFontSize = Units.FontSizeM,
                WeekdaysTextColor = Color.White,
                WeekdaysFontAttributes = FontAttributes.Bold,
                TitleLeftArrowTextColor = Color.Black,
                TitleRightArrowTextColor = Color.Black,
                DisableAllDates = true
            };

            calendar.OnStartRenderCalendar += Calendar_OnStartRenderCalendar;
            calendar.DateClicked += Calendar_DateClicked;

            var startDate = StaticData.monthlyMealPlan.week1.Data[0].date;
            DateTime minimumDate = DateTime.Parse(startDate);

            for (DateTime i = minimumDate; i < minimumDate.AddDays(StaticData.chosenWeeks * 7); i = i.AddDays(1))
            {
                // Week 1
                if (i < minimumDate.AddDays(7))
                {
                    calendar.SpecialDates.Add(new SpecialDate(i)
                    {
                        TextColor = Color.Black,
                        Selectable = true,
                        FontSize = 20,
                        BackgroundPattern = new BackgroundPattern(1)
                        {
                            Pattern = new List<Pattern>{new Pattern{ WidthPercent = 1f, HightPercent = 0.25f, Color = Color.FromHex(Colors.CC_ORANGE),
                            TextColor = Color.White, Text = "Week 1", TextSize = 11, TextAlign=TextAlign.CenterTop}}
                        }
                    });
                }
                // Week 2
                if (i < minimumDate.AddDays(14))
                {
                    calendar.SpecialDates.Add(new SpecialDate(i)
                    {
                        TextColor = Color.Black,
                        Selectable = true,
                        FontSize = 20,
                        BackgroundPattern = new BackgroundPattern(1)
                        {
                            Pattern = new List<Pattern>{new Pattern{ WidthPercent = 1f,HightPercent = 0.25f, Color = Color.FromHex(Colors.CC_DARK_ORANGE),
                            TextColor = Color.White, Text = "Week 2", TextSize = 11, TextAlign=TextAlign.CenterTop}}
                        }
                    });
                }
                // Week 3
                if (i < minimumDate.AddDays(21))
                {
                    calendar.SpecialDates.Add(new SpecialDate(i)
                    {
                        TextColor = Color.Black,
                        Selectable = true,
                        FontSize = 20,
                        BackgroundPattern = new BackgroundPattern(1)
                        {
                            Pattern = new List<Pattern>{new Pattern{ WidthPercent = 1f, HightPercent = 0.25f, Color = Color.FromHex(Colors.CC_ORANGE),
                            TextColor = Color.White, Text = "Week 3", TextSize = 11, TextAlign=TextAlign.CenterTop}}
                        }
                    });
                }
                // Week 4
                if (i < minimumDate.AddDays(28))
                {
                    calendar.SpecialDates.Add(new SpecialDate(i)
                    {
                        TextColor = Color.Black,
                        Selectable = true,
                        FontSize = 20,
                        BackgroundPattern = new BackgroundPattern(1)
                        {
                            Pattern = new List<Pattern>{new Pattern{ WidthPercent = 1f, HightPercent = 0.25f, Color = Color.FromHex(Colors.CC_DARK_ORANGE),
                            TextColor = Color.White, Text = "Week 4", TextSize = 11, TextAlign=TextAlign.CenterTop}}
                        }
                    });
                }
            }
            StackLayout closeContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                VerticalOptions = LayoutOptions.End,
                HeightRequest = Units.ScreenWidth5Percent,
                WidthRequest = Units.ScreenWidth,
                BackgroundColor = Color.FromHex(Colors.CC_ORANGE)
            };

            Label closeLabel = new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = Units.FontSizeM,
                TextColor = Color.White,
                Text = "Close",
                HorizontalTextAlignment = TextAlignment.End
            };

            TouchEffect.SetNativeAnimation(closeLabel, true);
            TouchEffect.SetCommand(closeLabel,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.HideCalendar();
                });
            }));

            closeContainer.Children.Add(closeLabel);
            masterContainer.Children.Add(closeContainer);
            masterContainer.Children.Add(calendar);
            Container.Children.Add(masterContainer);
            Content.Children.Add(Container);
        }

        private void Calendar_DateClicked(object sender, DateTimeEventArgs e)
        {
            var clickedDate = e.DateTime.Date.ToString("M-d-yyyy");
            var hyphenedDate = e.DateTime.Date.ToString("yyyy-MM-dd");
            //var subText = pos.Substring(1, pos - 1);
            var resultWeek1 = (from x in StaticData.monthlyMealPlan.week1.Data
                               where x.date != null && x.date == clickedDate
                               select x).Count();
            var resultWeek2 = (from x in StaticData.monthlyMealPlan.week2.Data
                               where x.date != null && x.date == clickedDate
                               select x).Count();
            var resultWeek3 = (from x in StaticData.monthlyMealPlan.week3.Data
                               where x.date != null && x.date == clickedDate
                               select x).Count();
            var resultWeek4 = (from x in StaticData.monthlyMealPlan.week4.Data
                               where x.date != null && x.date == clickedDate
                               select x).Count();
            int i = 0;
            if (resultWeek1 == 1)
            {
                AddWeek(hyphenedDate);
                foreach (Week.Datum x in StaticData.monthlyMealPlan.week1.Data)
                {
                    x.mealTemplates = StaticData.previewMealPlan.Data[i].mealTemplates;
                    i++;
                }
                i = 0;
                Console.WriteLine("Week 1 Meal Plan set.");
                App.HideCalendar();
            }

            if (resultWeek2 == 1)
            {
                AddWeek(hyphenedDate);
                foreach (Week.Datum x in StaticData.monthlyMealPlan.week2.Data)
                {
                    x.mealTemplates = StaticData.previewMealPlan.Data[i].mealTemplates;
                    i++;
                }
                i = 0;
                Console.WriteLine("Week 2 Meal Plan set.");
                App.HideCalendar();
            }

            if (resultWeek3 == 1)
            {
                AddWeek(hyphenedDate);
                foreach (Week.Datum x in StaticData.monthlyMealPlan.week3.Data)
                {
                    x.mealTemplates = StaticData.previewMealPlan.Data[i].mealTemplates;
                    i++;
                }
                i = 0;
                Console.WriteLine("Week 3 Meal Plan set.");
                App.HideCalendar();
            }

            if (resultWeek4 == 1)
            {
                AddWeek(hyphenedDate);
                foreach (Week.Datum x in StaticData.monthlyMealPlan.week4.Data)
                {
                    x.mealTemplates = StaticData.previewMealPlan.Data[i].mealTemplates;
                    i++;
                }
                i = 0;
                Console.WriteLine("Week 4 Meal Plan set.");
                App.HideCalendar();
            }
            Console.WriteLine();
        }

        private void Calendar_OnStartRenderCalendar(object sender, EventArgs e)
        {
        }

        public async void AddWeek(string date)
        {
            await App.ApiBridge.AddWeek(AppSession.CurrentUser, date);
        }

    }
}
