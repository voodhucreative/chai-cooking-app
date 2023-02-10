using System;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using ChaiCooking.Views.CollectionViews;
using ChaiCooking.Views.CollectionViews.IngredientFilter;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Tiles
{
    // This tile is for 
    public class IngredientFilterTile : ActiveComponent
    {
        Grid masterGrid; // Master Grid will define the base layout
        public StaticLabel nameLabel { get; set; }
        public StaticImage removeImage { get; set; }
        public Ingredient ingredient { get; set; }
        public IngredientFilterTile(Ingredient input)
        {
            this.ingredient = input;
            nameLabel = new StaticLabel(input.Name);
            nameLabel.Content.TextColor = Color.White;
            nameLabel.CenterAlign();
            nameLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            nameLabel.Content.FontSize = Units.FontSizeL;
            nameLabel.Content.LineBreakMode = LineBreakMode.CharacterWrap;

            removeImage = new StaticImage("closecirclewhite.png", 16, null);
            removeImage.Content.HeightRequest = 16;
            removeImage.Content.WidthRequest = 16;
            removeImage.Content.HorizontalOptions = LayoutOptions.EndAndExpand;
            removeImage.Content.VerticalOptions = LayoutOptions.CenterAndExpand;

            masterGrid = new Grid
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                ColumnSpacing = 0,
                RowSpacing = 0,
                Padding = 0,
                HeightRequest = 20,
                WidthRequest = Units.ThirdScreenWidth,
                IsClippedToBounds = true,
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(24) } }
                },
                ColumnDefinitions =
                {
                    { new ColumnDefinition { Width = new GridLength(Units.ThirdScreenWidth - 24)} },
                    { new ColumnDefinition { Width = new GridLength(24)} }
                },
                Children =
                {
                    {nameLabel.Content,0,0},
                    {removeImage.Content,1,0}
                }
            };
            TouchEffect.SetNativeAnimation(masterGrid, true);
            TouchEffect.SetCommand(masterGrid,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        AppDataContent.AvailableIngredients.Remove(ingredient);
                        AppSession.CurrentPageWaste = 1;
                        //StaticData.callUpdate();
                    }
                    catch { }
                });
            }));

            Frame newframe = new Frame
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Content = masterGrid,
                BackgroundColor = Color.FromHex(Colors.CC_ORANGE),
                HeightRequest = 20,
                CornerRadius = 10,
                HasShadow = false,
                IsClippedToBounds = true,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
            };

            Content.Children.Add(newframe);
        }

        public IngredientFilterTile(Ingredient input, bool isModal)
        {
            this.ingredient = input;
            nameLabel = new StaticLabel(input.Name);
            nameLabel.Content.TextColor = Color.White;
            nameLabel.CenterAlign();
            nameLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            nameLabel.Content.FontSize = Units.FontSizeL;
            nameLabel.Content.LineBreakMode = LineBreakMode.TailTruncation;

            removeImage = new StaticImage("closecirclewhite.png", 16, null);
            removeImage.Content.HeightRequest = 16;
            removeImage.Content.WidthRequest = 16;
            removeImage.Content.HorizontalOptions = LayoutOptions.EndAndExpand;
            removeImage.Content.VerticalOptions = LayoutOptions.CenterAndExpand;

            masterGrid = new Grid
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                ColumnSpacing = 0,
                RowSpacing = 0,
                Padding = 0,
                HeightRequest = 20,
                WidthRequest = Units.ThirdScreenWidth,
                IsClippedToBounds = true,
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(24) } }
                },
                ColumnDefinitions =
                {
                    { new ColumnDefinition { Width = new GridLength(Units.ThirdScreenWidth - 24)} },
                    { new ColumnDefinition { Width = new GridLength(24)} }
                },
                Children =
                {
                    {nameLabel.Content,0,0},
                    {removeImage.Content,1,0}
                }
            };

            TouchEffect.SetNativeAnimation(masterGrid, true);
            TouchEffect.SetCommand(masterGrid,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        if (AppSession.InfoModeOn)
                        {
                            double x = Tools.Screen.GetScreenCoordinates(masterGrid).X;
                            double y = Tools.Screen.GetScreenCoordinates(masterGrid).Y;
                            //App.ShowInfoBubble(new Label { Text = "Tap the x button on an ingredient to remove it from the filter list. The recipe finder will update to reflect your changes" }, (int)x+240, (int)y);
                            App.ShowInfoBubble(new Paragraph("Remove Ingredient", "Tap the X button on an ingredient to remove it from the filter list.The recipe finder will update to reflect your changes", null).Content, (int)x, (int)y);

                        }
                        else
                        {

                            if (isModal)
                            {
                                AppSession.ingredientsFilterModalList.Remove(ingredient);
                                var ingredientsFilterGroup = new IngredientsCollectionViewSection(AppSession.ingredientsFilterModalList, BuildEmpty());
                                AppSession.ingredientsFilterModalCollection.Add(ingredientsFilterGroup);
                                AppSession.ingredientsFilterModalCollection.RemoveAt(0);
                            }
                            else
                            {
                                AppDataContent.AvailableIngredients.Remove(ingredient);
                                var ingredientsGroup = new IngredientsCollectionViewSection(AppDataContent.AvailableIngredients, BuildEmpty());
                                AppSession.ingredientsCollection.Add(ingredientsGroup);
                                AppSession.ingredientsCollection.RemoveAt(0);
                                DataManager.FilterRecipesByIngredients(AppDataContent.AvailableIngredients, AppDataContent.AvoidedIngredients);
                                AppSession.WasteLessRecipes = DataManager.GetWasteLessRecipes(AppSession.CurrentUser, true);
                                var wasteLessGroup = new RecipesCollectionViewSection(AppSession.WasteLessRecipes);
                                AppSession.wasteLessCollection.Add(wasteLessGroup);
                                AppSession.wasteLessCollection.RemoveAt(0);
                                AppSession.wasteLessUpdate();
                            }
                            // changes
                            AppSession.CurrentPageWaste = 1;
                            //StaticData.callUpdate();
                        }
                    }
                    catch { }
                });
            }));

            Frame newframe = new Frame
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Content = masterGrid,
                BackgroundColor = Color.FromHex(Colors.CC_ORANGE),
                HeightRequest = 20,
                CornerRadius = 10,
                HasShadow = false,
                IsClippedToBounds = true,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
            };

            Content.Children.Add(newframe);
        }

        private StackLayout BuildEmpty()
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
                            Text = "No ingredients added.",
                            FontSize = Units.FontSizeL,
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Color.White,
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                    }
            };

            return emptyCont;
        }
    }
}
