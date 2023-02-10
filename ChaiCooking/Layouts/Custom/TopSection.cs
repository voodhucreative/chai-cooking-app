using System;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Images;
using ChaiCooking.Helpers;
using Xamarin.Forms;
using static ChaiCooking.Helpers.Fonts;

namespace ChaiCooking.Layouts.Custom
{
    public class TopSection
    {
        public Grid Content;
        Image TopElement;
        //Image TopLeftElement;
        public Label TitleLabel;
        //ActiveImage Logo;
        //IconButton MenuButton;
        //IconButton MenuCloseButton;
        //public bool IsMenuPage;

        public TopSection(string title)
        {
            //IsMenuPage = false;
            Content = new Grid { VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.Start, BackgroundColor = Color.Transparent, WidthRequest = Units.ScreenWidth };
            //TopLeftElement = new Image { Source = "element_topleft.png", WidthRequest = Units.HalfScreenWidth, HorizontalOptions = LayoutOptions.Start };
            TopElement = new Image { Source = "top_light_blue.png", VerticalOptions = LayoutOptions.Start , WidthRequest = Units.ScreenWidth, HeightRequest = Units.HalfScreenWidth, Aspect = Aspect.AspectFill };

            TitleLabel = new Label {
                Text = title,
                HorizontalOptions = LayoutOptions.Start,
                FontSize = Units.DynamicFontSizeXXL,
                FontFamily = Fonts.GetFont(FontName.PoppinsBold),
                TextColor = Color.White,
                WidthRequest = Units.HalfScreenWidth,
                Margin = new Thickness(Units.ScreenUnitL, Units.ScreenUnitM)
            };

            if (Device.RuntimePlatform == Device.iOS)
            {
                TitleLabel.FontSize = Units.FontSizeXXL;
            }

                //Logo = new ActiveImage("logo.png", Units.ThirdScreenWidth, Units.TapSizeL, null, new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.TELandingPage));
                //Logo.Content.VerticalOptions = LayoutOptions.Start;
                //Logo.Content.HorizontalOptions = LayoutOptions.Start;
                //Logo.Content.BackgroundColor = Color.Transparent;
                //Logo.Content.Margin = new Thickness(Units.ScreenWidth5Percent, Units.ScreenWidth5Percent);

                /*
                MenuButton = new IconButton(Units.TapSizeM, Units.TapSizeM, Color.Transparent, Color.Transparent, "", "menuicon.png", new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.TEMenuPage));
                MenuCloseButton = new IconButton(Units.TapSizeM, Units.TapSizeM, Color.Transparent, Color.Transparent, "", "menucloseicon.png", null);

                MenuCloseButton.Content.GestureRecognizers.Add(
                        new TapGestureRecognizer()
                        {
                            Command = new Command(() =>
                            {
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    //await Update();
                                    await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.TELandingPage);
                                });
                            })
                        }
                    );




                MenuButton.Content.IsVisible = true;
                MenuCloseButton.Content.IsVisible = false;


                MenuButton.Content.Margin = new Thickness(Units.ScreenUnitL, Units.ScreenUnitM);
                MenuButton.Content.BackgroundColor = Color.Transparent;
                MenuButton.Content.HorizontalOptions = LayoutOptions.End;
                MenuButton.Content.VerticalOptions = LayoutOptions.Start;
                MenuButton.Icon.Content.Margin = new Thickness(0, Units.ScreenUnitXS);
                MenuButton.Icon.Content.VerticalOptions = LayoutOptions.Start;
                MenuButton.Icon.Content.HorizontalOptions = LayoutOptions.End;

                MenuCloseButton.Content.Margin = new Thickness(Units.ScreenUnitL, Units.ScreenUnitM);
                MenuCloseButton.Content.BackgroundColor = Color.Transparent;
                MenuCloseButton.Content.HorizontalOptions = LayoutOptions.End;
                MenuCloseButton.Content.VerticalOptions = LayoutOptions.Start;
                MenuCloseButton.Icon.Content.Margin = new Thickness(0, Units.ScreenUnitXS);
                MenuCloseButton.Icon.Content.VerticalOptions = LayoutOptions.Start;
                MenuCloseButton.Icon.Content.HorizontalOptions = LayoutOptions.End;

                */
                Content.Children.Add(TopElement, 0, 0);
            //Content.Children.Add(TopLeftElement, 0, 0);
            Content.Children.Add(TitleLabel, 0, 0);
            //Content.Children.Add(Logo.Content, 0, 0);
            //Content.Children.Add(MenuButton.Content, 0, 0);
            //Content.Children.Add(MenuCloseButton.Content, 0, 0);

            //TitleLabel.TranslateTo(0, Units.ScreenWidth35Percent, 0, null);
            
        }

        public void SetBackgroundImage(string imageSrc)
        {
            TopElement.Source = imageSrc;
        }

        public void HideSmallTopElement()
        {
            TopElement.Opacity = 0;
        }

        public void ShowSmallTopElement()
        {
            TopElement.Opacity = 1;
        }

        public void ShowMenuClose()
        {
            //IsMenuPage = true;
            //MenuButton.Content.IsVisible = false;
            //MenuButton.Content.IsEnabled = false;
            //MenuCloseButton.Content.IsVisible = true;
            //MenuCloseButton.Content.IsEnabled = true;
        }

        public void ShowMenuOpen()
        {
            //MenuButton.Content.IsVisible = true;
            //MenuButton.Content.IsEnabled = true;
            //MenuCloseButton.Content.IsVisible = false;
            //MenuCloseButton.Content.IsEnabled = false;

        }

        
        /*public void HideSmallTopElement()
        {
            TopElement.Opacity = 0;
        }

        public void ShowSmallTopElement()
        {
            TopElement.Opacity = 1;
        }*/
    }
}
