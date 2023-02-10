using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChaiCooking.Models.Custom
{
    public partial class Week
    {
        [JsonProperty("data")]
        public List<Datum> Data { get; set; }

        public partial class Datum
        {
            [JsonProperty("date")]
            public string date;

            [JsonProperty("meals")]
            public List<MealTemplate> mealTemplates;
        }
    }
}
