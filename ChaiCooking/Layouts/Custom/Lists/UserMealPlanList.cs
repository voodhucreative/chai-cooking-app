using System;
using System.Collections.Generic;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Images;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom.InfluencerAPI;
using ChaiCooking.Models.Custom.MealPlanAPI;
using ChaiCooking.Services.Storage;
using ChaiCooking.Views.CollectionViews.CreateOrSelect;
using ChaiCooking.Views.CollectionViews.MealPlanner;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Lists
{
    public class UserMealPlanList : ActiveComponent
    {
        public Label titleLabel { get; set; }
        public Label addedByLabel { get; set; }
        public Label durationLabel { get; set; }
        public Label startDateLabel { get; set; }
        public Label endDateLabel { get; set; }
        public int id { get; set; }
        public int weeks { get; set; }
        public string name { get; set; }

        public UserMealPlanList(Datum datum, Action buildConfirm)
        {
            this.weeks = datum.numOfWeeks;
            this.name = datum.name;
            this.id = datum.id;

            List<View> views = new List<View>();
            views.Add(BuildLeftContainer(datum.name, datum.created_at.ToString("dd-MM-yyyy"),
                $"{ datum.numOfWeeks} WKS", datum.start_date.ToString("dd-MM-yyyy"), datum.end_date.ToString("dd-MM-yyyy"),
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        StaticData.mealPlanClicked = true;
                        AppSession.CurrentUser.defaultMealPlanID = id;
                        AppSession.CurrentUser.defaultMealPlanName = name;
                        AppSession.CurrentUser.defaultMealPlanWeeks = weeks;
                        LocalDataStore.SaveAll();
                        AppSession.mealPlannerCalendar = await App.ApiBridge.GetWeek(AppSession.CurrentUser, AppSession.CurrentUser.defaultMealPlanID.ToString(), "1", AppSession.mealPlanTokenSource.Token);
                        var mealPlannerGroup = new MealPlannerCollectionViewSection(AppSession.mealPlannerCalendar.Data);
                        AppSession.mealPlannerCollection.RemoveAt(0);
                        AppSession.mealPlannerCollection.Add(mealPlannerGroup);
                        AppSession.SetMealPlanner(titleLabel.Text, true, weeks, false);
                        await App.HideSelectOrCreateModal();
                        StaticData.isTemplate = false;
                    });
                })));

            views.Add(BuildRightContainer(new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    StaticData.mealPlanToDelete = this.id;
                    buildConfirm();
                });
            })));

            Content.Children.Add(BuildOuterContainer(views));
        }

        public UserMealPlanList(InfluencerMealPlans.Datum templates, Action buildConfirm)
        {
            List<View> views = new List<View>();
            views.Add(BuildLeftContainer(templates.name, templates.created_at.ToString("dd-MM-yyyy"),
                $"{ templates.number_of_weeks } WKS", "", "",
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.ShowLoading();
                        StaticData.currentTemplateID = templates.id;
                        StaticData.currentTemplateName = templates.name;
                        await StaticData.UpdateMealTemplate();
                        AppSession.SetMealPlanner(templates.name, true, 1, true);
                        await App.HideLoading();
                        await App.HideSelectOrCreateModal();
                        StaticData.isTemplate = true;
                    });
                }), true, templates.published_at));

            views.Add(BuildRightContainer(new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    StaticData.templateToDelete = templates.id;
                    buildConfirm();
                });
            })));

            Content.Children.Add(BuildOuterContainer(views));
        }

        public StackLayout BuildLeftContainer(string name,
            string createdAt, string numOfWeeks, string startDate, string endDate,
            Command command, bool isTemplate = false, string publishedStatus = "")
        {

            titleLabel = new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = Units.FontSizeXL,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                Text = name
            };

            StackLayout titleContainer = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Vertical,
                Padding = 5,
                Children =
                {
                    titleLabel
                }
            };

            string addedText = "Added: ";
            if (isTemplate)
            {
                addedText = "Status: ";
                publishedStatus = (publishedStatus == null ? "Unpublished" : "Published");
                createdAt = publishedStatus;
            }

            Label addedByTitle = new Label
            {
                Text = addedText,
                FontSize = Units.FontSizeM,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center
            };

            addedByLabel = new Label
            {
                FontSize = Units.FontSizeM,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                Text = createdAt
            };

            StackLayout addedByContainer = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation. Horizontal,
                Spacing = 0,
                Children =
                {
                    addedByTitle,
                    addedByLabel
                }
            };

            Label durationTitle = new Label
            {
                Text = "Plan Length:",
                FontSize = Units.FontSizeM,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center
            };

            durationLabel = new Label
            {
                FontSize = Units.FontSizeM,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                Text = numOfWeeks
            };

            StackLayout weeksContainer = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    durationTitle,
                    durationLabel
                }
            };

            StackLayout detailsContainer = new StackLayout
            {
                Padding = 5,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    addedByContainer,
                    weeksContainer
                }
            };

            startDateLabel = new Label
            {
                FontSize = Units.FontSizeM,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                Text = startDate,
            };


            string startDateText = ((isTemplate) ? "" : "Start Date:");

            Label startDateTitle = new Label
            {
                Text = startDateText,
                FontSize = Units.FontSizeM,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center
            };

            StackLayout startDateDetailsContainer = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    startDateTitle,
                    startDateLabel
                }
            };

            endDateLabel = new Label
            {
                FontSize = Units.FontSizeM,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                Text = endDate
            };

            string endDateText = ((isTemplate) ? "" : "End Date:");
            Label endDateTitle = new Label
            {
                Text = endDateText,
                FontSize = Units.FontSizeM,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center
            };

            StackLayout endDateDetailsContainer = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    endDateTitle,
                    endDateLabel
                }
            };

            StackLayout secondDetailsContainer = new StackLayout
            {
                Padding = 5,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    startDateDetailsContainer,
                    endDateDetailsContainer
                }
            };

            StackLayout leftContainer = new StackLayout
            {
                Padding = 1,
                Spacing = 1,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    titleContainer,
                    detailsContainer,
                    secondDetailsContainer
                }
            };

            leftContainer.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = command
                   }
               );

            return leftContainer;
        }

        public StackLayout BuildRightContainer(Command command)
        {
            Label iconLabel = new Label
            {
                Text = "Remove",
                FontSize = Units.FontSizeS,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold
            };

            StaticImage closeImage = new StaticImage("closecirclewhite.png", 25, 25, null);

            StackLayout imageContainer = new StackLayout
            {
                Margin = 5,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    closeImage.Content,
                    //iconLabel
                }
            };

            imageContainer.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = command
                   }
               );

            StackLayout rightContainer = new StackLayout
            {
                Padding = 1,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    imageContainer
                }
            };

            return rightContainer;
        }

        public StackLayout BuildOuterContainer(List<View> views)
        {
            StackLayout masterContainer = new StackLayout()
            {
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                Margin = 1,
                VerticalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                WidthRequest = Units.ScreenWidth
            };

            foreach (View v in views)
            {
                masterContainer.Children.Add(v);
            }

            StackLayout outerContainer = new StackLayout()
            {
                BackgroundColor = Color.White,
                Padding = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    masterContainer
                }
            };

            return outerContainer;
        }
    }
}
