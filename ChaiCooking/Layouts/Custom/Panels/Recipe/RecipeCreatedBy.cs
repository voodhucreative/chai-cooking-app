using System;
using ChaiCooking.Helpers;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Panels.Recipe
{
    public class RecipeCreatedBy
    {
        public StackLayout Content;

        public RecipeCreatedBy(ChaiCooking.Models.Custom.Recipe recipe)
        {
            int fontSize = Units.FontSizeL;

            if (App.IsSmallScreen())
            {
                fontSize = Units.FontSizeM;
            }

            string creatorName = recipe.Author;

            Label createdTitleLbl = new Label
            {
                TextColor = Color.White,
                Text = EmptyCreator("ChaiCooking"),
                FontSize = fontSize,
                FontAttributes = FontAttributes.Bold,
                LineBreakMode = LineBreakMode.WordWrap
            };

            if (AppSettings.ShowAuthor)
            {
                if (recipe.Creator != null)
                {
                    if (recipe.Creator.FirstName != null && recipe.Creator.FirstName.Length > 0)
                    {
                        createdTitleLbl.Text = recipe.Creator.FirstName;
                    }
                }


                if (recipe.Author != null && recipe.Author.Length > 0)
                {
                    createdTitleLbl.Text = recipe.Author;
                }
            }

            StackLayout createdByTitleStack = new StackLayout // stack em high
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Horizontal,
                Spacing = 0,
                Children =
                {
                    createdTitleLbl
                }
            };

            Label createdByLbl = new Label
            {
                TextColor = Color.White,
                Text = "Created by:",
                FontSize = Units.FontSizeM
            };

            StackLayout createdByStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Spacing = 0,
                Children =
                {
                    createdByLbl,
                    createdByTitleStack
                },
                WidthRequest = Units.ScreenWidth30Percent
            };

            Content = createdByStack;
        }

        public StackLayout GetContent()
        {
            return Content;
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
    }
}
