using System;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Images;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Lists
{
    public class AlbumList : ActiveComponent
    {
        public Label title { get; set; }
        public FolderTile folderTile { get; set; }
        public StackLayout folderContainer { get; set; }
        public Action updateAlbums { get; set; }
        public Album album { get; set; }
        StackLayout outerContainer;

        public AlbumList()
        {
            folderContainer = new StackLayout
            {
                //BackgroundColor = Color.Red,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                //WidthRequest = 50,
                Children =
                {
                }
            };

            title = new Label
            {
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = Units.FontSizeL,
                LineBreakMode = LineBreakMode.WordWrap
            };

            StackLayout nameContainer = new StackLayout
            {
                //BackgroundColor = Color.Green,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                //WidthRequest = 150,
                IsClippedToBounds = true,
                Children =
                {
                    title
                }
            };

            Grid mainGrid = new Grid
            {
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = 0,
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(70)}},
                },
                ColumnDefinitions =
                {
                    { new ColumnDefinition { Width = new GridLength(Units.ScreenWidth25Percent)}},
                    { new ColumnDefinition { Width = new GridLength(Units.ScreenWidth25Percent)}}
                },
                Children =
                {
                    {folderContainer, 0, 0},
                    {nameContainer, 1, 0}
                }
            };

            mainGrid.GestureRecognizers.Add(
                new TapGestureRecognizer()
                    {
                    Command = new Command(() =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>{
                            ToggleHighlight();
                        });
                    })
                });

            outerContainer = new StackLayout
            {
                BackgroundColor = Color.White,
                Padding = 2,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    mainGrid
                }
            };

            Content.Children.Add(outerContainer);
        }

        public void SetFolder(FolderTile input)
        {
            folderContainer.Children.Add(input.Content);
        }

        public void SetAlbumName(string input)
        {
            this.title.Text = input;
        }

        public void SetUpdateAction(Action input)
        {
            this.updateAlbums = input;
        }

        public void SetAlbum(Album input)
        {
            this.album = input;
        }

        public void ToggleHighlight()
        {
            if (AppSession.CurrentUser.Albums != null)
            {
                foreach (Album a in AppSession.CurrentUser.Albums)
                {
                    a.isHighlighted = false;
                }
            }
            if (StaticData.selectedAlbum.Id != this.album.Id)
            {
                album.isHighlighted = true;
                StaticData.selectedAlbum = this.album;
            }
            else
            {
                album.isHighlighted = false;
                StaticData.selectedAlbum = new Album();
            }
            this.updateAlbums();
        }

        public void SetHighlight(bool isHighlighted)
        {
            if (isHighlighted)
            {
                outerContainer.BackgroundColor = Color.Orange;
            }
            else
            {
                outerContainer.BackgroundColor = Color.Transparent;
            }

        }
    }
}
