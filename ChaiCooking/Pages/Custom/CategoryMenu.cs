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
    public class CategoryMenu : Page
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

        protected TiledList CatergoryList;

        protected TopSlider TopSlider;

        protected int TilesPerRow = 1;

        public CategoryMenu()
        {
            this.IsScrollable = true;
            this.FullScreen = true;
            //this.Id = (int)AppSettings.PageNames.CategoryMenu;
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

            Title = new Layouts.Custom.PageTitle("Category Menu", Color.White, Color.Black);

            ContentContainer.Children.Add(Header.Content);
            ContentContainer.Children.Add(Title.Content);

            Restaurant testRestaurant1 = new Restaurant(0, 5, "Deli No 1", "Nice place for nice food", "deli1.png", 5);
            Restaurant testRestaurant2 = new Restaurant(1, 5, "Fountells", "Nice place for nice food", "fountells.png", 4);
            Restaurant testRestaurant3 = new Restaurant(2, 5, "Ladygate", "Nice place for nice food", "ladygate.png", 3);
            Restaurant testRestaurant4 = new Restaurant(3, 5, "Akash", "Nice place for nice food", "akash.png", 4);
            Restaurant testRestaurant5 = new Restaurant(4, 5, "Maa", "Nice place for nice food", "maa.png", 3);


            TopSlider = new TopSlider();
            TopSlider.Clear();

            TopSlider.AddItem(new RestaurantSliderTileLayout(testRestaurant5).Content);
            TopSlider.AddItem(new RestaurantSliderTileLayout(testRestaurant4).Content);
            TopSlider.AddItem(new RestaurantSliderTileLayout(testRestaurant3).Content);
            TopSlider.AddItem(new RestaurantSliderTileLayout(testRestaurant2).Content);
            TopSlider.AddItem(new RestaurantSliderTileLayout(testRestaurant1).Content);

            CatergoryList = new TiledList(TilesPerRow);

            foreach(Category category in FakeData.Categories)
            {
                CategoryLayout categoryLayout = new CategoryLayout(category);
                categoryLayout.Content.WidthRequest = Units.ScreenWidth / TilesPerRow;
                categoryLayout.Content.HeightRequest = Units.TapSizeXL;
                categoryLayout.Content.BackgroundColor = Color.White;

                Tile tile = new Tile();
                tile.DefaultAction = new Models.Action((int)Actions.ActionName.ToggleHeader, -1);
                tile.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {

                                Console.WriteLine("Chosen: " + category.Name + " id: " + category.Id);
                                await tile.DefaultAction.Execute();
                            });
                        })
                    }
                );


                tile.AddContent(categoryLayout.Content);
                CatergoryList.AddTile(tile);
            }


            NextButton = new StandardButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Branding.Colors.PINK), Color.White, "Next", null);
            //NextButton.UpAction = new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.RestaurantMenu);

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
            ContentContainer.Children.Add(Title.Content);
            ContentContainer.Children.Add(TopSlider.Content);
            ContentContainer.Children.Add(CatergoryList.Content);
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
