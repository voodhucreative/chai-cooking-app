using System;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Models.Custom;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Tiles
{
    public class TemplateDayNumberTile : ActiveComponent
    {
        Grid numberTile { get; set; }
        bool isSelected { get; set; }
        string number { get; set; }
        int dayTemplateID { get; set; }

        public TemplateDayNumberTile()
        {
            numberTile = new Grid
            {
                WidthRequest = 25,
                HeightRequest = 25,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            Frame tileFrame = new Frame
            {
                Content = numberTile,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                CornerRadius = 5,
                IsClippedToBounds = true,
                BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY),
                Padding = 0,
            };

            Content.Children.Add(tileFrame);
        }

        public void SetTile(int dayTemplateID, string input, bool dayExists, bool isDeleting)
        {
            this.number = input;
            this.dayTemplateID = dayTemplateID;
            Color textColour = Color.Gray;
            if (dayExists && !isDeleting || !dayExists && isDeleting)
            {
                textColour = Color.White;
            }
            TemplateDays templateDays = new TemplateDays();
            templateDays.dayNumber = number;
            templateDays.templateID = dayTemplateID;

            StaticLabel staticLabel = new StaticLabel(input);
            staticLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            staticLabel.Content.FontSize = Units.FontSizeM;
            staticLabel.Content.TextColor = textColour;
            staticLabel.CenterAlign();
            TouchEffect.SetNativeAnimation(numberTile, true);
            TouchEffect.SetCommand(numberTile,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (dayExists && !isDeleting || !dayExists && isDeleting)
                    {
                        numberTile.BackgroundColor = isSelected ? Color.FromHex(Colors.CC_DARK_BLUE_GREY) : Color.Orange;
                        if (isSelected) { StaticData.daysList.Remove(templateDays); } else { StaticData.daysList.Add(templateDays); }
                        isSelected = !isSelected;
                    }
                });
            }));
            numberTile.Children.Add(staticLabel.Content);
        }
    }
}
