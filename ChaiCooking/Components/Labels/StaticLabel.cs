using System;
using ChaiCooking.Helpers;
using Xamarin.Forms;

namespace ChaiCooking.Components.Labels
{
    public class StaticLabel
    {
        
        public string ComponentInfo { get; set; }

        public Label Content;

        public StaticLabel(string text)
        {
            ComponentInfo = null;

            this.Content = new Label
            {
                Text = text,
                FontFamily = ChaiCooking.Helpers.Fonts.GetRegularAppFont(),
                TextColor = Color.Black
            };
        }

        public void RightAlign()
        {
            Content.HorizontalOptions = LayoutOptions.EndAndExpand;
            Content.HorizontalTextAlignment = TextAlignment.End;

            CenterAlignVertical();
        }

        public void LeftAlign()
        {
            Content.HorizontalOptions = LayoutOptions.StartAndExpand;
            Content.HorizontalTextAlignment = TextAlignment.Start;

            CenterAlignVertical();
        }

        public void CenterAlign()
        {
            Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            Content.HorizontalTextAlignment = TextAlignment.Center;

            CenterAlignVertical();
        }

        public void CenterAlignVertical()
        {
            Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            Content.VerticalTextAlignment = TextAlignment.Center;
        }
    }
}
