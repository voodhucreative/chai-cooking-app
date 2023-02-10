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
    public class OptionsMenu : Page
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

        protected TiledList OptionsList;

        protected int TilesPerRow = 1;

        public OptionsMenu()
        {
            this.IsScrollable = true;
            this.FullScreen = true;
            //this.Id = (int)AppSettings.PageNames.OptionsMenu;
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

            Title = new Layouts.Custom.PageTitle("Options Menu", Color.White, Color.Black);

            OptionsList = new TiledList(TilesPerRow);

            foreach (Option option in FakeData.Options)
            {
                OptionLayout optionLayout = new OptionLayout(option);
                optionLayout.Content.WidthRequest = Units.ScreenWidth / TilesPerRow;
                optionLayout.Content.HeightRequest = Units.TapSizeXL;
                optionLayout.Content.BackgroundColor = Color.White;

                Tile tile = new Tile();
                tile.DefaultAction = new Models.Action((int)Actions.ActionName.ToggleHeader, -1);
                tile.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await tile.DefaultAction.Execute();
                            });
                        })
                    }
                );
                tile.AddContent(optionLayout.Content);
                OptionsList.AddTile(tile);
            }

            NextButton = new StandardButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Branding.Colors.PINK), Color.White, "Next", null);

            //NextButton.UpAction = new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.OrderConfirmation);

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
            ContentContainer.Children.Add(OptionsList.Content);
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
