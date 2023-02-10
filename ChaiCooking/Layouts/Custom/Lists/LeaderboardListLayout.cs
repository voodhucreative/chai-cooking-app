using System;
using System.Collections.Generic;
using System.Linq;
using ChaiCooking.DebugData.Custom;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Lists
{
    public class LeaderboardListLayout
    {
        public StackLayout Content;

        private List<List<Influencer>> LeaderboardData;

        public LeaderboardListLayout()
        {
            Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.Transparent,
                Spacing = Dimensions.GENERAL_COMPONENT_SPACING,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                HorizontalOptions = LayoutOptions.StartAndExpand
                
            };

            LeaderboardData = DataManager.GetLeaderboard();

            Populate(LeaderboardData[0]);
        }

        public void Populate(List<Influencer> list)
        {
            int position = 1;
            List<Influencer> UserList = list.OrderBy(x => x.CreatorPoints).ToList();

            UserList.Reverse();

            Content.Children.Clear();

            foreach (Influencer influencer in UserList)
            {
                LeaderboardTile leaderboardTile = new LeaderboardTile(influencer, position, influencer.CreatorPoints);
                leaderboardTile.Icon.Content.WidthRequest = 40;
                leaderboardTile.Icon.Content.HeightRequest = 40;

                Content.Children.Add(leaderboardTile.Content);
                position++;
            }
        }

        public StackLayout GetContent()
        {
            return Content;
        }
    }
}
