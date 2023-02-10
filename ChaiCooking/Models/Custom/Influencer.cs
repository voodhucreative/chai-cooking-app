using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChaiCooking.Models.Custom
{
    public class Influencer : User
    {
        public MealPlanModel PublicMealPlans;
        //public string BioText { get; set; }
        public int CreatorPoints { get; set; }

        [JsonProperty("data")]
        public List<Datum> data { get; set; }

        public partial class Datum
        {
            public int id { get; set; }
            public string bio { get; set; }
            public string display_name { get; set; }
            public string image_url { get; set; }
            [JsonProperty("website_links")]
            public List<WebsiteLinks> website_links { get; set; }
        }

        public Influencer()
        {
            PublicMealPlans = new MealPlanModel();
            //BioText = "";
            CreatorPoints = 0;
        }

        public partial class WebsiteLinks
        {
            public int id { get; set; }
            public string user_id { get; set; }
            public string type { get; set; }
            public string ?text { get; set; }
            public string url { get; set; }
        }
    }
}
