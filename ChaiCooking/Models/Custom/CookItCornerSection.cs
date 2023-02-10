using System;
using Newtonsoft.Json;

namespace ChaiCooking.Models.Custom
{
    public class CookItCornerSection
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public string ComponentInfo { get; set; }

        public string ImageSource { get; set; }

        public Action LinkAction { get; set; }

        public CookItCornerSection()
        {
           
        }
    }
}
