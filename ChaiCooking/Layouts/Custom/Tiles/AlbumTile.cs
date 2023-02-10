using System;
using ChaiCooking.Components;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using Xamarin.Forms;
using XFShapeView;

namespace ChaiCooking.Layouts.Custom.Tiles
{
    public class AlbumTile : ActiveComponent
    {
        Grid BackGroundLayer;
        Grid ShapeLayer1;
        Grid ShapeLayer2;
        Grid ContentLayer;

        ShapeView BackgroundSquare;
        ShapeView TopSection;
        ShapeView BottomSection;
        ShapeView MiddleSection;

        public string IconUncheckedImageSource { get; set; }
        public string IconCheckedImageSource { get; set; }
        public StaticImage Icon { get; set; }

        bool IsSelected { get; set; }

        public StaticLabel NameLabel { get; set; }

        public Recipe Recipe { get; set; }

        public AlbumTile(bool isSelected, int width, int height)
        {
            IsSelected = isSelected;
            Recipe = null;


            int tileWidth = width;
            int tileHeight = tileWidth;
            int cornerRadius = tileWidth / (tileWidth/12);
            int innerMargin = cornerRadius/2;

            IconCheckedImageSource = "tick.png";
            IconUncheckedImageSource = "tickbg.png";

            Icon = new StaticImage(IconUncheckedImageSource, height, height, null);

            if (IsSelected)
            {
                Icon.Content.Source = IconCheckedImageSource;
            }

            // re-adjust our parent content layer rows
            Content.HeightRequest = 160;
            Content.RowDefinitions.Add(new RowDefinition { Height = tileHeight });
            Content.RowDefinitions.Add(new RowDefinition { Height = 32 });
            //Container.Margin = Dimensions.GENERAL_COMPONENT_PADDING;

            BackGroundLayer = new Grid
            {
                BackgroundColor = Color.Transparent,
                WidthRequest = tileWidth,
                HeightRequest = tileHeight,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            ShapeLayer1 = new Grid
            {
                BackgroundColor = Color.Transparent,
                WidthRequest = tileWidth-innerMargin,
                HeightRequest = tileHeight-innerMargin,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            ShapeLayer2 = new Grid
            {
                BackgroundColor = Color.Transparent,
                WidthRequest = tileWidth - innerMargin,
                HeightRequest = tileHeight - innerMargin,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            ContentLayer = new Grid
            {
                BackgroundColor = Color.Transparent,
                WidthRequest = tileWidth - innerMargin*2,
                HeightRequest = tileHeight - innerMargin*2,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, innerMargin*4, 0, 0)
            };

            BackgroundSquare = new ShapeView
            {
                ShapeType = ShapeType.Box,
                WidthRequest = tileWidth,
                HeightRequest = tileHeight,
                Color = Color.Orange,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                CornerRadius = cornerRadius
            };

            TopSection = new ShapeView
            {
                ShapeType = ShapeType.Box,
                WidthRequest = tileWidth - innerMargin,
                HeightRequest = tileHeight/2,
                Color = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                CornerRadius = cornerRadius-(innerMargin/2),
            };

            BottomSection = new ShapeView
            {
                ShapeType = ShapeType.Box,
                WidthRequest = tileWidth - innerMargin,
                HeightRequest = tileHeight / 2,
                Color = Color.Black,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                CornerRadius = cornerRadius-(innerMargin/2),
                //Margin = 4
            };

            MiddleSection = new ShapeView
            {
                ShapeType = ShapeType.Box,
                WidthRequest = tileWidth - innerMargin,
                HeightRequest = tileHeight / 2.5,
                Color = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                //Margin = new Thickness(0, tileHeight/4, 0, tileHeight/4)
                //Margin = new Thickness(0, 16, 0, 16)
            };

            NameLabel = new StaticLabel("");
            NameLabel.Content.TextColor = Color.White;
            NameLabel.Content.FontSize = 10;
            NameLabel.Content.FontFamily = Fonts.GetRegularAppFont();
            NameLabel.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            NameLabel.Content.HorizontalTextAlignment = TextAlignment.Center;
            NameLabel.Content.VerticalOptions = LayoutOptions.EndAndExpand;
            NameLabel.Content.VerticalTextAlignment = TextAlignment.End;
            NameLabel.Content.Margin = new Thickness(0, 0, 8, 2);
            NameLabel.Content.HeightRequest = tileHeight / 4;



            BackGroundLayer.Children.Add(BackgroundSquare);

            ShapeLayer1.Children.Add(TopSection, 0, 0);
            ShapeLayer1.Children.Add(BottomSection, 0, 1);
            ShapeLayer1.Children.Add(NameLabel.Content, 0, 1);

            ShapeLayer2.Children.Add(MiddleSection, 0, 0);

            Container.Children.Add(BackGroundLayer);
            Container.Children.Add(ShapeLayer1);
            Container.Children.Add(ShapeLayer2);


            Container.Children.Add(ContentLayer);

            if (IsSelected)
            {
                BackgroundSquare.Opacity = 1;
            }
            else
            {
                BackgroundSquare.Opacity = 0;
            }

            Container.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                Toggle();
                            });
                        })
                    }
                );

            Content.Children.Add(Icon.Content, 0, 1);
            Icon.Content.TranslateTo(0, 20, 0, null);
        }

        public void Select()
        {
            IsSelected = true;

            BackgroundSquare.Opacity = 1;
            Icon.Content.Source = IconCheckedImageSource;

            if (Recipe != null) { Recipe.IsSelected = false; }
        }

        public void UnSelect()
        {
            IsSelected = false;

            BackgroundSquare.Opacity = 0;
            Icon.Content.Source = IconUncheckedImageSource;

            if (Recipe != null) { Recipe.IsSelected = true; }

        }

        public void Toggle()
        {
            IsSelected = !IsSelected;

            if (IsSelected)
            {
                BackgroundSquare.Opacity = 1;
                Icon.Content.Source = IconCheckedImageSource;
                if (Recipe != null) { Recipe.IsSelected = true; }
            }
            else
            {
                BackgroundSquare.Opacity = 0;
                Icon.Content.Source = IconUncheckedImageSource;
                if (Recipe != null) { Recipe.IsSelected = false; }
            }
        }

        public void AddContent(View content)
        {
            ContentLayer.Children.Clear();
            ContentLayer.Children.Add(content);
        }

        public void SetName(string name)
        {
            NameLabel.Content.Text = name;
        }

        // move this elsewhere
        public void SetRecipe(Recipe recipe, int width, int height)
        {
            Recipe = recipe;

            StackLayout RecipeContainer = new StackLayout {
                Orientation = StackOrientation.Horizontal,
                HeightRequest = height,
                WidthRequest = width,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };
            StackLayout IconsContainer = new StackLayout { Orientation = StackOrientation.Vertical, WidthRequest = 24, HeightRequest = 72, Spacing = 4};

            StaticImage RecipeImage = new StaticImage(Recipe.Images[0].Url.ToString(), height, height, null);
            RecipeImage.Content.HorizontalOptions = LayoutOptions.StartAndExpand;
            RecipeImage.Content.VerticalOptions = LayoutOptions.StartAndExpand;

/*
            try
            {
                RecipeImage.Content.Source = Recipe.MainImageSource;
            }
            catch (Exception e)
            {

            }
*/
            StaticImage FaveRatingIcon = new StaticImage("heart.png", height/4, height / 4, null);
            StaticImage StarRatingIcon = new StaticImage("star.png", height / 4, height / 4, null);
            StaticImage TypeIcon = new StaticImage("pizza.png", height / 4, height / 4, null);

            IconsContainer.Children.Add(FaveRatingIcon.Content);
            IconsContainer.Children.Add(StarRatingIcon.Content);
            IconsContainer.Children.Add(TypeIcon.Content);

            RecipeContainer.Children.Add(RecipeImage.Content);
            RecipeContainer.Children.Add(IconsContainer);


            SetName(Recipe.Name);


            ContentLayer.Children.Add(RecipeContainer);
        }
    }
}
