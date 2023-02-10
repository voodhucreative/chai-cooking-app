using System;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Images;
using ChaiCooking.Helpers;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom
{
    public class MenuHeader
    {
        public Grid Content;
        ActiveImage Logo;
        IconButton MenuButton;

        //Image RightDots;

        public MenuHeader()
        {
            Content = new Grid
            {
                HeightRequest = 80,//Units.ScreenHeight15Percent,
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(Units.ScreenHeight5Percent, 0, Units.ScreenWidth10Percent, 1),
                ColumnSpacing = 0,
                RowSpacing = 0,
                BackgroundColor = Color.FromHex(Colors.CC_ORANGE)
            };

            //Logo = new ActiveImage("logo.png", Units.HalfScreenWidth, Units.HalfScreenWidth, null, new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.TELandingPage));
            //Logo.Content.VerticalOptions = LayoutOptions.Start;
            //Logo.Content.HorizontalOptions = LayoutOptions.Start;
            //Logo.Content.BackgroundColor = Color.Transparent;
            //Logo.Content.Margin = new Thickness(Units.ScreenWidth5Percent, Units.ScreenWidth5Percent);


            //MenuButton = new IconButton(Units.TapSizeM, Units.TapSizeM, Color.Transparent, Color.Transparent, "", "menucloseicon.png", new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.TEMenuPage));
            MenuButton = new IconButton(Units.TapSizeL, Units.ScreenHeight20Percent, Color.Transparent, Color.Transparent, "", "minus.png", null);

            MenuButton.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                AppSession.InfoModeOn = true;
                                if (AppSession.InfoModeOn)
                                {
                                    double x = Tools.Screen.GetScreenCoordinates(MenuButton.Content).X;
                                    double y = Tools.Screen.GetScreenCoordinates(MenuButton.Content).Y;
                                    //App.ShowInfoBubble(new Label { Text = "This is the home button\n\nEnabling this, will show info pops ups!" }, (int)x + MenuButton.Width / 2, (int)y);
                                    //App.ShowInfoBubble(new Paragraph("Recipe Editor", "Create your own content.", null).Content, (int)Units.HalfScreenWidth, (int)Units.HalfScreenHeight);

                                }
                                else
                                {
                                    await App.PerformActionAsync(new Models.Action((int)Actions.ActionName.ToggleMenu));
                                }

                            });
                        })
                    }
                );
            


            /*
            MenuButton.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await App.CloseMenu();

                            });
                        })
                    }
                );*/

            /*MenuButton.Content.BackgroundColor = Color.Transparent;
            MenuButton.Content.HorizontalOptions = LayoutOptions.End;
            MenuButton.Content.VerticalOptions = LayoutOptions.Center;

            MenuButton.Icon.Content.Margin = new Thickness(0, Units.ScreenUnitXS);
            MenuButton.Icon.Content.VerticalOptions = LayoutOptions.Center;
            MenuButton.Icon.Content.HorizontalOptions = LayoutOptions.End;*/
            MenuButton.Content.BackgroundColor = Color.Transparent;
            MenuButton.Content.HorizontalOptions = LayoutOptions.End;
            MenuButton.Content.VerticalOptions = LayoutOptions.CenterAndExpand;


            //MenuButton.Icon.Content.Margin = new Thickness(0, Units.ScreenUnitXS);
            MenuButton.Icon.Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            MenuButton.Icon.Content.HorizontalOptions = LayoutOptions.End;
            MenuButton.Icon.Content.Aspect = Aspect.AspectFill;

            //RightDots = new Image { Source = "dotssquares.png" };

            //Content.Children.Add(Logo.Content, 0, 0);
            Content.Children.Add(MenuButton.Content, 2, 0);
            //Content.Children.Add(RightDots, 2, 0);

           //Grid.SetColumnSpan(Logo.Content, 2);

            //SetSmallIcon();
            SetLargeIcon();

        }

        public void SetLargeIcon()
        {
            Content.Opacity = 1;

            //Logo.Content.WidthRequest = Units.ScreenWidth;
            //Logo.Image.WidthRequest = Units.ScreenWidth;

            //Logo.Content.Padding = new Thickness(0, Units.ScreenUnitM, Units.ScreenUnitL, Units.ScreenUnitM);
            //Logo.Image.Margin = new Thickness(0, Units.ScreenUnitM);
        }

        public void SetSmallIcon()
        {
            Content.Opacity = 0;

            //Logo.Content.WidthRequest = Units.ScreenWidth;
            //Logo.Image.WidthRequest = Units.ScreenWidth;

            //Logo.Content.Padding = new Thickness(0, Units.ScreenUnitS, Units.ScreenWidth20Percent, Units.ScreenUnitM);
            //Logo.Image.Margin = new Thickness(0, Units.ScreenUnitM);
        }
    }
}
