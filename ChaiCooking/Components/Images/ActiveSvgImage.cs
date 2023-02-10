using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FFImageLoading.Svg.Forms;
using FFImageLoading.Work;
using Xamarin.Forms;

namespace ChaiCooking.Components.Images
{
    public class ActiveSvgImage : ActiveComponent
    {
        public SvgCachedImage Image;
        private string v;
        private object t;

        public ActiveSvgImage(string v, object t)
        {
            this.v = v;
            this.t = t;
        }

        public ActiveSvgImage(string imgSource, int width, int height, List<ITransformation> transformations, Models.Action action)
        {
            this.Content = new Grid
            {
                HeightRequest = height,
                WidthRequest = width,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
            };

            this.Width = width;
            this.Height = height;
            this.DefaultAction = action;

            SvgCachedImage Image = new SvgCachedImage()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
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
            Content.Children.Add(Image);

            if (this.DefaultAction != null)
            {
                this.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                //await Update();
                                await DefaultAction.Execute();
                            });
                        })
                    }
                );
            }
        }

        public override async Task<bool> Update()
        {
            await this.Content.FadeTo(0, 5000, Easing.Linear);
            return true;
        }

    }
}
