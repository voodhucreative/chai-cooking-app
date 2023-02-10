using System;
using ChaiCooking.Components.Images;
using ChaiCooking.Helpers;
using Xamarin.Forms;

namespace ChaiCooking
{
    public class Layer
    {
        public Grid Layout { get; set; }
        public StaticImage BackgroundImage;
        public string BackgroundImageSource;
        public bool DefaultEventDisabled { get; set; }

        public Layer()
        {
            Layout = new Grid
            {
                RowSpacing = 0,
                ColumnSpacing = 0
            };

            DefaultEventDisabled = false;
        }

        public Layer(int width, int height)
        {
            Layout = new Grid
            {
                WidthRequest = width,
                HeightRequest = height,
                RowSpacing = 0,
                ColumnSpacing = 0
            };

            DefaultEventDisabled = false;
        }

        public virtual void AddBackgroundImage(string backgroundImageSource)
        {
            BackgroundImageSource = backgroundImageSource;
            BackgroundImage = new StaticImage(
            BackgroundImageSource,
            Units.ScreenWidth,
            Units.ScreenHeight,
            null);
            BackgroundImage.Content.Aspect = Aspect.Fill;
            Layout.Children.Add(BackgroundImage.Content);
        }

        public virtual void SetBackGroundImageAspect(Aspect aspect)
        {
            if (BackgroundImage != null)
            {
                BackgroundImage.Content.Aspect = aspect;
            }
            
        }

        public void Activate()
        {
            Layout.IsEnabled = true;
            Layout.IsVisible = true;
        }

        public void Deactivate()
        {
            Layout.IsEnabled = false;
            Layout.IsVisible = false;
        }

        public bool IsActive()
        {
            return Layout.IsEnabled;
        }

    }
}
