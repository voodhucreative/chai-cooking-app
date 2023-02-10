using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ChaiCooking.Models
{
    public class Location : ICloneable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty("id")]
        public string id;
        public string Id
        {
            set
            {
                if (id != value)
                {
                    id = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Id"));
                }
            }
            get
            {
                return id;
            }
        }


        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        public Location()
        {

        }

        public object Clone()
        {
            return (User)this.MemberwiseClone();
        }
    }
}
