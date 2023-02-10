using System;
using System.Collections.Generic;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Composites;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.DebugData.Custom;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom.Lists;
using ChaiCooking.Layouts.Custom.Panels;
using ChaiCooking.Layouts.Custom.Panels.Account;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class Leaderboard : Page
    {

        StackLayout ContentContainer;

        StackLayout HeaderContainer;

        StackLayout TabContainer;

        StackLayout ListContainer;

        StaticLabel Title;
        StaticImage HeaderIcon;
        ActiveLabel CloseLabel;

        Grid Seperator;

        LeaderboardListLayout MostPointsList;
        LeaderboardListLayout LeastWasteList;
        LeaderboardListLayout TopCreatorsList;


        private const int MOST_POINTS = 0;
        private const int LEAST_WASTE = 1;
        private const int TOP_CREATORS = 2;

        private int CurrentList;

        private List<List<Influencer>> LeaderboardData;

        public Leaderboard()
        {
            this.IsScrollable = true;
            this.IsRefreshable = true;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;


            this.Id = (int)AppSettings.PageNames.Leaderboard;
            this.Name = AppData.AppText.LEADERBOARD;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.SlideInFromRight;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.SlideOutToRight;

            LeaderboardData = DataManager.GetLeaderboard();
            MostPointsList = new LeaderboardListLayout();
            MostPointsList.Populate(LeaderboardData[MOST_POINTS]);

            LeastWasteList = new LeaderboardListLayout();
            LeastWasteList.Populate(LeaderboardData[LEAST_WASTE]);

            TopCreatorsList = new LeaderboardListLayout();
            TopCreatorsList.Populate(LeaderboardData[TOP_CREATORS]);

            CurrentList = MOST_POINTS;

            PageContent = new Grid
            {
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY)
            };

            ContentContainer = BuildContent();

            //ShowList(TOP_CREATORS);
        }

        public StackLayout BuildContent()
        {
            ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING)
            };

            HeaderContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = Dimensions.GENERAL_COMPONENT_SPACING,
                Padding = new Thickness(0, Dimensions.GENERAL_COMPONENT_PADDING)
            };

            TabContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = Dimensions.GENERAL_COMPONENT_SPACING,
                Padding = new Thickness(0, Dimensions.GENERAL_COMPONENT_PADDING)
            };

            ListContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                Padding = Dimensions.GENERAL_COMPONENT_PADDING
            };

            HeaderIcon = new StaticImage("trophyicon.png", 16, null);

            Title = new StaticLabel(AppData.AppText.LEADERBOARD);
            Title.Content.TextColor = Color.White;
            Title.Content.FontSize = Units.FontSizeL;
            Title.Content.FontFamily = Fonts.GetBoldAppFont();
            Title.LeftAlign();

            CloseLabel = new ActiveLabel(AppText.CLOSE, Units.FontSizeM, Color.Transparent, Color.White, null);
            CloseLabel.CenterAlign();

            CloseLabel.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await App.PerformActionAsync((int)Actions.ActionName.GoToPage, AppSession.LastPageId);
                                await App.ShowMenu();
                            });
                        })
                    }
                );

            Seperator = new Grid { WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) };

            HeaderContainer.Children.Add(HeaderIcon.Content);
            HeaderContainer.Children.Add(Title.Content);
            HeaderContainer.Children.Add(CloseLabel.Content);

            ContentContainer.Children.Add(HeaderContainer);
            ContentContainer.Children.Add(Seperator);

            ContentContainer.Children.Add(ListContainer);

            TabbedPanel loginCreatePanel = new TabbedPanel();
            loginCreatePanel.SetLayoutWidth(Units.ScreenWidth); //Dimensions.INFO_PANEL_WIDTH;
            //loginCreatePanel.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;

            GeneralInfoPanel mostPointsPanel = new GeneralInfoPanel("Most Points", Themes.PANEL_THEME_GREY, Units.ScreenWidth);
            GeneralInfoPanel leastWastePanel = new GeneralInfoPanel("Least Waste", Themes.PANEL_THEME_GREY, Units.ScreenWidth);
            GeneralInfoPanel topCreatorsPanel = new GeneralInfoPanel("Top Creators", Themes.PANEL_THEME_GREY, Units.ScreenWidth);

            mostPointsPanel.Title.Content.FontFamily = Fonts.GetBoldAppFont();
            leastWastePanel.Title.Content.FontFamily = Fonts.GetBoldAppFont();
            topCreatorsPanel.Title.Content.FontFamily = Fonts.GetBoldAppFont();

            mostPointsPanel.Title.CenterAlign();
            leastWastePanel.Title.CenterAlign();
            topCreatorsPanel.Title.CenterAlign();


            mostPointsPanel.AddContent(MostPointsList.GetContent());
            leastWastePanel.AddContent(LeastWasteList.GetContent());
            topCreatorsPanel.AddContent(TopCreatorsList.GetContent());
            

            loginCreatePanel.AddChildPanel(mostPointsPanel);
            loginCreatePanel.AddChildPanel(leastWastePanel);
            loginCreatePanel.AddChildPanel(topCreatorsPanel);

            StaticLabel listInfo = new StaticLabel("Here, you can check the Leaderboard for various categories - is your name up there yet?");
            listInfo.Content.BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY);
            //listInfo.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;
            listInfo.Content.TextColor = Color.White;

            ListContainer.Children.Add(listInfo.Content);
            ListContainer.Children.Add(loginCreatePanel.GetContent());



            PageContent.Children.Add(ContentContainer);

            return ContentContainer;
        }



        public void ShowList(int listId)
        {
            CurrentList = listId;

            ListContainer.Children.Clear();

            switch(CurrentList)
            {
                case MOST_POINTS:
                    ListContainer.Children.Add(MostPointsList.GetContent());
                    break;

                case LEAST_WASTE:
                    ListContainer.Children.Add(LeastWasteList.GetContent());
                    break;

                case TOP_CREATORS:
                    ListContainer.Children.Add(TopCreatorsList.GetContent());
                    break;
            }
        }
    }
}
