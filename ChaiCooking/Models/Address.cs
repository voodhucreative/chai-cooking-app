using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ChaiCooking.Models
{
    public class Address : ICloneable, INotifyPropertyChanged
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


        [JsonProperty("address_line_1")]
        public string address_line_1;
        public string AddressLine1
        {
            set
            {
                if (address_line_1 != value)
                {
                    address_line_1 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AddressLine1"));
                }
            }
            get
            {
                return address_line_1;
            }
        }

        [JsonProperty("address_line_2")]
        public string address_line_2;
        public string AddressLine2
        {
            set
            {
                if (address_line_2 != value)
                {
                    address_line_2 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AddressLine2"));
                }
            }
            get
            {
                return address_line_2;
            }
        }


        [JsonProperty("address_line_3")]
        public string address_line_3;
        public string AddressLine3
        {
            set
            {
                if (address_line_3 != value)
                {
                    address_line_3 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AddressLine3"));
                }
            }
            get
            {
                return address_line_3;
            }
        }

        [JsonProperty("address_line_4")]
        public string address_line_4;
        public string AddressLine4
        {
            set
            {
                if (address_line_4 != value)
                {
                    address_line_4 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AddressLine4"));
                }
            }
            get
            {
                return address_line_4;
            }
        }

        [JsonProperty("area_code")]
        public string area_code;
        public string AreaCode
        {
            set
            {
                if (area_code != value)
                {
                    area_code = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AreaCode"));
                }
            }
            get
            {
                return area_code;
            }
        }

        [JsonProperty("county")]
        public string county;
        public string County
        {
            set
            {
                if (county != value)
                {
                    county = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("County"));
                }
            }
            get
            {
                return county;
            }
        }

        [JsonProperty("country")]
        public string country;
        public string Country
        {
            set
            {
                if (country != value)
                {
                    country = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Country"));
                }
            }
            get
            {
                return country;
            }
        }

        public Location Location { get; set; }

        public Address()
        {

        }

        public object Clone()
        {
            return (User)this.MemberwiseClone();
        }
    }
}
