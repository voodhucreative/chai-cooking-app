using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.ShoppingBasket;
using ChaiCooking.Services;
using ChaiCooking.Views.CollectionViews.ShoppingBasket;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Modals
{
    public class RemoveFromBasketModal : ActiveComponent
    {
        List<Recipe> tempList;
        public RemoveFromBasketModal(Recipe recipe)
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
                        AppSession.SelectedRecipe = null;
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

            descText = "This will remove the recipe from your shopping basket:";

            Label desc1Label = new Label
            {
                Text = descText,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White
            };

            Label recipeLabel = new Label
            {
                Text = recipe.Name,
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
                        recipeLabel,
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
                        var result = await DataManager.RemoveRecipeFromShoppingList(recipe);
                        if (result)
                        {
                            App.ShowAlert("Successfully removed recipe.");
                            await App.ShowLoading();
                            AppSession.shoppingList = DataManager.GetShoppingList().Result;
                            AppSession.updateBasketShortcut();
                            AppSession.basketClearVisible();
                            await App.HideLoading();
                        }
                        else
                        {
                            App.ShowAlert("Failed to remove recipe.");
                        }
                        AppSession.SelectedRecipe = null;
                        await App.HideModalAsync();
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

        public RemoveFromBasketModal()
        {
            Label titleLabel = new Label
            {
                Text = "Clear Basket",
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
                        AppSession.SelectedRecipe = null;
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

            descText = "This will remove all recipes from your shopping basket.";

            Label desc1Label = new Label
            {
                Text = descText,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White
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
                        var result = await DataManager.ClearShoppingBasket();
                        if (result)
                        {
                            await Task.Delay(100);
                            // Important to rebuild the object to avoid nullpointerexception
                            AppSession.shoppingList = new ShoppingList();
                            AppSession.shoppingList.content = new ShoppingList.Content();
                            AppSession.shoppingList.content.recipes = new List<Recipe>();
                            AppSession.updateBasketShortcut();
                            AppSession.SelectedRecipe = null;
                            await App.HideModalAsync();
                            AppSession.basketClearVisible();
                        }
                        else
                        {
                            App.ShowAlert("Could not clear basket.");
                            await App.HideModalAsync();
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

        /*private StackLayout BuildEmpty()
        {
            StackLayout emptyCont = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                Children =
                    {
                        new Label
                        {
                            Text = "No recipes in basket.",
                            FontSize = Units.FontSizeL,
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Color.White,
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                    }
            };

            return emptyCont;
        }*/
    }
}
