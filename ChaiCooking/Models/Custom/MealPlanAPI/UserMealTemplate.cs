using System;
using System.Collections.Generic;
using ChaiCooking.Models.Custom.Feed;
using Newtonsoft.Json;

namespace ChaiCooking.Models.Custom.MealPlanAPI
{
    public class UserMealTemplate
    {
        [JsonProperty("data")]
        public List<Datum> Data { get; set; }

        public partial class Datum
        {
            [JsonProperty("date")]
            public string date { get; set; }

            [JsonProperty("meals")]
            public List<Meal> Meals { get; set; }

            [JsonProperty("error")]
            public bool Error { get; set; }

            public int rowIndex { get; set; }
        }
    }
}
