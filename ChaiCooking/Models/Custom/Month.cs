using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChaiCooking.Models.Custom
{
    public partial class Month
    {
        [JsonProperty("name")]
        public string mealPlanName;

        [JsonProperty("week_1")]
        public Week week1;

        [JsonProperty("week_2")]
        public Week week2;

        [JsonProperty("week_3")]
        public Week week3;

        [JsonProperty("week_4")]
        public Week week4;
    }
}
