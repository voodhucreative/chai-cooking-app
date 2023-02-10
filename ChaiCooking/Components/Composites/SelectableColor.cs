using System;
using Xamarin.Forms;

namespace ChaiCooking.Components.Composites
{
    public class SelectableColor : ActiveComponent
    {
        public string ColorValue { get; set; }
        public Color MainColor;
        public Color BgColor;

        public SelectableColor(int id, string bgColor, string colorValue)
        {
            Id = id;
            ColorValue = colorValue;
            MainColor = Color.FromHex(colorValue);
            BgColor = Color.FromHex(bgColor);

            Content = new Grid { BackgroundColor = Color.Transparent, Padding = 2};

            Container = new Grid { BackgroundColor = MainColor };

            Content.Children.Add(Container);
        }

        public void Activate()
        {
            Content.BackgroundColor = BgColor;
            //Content.ScaleTo(1.1f, 0, null);
        }

        public void Deactivate()
        {
            Content.BackgroundColor = Color.Transparent;
            //Content.ScaleTo(1.0f, 0, null);
        }


    }
}
