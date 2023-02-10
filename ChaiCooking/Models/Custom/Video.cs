using System;
using Newtonsoft.Json;

namespace ChaiCooking.Models.Custom
{
    public class Video
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("mobile_image")]
        public string MainImage { get; set; }

        [JsonProperty("url")]
        public string VideoUrl { get; set; }

        public Video()
        {

        }
    }
}
