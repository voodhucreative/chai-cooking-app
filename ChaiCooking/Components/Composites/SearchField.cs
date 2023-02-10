using System;
using ChaiCooking.Components.Fields;
using ChaiCooking.Components.Images;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using Xamarin.Forms;
using XFShapeView;

namespace ChaiCooking.Components.Composites
{
    public class SearchField
    {
        public Grid Content { get; set; }
        public StackLayout Container { get; set; }

        public string PlaceHolder { get; set; }
        public bool IsFuzzy { get; set; }

        public ShapeView SearchBoxShape { get; set; }
        public StaticImage Icon { get; set; }
        public SimpleInputField TextInput { get; set; }

        public SearchField(string placeholder)
        {
            Content = new Grid
            {
                BackgroundColor = Color.Transparent,
                WidthRequest = Dimensions.SEARCH_INPUT_WIDTH,
            };

            Frame searchBox = CreateSearchBox(placeholder, null, new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {

                    await App.PerformRecipeSearch(TextInput.TextEntry.Text);
                    await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.SearchResults);
                    TextInput.TextEntry.Text = "";
                    //SearchKeywords
                });
            }));

            Content.Children.Add(searchBox);
        }

        // Use this only for Add/Edit Meals menu
        public SearchField(string placeholder, Action updateResults)
        {
            Content = new Grid
            {
                BackgroundColor = Color.Transparent,
                WidthRequest = Dimensions.SEARCH_INPUT_WIDTH,
            };

            Frame searchBox = CreateSearchBox(placeholder, updateResults,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.PerformRecipeSearch(TextInput.TextEntry.Text);
                        updateResults();
                        //SearchKeywords
                    });
                }));

            Content.Children.Add(searchBox);
        }

        public Frame CreateSearchBox(string placeholder, Action updateResults, Command iconCommand)
        {
            TextInput = new SimpleInputField($"{placeholder}", Keyboard.Plain);
            TextInput.Content.HeightRequest = Dimensions.SEARCH_INPUT_HEIGHT;
            TextInput.Content.WidthRequest = Dimensions.SEARCH_INPUT_WIDTH;
            TextInput.Content.HorizontalOptions = LayoutOptions.StartAndExpand;
            TextInput.Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            TextInput.TextEntry.FontSize = Units.FontSizeL;
            TextInput.TextEntry.HorizontalTextAlignment = TextAlignment.Center;
            //Center Text Vertically and force vertical options as they are set in its class
            TextInput.TextEntry.VerticalTextAlignment = TextAlignment.Center;
            TextInput.TextEntry.VerticalOptions = LayoutOptions.Center;

            Icon = new StaticImage("searchicon.png", Dimensions.SEARCH_INPUT_ICON_WIDTH, Dimensions.SEARCH_INPUT_ICON_WIDTH, null);
            Icon.Content.Opacity = 0.5;
            Icon.Content.Margin = new Thickness(4, 0);
            Icon.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = iconCommand
                    }
                );
            Icon.CenterAlignVertical();

            StackLayout searchContainer = new StackLayout
            {
                BackgroundColor = Color.Transparent,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = Dimensions.SEARCH_INPUT_HEIGHT,
                WidthRequest = Dimensions.SEARCH_INPUT_WIDTH,
                Margin = new Thickness(4, 0, 0, 0),
                Children =
                {
                    Icon.Content,
                    TextInput.Content
                }
            };

            Frame searchFrame = new Frame
            {
                Content = searchContainer,
                CornerRadius = 20,
                BackgroundColor = Color.White,
                Padding = 0,
                HeightRequest = Dimensions.SEARCH_INPUT_HEIGHT,
                WidthRequest = Dimensions.SEARCH_INPUT_WIDTH,
            };

            //Focuses on the entry even if we dont quite hit it.
            searchFrame.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    TextInput.TextEntry.Focus();
                })
            });

            return searchFrame;
        }
    }
}
