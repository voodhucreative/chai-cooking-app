using System;
using ChaiCooking.Branding;
using ChaiCooking.Components.Composites;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts
{
    public class Section
    {
        // a section is an area that contains one or more layouts,
        // typically, with a SectionToggle, which shows / hides it's child layouts

        public bool IsToggleabe { get; set; }
        public bool IsOpen { get; set; }

        public StackLayout Content { get; set; }

        public StandardLayout MainLayout { get; set; }

        public SectionToggle Toggle { get; set; }

        public StaticLabel Title { get; set; }
        public string InfoHeader { get; set; }
        public string InfoText { get; set; }

        public Section(string titleText, bool isToggleable, bool isOpen)
        {
            InfoHeader = titleText;
            InfoText = "";

            IsToggleabe = isToggleable;
            IsOpen = isOpen;

            if (!IsToggleabe) // keep open if not toggleable
            {
                IsOpen = true;
            }

            Content = new StackLayout
            {
                Spacing = 0
            };

            MainLayout = new StandardLayout();
            MainLayout.Content = new Grid();

            Toggle = new SectionToggle(titleText);

            Title = new StaticLabel(titleText);


            // build standard layout
            // this can be overridden after creation, if necessary
            Content.Orientation = StackOrientation.Vertical;
            Content.WidthRequest = Dimensions.MENU_SECTION_WIDTH;


            Content.BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY);
            Content.Padding = 8;
            MainLayout = new StandardLayout();

            Toggle.Content.MinimumWidthRequest = Dimensions.MENU_SECTION_WIDTH;
            Toggle.Content.HeightRequest = Units.TapSizeXS;
            Toggle.Content.BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY);
            Toggle.Arrow.Color = Color.White;
            Toggle.Title.Content.TextColor = Color.White;
            Toggle.Title.Content.FontFamily = Fonts.GetBoldAppFont();

            Title.Content.MinimumWidthRequest = Dimensions.MENU_SECTION_WIDTH;
            Title.Content.HeightRequest = Units.TapSizeXS;
            Title.Content.BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY);
            Title.Content.FontFamily = Fonts.GetBoldAppFont();



            MainLayout.Container = new Grid
            {

                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                Padding = 8
            };

            MainLayout.Content = new Grid
            {
                WidthRequest = Dimensions.MENU_SECTION_WIDTH,
                //HeightRequest = Dimensions.MENU_SECTION_HEIGHT,

                BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY),
                Padding = 1 // create a border
            };

            MainLayout.Content.Children.Add(MainLayout.Container);

            if (IsToggleabe)
            {
                Content.Children.Add(Toggle.Content);
            }
            else
            {
                Content.Children.Add(Title.Content);
            }

            Content.Children.Add(MainLayout.Content);

            if (IsOpen)
            {
                Open();
            }
            else
            {
                Close();
            }


            TouchEffect.SetNativeAnimation(Toggle.Content, true);
            TouchEffect.SetCommand(Toggle.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        Paragraph itemInfo = new Paragraph(InfoHeader, InfoText, null);
                        App.ShowInfoBubble(itemInfo.Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                    }
                    else
                    {
                        ToggleVisibility();
                    }
                });
            }));
        }

        public void Open()
        {
            MainLayout.Content.IsVisible = true;
            Toggle.SetIsOpen(true);
        }

        public void Close()
        {
            MainLayout.Content.IsVisible = false;
            Toggle.SetIsOpen(false);
        }

        public void HideTitle()
        {
            Title.Content.IsVisible = false;
        }

        public void SetInfo(string infoHeader, string infoText)
        {
            InfoHeader = infoHeader;
            InfoText = infoText;
        }

        public void ToggleVisibility()
        {
            if (IsToggleabe)
            {
                if (MainLayout.Content.IsVisible)
                {
                    Close();
                }
                else
                {
                    Open();
                }
            }
        }

        public void AddContent(View contentView)
        {
            MainLayout.Container.Children.Add(contentView);
        }

        public void Update()
        {

        }

        public void SetBorder(int borderWidth, Color borderColor)
        {
            MainLayout.Content.BackgroundColor = borderColor;
            MainLayout.Container.Margin = borderWidth;
            Content.Padding = 8;
            //Content.Margin = borderWidth;
        }

    }
}
