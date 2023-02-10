using System;
using System.Collections.Generic;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Tiles
{
    public class LargeRecipeTile : StandardLayout
    {
        StackLayout ContentContainer;
        ScrollView RecipeScroller;

        StaticLabel Title { get; set; }
        StaticImage MainImage { get; set; }

        StaticLabel IngredientsLabel { get; set; }
        StaticLabel MethodLabel { get; set; }

        Paragraph MainIngredients { get; set; }
        Paragraph ExtraIngredients { get; set; }

        List<Paragraph> Method;

        Recipe Recipe;

        public LargeRecipeTile(Recipe recipe)
        {
            Recipe = recipe;
            ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.White,
                Padding = Dimensions.GENERAL_COMPONENT_SPACING
            };

            RecipeScroller = new ScrollView
            {
                Orientation = ScrollOrientation.Vertical,
                Content = ContentContainer
            };

            StackLayout headerContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            StackLayout ingredientsContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical
            };

            StackLayout methodContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Margin = new Thickness(0, 0, 0, 40)
            };

            StackLayout optionsContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                //Margin = new Thickness(0, 0, 0, 40),
                Margin = Dimensions.GENERAL_COMPONENT_SPACING,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
            };

            Method = new List<Paragraph>();

            Title = new StaticLabel(Recipe.Name);
            Title.Content.TextColor = Color.FromHex(Colors.CC_ORANGE);
            Title.Content.FontFamily = Fonts.GetBoldAppFont();
            Title.Content.FontSize = Units.FontSizeL;


            MainImage = new StaticImage("chaismallbag.png", 128, 128, null);

            /*try
            {
                MainImage = new StaticImage(Recipe.MainImageSource, 128, 128, null);
            }
            catch (Exception e)
            {

            }*/

            try
            {
                if (Recipe.Images != null)
                {
                    if (Recipe.Images[0].Url.ToString().Length == 0)
                    {
                        MainImage = new StaticImage("chaismallbag.png", 128, 128, null);
                    }
                    else
                    {
                        MainImage = new StaticImage(Recipe.Images[0].Url.ToString(), 128, 128, null);
                    }
                }
            }
            catch (Exception e) { }



            MainImage.Content.HorizontalOptions = LayoutOptions.EndAndExpand;

            IngredientsLabel = new StaticLabel("Ingredients");
            IngredientsLabel.Content.FontFamily = Fonts.GetBoldAppFont();

            string ingredientsText = "";

            if (Recipe.Ingredients != null)
            {
                foreach (Ingredient ingredient in Recipe.Ingredients)
                {
                    ingredientsText = ingredient.Text + "\n";
                }
            }


            MainIngredients = new Paragraph(null, ingredientsText, null);
            ExtraIngredients = new Paragraph(null, "Some extra stuff", null);

            MethodLabel = new StaticLabel("Method");
            MethodLabel.Content.FontFamily = Fonts.GetBoldAppFont();

            //string methodText = "";
            if (Recipe.Instructions != null)
            {
                // OUCH
                foreach (Ingredient step in Recipe.Instructions.Steps)
                {
                    Method.Add(new Paragraph(null, step.Text, null));
                }
                /*foreach (Step step in Recipe.Instructions.Steps)
                {
                    Method.Add(new Paragraph(null, step.Text, null));
                }*/
            }


            /*
            Method.Add(new Paragraph(null, "Method part 1", null));
            Method.Add(new Paragraph(null, "Method part 2", null));
            Method.Add(new Paragraph(null, "Method part 3", null));
            Method.Add(new Paragraph(null, "Method part 4", null));*/

            headerContainer.Children.Add(Title.Content);
            headerContainer.Children.Add(MainImage.Content);

            ingredientsContainer.Children.Add(IngredientsLabel.Content);
            ingredientsContainer.Children.Add(new Grid { Opacity = 0.75, WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) });
            ingredientsContainer.Children.Add(MainIngredients.Content);
            ingredientsContainer.Children.Add(ExtraIngredients.Content);
            methodContainer.Children.Add(MethodLabel.Content);
            methodContainer.Children.Add(new Grid { Opacity = 0.75, WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) });

            foreach (Paragraph p in Method)
            {
                methodContainer.Children.Add(p.Content);
            }

            IconLabel Favourites = new IconLabel("faveblack.png", "Favourite", 120, 24);


            Favourites.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                if (AppSession.UserCollectionRecipes.Count < 50)
                                {
                                    await App.ApiBridge.AddFavourite(AppSession.CurrentUser, recipe.Id);
                                    App.ShowAlert(recipe.Name + " added to your favourites.");
                                    Console.WriteLine("Fave Added");
                                }
                                else
                                {
                                    App.ShowAlert("Failed to add to your favourites. Favourite limit (50) has been reached.");
                                }
                            });
                        })
                    }
                );


            IconLabel AddTo = new IconLabel("cartblack.png", "Add to", 100, 24);

            IconLabel ShareThis = new IconLabel("facebookblack.png", "Share this", 120, 24);

            ShareThis.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                if (AppSession.InfoModeOn)
                                {
                                    double x = Tools.Screen.GetScreenCoordinates(ShareThis.Content).X;
                                    double y = Tools.Screen.GetScreenCoordinates(ShareThis.Content).Y;
                                    //App.ShowInfoBubble(new Label { Text = "Tap ‘Share’ to share the recipe on your Facebook page." }, (int)x, (int)y);
                                    App.ShowInfoBubble(new Paragraph("Share", "Tap ‘Share’ to share the recipe on your Facebook page.", null).Content, (int)Units.HalfScreenWidth, (int)Units.HalfScreenHeight);

                                }
                                else
                                {
                                    await Share.RequestAsync(new ShareTextRequest
                                    {
                                        Uri = "https://chai.cooking/",
                                        Title = "Chai Cooking"
                                    });
                                }
                            });
                        })
                    }
                );


            optionsContainer.Children.Add(Favourites.Content);
            //optionsContainer.Children.Add(AddTo.Content); // take out for now
            optionsContainer.Children.Add(ShareThis.Content);


            ContentContainer.Children.Add(headerContainer);
            ContentContainer.Children.Add(ingredientsContainer);
            ContentContainer.Children.Add(methodContainer);
            ContentContainer.Children.Add(optionsContainer);

            Container.Children.Add(RecipeScroller);
            Content.Children.Add(Container);

        }

        public void ResetScroll()
        {
            RecipeScroller.ScrollToAsync(0, 0, true);
        }

    }
}