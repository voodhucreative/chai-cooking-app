using System;
using System.Collections.Generic;
using ChaiCooking.Components.Tiles;
using ChaiCooking.Helpers;
using ChaiCooking.Layouts.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Components.Lists
{
    public class TiledList
    {
        public ScrollView Content;
        StackLayout ListContainer;
        List<Tile> TileList;

        private int CurrentTile = 0;
        StackLayout Row;

        public int TilesPerRow { get; set; }

        public TiledList(int tilesPerRow)
        {
            TilesPerRow = tilesPerRow;

            // create a scrollable list view
            Content = new ScrollView 
            { 
                BackgroundColor = Color.Transparent
            };

            ListContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(Units.ScreenUnitS)
            };

            // create a list of tiles
            TileList = new List<Tile>();

            // create the tile layout
            CurrentTile = 0;

            Row = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center
            };

            Content.Content = ListContainer;
        }


        public void AddTile(Tile tile)
        {
            if (CurrentTile > TilesPerRow - 1)
            {
                CurrentTile = 0;

                ListContainer.Children.Add(Row);
                Row = new StackLayout // create a new row
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center
                };
            }
            CurrentTile++;
            Row.Children.Add(tile.Content);
            TileList.Add(tile);
        }

        public void AddRow(StackLayout listRow)
        {
            // add a row to the container
            this.ListContainer.Children.Add(listRow);
        }

        private void Update()
        {

        }
    }
}
