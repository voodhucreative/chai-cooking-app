using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChaiCooking.Models.Custom.InfluencerAPI
{
    public class InfluencerMealPlans
    {
        [JsonProperty("data")]
        public List<Datum> Data { get; set; }

        public partial class Datum
        {
            public int id { get; set; }
            public int user_id { get; set; }
            public int number_of_weeks { get; set; }
            public string name { get; set; }
            public string published_at { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
        }
    }
}
