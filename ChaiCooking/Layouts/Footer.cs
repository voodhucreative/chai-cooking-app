using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ChaiCooking.Layouts
{
    public class Footer
    {
        public Grid Content { get; set; }
        public int Height { get; set; }
        public uint TransitionTime { get; set; }

        public Footer()
        {
            TransitionTime = 100;

            Content = new Grid
            {
                BackgroundColor = Color.LightGray
            };
        }

        public void SetHeight(int height)
        {
            Height = height;
            Content.HeightRequest = height;
        }

        public async Task<bool> Show()
        {
            Content.IsVisible = true;
            await Task.WhenAll(
                Content.TranslateTo(0, 0, TransitionTime, Easing.Linear),
                Content.FadeTo(1, TransitionTime, Easing.Linear)
                );
            return true;
        }

        public async Task<bool> Hide()
        {
            await Task.WhenAll(
                Content.TranslateTo(0, Height, TransitionTime, Easing.Linear),
                Content.FadeTo(1, TransitionTime, Easing.Linear)
                );
            Content.IsVisible = false;
            return true;
        }
    }
}
