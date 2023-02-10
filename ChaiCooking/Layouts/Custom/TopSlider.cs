using System;
using System.Collections.Generic;
using ChaiCooking.Components.Tiles;
using ChaiCooking.Helpers;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom
{
    public class TopSlider
    {
        public int NumberOfTiles;
        public int TileCount;
        public int TileWidth;
        public int TileHeight;

       //public Grid Content { get; set; }
        public StackLayout TileContainer { get; set; }
        public List<Tile> TileList;

        public ScrollView Content;

        public TopSlider()
        {
            TileCount = 0;

            TileList = new List<Tile>();

            Content = new ScrollView
            {
                Orientation = ScrollOrientation.Horizontal
            };

            TileContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            AddItem("pin_icon_medium.png", "Test 1", "https://www.google.com/");
            AddItem("pin_icon_medium.png", "Test 2", "https://www.google.com/");
            AddItem("pin_icon_medium.png", "Test 3", "https://www.google.com/");
            AddItem("pin_icon_medium.png", "Test 4", "https://www.google.com/");
            AddItem("pin_icon_medium.png", "Test 5", "https://www.google.com/");
            AddItem("pin_icon_medium.png", "Test 6", "https://www.google.com/");
            AddItem("pin_icon_medium.png", "Test 7", "https://www.google.com/");
            AddItem("pin_icon_medium.png", "Test 8", "https://www.google.com/");
            AddItem("pin_icon_medium.png", "Test 9", "https://www.google.com/");

           // Content.WidthRequest = TileList.Count * Units.ScreenWidth30Percent;
        }

        public void Clear()
        {
            TileContainer.Children.Clear();
        }

        public void AddItem(Grid itemContent)
        {
            Tile item = new Tile();
            item.Content.BackgroundColor = Color.White;
            item.AddContent(itemContent);
            TileContainer.Children.Add(item.Content);
            TileCount++;
            Content.Content = TileContainer;
        }

        public void AddItem(string logoImageSource, string name, string link)
        {
            Tile item = new Tile();
            item.Content.BackgroundColor = Color.White;
            item.AddContent(new TopSliderItemLayout(logoImageSource, name, link).Content);

            TileContainer.Children.Add(item.Content);

            TileCount++;

            Content.Content = TileContainer;


        }

        public void Update()
        {

        }
    }
}
