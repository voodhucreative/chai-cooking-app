using System;
using System.Collections.Generic;
using ChaiCooking.Branding;
using ChaiCooking.Helpers.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Panels
{
    public class TabbedPanel : StandardLayout
    {
        StackLayout ContentContainer;
        StackLayout TabsContainer;
        Grid ChildPanelContainer;
        string SelectedChildPanelId;
        int NumberOfChildPanels;
        List<GeneralInfoPanel> ChildPanels;
        
        public TabbedPanel()
        {
            ChildPanels = new List<GeneralInfoPanel>();
            SelectedChildPanelId = "";
            NumberOfChildPanels = 0;

            Content.WidthRequest = Dimensions.INFO_PANEL_WIDTH;
            Content.HorizontalOptions = LayoutOptions.CenterAndExpand;

            ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 0
            };

            TabsContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Color.Transparent,//FromHex(Colors.CC_DARK_ORANGE),
                HeightRequest = Dimensions.TABBED_PANEL_HEADER_HEIGHT,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                //Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Spacing = 0
            };

            ChildPanelContainer = new Grid { };

            ContentContainer.Children.Add(TabsContainer);
            ContentContainer.Children.Add(ChildPanelContainer);



            Container.Children.Add(ContentContainer);
            Content.Children.Add(Container);
        }

        public void SetTargetPanel(GeneralInfoPanel targPanel)
        {
            SelectedChildPanelId = targPanel.Id;
            ShowPanel(SelectedChildPanelId);
        }

        public void AddChildPanel(GeneralInfoPanel childPanel)
        {
            ChildPanels.Add(childPanel);
            AddChildPanelView(childPanel);
            NumberOfChildPanels++;
            ShowPanel(SelectedChildPanelId);
        }

        public void ClearChildPanels()
        {
            ChildPanels.Clear();
            TabsContainer.Children.Clear();
            ChildPanelContainer.Children.Clear();
        }

        public void AddChildPanelView(GeneralInfoPanel childPanel)
        {
            childPanel.HideHeader();

            //childPanel.Title.Content.MinimumHeightRequest = Dimensions.PANEL_HEADER_HEIGHT + (Dimensions.GENERAL_COMPONENT_PADDING*2); // huh?
            childPanel.Title.Content.HeightRequest = Dimensions.TABBED_PANEL_HEADER_HEIGHT;
            childPanel.Title.Content.Padding = Dimensions.GENERAL_COMPONENT_PADDING;
            childPanel.Title.Content.VerticalOptions = LayoutOptions.CenterAndExpand;

            if (NumberOfChildPanels > 0)
            {
                childPanel.Title.Content.BackgroundColor = Color.FromHex(Colors.CC_ORANGE);
                childPanel.Content.IsVisible = true;
            }
            else
            {
                childPanel.Title.Content.BackgroundColor = Color.FromHex(Colors.CC_DARK_ORANGE);
                childPanel.Content.IsVisible = false;
                SelectedChildPanelId = childPanel.Id;
            }

            childPanel.Title.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {

                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                SelectedChildPanelId = childPanel.Id;
                                Console.WriteLine("Selected panel: " + SelectedChildPanelId);
                                ShowPanel(SelectedChildPanelId);
                                App.UpdateLayout(SelectedChildPanelId);
                            });

                        })
                    }
                );

            TabsContainer.Children.Add(childPanel.Title.Content);
            ChildPanelContainer.Children.Add(childPanel.GetContent(), 0, 0);
            NumberOfChildPanels++;

          
        }

        private void ShowPanel(string selectedPanelId)
        {
            foreach(GeneralInfoPanel childPanel in ChildPanels)
            {
                if (childPanel.Id == selectedPanelId)
                {
                    childPanel.Title.Content.BackgroundColor = Color.FromHex(Colors.CC_ORANGE);
                    TabsContainer.BackgroundColor = Color.FromHex(Colors.CC_DARK_ORANGE);
                    childPanel.Content.IsVisible = true;
                }
                else
                {
                    childPanel.Title.Content.BackgroundColor = Color.FromHex(Colors.CC_DARK_ORANGE);
                    TabsContainer.BackgroundColor = Color.FromHex(Colors.CC_DARK_ORANGE);
                    childPanel.Content.IsVisible = false;
                }
            }

        }

        public void SetLayoutWidth(int width)
        {
            Content.WidthRequest = width;
            ContentContainer.WidthRequest = width;
            TabsContainer.WidthRequest = width;
            ChildPanelContainer.WidthRequest = width;

            foreach (GeneralInfoPanel childPanel in ChildPanels)
            {
                childPanel.SetLayoutWidth(width);
            }
        }
    }
}
