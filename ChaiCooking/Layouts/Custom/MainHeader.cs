using System;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom
{
    public class MainHeader
    {
        public Grid Content;
        ActiveImage Logo;
        IconLabel YourMenu;
        StaticLabel Title;


        public MainHeader()
        {
            Content = new Grid
            {
                HeightRequest = Dimensions.HEADER_HEIGHT,
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(Dimensions.HEADER_PADDING),
                ColumnSpacing = 0,
                RowSpacing = 0,
                BackgroundColor = Color.FromHex(Colors.CC_ORANGE),

            };

            

            Logo = new ActiveImage("chailogo.png", Dimensions.HEADER_LOGO_WIDTH, Dimensions.HEADER_LOGO_HEIGHT, null, null);

            YourMenu = new IconLabel("eyeicon.png", "Menu", Dimensions.ICON_LABEL_WIDTH, Dimensions.ICON_LABEL_HEIGHT);
            YourMenu.SetIconSize(Dimensions.ICON_LABEL_ICON_SIZE, Dimensions.ICON_LABEL_ICON_SIZE);
            YourMenu.SetIconLeft();
            YourMenu.TextContent.Content.FontSize = Dimensions.HEADER_LABEL_FONT_SIZE;
            YourMenu.TextContent.Content.TextColor = Color.White;
            YourMenu.TextContent.Content.FontFamily = Fonts.GetBoldAppFont();

            if (App.IsSmallScreen())
            {
                YourMenu.TextContent.Content.Text = "Menu";
                Logo.Image.Source = "chaiface.png";
                Content.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(Units.ScreenWidth25Percent) });
                Content.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(Units.ScreenWidth*0.55) }); ;
                Content.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(Units.ScreenWidth20Percent) });
            }

            TouchEffect.SetNativeAnimation(YourMenu.Content, true);
            TouchEffect.SetCommand(YourMenu.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        double x = Tools.Screen.GetScreenCoordinates(YourMenu.Content).X;
                        double y = Tools.Screen.GetScreenCoordinates(YourMenu.Content).Y;
                        //App.ShowInfoBubble(new Label { Text = "Global Feature - search tool, My Library, Filter options, and Profile and Settings." }, (int)(x + YourMenu.Content.Width / 2), (int)y);
                        App.ShowInfoBubble(new Paragraph("Menu", "This menu can be opened in any CHAI page and contains a search tool, a library, filters, settings and more...", null).Content, (int)Units.HalfScreenWidth, (int)Units.HalfScreenHeight);

                    }
                    else
                    {
                        await App.PerformActionAsync(new Models.Action((int)Actions.ActionName.ToggleMenu));
                    }
                });
            }));

            Title = new StaticLabel("Home");
            Title.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            Title.Content.HorizontalTextAlignment = TextAlignment.Center;
            Title.Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            Title.Content.VerticalTextAlignment = TextAlignment.Center;
            Title.Content.TextColor = Color.White;
            Title.Content.FontSize = Dimensions.HEADER_LABEL_FONT_SIZE;
            Title.Content.FontFamily = Fonts.GetBoldAppFont();

            Logo.Content.VerticalOptions = LayoutOptions.End;
            Logo.Content.HorizontalOptions = LayoutOptions.EndAndExpand;
            Logo.Content.BackgroundColor = Color.Transparent;
            Logo.Image.Aspect = Aspect.AspectFit;

            TouchEffect.SetNativeAnimation(Logo.Content, true);
            TouchEffect.SetCommand(Logo.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        double x = Tools.Screen.GetScreenCoordinates(Logo.Content).X;
                        double y = Tools.Screen.GetScreenCoordinates(Logo.Content).Y;
                        //App.ShowInfoBubble(new Label { Text = "Skip back ONE page at a time." }, (int)x + Logo.Width / 2, (int)y);
                        App.ShowInfoBubble(new Paragraph("CHAI Logo", "Skip back ONE page at a time.", null).Content, (int)Units.HalfScreenWidth, (int)Units.HalfScreenHeight);

                    }
                    else
                    {
                        App.GoBack();
                        //await App.PerformActionAsync(new Models.Action((int)Actions.ActionName.ToggleMenu));
                    }
                });
            }));

            Content.Children.Add(YourMenu.Content, 0, 0);
            Content.Children.Add(Title.Content, 1, 0);

            if (AppSettings.IsPublicBuild)
            {
                Content.Children.Add(Logo.Content, 2, 0);
            }
            else // include build id
            {
                LayoutOptions dLoption = LayoutOptions.EndAndExpand;
                TextAlignment tLoption = TextAlignment.End;

                if (App.IsSmallScreen())
                {
                    dLoption = LayoutOptions.CenterAndExpand;
                    tLoption = TextAlignment.Center;
                }
                Content.Children.Add(new StackLayout
                {
                    Spacing = 0,
                    Orientation = StackOrientation.Vertical,
                    Children = {
                    Logo.Content,
                    new Label
                    {
                        TextColor = Color.Black,
                        FontSize = 10,
                        HeightRequest = 12,
                        HorizontalOptions = dLoption,
                        HorizontalTextAlignment = tLoption,
                        Text = AppSettings.VersionID,
                        Margin = new Thickness(0, 4, 0, 0),
                        Opacity = 0.25
                    }
                }
                }, 2, 0);
            }
        }

        public void HideMenuButton()
        {
            /*Logo.Content.HorizontalOptions = LayoutOptions.Center;
            Content.Padding = Units.ScreenWidth10Percent;// new Thickness(Units.ScreenWidth10Percent, Units.ScreenWidth10Percent, Units.ScreenWidth10Percent, 1);
            Content.Children.Clear();
            Content.Children.Add(Logo.Content, 0, 0);*/
            //Content.Children.Add(MenuButton.Content, 2, 0);
            //Grid.SetColumnSpan(Logo.Content, 2);
            YourMenu.Content.IsVisible = false;
        }

        public void ShowMenuButton()
        {
            /*Logo.Content.HorizontalOptions = LayoutOptions.Start;
            Content.Padding = new Thickness(Units.ScreenHeight5Percent, 0, Units.ScreenWidth10Percent, 1);
            Content.Children.Clear();
            Content.Children.Add(Logo.Content, 0, 0);
            Content.Children.Add(MenuButton.Content, 2, 0);
            Grid.SetColumnSpan(Logo.Content, 2);*/
            YourMenu.Content.IsVisible = true;
        }

        public void SetLargeIcon()
        {
            Content.Opacity = 1;
        }

        public void SetSmallIcon()
        {
            Content.Opacity = 0;
        }

        public void SetTitle(string title)
        {
            Title.Content.Text = title;
        }

        public void ShowMenuOpen()
        {
            YourMenu.Icon.Content.Source = "eyeopen.png";
        }

        public void ShowMenuClosed()
        {
            YourMenu.Icon.Content.Source = "eyeicon.png";
        }

        public void Update()
        {
            SetTitle(Helpers.Pages.GetCurrent().Name);
        }
    }
}
