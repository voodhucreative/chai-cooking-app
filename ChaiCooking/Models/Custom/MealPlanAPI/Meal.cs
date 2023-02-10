using System;
using System.Collections.Generic;
using ChaiCooking.Models.Custom.Feed;
using Newtonsoft.Json;

namespace ChaiCooking.Models.Custom.MealPlanAPI
{
    public class Meal
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("meal_type")]
        public string? MealType { get; set; }

        [JsonProperty("recipe")]
        public Recipe Recipe { get; set; }

        [JsonProperty("violates_plan")]
        public bool violatesMealPlan { get; set; }
    }
}
