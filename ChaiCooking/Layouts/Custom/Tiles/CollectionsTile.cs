using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
    public class CollectionsTile : ActiveComponent
    {
        public Label title { get; set; }
        public FolderTile folderTile { get; set; }
        public StackLayout folderContainer { get; set; }
        public StackLayout horizontalContainer { get; set; }
        public Action updateAlbums { get; set; }
        public Action updateTiles { get; set; }
        public Album album { get; set; }
        private DragGestureRecognizer dragGesture { get; set; }

        public CollectionsTile(Album input)
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
                LineBreakMode = LineBreakMode.TailTruncation,
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

            StaticImage editImage = new StaticImage("pencil.png", 24, 24, null);

            StackLayout editContainer = new StackLayout
            {
                WidthRequest = 50,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {

                }
            };


            dragGesture = new DragGestureRecognizer()
            {
                CanDrag = false,
                DragStartingCommand = new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async ()
                     =>
                    {
                        AppSession.collectionsSelectedItem = album;
                        AppSession.IsDraggable = false;
                    });
                }),
            };

            if (album.Name != "Favourites")
            {
                editContainer.Children.Add(editImage.Content);
                if (album.isHighlighted)
                {
                    dragGesture.CanDrag = true;
                }
                else
                {
                    dragGesture.CanDrag = false;
                }
            }

            editContainer.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           if (AppSession.InfoModeOn)
                           {
                               App.ShowInfoBubble(new Paragraph("Edit Album", "To Edit an Album, tap the ‘pencil’ icon to rename or change the colour of a highlighted Collection (album).", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                           }
                           else
                           {
                               Device.BeginInvokeOnMainThread(async () =>
                               {
                                   AppSession.collectionsSelectedItem = album;
                                   AppSession.collectionsCollectionView.SelectedItem = album;
                                   await App.ShowEditAlbumModal(null);
                               });
                           }
                       })
                   }
               );

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
                    editContainer,
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

            outerContainer.GestureRecognizers.Add(dragGesture);

            if (album.Name != "Favourites")
            {
                AppSession.SelectedRecipe = null;
                outerContainer.GestureRecognizers.Add(new DropGestureRecognizer()
                {
                    AllowDrop = true,
                    DragOverCommand = new Command(() =>
                    {
                        Device.BeginInvokeOnMainThread(async ()
                         =>
                        {
                            if (AppSession.IsDraggable)
                            {
                                horizontalContainer.BackgroundColor = Color.FromHex(Colors.CC_DARK_ORANGE);
                                folderTile.SetScale(1.2);
                            }
                        });
                    }),
                    DragLeaveCommand = new Command(() =>
                    {
                        Device.BeginInvokeOnMainThread(async ()
                         =>
                        {
                            horizontalContainer.BackgroundColor = Color.Transparent;
                            folderTile.SetScale(1);
                        });
                    }),
                    DropCommand = new Command(() =>
                    {
                        Device.BeginInvokeOnMainThread(async ()
                         =>
                        {
                            if (AppSession.IsDraggable)
                            {
                                horizontalContainer.BackgroundColor = Color.Transparent;
                                folderTile.SetScale(1);
                                if (AppSession.favouritesCollectionView.SelectedItems.Count <= 1)
                                {
                                    var result = await DataManager.AddRecipeToAlbum(AppSession.SelectedRecipe, album);
                                    if (result)
                                    {
                                        //StaticData.favouriteRecipes.Remove(r);
                                        App.ShowAlert("Successfully added favourite recipe.");
                                        AppSession.SelectedRecipe = null;
                                    }
                                    else
                                    {
                                        App.ShowAlert("Failed to add favourite recipe.");
                                        AppSession.SelectedRecipe = null;
                                    }
                                }
                                else
                                {
                                    bool result = false;
                                    await App.ShowLoading();
                                    foreach (Recipe r in AppSession.favouritesCollectionView.SelectedItems)
                                    {
                                        await Task.Delay(100);
                                        try
                                        {
                                            result = await DataManager.AddRecipeToAlbum(r, album);
                                        }
                                        catch
                                        {
                                            App.ShowAlert("An error occured trying to add the recipes.");
                                            AppSession.SelectedRecipe = null;
                                            return;
                                        }
                                    }
                                    if (result)
                                    {
                                        App.ShowAlert($"Successfully added {AppSession.favouritesCollectionView.SelectedItems.Count} recipes.");
                                        AppSession.SelectedRecipe = null;
                                    }
                                    else
                                    {
                                        App.ShowAlert("Failed to add favourite recipes.");
                                        AppSession.SelectedRecipe = null;
                                    }
                                    await App.HideLoading();
                                }
                            }
                            else
                            {
                                App.ShowAlert("You cannot add a collection to another collection.");
                            }
                        });
                    })
                });
            }

            outerContainer.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (AppSession.InfoModeOn)
                        {
                            App.ShowInfoBubble(new Paragraph("" + album.Name, "Tap a Collection (album) and highlight one or multiple recipes and select view to see the content of the album you highlighted. You can scroll the list of recipes and tap any recipe summary to highlight/unhighlight. Then highlight one or multiple recipes and you can copy them to another Collection (album) or remove them from the existing one.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                        }
                        else
                        {
                            try
                            {
                                List<Album> temp = new List<Album>();
                                Album res;
                                var a = AppSession.collectionsCollectionView.SelectedItems.ToList();
                                foreach (Album al in a)
                                {
                                    temp.Add(al);
                                }
                                try { res = temp.Find(x => x.Id == album.Id); }
                                catch { res = null; }
                                if (res != null)
                                {
                                    // Highlighted
                                    if (album.Name != "Favourites")
                                    {
                                        dragGesture.CanDrag = false;
                                    }
                                    AppSession.collectionsCollectionView.SelectedItems.Remove(res);
                                    AppSession.SetNavHeader("Select a Collection", "FFFFFF");
                                    AppSession.UserCollectionRecipes.Clear();
                                    var favsGroup = new FavouritesCollectionViewSection(null);
                                    AppSession.favouritesCollection.RemoveAt(0);
                                    AppSession.favouritesCollection.Add(favsGroup);
                                    AppSession.collectionsSelectedItem = null;
                                }
                                else
                                {
                                    if (AppSession.collectionsCollectionView.SelectedItems.Count > 0)
                                    {
                                        AppSession.collectionsCollectionView.SelectedItems.Clear();
                                        AppSession.favouritesCollectionView.SelectedItems.Clear();
                                        AppSession.collectionsSelectedItem = null;
                                    }
                                    AppSession.collectionsCollectionView.SelectedItems.Add(album);
                                    AppSession.collectionsSelectedItem = album;
                                    AppSession.SetNavHeader(album.Name, album.FolderColor);
                                    if (album.Name == "Favourites")
                                    {
                                        await App.ShowLoading();
                                        AppSession.UserCollectionRecipes = DataManager.GetFavouriteRecipes();
                                        var favsGroup = new FavouritesCollectionViewSection(AppSession.UserCollectionRecipes);
                                        AppSession.favouritesCollection.RemoveAt(0);
                                        AppSession.favouritesCollection.Add(favsGroup);
                                        await App.HideLoading();
                                    }
                                    else
                                    {
                                        dragGesture.CanDrag = true;
                                        var result = await DataManager.ViewAlbum(album.Id);
                                        AppSession.UserCollectionRecipes = result.Recipes;
                                        var favsGroup = new FavouritesCollectionViewSection(AppSession.UserCollectionRecipes);
                                        AppSession.favouritesCollection.RemoveAt(0);
                                        AppSession.favouritesCollection.Add(favsGroup);
                                        Console.WriteLine();
                                    }
                                    //App.ShowAlert("Successfully loaded collection recipes.");
                                }
                            }
                            catch { }
                            AppSession.SetClearText();
                            AppSession.SetViewText();
                        }
                    });
                })
            });

            Content.Children.Add(outerContainer);
        }
    }
}
