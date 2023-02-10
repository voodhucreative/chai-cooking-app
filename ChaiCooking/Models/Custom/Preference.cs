using System;
using Newtonsoft.Json;

namespace ChaiCooking.Models.Custom
{
    public class Preference
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("whisk_name")]
        public string WhiskName { get; set; }

        public bool IsSelected { get; set; }

        public Preference()
        {

        }

        public Preference(string name, string whiskName, bool selected)
        {
            Name = name;
            WhiskName = whiskName;
            IsSelected = selected;
        }

        public Preference(string name, bool selected)
        {
            Name = name;
            IsSelected = selected;
        }
    }
}
