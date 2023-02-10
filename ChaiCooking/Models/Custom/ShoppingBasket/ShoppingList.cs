using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChaiCooking.Models.Custom.ShoppingBasket
{
    public class ShoppingList
    {
        [JsonProperty("list")]
        public CustomList customList { get; set; }

        public partial class CustomList
        {
            [JsonProperty("id")]
            string id { get; set; }
            [JsonProperty("name")]
            string name { get; set; }
            [JsonProperty("primary")]
            bool primary { get; set; }
        }

        [JsonProperty("content")]
        public Content content { get; set; }
        public partial class Content
        {
            [JsonProperty("items")]
            public List<Items> items { get; set; }

            [JsonProperty("combined_items")]
            public List<CombinedItems> combinedItems { get; set; }

            [JsonProperty("recipes")]
            public List<Recipe> recipes { get; set; }
        }

        public partial class Items
        {
            [JsonProperty("id")]
            public string id { get; set; }

            [JsonProperty("item")]
            public Item item { get; set; }

            [JsonProperty("checked")]
            public bool isChecked { get; set; }

            [JsonProperty("image_url")]
            public string image_url { get; set; }

            [JsonProperty("analysis")]
            public Analysis analysis { get; set; }

            [JsonProperty("recipe")]
            public ShoppingRecipe recipe { get; set; }

            [JsonProperty("combined")]
            public Combined combined { get; set; }

            [JsonProperty("created_time")]
            public string created_time { get; set; }

            [JsonProperty("updated_at")]
            public string updated_at { get; set; }

            [JsonProperty("matching_properties")]
            public MatchingProperties matchingProperties { get; set; }

            [JsonProperty("custom_product_url")]
            public string custom_product_url { get; set; }
        }

        public partial class Item
        {
            [JsonProperty("name")]
            public string name { get; set; }
            [JsonProperty("brand")]
            public string brand { get; set; }
            [JsonProperty("comment")]
            public string comment { get; set; }
            [JsonProperty("quantity")]
            public double quantity { get; set; }
            [JsonProperty("unit")]
            public string unit { get; set; }
        }

        public partial class Analysis
        {
            [JsonProperty("product")]
            public Product product { get; set; }
            [JsonProperty("category")]
            public Category category { get; set; }
            [JsonProperty("brand")]
            public Brand brand { get; set; }
        }

        public partial class Product
        {
            [JsonProperty("canonical_name")]
            public string canonical_name { get; set; }
            [JsonProperty("original_name")]
            public string original_name { get; set; }
        }

        public partial class Category
        {
            [JsonProperty("canonical_name")]
            public string canonical_name { get; set; }
        }

        public partial class Brand
        {
            [JsonProperty("canonical_name")]
            public string canonical_name { get; set; }
        }

        public partial class ShoppingRecipe
        {
            [JsonProperty("recipe_id")]
            public string recipe_id { get; set; }
            [JsonProperty("position")]
            public int position { get; set; }
        }

        public partial class Combined
        {
            [JsonProperty("combined_item_id")]
            public string combined_item_id { get; set; }
            [JsonProperty("quantity")]
            public double quantity { get; set; }
        }

        public partial class MatchingProperties
        {
            [JsonProperty("gtin")]
            public string gtin { get; set; }
            [JsonProperty("custom_product_id")]
            public string custom_product_id { get; set; }
        }

        public partial class CombinedItems
        {
            [JsonProperty("id")]
            public string id { get; set; }

            [JsonProperty("item")]
            public Item item { get; set; }

            [JsonProperty("checked")]
            public bool isChecked { get; set; }

            [JsonProperty("image_url")]
            public string image_url { get; set; }

            [JsonProperty("analysis")]
            public Analysis analysis { get; set; }

            [JsonProperty("created_time")]
            public string created_time { get; set; }

            [JsonProperty("updated_at")]
            public string updated_at { get; set; }

            [JsonProperty("matching_properties")]
            public MatchingProperties matchingProperties { get; set; }

            [JsonProperty("custom_product_url")]
            public string custom_product_url { get; set; }
        }
    }
}
