using System;
using System.Threading.Tasks;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Images;
using ChaiCooking.Helpers;
using Xamarin.Forms;

namespace ChaiCooking.Layouts
{
    public class Header : StandardLayout
    {
        public Header()
        {
            Height = Units.TapSizeXS;
            Width = Units.ScreenWidth;
            TransitionTime = 150;
            TransitionType = (int)AppSettings.TransitionTypes.SlideOutTop;

            Content = new Grid
            {
                WidthRequest = Width,
                HeightRequest = Height,
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(Units.ScreenHeight5Percent, 0, Units.ScreenWidth10Percent, 1),
                ColumnSpacing = 0,
                RowSpacing = 0,
                BackgroundColor = Color.FromHex(Colors.CC_WARNING_RED)
            };
        }
    }
}