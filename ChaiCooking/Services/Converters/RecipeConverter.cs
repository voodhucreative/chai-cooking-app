using System;
using System.Collections.Generic;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.Feed;

namespace ChaiCooking.Services.Converters
{
    public static class RecipeConverter
    {
        public static List<Recipe> RecipeFeedToRecipeList(RecipeFeed inputFeed)
        {
            List<Recipe> outputList = new List<Recipe>();

            foreach(Datum datum in inputFeed.Data)
            {
                Recipe converted = FeedRecipeToFullRecipe(datum);

                if (converted != null)
                {
                    outputList.Add(FeedRecipeToFullRecipe(datum));
                }
            }

            return outputList;
        }

        public static Recipe FeedRecipeToFullRecipe(Datum datum)
        {
            /*
            try
            {
                Recipe recipe = new Recipe
                {
                    // fillable
                    Id = datum.Content.Id,
                    Name = datum.Content.Name,
                    Description = datum.Content.Description,

                    Source = new Models.Custom.Source
                    {
                        Name = datum.Content.Source.Name,
                        DisplayName = datum.Content.Source.DisplayName,
                        SourceRecipeUrl = datum.Content.Source.SourceRecipeUrl,
                        Image = null, // un-fillable
                    },

                    Images = new List<Image>(),
                    MainImageSource = datum.Display.Images[0].Url.ToString(),//.AbsoluteUri,

                    Author = new Models.Custom.Author
                    {
                        Name = datum.Content.Author.Name,
                        Image = null
                    },

                    Creator = new Models.User
                    {

                    },

                    Durations = new Durations
                    {
                        CookTime = datum.Chai.CookTime,
                        PrepTime = datum.Chai.PrepTime,
                        TotalTime = datum.Chai.TotalTime,
                    },

                    // fill any extra Chia specific info
                    CookingTime = datum.Chai.CookTime + " mins",
                    PrepTime = datum.Chai.PrepTime + " mins",
                    MealCategoryImageSource = datum.Chai.MealCategory.ImageUrl.ToString()

                };

                foreach(ImageElement imageElement in datum.Display.Images)
                {
                    recipe.Images.Add(new Image
                    {
                        Url = imageElement.Url,
                    });
                }

                // not available yet
                recipe.StarRating = 4;
                recipe.FavouriteRating = 3;
                recipe.IsFavourite = false;
                recipe.IsSelected = false;
                recipe.Type = "Lunch";


                return recipe;
            }
            catch (Exception e)
            {
                return null;
            }
            */
            return new Recipe();
        }
    }
}
