using System;
using ChaiCooking.Components;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Tiles
{
    public class SocialRewardTile : ActiveComponent
    {
        public Frame BackgroundFrame { get; set; }
        public StaticImage RewardImage { get; set; }
        public StaticLabel RewardName { get; set; }

        public SocialRewardTile(string imageUrl, string name)
        {
            BackgroundFrame = new Frame { HasShadow = AppSettings.ShadowsOnTiles, BackgroundColor = Color.White, CornerRadius = 8 , WidthRequest = 128, HeightRequest = 128, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand };
            RewardImage = new StaticImage(imageUrl, 128, null);
            RewardName = new StaticLabel(name);

            Container = new Grid { };
            Content = new Grid { };

            Container.Children.Add(BackgroundFrame, 0, 0);
            Container.Children.Add(RewardImage.Content, 0, 0);

            Content.Children.Add(Container);
        }
    }
}
