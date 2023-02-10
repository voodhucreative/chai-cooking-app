using System;
using System.Threading.Tasks;
using ChaiCooking.Branding;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using MediaManager;
using Octane.Xamarin.Forms.VideoPlayer;
using VimeoDotNet;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChaiCooking.Pages.Custom
{
    public class Videos : Page
    {
        StackLayout ContentContainer;
        StackLayout VideoListContainer;
        StackLayout SingleVideoContainer;

        VideoPlayer VimeoPlayer;

        StaticLabel Title;

        const int MODE_LIST = 0;

        int Mode;

        public Videos()
        {
            this.IsScrollable = true;
            this.IsRefreshable = true;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;

            this.Id = (int)AppSettings.PageNames.Videos;
            this.Name = AppData.AppText.COOK_IT_CORNER;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.SlideInFromRight;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.SlideOutToRight;

            Mode = MODE_LIST;

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
                Spacing = 0
            };

            VideoListContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                Spacing = Dimensions.GENERAL_COMPONENT_SPACING

            };

            SingleVideoContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical
            };

            Title = new StaticLabel(AppData.AppText.COOK_IT_CORNER + ".");
            Title.Content.TextColor = Color.White;
            Title.Content.FontSize = Units.FontSizeXXL;
            Title.Content.Padding = Dimensions.GENERAL_COMPONENT_SPACING;
            Title.CenterAlign();


            foreach (Video video in AppDataContent.Videos[AppSession.CurrentVideoFeed].Videos)
            {
                VideoTile videoTile = new VideoTile(video);
                TouchEffect.SetNativeAnimation(videoTile.Content, true);
                TouchEffect.SetCommand(videoTile.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        ShowSingleVideo(video);
                    });
                }));

                VideoListContainer.Children.Add(videoTile.Content);

            }
            ShowList();

            PageContent.Children.Add(ContentContainer);
            return ContentContainer;
        }

        public override async Task Update()
        {
            await base.Update();

            ContentContainer.Children.Clear();
            ContentContainer = BuildContent();
            ShowList();
            PageContent.Children.Add(ContentContainer);

        }

        private void ShowList()
        {
            try
            {
                Content.Children.Clear();
            }
            catch (Exception e) { }
            ContentContainer.Children.Clear();
            ContentContainer.Children.Add(VideoListContainer);
        }

        private void ShowSingleVideo(Video video)
        {

            VimeoPlayer = new VideoPlayer
            {
                Source = video.VideoUrl,
                AutoPlay = true,
                DisplayControls = true,
                BackgroundColor = Color.Black,
                FillMode = Octane.Xamarin.Forms.VideoPlayer.Constants.FillMode.ResizeAspect,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Content.BackgroundColor = Color.Transparent;

            Content.Children.Clear();
            Content.BackgroundColor = Color.Black;
            VimeoPlayer.HeightRequest = Units.ScreenWidth;
            VimeoPlayer.HeightRequest = Units.ScreenWidth * 0.5625;
            VimeoPlayer.FillMode = Octane.Xamarin.Forms.VideoPlayer.Constants.FillMode.ResizeAspectFill;

            Content.Padding = 0;
            Content.Children.Add(VimeoPlayer);

        }

        public async Task authenticate()
        {
            string accesstoken = AppSettings.VIMEO_ACCESS_TOKEN;

            try
            {
                VimeoClient vimeoClient = new VimeoClient(accesstoken);
                var authcheck = await vimeoClient.GetAccountInformationAsync();
                var videos = await vimeoClient.GetVideosAsync(authcheck.Id, 1, 1);

                Console.WriteLine("videos");
            }
            catch (Exception er)
            {
                Console.WriteLine("error" + er.StackTrace);
            }
        }
    }
}