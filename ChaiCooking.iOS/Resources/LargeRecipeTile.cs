using System;
using System.Collections.Generic;
using ChaiCooking.Branding;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
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
                Margin = new Thickness(0, 32)
            };

            StackLayout optionsContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            Method = new List<Paragraph>();

            Title = new StaticLabel(Recipe.Name);
            Title.Content.TextColor = Color.FromHex(Colors.CC_ORANGE);
            Title.Content.FontFamily = Fonts.GetBoldAppFont();
            Title.Content.FontSize = Units.FontSizeL;

            //MainImage = new StaticImage(Recipe.Images[0].Url.ToString(), 128, 128, null);
            MainImage = new StaticImage("no_image.png", 128, 128, null);
            MainImage.Content.HorizontalOptions = LayoutOptions.EndAndExpand;

            IngredientsLabel = new StaticLabel("Ingredients");
            IngredientsLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            

            string ingredientsText = "";

            if (Recipe.Ingredients != null)
            {
                foreach (Ingredient ingredient in Recipe.Ingredients)
                {
                    ingredientsText += ingredient.Text + "\n";
                }
            }
            

            MainIngredients = new Paragraph(null, ingredientsText, null);
            ExtraIngredients = new Paragraph(null, "Some extra stuff", null);

            MethodLabel = new StaticLabel("Method");
            MethodLabel.Content.FontFamily = Fonts.GetBoldAppFont();

            //string methodText = "";
            if (Recipe.Instructions != null)
            {
                foreach (Step step in Recipe.Instructions.Steps)
                {
                    //methodText += step.Text + "\n";
                    Method.Add(new Paragraph(null, step.Text, null));
                }
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

            IconLabel Favourites = new IconLabel("faveblack.png", "Favourites", 120, 24);

            IconLabel AddTo = new IconLabel("cartblack.png", "Add to", 100, 24);

            IconLabel ShareThis = new IconLabel("facebookblack.png", "Share this", 120, 24);


            optionsContainer.Children.Add(Favourites.Content);
            optionsContainer.Children.Add(AddTo.Content);
            optionsContainer.Children.Add(ShareThis.Content);


            ContentContainer.Children.Add(headerContainer);
            ContentContainer.Children.Add(ingredientsContainer);
            ContentContainer.Children.Add(methodContainer);
            ContentContainer.Children.Add(optionsContainer);

            Container.Children.Add(RecipeScroller);
            Content.Children.Add(Container);

        }
    }
}
