using System;
using System.Collections.Generic;
using ChaiCooking.DebugData.Custom;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Lists
{
    public class RecipeListLayout
    {
        public StackLayout Content;
        List<Recipe> RecipeList;

        int numChunks = 0;
        int currentChunk = 0;
        int currentItem = 0;
        int listPosition = 0;

        public RecipeListLayout(List<Recipe> recipeList)
        {
            Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.Transparent,
                Spacing = Dimensions.GENERAL_COMPONENT_SPACING
            };

            numChunks = recipeList.Count / ApiBridge.ITEMS_PER_CHUNK;
            currentChunk = 0;
            currentItem = 0;
            UpdateList(recipeList);
        }

        public void Reset()
        {
            numChunks = 0;
            currentChunk = 0;
            currentItem = 0;
            listPosition = 0;
            RecipeList.Clear();

        }

        public void UpdateList(List<Recipe> newList)
        {
            RecipeList = newList;



            BuildList();
        }


        public bool LimitReached()
        {
            if (currentChunk > numChunks)
            {
                return true;
            }
            return false;
        }

        public void BuildList()
        {
            int listPosition = 0;
            int startPos = currentChunk * ApiBridge.ITEMS_PER_CHUNK;
            int endPos = startPos + ApiBridge.ITEMS_PER_CHUNK;

            foreach (Recipe recipe in RecipeList)
            {
                if (listPosition >= startPos && listPosition < endPos)
                {
                    WideRecipeTile tile = new WideRecipeTile(recipe);
                    Content.Children.Add(tile.GetContent());
                    currentItem++;
                }
                listPosition++;
            }

        }

        public void LoadNextChunk()
        {
            if (currentChunk < numChunks)
            {
                currentChunk++;

                UpdateList(RecipeList);
            }
        }

        public void LoadLastChunk()
        {
            if (currentChunk > 0)
            {
                currentChunk--;
                UpdateList(RecipeList);
            }
        }

        public StackLayout GetContent()
        {
            return Content;
        }
    }
}