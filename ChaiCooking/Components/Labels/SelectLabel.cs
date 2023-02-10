using System;
using System.Threading.Tasks;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using Xamarin.Forms;

namespace ChaiCooking.Components.Composites
{
    public class SelectLabel
    {
        public Grid Content { get; set; }
        public StackLayout Container { get; set; }

        public Color UnselectedColor { get; set; }
        public Color SelectedColor { get; set; }
        
        public StaticLabel Title { get; set; }

        public bool IsSelected { get; set; }

        public bool IsToggleable { get; set; }

        public Models.Action SelectedAction { get; set; }
        public Models.Action UnselectedAction { get; set; }

        public SelectLabel(string title, Color selectedColor, Color unselectedColor, int width, int height, bool isSelected, bool isToggleable)
        {
            Content = new Grid
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = width,
                HeightRequest = height
            };

            Container = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center
            };

            SelectedColor = selectedColor;
            UnselectedColor = unselectedColor;

            Title = new StaticLabel(title);
            Title.Content.TextColor = UnselectedColor;

            IsSelected = isSelected;
            IsToggleable = isToggleable;

            if (IsSelected)
            {
                Title.Content.TextColor = SelectedColor;
            }

            Container.Children.Add(Title.Content);

            // override to switch this off
            if (IsToggleable)
            {
                Container.GestureRecognizers.Add(
                        new TapGestureRecognizer()
                        {
                            Command = new Command(() =>
                            {

                                Device.BeginInvokeOnMainThread(async () =>
                            {
                                        Toggle();
                                    });
                            })
                        }
                    );
            }
            Content.Children.Add(Container);
        }

        public void CenterAlign()
        {
            Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            Container.HorizontalOptions = LayoutOptions.CenterAndExpand;
            Title.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            Title.Content.HorizontalTextAlignment = TextAlignment.Center;
        }

        public void LeftAlign()
        {
            Content.HorizontalOptions = LayoutOptions.StartAndExpand;
            Container.HorizontalOptions = LayoutOptions.StartAndExpand;
            Title.Content.HorizontalOptions = LayoutOptions.StartAndExpand;
            Title.Content.HorizontalTextAlignment = TextAlignment.Start;
        }

        public void Toggle()
        {
            IsSelected = !IsSelected;

            if (IsSelected)
            {
                Title.Content.TextColor = SelectedColor;

                if(SelectedAction != null)
                {
                    PerformSelectedAction();
                }
            }
            else
            {
                Title.Content.TextColor = UnselectedColor;

                if (SelectedAction != null)
                {
                    PerformUnselectedAction();
                }
            }
        }


        private void PerformSelectedAction()
        {
            
            App.ShowAlert("Select label " + Title.Content.Text + " selected action.");

            Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(500);
                IsSelected = false;
                Title.Content.TextColor = UnselectedColor;
                await SelectedAction.Execute();
            });
        }

        private void PerformUnselectedAction()
        {
            
            App.ShowAlert("Select label " + Title.Content.Text + " unselected action.");

            Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(500);
                IsSelected = false;
                Title.Content.TextColor = UnselectedColor;
                await UnselectedAction.Execute();
            });
        }

        public void DisableDefaultInteraction()
        {
            Container.GestureRecognizers.Clear();
        }

        public void ShowClicked()
        {
            Title.Content.TextColor = SelectedColor;

            Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(500);
                Title.Content.TextColor = UnselectedColor;
            });
        }

        public void ShowSelected()
        {
            Title.Content.TextColor = SelectedColor;  
        }

        public void ShowUnselected()
        {
            Title.Content.TextColor = UnselectedColor;
        }
    }
}

