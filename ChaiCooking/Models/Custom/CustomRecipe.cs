using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ChaiCooking.Models.Custom
{
    public partial class Recipe : ICloneable, INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        /*
        [JsonProperty("prep_time")]
        public string PrepTime { get; set; }

        [JsonProperty("cooking_time")]
        public string CookingTime { get; set; }

        [JsonProperty("total_time")]
        public string TotalTime { get; set; }
        */
        

        [JsonProperty("cooking_time")]
        public string cooking_time { get; set; }
        public string CookingTime
        {
            set
            {
                if (cooking_time != value)
                {
                    cooking_time = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CookingTime"));
                }
            }
            get
            {
                return cooking_time;
            }

        }

        [JsonProperty("prep_time")]
        public string prep_time { get; set; }
        public string PrepTime
        {
            set
            {
                if (prep_time != value)
                {
                    prep_time = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PrepTime"));
                }
            }
            get
            {
                return prep_time;
            }

        }

        [JsonProperty("total_time")]
        public string total_time { get; set; }
        public string TotalTime
        {
            set
            {
                if (total_time != value)
                {
                    total_time = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TotalTime"));
                }
            }
            get
            {
                return total_time;
            }

        }

        [JsonProperty("main_image_source")]
        public string MainImageSource { get; set; }

        [JsonProperty("meal_category_image_source")]
        public string MealCategoryImageSource { get; set; }

        [JsonProperty("is_favourite")]
        public bool IsFavourite { get; set; }

        [JsonProperty("favourite_rating")]
        public int FavouriteRating { get; set; }

        [JsonProperty("star_rating")]
        public int StarRating { get; set; }

        //[JsonProperty("type")]
        //public string Type { get; set; }
        [JsonProperty("meal_type")]
        public string MealType { get; set; }

        [JsonProperty("dish_type")]
        public string DishType { get; set; }

        [JsonProperty("cuisine_type")]
        public string CuisineType { get; set; }

        public User Creator { get; set; }

        public bool IsSelected { get; set; }

        public bool IsOnFavouritesList { get; set; }

        public bool isHighlighted { get; set; }

        public string Author { get; set; }

        public Ingredient MainIngredient { get; set; }

        public Recipe()
        {
            Console.WriteLine("New recipe created (default)");
        }

        
        public object Clone()
        {
            return (Recipe)this.MemberwiseClone();
        }
    }
}
