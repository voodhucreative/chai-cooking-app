using System;
using System.Threading.Tasks;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Tools;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom
{
    public class NavHeader : StandardLayout
    {

        ActiveImage Logo;
        IconButton MenuButton;

        IconButton BackButton;
        IconButton NextButton;

        ActiveLabel CloseLabel;
        ActiveImage RecycleImage;

        public NavHeader()
        {
            Height = Dimensions.HEADER_HEIGHT;
            Width = Units.ScreenWidth;
            TransitionTime = 150;
            TransitionType = (int)AppSettings.TransitionTypes.SlideOutTop;

            Content = new Grid
            {
                WidthRequest = Width,
                HeightRequest = Height,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };

            Container = new Grid
            {
                WidthRequest = Width,
                HeightRequest = Height,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                ColumnSpacing = 0,
                RowSpacing = 0,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY)
            };

            CloseLabel = new ActiveLabel("Close", Units.FontSizeM, Color.Transparent, Color.White, null);
            CloseLabel.Content.Margin = new Thickness(Dimensions.GENERAL_COMPONENT_SPACING, 0);
            CloseLabel.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            CloseLabel.Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            CloseLabel.Label.HorizontalOptions = LayoutOptions.CenterAndExpand;
            CloseLabel.Label.HorizontalTextAlignment = TextAlignment.Center;
            CloseLabel.Label.VerticalOptions = LayoutOptions.CenterAndExpand;
            CloseLabel.Label.VerticalTextAlignment = TextAlignment.Center;
            TouchEffect.SetNativeAnimation(CloseLabel.Content, true);
            TouchEffect.SetCommand(CloseLabel.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Task.Delay(50);
                    App.ShowRecipeList();
                });
            }));

            RecycleImage = new ActiveImage("recyclewhite.png", Dimensions.STANDARD_ICON_WIDTH, Dimensions.STANDARD_ICON_HEIGHT, null, new Models.Action((int)Actions.ActionName.TogglePanel));
            RecycleImage.Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            RecycleImage.Container.VerticalOptions = LayoutOptions.CenterAndExpand;

            BackButton = new IconButton(32, 32, Color.Transparent, Color.Transparent, "", "chevronleftbold.png", null);
            BackButton.Content.HorizontalOptions = LayoutOptions.EndAndExpand;
            BackButton.SetPositionRight();

            NextButton = new IconButton(32, 32, Color.Transparent, Color.Transparent, "", "chevronrightbold.png", null);
            NextButton.Content.HorizontalOptions = LayoutOptions.StartAndExpand;
            NextButton.SetPositionLeft();

            Container.Children.Add(BackButton.Content, 1, 0);
            Container.Children.Add(NextButton.Content, 3, 0);

            //Container.Children.Add(RecycleImage.Content, 4, 0);
            Container.Children.Add(CloseLabel.Content, 4, 0);

            Content.Children.Add(Container, 0, 0);
        }

        public void ShowClose()
        {
            Container.Children.Remove(RecycleImage.Content);
            Container.Children.Add(CloseLabel.Content, 4, 0);
        }

        public void ShowRecycle()
        {
            Container.Children.Remove(CloseLabel.Content);
            Container.Children.Add(RecycleImage.Content, 4, 0);
        }

        public void ResetContent()
        {
            ClearContent();
            Content.Children.Add(Container, 0, 0);
        }

        public void ClearContent()
        {
            Content.Children.Clear();
        }

        public void SetContent(View view)
        {
            ClearContent();
            Content.Children.Add(view, 0, 0);
        }
    }
}