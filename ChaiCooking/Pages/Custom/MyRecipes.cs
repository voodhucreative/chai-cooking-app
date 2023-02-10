using System;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Helpers;
using ChaiCooking.Layouts.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class MyRecipes : Page
    {
        StackLayout ContentContainer;

        public MyRecipes()
        {
            this.IsScrollable = true;
            this.IsRefreshable = true;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;
            this.Id = (int)AppSettings.PageNames.MyRecipes;
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

            
            return mainLayout;
        }

        public override async Task Update()
        {
            if (this.NeedsRefreshing)
            {
                await DebugUpdate(AppSettings.TransitionVeryFast);
                await base.Update();
                PageContent.Children.Remove(ContentContainer);
                ContentContainer = BuildContent();
                PageContent.Children.Add(ContentContainer);

                App.SetSubHeaderTitle(AppText.RECOMMENDED_RECIPES, new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.RecommendedRecipes));
            }
        }
    }
}