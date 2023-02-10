using System;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Models.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Layouts
{
    public class ItemLayout
    {
        public Grid Content;
        public StackLayout Container;

        public ActiveLabel Name;
        public ActiveLabel ShortDescription;
        public ActiveLabel LongDescription;
        public ActiveImage MainImage;


        public ItemLayout(Item item)
        {
            Content = new Grid();

            Container = new StackLayout { Orientation = StackOrientation.Vertical };

            if (Name != null)
            {
                Name = new ActiveLabel(item.Name, Units.FontSizeL, Color.Transparent, Color.White, null);
                Container.Children.Add(Name.Content);
            }

            if (item.ShortDescription != null)
            {
                if (item.ShortDescription.Length > 0)
                {
                    ShortDescription = new ActiveLabel(item.ShortDescription, Units.FontSizeL, Color.Transparent, Color.White, null);
                    Container.Children.Add(ShortDescription.Content);
                }
            }

            if (item.LongDescription != null)
            {
                if (item.LongDescription.Length > 0)
                {
                    LongDescription = new ActiveLabel(item.LongDescription, Units.FontSizeL, Color.Transparent, Color.White, null);
                    Container.Children.Add(LongDescription.Content);
                }
            }

            if (item.MainImage != null)
            {
                if (item.MainImage.Length > 0)
                {
                    //MainImage = new ActiveImage(item.MainImage, Units.HalfScreenWidth, Units.HalfScreenWidth, null, null);
                    MainImage = new ActiveImage(item.MainImage, 32, 32, null, null);
                    MainImage.Image.Aspect = Aspect.AspectFill;
                    Container.Children.Add(MainImage.Content);
                }
            }
            
            

            Content.Children.Add(Container);

        }
    }
}
