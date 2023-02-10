using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Fields.Custom;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom.Panels.Recipe;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.MealPlanAPI;
using ChaiCooking.Tools;
using FFImageLoading.Transformations;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Modals
{
    class RecipeSummary : StandardLayout
    {
        int topContainerWidth = Units.ScreenWidth - Units.ScreenWidth10Percent;
        int bottomContainerWidth = Units.ScreenWidth - Units.ScreenWidth10Percent;
        StaticImage heartIcon { get; set; }
        Frame frame;

        public StackLayout masterContainer, heartContainer;
        public StaticImage favouriteHeart;
        public Grid favGrid;

        public RecipeSummary(Recipe recipe, string mealPeriod)
        {
            Container = new Grid { }; Content = new Grid { };

            DisplayShortRecipe(recipe, mealPeriod);

            if (AppSettings.GoDirectlyToRecipeFromMealPlan)
            {
                if (recipe.Id != null)
                {
                    //DisplayFullRecipeUser(recipe, mealPeriod);
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.ShowFullRecipe(recipe, false, false);
                    });
                }
            }
            else
            {
                DisplayShortRecipe(recipe, mealPeriod);
            }
        }

        public string EmptyCheck(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "0";// mins";
            }
            else
            {
                return s;// '// + " mins";
            }
        }

        public string EmptyCreator(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "-";
            }
            else
            {
                return s;
            }
        }

        public void DisplayShortRecipe(Recipe recipe, string mealPeriod)
        {
            masterContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.White,
                Spacing = 0,
            };

            heartContainer = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
            };

            if (AppSession.UserCollectionRecipes != null)
            {
                var s = AppSession.UserCollectionRecipes.Find(x => x.Id == recipe.Id);
                if (s != null && s.Id == recipe.Id)
                {
                    Color tintColour = Color.Red;

                    TintTransformation colorTint = new TintTransformation
                    {
                        HexColor = (string)tintColour.ToHex(),
                        EnableSolidColor = true

                    };

                    heartIcon = new StaticImage("hearticonsmall.png", 30, 30, null);
                    heartIcon.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
                    heartIcon.Content.Transformations.Add(colorTint);

                    heartContainer.Children.Add(heartIcon.Content);
                    TouchEffect.SetNativeAnimation(heartIcon.Content, true);
                    TouchEffect.SetCommand(heartIcon.Content,
                    new Command(() =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            App.ShowAlert($"{recipe.Name} has already been added to your favourites.");
                        });
                    }));
                    App.ShowAlert($"{recipe.Name} has already been added to your favourites.");
                }
                else
                {
                    heartIcon = new StaticImage("faveblack.png", 30, 30, null);

                    heartContainer.Children.Add(heartIcon.Content);
                    TouchEffect.SetNativeAnimation(heartIcon.Content, true);
                    TouchEffect.SetCommand(heartIcon.Content,
                    new Command(() =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            favouriteActivate(recipe);
                        });
                    }));
                }
            }
            else
            {
                heartIcon = new StaticImage("faveblack.png", 30, 30, null);

                heartContainer.Children.Add(heartIcon.Content);
                TouchEffect.SetNativeAnimation(heartIcon.Content, true);
                TouchEffect.SetCommand(heartIcon.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        favouriteActivate(recipe);
                    });
                }));
            }

            StaticImage starIcon = new StaticImage("star.png", 30, 30, null);

            StackLayout starContainer = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    starIcon.Content
                }
            };

            StaticSvgImage typeIcon = null;

            try
            {
                typeIcon = new StaticSvgImage(recipe.chai.MealCategory.ImageUrl.AbsoluteUri, 30, 30, null);
                typeIcon.Content.VerticalOptions = LayoutOptions.Start;
            }
            catch
            {
                typeIcon = new StaticSvgImage("", 30, 30, null);
                typeIcon.Content.VerticalOptions = LayoutOptions.Start;
            }

            StackLayout iconStack = new StackLayout // stack em high
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 5,
                Children =
                {
                    heartContainer,
                    //starContainer,
                    typeIcon.Content
                },
            };

            StaticImage recipeImage = null;

            if (recipe.Images != null)
            {
                recipeImage = new StaticImage(recipe.Images[0].Url.AbsoluteUri, 100, 100, null);
                recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
            }
            else if (recipe.Images == null && recipe.chai != null && recipe.chai.CoreIngredient != null)
            {
                try
                {
                    recipeImage = new StaticImage(recipe.chai.CoreIngredient.ImageUrl.AbsoluteUri, 100, 100, null);
                    recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                    recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                }
                catch
                {
                    recipeImage = new StaticImage("chaismallbag.png", 100, 100, null);
                    recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                    recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;
                }
            }
            else
            {

                recipeImage = new StaticImage("chaismallbag.png", 100, 100, null);
                recipeImage.Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                recipeImage.Content.VerticalOptions = LayoutOptions.FillAndExpand;

                try
                {
                    recipeImage = new StaticImage(recipe.MainImageSource, 128, 128, null);
                }
                catch (Exception e)
                {

                }
            }

            StackLayout imageContainer = new StackLayout
            {
                //BackgroundColor = Color.Pink,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    recipeImage.Content
                }
            };
            
            Color IconTintColour = Color.Black;

            TintTransformation iconColorTint = new TintTransformation
            {
                HexColor = (string)IconTintColour.ToHex(),
                EnableSolidColor = true

            };

            StaticImage addToAlbum = new StaticImage("folderadd.png", 30, 30, null);
            addToAlbum.Content.VerticalOptions = LayoutOptions.Start;
            addToAlbum.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
            addToAlbum.Content.Transformations.Add(iconColorTint);


            StaticImage edit = new StaticImage("editproperty.png", 30, 30, null);
            edit.Content.VerticalOptions = LayoutOptions.Start;
            edit.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
            edit.Content.Transformations.Add(iconColorTint);

            StaticImage trash = new StaticImage("trash.png", 30, 30, null);
            trash.Content.VerticalOptions = LayoutOptions.Start;
            trash.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
            trash.Content.Transformations.Add(iconColorTint);

            StackLayout moreIconCont = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 5,
                Children =
                {
                    addToAlbum.Content,
                    edit.Content,
                    trash.Content,
                }
            };

            StackLayout topContainer = new StackLayout
            {
                BackgroundColor = Color.White,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Spacing = 5,
                WidthRequest = topContainerWidth,
                Children =
                {
                    new RecipeDetail().GetContent(),
                    new RecipeSubDetail(recipe, mealPeriod).GetContent(),
                    imageContainer,
                    iconStack,
                }
            };

            topContainer.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.ShowFullRecipe(recipe, false, false);
                        //DisplayFullRecipeUser(recipe, mealPeriod);
                    });
                })
            });

            string recipeName = "";
            if (recipe.Name == null || recipe.Name == "")
            {
                recipeName = recipe.chai.Name;
            }
            else
            {
                recipeName = recipe.Name;
            }


            Label titleLbl = new Label
            {
                TextColor = Color.White,
                Text = TextTools.FirstCharToUpper(recipeName),
                FontAttributes = FontAttributes.Bold,
                FontSize = Units.FontSizeML,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.WordWrap
            };


            StackLayout titleStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                Spacing = 0,
                WidthRequest = Units.ScreenWidth40Percent,
                Children =
                {
                    titleLbl
                }
            };

            Label closeBtnLbl = new Label
            {
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                Text = "Close",
                FontSize = Units.FontSizeS,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.NoWrap
            };

            TouchEffect.SetNativeAnimation(closeBtnLbl, true);
            TouchEffect.SetCommand(closeBtnLbl,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (StaticData.storedInfluencerMealPlan != null)
                        {
                            StaticData.previewPlanRefresh = false;
                            await App.ShowPreviewMealPlan(StaticData.storedInfluencerMealPlan.mealPlanName, StaticData.storedInfluencerMealPlan.week);
                        }
                        else
                        {
                            await App.HideRecipeSummary();
                        }
                    });
                }));

            StackLayout closeBtn = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                Spacing = 0,
                WidthRequest = Units.ScreenWidth10Percent,
                Children =
                {
                    closeBtnLbl
                }
            };

            StackLayout bottomContainer = new StackLayout
            {
                BackgroundColor = Color.Black,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                WidthRequest = bottomContainerWidth,
                Children =
                {
                    //cookBtn,
                    titleStack,
                    //createdByStack,
                    new RecipeCreatedBy(recipe).GetContent(),
                    closeBtn
                }
            };

            masterContainer.Children.Add(topContainer);
            masterContainer.Children.Add(bottomContainer);

            frame = new Frame
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

        public async void favouriteActivate(Recipe recipe)
        {
            if (AppSession.UserCollectionRecipes.Count < 50)
            {
                var result = await App.ApiBridge.AddFavourite(AppSession.CurrentUser, recipe.Id);
                if (result)
                {
                    Color tintColour = Color.Red;

                    TintTransformation colorTint = new TintTransformation
                    {
                        HexColor = (string)tintColour.ToHex(),
                        EnableSolidColor = true

                    };
                    var s = AppSession.UserCollectionRecipes.Find(x => x.Id == recipe.Id);
                    if (s != null && s.Id == recipe.Id)
                    {
                        App.ShowAlert($"{recipe.Name} has already been added to your favourites.");
                    }
                    else
                    {
                        App.ShowAlert($"{recipe.Name} added to your favourites.");
                        AppSession.UserCollectionRecipes.Add(recipe);
                        heartContainer.Children.Remove(heartIcon.Content);
                        heartIcon = new StaticImage("hearticonsmall.png", 30, 30, null);
                        heartIcon.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
                        heartIcon.Content.Transformations.Add(colorTint);
                        heartContainer.Children.Add(heartIcon.Content);
                    }
                }
                else
                {
                    App.ShowAlert($"Failed to add {recipe.Name} to favourites.");
                }
            }
            else
            {
                App.ShowAlert("Failed to add to your favourites. Favourite limit (50) has been reached.");
            }
        }
    }
}
