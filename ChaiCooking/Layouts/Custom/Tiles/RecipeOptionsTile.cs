using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Images;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using ChaiCooking.Views.CollectionViews.Collections;
using ChaiCooking.Views.CollectionViews.Favourites;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
namespace ChaiCooking.Layouts.Custom.Tiles
{
    public class RecipeOptionsTile : ActiveComponent
    {
        public Label title { get; set; }
        public FolderTile folderTile { get; set; }
        public StackLayout folderContainer { get; set; }
        public StackLayout horizontalContainer { get; set; }
        public Action updateAlbums { get; set; }
        public Action updateTiles { get; set; }
        public Album album { get; set; }
        private Frame addFrame;

        public RecipeOptionsTile(Album input)
        {
            this.album = input;

            FolderTile folderTile = new FolderTile("", false, false);
            folderTile.Content.WidthRequest = Dimensions.ALBUM_TILE_WIDTH;
            folderTile.Content.HeightRequest = Dimensions.ALBUM_TILE_HEIGHT;
            if (album.FolderColor == null || album.FolderColor == "")
            {
                folderTile.SetActiveColor(Color.FromHex("ffffff"));
            }
            else
            {
                folderTile.SetActiveColor(Color.FromHex(album.FolderColor));
            }

            folderContainer = new StackLayout
            {
                //BackgroundColor = Color.Red,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                WidthRequest = 50,
                Children =
                {
                    folderTile.Content
                }
            };

            title = new Label
            {
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = Units.FontSizeL,
                LineBreakMode = LineBreakMode.CharacterWrap,
                Text = album.Name
            };

            StackLayout nameContainer = new StackLayout
            {
                //BackgroundColor = Color.Green,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                WidthRequest = 150,
                IsClippedToBounds = true,
                Children =
                {
                    title
                }
            };

            StaticImage plusImage = new StaticImage("plus.png", 24, 24, null);

            StackLayout plusContainer = new StackLayout
            {
                WidthRequest = 50,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    plusImage.Content
                }
            };

            addFrame = new Frame
            {
                Content = plusContainer,
                CornerRadius = 20,
                Padding = 10,
                HeightRequest = 20,
                WidthRequest = 60,
                BackgroundColor = Color.Orange,
                HasShadow = false,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
            };

            TouchEffect.SetNativeAnimation(addFrame, true);
            TouchEffect.SetCommand(addFrame,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var result = await DataManager.AddRecipeToAlbum(AppSession.SelectedRecipe, album);
                        if (result)
                        {
                            //var search = album.Recipes.Find(x => x.Id == AppSession.SelectedRecipe.Id);
                            //if(search != null) { }
                            addFrame.Content.BackgroundColor = Color.LightGray;
                            addFrame.BackgroundColor = Color.LightGray;
                            App.ShowAlert($"Successfully added recipe to {album.Name}");
                        }
                        else
                        {
                            App.ShowAlert($"Failed to add recipe to {album.Name}.");
                        }
                    });
                }));

            horizontalContainer = new StackLayout
            {
                //BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                Margin = 1,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = 70,
                WidthRequest = Units.ThirdScreenWidth * 2,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    folderContainer,
                    nameContainer,
                    addFrame,
                }
            };

            StackLayout outerContainer = new StackLayout
            {
                //BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                Padding = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    horizontalContainer
                }
            };

            Content.Children.Add(outerContainer);
        }
    }
}
