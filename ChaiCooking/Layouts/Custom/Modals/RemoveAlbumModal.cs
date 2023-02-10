using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using ChaiCooking.Views.CollectionViews.Collections;
using ChaiCooking.Views.CollectionViews.Favourites;
using ChaiCooking.Views.CollectionViews.MealPlanHolder;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Modals
{
    public class RemoveAlbumModal : ActiveComponent
    {
        public RemoveAlbumModal(Album album)
        {
            Label titleLabel = new Label
            {
                Text = "Remove Collection",
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
                        await App.HideModalAsync();
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

            StackLayout seperator = new StackLayout
            {
                WidthRequest = Units.ScreenWidth,
                HeightRequest = 1,
                BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY)
            };

            Label desc1Label = new Label
            {
                Text = "This will remove the selected collection: ",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White
            };

            Label albumLabel = new Label
            {
                Text = AppSession.collectionsSelectedItem.Name,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.Orange,
                FontSize = Units.FontSizeXL
            };

            StackLayout descCombCont = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    albumLabel,
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
                    desc5Label
                }
            };

            ColourButton confirmBtn = new ColourButton
            (Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CONFIRM, null);
            confirmBtn.Content.WidthRequest = 150;
            confirmBtn.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            confirmBtn.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(confirmBtn.Content, true);
            TouchEffect.SetCommand(confirmBtn.Content,
                            new Command(() =>
                            {
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    var result = await DataManager.RemoveAlbum(AppSession.collectionsSelectedItem.Id);
                                    if (result)
                                    {
                                        try
                                        {
                                            var a = AppSession.UserCollections.Find(x => x.Id == AppSession.collectionsSelectedItem.Id);
                                            AppSession.UserCollections.Remove(a);
                                            StaticData.selectedColour = null;
                                            var albumsGroup = new CollectionsCollectionViewSection(AppSession.UserCollections);
                                            AppSession.collectionsCollection.RemoveAt(0);
                                            AppSession.collectionsCollection.Add(albumsGroup);
                                            App.ShowAlert("Successfully removed collection.");
                                            AppSession.SetNavHeader("Select a Collection", "FFFFFF");
                                            AppSession.collectionsSelectedItem = null;
                                            AppSession.collectionsCollectionView.SelectedItem = null;
                                            var favsGroup = new FavouritesCollectionViewSection(null);
                                            AppSession.favouritesCollection.RemoveAt(0);
                                            AppSession.favouritesCollection.Add(favsGroup);
                                            await App.HideRecipeSummary();
                                        }
                                        catch
                                        {
                                            var albumsGroup = new CollectionsCollectionViewSection(AppSession.UserCollections);
                                            AppSession.collectionsCollection.RemoveAt(0);
                                            AppSession.collectionsCollection.Add(albumsGroup);
                                            App.ShowAlert("An error occured trying to remove the collection.");
                                            AppSession.SetNavHeader("Select a Collection", "FFFFFF");
                                            AppSession.collectionsSelectedItem = null;
                                            AppSession.collectionsCollectionView.SelectedItem = null;
                                            var favsGroup = new FavouritesCollectionViewSection(null);
                                            AppSession.favouritesCollection.RemoveAt(0);
                                            AppSession.favouritesCollection.Add(favsGroup);
                                            await App.HideRecipeSummary();
                                        }
                                        AppSession.collectionsCollectionView.SelectedItems.Clear();
                                    }
                                    else
                                    {
                                        App.ShowAlert("Failed to remove collection.");
                                    }
                                });
                            }));

            StackLayout btnCont = new StackLayout
            {
                WidthRequest = 150,
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

        public RemoveAlbumModal(Recipe recipe)
        {
            Label titleLabel = new Label
            {
                Text = "Remove Recipe",
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
                        await App.HideModalAsync();
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

            StackLayout seperator = new StackLayout
            {
                WidthRequest = Units.ScreenWidth,
                HeightRequest = 1,
                BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY)
            };

            string descText = "";
            if (AppSession.collectionsSelectedItem.Name == "Favourites")
            {
                descText = "This will remove the selected recipe from your favourites: ";
            }
            else
            {
                descText = "This will remove the selected recipe from the collection: ";
            }

            Label desc1Label = new Label
            {
                Text = descText,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White
            };

            Label albumLabel = new Label
            {
                Text = AppSession.SelectedRecipe.Name,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.Orange,
                FontSize = Units.FontSizeXL
            };

            StackLayout descCombCont = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    albumLabel,
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
                    desc5Label
                }
            };

            ColourButton confirmBtn = new ColourButton
            (Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CONFIRM, null);
            confirmBtn.Content.WidthRequest = 150;
            confirmBtn.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            confirmBtn.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(confirmBtn.Content, true);
            TouchEffect.SetCommand(confirmBtn.Content,
                            new Command(() =>
                            {
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    if (AppSession.collectionsSelectedItem.Name == "Favourites")
                                    {
                                        var result = DataManager.RemoveFavourite(AppSession.SelectedRecipe.Id);
                                        if (result)
                                        {
                                            var a = AppSession.UserCollectionRecipes.Find(x => x.Id == AppSession.SelectedRecipe.Id);
                                            AppSession.UserCollectionRecipes.Remove(a);
                                            AppSession.SelectedRecipe = null;
                                            var favsGroup = new FavouritesCollectionViewSection(AppSession.UserCollectionRecipes);
                                            AppSession.favouritesCollection.RemoveAt(0);
                                            AppSession.favouritesCollection.Add(favsGroup);
                                            App.ShowAlert("Successfully removed favourite from favourites collection.");
                                            await App.HideRecipeSummary();
                                        }
                                        else
                                        {
                                            App.ShowAlert("Failed to remove recipe from favourites collection.");
                                        }
                                    }
                                    else
                                    {
                                        var result = await DataManager.RemoveRecipeFromAlbum(
                                     AppSession.collectionsSelectedItem.Id, AppSession.SelectedRecipe.Id);
                                        if (result)
                                        {
                                            try
                                            {
                                                var a = AppSession.UserCollectionRecipes.Find(x => x.Id == AppSession.SelectedRecipe.Id);
                                                AppSession.UserCollectionRecipes.Remove(a);
                                                AppSession.SelectedRecipe = null;
                                                var favsGroup = new FavouritesCollectionViewSection(AppSession.UserCollectionRecipes);
                                                AppSession.favouritesCollection.RemoveAt(0);
                                                AppSession.favouritesCollection.Add(favsGroup);
                                                App.ShowAlert("Successfully removed recipe from collection.");
                                                await App.HideRecipeSummary();
                                            }
                                            catch
                                            {
                                                var favsGroup = new FavouritesCollectionViewSection(AppSession.UserCollectionRecipes);
                                                AppSession.favouritesCollection.RemoveAt(0);
                                                AppSession.favouritesCollection.Add(favsGroup);
                                                App.ShowAlert("An error occured trying to remove the recipe.");
                                                await App.HideRecipeSummary();
                                            }
                                        }
                                        else
                                        {
                                            App.ShowAlert("Failed to remove recipe from collection.");
                                        }
                                    }
                                });
                            }));

            StackLayout btnCont = new StackLayout
            {
                WidthRequest = 150,
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

        public RemoveAlbumModal(IList<object> recipes)
        {
            Label titleLabel = new Label
            {
                Text = "Remove Recipe",
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
                        await App.HideModalAsync();
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

            StackLayout seperator = new StackLayout
            {
                WidthRequest = Units.ScreenWidth,
                HeightRequest = 1,
                BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY)
            };

            string descText = "";
            if (AppSession.collectionsSelectedItem.Name == "Favourites")
            {
                descText = "This will remove the total selected recipes from your favourites: ";
            }
            else
            {
                descText = "This will remove the total selected recipes from the collection: ";
            }

            Label desc1Label = new Label
            {
                Text = descText,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White
            };

            Label albumLabel = new Label
            {
                Text = recipes.Count.ToString(),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.Orange,
                FontSize = Units.FontSizeXL
            };

            StackLayout descCombCont = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    albumLabel,
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
                    desc5Label
                }
            };

            ColourButton confirmBtn = new ColourButton
            (Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CONFIRM, null);
            confirmBtn.Content.WidthRequest = 150;
            confirmBtn.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            confirmBtn.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(confirmBtn.Content, true);
            TouchEffect.SetCommand(confirmBtn.Content,
                            new Command(() =>
                            {
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    var a = await RemoveRecipesSelected(recipes);
                                    if (a || !a)
                                    {
                                        AppSession.favouritesCollectionView.SelectedItems.Clear();
                                        AppSession.SelectedRecipe = null;
                                        await App.HideRecipeSummary();
                                    }
                                });
                            }));

            StackLayout btnCont = new StackLayout
            {
                WidthRequest = 150,
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

        public RemoveAlbumModal()
        {
            Label titleLabel = new Label
            {
                Text = "Remove Recipe",
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
                        await App.HideModalAsync();
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

            StackLayout seperator = new StackLayout
            {
                WidthRequest = Units.ScreenWidth,
                HeightRequest = 1,
                BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY)
            };

            string descText = "";
            if (AppSession.collectionsSelectedItem.Name == "Favourites")
            {
                descText = "This will remove all recipes from your favourites";
            }
            else
            {
                descText = "This will remove all recipes from the collection";
            }

            Label desc1Label = new Label
            {
                Text = descText,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White
            };

            Label albumLabel = new Label
            {
                Text = "",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.Orange,
                FontSize = Units.FontSizeXL
            };

            StackLayout descCombCont = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    albumLabel,
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
                    desc5Label
                }
            };

            ColourButton confirmBtn = new ColourButton
            (Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CONFIRM, null);
            confirmBtn.Content.WidthRequest = 150;
            confirmBtn.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            confirmBtn.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(confirmBtn.Content, true);
            TouchEffect.SetCommand(confirmBtn.Content,
                            new Command(() =>
                            {
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    var a = await RemoveRecipesAll(AppSession.UserCollectionRecipes);
                                    if (a || !a)
                                    {
                                        AppSession.favouritesCollectionView.SelectedItems.Clear();
                                        AppSession.SelectedRecipe = null;
                                        await App.HideRecipeSummary();
                                    }
                                });
                            }));

            StackLayout btnCont = new StackLayout
            {
                WidthRequest = 150,
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

        private async Task<bool> RemoveRecipesSelected(IList<object> recipes)
        {
            List<Recipe> tempList = new List<Recipe>();
            List<Recipe> errorList = new List<Recipe>();
            if (AppSession.collectionsSelectedItem.Name == "Favourites")
            {
                bool result = false;
                foreach (Recipe r in recipes)
                {
                    await Task.Delay(100);
                    result = DataManager.RemoveFavourite(r.Id);
                    if (result)
                    {
                        try
                        {
                            var a = AppSession.UserCollectionRecipes?.Find(x => x?.Id == r?.Id);
                            if (a != null)
                            {
                                tempList.Add(a);
                            }
                        }
                        catch
                        {
                            errorList.Add(r);
                        }
                    }
                    else
                    {
                        errorList.Add(r);
                    }
                }

                // Create a new list from the 2 lists excluding values in the temp list.
                AppSession.UserCollectionRecipes = AppSession.UserCollectionRecipes.Except(tempList).ToList();
                // Rebuild the collection view with th new list.
                var favsGroup = new FavouritesCollectionViewSection(AppSession.UserCollectionRecipes);
                AppSession.favouritesCollection.RemoveAt(0);
                AppSession.favouritesCollection.Add(favsGroup);


                if (errorList.Count > 0)
                {
                    App.ShowAlert($"{errorList.Count} recipes could not be removed.");
                    return false;
                }
                else
                {
                    App.ShowAlert("Successfully removed selected recipes from favourites collection.");
                    return true;
                }
            }
            else
            {
                bool result = false;
                List<object> rec = new List<object>();
                rec.AddRange(recipes);
                foreach (Recipe r in rec)
                {
                    await Task.Delay(100);
                    result = await DataManager.RemoveRecipeFromAlbum(
                 AppSession.collectionsSelectedItem.Id, r.Id);
                    if (result)
                    {
                        try
                        {
                            var a = AppSession.UserCollectionRecipes?.Find(x => x?.Id == r?.Id);
                            if (a != null)
                            {
                                tempList.Add(a);
                            }
                        }
                        catch
                        {
                            errorList.Add(r);
                        }
                    }
                    else
                    {
                        errorList.Add(r);
                    }
                }


                AppSession.UserCollectionRecipes = AppSession.UserCollectionRecipes.Except(tempList).ToList();
                var favsGroup = new FavouritesCollectionViewSection(AppSession.UserCollectionRecipes);
                AppSession.favouritesCollection.RemoveAt(0);
                AppSession.favouritesCollection.Add(favsGroup);

                if (errorList.Count > 0)
                {
                    App.ShowAlert($"{errorList.Count} recipes could not be removed.");
                    return false;
                }
                else
                {
                    App.ShowAlert("Successfully removed selected recipes from the collection.");
                    return true;
                }
            }
        }

        private async Task<bool> RemoveRecipesAll(List<Recipe> recipes)
        {
            List<Recipe> tempList = new List<Recipe>();
            List<Recipe> errorList = new List<Recipe>();
            if (AppSession.collectionsSelectedItem.Name == "Favourites")
            {
                bool result = false;
                foreach (Recipe r in recipes)
                {
                    await Task.Delay(100);
                    result = DataManager.RemoveFavourite(r.Id);
                    if (result)
                    {
                        try
                        {
                            var a = AppSession.UserCollectionRecipes?.Find(x => x?.Id == r?.Id);
                            if (a != null)
                            {
                                tempList.Add(a);
                            }
                        }
                        catch
                        {
                            errorList.Add(r);
                        }
                    }
                    else
                    {
                        errorList.Add(r);
                    }
                }

                // Create a new list from the 2 lists excluding values in the temp list.
                AppSession.UserCollectionRecipes = AppSession.UserCollectionRecipes.Except(tempList).ToList();
                // Rebuild the collection view with th new list.
                var favsGroup = new FavouritesCollectionViewSection(AppSession.UserCollectionRecipes);
                AppSession.favouritesCollection.RemoveAt(0);
                AppSession.favouritesCollection.Add(favsGroup);


                if (errorList.Count > 0)
                {
                    App.ShowAlert($"{errorList.Count} recipes could not be removed.");
                    return false;
                }
                else
                {
                    App.ShowAlert("Successfully removed all recipes from favourites collection.");
                    return true;
                }
            }
            else
            {
                bool result = false;
                foreach (Recipe r in recipes)
                {
                    await Task.Delay(100);
                    result = await DataManager.RemoveRecipeFromAlbum(AppSession.collectionsSelectedItem.Id, r.Id);

                    if (result)
                    {
                        try
                        {
                            var a = AppSession.UserCollectionRecipes?.Find(x => x?.Id == r?.Id);
                            if (a != null)
                            {
                                tempList.Add(a);
                            }
                        }
                        catch
                        {
                            errorList.Add(r);
                        }
                    }
                    else
                    {
                        errorList.Add(r);
                    }
                }


                AppSession.UserCollectionRecipes = AppSession.UserCollectionRecipes.Except(tempList).ToList();
                var favsGroup = new FavouritesCollectionViewSection(AppSession.UserCollectionRecipes);
                AppSession.favouritesCollection.RemoveAt(0);
                AppSession.favouritesCollection.Add(favsGroup);

                if (errorList.Count > 0)
                {
                    App.ShowAlert($"{errorList.Count} recipes could not be removed.");
                    return false;
                }
                else
                {
                    App.ShowAlert("Successfully removed selected recipes from the collection.");
                    return true;
                }
            }
        }
    }
}
