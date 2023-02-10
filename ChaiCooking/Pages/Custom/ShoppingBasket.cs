using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.ShoppingBasket;
using ChaiCooking.Services;
using ChaiCooking.Views.CollectionViews.ShoppingBasket;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class ShoppingBasket : Page
    {
        ShoppingBasketCollectionView shoppingBasketCollectionView;
        CollectionView shoppingBasketLayout;
        StaticLabel Title, totalItemsLabel;
        StaticLabel DisclaimerLabel;
        ColourButton confirmBtn;
        ColourButton clearBtn;
        StaticImage InfoIcon;
        int itemTotal;
        bool ShowDisclaimer;

        public ShoppingBasket()
        {
            this.IsScrollable = false;
            this.IsRefreshable = false;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;
            ShowDisclaimer = false;

            this.Id = (int)AppSettings.PageNames.ShoppingBasket;
            this.Name = AppData.AppText.SHOPPING_BASKET;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.SlideInFromRight;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.SlideOutToRight;

            AppSession.basketClearVisible = UpdateButtons;
            AppSession.updateBasketShortcut = RefreshCollectionView;

            PageContent = new Grid
            {
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                ColumnSpacing = 0,
                RowSpacing = 0,
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                    { new RowDefinition { Height = new GridLength(1)}},
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                    //{ new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Star)}},
                    { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)}},
                },
            };

            Title = new StaticLabel(AppData.AppText.SHOPPING_BASKET + $"({itemTotal} items)"/*"."*/);
            Title.Content.TextColor = Color.White;
            Title.Content.FontSize = Units.FontSizeXL;
            Title.Content.Padding = Dimensions.GENERAL_COMPONENT_SPACING;
            Title.CenterAlign();

            confirmBtn = new ColourButton
                (Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CHECKOUT, null);
            confirmBtn.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            confirmBtn.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            confirmBtn.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(confirmBtn.Content, true);
            TouchEffect.SetCommand(confirmBtn.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (itemTotal > 0)
                        {
                            var result = await DataManager.ConvertToBasket();
                            if (result != null)
                            {
                                await OpenBrowser(result);
                            }
                        }
                        else
                        {
                            App.ShowAlert("Your cart is empty!");
                        }
                    });
                }));

            clearBtn = new ColourButton
            (Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CLEAR, null);
            clearBtn.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            clearBtn.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            clearBtn.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(clearBtn.Content, true);
            TouchEffect.SetCommand(clearBtn.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.ShowRemoveFromBasketModal();
                    });
                }));

            Grid buttonContainer = new Grid
            {
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                ColumnDefinitions =
                {
                },
                RowDefinitions =
                {
                    new RowDefinition{ Height = new GridLength(Dimensions.STANDARD_BUTTON_HEIGHT)}
                },
                Children =
                {
                    {clearBtn.Content,0,0 },
                    {confirmBtn.Content,1,0 }
                }
            };
            clearBtn.Content.IsVisible = false;
            StackLayout seperator = new StackLayout { WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) };

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

            totalItemsLabel = new StaticLabel($"Total Items ({itemTotal})");
            totalItemsLabel.Content.TextColor = Color.White;
            totalItemsLabel.Content.FontSize = Units.FontSizeXL;
            totalItemsLabel.Content.Padding = Dimensions.GENERAL_COMPONENT_PADDING;
            totalItemsLabel.CenterAlign();

            DisclaimerLabel = new StaticLabel("Please be sure to check that the suggested products i.e., vegan pesto match your requirements before checking out. " +
                "We have integrated with Whisk to make this the best it can be but every now and then it may not recognise an exact match to a product.");
            DisclaimerLabel.Content.TextColor = Color.White;
            DisclaimerLabel.Content.FontSize = Units.FontSizeM;
            DisclaimerLabel.Content.Padding = Dimensions.GENERAL_COMPONENT_PADDING;
            DisclaimerLabel.LeftAlign();
            DisclaimerLabel.Content.IsVisible = false;

            InfoIcon = new StaticImage("infoicon.png", 24, null);
            InfoIcon.Content.HorizontalOptions = LayoutOptions.End;
            TouchEffect.SetNativeAnimation(InfoIcon.Content, true);
            TouchEffect.SetCommand(InfoIcon.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        DisclaimerLabel.Content.IsVisible = !DisclaimerLabel.Content.IsVisible;

                        PageContent.Children.Remove(DisclaimerLabel.Content);
                        if (DisclaimerLabel.Content.IsVisible)
                        {
                            PageContent.Children.Add(DisclaimerLabel.Content, 0, 2);
                        }
                    });
                }));

            PageContent.Children.Add(new StackLayout
            {
                WidthRequest = Units.ScreenWidth,
                Padding = new Thickness(16, 0),
                Orientation= StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Children =
                {
                    Title.Content,
                    InfoIcon.Content
                }
            }, 0, 0);

            PageContent.Children.Add(titleSepCont, 0, 1);
            
            PageContent.Children.Add(DisclaimerLabel.Content, 0, 2);
            //PageContent.Children.Add(totalItemsLabel.Content, 0, 3);


            PageContent.Children.Add(BuildCollectionView(), 0, 3);
            PageContent.Children.Add(buttonContainer, 0, 4);

            
        }

        private CollectionView BuildCollectionView()
        {
            shoppingBasketCollectionView = new ShoppingBasketCollectionView();
            shoppingBasketLayout = shoppingBasketCollectionView.GetCollectionView();
            shoppingBasketCollectionView.ShowBasket();
            return shoppingBasketLayout;
        }

        public override async Task Update()
        {
            await base.Update();

            App.SetSubHeaderTitle("", null);

            App.SetSubHeaderTitle(AppText.RECOMMENDED_RECIPES, new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.RecommendedRecipes));
            AppSession.UserCollectionRecipes = DataManager.GetFavouriteRecipes();
            UpdateData();
        }

        void UpdateButtons()
        {
            if(AppSession.shoppingList.content.recipes.Count > 0)
            {
                confirmBtn.Content.Opacity = 1;
                clearBtn.Content.IsVisible = true;
            }
            else
            {
                confirmBtn.Content.Opacity = 0.5f;
                clearBtn.Content.IsVisible = false;
            }
        }

        private async void UpdateData()
        {
            await Task.Delay(100);
            AppSession.shoppingList = DataManager.GetShoppingList().Result;
            RefreshCollectionView();
            UpdateButtons();
        }

        private async void RefreshCollectionView()
        {
            var basketGroup = new ShoppingBasketViewSection(AppSession.shoppingList.content.recipes);//, StaticData.BuildEmpty());
            AppSession.shoppingBasketCollection.RemoveAt(0);
            AppSession.shoppingBasketCollection.Add(basketGroup);
            itemTotal = AppSession.shoppingList.content.recipes.Count;
            totalItemsLabel.Content.Text = $"Total Items ({itemTotal})";
            Title.Content.Text = AppData.AppText.SHOPPING_BASKET + $"({itemTotal} items)";
        }

        private async Task OpenBrowser(Uri uri)
        {
            try
            {
                await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                // An unexpected error occured. No browser may be installed on the device.
            }
        }
    }
}
