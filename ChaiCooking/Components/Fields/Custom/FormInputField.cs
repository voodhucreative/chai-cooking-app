using System;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Components.Fields.Custom
{
    public class FormInputField : ActiveComponent
    {
        StaticLabel Title;
        StaticLabel CustomPlaceHolder;
        public CustomEntry TextEntry;
        StackLayout ContentContainer;

        bool Required;

        public FormInputField(string title, string placeholder, Keyboard keyboard, bool required)
        {
            Required = required;

            Content = new Grid();
            Container = new Grid();
            ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HeightRequest = 80,//Units.InputEntryHeight,
                BackgroundColor = Color.Transparent
            };

            Title = new StaticLabel(title);
            Title.Content.FontSize = 12;

            CustomPlaceHolder = new StaticLabel(title);
            Title.Content.FontSize = 12;

            TextEntry = new CustomEntry();
            TextEntry.Keyboard = keyboard;
            TextEntry.Placeholder = placeholder;
            TextEntry.Margin = new Thickness(0, 2);

            CustomPlaceHolder.Content.IsVisible = false;

            ContentContainer.Children.Add(Title.Content);
            ContentContainer.Children.Add(TextEntry);
            ContentContainer.Children.Add(CustomPlaceHolder.Content);

            Container.Children.Add(ContentContainer);
            Content.Children.Add(Container);
        }

        public void ShowValidInput()
        {
            SetTitleColor(Color.Black);
            TextEntry.TextColor = Color.Black;
        }

        public void ShowInvalidInput()
        {
            SetTitleColor(Color.Red);
            TextEntry.TextColor = Color.Red;
        }

        public void SetTitleColor(Color color)
        {
            Title.Content.TextColor = color;
        }

        public void DisableInput()
        {
            TextEntry.TextColor = Color.LightGray;

            CustomPlaceHolder.Content.Text = TextEntry.Text;
            CustomPlaceHolder.Content.TextColor = Color.Gray;
            TextEntry.IsVisible = false;
            TextEntry.IsEnabled = false;
            CustomPlaceHolder.Content.IsVisible = true;
        }


        public void SetHorizontal()
        {
            ContentContainer.Orientation = StackOrientation.Horizontal;
            ContentContainer.HeightRequest = 40;
            ContentContainer.VerticalOptions = LayoutOptions.CenterAndExpand;

        }

        public void LeftAlign()
        {
            TextEntry.HorizontalOptions = LayoutOptions.StartAndExpand;
            TextEntry.HorizontalTextAlignment = TextAlignment.Start;
            Title.Content.HorizontalTextAlignment = TextAlignment.Start;
            Title.Content.HorizontalOptions = LayoutOptions.StartAndExpand;
        }

        public void RightAlign()
        {
            TextEntry.HorizontalOptions = LayoutOptions.EndAndExpand;
            TextEntry.HorizontalTextAlignment = TextAlignment.End;
            Title.Content.HorizontalTextAlignment = TextAlignment.End;
            Title.Content.HorizontalOptions = LayoutOptions.EndAndExpand;
        }

        public void CenterAlign()
        {
            TextEntry.HorizontalOptions = LayoutOptions.CenterAndExpand;
            TextEntry.HorizontalTextAlignment = TextAlignment.Center;
            Title.Content.HorizontalTextAlignment = TextAlignment.Center;
            Title.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
        }


    }
}
