using System;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Modals
{
    public class RateModal : StandardLayout
    {
        StaticLabel oneLabel { get; set; }
        StaticLabel twoLabel { get; set; }
        StaticLabel threeLabel { get; set; }
        StaticLabel fourLabel { get; set; }
        StaticLabel fiveLabel { get; set; }
        StaticLabel rating { get; set; }

        public RateModal(Recipe recipe)
        {
            Label titleLabel = new Label
            {
                Text = AppText.RATE_RECIPE,
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
            TouchEffect.SetNativeAnimation(closeLabel, true);
            TouchEffect.SetCommand(closeLabel,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.HideRecipeSummary();
                });
            }));

            StackLayout seperator = new StackLayout { WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) };

            Label descLabel = new Label
            {
                Text = "Please tap a number to rate.",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White
            };

            StackLayout descContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Children =
                {
                    descLabel
                }
            };

            oneLabel = new StaticLabel("1");
            oneLabel.Content.TextColor = Color.White;
            oneLabel.Content.FontAttributes = FontAttributes.Bold;
            oneLabel.Content.HorizontalTextAlignment = TextAlignment.Center;
            oneLabel.Content.VerticalTextAlignment = TextAlignment.Center;

            StackLayout oneCont = new StackLayout
            {
                Children =
                {
                    oneLabel.Content
                }
            };

            oneLabel.Content.GestureRecognizers.Add(
             new TapGestureRecognizer()
             {
                 Command = new Command(() =>
                 {
                     Device.BeginInvokeOnMainThread(async () =>
                     {
                         SetRating(1);
                     });
                 })
             });

            twoLabel = new StaticLabel("2");
            twoLabel.Content.TextColor = Color.White;
            twoLabel.Content.FontAttributes = FontAttributes.Bold;
            twoLabel.Content.HorizontalTextAlignment = TextAlignment.Center;
            twoLabel.Content.VerticalTextAlignment = TextAlignment.Center;

            StackLayout twoCont = new StackLayout
            {
                Children =
                {
                    twoLabel.Content
                }
            };

            twoLabel.Content.GestureRecognizers.Add(
             new TapGestureRecognizer()
             {
                 Command = new Command(() =>
                 {
                     Device.BeginInvokeOnMainThread(async () =>
                     {
                         SetRating(2);
                     });
                 })
             });

            threeLabel = new StaticLabel("3");
            threeLabel.Content.TextColor = Color.White;
            threeLabel.Content.FontAttributes = FontAttributes.Bold;
            threeLabel.Content.HorizontalTextAlignment = TextAlignment.Center;
            threeLabel.Content.VerticalTextAlignment = TextAlignment.Center;

            StackLayout threeCont = new StackLayout
            {
                Children =
                {
                    threeLabel.Content
                }
            };

            threeLabel.Content.GestureRecognizers.Add(
             new TapGestureRecognizer()
             {
                 Command = new Command(() =>
                 {
                     Device.BeginInvokeOnMainThread(async () =>
                     {
                         SetRating(3);
                     });
                 })
             });

            fourLabel = new StaticLabel("4");
            fourLabel.Content.TextColor = Color.White;
            fourLabel.Content.FontAttributes = FontAttributes.Bold;
            fourLabel.Content.HorizontalTextAlignment = TextAlignment.Center;
            fourLabel.Content.VerticalTextAlignment = TextAlignment.Center;

            StackLayout fourCont = new StackLayout
            {
                Children =
                {
                    fourLabel.Content
                }
            };

            fourLabel.Content.GestureRecognizers.Add(
             new TapGestureRecognizer()
             {
                 Command = new Command(() =>
                 {
                     Device.BeginInvokeOnMainThread(async () =>
                     {
                         SetRating(4);
                     });
                 })
             });

            fiveLabel = new StaticLabel("5");
            fiveLabel.Content.TextColor = Color.White;
            fiveLabel.Content.FontAttributes = FontAttributes.Bold;
            fiveLabel.Content.HorizontalTextAlignment = TextAlignment.Center;
            fiveLabel.Content.VerticalTextAlignment = TextAlignment.Center;

            StackLayout fiveCont = new StackLayout
            {
                Children =
                {
                    fiveLabel.Content
                }
            };

            fiveLabel.Content.GestureRecognizers.Add(
             new TapGestureRecognizer()
             {
                 Command = new Command(() =>
                 {
                     Device.BeginInvokeOnMainThread(async () =>
                     {
                         SetRating(5);
                     });
                 })
             });

            StackLayout numberContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Spacing = 20,
                Children =
                {
                    oneCont,
                    twoCont,
                    threeCont,
                    fourCont,
                    fiveCont
                }
            };

            StaticImage recipeImage = new StaticImage("chaismallbag.png", 100, 100, null);

            try
            {
                recipeImage = new StaticImage(recipe.Images[0].Url.ToString(), 100, 100, null);
            }
            catch (Exception e)
            {

            }

            StackLayout imageContainer = new StackLayout
            {
                //BackgroundColor = Color.Blue,
                Children =
                {
                    recipeImage.Content
                }
            };

            StaticImage heartIcon = new StaticImage("heart.png", 25, 25, null);

            rating = new StaticLabel(recipe.FavouriteRating.ToString());
            rating.Content.TextColor = Color.White;
            rating.Content.FontAttributes = FontAttributes.Bold;
            rating.Content.HorizontalTextAlignment = TextAlignment.Center;
            rating.Content.VerticalTextAlignment = TextAlignment.Center;

            Grid heartContainer = new Grid
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    heartIcon.Content,
                    rating.Content
                }
            };

            StaticImage typeIcon = new StaticImage("pizza.png", 25, 25, null);

            try
            {
                typeIcon = new StaticImage(recipe.MealCategoryImageSource, 20, 20, null);
            }
            catch (Exception e) { }

            StackLayout typeContainer = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    typeIcon.Content
                }
            };

            StackLayout iconStack = new StackLayout // stack em high
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Vertical,
                //BackgroundColor = Color.Pink,
                Spacing = 5,
                Children =
                {
                    heartContainer,
                    typeContainer,
                },
            };

            StackLayout topTileCont = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                //BackgroundColor = Color.Green,
                Children =
                {
                    imageContainer,
                    iconStack,
                }
            };

            Label recipeName = new Label
            {
                Text = recipe.Name,
                FontSize = Units.FontSizeL,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.WordWrap
            };

            StackLayout botTileCont = new StackLayout
            {
                BackgroundColor = Color.Black,
                Orientation = StackOrientation.Horizontal,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Children =
                {
                    recipeName
                }
            };

            StackLayout tileCont = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 150,
                HeightRequest = 150,
                Spacing = 0,
                Padding = 0,
                Children =
                {
                    topTileCont,
                    botTileCont
                }
            };

            Frame tileframe = new Frame
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 150,
                HeightRequest = 150,
                BackgroundColor = Color.White,
                Content = tileCont,
                CornerRadius = 5,
                IsClippedToBounds = true,
                Padding = 0,
            };

            ColourButton confirmBtn = new ColourButton
            (Color.FromHex(Colors.CC_GREEN), Color.White, AppText.CONFIRM, null);
            confirmBtn.Content.WidthRequest = 150;
            confirmBtn.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            confirmBtn.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            confirmBtn.Content.GestureRecognizers.Add(
             new TapGestureRecognizer()
             {
                 Command = new Command(() =>
                 {
                     Device.BeginInvokeOnMainThread(async () =>
                     {


                         //if (nameEntry.Text == "" || nameEntry.Text == null || picker.SelectedIndex < 0)
                         //{
                         //    await App.PerformActionAsync(new Models.Action((int)Actions.ActionName.ShowErrorPopup));
                         //}
                         //else
                         //{
                         //    //PopulateCalendarData();
                         //}
                     });
                 })
             });

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
                WidthRequest = 250,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                Spacing = 0,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                Children =
                {
                    titleContent,
                    seperator,
                    descContainer,
                    numberContainer,
                    tileframe,
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

        public void SetRating(int input)
        {
            switch (input)
            {
                case (1):
                    oneLabel.Content.TextColor = Color.Orange;
                    twoLabel.Content.TextColor = Color.White;
                    threeLabel.Content.TextColor = Color.White;
                    fourLabel.Content.TextColor = Color.White;
                    fiveLabel.Content.TextColor = Color.White;
                    rating.Content.Text = "1";
                    break;
                case (2):
                    oneLabel.Content.TextColor = Color.White;
                    twoLabel.Content.TextColor = Color.Orange;
                    threeLabel.Content.TextColor = Color.White;
                    fourLabel.Content.TextColor = Color.White;
                    fiveLabel.Content.TextColor = Color.White;
                    rating.Content.Text = "2";
                    break;
                case (3):
                    oneLabel.Content.TextColor = Color.White;
                    twoLabel.Content.TextColor = Color.White;
                    threeLabel.Content.TextColor = Color.Orange;
                    fourLabel.Content.TextColor = Color.White;
                    fiveLabel.Content.TextColor = Color.White;
                    rating.Content.Text = "3";
                    break;
                case (4):
                    oneLabel.Content.TextColor = Color.White;
                    twoLabel.Content.TextColor = Color.White;
                    threeLabel.Content.TextColor = Color.White;
                    fourLabel.Content.TextColor = Color.Orange;
                    fiveLabel.Content.TextColor = Color.White;
                    rating.Content.Text = "4";
                    break;
                case (5):
                    oneLabel.Content.TextColor = Color.White;
                    twoLabel.Content.TextColor = Color.White;
                    threeLabel.Content.TextColor = Color.White;
                    fourLabel.Content.TextColor = Color.White;
                    fiveLabel.Content.TextColor = Color.Orange;
                    rating.Content.Text = "5";
                    break;
            }
        }
    }
}
