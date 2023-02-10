using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.MealPlanAPI;
using ChaiCooking.Services.Storage;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.MealPlanner
{
    public class MealPlannerCollectionView : BindableObject
    {
        public MealPlannerCollectionView()
        {
            AppSession.mealPlannerCollection = new ObservableCollection<MealPlannerCollectionViewSection>();
            AppSession.mealPlannerCollectionView = new CollectionView
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
                ItemTemplate = new MealPlannerDataTemplateSelector(),
                ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    VerticalItemSpacing = 5,
                },
                GroupFooterTemplate = new DataTemplate(typeof(MealPlannerFooter))
                {
                    Bindings =
                    {
                        { MealPlannerFooter.FooterNotVisibleViewProperty, new Binding("FooterNotVisible") },
                        { MealPlannerFooter.FooterIsVisibleProperty, new Binding("FooterIsVisible") }
                    }
                },
                //EmptyView = BuildEmpty(),
            };
            //AppSession.mealPlannerCollection.Clear();
            AppSession.mealPlanTokenSource = new CancellationTokenSource();
        }

        public async void ShowCalendar(string week)
        {
            //await App.ApiBridge.DeleteUserMealPlan(AppSession.CurrentUser, "226");
            // GET USER MEAL PLANS
            StaticData.userMealPlans = await App.ApiBridge.GetUserMealPlans(AppSession.CurrentUser);
            
            // ARE THE MEAL PLANS EMPTY?
            if (StaticData.userMealPlans == null || StaticData.userMealPlans.Data == null || StaticData.userMealPlans.Data.Count == 0)
            {
                AppSession.mealPlanCheck = false; // YES NO MEAL PLANS
                // CREATE AN EMPTY OBJECT
                AppSession.mealPlannerCalendar = new UserMealTemplate();
                AppSession.mealPlannerCalendar.Data = new List<UserMealTemplate.Datum>();
                UserMealTemplate userMealTemplate = new UserMealTemplate();
                UserMealTemplate.Datum datum = new UserMealTemplate.Datum();
                datum.date = "EMPTY";
                // ADD THE EMPTY OBJECT
                AppSession.mealPlannerCalendar.Data.Add(datum);
            }
            else // WE HAVE MEAL PLANS
            {
                AppSession.mealPlanCheck = true;
                if (StaticData.userMealPlans.Data != null)
                {
                    try
                    {
                        var isID = StaticData.userMealPlans.Data.SingleOrDefault(x => x.id == AppSession.CurrentUser.defaultMealPlanID);
                        if (isID == null)
                        {
                            var newDefault = StaticData.userMealPlans.Data.First();
                            AppSession.CurrentUser.defaultMealPlanID = newDefault.id;
                            AppSession.CurrentUser.defaultMealPlanName = newDefault.name;
                            AppSession.CurrentUser.defaultMealPlanWeeks = newDefault.numOfWeeks;
                            StaticData.chosenWeeks = newDefault.numOfWeeks;
                        }
                    }
                    catch { }
                }
            }

            // DO WE HAVE ANY MEAL PLANS?
            if (AppSession.mealPlanCheck)
            {
                // YES
                AppSession.SetMealPlanner(AppSession.CurrentUser.defaultMealPlanName, true, AppSession.CurrentUser.defaultMealPlanWeeks, false);
                AppSession.mealPlannerCalendar = await App.ApiBridge.GetWeek(AppSession.CurrentUser, AppSession.CurrentUser.defaultMealPlanID.ToString(), week, AppSession.mealPlanTokenSource.Token);
            }
            else
            {
                // NO
                AppSession.SetMealPlanner("", false, 0, false);
            }

            await Task.Delay(10);

            // CREATE THE GROUP
            var mealPlannerGroup = new MealPlannerCollectionViewSection(AppSession.mealPlannerCalendar.Data);
            // ADD THE GROUP TO THE COLLECTION
            AppSession.mealPlannerCollection.Add(mealPlannerGroup);
            // APPLY THE ITEM SOURCE TO THE COLLECTION
            AppSession.mealPlannerCollectionView.ItemsSource = AppSession.mealPlannerCollection;
        }

        public CollectionView GetCollectionView()
        {
            return AppSession.mealPlannerCollectionView;
        }
    }
}
