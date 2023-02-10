using System;
using System.Collections.Generic;
using ChaiCooking.Branding;
using ChaiCooking.Helpers;
using Xamarin.Forms;

namespace ChaiCooking.Views.Custom
{
    public class IngredientPicker
    {
        Grid Content;
        Label Text;

        public Picker FoundIngredientsPicker;

        public IngredientPicker()
        {
            FormattedString titleString = new FormattedString();
            titleString.Spans.Add(new Span { Text = "Click" });
            titleString.Spans.Add(new Span
            { Text = " HERE ", ForegroundColor = Color.FromHex(Colors.CC_ORANGE),
                FontAttributes = FontAttributes.Bold,
                TextDecorations = TextDecorations.Underline
            });
            titleString.Spans.Add(new Span { Text = "to select matched ingredients.." });

            Content = new Grid
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };

            Text = new Label
            {
                FormattedText = titleString,
                InputTransparent = true,
                FontSize = Units.FontSizeL,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };

            FoundIngredientsPicker = new Picker
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = Units.ScreenWidth45Percent,
                FontSize = Units.FontSizeL,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromHex(Colors.CC_ORANGE),
                TitleColor = Color.White
            };

            FoundIngredientsPicker.SelectedIndexChanged += Selected;

            Content.Children.Add(FoundIngredientsPicker, 0, 0);
            Content.Children.Add(Text, 0, 0);
        }

        public Grid GetContent()
        {
            if(Content == null)
            {
                Content = new Grid { BackgroundColor = Color.Purple };
            }

            return Content; 
        }

        public void ShowText(bool show)
        {
            if (show)
            {
                Text.Opacity = 1;
            }
            else
            {
                Text.Opacity = 0;
            }
        }

        private void Selected(object sender, EventArgs e)
        {
            Text.Opacity = 0;
        }
    }
}
