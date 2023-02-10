using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Panels
{
    public class GeneralInfoPanel : StandardLayout
    {
        StackLayout PanelContainer;
        StackLayout HeaderContainer;
        StackLayout ContentContainer;
        StackLayout ButtonContainer;
        Grid BackgroundLayer;

        public StaticLabel Title { get; set; }

        List<ColourButton> Buttons;

        public Color HeaderActiveColor { get; set; }
        public Color HeaderActiveTextColor { get; set; }
        public Color HeaderInactiveColor { get; set; }
        public Color HeaderInactiveTextColor { get; set; }
        public Color PanelBackgroundColor { get; set; }
        public Color PanelTextColor { get; set; }
        public Color ButtonActiveColor { get; set; }
        public Color ButtonActiveTextColor { get; set; }
        public Color ButtonInactiveColor { get; set; }
        public Color ButtonInactiveTextColor { get; set; }

        int Theme;

        public GeneralInfoPanel(string title, int theme, int width, int height = Dimensions.INFO_PANEL_HEIGHT, int padding = Dimensions.GENERAL_COMPONENT_PADDING, bool dismissable = false)
        {
            Content.WidthRequest = width;// Dimensions.INFO_PANEL_WIDTH;
            //Content.HeightRequest = height;
            Content.HeightRequest = Units.ScreenHeight;
            Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            //Content.BackgroundColor = Color.FromHex(Colors.CC_DARK_GREY);
            //Content.Padding = 2;

            BackgroundLayer = new Grid { BackgroundColor = Color.Transparent };

            Theme = theme;

            HeaderActiveColor = Color.FromHex(Colors.CC_ORANGE);
            HeaderActiveTextColor = Color.White;
            HeaderInactiveColor = Color.FromHex(Colors.CC_ORANGE);
            HeaderInactiveTextColor = Color.White;
            PanelBackgroundColor = Color.White;
            PanelTextColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY);
            ButtonActiveColor = Color.FromHex(Colors.CC_ORANGE);
            ButtonActiveTextColor = Color.White;
            ButtonInactiveColor = Color.FromHex(Colors.CC_RED);
            ButtonInactiveTextColor = Color.White;

            SetTheme(Theme);

            Id = Guid.NewGuid().ToString();

            PanelContainer = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Spacing = 0
            };

            HeaderContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = HeaderActiveColor,
                HeightRequest = Dimensions.PANEL_HEADER_HEIGHT,
                WidthRequest = Units.ScreenWidth,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING
            };

            ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = PanelBackgroundColor,
                Padding = padding
            };

            ButtonContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = PanelBackgroundColor,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING
            };

            Buttons = new List<ColourButton>();

            Title = new StaticLabel(title);
            Title.Content.WidthRequest = Units.ScreenWidth;
            Title.Content.TextColor = HeaderActiveTextColor;
            Title.Content.FontFamily = Fonts.GetRegularAppFont();
            Title.Content.FontSize = 12;

            HeaderContainer.Children.Add(Title.Content);

            PanelContainer.Children.Add(HeaderContainer);
            PanelContainer.Children.Add(ContentContainer);
            PanelContainer.Children.Add(ButtonContainer);

            Container.Children.Add(PanelContainer);
            
            Content.Children.Add(BackgroundLayer);
            Content.Children.Add(Container);

            UpdateHeader();

            if (dismissable)
            {
                Buttons.Add(new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "OK", new Models.Action((int)Actions.ActionName.HideModal)));
            }

        }

        public void AddContent(View content)
        {
            ContentContainer.Children.Add(content);
        }

        public void AddButton(string buttonText, int buttonType, ChaiCooking.Models.Action action, bool active)
        {
            ColourButton newButton = new ColourButton(ButtonActiveColor, ButtonActiveTextColor, buttonText, action);
            newButton.Content.WidthRequest = Units.SmallButtonWidth;
            newButton.Content.HeightRequest = Units.SmallButtonHeight;
            newButton.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            newButton.IsActive = active;

            Buttons.Add(newButton);

            if(active)
            {
                newButton.Activate();
            }
            else
            {
                newButton.Deactivate();
            }

            UpdateButtons();
            
        }

        public void ActivateButton(int buttonId)
        {
            Buttons[buttonId].Activate();
        }

        public void DeactivateButton(int buttonId)
        {
            Buttons[buttonId].Deactivate();
        }

        private void UpdateButtons()
        {
            if (ButtonContainer != null)
            {
                ButtonContainer.Children.Clear();

                foreach (ColourButton button in Buttons)
                {
                    ButtonContainer.Children.Add(button.Content);
                    button.SetThemeColors(ButtonActiveColor, ButtonActiveTextColor, ButtonInactiveColor, ButtonInactiveTextColor);
                }
            }
        }

        public void UpdateHeader()
        {
            if (Theme == Themes.PANEL_THEME_GREY)
            {
                HideHeader();
            }
            else
            {
                ShowHeader();
            }

        }

        public void HideHeader()
        {
            HeaderContainer.IsVisible = false;
        }

        public void ShowHeader()
        {
            HeaderContainer.IsVisible = true;
        }

        public void SetActive()
        {
            HeaderContainer.BackgroundColor = HeaderActiveColor;
        }

        public void SetInactive()
        {
            HeaderContainer.BackgroundColor = HeaderInactiveColor;
        }

        public void SetTheme(int panelTheme)
        {
            if (panelTheme == Themes.PANEL_THEME_ORANGE_AND_WHITE)
            {
                HeaderActiveColor = Color.FromHex(Colors.CC_ORANGE);
                HeaderActiveTextColor = Color.White;
                HeaderInactiveColor = Color.FromHex(Colors.CC_ORANGE);
                HeaderInactiveTextColor = Color.White;
                PanelBackgroundColor = Color.White;
                PanelTextColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY);
                ButtonActiveColor = Color.FromHex(Colors.CC_ORANGE);
                ButtonActiveTextColor = Color.White;
                ButtonInactiveColor = Color.FromHex(Colors.CC_RED);
                ButtonInactiveTextColor = Color.White;
            }
            else
            {
                // grey theme
                //HideHeader();
                HeaderActiveColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY);
                HeaderActiveTextColor = Color.White;
                HeaderInactiveColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY);
                HeaderInactiveTextColor = Color.White;
                PanelBackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY);
                PanelTextColor = Color.White;
                ButtonActiveColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY);
                ButtonActiveTextColor = Color.White;
                ButtonInactiveColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY);
                ButtonInactiveTextColor = Color.Yellow;
            }
            UpdateButtons();
        }

        public void SetPanelBorder(int width, Color color, float opacity, bool outer)
        {
            BackgroundLayer.BackgroundColor = color;
            BackgroundLayer.Opacity = opacity;

            if (outer)
            {
                BackgroundLayer.ScaleXTo(1.02f, 0, null);
                BackgroundLayer.ScaleYTo(1.06f, 0, null);
            }
            else
            {
                Container.Padding = width;
            }
        }

        public void SetLayoutWidth(int width)
        {
            Content.WidthRequest = width;
        }

    }
}
