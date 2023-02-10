using System;
using System.Collections.Generic;
using ChaiCooking.Branding;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using Xamarin.Forms;
using XFShapeView;

namespace ChaiCooking.Layouts.Custom.Panels
{
    public class InfoBubblePanel : StandardLayout
    {
        Grid ContentContainer;
        Grid PointerContainer;
        StackLayout BubbleContainer;
        StackLayout BubbleContent;


        StaticLabel Title { get; set; }
        Paragraph SpeechContent { get; set; }

        ShapeView Pointer;
        ShapeView PointerBg;

        StaticImage CloseImage;

        int PointerPosition;
        List<int> PointerXPositions;
        List<int> PointerYPositions;
        int PointerWidth;
        int PointerHeight;
        int PointerX;
        int PointerY;
        int BorderWidth;

        int Width;
        int Height;

        bool InMenu;


        public InfoBubblePanel(string title, string speechContent, bool inMenu)
        {
            BorderWidth = 2;
            Width = 300;
            Height = 300;
            PointerWidth = 32;
            PointerHeight = 32;
            InMenu = inMenu;

            Content = new Grid
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
            };

            ContentContainer = new Grid
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                
            };

            BubbleContainer = new StackLayout
            {
                BackgroundColor = Color.FromHex(Colors.CC_DARK_GREY),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                WidthRequest = Width,
                //HeightRequest = Height,
                Padding = BorderWidth,
                Spacing = 0
            };

            BubbleContent = new StackLayout
            {
                BackgroundColor = Color.FromHex(Colors.CC_MUSTARD),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                WidthRequest = Width,
                //HeightRequest = Height,
                Orientation = StackOrientation.Vertical,
                Padding = Dimensions.GENERAL_COMPONENT_SPACING
            };

            Title = new StaticLabel(title);

            CloseImage = new StaticImage("closecircleblack.png", 25, 25, null);
            CloseImage.Content.HorizontalOptions = LayoutOptions.End;


            SpeechContent = new Paragraph(null, speechContent, null);

            PointerContainer = new Grid { };

            Pointer = new ShapeView
            {
                ShapeType = ShapeType.Triangle,
                HeightRequest = PointerHeight,
                WidthRequest = PointerWidth,
                Color = Color.FromHex(Colors.CC_MUSTARD),
                BorderColor = Color.FromHex(Colors.CC_MUSTARD),
                BorderWidth = 0f,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start
            };

            PointerBg = new ShapeView
            {
                ShapeType = ShapeType.Triangle,
                HeightRequest = PointerHeight,
                WidthRequest = PointerWidth,
                Color = Color.FromHex(Colors.CC_DARK_GREY),
                BorderColor = Color.FromHex(Colors.CC_DARK_GREY),
                BorderWidth = 0f,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start
            };

            Title = new StaticLabel(title);

            SpeechContent = new Paragraph(title, speechContent, "chaismallbag.png");
            SpeechContent.Image.Content.WidthRequest = 264;
            SpeechContent.Image.Content.HeightRequest = 264;

            StackLayout header = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    Title.Content,
                    CloseImage.Content
                }
            };

            BubbleContent.Children.Add(header);// Title.Content);
            BubbleContent.Children.Add(SpeechContent.Content);

            PointerContainer.Children.Add(PointerBg, 0, 0);
            PointerContainer.Children.Add(Pointer, 0, 0);
            Pointer.TranslateTo(0, BorderWidth+2, 0, null);

            BubbleContainer.Children.Add(BubbleContent);

            



            ContentContainer.Children.Add(BubbleContainer, 0, 0);
            ContentContainer.Children.Add(PointerContainer, 0, 0);

            PointerContainer.TranslateTo(0, -PointerHeight, 0, null);
            ContentContainer.TranslateTo(0, PointerHeight, 0, null);



            Container.Children.Add(ContentContainer);
            Content.Children.Add(Container);

            // test
            

            Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {

                                App.HideInfoBubble();
                                App.HideMenuInfoBubble();
                                //SetPointerXPosition(nApp.HideInfoBubble();ew Random().Next(-(Width/2)+PointerWidth, Width/2 - PointerWidth));
                            });

                        })
                    }
                );
        }

        public void HidePointer()
        {
            PointerContainer.Opacity = 0;
        }

        public void SetPointerXPosition(int xPos)
        { 
            PointerContainer.TranslateTo(xPos, -PointerHeight, 0, Easing.Linear);           
        }


        public void SetBubbleContent(View content)
        {
            try
            {
                BubbleContent.Children.Clear();
                //BubbleContent.Children.Add(CloseImage.Content);
                BubbleContent.Children.Add(content);
            }
            catch (Exception e)
            {

            }
        }

        public void SetPosition(int x, int y)
        {
            int xPos = x;
            int yPos = y;

            int bubbleWidth = Width;
            int bubbleHeight = Height;

            int newY = (int)(yPos);
            int pointerX = 0;

            if (Device.RuntimePlatform == Device.Android)
            {
                newY += PointerHeight;
            }

            // move to initial position
            Content.BackgroundColor = Color.Transparent;

            if (InMenu)
            {
                pointerX = xPos - (bubbleWidth / 2);
                int boxX = 110;

                Content.TranslateTo(boxX, newY, 0, null);

                // set the pointer position
                SetPointerXPosition(pointerX - boxX); // 0 center
            }
            else
            {
                pointerX = xPos - (bubbleWidth / 2);

                int boxX = pointerX;

                

                if (boxX < pointerX + PointerWidth - bubbleWidth / 2)
                {
                    boxX = pointerX + PointerWidth - bubbleWidth / 2;
                }

                if (boxX < Dimensions.GENERAL_COMPONENT_SPACING)
                {
                    boxX = Dimensions.GENERAL_COMPONENT_SPACING;
                }

                if (boxX > Units.ScreenWidth - (bubbleWidth + Dimensions.GENERAL_COMPONENT_SPACING))
                {
                    boxX = Units.ScreenWidth - (bubbleWidth + Dimensions.GENERAL_COMPONENT_SPACING);
                }

                Console.WriteLine("Pointer X " + pointerX);
                Console.WriteLine("Box X " + boxX);

                Content.TranslateTo(boxX, newY, 0, null);

                // set the pointer position
                SetPointerXPosition(pointerX - boxX); // 0 center
            }
        }


    }
}
