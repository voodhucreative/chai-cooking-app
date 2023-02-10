using System;
using ChaiCooking.Helpers;
using Xamarin.Forms;

namespace ChaiCooking.Components.Tiles
{
    public class Tile : ActiveComponent
    {
        public Tile()
        {
            this.Content = new Grid
            {
                BackgroundColor = Color.LightGray,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Padding = 1
            };
        }

        public void AddContent(View content)
        {
            this.Content.Children.Add(content);
        }

    }
}
