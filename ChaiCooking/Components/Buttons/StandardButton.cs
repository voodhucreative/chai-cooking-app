using System;
using System.Threading.Tasks;
using ChaiCooking.Helpers;
using ChaiCooking.Tools;
using Xamarin.Forms;
using XFShapeView;

namespace ChaiCooking.Components.Buttons
{
    public class StandardButton : Components.ActiveComponent
    {
        public Grid Button;
        public Label Label;
        public ShapeView ButtonShape;

        public StandardButton(int width, int height, Color backgroundColor, Color textColor, string buttonText, Models.Action action)
        {
            this.DefaultAction = action;

            this.Content = new Grid 
            {
                WidthRequest = width,
                HeightRequest = height,
            };

            ButtonShape = new ShapeView
            {
                ShapeType = ShapeType.Box,
                HeightRequest = 4,
                WidthRequest = Units.ScreenWidth,
                Color = backgroundColor,
                HorizontalOptions = LayoutOptions.Center,
                CornerRadius = 5,
                Opacity = 1
            };


            Button = new Grid
            {
                BackgroundColor = Color.Transparent,
                WidthRequest = width,
                HeightRequest = height,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            Label = new Label
            {
                TextColor = textColor,
                Text = buttonText,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            if (this.DefaultAction != null)
            {
                this.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await ShowClicked();
                                await this.DefaultAction.Execute();
                                await ShowUnclicked();
                            });
                            
                        })
                    }
                );
            }

            Button.Children.Add(ButtonShape, 0, 0);
            this.Content.Children.Add(Button, 0, 0);
            this.Content.Children.Add(Label, 0, 0);
        }

        public override async Task<bool> ShowClicked()
        {
            await Task.Delay(10);
            await Content.ScaleTo(0.95, 50, Easing.Linear);
            return true;
        }

        public override async Task<bool> ShowUnclicked()
        {
            await Task.Delay(10);
            await Content.ScaleTo(1.0, 50, Easing.Linear);
            return true;
        }

        public override async Task<bool> ShowFocussed()
        {
            await Task.Delay(10);
            return true;
        }

        public override async Task<bool> ShowUnfocussed()
        {
            await Task.Delay(10);
            return true;
        }
    }
}
