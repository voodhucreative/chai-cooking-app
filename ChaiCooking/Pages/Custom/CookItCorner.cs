using System;
using System.Threading.Tasks;
using ChaiCooking.Branding;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class CookItCorner : Page
    {
        StackLayout ContentContainer;
        StaticLabel Title;

        public CookItCorner()
        {
            this.IsScrollable = true;
            this.IsRefreshable = true;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;


            this.Id = (int)AppSettings.PageNames.CookItCorner;
            this.Name = AppData.AppText.COOK_IT_CORNER;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.SlideInFromRight;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.SlideOutToRight;

            PageContent = new Grid
            {
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY)
            };

            ContentContainer = BuildContent();
        }

        public StackLayout BuildContent()
        {
            ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 0
            };

            Title = new StaticLabel(AppData.AppText.COOK_IT_CORNER + ".");
            Title.Content.TextColor = Color.White;
            Title.Content.FontSize = Units.FontSizeXXL;
            Title.Content.Padding = Dimensions.GENERAL_COMPONENT_SPACING;
            Title.CenterAlign();

            foreach (VideoFeed videoFeed in AppDataContent.Videos)
            {
                VideoFeedTile sectionTile = new VideoFeedTile(videoFeed);
                TouchEffect.SetNativeAnimation(sectionTile.Content, true);
                TouchEffect.SetCommand(sectionTile.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        AppSession.CurrentVideoFeed = videoFeed.Id - 1;

                        Console.WriteLine("Current Feed Id " + AppSession.CurrentVideoFeed);
                        if (AppSession.InfoModeOn)
                        {
                            //App.ShowInfoBubble(new Label { Text = videoFeed.Name }, (int)Units.HalfScreenWidth, (int)Units.HalfScreenHeight);
                            App.ShowInfoBubble(new Paragraph("Video",videoFeed.Name, null).Content, (int)Units.HalfScreenWidth, (int)Units.HalfScreenHeight);


                        }
                        else
                        {
                            await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Videos);
                        }
                    });
                }));

                ContentContainer.Children.Add(sectionTile.Content);
            }

            PageContent.Children.Add(ContentContainer);

            return ContentContainer;
        }

        public override async Task Update()
        {
            await base.Update();
            App.SetSubHeaderTitle("", null);
            PageContent.Children.Clear();
            AppDataContent.PopulateCookItCornerVideos();
            PageContent.Children.Add(BuildContent());
        }
    }
}

