using System;
using ChaiCooking.Branding;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Tiles
{
    public class VideoFeedTile : StandardLayout
    {
        public StaticLabel Title { get; set; }
        public string ComponentInfo { get; set; }
        //public StaticLabel Info { get; set; }

        public StaticImage BackgroundImage { get; set; }

        Grid BackgroundLayer { get; set; }
        StackLayout ContentContainer { get; set; }

        public VideoFeedTile(VideoFeed videoFeed)//string title, string info, string imageSource)
        {

            ComponentInfo = videoFeed.Name;
            Container.HeightRequest = Units.QuarterScreenHeight;
            Container.VerticalOptions = LayoutOptions.EndAndExpand;
            //Container.RowDefinitions.Add(new RowDefinition { Height = Units.ThirdScreenHeight });
            //Container.RowDefinitions.Add(new RowDefinition { Height = Dimensions.HOME_PAGE_TILE_TEXT_PANEL_HEIGHT });

            BackgroundLayer = new Grid
            {
                BackgroundColor = Color.Black,
                Opacity = 0.75,
                HeightRequest = 64,
                VerticalOptions = LayoutOptions.EndAndExpand
            };

            ContentContainer = new StackLayout
            {
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.EndAndExpand,
                HeightRequest = 64,
                Spacing = 0
            };

            Title = new StaticLabel(videoFeed.Name);
            Title.Content.TextColor = Color.White;
            Title.Content.FontSize = Units.FontSizeXXL;
            Title.Content.FontFamily = Fonts.GetBoldAppFont();

            Title.CenterAlign();
            Title.Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            Title.Content.VerticalTextAlignment = TextAlignment.Center;

            BackgroundImage = new StaticImage(videoFeed.MainImage, Units.ScreenWidth, null);
            BackgroundImage.Content.Aspect = Aspect.AspectFill;

            //Info = new StaticLabel(info);
            //Info.Content.TextColor = Color.FromHex(Colors.CC_ORANGE);
            //Info.Content.FontSize = Units.FontSizeL;
            //Info.Content.FontFamily = Fonts.GetBoldAppFont();
            //Info.CenterAlign();

            ContentContainer.Children.Add(Title.Content);
            //ContentContainer.Children.Add(Info.Content);

            //BackgroundLayer.Children.Add(BackgroundImage.Content);

            Container.Children.Add(BackgroundLayer, 0, 1);
            Container.Children.Add(ContentContainer, 0, 1);

            //Grid.SetRowSpan(BackgroundLayer, 3);
            Content.Children.Add(BackgroundImage.Content, 0, 0);
            Content.Children.Add(Container, 0, 0);


        }
    }
}
