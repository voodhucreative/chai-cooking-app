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
    public class MealMenu : Page
    {
        // declare all components for this page
        StackLayout ContentContainer;

        // top logo
        protected StaticImage Logo;

        // themed header
        protected Layouts.Custom.Header Header;

        // Title
        protected Layouts.Custom.PageTitle Title;

        protected StandardButton NextButton;

        protected TiledList MealList;

        protected int TilesPerRow = 1;

        StackLayout FilterBar;
        protected StaticLabel NumberOfLocalRestaurants;
        protected ActiveLabel SearchFilter; // replace with custom component
        protected ActiveImage FilterIcon;

        protected MealSearchBox MealSearchBox;

        protected RestaurantSummaryLayout RestaurantSummary;

        public MealMenu()
        {
            this.IsScrollable = true;
            this.FullScreen = true;
            //this.Id = (int)AppSettings.PageNames.MealMenu;
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
            Header.HideDivider();
            Header.SetHeight(Units.ScreenHeight10Percent);

            Title = new Layouts.Custom.PageTitle("Meal Menu", Color.White, Color.Black);

            FilterBar = new StackLayout
            {
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.TapSizeS,
                BackgroundColor = Color.Black,
                Orientation = StackOrientation.Horizontal
            };

            NumberOfLocalRestaurants = new StaticLabel("Info");
            NumberOfLocalRestaurants.Content.HorizontalOptions = LayoutOptions.Start;
            NumberOfLocalRestaurants.Content.HorizontalTextAlignment = TextAlignment.Start;
            NumberOfLocalRestaurants.Content.VerticalOptions = LayoutOptions.Center;
            NumberOfLocalRestaurants.Content.VerticalTextAlignment = TextAlignment.Center;
            NumberOfLocalRestaurants.Content.WidthRequest = Units.ScreenWidth45Percent;
            NumberOfLocalRestaurants.Content.Margin = new Thickness(Units.ScreenUnitM, 0);
            NumberOfLocalRestaurants.Content.FontFamily = TechExpo.Helpers.Fonts.GetFont(FontName.MuliBold);
            NumberOfLocalRestaurants.Content.TextColor = Color.White;

            SearchFilter = new ActiveLabel("Display Only", 10, FontName.MontserratRegular, Color.Transparent, Color.White, null);
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

            MealSearchBox = new MealSearchBox();
            MealSearchBox.Content.Margin = new Thickness(Units.ScreenUnitL, 0);

            MealList = new TiledList(TilesPerRow);

            foreach (Meal meal in FakeData.Meals)
            {
                MealLayout mealLayout = new MealLayout(meal);
                mealLayout.Content.WidthRequest = Units.ScreenWidth / TilesPerRow;
                mealLayout.Content.HeightRequest = Units.TapSizeXL;
                mealLayout.Content.BackgroundColor = Color.White;

                Tile tile = new Tile();
                tile.DefaultAction = new Models.Action((int)Actions.ActionName.ToggleHeader, -1);
                tile.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                AppSession.CurrentUser.CurrentOrder.Meals.Add(meal);
                                Console.WriteLine("Chosen: " + meal.Name + " id: " + meal.Id);

                                Customer c = AppSession.CurrentUser;

                                await tile.DefaultAction.Execute();
                            });
                        })
                    }
                );
                tile.AddContent(mealLayout.Content);
                MealList.AddTile(tile);
            }

            NextButton = new StandardButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Branding.Colors.PINK), Color.White, "Next", null);

            //NextButton.UpAction = new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.OptionsMenu);

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

            Restaurant testRest = new Restaurant(0, 0, "Deli No 1", "Cracking place. Well tasty!", "deli1.png", 5);
            RestaurantSummary = new RestaurantSummaryLayout(testRest);




            ContentContainer.Children.Add(Header.Content);
            //ContentContainer.Children.Add(Title.Content);
            ContentContainer.Children.Add(FilterBar);
            ContentContainer.Children.Add(RestaurantSummary.Content);
            ContentContainer.Children.Add(MealSearchBox.Content);
            ContentContainer.Children.Add(MealList.Content);
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
