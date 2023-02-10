using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using ChaiCooking.Views.CollectionViews.Collections;
using ChaiCooking.Views.CollectionViews.Favourites;
using FFImageLoading.Transformations;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class Collections : Page
    {
        StackLayout titleContent;
        CollectionsCollectionView collectionsCollectionView;
        FavouritesCollectionView favouritesCollectionView;
        CollectionView albumsLayout, favouritesLayout;
        IconLabel TitleLabel;
        StaticLabel viewLabel;
        VisualStateGroup stateGroup;
        VisualState textActiveState, textInactiveState;
        CancellationTokenSource _tokenSource = null;

        public Collections()
        {
            this.IsScrollable = false;
            this.IsRefreshable = false;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;

            this.Id = (int)AppSettings.PageNames.Collections;
            this.Name = AppData.AppText.COLLECTIONS;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.SlideInFromRight;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.SlideOutToRight;
            _tokenSource = new CancellationTokenSource();
            PageContent = new Grid
            {
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                WidthRequest = Units.ScreenWidth
            };

            stateGroup = new VisualStateGroup();

            textActiveState = new VisualState()
            {
                Name = "Active"
            };
            textActiveState.Setters.Add(new Setter
            {
                Property = Label.TextColorProperty,
                Value = Color.White
            });

            textInactiveState = new VisualState()
            {
                Name = "Inactive"
            };

            textInactiveState.Setters.Add(new Setter
            {
                Property = Label.TextColorProperty,
                Value = Color.White.MultiplyAlpha(0.1),
            });

            stateGroup.States.Add(textActiveState);
            stateGroup.States.Add(textInactiveState);


            StackLayout seperator = new StackLayout
            {
                WidthRequest = Units.ScreenWidth - 8,
                HeightRequest = 1,
                BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY),
            };

            StackLayout titleSepCont = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Center,
                Padding = 0,
                Children =
                {
                    seperator
                }
            };

            Components.Buttons.ImageButton newCollectionBtn = new Components.Buttons.ImageButton("plus.png", "plus.png", "", Color.White, null);
            newCollectionBtn.Content.WidthRequest = 32;
            newCollectionBtn.Content.HorizontalOptions = LayoutOptions.EndAndExpand;
            newCollectionBtn.RightAlign();
            newCollectionBtn.Content.Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING);
            newCollectionBtn.ActiveStateImage.Content.HorizontalOptions = LayoutOptions.EndAndExpand;
            newCollectionBtn.InactiveStateImage.Content.HorizontalOptions = LayoutOptions.EndAndExpand;

            TouchEffect.SetNativeAnimation(newCollectionBtn.Content, true);
            TouchEffect.SetCommand(newCollectionBtn.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    //if (AppSession.InfoModeOn)
                    //{
                    //    App.ShowInfoBubble(new Paragraph("Add", "Tap '+' to make a new empty Collection.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                    //}
                    //else
                    //{
                        await App.ShowCreateAlbumModal(null, true);
                    //}
                });
            }));

            viewLabel = new StaticLabel("View");
            viewLabel.Content.TextColor = Color.White.MultiplyAlpha(0.1);
            viewLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            viewLabel.Content.FontSize = Units.FontSizeL;
            viewLabel.Content.HorizontalTextAlignment = TextAlignment.End;
            viewLabel.Content.VerticalTextAlignment = TextAlignment.Center;

            TouchEffect.SetNativeAnimation(viewLabel.Content, true);
            TouchEffect.SetCommand(viewLabel.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("View", "Tap ‘View Recipe’ to see the full recipe including ingredients and method.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                    }
                    else
                    {
                        if (stateGroup.CurrentState == textInactiveState)
                        {
                            // Do nothing
                        }
                        else if (stateGroup.CurrentState == textActiveState)
                        {
                            if (AppSession.favouritesCollectionView.SelectedItems.Count > 0)
                            {
                                var dataList = AppSession.favouritesCollectionView.SelectedItems.Cast<Recipe>().ToList();

                                await App.ShowFullRecipe(dataList[0], dataList);
                            }
                        }
                    }
                });
            }));

            StackLayout viewCont = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 0,
                HorizontalOptions = LayoutOptions.Center,
                //BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY),
                Children =
                {
                    viewLabel.Content
                }
            };

            VisualStateManager.GetVisualStateGroups(viewLabel.Content).Add(stateGroup);

            StaticImage sortImage = new StaticImage("sorticon.png", 40, null);
            sortImage.Content.HeightRequest = 40;

            Color tintColour = Color.White;

            TintTransformation colorTint = new TintTransformation
            {
                HexColor = (string)tintColour.ToHex(),
                EnableSolidColor = true

            };
            sortImage.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
            sortImage.Content.Transformations.Add(colorTint);

            StaticLabel sortLabel = new StaticLabel("Sort");
            sortLabel.Content.TextColor = Color.White;
            sortLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            sortLabel.Content.FontSize = Units.FontSizeL;
            sortLabel.Content.HorizontalTextAlignment = TextAlignment.End;
            sortLabel.Content.VerticalTextAlignment = TextAlignment.Center;

            StackLayout sortByCont = new StackLayout
            {
                //BackgroundColor = Color.Red,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.End,
                Spacing = 0,
                Children =
                {
                    sortImage.Content,
                    sortLabel.Content
                }
            };

            TouchEffect.SetNativeAnimation(sortByCont, true);
            TouchEffect.SetCommand(sortByCont,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("Sort", "Order your recipes in the sidebar.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                    }
                    else
                    {
                        string favActionSheet = await App.ShowActionSheet("Options", "Cancel", null, new string[] { "Sort Collections", "Sort Recipes" });
                        switch (favActionSheet)
                        {
                            case "Sort Collections":
                                string sortSheet = await App.ShowActionSheet("Sort Collections", "Cancel", null, new string[] { "Ascending", "Descending" });
                                switch (sortSheet)
                                {
                                    case "Ascending":
                                        AppSession.UserCollections = AppSession.UserCollections.OrderBy(x => x.Name).ToList();
                                        var collectionsGroupAscending = new CollectionsCollectionViewSection(AppSession.UserCollections);
                                        AppSession.collectionsCollection.RemoveAt(0);
                                        AppSession.collectionsCollection.Add(collectionsGroupAscending);
                                        break;
                                    case "Descending":
                                        AppSession.UserCollections = AppSession.UserCollections.OrderByDescending(x => x.Name).ToList();
                                        var collectionsGroupDescending = new CollectionsCollectionViewSection(AppSession.UserCollections);
                                        AppSession.collectionsCollection.RemoveAt(0);
                                        AppSession.collectionsCollection.Add(collectionsGroupDescending);
                                        break;
                                }
                                break;
                            case "Sort Recipes":
                                string sortRecipesSheet = await App.ShowActionSheet("Sort Recipes", "Cancel", null, new string[] { "Ascending", "Descending" });
                                if (AppSession.UserCollectionRecipes != null)
                                {
                                    switch (sortRecipesSheet)
                                    {
                                        case "Ascending":
                                            if (AppSession.collectionsSelectedItem != null && AppSession.UserCollectionRecipes.Count > 0)
                                            {
                                                AppSession.UserCollectionRecipes = AppSession.UserCollectionRecipes.OrderBy(x => x.Name).ToList();
                                                var favCollectionGroupAscending = new FavouritesCollectionViewSection(AppSession.UserCollectionRecipes);
                                                AppSession.favouritesCollection.RemoveAt(0);
                                                AppSession.favouritesCollection.Add(favCollectionGroupAscending);
                                            }
                                            else
                                            {
                                                App.ShowAlert("No recipes to sort.");
                                            }
                                            break;
                                        case "Descending":
                                            if (AppSession.collectionsSelectedItem != null && AppSession.UserCollectionRecipes.Count > 0)
                                            {
                                                AppSession.UserCollectionRecipes = AppSession.UserCollectionRecipes.OrderByDescending(x => x.Name).ToList();
                                                var favCollectionGroupDescending = new FavouritesCollectionViewSection(AppSession.UserCollectionRecipes);
                                                AppSession.favouritesCollection.RemoveAt(0);
                                                AppSession.favouritesCollection.Add(favCollectionGroupDescending);
                                            }
                                            else
                                            {
                                                App.ShowAlert("No recipes to sort.");
                                            }
                                            break;
                                    }
                                }
                                else
                                {
                                    App.ShowAlert("No recipes to sort.");
                                }
                                break;
                        }
                    }
                });
            }));

            Grid trashLayout = new Grid
            {
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                //HeightRequest = 70,
                WidthRequest = Units.ScreenWidth - Units.ThirdScreenWidth,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.FromHex(Colors.CC_ORANGE),
                RowSpacing = 2,
                IsClippedToBounds = true,
                RowDefinitions =
                {
                    { new RowDefinition { Height =  new GridLength(1, GridUnitType.Auto)}},
                },
                ColumnDefinitions =
                {
                    { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto)}},
                    { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto)}},
                }
            };

            StaticLabel removeLabel = new StaticLabel("Select recipe(s) or a collection and drag here to remove.");
            removeLabel.Content.TextColor = Color.White;
            removeLabel.Content.FontSize = Units.FontSizeM;
            removeLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            removeLabel.Content.LineBreakMode = LineBreakMode.WordWrap;
            removeLabel.Content.HorizontalTextAlignment = TextAlignment.Start;
            removeLabel.Content.VerticalTextAlignment = TextAlignment.Center;

            //StaticLabel removeAllLabel = new StaticLabel("Or tap here to remove all recipes from the selected collection.");
            //removeAllLabel.Content.TextColor = Color.White;
            //removeAllLabel.Content.FontSize = Units.FontSizeM;
            //removeAllLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            //removeAllLabel.Content.FontAttributes = FontAttributes.Italic;
            //removeAllLabel.Content.LineBreakMode = LineBreakMode.WordWrap;
            //removeAllLabel.Content.HorizontalTextAlignment = TextAlignment.Start;
            //removeAllLabel.Content.VerticalTextAlignment = TextAlignment.Center;

            Components.Buttons.ImageButton removeButton = new Components.Buttons.ImageButton("trash.png", "trash.png", "", Color.White, null);
            removeButton.Content.WidthRequest = 50;
            removeButton.Content.HeightRequest = 50;
            removeButton.Content.HorizontalOptions = LayoutOptions.StartAndExpand;
            removeButton.ActiveStateImage.Content.HorizontalOptions = LayoutOptions.StartAndExpand;
            removeButton.InactiveStateImage.Content.HorizontalOptions = LayoutOptions.StartAndExpand;


            
            TouchEffect.SetNativeAnimation(removeButton.Content, true);
            TouchEffect.SetCommand(removeButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("Delete", "Tap to highlight a collection / one or multiple recipes then drag and drop into the Bin to remove them.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                    }
                });
            }));

            trashLayout.GestureRecognizers.Add(new DropGestureRecognizer()
            {
                AllowDrop = true,
                DragOverCommand = new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async ()
                     =>
                    {
                        trashLayout.BackgroundColor = Color.FromHex(Colors.CC_DARK_ORANGE);
                        removeButton.Content.Scale = 1.3;
                    });
                }),
                DragLeaveCommand = new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async ()
                     =>
                    {
                        trashLayout.BackgroundColor = Color.FromHex(Colors.CC_ORANGE);
                        removeButton.Content.Scale = 1;
                    });
                }),
                DropCommand = new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async ()
                     =>
                    {
                        trashLayout.BackgroundColor = Color.FromHex(Colors.CC_ORANGE);
                        removeButton.Content.Scale = 1;
                        if (AppSession.favouritesCollectionView.SelectedItems.Count <= 1)
                        {
                            if (AppSession.SelectedRecipe != null)
                            {
                                await App.ShowRemoveAlbumModal(AppSession.SelectedRecipe);
                            }
                            else
                            {
                                await App.ShowRemoveAlbumModal(AppSession.collectionsSelectedItem);
                            }
                        }
                        else
                        {
                            var a = AppSession.favouritesCollectionView.SelectedItems;
                            await App.ShowRemoveAlbumModal(recipes: AppSession.favouritesCollectionView.SelectedItems);
                        }
                    });
                })
                //DropCommand = command,
            });

            Grid TopContainer = new Grid
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                WidthRequest = Units.ScreenWidth - Units.ThirdScreenWidth,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                //BackgroundColor = Color.Green,
                ColumnSpacing = 0,
                ColumnDefinitions =
                {
                    { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }}
                },
                Children =
                {
                    { sortByCont, 0, 0 },
                }
            };

            trashLayout.Children.Add(removeButton.Content, 0, 0);
            Grid.SetRowSpan(removeButton.Content, 2);
            trashLayout.Children.Add(removeLabel.Content, 1, 0);
            //trashLayout.Children.Add(removeAllLabel.Content, 1, 1);

            StackLayout albumContainer = new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Color.Yellow,
                Orientation = StackOrientation.Vertical,
                Spacing = 0,
                Children =
                {
                    BuildAlbumContent(),
                    trashLayout
                }
            };

            Label clearLabel = new Label()
            {
                FontFamily = Fonts.GetBoldAppFont(),
                Text = "Clear",
                FontSize = Units.FontSizeL,
                TextColor = Color.White.MultiplyAlpha(0.1),
            };

            StackLayout clearLabelCont = new StackLayout()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                Children =
                {
                    clearLabel
                }
            };

            TouchEffect.SetNativeAnimation(clearLabel, true);
            TouchEffect.SetCommand(clearLabel,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("Clear", "Clear collection.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                    }
                    else
                    {
                        if (AppSession.collectionsSelectedItem != null)
                        {
                            if (AppSession.UserCollectionRecipes != null && AppSession.UserCollectionRecipes.Count > 0)
                            {
                                await App.ShowRemoveAlbumModal();
                            }
                            else
                            {
                                App.ShowAlert("No recipes available to delete.");
                            }
                        }
                        else
                        {
                            App.ShowAlert("Please select a collection.");
                        }
                    }
                });
            }));

            Grid favouritesContainer = new Grid
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                HeightRequest = Units.ScreenHeight,
                WidthRequest = Units.ThirdScreenWidth,
                BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY),
                Padding = 0,
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Star)}},
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                },
                Children =
                {
                    { BuildFavouritesContent(), 0, 0 },
                    { clearLabelCont, 0, 1},
                }
            };


            favouritesContainer.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                if (AppSession.InfoModeOn)
                                {
                                    App.ShowInfoBubble(new Paragraph("Recipes", "Drag and drop a single or multiple recipes onto an album to add them to it. The < > buttons will cycle through the albums you’ve already created.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);

                                }
                            });
                        })
                    }
                );

            titleContent = new StackLayout
            {
                /* BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                 WidthRequest = Units.ScreenWidth,
                 HeightRequest = Dimensions.NAVHEADER_HEIGHT,
                 Orientation = StackOrientation.Horizontal,
                 Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                 Spacing = Dimensions.GENERAL_COMPONENT_SPACING,*/
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                WidthRequest = Units.ScreenWidth,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center
            };


            /*StackLayout buttonsContainer = new StackLayout
            {
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                WidthRequest = Units.ScreenWidth,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center
            };*/


            TitleLabel = new IconLabel("folderfg.png", "Select a Collection", 200, Dimensions.HEADER_HEIGHT);
            TitleLabel.SetIconColour(Color.White.ToHex());
            TitleLabel.TextContent.Content.TextColor = Color.White;
            TitleLabel.TextContent.Content.FontFamily = Fonts.GetBoldAppFont();
            TitleLabel.Content.WidthRequest = Units.ScreenWidth;
            TitleLabel.TextContent.Content.FontSize = Units.FontSizeL;
            TitleLabel.SetIconSize(Dimensions.ICON_LABEL_ICON_SIZE, Dimensions.ICON_LABEL_ICON_SIZE);
            titleContent.Children.Add(TitleLabel.Content);

            titleContent.Children.Add(newCollectionBtn.Content);

            Grid masterGrid = new Grid
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                ColumnSpacing = 0,
                RowSpacing = 0,
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                    { new RowDefinition { Height = new GridLength(1)}},
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                },
                ColumnDefinitions =
                {
                     { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)}},
                     { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto)}},
                },
                Children =
                {
                    { titleContent, 0, 0 },
                    { titleSepCont, 0, 1 },
                    { TopContainer, 0, 2},
                    { albumContainer, 0, 3 },
                    { viewCont, 1, 2 },
                    { favouritesContainer, 1, 3 },
                }
            };
            Grid.SetColumnSpan(titleContent, 2);
            Grid.SetColumnSpan(titleSepCont, 2);

            StackLayout iOSPadding = new StackLayout
            {
                HeightRequest = 15,
            };

            if (Device.RuntimePlatform == Device.iOS)
            {
                masterGrid.Children.Add(iOSPadding, 0, 4);
                Grid.SetColumnSpan(iOSPadding, 2);
            }

            Action<string, string> selectedAction = (r, y) =>
            {
                BuildTitle(r, y);
            };

            Action setViewText = () =>
            {
                if (AppSession.favouritesCollectionView.SelectedItems.Count > 0)
                {
                    VisualStateManager.GoToState(viewLabel.Content, "Active");
                }
                else
                {
                    VisualStateManager.GoToState(viewLabel.Content, "Inactive");
                }
            };

            Action setClearText = () =>
            {
                if (AppSession.UserCollectionRecipes.Count < 1)
                {
                    clearLabel.TextColor = Color.White.MultiplyAlpha(0.1);
                }
                else
                {
                    clearLabel.TextColor = Color.White;
                }
            };

            AppSession.SetNavHeader = selectedAction;
            AppSession.SetViewText = setViewText;
            AppSession.SetClearText = setClearText;
            PageContent.Children.Add(masterGrid);
        }

        public void BuildTitle(string collectionName, string colour)
        {
            TitleLabel.TextContent.Content.Text = collectionName;
            TitleLabel.SetIconColour(colour);
            if (collectionName == "Select a Collection")
            {
                VisualStateManager.GoToState(viewLabel.Content, "Inactive");
            }
        }

        public override Task Update()
        {
            if (StaticData.collectionsClicked)
            {
                AppSession.UserCollectionRecipes = DataManager.GetFavouriteRecipes();
                UpdateData();
                StaticData.collectionsClicked = false;
            }
            App.SetSubHeaderTitle(AppText.RECOMMENDED_RECIPES, new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.RecommendedRecipes));

            return base.Update();
        }

        private async void UpdateData()
        {
            if (AppSession.UserCollections != null)
            {
                await Task.Delay(100);
                _tokenSource.Cancel();
                _tokenSource = new CancellationTokenSource();
                AppSession.UserCollections = await DataManager.GetAlbums(_tokenSource.Token, null);
                await Task.Delay(1000);
                Album favAlbum = new Album()
                {
                    Id = "-1",
                    Name = "Favourites",
                    FolderColor = "FFFFFF",
                };
                AppSession.UserCollections.Insert(0, favAlbum);
                var albumsGroup = new CollectionsCollectionViewSection(AppSession.UserCollections);
                AppSession.collectionsCollection.RemoveAt(0);
                AppSession.collectionsCollection.Add(albumsGroup);
            }
        }

        private CollectionView BuildAlbumContent()
        {
            collectionsCollectionView = new CollectionsCollectionView();
            albumsLayout = collectionsCollectionView.GetCollectionView();
            collectionsCollectionView.ShowAlbums();
            return albumsLayout;
        }

        private CollectionView BuildFavouritesContent()
        {
            favouritesCollectionView = new FavouritesCollectionView();
            favouritesLayout = favouritesCollectionView.GetCollectionView();
            favouritesCollectionView.ShowFavourites();


            return favouritesLayout;
        }
    }
}
