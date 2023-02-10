using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChaiCooking.Models.Custom.MealPlanAPI
{
    public partial class UserMealPlans
    {
        [JsonProperty("data")]
        public List<Datum> Data { get; set; }
    }

    public partial class Datum
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("number_of_weeks")]
        public int numOfWeeks { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
