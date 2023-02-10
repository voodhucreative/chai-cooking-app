using System;
using ChaiCooking.Components.Images;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Tools;
using Xamarin.Forms;
using XFShapeView;
using static ChaiCooking.Helpers.Fonts;

namespace ChaiCooking.Components.Buttons
{
    public class ImageButton : Components.ActiveComponent
    {
        public Label Label;
       
        public bool IsActive;

        public Color TextColorActive;
        public Color TextColorInactive;

        public StaticImage ActiveStateImage;
        public StaticImage InactiveStateImage;

        public Models.Action Action;

        public ImageButton(string activeImagePath, string inactiveImagePath, string buttonText, Color textColor, Models.Action action)
        {
            this.DefaultAction = action;
            this.IsActive = true;

            this.TextColorActive = textColor;

            this.TextColorInactive = Color.Gray;

            this.Action = action;

            this.Content = new Grid
            {
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.Center
            };


            ActiveStateImage = new StaticImage(activeImagePath, 240, null);
            InactiveStateImage = new StaticImage(inactiveImagePath, 240, null);

            Label = new Label
            {
                TextColor = TextColorActive,
                Text = buttonText,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                VerticalTextAlignment = TextAlignment.Center,
                FontFamily = Fonts.GetFont(FontName.PoppinsBold),
                FontSize = Units.FontSizeM,
                Margin = new Thickness(0, 0, 16, 0)

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


            this.Content.Children.Add(InactiveStateImage.Content, 0, 0);
            this.Content.Children.Add(ActiveStateImage.Content, 0, 0);

            this.Content.Children.Add(Label, 0, 0);
        }

        public void Activate()
        {
            Label.TextColor = TextColorActive;
            DefaultAction = Action;
            IsActive = true;
        }

        public void Deactivate()
        {
           
            Label.TextColor = TextColorInactive;
            DefaultAction = null;
            IsActive = false;
        }

       

        public void SetThemeColors(Color buttonActiveColor, Color textActiveColor, Color buttonInactiveColor, Color textInactiveColor)
        {
            
            TextColorActive = textActiveColor;
            
            TextColorInactive = textInactiveColor;
            
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
            ActiveStateImage.Content.HorizontalOptions = horizontal;
            InactiveStateImage.Content.HorizontalOptions = horizontal;


            ActiveStateImage.Content.VerticalOptions = vertical;
            InactiveStateImage.Content.VerticalOptions = vertical;

            Label.HorizontalOptions = horizontal;

            if (Label.HorizontalOptions.Equals(LayoutOptions.Center) || Label.HorizontalOptions.Equals(LayoutOptions.CenterAndExpand))
            {
                Label.HorizontalTextAlignment = TextAlignment.Center;
            }

            if (Label.HorizontalOptions.Equals(LayoutOptions.Start) || Label.HorizontalOptions.Equals(LayoutOptions.StartAndExpand))
            {
                Label.HorizontalTextAlignment = TextAlignment.Center;
            }

            if (Label.HorizontalOptions.Equals(LayoutOptions.End) || Label.HorizontalOptions.Equals(LayoutOptions.EndAndExpand))
            {
                Label.HorizontalTextAlignment = TextAlignment.Center;
            }

        }

        public void SetSize(int width, int height)
        {
            ActiveStateImage.Content.WidthRequest = width;
            ActiveStateImage.Content.HeightRequest = height;
            InactiveStateImage.Content.WidthRequest = width;
            InactiveStateImage.Content.HeightRequest = height;
            Label.WidthRequest = width;
            Label.HeightRequest = height;

        }

        public void SetFont(string font, int fontSize)
        {
            Label.FontFamily = font;
            Label.FontSize = fontSize;
        }
    }
}
