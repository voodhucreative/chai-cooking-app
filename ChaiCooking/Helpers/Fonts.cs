using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ChaiCooking.Helpers
{
    public static class Fonts
    {
        public enum FontName
        {
            MontserratRegular,
            MontserratBold,
            MontserratMedium,
            MuliBlack,
            MuliBlackItalic,
            MuliBold,
            MuliBoldItalic,
            MuliExtraBold,
            MuliExtraBoldItalic,
            MuliExtraLight,
            MuliExtraLightItalic,
            MuliItalic,
            MuliLight,
            MuliLightItalic,
            MuliRegular,
            MuliSemiBold,
            MuliSemiBoldItalic,
            PoppinsRegular,
            PoppinsBold,
            GothamBold
        }
        static Dictionary<FontName, string> fontDictionary;

        public static void Init()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                fontDictionary = new Dictionary<FontName, string>
                {
                    [FontName.MontserratRegular] = "Montserrat-Regular.ttf#Montserrat-Regular",
                    [FontName.MontserratBold] = "Montserrat-Bold.ttf#Montserrat-Bold",
                    [FontName.MontserratMedium] = "Montserrat-Medium.ttf#Montserrat-Medium",
                    [FontName.MuliBlack] = "Muli-Black.ttf#Muli-Black",
                    [FontName.MuliBlackItalic] = "Muli-BlackItalic.ttf#Muli-BlackItalic",
                    [FontName.MuliBold] = "Muli-Bold.ttf#Muli-Bold",
                    [FontName.MuliBoldItalic] = "Muli-BoldItalic.ttf#Muli-BoldItalic",
                    [FontName.MuliExtraBold] = "Muli-ExtraBold.ttf#Muli-ExtraBold",
                    [FontName.MuliExtraBoldItalic] = "Muli-ExtraBoldItalic.ttf#Muli-ExtraBoldItalic",
                    [FontName.MuliExtraLight] = "Muli-ExtraLight.ttf#Muli-ExtraLight",
                    [FontName.MuliExtraLightItalic] = "Muli-ExtraLightItalic.ttf#Muli-ExtraLightItalic",
                    [FontName.MuliItalic] = "Muli-Italic.ttf#Muli-Italic",
                    [FontName.MuliLight] = "Muli-Light.ttf#Muli-Light",
                    [FontName.MuliLightItalic] = "Muli-LightItalic.ttf#Muli-LightItalic",
                    [FontName.MuliRegular] = "Muli-Regular.ttf#Muli-Regular",
                    [FontName.MuliSemiBold] = "Muli-SemiBold.ttf#Muli-SemiBold",
                    [FontName.MuliSemiBoldItalic] = "Muli-SemiBoldItalic.ttf#Muli-SemiBoldItalic",
                    [FontName.PoppinsRegular] = "Poppins-Regular.otf#Poppins-Regular",
                    [FontName.PoppinsBold] = "Poppins-Bold.otf#Poppins-Bold",
                    [FontName.GothamBold] = "Gotham-Bold.otf#Gotham-Bold",

                };
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                fontDictionary = new Dictionary<FontName, string>
                {
                    [FontName.MontserratRegular] = "Muli-Regular",
                    [FontName.MontserratBold] = "Muli-Bold",
                    [FontName.MontserratMedium] = "Muli-Medium",
                    [FontName.MuliBlack] = "Muli-Black",
                    [FontName.MuliBlackItalic] = "Muli-BlackItalic",
                    [FontName.MuliBold] = "Muli-Bold",
                    [FontName.MuliBoldItalic] = "Muli-BoldItalic",
                    [FontName.MuliExtraBold] = "Muli-ExtraBold",
                    [FontName.MuliExtraBoldItalic] = "Muli-ExtraBoldItalic",
                    [FontName.MuliExtraLight] = "Muli-ExtraLight",
                    [FontName.MuliExtraLightItalic] = "Muli-ExtraLightItalic",
                    [FontName.MuliItalic] = "Muli-Italic",
                    [FontName.MuliLight] = "Muli-Light",
                    [FontName.MuliLightItalic] = "Muli-LightItalic",
                    [FontName.MuliRegular] = "Muli-Regular",
                    [FontName.MuliSemiBold] = "Muli-SemiBold",
                    [FontName.MuliSemiBoldItalic] = "Muli-SemiBoldItalic",
                    [FontName.PoppinsRegular] = "Poppins-Regular",
                    [FontName.PoppinsBold] = "Poppins-Bold",
                    [FontName.GothamBold] = "Gotham-Bold",
                };
            }
        }

        public static string GetFont(FontName font)
        {
            return fontDictionary[font];
        }

        public static string GetRegularAppFont()
        {
            return GetFont(FontName.PoppinsRegular);
        }

        public static string GetBoldAppFont()
        {
            return GetFont(FontName.PoppinsBold);
            //return GetFont(FontName.GothamBold);
        }


    }
}
