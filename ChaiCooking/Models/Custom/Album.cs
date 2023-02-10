using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChaiCooking.Models.Custom
{
    public class Album
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("colour")]
        public string FolderColor { get; set; }

        public List<Recipe> Recipes;

        public bool isHighlighted { get; set; }

        public Album()
        {
            Recipes = new List<Recipe>();
        }

        public void AddRecipe(Recipe recipe)
        {
            Recipes.Add(recipe);
        }
    }
}
