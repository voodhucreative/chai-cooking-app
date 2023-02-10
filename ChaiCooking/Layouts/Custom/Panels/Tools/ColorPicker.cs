using System;
using System.Collections.Generic;
using ChaiCooking.Branding;
using ChaiCooking.Components.Composites;
using ChaiCooking.Components.Labels;
using ChaiCooking.DebugData.Custom;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Panels.Tools
{
    public class ColorPicker : StandardLayout
    {
        StaticLabel Title;
        List<List<string>> ColorValues;

        string SelectedColorValue;
        Grid ColourSelectorContainer;

        List<SelectableColor> SelectableColors;

        int ColorCount = 0;
        public bool ColorPicked { get; set; }

        public ColorPicker(string title)
        {
            Title = new StaticLabel(title);
            Title.Content.TextColor = Color.White;
            Title.Content.FontSize = Units.FontSizeM;
            Title.Content.FontFamily = Fonts.GetBoldAppFont();
            Title.CenterAlign();

            Title.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                SelectedColorValue = Colors.CC_LIGHT_BLUE;
                                RefreshColors();
                                ColorPicked = false;
                                Title.Content.TextColor = Color.White;
                                Update();
                            });

                        })
                    }
                );


            ColorValues = new List<List<string>>();

            int colorId = 0;
            foreach(string color in FakeData.FolderColors) // replace FakeData with DataManager call
            {
                ColorValues.Add(new List<string> { "#eeeeee", FakeData.FolderColors[colorId] });
                colorId++;
            }
            
            SelectedColorValue = ColorValues[0][0];

            SelectableColors = new List<SelectableColor>();

            foreach (List<string> colors in ColorValues)
            {
                SelectableColor colorBox = new SelectableColor(ColorCount, colors[0], colors[1]);

                colorBox.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                SelectedColorValue = colorBox.ColorValue;
                                RefreshColors(); 
                                colorBox.Activate();
                                ColorPicked = true;
                                Title.Content.TextColor = Color.FromHex(Colors.CC_ORANGE);
                                Update();
                            });

                        })
                    }
                );

                SelectableColors.Add(colorBox);
                ColorCount++;
            }


            // split into colour picker class

            ColourSelectorContainer = new Grid
            {
                BackgroundColor = Color.Transparent,//FromHex(Colors.CC_BLUE_GREY),
                WidthRequest = 180,
                HeightRequest = 380,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                RowSpacing = Dimensions.GENERAL_COMPONENT_SPACING,
                ColumnSpacing = Dimensions.GENERAL_COMPONENT_SPACING
            };


            ColourSelectorContainer.Children.Add(Title.Content, 0, 0);

            
            int col = 0;
            int row = 1;

           
            foreach (SelectableColor colorBox in SelectableColors)
            {
                ColourSelectorContainer.Children.Add(colorBox.Content, col, row);
                col++;
                if (col >= 4) { col = 0; row++; }
            }


            Container.Children.Add(ColourSelectorContainer);

            Grid.SetColumnSpan(Title.Content, 4);

            Content.Children.Add(Container);
        }


        private void RefreshColors()
        {
            foreach (SelectableColor colorBox in SelectableColors)
            {
                colorBox.Deactivate();
            }
        }

        public string GetSelectedColorHexValue()
        {
            return SelectedColorValue;
        }

        public Color GetSelectedColor()
        {
            return Color.FromHex(SelectedColorValue);
        }

       
    }
}
