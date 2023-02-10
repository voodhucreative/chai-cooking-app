using System;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Images;
using ChaiCooking.Helpers;
using Xamarin.Forms;
using static ChaiCooking.Helpers.Fonts;

namespace ChaiCooking.Layouts.Custom
{
    public class BottomSection
    {
        public Grid Content;
        Image TopElement;
        public Label TitleLabel;

        StackLayout ContentContainer;
        StackLayout SocialLayoutContainer;
        Image TwitterIcon;
        Image LinkedInIcon;
        Image FacebookIcon;

        public BottomSection(string title)
        {
            Content = new Grid { VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.End, BackgroundColor = Color.Transparent };

            TopElement = new Image { Source = "bottom_section.png", VerticalOptions = LayoutOptions.End, WidthRequest = Units.ScreenWidth, HeightRequest = Units.HalfScreenWidth, Aspect = Aspect.AspectFill };
            TitleLabel = new Label {
                Text = title,
                HorizontalOptions = LayoutOptions.End,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.End,
                FontSize = Units.DynamicFontSizeM,
                FontFamily = Fonts.GetFont(FontName.PoppinsBold),
                TextColor = Color.White,
                WidthRequest = Units.ScreenWidth,
                VerticalOptions = LayoutOptions.End
            };


            SocialLayoutContainer = new StackLayout { Orientation = StackOrientation.Horizontal , HorizontalOptions = LayoutOptions.End};
            ContentContainer = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.End, Margin = new Thickness(Units.ScreenUnitL) };

            TwitterIcon = new Image { Source = "twitter.png", Margin = Units.ScreenUnitXS, WidthRequest = Units.TapSizeM, HeightRequest = Units.TapSizeM };
            LinkedInIcon = new Image { Source = "linkedin.png", Margin = Units.ScreenUnitXS, WidthRequest = Units.TapSizeM, HeightRequest = Units.TapSizeM };
            FacebookIcon = new Image { Source = "facebook.png", Margin = Units.ScreenUnitXS, WidthRequest = Units.TapSizeM, HeightRequest = Units.TapSizeM };

            if (Device.Idiom != TargetIdiom.Tablet)
            {
                TwitterIcon.WidthRequest = Units.TapSizeXS;
                TwitterIcon.HeightRequest = Units.TapSizeXS;

                LinkedInIcon.WidthRequest = Units.TapSizeXS;
                LinkedInIcon.HeightRequest = Units.TapSizeXS;

                FacebookIcon.WidthRequest = Units.TapSizeXS;
                FacebookIcon.HeightRequest = Units.TapSizeXS;

                TitleLabel.FontSize = Units.DynamicFontSizeL;

            }


            if (Device.RuntimePlatform == Device.iOS)
            {
                TitleLabel.FontSize = Units.FontSizeL;

            }

            TwitterIcon.GestureRecognizers.Add(
                new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            Device.OpenUri(new Uri(AppSettings.TwitterUrl));
                        });
                    })
                }
            );

            LinkedInIcon.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                Device.OpenUri(new Uri(AppSettings.LinkedInUrl));
                            });
                        })
                    }
                );

            FacebookIcon.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                Device.OpenUri(new Uri(AppSettings.FacebookUrl));
                            });
                        })
                    }
                );

            SocialLayoutContainer.Children.Add(TwitterIcon);
            SocialLayoutContainer.Children.Add(LinkedInIcon);
            SocialLayoutContainer.Children.Add(FacebookIcon);

            ContentContainer.Children.Add(TitleLabel);
            ContentContainer.Children.Add(SocialLayoutContainer);


            Content.Children.Add(TopElement, 0, 0);
            Content.Children.Add(ContentContainer, 0, 0);  
        }

        public void HideSmallTopElement()
        {
            TopElement.Opacity = 0;
        }

        public void ShowSmallTopElement()
        {
            TopElement.Opacity = 1;
        }

        public void ShowMenuClose()
        {

        }

        public void ShowMenuOpen()
        {

        }
    }
}
