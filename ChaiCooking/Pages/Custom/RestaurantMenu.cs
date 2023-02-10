using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechExpo.Components.Buttons;
using TechExpo.Components.Composites;
using TechExpo.Components.Fields;
using TechExpo.Components.Images;
using TechExpo.Components.Labels;
using TechExpo.Components.Lists;
using TechExpo.Components.Tiles;
using TechExpo.DebugData.Custom;
using TechExpo.Helpers;
using TechExpo.Layouts.Custom;
using TechExpo.Models.Custom;
using TechExpo.Tools;
using Xamarin.Forms;

namespace TechExpo.Pages
{
    public class RestaurantMenu : Page
    {
        // declare all components for this page
        StackLayout ContentContainer;

        // top logo
        protected StaticImage Logo;

        // themed header
        protected Layouts.Custom.Header Header;

        // Title
        protected Layouts.Custom.PageTitle Title;
        protected StaticLabel Trending;
        protected StaticLabel PostCode;

        protected StandardButton NextButton;

        protected TiledList RestaurantList;

        protected TopSlider TrendingSlider;

        protected int TilesPerRow = 1;


        StackLayout TrendingBar;

        StackLayout FilterBar;
        protected StaticLabel NumberOfLocalRestaurants;
        protected ActiveLabel SearchFilter; // replace with custom component
        protected ActiveImage FilterIcon;

        public RestaurantMenu()
        {
            this.IsScrollable = true;
            this.FullScreen = true;
            //this.Id = (int)AppSettings.PageNames.RestaurantMenu;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.FadeIn;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.FadeOut;

            PageContent = new Grid
            {
                BackgroundColor = Color.White
            };

            // build labels
            ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Start
            };

            Header = new Layouts.Custom.Header(false);

            Title = new Layouts.Custom.PageTitle("Restaurant Menu", Color.White, Color.Black);

            Trending = new StaticLabel("Trending Food");
            Trending.Content.HorizontalOptions = LayoutOptions.Start;
            Trending.Content.HorizontalTextAlignment = TextAlignment.Start;
            Trending.Content.WidthRequest = Units.HalfScreenWidth;
            Trending.Content.Margin = new Thickness(Units.ScreenUnitM, 0);
            Trending.Content.FontFamily = TechExpo.Helpers.Fonts.GetFont(FontName.MuliBold);
            Trending.Content.TextColor = Color.Black;

            PostCode = new StaticLabel("HU5 3UW"); //AppSession.CurrentUser.Address.AreaCode);
            PostCode.Content.HorizontalOptions = LayoutOptions.End;
            PostCode.Content.HorizontalTextAlignment = TextAlignment.End;
            PostCode.Content.WidthRequest = Units.HalfScreenWidth;
            PostCode.Content.Margin = new Thickness(Units.ScreenUnitM, 0);
            PostCode.Content.FontFamily = TechExpo.Helpers.Fonts.GetFont(FontName.MuliRegular);

            TrendingBar = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                WidthRequest = Units.ScreenWidth
            };

            TrendingBar.Children.Add(Trending.Content);
            TrendingBar.Children.Add(PostCode.Content);

            TrendingSlider = new TopSlider();
            TrendingSlider.Clear();

            FilterBar = new StackLayout
            {
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.TapSizeS,
                BackgroundColor = Color.Black,
                Orientation = StackOrientation.Horizontal
            };

            NumberOfLocalRestaurants = new StaticLabel(FakeData.Restaurants.Count + " in Your Location");
            NumberOfLocalRestaurants.Content.HorizontalOptions = LayoutOptions.Start;
            NumberOfLocalRestaurants.Content.HorizontalTextAlignment = TextAlignment.Start;
            NumberOfLocalRestaurants.Content.VerticalOptions = LayoutOptions.Center;
            NumberOfLocalRestaurants.Content.VerticalTextAlignment = TextAlignment.Center;
            NumberOfLocalRestaurants.Content.WidthRequest = Units.ScreenWidth45Percent;
            NumberOfLocalRestaurants.Content.Margin = new Thickness(Units.ScreenUnitM, 0);
            NumberOfLocalRestaurants.Content.FontFamily = TechExpo.Helpers.Fonts.GetFont(FontName.MuliBold);
            NumberOfLocalRestaurants.Content.TextColor = Color.White;

            SearchFilter = new ActiveLabel("Search by Cuisine", 10, FontName.MontserratRegular, Color.Transparent, Color.White, null);
            SearchFilter.Content.HorizontalOptions = LayoutOptions.End;
            SearchFilter.Label.HorizontalTextAlignment = TextAlignment.End;
            SearchFilter.Label.HorizontalOptions = LayoutOptions.End;
            SearchFilter.Content.VerticalOptions = LayoutOptions.Center;
            SearchFilter.Content.WidthRequest = Units.QuarterScreenWidth;
            SearchFilter.Content.Margin = new Thickness(8, 0);

            FilterIcon = new ActiveImage("no_image.png", 16, 16, null, null);
            FilterIcon.Content.VerticalOptions = LayoutOptions.Center;
            FilterIcon.Content.HorizontalOptions = LayoutOptions.Start;
            SearchFilter.Content.Margin = new Thickness(8, 0, Units.ScreenUnitM, 0);

            FilterBar.Children.Add(NumberOfLocalRestaurants.Content);
            FilterBar.Children.Add(SearchFilter.Content);
            FilterBar.Children.Add(FilterIcon.Content);

            Restaurant testRestaurant1 = new Restaurant(0, 5, "Deli No 1", "Nice place for nice food", "deli1.png", 5);
            Restaurant testRestaurant2 = new Restaurant(1, 5, "Fountells", "Nice place for nice food", "fountells.png", 4);
            Restaurant testRestaurant3 = new Restaurant(2, 5, "Ladygate", "Nice place for nice food", "ladygate.png", 3);
            Restaurant testRestaurant4 = new Restaurant(3, 5, "Akash", "Nice place for nice food", "akash.png", 4);
            Restaurant testRestaurant5 = new Restaurant(4, 5, "Maa", "Nice place for nice food", "maa.png", 3);

            
            //testRestaurant1.


            TrendingSlider.AddItem(new TrendingSliderTileLayout(testRestaurant1).Content);
            TrendingSlider.AddItem(new TrendingSliderTileLayout(testRestaurant2).Content);
            TrendingSlider.AddItem(new TrendingSliderTileLayout(testRestaurant3).Content);
            TrendingSlider.AddItem(new TrendingSliderTileLayout(testRestaurant4).Content);
            TrendingSlider.AddItem(new TrendingSliderTileLayout(testRestaurant5).Content);


            RestaurantList = new TiledList(TilesPerRow);

            foreach (Restaurant restaurant in FakeData.Restaurants)
            {
                RestaurantLayout restaurantLayout = new RestaurantLayout(restaurant);
                restaurantLayout.Content.WidthRequest = Units.ScreenWidth / TilesPerRow;
                restaurantLayout.Content.HeightRequest = 92; //Units.TapSizeXL;
                restaurantLayout.Content.BackgroundColor = Color.White;

                Tile tile = new Tile();
                tile.DefaultAction = new Models.Action((int)Actions.ActionName.ToggleHeader, -1);
                tile.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                AppSession.CurrentUser.CurrentOrder.Restaurant = restaurant;
                                Console.WriteLine("Chosen: " + restaurant.Name + " id: " + restaurant.Id);
                                await tile.DefaultAction.Execute();
                            });
                        })
                    }
                );
                tile.AddContent(restaurantLayout.Content);
                RestaurantList.AddTile(tile);
            }


            NextButton = new StandardButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Branding.Colors.PINK), Color.White, "Next", null);

            //NextButton.UpAction = new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.MealMenu);

            Gestures NextGestures = new Gestures();
            NextGestures.TouchBegan += (s, e) =>
            {
                NextButton.ButtonShape.Color = Color.FromHex(Branding.Colors.DARK_PINK);
            };
            NextGestures.TouchEnded += (s, e) =>
            {
                NextButton.ButtonShape.Color = Color.FromHex(Branding.Colors.PINK);
                if (NextButton.UpAction != null)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await NextButton.UpAction.Execute();
                    });
                }
            };
            NextButton.Content.Children.Add(NextGestures, 0, 0);

            ContentContainer.Children.Add(Header.Content);
            //ContentContainer.Children.Add(Title.Content);
            ContentContainer.Children.Add(TrendingBar);
            ContentContainer.Children.Add(TrendingSlider.Content);
            ContentContainer.Children.Add(FilterBar);
            ContentContainer.Children.Add(RestaurantList.Content);
            ContentContainer.Children.Add(NextButton.Content);

            PageContent.Children.Add(ContentContainer);
        }

        public override void Destroy()
        {

        }

        public override async Task Update()
        {
            await DebugUpdate(AppSettings.TransitionVeryFast);
        }

        public override async Task DebugUpdate(int time)
        {
            await Task.Delay(time);
        }

        // Override these functions for any page specific transitions. 
        // Otherwise the default base Page class methods will be called instead.

        /*
        public override async Task TransitionIn()
        {
            await Task.WhenAll(
                PageContent.FadeTo(1, 500, Easing.Linear)
            ).ConfigureAwait(false);
        }

        public override async Task TransitionOut()
        {
            await Task.WhenAll(
                PageContent.FadeTo(0, 500, Easing.Linear)
            ).ConfigureAwait(false);
        }*/
    }
}
