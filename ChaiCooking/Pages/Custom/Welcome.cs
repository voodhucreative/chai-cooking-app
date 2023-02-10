using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechExpo.Branding;
using TechExpo.Components.Buttons;
using TechExpo.Components.Composites;
using TechExpo.Components.Fields;
using TechExpo.Components.Images;
using TechExpo.Components.Labels;
using TechExpo.Helpers;
using TechExpo.Layouts.Custom;
using TechExpo.Models.Custom;
using TechExpo.Tools;
using Xamarin.Forms;
using XFShapeView;

namespace TechExpo.Pages
{
    public class Welcome : Page
    {
        // declare all components for this page
        StackLayout ContentContainer;

        // top logo
        protected StaticImage Logo;

        // themed header
        protected Layouts.Custom.Header Header;

        // Title
        protected Layouts.Custom.PageTitle Title;

        protected ShapeView TopDivider;

        protected StandardButton NextButton;

        protected StaticLabel PlaceOrderLabel;
        protected StaticLabel PreviousOrderLabel;
        protected StaticLabel PreviouslyUsedLabel;

        protected LocationSearchBox LocationSeachBox;

        protected TopSlider PreviousOrdersSlider;
        protected TopSlider PreviouslyUsedSlider;


        public Welcome()
        {
            this.IsScrollable = true;
            this.FullScreen = true;
            //this.Id = (int)AppSettings.PageNames.Welcome;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.FadeIn;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.FadeOut;

            PageContent = new Grid
            {
                BackgroundColor = Color.White,
                //Padding = Units.ScreenUnitL
            };

            // build labels
            ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Start
            };

            Header = new Layouts.Custom.Header(false);

            Title = new Layouts.Custom.PageTitle("Welcome", Color.White, Color.Black);

            TopDivider = new ShapeView
            {
                ShapeType = ShapeType.Box,
                HeightRequest = 1,
                WidthRequest = Units.ScreenWidth,
                Color = Color.FromHex(Colors.DARK_GREY),
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(Units.ScreenUnitXS, 2),
                Opacity = 1
            };

            PlaceOrderLabel = new StaticLabel("Please place your order");
            PlaceOrderLabel.Content.HorizontalOptions = LayoutOptions.Center;
            PlaceOrderLabel.Content.FontSize = Units.FontSizeM;

            PreviousOrderLabel = new StaticLabel("Review your previous order");
            PreviousOrderLabel.Content.HorizontalOptions = LayoutOptions.Start;
            PreviousOrderLabel.Content.FontSize = Units.FontSizeL;
            PreviousOrderLabel.Content.FontFamily = Fonts.GetFont(FontName.MuliBold);

            PreviouslyUsedLabel = new StaticLabel("Previously used");
            PreviouslyUsedLabel.Content.HorizontalOptions = LayoutOptions.Start;
            PreviouslyUsedLabel.Content.FontSize = Units.FontSizeL;
            PreviouslyUsedLabel.Content.FontFamily = Fonts.GetFont(FontName.MuliBold);


            LocationSeachBox = new LocationSearchBox();


            PreviousOrdersSlider = new TopSlider();
            PreviousOrdersSlider.Clear();

           
            Restaurant testRestaurant1 = new Restaurant(0, 5, "Deli No 1", "Nice place for nice food", "deli1.png", 5);
            Restaurant testRestaurant2 = new Restaurant(1, 5, "Fountells", "Nice place for nice food", "fountells.png", 4);
            Restaurant testRestaurant3 = new Restaurant(2, 5, "Ladygate", "Nice place for nice food", "ladygate.png", 3);
            Restaurant testRestaurant4 = new Restaurant(3, 5, "Akash", "Nice place for nice food", "akash.png", 4);
            Restaurant testRestaurant5 = new Restaurant(4, 5, "Maa", "Nice place for nice food", "maa.png", 3);


            Restaurant testRestaurant6 = new Restaurant(5, 5, "Ciao", "Nice place for nice food", "ciao.png", 4);
            Restaurant testRestaurant7 = new Restaurant(6, 5, "Uppercrust", "Nice place for nice food", "uppercrust.png", 3);
            Restaurant testRestaurant8 = new Restaurant(7, 5, "Lavera", "Nice place for nice food", "lavera.png", 5);
            Restaurant testRestaurant9 = new Restaurant(8, 5, "Pizza Fez", "Nice place for nice food", "pizzafez.png", 1);


            PreviousOrdersSlider.AddItem(new RestaurantSliderTileLayout(testRestaurant5).Content);
            PreviousOrdersSlider.AddItem(new RestaurantSliderTileLayout(testRestaurant4).Content);
            PreviousOrdersSlider.AddItem(new RestaurantSliderTileLayout(testRestaurant3).Content);
            PreviousOrdersSlider.AddItem(new RestaurantSliderTileLayout(testRestaurant2).Content);
            PreviousOrdersSlider.AddItem(new RestaurantSliderTileLayout(testRestaurant1).Content);


            PreviouslyUsedSlider = new TopSlider();
            PreviouslyUsedSlider.Clear();

            PreviouslyUsedSlider.AddItem(new RestaurantSliderTileLayout(testRestaurant9).Content);
            PreviouslyUsedSlider.AddItem(new RestaurantSliderTileLayout(testRestaurant8).Content);
            PreviouslyUsedSlider.AddItem(new RestaurantSliderTileLayout(testRestaurant7).Content);
            PreviouslyUsedSlider.AddItem(new RestaurantSliderTileLayout(testRestaurant6).Content);
            


            ContentContainer.Children.Add(Header.Content);
            ContentContainer.Children.Add(Title.Content);
            //ContentContainer.Children.Add(TopDivider);
            //ContentContainer.Children.Add(PlaceOrderLabel.Content);
            ContentContainer.Children.Add(LocationSeachBox.Content);

            ContentContainer.Children.Add(PreviousOrderLabel.Content);
            ContentContainer.Children.Add(PreviousOrdersSlider.Content);

            ContentContainer.Children.Add(PreviouslyUsedLabel.Content);
            ContentContainer.Children.Add(PreviouslyUsedSlider.Content);


            NextButton = new StandardButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Branding.Colors.PINK), Color.White, "Next", null);
            //NextButton.UpAction = new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.CategoryMenu);

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

            ContentContainer.Children.Add(NextButton.Content);

            PageContent.Children.Add(ContentContainer);
        }

        public override void Destroy()
        {

        }

        public override async Task Update()
        {
            await DebugUpdate(AppSettings.TransitionVeryFast);

            if (AppSession.CurrentUser != null)
            {
                if (AppSession.CurrentUser.FirstName.Length > 0)
                {
                    Title.Title.Text = "";
                    var s = new FormattedString();

                    s.Spans.Add(new Span { Text = "Welcome ", FontFamily = Fonts.GetFont(FontName.MuliRegular), FontSize = Units.FontSizeXXL, FontAttributes = FontAttributes.None});
                    s.Spans.Add(new Span { Text = AppSession.CurrentUser.FirstName, FontFamily = Fonts.GetFont(FontName.MuliBold), FontSize = Units.FontSizeXXL, FontAttributes = FontAttributes.Bold});
                    Title.Title.FormattedText = s;
                }
            }
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
