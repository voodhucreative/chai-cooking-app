//using System;
//using System.Collections.Generic;
//using Newtonsoft.Json;

//namespace ChaiCooking.Models.Custom
//{
//    public class MealPlan : ICloneable
//    {
//        [JsonProperty("id")]
//        public string Id { get; set; }

//        [JsonProperty("name")]
//        public string Name { get; set; }

//        [JsonProperty("description")]
//        public string Description { get; set; }

//        public List<Recipe> recipe;


//        public MealPlan()
//        {
//            Console.WriteLine("New meal plan created (default)");

//            recipe = new List<Recipe>();

//        }

//        public object Clone()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
