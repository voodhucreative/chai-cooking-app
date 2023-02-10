using System;
using System.Threading.Tasks;
using ChaiCooking.Helpers;
using Xamarin.Forms;

namespace ChaiCooking.Components.Labels
{
    public class ActiveLabel : Components.ActiveComponent
    {
        public Label Label { get; set; }
        public string Title { get; set; }
        public string ComponentInfo { get; set; }
        public int FontSize { get; set; }

        public ActiveLabel(string text, int fontSize, Color backgroundColor, Color textColor, Models.Action action)
        {
            this.Title = text;
            this.ComponentInfo = null;
            this.DefaultAction = action;

            this.Content = new Grid
            {

            };


            Label = new Label
            {
                FontFamily = ChaiCooking.Helpers.Fonts.GetRegularAppFont(),
                Text = text,
                FontSize = fontSize,
                BackgroundColor = backgroundColor,
                TextColor = textColor,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };


            if (this.DefaultAction != null)
            {
                this.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await DefaultAction.Execute();
                            });
                        })
                    }
                );
            }
            Content.Children.Add(Label);
        }


        public void RightAlign()
        {
            Content.HorizontalOptions = LayoutOptions.EndAndExpand;
            Label.HorizontalOptions = LayoutOptions.EndAndExpand;
            Label.HorizontalTextAlignment = TextAlignment.End;

            Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            Label.VerticalOptions = LayoutOptions.CenterAndExpand;
            Label.VerticalTextAlignment = TextAlignment.Center;
        }

        public void LeftAlign()
        {
            Content.HorizontalOptions = LayoutOptions.StartAndExpand;
            Label.HorizontalOptions = LayoutOptions.StartAndExpand;
            Label.HorizontalTextAlignment = TextAlignment.Start;

            Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            Label.VerticalOptions = LayoutOptions.CenterAndExpand;
            Label.VerticalTextAlignment = TextAlignment.Center;
        }

        public void CenterAlign()
        {
            Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            Label.HorizontalOptions = LayoutOptions.CenterAndExpand;
            Label.HorizontalTextAlignment = TextAlignment.Center;

            Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            Label.VerticalOptions = LayoutOptions.CenterAndExpand;
            Label.VerticalTextAlignment = TextAlignment.Center;
        }

        public override async Task<bool> Update()
        {
            await Task.Delay(50);
            Console.WriteLine("Updating Standard Label : " + this.Title);
            return true;
        }
    }
}
