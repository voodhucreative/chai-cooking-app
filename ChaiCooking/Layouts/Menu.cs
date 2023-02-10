using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Images;
using ChaiCooking.Helpers;
using Xamarin.Forms;

namespace ChaiCooking.Layouts
{
    public class Menu
    {
        private enum Orientations
        {
            Horizontal,
            Vertical
        };

        public Grid Content { get; set; }
        public Grid Background { get; set; }
        public Grid Container { get; set; }

        private int SlideOnDirection;
        private int Orientation;
        public int Width;
        public int Height;
        private int ScreenWidth;
        private int ScreenHeight;
        public uint TransitionTime { get; set; }

        public ActiveImage CloseButton;

        public IconButton HomeButton;
        public IconButton RestaurantsButton;
        public IconButton AccountButton;
        public IconButton CheckoutButton;

        public IconButton SignOutButton;


        //FaceBookLoginButton = new IconButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Branding.Colors.FACEBOOK_BLUE), Color.White, "Log in with Facebook", "facebook_icon.png", new Models.Action((int) Actions.ActionName.GoToPage, (int) AppSettings.PageNames.CreateAccount));
        //FaceBookLoginButton.UpAction = new Models.Action((int) Actions.ActionName.GoToPage, (int) AppSettings.PageNames.CreateAccount);




        public Menu()
        {
            TransitionTime = 250;

            Content = new Grid
            {
                BackgroundColor = Color.Transparent,
                Opacity = 0
            };

            Background = new Grid
            {
                BackgroundColor = Color.FromHex(Branding.Colors.CC_WARNING_RED),
                Opacity = 1
            };

            Container = new Grid
            {
                BackgroundColor = Color.Transparent,
                Padding = Units.ScreenUnitS,
            };

            Container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(Units.TapSizeL) });
            Container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(Units.ScreenHeight - (Units.TapSizeXL*3)) });
            Container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(Units.TapSizeXL) });



            CloseButton = new ActiveImage("no_image.png", Units.TapSizeM, Units.TapSizeM, null, new Models.Action((int)Actions.ActionName.ToggleMenu, -1));
            CloseButton.Content.HorizontalOptions = LayoutOptions.End;

            SignOutButton = new IconButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.Transparent, Color.White, "Sign Out", "facebook_icon.png", new Models.Action((int) Actions.ActionName.GoToPage, (int) AppSettings.PageNames.Landing));
            SignOutButton.Content.HorizontalOptions = LayoutOptions.Start;

            StackLayout OptionsContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(0, Units.TapSizeM, 0, 0)
            };

            HomeButton = new IconButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.Transparent, Color.White, "Home", "facebook_icon.png", null);
            HomeButton.Content.HorizontalOptions = LayoutOptions.Start;
            HomeButton.Content.GestureRecognizers.Add(
                new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            Console.WriteLine("Testing");
                            await App.PerformActionAsync((int)Actions.ActionName.ToggleMenu, -1);
                            await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Landing);
                        });
                    })
                }
            );
            /*
            RestaurantsButton = new IconButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.Transparent, Color.White, "Restaurants", "facebook_icon.png", null);
            RestaurantsButton.Content.HorizontalOptions = LayoutOptions.Start;
            RestaurantsButton.Content.GestureRecognizers.Add(
                new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            Console.WriteLine("Testing");
                            await App.PerformActionAsync((int)Actions.ActionName.ToggleMenu, -1);
                            await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.RestaurantMenu);
                        });
                    })
                }
            );

            AccountButton = new IconButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.Transparent, Color.White, "Account", "facebook_icon.png", null);
            AccountButton.Content.HorizontalOptions = LayoutOptions.Start;
            AccountButton.Content.GestureRecognizers.Add(
                new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            Console.WriteLine("Testing");
                            await App.PerformActionAsync((int)Actions.ActionName.ToggleMenu, -1);
                            await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.CreateAccount);
                        });
                    })
                }
            );

            CheckoutButton = new IconButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.Transparent, Color.White, "Checkout", "facebook_icon.png", null);
            CheckoutButton.Content.HorizontalOptions = LayoutOptions.Start;
            CheckoutButton.Content.GestureRecognizers.Add(
                new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            Console.WriteLine("Testing");
                            await App.PerformActionAsync((int)Actions.ActionName.ToggleMenu, -1);
                            await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.OrderConfirmation);
                        });
                    })
                }
            );
            */
            HomeButton.SetPositionCenter();
            /*RestaurantsButton.SetPositionCenter();
            AccountButton.SetPositionCenter();
            CheckoutButton.SetPositionCenter();
            SignOutButton.SetPositionCenter();*/

            HomeButton.SetContentLeft();
            /*RestaurantsButton.SetContentLeft();
            AccountButton.SetContentLeft();
            CheckoutButton.SetContentLeft();
            SignOutButton.SetContentLeft();*/

            OptionsContainer.Children.Add(HomeButton.Content);
            /*OptionsContainer.Children.Add(RestaurantsButton.Content);
            OptionsContainer.Children.Add(AccountButton.Content);
            OptionsContainer.Children.Add(CheckoutButton.Content);*/

            Container.Children.Add(CloseButton.Content, 0, 0);
            Container.Children.Add(OptionsContainer, 0, 1);
            Container.Children.Add(SignOutButton.Content, 0, 2);




            Content.Children.Add(Background);
            Content.Children.Add(Container);



            /*
            Content.GestureRecognizers.Add(
                new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await App.PerformActionAsync((int)Actions.ActionName.ToggleMenu, -1);
                        });
                    })
                }
            );*/
        }

        public void SetSize(int screenWidth, int screenHeight, int menuWidth, int menuHeight)
        {
            Width = menuWidth;
            Height = menuHeight;
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            Update();
        }

        public void SetLeft()
        {
            SlideOnDirection = (int)AppSettings.MenuPositions.Left;
            Orientation = (int)Orientations.Vertical;
            Update();
        }

        public void SetRight()
        {
            SlideOnDirection = (int)AppSettings.MenuPositions.Right;
            Orientation = (int)Orientations.Vertical;
            Update();
        }

        public void SetTop()
        {
            SlideOnDirection = (int)AppSettings.MenuPositions.Top;
            Orientation = (int)Orientations.Horizontal;
            Update();
        }

        public void SetBottom()
        {
            SlideOnDirection = (int)AppSettings.MenuPositions.Bottom;
            Orientation = (int)Orientations.Horizontal;
            Update();
        }

        public void Toggle()
        {
            if (Content.IsVisible)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        public async Task Show()
        {
            await Task.Delay(10);
            Device.BeginInvokeOnMainThread(async () =>
            {
            Content.IsVisible = true;

            switch (SlideOnDirection)
            {
                case (int)AppSettings.MenuPositions.Left:
                    break;
                case (int)AppSettings.MenuPositions.Right:
                    break;
                case (int)AppSettings.MenuPositions.Top:
                    break;
                case (int)AppSettings.MenuPositions.Bottom:
                    break;
            }

            List<Task> tasks = new List<Task> {Content.TranslateTo(0, 0, TransitionTime, Easing.CubicIn), Content.FadeTo(1, TransitionTime, Easing.Linear) };
            await Task.WhenAll(tasks);


            });

        }

        public async Task Hide()
        {
            await Task.Delay(10);
            List<Task> tasks;
            Device.BeginInvokeOnMainThread(async () =>
            {
                switch (SlideOnDirection)
                {
                    case (int)AppSettings.MenuPositions.Left:
                        tasks = new List<Task> { Content.TranslateTo(-Width, 0, TransitionTime, Easing.CubicOut), Content.FadeTo(0, TransitionTime, Easing.Linear) };
                        await Task.WhenAll(tasks);
                        break;
                    case (int)AppSettings.MenuPositions.Right:
                        tasks = new List<Task> { Content.TranslateTo(Width, 0, TransitionTime, Easing.CubicOut), Content.FadeTo(0, TransitionTime, Easing.Linear) };
                        await Task.WhenAll(tasks);
                        break;
                    case (int)AppSettings.MenuPositions.Top:
                        tasks = new List<Task> { Content.TranslateTo(0, -Height, TransitionTime, Easing.CubicOut), Content.FadeTo(0, TransitionTime, Easing.Linear) };
                        await Task.WhenAll(tasks);
                        break;
                    case (int)AppSettings.MenuPositions.Bottom:
                        tasks = new List<Task> { Content.TranslateTo(0, Height, TransitionTime, Easing.CubicOut), Content.FadeTo(0, TransitionTime, Easing.Linear) };
                        await Task.WhenAll(tasks);
                        break;
                }
                Content.IsVisible = false;
            });
        }

        private void Update()
        {
            switch (SlideOnDirection)
            {
                case (int)AppSettings.MenuPositions.Left:
                    Content.WidthRequest = Width;
                    Content.HeightRequest = ScreenWidth;
                    Content.Margin = new Thickness(0, 0, ScreenWidth - Width, 0);
                    break;
                case (int)AppSettings.MenuPositions.Right:
                    Content.WidthRequest = Width;
                    Content.HeightRequest = ScreenHeight;
                    Content.Margin = new Thickness(ScreenWidth - Width, 0, 0, 0);
                    break;
                case (int)AppSettings.MenuPositions.Top:
                    Content.WidthRequest = ScreenWidth;
                    Content.HeightRequest = Height;
                    Content.Margin = new Thickness(0, 0, 0, ScreenHeight - Height);
                    break;
                case (int)AppSettings.MenuPositions.Bottom:
                    Content.WidthRequest = ScreenWidth;
                    Content.HeightRequest = Height;
                    Content.Margin = new Thickness(0, ScreenHeight - Height, 0, 0);
                    break;
            }
        }


        public void DebugNextType()
        {
            if (Content.IsVisible)
            {
                return;
            }
            else
            {
                switch (SlideOnDirection)
                {
                    case (int)AppSettings.MenuPositions.Left:
                        SetTop();
                        break;
                    case (int)AppSettings.MenuPositions.Right:
                        SetBottom();
                        break;
                    case (int)AppSettings.MenuPositions.Top:
                        SetRight();
                        break;
                    case (int)AppSettings.MenuPositions.Bottom:
                        SetLeft();
                        break;
                }
                Hide();
            }
        }
    }
}
