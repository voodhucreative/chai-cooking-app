using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Images;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Tools;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Panels
{
    public class RecommendedRecipeFilterPanel : StandardLayout
    {
        public RecommendedRecipeFilterPanel()
        {
            Content = new Grid { BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY), Padding = 2 };

            Container = new Grid
            {
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                Padding = Dimensions.GENERAL_COMPONENT_PADDING
            };

            Container.Children.Add(new Label { Text = "Recommended Recipe Filters", TextColor = Color.Yellow });

            Container.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await App.PerformActionAsync(new Models.Action((int)Actions.ActionName.ClearPanels));
                            });
                        })
                    }
                );

            Content.Children.Add(Container);

        }
    }
}