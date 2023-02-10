using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Images;
using ChaiCooking.Helpers;
using ChaiCooking.Tools;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Panels
{
    public class ActionResultPanel : StandardLayout
    {
        ActiveImage Logo;
        IconButton MenuButton;
        Grid BackgroundLayer;
        StackLayout PanelContainer;
        StackLayout ContentContainer;
        StackLayout ButtonContainer;
        List<ColourButton> Buttons;

        public ActionResultPanel(bool showBackground)
        {
            Height = 480;
            Width = 480;
            TransitionTime = 150;
            TransitionType = (int)AppSettings.TransitionTypes.SlideOutRight;

            Content = new Grid();
            BackgroundLayer = new Grid { BackgroundColor = Color.Transparent, Opacity = 0};

            Container = new Grid
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                ColumnSpacing = 0,
                RowSpacing = 0,
                BackgroundColor = Color.Transparent
            };

            PanelContainer = new StackLayout { Orientation = StackOrientation.Vertical };
            ContentContainer = new StackLayout { Orientation = StackOrientation.Vertical };
            ButtonContainer = new StackLayout { Orientation = StackOrientation.Horizontal };

            
            /*
            var leftSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };
            leftSwipeGesture.Swiped += OnSwiped;
            var rightSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Right };
            rightSwipeGesture.Swiped += OnSwiped;
            var upSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Up };
            upSwipeGesture.Swiped += OnSwiped;
            var downSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Down };
            downSwipeGesture.Swiped += OnSwiped;


            DraggableView = new DraggableView
            {
                BackgroundColor = Color.Transparent,
                WidthRequest = Content.Width,
                HeightRequest = Content.Height,
                DragMode = DragMode.Touch,
                DragDirection = DragDirectionType.Horizontal
            };

            DraggableView.DragStart += delegate (object sender, EventArgs e) { DragStarted(sender, e); };
            DraggableView.DragEnd += delegate (object sender, EventArgs e) { DragEnded(sender, e); };

            DraggableView.GestureRecognizers.Add(leftSwipeGesture);
            DraggableView.GestureRecognizers.Add(rightSwipeGesture);
            DraggableView.GestureRecognizers.Add(upSwipeGesture);
            DraggableView.GestureRecognizers.Add(downSwipeGesture);

            DraggableView.Content = Container;*/

            if (showBackground)
            {
                Content.Children.Add(BackgroundLayer);
            }
            
            //Content.Children.Add(DraggableView, 0, 0);
            Content.Children.Add(Container, 0, 0);
        }

        public void SetBackground(Color backgroundColour, float opacity)
        {
            BackgroundLayer.BackgroundColor = backgroundColour;
            BackgroundLayer.Opacity = opacity;
        }
        /*
        void DragStarted(object sender, EventArgs e)
        {
            Container.BackgroundColor = Color.FromHex(Colors.BH_PURPLE);
        }

        void DragEnded(object sender, EventArgs e)
        {

            Console.WriteLine("SCREEN WIDTH: " + Units.ScreenWidth);
            Console.WriteLine("SCREEN WIDTH: " + Units.PixelWidth);
            Console.WriteLine("X: " + DraggableView.MovedX);

            Container.BackgroundColor = Color.FromHex(Colors.BH_PINK);
            if (DraggableView.MovedX > Units.PixelWidth / 2)
            {
                App.HideNavHeader();
            }
            else
            {
                DraggableView.RestorePositionCommand.Execute(null);

                // perform tap command

            }
        }

        void OnSwiped(object sender, SwipedEventArgs e)
        {
            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    // Handle the swipe
                    //App.HideNavHeader();
                    Console.WriteLine("SWIPE LEFT");
                    break;
                case SwipeDirection.Right:
                    // Handle the swipe
                    Console.WriteLine("SWIPE RIGHT");
                    break;
                case SwipeDirection.Up:
                    // Handle the swipe
                    Console.WriteLine("SWIPE UP");
                    break;
                case SwipeDirection.Down:
                    // Handle the swipe
                    Console.WriteLine("SWIPE DOWN");
                    break;
            }
        }
        */

        public void AddContent(View content)
        {
            ContentContainer.Children.Add(content);
        }

        public void AddButton(string buttonText, int buttonType, ChaiCooking.Models.Action action, bool active)
        {
            ColourButton newButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, buttonText, action);
            newButton.Content.WidthRequest = Units.SmallButtonWidth;
            newButton.Content.HeightRequest = Units.SmallButtonHeight;
            newButton.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            newButton.IsActive = active;

            Buttons.Add(newButton);

            if (active)
            {
                newButton.Activate();
            }
            else
            {
                newButton.Deactivate();
            }

            UpdateButtons();

        }

        public void ActivateButton(int buttonId)
        {
            Buttons[buttonId].Activate();
        }

        public void DeactivateButton(int buttonId)
        {
            Buttons[buttonId].Deactivate();
        }

        private void UpdateButtons()
        {
            ButtonContainer.Children.Clear();

            foreach (ColourButton button in Buttons)
            {
                ButtonContainer.Children.Add(button.Content);
            }
        }

    }
}