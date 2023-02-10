using System;
using ChaiCooking.Layouts.Custom.Tiles;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class SingleRecipe
    {
        StackLayout Content;
        
        LargeRecipeTile Recipe;

        public SingleRecipe()
        {
            Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 0
            };
        }

        public StackLayout GetContent()
        {
            return Content;
        }
    }
}
