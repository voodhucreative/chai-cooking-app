using System;
using ChaiCooking.Helpers;
using ChaiCooking.Tools;
using Xamarin.Forms;
using XFShapeView;
using static ChaiCooking.Helpers.Fonts;

namespace ChaiCooking.Components.Buttons
{
    public class ColourButton : Components.ActiveComponent
    {
        public Grid Button;
        public Label Label;
        public ShapeView ButtonShape;
        public ShapeView DropShadow;
        public bool IsActive;

        public Color BackgroundColorActive;
        public Color BackgroundColorInactive;

        public Color TextColorActive;
        public Color TextColorInactive;

        public Models.Action Action;

        public ColourButton(Color backgroundColor, Color textColor, string buttonText, Models.Action action)
        {
            this.DefaultAction = action;
            this.IsActive = true;

            this.BackgroundColorActive = backgroundColor;
            this.TextColorActive = textColor;

            this.BackgroundColorInactive = Color.LightGray;
            this.TextColorInactive = Color.Gray;

            this.Action = action;

            this.Content = new Grid
            {
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.Center
            };

            ButtonShape = new ShapeView
            {
                ShapeType = ShapeType.Box,
                HeightRequest = Units.TapSizeM,
                WidthRequest = Units.LargeButtonWidth,
                Color = BackgroundColorActive,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                CornerRadius = Units.TapSizeM/2,
                Opacity = 1
            };

            DropShadow = new ShapeView
            {
                ShapeType = ShapeType.Box,
                HeightRequest = Units.TapSizeM,
                WidthRequest = Units.LargeButtonWidth,
                Color = Color.Gray,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                CornerRadius = Units.TapSizeM / 2,
                Opacity = 0.5
            };


            Button = new Grid
            {
                BackgroundColor = Color.Transparent,
                WidthRequest = ButtonShape.Width,
                HeightRequest = ButtonShape.Height,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Label = new Label
            {
                TextColor = TextColorActive,
                Text = buttonText,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                VerticalTextAlignment = TextAlignment.Center,
                FontFamily = Fonts.GetFont(FontName.PoppinsBold)
            };

            /*if (this.IsActive)
            {
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
            }*/

            /*
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
            );*/

            this.Content.GestureRecognizers.Add(
                new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            if (IsActive)
                            {
                                if (this.DefaultAction != null)
                                {
                                    await this.DefaultAction.Execute();
                                }
                                else
                                {
                                    //App.ShowAlert("No action linked with this component");
                                }
                            }
                        });
                    })
                }
            );

            DropShadow.TranslateTo(3, 2, 0, null);
            //Button.Children.Add(DropShadow, 0, 0);
            Button.Children.Add(ButtonShape, 0, 0);

            this.Content.Children.Add(Button, 0, 0);
            this.Content.Children.Add(Label, 0, 0);
        }

        public void Activate()
        {
            ButtonShape.Color = BackgroundColorActive;
            Label.TextColor = TextColorActive;
            DefaultAction = Action;
            IsActive = true;
        }

        public void Deactivate()
        {
            ButtonShape.Color = BackgroundColorInactive;
            Label.TextColor = TextColorInactive;
            DefaultAction = null;
            IsActive = false;
        }

        public void SetColour(Color colour)
        {
            this.ButtonShape.Color = colour;
        }

        public void SetThemeColors(Color buttonActiveColor, Color textActiveColor, Color buttonInactiveColor, Color textInactiveColor)
        {
            BackgroundColorActive = buttonActiveColor;
            TextColorActive = textActiveColor;
            BackgroundColorInactive = buttonInactiveColor;
            TextColorInactive = textInactiveColor;
            this.ButtonShape.Color = BackgroundColorActive;
            this.Label.TextColor = TextColorActive;
            UpdateStateColor();
        }

        public void UpdateStateColor()
        {
            if (IsActive)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }
        }

        public void SetText(string text)
        {
            this.Label.Text = text;
        }

        public void LeftAlign()
        {
            SetLayoutOptions(LayoutOptions.StartAndExpand, LayoutOptions.CenterAndExpand);

        }

        public void RightAlign()
        {
            SetLayoutOptions(LayoutOptions.EndAndExpand, LayoutOptions.CenterAndExpand);

           
        }

        public void CenterAlign()
        {
            SetLayoutOptions(LayoutOptions.CenterAndExpand, LayoutOptions.CenterAndExpand);

         
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
