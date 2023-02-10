using System;
using System.Collections.Generic;
using System.Linq;
using FFImageLoading.Transformations;
using ChaiCooking.Components.Images;
using Xamarin.Forms;

namespace ChaiCooking.Tools
{
    public class ImageTools
    {

        public static List<Color> GetGradientSplit(Color start, Color end, int numberOfSplits)
        {
            List<Color> splits = new List<Color>();

            return splits;
        }

        public static Color GetMiddleColor(Color c1, Color c2)
        {

            var interval_R = (c2.R - c1.R) / 2;
            var interval_G = (c2.G - c1.G) / 2;
            var interval_B = (c2.B - c1.B) / 2;
            var current_R = c1.R + interval_R;
            var current_G = c1.G + interval_G;
            var current_B = c1.B + interval_B;

            return new Color(current_R, current_G, current_B);
            /*
            for (var i = 0; i <= 2; i++)
            {
                var color = Color.FromRgb(current_R, current_G, current_B);
                //increment.
                current_R += interval_R;
                current_G += interval_G;
                current_B += interval_B;
            }*/


            /*
                int numberOfIntervals = 10; //or change to whatever you want.
                var interval_R = (R2 - R1) / numberOfIntervals;
                var interval_G = (G2 - G1) / numberOfIntervals;
                var interval_B = (B2 - B1) / numberOfIntervals;

                var current_R = R1;
                var current_G = G1;
                var current_B = B1;

                for (var i = 0; i <= numberOfIntervals; i++)
                {
                    var color = Color.FromRGB(current_R, current_G, current_B);
                    //do something with color.

                    //increment.
                    current_R += interval_R;
                    current_G += interval_G;
                    current_B += interval_B;
                }
            */
            //return new Color(RGBdiff(c1, c2));
            //return new Color(((byte)(c1.R + c2.R) / (byte)2), ((byte)(c1.G + c2.G) / (byte)2), ((byte)(c1.B + c2.B) / (byte)2));




        }

        public static int RGBdiff(Color c1, Color c2)
        {
            return (int)(Math.Abs(c1.R - c2.R) + Math.Abs(c1.G - c2.G) + Math.Abs(c1.B - c2.B));
        }

        public static Color ClosestColor(Color target, IEnumerable<Color> colors)
        {
            return colors.Min(c => Tuple.Create(RGBdiff(c, target), c)).Item2;
        }

        public static StaticImage Tint(StaticImage untintedImage, string tintColor)
        {
            string colorVal = "#" + tintColor;

            StaticImage TintImage = untintedImage;
            TintImage.Content.Opacity = 1;
            TintImage.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
            TintImage.Content.Transformations.Add(new TintTransformation { HexColor = colorVal });
            return TintImage;
        }

        public static StaticImage Tint(StaticImage untintedImage, Color tintColor)
        {
            TintTransformation colorTint = new TintTransformation
            {
                HexColor = (string)tintColor.ToHex(),
                EnableSolidColor = true

            };

            StaticImage TintImage = untintedImage;
            TintImage.Content.Transformations = new List<FFImageLoading.Work.ITransformation>();
            TintImage.Content.Transformations.Add(colorTint);
            TintImage.Content.Opacity = 1;

            return TintImage;
        }

        public static ActiveImage Tint(ActiveImage untintedImage, string tintColor)
        {
            string colorVal = "#" + tintColor;

            ActiveImage TintImage = untintedImage;
            TintImage.Image.Opacity = 1;
            TintImage.Image.Transformations = new List<FFImageLoading.Work.ITransformation>();
            TintImage.Image.Transformations.Add(new TintTransformation { HexColor = colorVal });

            return TintImage;
        }

        public static ActiveImage Tint(ActiveImage untintedImage, Color tintColor)
        {
            TintTransformation colorTint = new TintTransformation
            {
                HexColor = (string)tintColor.ToHex(),
                EnableSolidColor = true

            };

            ActiveImage TintImage = untintedImage;
            TintImage.Image.Transformations = new List<FFImageLoading.Work.ITransformation>();
            TintImage.Image.Transformations.Add(colorTint);
            TintImage.Image.Opacity = 1;

            return TintImage;
        }

        public static ActiveSvgImage Tint(ActiveSvgImage untintedImage, string tintColor)
        {
            string colorVal = "#" + tintColor;

            ActiveSvgImage TintImage = untintedImage;
            TintImage.Image.Opacity = 1;
            TintImage.Image.Transformations = new List<FFImageLoading.Work.ITransformation>();
            TintImage.Image.Transformations.Add(new TintTransformation { HexColor = colorVal });

            return TintImage;
        }

        public static ActiveSvgImage Tint(ActiveSvgImage untintedImage, Color tintColor)
        {
            TintTransformation colorTint = new TintTransformation
            {
                HexColor = (string)tintColor.ToHex(),
                EnableSolidColor = true

            };

            ActiveSvgImage TintImage = untintedImage;
            TintImage.Image.Transformations = new List<FFImageLoading.Work.ITransformation>();
            TintImage.Image.Transformations.Add(colorTint);
            TintImage.Image.Opacity = 1;

            return TintImage;
        }
    }
}

