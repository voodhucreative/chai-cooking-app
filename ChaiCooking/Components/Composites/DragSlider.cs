using System;
using System.Collections.Generic;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using Xamarin.Forms;
using static ChaiCooking.Helpers.Fonts;

namespace ChaiCooking.Components.Composites
{
    public class DragSlider : ActiveComponent
    {
        // the drag slider is made up of several separate elements
        StackLayout SectionContiner;

        // title
        public StaticLabel Title;

        // single slide bar point value label
        public StaticLabel BarPointValueLabel;

        public string CurrentValue { get; set; }

        public Dictionary<int, string> SliderValues;

        public Slider ValueSlider { get; set; }

        public DragSlider(string title, Dictionary<int, string> sliderValues, int width, int height)
        {
            SliderValues = sliderValues;

            Content = new Xamarin.Forms.Grid();

            SectionContiner = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Padding = new Thickness(Units.ScreenUnitL, Units.ScreenUnitM),
                Margin = new Thickness(0, Units.ScreenUnitM)
            };

            Title = new StaticLabel(title);
            Title.Content.HorizontalOptions = LayoutOptions.StartAndExpand;
            Title.Content.HorizontalTextAlignment = TextAlignment.Start;
            Title.Content.FontSize = Units.FontSizeXL;
            Title.Content.FontFamily = Fonts.GetFont(FontName.MontserratBold);
            Title.Content.TextColor = Color.White;

            BarPointValueLabel = new StaticLabel(sliderValues[0]);
            BarPointValueLabel.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            BarPointValueLabel.Content.HorizontalTextAlignment = TextAlignment.Center;
            BarPointValueLabel.Content.FontSize = Units.FontSizeL;
            BarPointValueLabel.Content.FontFamily = Fonts.GetFont(FontName.MontserratRegular);
            BarPointValueLabel.Content.TextColor = Color.White;

            ValueSlider = new Slider
            {
                Maximum = sliderValues.Count,
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.TapSizeL
            };

            ValueSlider.ValueChanged += (sender, args) =>
            {
                UpdateValue((int)ValueSlider.Value);
            };

            SectionContiner.Children.Add(Title.Content);
            SectionContiner.Children.Add(BarPointValueLabel.Content);
            SectionContiner.Children.Add(ValueSlider);

            Content.Children.Add(SectionContiner);


        }


        public string GetSelectedValue()
        {
            return BarPointValueLabel.Content.Text;// SliderValues[(int)ValueSlider.Value];
        }

        public void UpdateValue(int index)
        {
            try
            {
                BarPointValueLabel.Content.Text = SliderValues[index];
            }
            catch (Exception e)
            {
                Console.WriteLine("Error at index: " + index);
            }
        }
    }
}
