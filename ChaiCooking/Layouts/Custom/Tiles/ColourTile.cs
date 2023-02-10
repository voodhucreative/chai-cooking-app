using System;
using ChaiCooking.AppData;
using ChaiCooking.Components;
using ChaiCooking.Helpers;
using ChaiCooking.Models.Custom.AlbumAPI;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Tiles
{
    public class ColourTile : ActiveComponent
    {
        Grid colourTile { get; set; }
        Frame outerFrame { get; set; }
        TileColour tileColour { get; set; }
        public Action UpdateData { get; set; }

        public ColourTile()
        {
            colourTile = new Grid
            {
                WidthRequest = 25,
                HeightRequest = 25,
            };

            Frame tileFrame = new Frame
            {
                Content = colourTile,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                CornerRadius = 5,
                IsClippedToBounds = true,
                Padding = 0,
            };

            outerFrame = new Frame
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Content = tileFrame,
                CornerRadius = 5,
                IsClippedToBounds = true,
                Padding = 5,
                BorderColor = Color.Transparent,
                Margin = 0,
                BackgroundColor = Color.Transparent,
            };
            TouchEffect.SetNativeAnimation(colourTile, true);
            TouchEffect.SetCommand(colourTile,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    ToggleHighlight();
                });
            }));

            Content.Children.Add(outerFrame);
        }

        public void SetTile(TileColour input)
        {
            this.tileColour = input;
            this.colourTile.BackgroundColor = Color.FromHex(input.colour);
        }

        public void ToggleHighlight()
        {
            if (AppSession.InfoModeOn)
            {
                App.ShowInfoBubble(new Paragraph("Colour Palette", "Tap a colour swatch to change the colour for a highlighted Collection(album).", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
            }
            else
            {
                if (StaticData.colourModel != null)
                {
                    if (StaticData.colourModel.colourList != null)
                    {
                        foreach (TileColour c in StaticData.colourModel.colourList)
                        {
                            c.isHighlighted = false;
                        }
                    }
                }
                if (StaticData.selectedColour != this.tileColour.colour)
                {
                    tileColour.isHighlighted = true;
                    StaticData.selectedColour = this.tileColour.colour;
                }
                else
                {
                    tileColour.isHighlighted = false;
                    StaticData.selectedColour = null;
                }
                this.UpdateData();
            }
        }

        public void SetHighlight(bool isHighlighted)
        {
            if (isHighlighted)
            {
                outerFrame.BorderColor = Color.LightGray;
                outerFrame.BackgroundColor = Color.LightGray;
            }
            else
            {
                outerFrame.BorderColor = Color.Transparent;
                outerFrame.BackgroundColor = Color.Transparent;
            }

        }
    }
}
