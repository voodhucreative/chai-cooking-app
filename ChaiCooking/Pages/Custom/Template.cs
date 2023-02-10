using System;
using System.Threading.Tasks;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Helpers;
using ChaiCooking.Layouts.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class Template : Page
    {
        StackLayout ContentContainer;

        public Template()
        {
            this.IsScrollable = false;
            this.HasHeader = false;
            this.Id = (int)AppSettings.PageNames.AboutUs;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.FadeIn;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.FadeOut;

            PageContent = new Grid
            {
                BackgroundColor = Color.White
            };

            // add a background?
            AddBackgroundImage("chaismallbag.png");

            ContentContainer = BuildContent();
            PageContent.Children.Add(ContentContainer);
        }

        public StackLayout BuildContent()
        {
            // build labels
            StackLayout mainLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.End,
                BackgroundColor = Color.Transparent,// FromHex("eeeeee"),
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight
            };

            mainLayout.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Landing);
                           });
                       })
                   }
               );
            return mainLayout;
        }

        public override async Task Update()
        {
            if (this.NeedsRefreshing)
            {
                await DebugUpdate(AppSettings.TransitionVeryFast);

                this.HasHeader = false;
                this.HasFooter = true;
                await base.Update();

                PageContent.Children.Remove(ContentContainer);
                ContentContainer = BuildContent();
                PageContent.Children.Add(ContentContainer);
                //App.ShowMenuButton();
                App.HideMenuButton();
            }
        }
    }
}