using System;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using Xamarin.Forms;
using XFShapeView;

namespace ChaiCooking.Components.Composites
{
    public class SectionToggle
    {
        public bool IsOpen { get; set; }
        public StackLayout Content { get; set; }
        public StaticLabel Title { get; set; }
        public StaticImage OpenCloseIcon { get; set; }

        public ShapeView Arrow;
        

        public SectionToggle(string titleText)
        {
            Content = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Padding = 8
            };

            Title = new StaticLabel(titleText);
            Title.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            Title.Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            Title.Content.HorizontalTextAlignment = TextAlignment.Center;
            Title.Content.VerticalTextAlignment = TextAlignment.Center;
            
            

            Arrow = new ShapeView
            {
                ShapeType = ShapeType.Triangle,
                HeightRequest = 12,
                WidthRequest = 16,
                Color = Color.White,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            OpenCloseIcon = new StaticImage("fb_icon.png", 16, null);

            Content.Children.Add(Title.Content);
            //Content.Children.Add(OpenCloseIcon.Content);
            Content.Children.Add(Arrow);

            IsOpen = false;
            SetIsOpen(IsOpen);
        }

        public void SetIsOpen(bool isOpen)
        {
            IsOpen = isOpen;

            if (IsOpen)
            {
                OpenCloseIcon.Content.Source = "twitter_icon.png";
                Arrow.RotateTo(180, 0, null);
            }
            else
            {
                OpenCloseIcon.Content.Source = "fb_icon.png";
                Arrow.RotateTo(90, 0, null);
            }
        }

        public void LeftAlign()
        {
            Content.HorizontalOptions = LayoutOptions.StartAndExpand;               
            Title.Content.HorizontalOptions = LayoutOptions.StartAndExpand;
            Title.Content.HorizontalTextAlignment = TextAlignment.Start;
           
        }

        public void RightAlign()
        {
            Content.HorizontalOptions = LayoutOptions.EndAndExpand;
            Title.Content.HorizontalOptions = LayoutOptions.EndAndExpand;
            Title.Content.HorizontalTextAlignment = TextAlignment.End;

        }

        public void CenterAlign()
        {
            Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            Title.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            Title.Content.HorizontalTextAlignment = TextAlignment.Center;

        }

    }
}
