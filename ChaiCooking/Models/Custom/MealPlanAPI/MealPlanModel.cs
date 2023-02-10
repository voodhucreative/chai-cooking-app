using System;
using System.Collections.Generic;
using ChaiCooking.Models.Custom.Feed;
using Newtonsoft.Json;

namespace ChaiCooking.Models.Custom
{
    public partial class MealPlanModel
    {
        [JsonProperty("data")]
        public List<Datum> Data { get; set; }

        public partial class Datum
        {
            [JsonProperty("id")]
            public int id { get; set; }

            [JsonProperty("day_number")]
            public int? day_Number { get; set; }

            [JsonProperty("meal_templates")]
            public List<MealTemplate> mealTemplates;
        }
    }
}
