using ChaiCooking.Models.Custom.Feed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChaiCooking.Models.Custom
{
    public partial class MealTemplate
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("meal_type")]
        public string? mealType { get; set; }

        [JsonProperty("recipe")]
        public Recipe recipe;

        [JsonProperty("whisk_recipe_id")]
        public string whiskID { get; set; }
    }
}