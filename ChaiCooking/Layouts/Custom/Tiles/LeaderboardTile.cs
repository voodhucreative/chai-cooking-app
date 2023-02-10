using System;
using ChaiCooking.Components;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models;
using ChaiCooking.Models.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Tiles
{
    public class LeaderboardTile : ActiveComponent
    {
        StackLayout UserContainer { get; set; }

        StaticLabel PositionLabel { get; set; }
        StaticLabel UserNameLabel { get; set; }
        StaticLabel PointsLabel { get; set; }

        public SocialRewardTile Icon { get; set; }

        public LeaderboardTile(Influencer user, int position, int points)
        {
            string positionText = "#";

            if (position < 100) { positionText += "0"; };
            if (position < 10) { positionText += "0"; };

            positionText += position;

            Container = new Grid { };
            Content = new Grid { };

            UserContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                WidthRequest = Units.ScreenWidth,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Spacing = Dimensions.GENERAL_COMPONENT_SPACING
            };

            PositionLabel = new StaticLabel(positionText);
            PositionLabel.LeftAlign();
            PositionLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            PositionLabel.Content.WidthRequest = 48;
            PositionLabel.Content.TextColor = Color.White;
            PositionLabel.Content.FontSize = Units.FontSizeM;

            Icon = new SocialRewardTile(user.AvatarImageUrl, user.Username);
            Icon.Content.WidthRequest = 64;

            UserNameLabel = new StaticLabel(user.Username);
            UserNameLabel.LeftAlign();
            UserNameLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            UserNameLabel.Content.TextColor = Color.White;
            UserNameLabel.Content.FontSize = Units.FontSizeM;


            PointsLabel = new StaticLabel(points + " pts");
            PointsLabel.LeftAlign();
            PointsLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            PointsLabel.Content.TextColor = Color.White;
            PointsLabel.Content.FontSize = Units.FontSizeM;


            UserContainer.Children.Add(PositionLabel.Content);
            UserContainer.Children.Add(Icon.Content);
            UserContainer.Children.Add(UserNameLabel.Content);
            UserContainer.Children.Add(PointsLabel.Content);

            Container.Children.Add(UserContainer);
            Content.Children.Add(Container);

            // 14031975
            // 5791
        }
    }
}
