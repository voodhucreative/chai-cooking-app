using System;
using ChaiCooking.Components.Images;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Tools;
using Xamarin.Forms;
using XFShapeView;
using static ChaiCooking.Helpers.Fonts;

namespace ChaiCooking.Components.Labels
{
    public class RemovableLabel : Components.ActiveComponent
    {
        public Grid Button;
        public StackLayout LabelContainer;
        public Label Label;
        //public Label Remove; // replace with image
        public StaticImage Remove;
        public ShapeView ButtonShape;
        public ShapeView DropShadow;

        public RemovableLabel(Color backgroundColor, Color textColor, string buttonText, Models.Action action)
        {
            // deafult action will be remove
            this.DefaultAction = action;

            int width = 128;
            int height = 32;

            this.Content = new Grid
            {
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                //WidthRequest = width
            };

            LabelContainer = new StackLayout {
                HeightRequest = height,
                //WidthRequest = width,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Padding = new Thickness(3, 0, 2, 0),
                Orientation = StackOrientation.Horizontal
            };
            

            ButtonShape = new ShapeView
            {
                ShapeType = ShapeType.Box,
                HeightRequest = height,
                WidthRequest = width,
                MinimumWidthRequest = 80,
                Color = backgroundColor,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                CornerRadius = height / 2,
                Opacity = 1
            };

            DropShadow = new ShapeView
            {
                ShapeType = ShapeType.Box,
                HeightRequest = height,
                //WidthRequest = width,
                Color = Color.Gray,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                CornerRadius = height / 2,
                Opacity = 0.5
            };


            Button = new Grid
            {
                BackgroundColor = Color.Transparent,
                //WidthRequest = ButtonShape.Width,
                HeightRequest = ButtonShape.Height,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Label = new Label
            {
                TextColor = textColor,
                Text = buttonText,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                VerticalTextAlignment = TextAlignment.Center,
                FontFamily = Fonts.GetFont(FontName.PoppinsBold),
                FontSize = 12,
            };

            /*
            Remove = new Label
            {
                TextColor = textColor,
                Text = "-",
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                VerticalTextAlignment = TextAlignment.Center,
                FontFamily = Fonts.GetFont(FontName.PoppinsBold)
            };*/
            Remove = new StaticImage("closecirclewhite.png", 16, null);
            Remove.Content.HorizontalOptions = LayoutOptions.EndAndExpand;
            Remove.Content.VerticalOptions = LayoutOptions.CenterAndExpand;

            if (this.DefaultAction != null)
            {
                this.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await this.DefaultAction.Execute();
                            });
                        })
                    }
                );
            }
            DropShadow.TranslateTo(3, 2, 0, null);
            //Button.Children.Add(DropShadow, 0, 0);
            Button.Children.Add(ButtonShape, 0, 0);




            LabelContainer.Children.Add(Label);
            LabelContainer.Children.Add(Remove.Content);

            this.Content.Children.Add(Button, 0, 0);



            this.Content.Children.Add(LabelContainer, 0, 0);
        }

        public void SetColour(Color colour)
        {
            this.ButtonShape.Color = colour;
        }

        public void SetText(string text)
        {
            this.Label.Text = text;
        }

        public void SetLayoutOptions(LayoutOptions horizontal, LayoutOptions vertical)
        {
            Button.HorizontalOptions = horizontal;
            Button.VerticalOptions = vertical;
            ButtonShape.HorizontalOptions = horizontal;
            ButtonShape.VerticalOptions = vertical;
        }

        public void SetSize(int width, int height)
        {
            Button.WidthRequest = width;
            Button.HeightRequest = height;
            ButtonShape.WidthRequest = width;
            ButtonShape.HeightRequest = height;

        }

        public void SetFont(string font, int fontSize)
        {
            Label.FontFamily = font;
            Label.FontSize = fontSize;
        }
    }
}
