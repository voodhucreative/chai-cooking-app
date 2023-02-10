using System;
using System.Collections.ObjectModel;
using System.Threading;
using ChaiCooking.AppData;
using ChaiCooking.Helpers;
using ChaiCooking.Services.Storage;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.Calendar
{
    public class CalendarCollectionView : BindableObject
    {
        public CalendarCollectionView()
        {
            AppSession.calendarCollection = new ObservableCollection<CalendarCollectionViewSection>();
            AppSession.calendarCollectionView = new CollectionView
            {
                IsGrouped = true,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight,
                ItemsSource = null,
                ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                BackgroundColor = Color.Transparent,
                SelectionMode = SelectionMode.None,
                ItemTemplate = new CalendarDataTemplateSelector(),
                ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    VerticalItemSpacing = 5,
                },
            };

            AppSession.calendarTokenSource = new CancellationTokenSource();
        }

        public async void ShowCalendar()
        {
            string dateString = DateTime.Now.ToString("yyyy-MM-dd");
            AppSession.calendar = await App.ApiBridge.GetCalendar(AppSession.CurrentUser, dateString, AppSession.calendarTokenSource.Token);
            var calendarGroup = new CalendarCollectionViewSection(AppSession.calendar.Data);
            AppSession.calendarCollection.Add(calendarGroup);
            AppSession.calendarCollectionView.ItemsSource = AppSession.calendarCollection;
            AppSession.calendarTokenSource.Cancel();
        }

        public CollectionView GetCollectionView()
        {
            return AppSession.calendarCollectionView;
        }
    }
}
