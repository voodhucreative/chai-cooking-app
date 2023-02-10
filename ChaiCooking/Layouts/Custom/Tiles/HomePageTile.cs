using System;
using ChaiCooking.Branding;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Tiles
{
    public class HomePageTile : StandardLayout
    {
        public StaticLabel Title { get; set; }
        public StaticLabel Info { get; set; }
        public StaticImage BackgroundImage { get; set; }

        Grid BackgroundLayer { get; set; }
        StackLayout ContentContainer { get; set; }

        public HomePageTile(string title, string info)
        {
            Container.HeightRequest = 480;
            Container.VerticalOptions = LayoutOptions.EndAndExpand;
            Container.ColumnSpacing = 0;
            Container.RowSpacing = 0;
            //Container.RowDefinitions.Add(new RowDefinition { Height = Units.ThirdScreenHeight });
            //Container.RowDefinitions.Add(new RowDefinition { Height = Dimensions.HOME_PAGE_TILE_TEXT_PANEL_HEIGHT });

            BackgroundLayer = new Grid
            {
                BackgroundColor = Color.Black,
                Opacity = 0.75,
                HeightRequest = 92,
                VerticalOptions = LayoutOptions.EndAndExpand,
                RowSpacing = 0,
                ColumnSpacing = 0
            };

            ContentContainer = new StackLayout
            {
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.EndAndExpand,
                HeightRequest = 92,
                Spacing = 0
            };

            Title = new StaticLabel(title);
            Title.Content.TextColor = Color.White;
            Title.Content.FontSize = Units.FontSizeXXL;
            Title.Content.FontFamily = Fonts.GetBoldAppFont();

            Title.CenterAlign();
            Title.Content.VerticalOptions = LayoutOptions.EndAndExpand;
            //Title.Content.VerticalTextAlignment = TextAlignment.Center;

            Info = new StaticLabel(info);
            Info.Content.TextColor = Color.FromHex(Colors.CC_ORANGE);
            Info.Content.FontSize = Units.FontSizeL;
            Info.Content.FontFamily = Fonts.GetBoldAppFont();
            Info.CenterAlign();
            Info.Content.VerticalOptions = LayoutOptions.StartAndExpand;

            BackgroundImage = new StaticImage("drnow.jpg", Units.ScreenWidth, null);
            BackgroundImage.Content.Aspect = Aspect.Fill;

            //Info = new StaticLabel(info);
            //Info.Content.TextColor = Color.FromHex(Colors.CC_ORANGE);
            //Info.Content.FontSize = Units.FontSizeL;
            //Info.Content.FontFamily = Fonts.GetBoldAppFont();
            //Info.CenterAlign();

            ContentContainer.Children.Add(Title.Content);
            ContentContainer.Children.Add(Info.Content);

            //BackgroundLayer.Children.Add(BackgroundImage.Content);

            Container.Children.Add(BackgroundLayer, 0, 1);
            Container.Children.Add(ContentContainer, 0, 1);

            //Grid.SetRowSpan(BackgroundLayer, 3);
            Content.Children.Add(BackgroundImage.Content, 0, 0);
            Content.Children.Add(Container, 0, 0);


        }
    }
}
