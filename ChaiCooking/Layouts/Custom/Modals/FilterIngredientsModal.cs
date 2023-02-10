using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using ChaiCooking.Views.CollectionViews;
using ChaiCooking.Views.CollectionViews.IngredientFilter;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Modals
{
    public class FilterIngredientsModal : StandardLayout
    {
        CollectionView ingredientsLayout;
        IngredientsFilterModalCollectionView ingredientsCollectionView;
        CustomEntry availableInput;
        public FilterIngredientsModal()
        {
            AppSession.ingredientsFilterModalList = AppDataContent.AvailableIngredients;

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

            StackLayout SearchBox = new StackLayout
            {
                WidthRequest = Units.ScreenWidth,
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };

            StaticLabel SearchBoxDesc1 = new StaticLabel("Add your available ingredients.");
            SearchBoxDesc1.Content.TextColor = Color.White;
            SearchBoxDesc1.Content.FontSize = Units.FontSizeM;
            SearchBoxDesc1.Content.Padding = new Thickness(Dimensions.GENERAL_COMPONENT_SPACING, 8);
            SearchBoxDesc1.CenterAlign();

            availableInput = new CustomEntry
            {
                Placeholder = "Add an Ingredient",
                PlaceholderColor = Color.LightGray,
                HorizontalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.White,
                WidthRequest = Units.HalfScreenWidth,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = Units.TapSizeS,
                FontSize = Units.FontSizeL,
            };

            IconButton addButton = new IconButton(52, 32, Color.FromHex(Colors.CC_ORANGE), Color.White, "", "plus.png", null);
            addButton.SetContentCenter();
            addButton.ContentContainer.Padding = 4;
            addButton.SetIconSize(32, 32);

            TouchEffect.SetNativeAnimation(addButton.Content, true);
            TouchEffect.SetCommand(addButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        double x = Tools.Screen.GetScreenCoordinates(addButton.Content).X;
                        double y = Tools.Screen.GetScreenCoordinates(addButton.Content).Y;
                        //App.ShowInfoBubble(new Label { Text = "Tap the + button, when you’ve typed in one or multiple ingredients, to add them to the list of filters on this page.The recipe finder will update appropriately." }, (int)x, (int)y);
                        App.ShowInfoBubble(new Paragraph("Add Ingredient", "Tap the + button, when you’ve typed in one or multiple ingredients, to add them to the list of filters on this page.The recipe finder will update appropriately.", null).Content, (int)x, (int)y);


                    }
                    else
                    {
                        await AddIngredient();
                    }
                });
            }));

            StackLayout SearchBoxMid = new StackLayout
            {
                WidthRequest = Units.ScreenWidth,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                Children =
                {
                    availableInput,
                    addButton.Content
                }
            };

            StaticLabel SearchBoxDesc2 = new StaticLabel("Your ingredients.");
            SearchBoxDesc2.Content.TextColor = Color.White;
            SearchBoxDesc2.Content.FontSize = Units.FontSizeM;
            SearchBoxDesc2.Content.Padding = new Thickness(Dimensions.GENERAL_COMPONENT_SPACING, 8);
            SearchBoxDesc2.CenterAlign();


            SearchBox.Children.Add(SearchBoxDesc1.Content);
            SearchBox.Children.Add(SearchBoxMid);
            SearchBox.Children.Add(SearchBoxDesc2.Content);

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
                    AppSession.CurrentPageWaste = 1;
                    // update logic
                    AppDataContent.AvailableIngredients = AppSession.ingredientsFilterModalList;
                    var ingredientsGroup = new IngredientsCollectionViewSection(AppDataContent.AvailableIngredients, BuildEmpty());
                    AppSession.ingredientsCollection.Add(ingredientsGroup);
                    AppSession.ingredientsCollection.RemoveAt(0);
                    DataManager.FilterRecipesByIngredients(AppDataContent.AvailableIngredients, AppDataContent.AvoidedIngredients);
                    AppSession.WasteLessRecipes = DataManager.GetWasteLessRecipes(AppSession.CurrentUser, true);
                    var wasteLessGroup = new RecipesCollectionViewSection(AppSession.WasteLessRecipes);
                    AppSession.wasteLessCollection.Add(wasteLessGroup);
                    AppSession.wasteLessCollection.RemoveAt(0);
                    AppSession.wasteLessUpdate();
                    await App.HideModalAsync();
                });
            }));

            StackLayout btnCont = new StackLayout
            {
                WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Children =
                {
                    confirmBtn.Content
                }
            };

            Label titleLabel = new Label
            {
                Text = "Add Ingredients",
                FontFamily = Fonts.GetBoldAppFont(),
                FontSize = Units.FontSizeXL,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White
            };

            Grid masterGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                RowSpacing = 5,
                RowDefinitions =
                    {
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    },
                Children =
                {
                    {closeLabelContainer, 0,0 },
                    { titleLabel, 0,1 },
                    { SearchBox, 0, 2 },
                    {BuildIngredientsSection(), 0, 3 },
                    { btnCont, 0, 4 }
                }
            };

            Frame frame = new Frame
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Content = masterGrid,
                CornerRadius = 5,
                IsClippedToBounds = true,
                Padding = 0,
            };

            Content.Children.Add(frame);
        }

        private CollectionView BuildIngredientsSection()
        {
            ingredientsCollectionView = new IngredientsFilterModalCollectionView();
            ingredientsLayout = ingredientsCollectionView.GetCollectionView();
            ingredientsCollectionView.ShowIngredients();
            return ingredientsLayout;
        }

        private async Task<bool> AddIngredient()
        {
            if (availableInput.Text != null)
            {
                if (availableInput.Text.Count() > 0)
                {
                    await Task.Delay(50);
                    bool containsItem = AppDataContent.AvailableIngredients.Any(item => item.Name.ToLower() == availableInput.Text.ToLower());
                    bool containsItem2 = AppSession.ingredientsFilterModalList.Any(item => item.Name.ToLower() == availableInput.Text.ToLower());
                    if (containsItem || containsItem2)
                    {
                        //AvailableInput.Text = "";
                        App.ShowAlert("Ingredient already listed.");
                        return false;
                    }
                    else
                    {
                        //AvailableInput.Text = "";
                        AppSession.ingredientsFilterModalList.Add(new Ingredient { Id = 0, Name = availableInput.Text, ShortDescription = "", LongDescription = "", MainImage = "" });
                        var ingredientsFilterGroup = new IngredientsCollectionViewSection(AppSession.ingredientsFilterModalList, BuildEmpty());
                        AppSession.ingredientsFilterModalCollection.Add(ingredientsFilterGroup);
                        AppSession.ingredientsFilterModalCollection.RemoveAt(0);

                        availableInput.Text = "";

                        return true;
                    }
                }
                App.ShowAlert("Enter a valid ingredient");
                availableInput.Text = "";
                return false;
            }
            App.ShowAlert("Enter a valid ingredient");
            availableInput.Text = "";
            return false;
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
