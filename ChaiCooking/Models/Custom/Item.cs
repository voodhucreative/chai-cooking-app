using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ChaiCooking.Models.Custom
{
    public class Item : INotifyPropertyChanged
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("short_description")]
        public string ShortDescription { get; set; }

        [JsonProperty("long_description")]
        public string LongDescription { get; set; }

        [JsonProperty("image_path")]
        public string MainImage { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
