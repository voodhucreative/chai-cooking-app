using System;
using ChaiCooking.Components.Images;
using ChaiCooking.Helpers;
using Xamarin.Forms;
using XFShapeView;
using static ChaiCooking.Helpers.Fonts;

namespace ChaiCooking.Components.Buttons
{
    public class IconButton : Components.ActiveComponent
    {
        public Grid Button;
        public Label Label;
        public StaticImage Icon;
        public ShapeView ButtonShape;
        public StackLayout ContentContainer;

        public IconButton(int width, int height, Color backgroundColor, Color textColor, string buttonText, string iconSource, Models.Action action)
        {
            this.DefaultAction = action;

            this.Content = new Grid
            {
                WidthRequest = width,
                HeightRequest = height,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            ButtonShape = new ShapeView
            {
                ShapeType = ShapeType.Box,
                HeightRequest = height,
                WidthRequest = width,
                Color = backgroundColor,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                CornerRadius = height/2,
                Opacity = 1
            };


            Button = new Grid
            {
                BackgroundColor = Color.Transparent,
                WidthRequest = width,
                HeightRequest = height,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };


            //Button.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(Units.TapSizeL) });
            //Button.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Icon = new StaticImage(
                    iconSource,
                    width,
                    height, null);

            

            Icon.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            
            Label = new Label
            {
                TextColor = textColor,
                Text = buttonText,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontSize= 12,
                FontFamily = Fonts.GetFont(FontName.PoppinsBold)
            };

            Button.Children.Add(ButtonShape, 0, 0);

            ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                //WidthRequest = Units.SmallButtonWidth,
                //Padding = new Thickness(Units.ScreenUnitXS, 0),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.Transparent
            };

            ContentContainer.Children.Add(Icon.Content);
            if (buttonText.Length > 0)
            {
                ContentContainer.Children.Add(Label);
            }

            //Button.Children.Add(Icon.Content, 0, 0);
            //Button.Children.Add(Label, 1, 0);
            Button.Children.Add(ContentContainer, 0, 0);
            //Grid.SetColumnSpan(ButtonShape, 2);
            //Grid.SetColumnSpan(ContentContainer, 2);


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

            this.Content.Children.Add(Button, 0, 0);
        }

        public void SetPositionLeft()
        {
            Content.HorizontalOptions = LayoutOptions.StartAndExpand;
        }

        public void SetPositionCenter()
        {
            Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
        }

        public void SetPositionRight()
        {
            Content.HorizontalOptions = LayoutOptions.EndAndExpand;
        }

        public void SetContentLeft()
        {
            //Content.HorizontalOptions = LayoutOptions.StartAndExpand;
            Button.HorizontalOptions = LayoutOptions.StartAndExpand;
            ContentContainer.HorizontalOptions = LayoutOptions.StartAndExpand;
        }

        public void SetContentCenter()
        {
            Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            Icon.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            Button.HorizontalOptions = LayoutOptions.CenterAndExpand;
            ContentContainer.HorizontalOptions = LayoutOptions.CenterAndExpand;
        }

        public void SetContentRight()
        {
            //Content.HorizontalOptions = LayoutOptions.EndAndExpand;
            Button.HorizontalOptions = LayoutOptions.EndAndExpand;
            ContentContainer.HorizontalOptions = LayoutOptions.EndAndExpand;
        }

        public void SetIconSize(int width, int height)
        {
            Icon.Content.WidthRequest = width;
            Icon.Content.HeightRequest = height;
        }

        public void SetIconLeft()
        {
            ContentContainer.Children.Clear();
            ContentContainer.Children.Add(Icon.Content);
            if (Label.Text.Length > 0)
            {
                ContentContainer.Children.Add(Label);
            }
        }

        public void SetIconRight()
        {
            ContentContainer.Children.Clear();
            
            if (Label.Text.Length > 0)
            {
                ContentContainer.Children.Add(Label);
            }
            ContentContainer.Children.Add(Icon.Content);
        }
    }
}
