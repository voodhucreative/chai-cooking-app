using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom.MealPlanAPI;
using Xamarin.Forms;

namespace ChaiCooking.Views.CollectionViews.CreateOrSelect
{
    public class CreateOrSelectCollectionView
    {
        public CreateOrSelectCollectionView(Action buildConfirm)
        {
            AppSession.createOrSelectCollection = new ObservableCollection<CreateOrSelectViewSection>();
            AppSession.createOrSelectCollectionView = new CollectionView
            {
                IsGrouped = true,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = Units.ScreenWidth,
                HeightRequest = 300,
                ItemsSource = AppSession.createOrSelectCollection,
                ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                BackgroundColor = Color.Transparent,
                SelectionMode = SelectionMode.None,
                ItemTemplate = new CreateOrSelectDataTemplateSelector(buildConfirm),
                ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    VerticalItemSpacing = 5,
                },
                GroupHeaderTemplate = new DataTemplate(typeof(CreateOrSelectViewHeader))
                {
                    Bindings =
                    {
                        {  CreateOrSelectViewHeader.EmptyViewProperty, new Binding("EmptyView")},
                        {  CreateOrSelectViewHeader.EmptyViewVisibleProperty, new Binding("EmptyViewIsVisible")}
                    }
                },
            };
            AppSession.createOrSelectCollection.Clear();
        }

        public async void ShowMealPlans()
        {
            //await App.ShowLoading();
            UserMealPlans tempList = StaticData.userMealPlans;
            await Task.Delay(10);
            if (StaticData.userMealPlans.Data != null)
            {
                try
                {
                    var matchID = StaticData.userMealPlans.Data.FirstOrDefault(x => x.id == AppSession.CurrentUser.defaultMealPlanID).id;
                    tempList.Data.RemoveAll(x => x.id == matchID);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error {e}");
                }
            }
            var createOrSelectGroup = new CreateOrSelectViewSection(tempList.Data);
            //When Modifiying a collection view, you must always ADD the new objects
            //BEFORE you REMOVE the old ones otherwise it will crash on iOS
            AppSession.createOrSelectCollection.Add(createOrSelectGroup);
            await Task.Delay(250);
            // AppSession.createOrSelectCollection.RemoveAt(0);
            //await App.HideLoading();
        }

        public async void ShowTemplates()
        {
            //await App.ShowLoading();
            await Task.Delay(10);
            StaticData.mealTemplates = App.ApiBridge.GetUserMealPlanTemplates(AppSession.CurrentUser).Result;
            var createOrSelectTemplateGroup = new CreateOrSelectViewSection(StaticData.mealTemplates.Data);
            AppSession.createOrSelectCollection.Add(createOrSelectTemplateGroup);
            await Task.Delay(250);
            // AppSession.createOrSelectCollection.RemoveAt(0);
            //await App.HideLoading();
        }

        public CollectionView GetCollectionView()
        {
            return AppSession.createOrSelectCollectionView;
        }

        private StackLayout BuildEmpty()
        {
            StackLayout emptyCont = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                Children =
                    {
                        new Label
                        {
                            Text = "No other meal plans found.",
                            FontSize = Units.FontSizeL,
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Color.White,
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                    }
            };

            return emptyCont;
        }
    }
}
