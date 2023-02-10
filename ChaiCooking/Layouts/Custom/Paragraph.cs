using System;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom
{
    public class Paragraph
    {
        private string v1;
        private string v2;

        public StackLayout Content { get; set; }
        public StaticLabel Header { get; set; }
        public StaticLabel MainContent { get; set; }
        public StaticImage Image { get; set; }

        public Paragraph(string header, string mainContent, string imageUrl)
        {
            Content = new StackLayout { Orientation = StackOrientation.Vertical, Spacing = 0, Margin = new Thickness(0, Units.ScreenUnitS) };

            Header = null;
            MainContent = null;
            Image = null;

            if (header != null)
            {
                Header = new StaticLabel(header);
                Header.Content.TextColor = Color.Black;
                Header.Content.FontSize = Units.FontSizeXL;
                Header.Content.FontFamily = Fonts.GetBoldAppFont();

                Content.Children.Add(Header.Content);
            }

            if (mainContent != null)
            {
                MainContent = new StaticLabel(mainContent);
                MainContent.Content.TextColor = Color.Black;// FromHex(Colors.CC_DARK_BLUE_GREY);
                MainContent.Content.FontSize = Units.FontSizeL;

                Content.Children.Add(MainContent.Content);
            }

            if (imageUrl != null)
            {
                Image = new StaticImage(imageUrl, Units.ScreenWidth, (int)(Units.ScreenWidth*0.6), null);
                Image.Content.VerticalOptions = LayoutOptions.CenterAndExpand;

                Content.Children.Add(Image.Content);
            }
        }

        public Paragraph(string v1, string v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public void SetTextColor(Color color)
        {
            Header.Content.TextColor = color;
            MainContent.Content.TextColor = color;
        }

        
        
    }
}
