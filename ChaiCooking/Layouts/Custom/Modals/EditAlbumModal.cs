﻿using System;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Fields.Custom;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.AlbumAPI;
using ChaiCooking.Services;
using ChaiCooking.Views.CollectionViews.Collections;
using ChaiCooking.Views.CollectionViews.Favourites;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Modals
{
    public class EditAlbumModal : ActiveComponent
    {
        Grid colourGrid;
        FolderTile folderTile;
        bool firstTime;
        public EditAlbumModal(Action updateAlbums)
        {
            firstTime = true;
            // If a recipe is present we can add it after making the collection I guess.
            Label titleLabel = new Label
            {
                Text = AppText.EDIT_COLLECTION_TITLE,
                FontSize = Units.FontSizeXL,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold
            };

            StackLayout titleContainer = new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                //BackgroundColor = Color.Orange,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    titleLabel
                }
            };

            Label closeLabel = new Label
            {
                Text = AppText.CLOSE,
                FontSize = Units.FontSizeL,
                TextColor = Color.White
            };

            TouchEffect.SetNativeAnimation(closeLabel, true);
            TouchEffect.SetCommand(closeLabel,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.HideRecipeSummary();
                    AppSession.collectionsSelectedItem = null;
                    AppSession.collectionsCollectionView.SelectedItem = null;
                });
            }));

            StackLayout closeLabelContainer = new StackLayout
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                //BackgroundColor = Color.Blue,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    closeLabel
                }
            };

            StackLayout titleContent = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                //BackgroundColor = Color.Red,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    titleContainer,
                    closeLabelContainer
                }
            };

            StackLayout seperator = new StackLayout { WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) };

            Label desc1Label = new Label
            {
                Text = "Type your collection name:",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White
            };


            CustomEntry nameEntry = new CustomEntry
            {
                Placeholder = "Type Text Here",
                PlaceholderColor = Color.LightGray,
                Text = AppSession.collectionsSelectedItem.Name,
                HorizontalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.White,
                WidthRequest = Units.HalfScreenWidth,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = Units.TapSizeS,
                FontSize = Units.FontSizeL,
            };

            StackLayout descCombCont = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    nameEntry
                }
            };

            folderTile = new FolderTile("", false, false);
            folderTile.Content.WidthRequest = Dimensions.ALBUM_TILE_WIDTH;
            folderTile.Content.HeightRequest = Dimensions.ALBUM_TILE_HEIGHT;

            StackLayout folderCont = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                  folderTile.Content
                }
            };

            GetColourList();

            Label desc2Label = new Label
            {
                Text = "Select a collection colour:",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White
            };

            colourGrid = new Grid
            {
                ColumnSpacing = 2,
                RowSpacing = 2,
                ColumnDefinitions =
                {
                    { new ColumnDefinition { Width = new GridLength(50)}},
                    { new ColumnDefinition { Width = new GridLength(50)}},
                    { new ColumnDefinition { Width = new GridLength(50)}},
                    { new ColumnDefinition { Width = new GridLength(50)}},
                },
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(50)}},
                    { new RowDefinition { Height = new GridLength(50)}},
                    { new RowDefinition { Height = new GridLength(50)}},
                }
            };

            StackLayout colourCont = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Spacing = 0,
                Children =
                {
                    colourGrid
                }
            };

            Label desc5Label = new Label
            {
                Text = "Do you wish to continue?",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White
            };

            StackLayout descriptionContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Children =
                {
                    desc1Label,
                    descCombCont,
                    desc2Label,
                    colourCont,
                    folderCont,
                    desc5Label
                }
            };

            ColourButton confirmBtn = new ColourButton
            (Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CONFIRM, null);
            confirmBtn.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            confirmBtn.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            confirmBtn.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(confirmBtn.Content, true);
            TouchEffect.SetCommand(confirmBtn.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (StaticData.selectedColour != "" && StaticData.selectedColour != null)
                    {
                        if (nameEntry.Text == "" || nameEntry.Text == null)
                        {
                            App.ShowAlert("Please check if details are correct.");
                        }
                        else
                        {
                            if (nameEntry.Text.ToLower() != "favourites")
                            {
                                var result = await DataManager.EditAlbum(AppSession.collectionsSelectedItem.Id, nameEntry.Text, StaticData.selectedColour);
                                if (result)
                                {
                                    var a = AppSession.UserCollections.Find(x => x.Id == AppSession.collectionsSelectedItem.Id);
                                    a.FolderColor = StaticData.selectedColour;
                                    a.Name = nameEntry.Text;
                                    StaticData.selectedColour = null;
                                    var collectionsGroup = new CollectionsCollectionViewSection(AppSession.UserCollections);
                                    AppSession.collectionsCollection.RemoveAt(0);
                                    AppSession.collectionsCollection.Add(collectionsGroup);
                                    AppSession.SetNavHeader("Select a Collection", "FFFFFF");
                                    AppSession.collectionsSelectedItem = null;
                                    AppSession.collectionsCollectionView.SelectedItem = null;
                                    var favsGroup = new FavouritesCollectionViewSection(null);
                                    AppSession.favouritesCollection.RemoveAt(0);
                                    AppSession.favouritesCollection.Add(favsGroup);
                                    App.ShowAlert("Successfully updated collection.");
                                    await App.HideRecipeSummary();
                                }
                                else
                                {
                                    App.ShowAlert("Failed to update collection.");
                                }
                            }
                            else
                            {
                                App.ShowAlert("Collection name cannot be used.");
                            }
                        }
                    }
                    else
                    {
                        if (AppSession.collectionsSelectedItem.FolderColor != null && AppSession.collectionsSelectedItem.FolderColor != "")
                        {
                            if (nameEntry.Text == "" || nameEntry.Text == null)
                            {
                                App.ShowAlert("Please check if details are correct.");
                            }
                            else
                            {
                                if (nameEntry.Text.ToLower() != "favourites")
                                {
                                    var result = await DataManager.EditAlbum(AppSession.collectionsSelectedItem.Id, nameEntry.Text, AppSession.collectionsSelectedItem.FolderColor);
                                    if (result)
                                    {
                                        var a = AppSession.UserCollections.Find(x => x.Id == AppSession.collectionsSelectedItem.Id);
                                        a.Name = nameEntry.Text;
                                        StaticData.selectedColour = null;
                                        var collectionsGroup = new CollectionsCollectionViewSection(AppSession.UserCollections);
                                        AppSession.collectionsCollection.RemoveAt(0);
                                        AppSession.collectionsCollection.Add(collectionsGroup);
                                        AppSession.SetNavHeader("Select a Collection", "FFFFFF");
                                        AppSession.collectionsSelectedItem = null;
                                        AppSession.collectionsCollectionView.SelectedItem = null;
                                        var favsGroup = new FavouritesCollectionViewSection(null);
                                        AppSession.favouritesCollection.RemoveAt(0);
                                        AppSession.favouritesCollection.Add(favsGroup);
                                        App.ShowAlert("Successfully updated collection.");
                                        await App.HideRecipeSummary();
                                    }
                                    else
                                    {
                                        App.ShowAlert("Failed to update collection.");
                                    }
                                }
                                else
                                {
                                    App.ShowAlert("Collection name cannot be used.");
                                }
                            }
                        }
                        App.ShowAlert("Please select a colour.");
                    }
                });
            }));

            StackLayout btnCont = new StackLayout
            {
                WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH,
                HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Children =
                {
                    confirmBtn.Content
                }
            };

            StackLayout masterContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                WidthRequest = 300,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                Spacing = 0,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                Children =
                {
                    titleContent,
                    seperator,
                    descriptionContainer,
                    btnCont
                }
            };

            Frame frame = new Frame
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Content = masterContainer,
                CornerRadius = 5,
                IsClippedToBounds = true,
                Padding = 0,
            };

            Content.Children.Add(frame);
        }

        public async void GetColourList()
        {
            StaticData.selectedColour = null;
            StaticData.colourModel = await DataManager.GetAlbumColours();
            UpdateTiles();
        }

        public void UpdateTiles()
        {
            if (firstTime)
            {
                folderTile.SetActiveColor(Color.FromHex(AppSession.collectionsSelectedItem.FolderColor));
                firstTime = false;
            }
            else
            {
                if (StaticData.selectedColour == null)
                {
                    folderTile.SetActiveColor(Color.FromHex("ffffff"));
                }
                else
                {
                    folderTile.SetActiveColor(Color.FromHex(StaticData.selectedColour));
                }
            }

            colourGrid.Children.Clear();
            if (StaticData.colourModel.colourList.Count == 0)
            {
                StackLayout emptyCont = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new Label
                        {
                            Text = "Colours could not be retrieved please check your internet connection.",
                            FontSize = Units.FontSizeM,
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Color.White,
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                    }
                };
                colourGrid.Children.Add(emptyCont, 0, 0);
            }
            else
            {
                int col = 0;
                int row = 0;
                foreach (TileColour c in StaticData.colourModel.colourList)
                {
                    ColourTile colourTile = new ColourTile();
                    colourTile.SetTile(c);
                    colourTile.UpdateData += this.UpdateTiles;
                    if (c.isHighlighted)
                    {
                        colourTile.SetHighlight(true);
                    }
                    else
                    {
                        colourTile.SetHighlight(false);
                    }

                    colourGrid.Children.Add(colourTile.Content, col, row);
                    col++;
                    if (col == 4) { row++; col = 0; }
                }
            }
        }
    }
}
