using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Xamarin.Forms;

namespace ChaiCooking.Components.Images
{
    public class ActiveImage : Components.ActiveComponent
    {
        public CachedImage Image;
        private string v;
        private object t;

        public ActiveImage(string v, object t)
        {
            this.v = v;
            this.t = t;
        }

        public ActiveImage(string imgSource, int width, int height, List<ITransformation> transformations, Models.Action action)
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

            Image = new CachedImage()
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
