using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom.InfluencerAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Lists
{
    class InfluencerMealPlanList : ActiveComponent
    {
        Grid masterGrid;
        StackLayout horizontalContainer, listContainer;
        Label createdLabel, addedByLabel, planLengthLabel;
        public Label title { get; set; }
        public Label createdLabelName { get; set; }
        public Label addedByLabelDate { get; set; }
        public Label planLengthLabelWeeks { get; set; }
        Color frameColour = Color.White;
        ColourButton addButton, previewButton;

        public InfluencerMealPlanList(InfluencerMealPlans.Datum datum)
        {
            masterGrid = new Grid
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                ColumnSpacing = 0,
                RowSpacing = 0,
                //BackgroundColor = Color.Purple,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                WidthRequest = Units.ScreenWidth
            };

            masterGrid.RowDefinitions.Add(new RowDefinition { Height = Units.ScreenHeight5Percent });
            masterGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            title = new Label
            {
                Text = datum.name,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                FontAttributes = FontAttributes.Bold,
                FontSize = Units.FontSizeL,
                TextColor = Color.White,
            };

            StackLayout titleContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    title
                }
            };


            addedByLabel = new Label
            {
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                FontSize = Units.FontSizeM,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                Text = "Added: ",
                TextColor = Color.White
            };

            addedByLabelDate = new Label
            {
                Text = datum.created_at.ToString("dd/MM/yyyy"),
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                FontAttributes = FontAttributes.Bold,
                FontSize = Units.FontSizeM,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
            };

            planLengthLabel = new Label
            {
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                FontSize = Units.FontSizeM,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                Text = "Plan Length: ",
                TextColor = Color.White
            };
            planLengthLabelWeeks = new Label
            {
                Text = datum.number_of_weeks.ToString() + " Weeks",
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = Units.FontSizeM,
                TextColor = Color.White
            };

            StackLayout labelContainer1 = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    //createdLabel,
                    //createdLabelName,
                    planLengthLabel,
                    planLengthLabelWeeks,
                    addedByLabel,
                    addedByLabelDate
                }
            };

            addButton = new ColourButton(Color.FromHex(Colors.CC_GREEN), Color.White, AppText.ADD_BUTTON_TEXT, null);
            addButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            addButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            addButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            TouchEffect.SetNativeAnimation(addButton.Content, true);
            TouchEffect.SetCommand(addButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("Add", "Tap Add and then select a Start Date by tapping on it and then confirm a date in the calendar. Tap the Create button to confirm your date.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                    }
                    else
                    {
                        StaticData.selectedPreviewMealPlanID = datum.id;
                        var result = await App.ShowStartDateMenu(AppSession.SelectedInfluencer, planLengthLabelWeeks.Text, datum.name);
                    }
                });
            }));

            previewButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.PREVIEW_BUTTON_TEXT, null);
            previewButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            previewButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            previewButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            TouchEffect.SetNativeAnimation(previewButton.Content, true);
            TouchEffect.SetCommand(previewButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("Preview", "Tap on Preview to see the influencer meal plan you’d like to view.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                    }
                    else
                    {
                        StaticData.selectedPreviewMealPlanID = datum.id;
                        StaticData.previewPlanRefresh = true;
                        StaticData.isTemplateView = true;

                        if (AppSession.BrowsePlans)
                        {
                            // browse route?
                            await App.ShowPreviewMealPlan(title.Text, planLengthLabelWeeks.Text);
                        }
                        else
                        {
                            // single influencer route
                            StaticData.callTemplateView(datum.name, datum.number_of_weeks);
                        }
                    }


                    
                });
            }));

            StackLayout btnContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    addButton.Content,
                    previewButton.Content
                }
            };

            listContainer = new StackLayout
            {
                BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY),
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    labelContainer1,
                    btnContainer
                }
            };

            horizontalContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = frameColour,
                Padding = new Thickness(2, 2, 2, 2),
                Children =
                {
                    masterGrid
                }
            };

            masterGrid.Children.Add(titleContainer, 0, 0);

            masterGrid.Children.Add(listContainer, 0, 1);

            Content.Children.Add(horizontalContainer);
        }
    }
}
