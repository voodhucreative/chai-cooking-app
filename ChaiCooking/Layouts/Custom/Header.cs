using System;
using TechExpo.Components.Images;
using TechExpo.Helpers;
using Xamarin.Forms;
using XFShapeView;

namespace TechExpo.Layouts.Custom
{
    public class Header
    {
        public Grid Content { get; set; }
        public int Height { get; set; }
        public uint TransitionTime { get; set; }

        private StaticImage Logo;
        private ActiveImage MenuButton;
        private ActiveImage BackButton;
        private StackLayout MainContainer;
        private RestaurantOptions RestaurantOptions;

        private Grid DividingLine;
         

        public Header(bool showTagLine)
        {
            TransitionTime = 500;

            Content = new Grid
            {
                BackgroundColor = Color.White
            };

            Content.RowDefinitions.Add(new RowDefinition { Height = new GridLength(Units.TapSizeM) });


            if (showTagLine)
            {
                Content.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            else
            {
                Content.RowDefinitions.Add(new RowDefinition { Height = new GridLength(Units.ScreenUnitM) });
            }

            Content.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1) });

            Logo = new StaticImage("logo.png", Units.ScreenWidth30Percent, Units.TapSizeM, null);
            //Logo.Content.Margin = new Thickness(0, Units.ScreenUnitM, 0, 0);

            BackButton = new ActiveImage("back_arrow.png", Units.TapSizeM, Units.TapSizeM, null, new Models.Action((int)Actions.ActionName.GoToLastPage, -1));

            MenuButton = new ActiveImage("menu_icon.png", Units.TapSizeM, Units.TapSizeM, null, new Models.Action((int)Actions.ActionName.ToggleMenu, -1));

            MainContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = Units.ScreenWidth,
                Margin = new Thickness(Units.ScreenUnitXS, Units.ScreenUnitXS)
            };

            RestaurantOptions = new RestaurantOptions(true, true, true, LayoutOptions.CenterAndExpand);

            DividingLine = new Grid
            {
                BackgroundColor = Color.LightGray,
                WidthRequest = Units.ScreenWidth,
                HeightRequest = 1
            };

            MainContainer.Children.Add(BackButton.Content);
            MainContainer.Children.Add(Logo.Content);
            MainContainer.Children.Add(MenuButton.Content);

            SetHeight(Units.ScreenHeight15Percent);

            Content.Children.Add(MainContainer, 0, 0);

            if (showTagLine)
            {
                Content.Children.Add(RestaurantOptions.Content, 0, 1);
            }

            Content.Children.Add(DividingLine, 0, 2);
            Show();
        }

        public void SetHeight(int height)
        {
            Height = height;
            Content.HeightRequest = height;
        }

        public void ShowBackButton()
        {

        }

        public void HideBackButton()
        {

        }

        public void ShowMenuButton()
        {

        }

        public void HideMenuButton()
        {

        }

        public void ShowTagLine()
        {
            Content.Children.Add(RestaurantOptions.Content, 0, 1);
        }

        public void HideTagLine()
        {
            Content.Children.Remove(RestaurantOptions.Content);
        }

        public void ShowDivider()
        {
            Content.Children.Add(DividingLine, 0, 2);
        }

        public void HideDivider()
        {
            Content.Children.Remove(DividingLine);
        }

       

        public void Show()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                Content.IsVisible = true;
                await Content.TranslateTo(0, 0, 100, Easing.Linear);
            });
        }

        public void Hide()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Content.TranslateTo(0, -Height, 100, Easing.Linear);
                Content.IsVisible = false;
            });
        }

        public void Update()
        {

        }

    }
}
