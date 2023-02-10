using System;
using System.Collections.Generic;
using FFImageLoading.Forms;
using FFImageLoading.Work;
using Xamarin.Forms;

namespace ChaiCooking.Components.Images
{
    public class StaticImage
    {
        public CachedImage Content;

        public StaticImage(string imgSource, int width, int height, List<ITransformation> transformations)
        {
            this.Content = new CachedImage()
            {

                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Aspect = Aspect.AspectFit,
                CacheDuration = TimeSpan.FromDays(30),
                DownsampleToViewSize = true,
                RetryCount = 3,
                RetryDelay = 250,
                LoadingPlaceholder = "no_image.png",
                ErrorPlaceholder = "no_image.png",
                Source = imgSource,
                HeightRequest = height,
                WidthRequest = width,
                Transformations = transformations
            };
        }

        public StaticImage(string imgSource, int width, List<ITransformation> transformations)
        {
            this.Content = new CachedImage()
            {

                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Aspect = Aspect.AspectFit,
                CacheDuration = TimeSpan.FromDays(30),
                DownsampleToViewSize = true,
                RetryCount = 3,
                RetryDelay = 250,
                LoadingPlaceholder = "no_image.png",
                ErrorPlaceholder = "no_image.png",
                Source = imgSource,
                WidthRequest = width,
                Transformations = transformations
            };
        }

        public void CenterAlignVertical()
        {
            Content.VerticalOptions = LayoutOptions.CenterAndExpand;
        }
    }
}
