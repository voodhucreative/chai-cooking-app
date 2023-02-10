using System;
using ChaiCooking.Components;
using ChaiCooking.Components.Images;
using Xamarin.Forms;
using XFShapeView;

namespace ChaiCooking.Layouts.Custom.Tiles
{
    public class SmallRecipeTile : ActiveComponent
    {
        Grid BackGroundLayer;
        Grid ShapeLayer1;
        Grid ShapeLayer2;
        Grid ContentLayer;

        ShapeView BackgroundSquare;
        ShapeView TopSection;
        ShapeView BottomSection;
        ShapeView MiddleSection;

        public string IconUncheckedImageSource { get; set; }
        public string IconCheckedImageSource { get; set; }
        public StaticImage Icon { get; set; }

        bool IsSelected { get; set; }

        public SmallRecipeTile(bool isSelected, int width, int height)
        {
            IsSelected = isSelected;

            int tileWidth = width;
            int tileHeight = tileWidth;
            int cornerRadius = tileWidth / (tileWidth / 12);
            int innerMargin = cornerRadius / 2;

            IconCheckedImageSource = "icon.png";
            IconUncheckedImageSource = "chaismallbag.png";

            Icon = new StaticImage(IconUncheckedImageSource, height, height, null);

            if (IsSelected)
            {
                Icon.Content.Source = IconCheckedImageSource;
            }

            // re-adjust our parent content layer rows
            Content.RowDefinitions.Add(new RowDefinition { Height = tileHeight });
            Content.RowDefinitions.Add(new RowDefinition { Height = 32 });

            BackGroundLayer = new Grid
            {
                BackgroundColor = Color.Transparent,
                WidthRequest = tileWidth,
                HeightRequest = tileHeight,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            ShapeLayer1 = new Grid
            {
                BackgroundColor = Color.Transparent,
                WidthRequest = tileWidth - innerMargin,
                HeightRequest = tileHeight - innerMargin,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            ShapeLayer2 = new Grid
            {
                BackgroundColor = Color.Transparent,
                WidthRequest = tileWidth - innerMargin,
                HeightRequest = tileHeight - innerMargin,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            ContentLayer = new Grid
            {
                BackgroundColor = Color.Transparent,
                WidthRequest = tileWidth - innerMargin * 2,
                HeightRequest = tileHeight - innerMargin * 2,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Margin = innerMargin * 8
            };

            BackgroundSquare = new ShapeView
            {
                ShapeType = ShapeType.Box,
                WidthRequest = tileWidth,
                HeightRequest = tileHeight,
                Color = Color.Orange,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                CornerRadius = cornerRadius
            };

            TopSection = new ShapeView
            {
                ShapeType = ShapeType.Box,
                WidthRequest = tileWidth - innerMargin,
                HeightRequest = tileHeight / 2,
                Color = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                CornerRadius = cornerRadius - (innerMargin / 2),
            };

            BottomSection = new ShapeView
            {
                ShapeType = ShapeType.Box,
                WidthRequest = tileWidth - innerMargin,
                HeightRequest = tileHeight / 2,
                Color = Color.Black,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                CornerRadius = cornerRadius - (innerMargin / 2),
                //Margin = 4
            };

            MiddleSection = new ShapeView
            {
                ShapeType = ShapeType.Box,
                WidthRequest = tileWidth - innerMargin,
                HeightRequest = tileHeight / 2.5,
                Color = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                //Margin = new Thickness(0, tileHeight/4, 0, tileHeight/4)
                //Margin = new Thickness(0, 16, 0, 16)
            };

            BackGroundLayer.Children.Add(BackgroundSquare);

            ShapeLayer1.Children.Add(TopSection, 0, 0);
            ShapeLayer1.Children.Add(BottomSection, 0, 1);

            ShapeLayer2.Children.Add(MiddleSection, 0, 0);

            Container.Children.Add(BackGroundLayer);
            Container.Children.Add(ShapeLayer1);
            Container.Children.Add(ShapeLayer2);





            Container.Children.Add(ContentLayer);

            if (IsSelected)
            {
                BackgroundSquare.Opacity = 1;
            }
            else
            {
                BackgroundSquare.Opacity = 0;
            }

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

            Content.Children.Add(Icon.Content, 0, 1);
            Icon.Content.TranslateTo(0, 40, 0, null);
        }

        public void Toggle()
        {
            IsSelected = !IsSelected;

            if (IsSelected)
            {
                
                BackgroundSquare.Opacity = 1;
                Icon.Content.Source = IconCheckedImageSource;
            }
            else
            {
                BackgroundSquare.Opacity = 0;
                Icon.Content.Source = IconUncheckedImageSource;
            }
        }

        public void AddContent(View content)
        {
            ContentLayer.Children.Clear();
            ContentLayer.Children.Add(content);
        }
    }
}
