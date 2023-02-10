using System;
using System.Threading.Tasks;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Tools;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom
{
    public class SubHeader : StandardLayout
    {
        const int MODE_MAIN = 0;
        const int MODE_CLOSE = 1;


        StackLayout ContentContainer;
        ActiveImage Logo;
        IconButton MenuButton;

        ActiveImage InfoIcon;
        ActiveImage HomeIcon;

        ActiveLabel TitleLabel;

        ActiveLabel CloseLabel;

        int Mode;

        public SubHeader()
        {
            Mode = MODE_MAIN;
            Height = Dimensions.SUBHEADER_HEIGHT;
            Width = Units.ScreenWidth;
            TransitionTime = 150;
            TransitionType = (int)AppSettings.TransitionTypes.SlideOutRight;

            Content = new Grid
            {
                HeightRequest = Dimensions.HEADER_HEIGHT,//Units.ScreenHeight15Percent,
                VerticalOptions = LayoutOptions.Start,
                //Padding = new Thickness(Dimensions.HEADER_PADDING),// 0, Units.ScreenWidth10Percent, 1),
                Padding = new Thickness(0, 0, 0, 2),
                ColumnSpacing = 0,
                RowSpacing = 0,
                BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY)
            };

            Container = new Grid
            {
                WidthRequest = Width,
                HeightRequest = Height,
                VerticalOptions = LayoutOptions.Start,
                Padding = 0,//new Thickness(Units.ScreenHeight5Percent, 0, Units.ScreenWidth10Percent, 1),
                ColumnSpacing = 0,
                RowSpacing = 0,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY)
            };

            ContentContainer = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };

            TitleLabel = new ActiveLabel("", Dimensions.HEADER_LABEL_FONT_SIZE, Color.FromHex(Colors.CC_BLUE_GREY), Color.White, null);
            TitleLabel.Label.FontFamily = Fonts.GetBoldAppFont();
            TitleLabel.Label.Margin = new Thickness(Dimensions.HEADER_PADDING, 0, Dimensions.GENERAL_COMPONENT_PADDING, 0);
            TitleLabel.LeftAlign();

            InfoIcon = new ActiveImage("infoicon.png", Dimensions.STANDARD_ICON_WIDTH, Dimensions.STANDARD_ICON_HEIGHT, null, null);
            InfoIcon.Image.HorizontalOptions = LayoutOptions.CenterAndExpand;
            InfoIcon.Image.VerticalOptions = LayoutOptions.CenterAndExpand;
            InfoIcon.Content.Margin = new Thickness(Dimensions.GENERAL_COMPONENT_SPACING, 0);

            TouchEffect.SetNativeAnimation(InfoIcon.Content, true);
            TouchEffect.SetCommand(InfoIcon.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        AppSession.InfoModeOn = !AppSession.InfoModeOn;
                        if (AppSession.InfoModeOn)
                        {
                            App.ShowCurrentPageInfo();
                        }
                        this.Update();
                    });
                }));

            if (AppSession.InfoModeOn)
            {
                InfoIcon.Image.Source = "infoiconon.png";
            }

            HomeIcon = new ActiveImage("homeicon.png", Dimensions.STANDARD_ICON_WIDTH, Dimensions.STANDARD_ICON_HEIGHT, null, null);
            HomeIcon.Image.HorizontalOptions = LayoutOptions.CenterAndExpand;
            HomeIcon.Image.VerticalOptions = LayoutOptions.CenterAndExpand;
            HomeIcon.Content.Margin = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING, 0, 24, 0);

            TouchEffect.SetNativeAnimation(HomeIcon.Content, true);
            TouchEffect.SetCommand(HomeIcon.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (AppSession.InfoModeOn)
                        {
                            double x = Tools.Screen.GetScreenCoordinates(HomeIcon.Content).X;
                            double y = Tools.Screen.GetScreenCoordinates(HomeIcon.Content).Y;
                            //App.ShowInfoBubble(new Label { Text = "Will always take you back to the home page." }, (int)x + HomeIcon.Width / 2, (int)y);
                            App.ShowInfoBubble(new Paragraph("Home", "Will always take you back to the home page.", null).Content, (int)Units.HalfScreenWidth, (int)Units.HalfScreenHeight);

                        }
                        else
                        {
                            await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Landing);
                            //new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Landing)
                        }
                    });
                }));

            CloseLabel = new ActiveLabel("Close", Units.FontSizeM, Color.Transparent, Color.White, null);
            CloseLabel.Content.Margin = new Thickness(Dimensions.GENERAL_COMPONENT_SPACING, 0);
            CloseLabel.CenterAlign();
            TouchEffect.SetNativeAnimation(CloseLabel.Content, true);
            TouchEffect.SetCommand(CloseLabel.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        AppSession.InfoModeOn = false;
                        Mode = MODE_MAIN;
                        this.Update();
                    });
                }));

            if (Mode == MODE_MAIN)
            {
                
                if (Helpers.Pages.GetCurrentPageId() != (int)AppSettings.PageNames.Landing)
                {
                    ContentContainer.Children.Add(HomeIcon.Content);
                }

                if (AppSettings.InfoModeEnabled)
                {
                    ContentContainer.Children.Add(InfoIcon.Content);
                }
            }
            else
            {
                ContentContainer.Children.Add(CloseLabel.Content);
            }


            //Container.Children.Add(ContentContainer);
            //Content.Children.Add(TitleLabel.Content, 0, 0);
            //Content.Children.Add(Container, 1, 0);

            Container.Children.Add(TitleLabel.Content, 0, 0);
            Container.Children.Add(ContentContainer, 2, 0);

            Grid.SetColumnSpan(TitleLabel.Content, 2);

            Content.Children.Add(Container);
        }

        public new void Update()
        {
            if (AppSession.InfoModeOn)
            {
                InfoIcon.Image.Source = "infoiconon.png";
            }
            else
            {
                InfoIcon.Image.Source = "infoicon.png";
            }

            switch (Mode)
            {
                case MODE_MAIN:
                    ContentContainer.Children.Clear();
                    
                    if (Helpers.Pages.GetCurrentPageId() != (int)AppSettings.PageNames.Landing)
                    {
                        //Only add the home icon if we're not on the landing page!
                        ContentContainer.Children.Add(HomeIcon.Content);
                    }
                    if (AppSettings.InfoModeEnabled)
                    {
                        ContentContainer.Children.Add(InfoIcon.Content);
                    }
                    break;
                case MODE_CLOSE:
                    ContentContainer.Children.Clear();
                    ContentContainer.Children.Add(CloseLabel.Content);
                    break;
            }
        }

        public void SetTitle(string text, ChaiCooking.Models.Action action)
        {
            TitleLabel.Label.Text = text;

            TitleLabel.DefaultAction = action;

            TouchEffect.SetNativeAnimation(TitleLabel.Content, true);
            TouchEffect.SetCommand(TitleLabel.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (AppSession.InfoModeOn)
                        {
                            /*
                            if (TitleLabel.ComponentInfo != null)
                            {
                                

                                double x = 80;
                                double y = Tools.Screen.GetScreenCoordinates(TitleLabel.Content).Y;
                                App.ShowInfoBubble(new Label { Text = TitleLabel.ComponentInfo }, (int)x + TitleLabel.Width / 2, (int)y);
                            }*/
                            if (AppSession.InfoModeOn)
                            {
                                double x = 80;
                                double y = Tools.Screen.GetScreenCoordinates(TitleLabel.Content).Y;
                                //App.ShowInfoBubble(new Label { Text = "Global Feature - find recipes based on your filter choices and stored favourites. The content will update regularly so keep checking back in." }, (int)x + TitleLabel.Width / 2, (int)y);
                                /*
                                App.ShowInfoBubble(new Label
                                {
                                    Text = "This menu can be opened in any CHAI page and contains a search tool, a library, filters, settings and more..."
                                }, (int)x + TitleLabel.Width / 2, (int)y);
                                */
                                App.ShowInfoBubble(new Paragraph("Recommended Recipes", "Global Feature - find recipes based on your filter choices and stored favourites. The content will update regularly so keep checking back in.", null).Content, (int)Units.HalfScreenWidth, (int)Units.HalfScreenHeight);

                            }
                        }
                        else
                        {
                            // if info is enabled, show the info or...
                            await TitleLabel.DefaultAction.Execute();
                        }
                    });
                }));
        }

        public void SetTitle(string text, Action action)
        {
            TitleLabel.Label.Text = text;

            TouchEffect.SetNativeAnimation(TitleLabel.Content, true);
            TouchEffect.SetCommand(TitleLabel.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (AppSession.InfoModeOn)
                        {
                            /*if (TitleLabel.ComponentInfo != null)
                            {
                                double x = 80;
                                double y = Tools.Screen.GetScreenCoordinates(TitleLabel.Content).Y;
                                App.ShowInfoBubble(new Label { Text = TitleLabel.ComponentInfo }, (int)x + TitleLabel.Width / 2, (int)y);
                            }*/
                        }
                        else
                        {
                            // if info is enabled, show the info or...
                            action();
                        }
                    });
                }));
        }

        public void SetDescription(string componentInfo)
        {
            TitleLabel.ComponentInfo = null;// componentInfo;
        }

        public void SetTitleActivated()
        {
            TitleLabel.Label.TextColor = Color.FromHex(Colors.CC_ORANGE);
        }

        public void SetTitleDeactivated()
        {
            TitleLabel.Label.TextColor = Color.White;
        }

        public void ClearTitle()
        {
            if (TitleLabel != null)
            {
                if (TitleLabel.Label != null)
                {
                    TitleLabel.Label.Text = "";
                }

                if (TitleLabel.Actions != null)
                {
                    TitleLabel.Actions.Clear();
                }

            }
        }

        public void SwitchToCloseMode()
        {
            Mode = MODE_CLOSE;
        }

        public void SwitchToMainMode()
        {
            Mode = MODE_MAIN;
        }
    }
}