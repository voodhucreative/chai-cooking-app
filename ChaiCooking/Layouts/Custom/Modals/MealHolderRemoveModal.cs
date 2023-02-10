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
using ChaiCooking.Services.Storage;
using ChaiCooking.Views.CollectionViews.MealPlanHolder;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Modals
{
    public class MealHolderRemoveModal : ActiveComponent
    {
        List<Recipe> tempList;

        public MealHolderRemoveModal()
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

            descText = "This will remove all recipes from the Recipe Holder.";

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
                    if (AppSession.CurrentUser.recipeHolder != null)
                    {
                        AppSession.CurrentUser.recipeHolder.Clear();
                        App.ShowAlert("Successfully removed all recipes.");
                        AppSession.mealPlanHolderCollectionView.SelectedItems.Clear();
                        AppSession.SelectedRecipe = null;
                        LocalDataStore.SaveAll();
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

        public MealHolderRemoveModal(Recipe recipe)
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

            descText = "This will remove the selected recipe from the Recipe Holder.";

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
                    if (AppSession.CurrentUser.recipeHolder != null)
                    {
                        try
                        {
                            var a = AppSession.CurrentUser.recipeHolder?.Find(x => x?.Id == AppSession.SelectedRecipe?.Id);
                            AppSession.CurrentUser.recipeHolder?.Remove(a);
                            AppSession.SelectedRecipe = null;
                            var mealHolderGroup = new MealPlanHolderCollectionViewSection(AppSession.CurrentUser.recipeHolder);
                            AppSession.mealPlanHolderCollection.RemoveAt(0);
                            AppSession.mealPlanHolderCollection.Add(mealHolderGroup);
                            App.ShowAlert("Successfully removed recipe from recipe holder.");
                            LocalDataStore.SaveAll();
                            await App.HideRecipeSummary();
                        }
                        catch
                        {
                            var mealHolderGroup = new MealPlanHolderCollectionViewSection(AppSession.CurrentUser.recipeHolder);
                            AppSession.mealPlanHolderCollection.RemoveAt(0);
                            AppSession.mealPlanHolderCollection.Add(mealHolderGroup);
                            App.ShowAlert("An error occured trying to remove the recipe.");
                            await App.HideRecipeSummary();
                        }
                    }
                    else
                    {
                        App.ShowAlert("An error occured.");
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

        public MealHolderRemoveModal(IList<object> recipes)
        {
            tempList = new List<Recipe>();

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

            descText = "This will remove the selected recipes from the Recipe Holder.";

            Label desc1Label = new Label
            {
                Text = descText,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White
            };

            Label albumLabel = new Label
            {
                Text = $"{recipes.Count}",
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
                    if (AppSession.CurrentUser.recipeHolder != null)
                    {
                        foreach (Recipe r in AppSession.mealPlanHolderCollectionView.SelectedItems)
                        {
                            tempList.Add(r);
                        }

                        AppSession.CurrentUser.recipeHolder = AppSession.CurrentUser.recipeHolder.Except(tempList).ToList();
                        App.ShowAlert("Successfully removed selected recipes.");
                        AppSession.mealPlanHolderCollectionView.SelectedItems.Clear();
                        AppSession.SelectedRecipe = null;
                        LocalDataStore.SaveAll();
                        var mealHolderGroup = new MealPlanHolderCollectionViewSection(AppSession.CurrentUser.recipeHolder);
                        AppSession.mealPlanHolderCollection.RemoveAt(0);
                        AppSession.mealPlanHolderCollection.Add(mealHolderGroup);
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
                    albumLabel,
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
    }
}
