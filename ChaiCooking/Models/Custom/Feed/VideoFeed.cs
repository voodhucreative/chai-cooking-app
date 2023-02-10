using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChaiCooking.Models.Custom
{
    public class VideoFeed
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("mobile_image")]
        public string MainImage { get; set; }

        [JsonProperty("videos")]
        public List<Video> Videos;


        public VideoFeed()
        {
        }
    }
}

