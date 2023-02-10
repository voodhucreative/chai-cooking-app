using System;
using TechExpo.Helpers;
using Xamarin.Forms;

namespace TechExpo.Layouts.Custom
{
    public class PageTitle
    {
        public Grid Content { get; set; }
        public int Height { get; set; }
        public uint TransitionTime { get; set; }
        public Label Title { get; set; }

        public PageTitle(string titleText, Color backgroundColor, Color textColor)
        {
            TransitionTime = 500;

            Content = new Grid
            {
                BackgroundColor = backgroundColor,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            Title = new Label
            {
                FontFamily = Fonts.GetFont(FontName.MuliRegular),
                Text = titleText,
                TextColor = textColor,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontSize = Units.FontSizeXXL,
            };

            SetHeight(Units.ScreenHeight10Percent);
            Content.Children.Add(Title);
        }

        public void SetFormattedText(FormattedString formattedString)
        {
            this.Title.FormattedText = formattedString;
        }

        public void SetHeight(int height)
        {
            Height = height;
            Content.HeightRequest = height;
        }

        public void Show()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                Content.IsVisible = true;
                await Content.TranslateTo(0, 0, 100, Easing.Linear);
            });
        }

        public void Hide()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Content.TranslateTo(0, -Height, 100, Easing.Linear);
                Content.IsVisible = false;
            });
        }
    }
}
