using System;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers.Custom;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Components.Composites
{
    public class CheckBox
    {
        public Grid Content { get; set; }
        public StackLayout Container { get; set; }

        public string IconUncheckedImageSource { get; set; }
        public string IconCheckedImageSource { get; set; }
        public StaticImage Icon { get; set; }

        public StaticLabel Title { get; set; }

        public bool IsChecked { get; set; }

        public CheckBox()
        { }


        public CheckBox(string title, string iconCheckedImageSource, string iconUncheckedImageSource, int width, int height, bool isChecked)
        {
            Content = new Grid
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = width,
                MinimumHeightRequest = height,
            };


            Container = new StackLayout
            {
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            Title = new StaticLabel(title);
            Title.Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            Title.Content.VerticalTextAlignment = TextAlignment.Center;


            IconCheckedImageSource = iconCheckedImageSource;
            IconUncheckedImageSource = iconUncheckedImageSource;

            Icon = new StaticImage(IconUncheckedImageSource, height, height, null);

            IsChecked = isChecked;

            if (IsChecked)
            {
                Icon.Content.Source = IconCheckedImageSource;
            }

            /*Container.Children.Add(Icon.Content);
            Container.Children.Add(Title.Content);

            
            Container.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                Toggle();
                            });
                        })
                    }
                );
            */
            Content.Children.Add(Container);

            RebuildContent(true);
        }

        public void RebuildContent(bool defaultGestures)
        {
            Content.Children.Clear();
            Container = new StackLayout
            {
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Fill
            };
            /*
            Title = new StaticLabel(title);
            Title.Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            Title.Content.VerticalTextAlignment = TextAlignment.Center;


            IconCheckedImageSource = iconCheckedImageSource;
            IconUncheckedImageSource = iconUncheckedImageSource;

            Icon = new StaticImage(IconUncheckedImageSource, height, height, null);

            IsChecked = isChecked;

            if (IsChecked)
            {
                Icon.Content.Source = IconCheckedImageSource;
            }
            */
            if (defaultGestures)
            {
                TouchEffect.SetNativeAnimation(Icon.Content, true);
                TouchEffect.SetCommand(Icon.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        Toggle();
                    });
                }));
            }

            Container.Children.Add(Icon.Content);
            Container.Children.Add(Title.Content);


            Content.Children.Add(Container);
        }

        public void SetIconSize(int width, int height)
        {
            Icon.Content.WidthRequest = width;
            Icon.Content.HeightRequest = height;
        }

        public void SetCheckBoxRight()
        {

        }

        public void SetCheckboxLeft()
        {

        }

        public void Toggle()
        {

            IsChecked = !IsChecked;

            if (IsChecked)
            {
                Icon.Content.Source = IconCheckedImageSource;
            }
            else
            {
                Icon.Content.Source = IconUncheckedImageSource;
            }
        }


        public void Select()
        {

            IsChecked = true;
            Icon.Content.Source = IconCheckedImageSource;
        }

        public void UnSelect()
        {
            IsChecked = false;
            Icon.Content.Source = IconUncheckedImageSource;
        }

        public void OverrideGestures()
        {
            Container.GestureRecognizers.Clear();
            RebuildContent(false);
        }
    }
}
