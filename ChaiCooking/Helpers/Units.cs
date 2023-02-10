using System;
namespace ChaiCooking.Helpers
{
    public static class Units
    {
        // device Screen dimensions - set upon app start up
        public static int ScreenWidth = 360;
        public static int ScreenHeight = 640;
        public static int PixelWidth = 360;
        public static int PixelHeight = 640;
        public static float Scale = 1.0f;

        // basic layout units
        public static int HalfScreenWidth = (int)(ScreenWidth / 2);
        public static int HalfScreenHeight = (int)(ScreenHeight / 2);

        public static int ThirdScreenWidth = (int)(ScreenWidth / 3);
        public static int ThirdScreenHeight = (int)(ScreenHeight / 3);

        public static int QuarterScreenWidth = (int)(ScreenWidth / 4);
        public static int QuarterScreenHeight = (int)(ScreenHeight / 4);

        // Screen units
        public static int ScreenUnitL = (int)(ScreenWidth / 12);
        public static int ScreenUnitM = (int)(ScreenWidth / 24);
        public static int ScreenUnitS = (int)(ScreenWidth / 36);
        public static int ScreenUnitXS = (int)(ScreenWidth / 48);

        // Screen percentages width
        public static int ScreenWidth2Percent = (int)(ScreenWidth * 0.02);
        public static int ScreenWidth5Percent = (int)(ScreenWidth * 0.05);
        public static int ScreenWidth10Percent = (int)(ScreenWidth * 0.1);
        public static int ScreenWidth12Percent = (int)(ScreenWidth * 0.12);
        public static int ScreenWidth15Percent = (int)(ScreenWidth * 0.15);
        public static int ScreenWidth20Percent = (int)(ScreenWidth * 0.20);
        public static int ScreenWidth25Percent = (int)(ScreenWidth * 0.25);
        public static int ScreenWidth30Percent = (int)(ScreenWidth * 0.30);
        public static int ScreenWidth35Percent = (int)(ScreenWidth * 0.35);
        public static int ScreenWidth40Percent = (int)(ScreenWidth * 0.40);
        public static int ScreenWidth45Percent = (int)(ScreenWidth * 0.45);

        // Screen percentages height
        public static int ScreenHeight2Percent = (int)(ScreenHeight * 0.02);
        public static int ScreenHeight5Percent = (int)(ScreenHeight * 0.05);
        public static int ScreenHeight10Percent = (int)(ScreenHeight * 0.1);
        public static int ScreenHeight12Percent = (int)(ScreenHeight * 0.12);
        public static int ScreenHeight15Percent = (int)(ScreenHeight * 0.15);
        public static int ScreenHeight20Percent = (int)(ScreenHeight * 0.20);
        public static int ScreenHeight25Percent = (int)(ScreenHeight * 0.25);
        public static int ScreenHeight30Percent = (int)(ScreenHeight * 0.30);
        public static int ScreenHeight35Percent = (int)(ScreenHeight * 0.35);
        public static int ScreenHeight40Percent = (int)(ScreenHeight * 0.40);
        public static int ScreenHeight45Percent = (int)(ScreenHeight * 0.45);

        // constant sizes
        public const int TapSizeXL = 80;
        public const int TapSizeL = 64;
        public const int TapSizeM = 52;
        public const int TapSizeS = 40;
        public const int TapSizeXS = 32;
        public const int TapSizeXXS = 24;

        public const int HeaderHeight = 64;
        public const int FooterHeight = 52;
        public const int TopNavHeight = (int)(HeaderHeight * 2);
        public const int TitleBarHeight = 40;
        public const int HomeWidth = 90;
        public const int BannerHeight = HeaderHeight * 3;

        public const int MarginXXL = 40;
        public const int MarginXL = 32;
        public const int MarginL = 24;
        public const int MarginM = 16;
        public const int MarginS = 8;
        public const int MarginXS = 4;
        public const int MarginXXS = 2;

        public const int StandardElementMarginHeight = 8;
        public const int LabelHeight = 24;
        public const int LabelWidth = 240;

        public const int FontSizeXXXL = 32;
        public const int FontSizeXXL = 24;
        public const int FontSizeXL = 20;
        public const int FontSizeL = 16;
        public const int FontSizeML = 14;
        public const int FontSizeM = 12;
        public const int FontSizeS = 10;
        public const int FontSizeXS = 8;


        public static int DynamicFontSizeXXXL = 32;
        public static int DynamicFontSizeXXL = 24;
        public static int DynamicFontSizeXL = 20;
        public static int DynamicFontSizeL = 16;
        public static int DynamicFontSizeM = 12;
        public static int DynamicFontSizeS = 10;
        public static int DynamicFontSizeXS = 8;

        public const int SmallButtonWidth = 100;
        public const int SmallButtonHeight = 28;

        public const int MediumButtonWidth = 180;
        public const int MediumButtonHeight = 36;

        public const int LargeButtonWidth = 256;
        public const int LargeButtonHeight = 40;

        public const int NavDividerHeight = 3;

        public static int InputTitleHeight = 26;
        public static int InputEntryHeight = 36;

        public static int InputHeight = InputTitleHeight + InputEntryHeight + (StandardElementMarginHeight * 2);

        // Transition times
        public static uint TransitionSnail = 1000;
        public static uint TransitionReallySlow = 750;
        public static uint TransitionSlow = 500;
        public static uint TransitionMedium = 250;
        public static uint TransitionFast = 125;
        public static uint TransitionVeryFast = 75;
        public static uint TransitionRocketSpeed = 50;
        public static uint TransitionLightSpeed = 25;

     
        public static int ContentHeight = ScreenHeight;

        public static int DeviceStatusBarHeight = 24;

        public static bool SmallScreen = false;

        public static void Init(int width, int height, int pixelWidth, int pixelHeight, float scale, int statusBarHeight)
        {
            PixelWidth = pixelWidth;
            PixelHeight = pixelHeight;
            Init(width, height, scale, statusBarHeight);
        }
        public static void Init(int width, int height, float scale, int statusBarHeight)
        {
            // device Screen dimensions - set upon app start up
            ScreenWidth = width;
            ScreenHeight = height;
            

            DeviceStatusBarHeight = statusBarHeight;
            Scale = 1;

            HalfScreenWidth = (int)(ScreenWidth / 2);
            HalfScreenHeight = (int)(ScreenHeight / 2);

            ThirdScreenWidth = (int)(ScreenWidth / 3);
            ThirdScreenHeight = (int)(ScreenHeight / 3);

            QuarterScreenWidth = (int)(ScreenWidth / 4);
            QuarterScreenHeight = (int)(ScreenHeight / 4);

            // Screen units
            ScreenUnitL = (int)(ScreenWidth / 12);
            ScreenUnitM = (int)(ScreenWidth / 24);
            ScreenUnitS = (int)(ScreenWidth / 36);
            ScreenUnitXS = (int)(ScreenWidth / 48);

            ScreenWidth2Percent = (int)(ScreenWidth * 0.02);
            ScreenWidth5Percent = (int)(ScreenWidth * 0.05);
            ScreenWidth10Percent = (int)(ScreenWidth * 0.1);
            ScreenWidth15Percent = (int)(ScreenWidth * 0.15);
            ScreenWidth20Percent = (int)(ScreenWidth * 0.20);
            ScreenWidth25Percent = (int)(ScreenWidth * 0.25);
            ScreenWidth30Percent = (int)(ScreenWidth * 0.30);
            ScreenWidth35Percent = (int)(ScreenWidth * 0.35);
            ScreenWidth40Percent = (int)(ScreenWidth * 0.40);
            ScreenWidth45Percent = (int)(ScreenWidth * 0.45);

            ScreenHeight2Percent = (int)(ScreenHeight * 0.02);
            ScreenHeight5Percent = (int)(ScreenHeight * 0.05);
            ScreenHeight10Percent = (int)(ScreenHeight * 0.1);
            ScreenHeight15Percent = (int)(ScreenHeight * 0.15);
            ScreenHeight20Percent = (int)(ScreenHeight * 0.20);
            ScreenHeight25Percent = (int)(ScreenHeight * 0.25);
            ScreenHeight30Percent = (int)(ScreenHeight * 0.30);
            ScreenHeight35Percent = (int)(ScreenHeight * 0.35);
            ScreenHeight40Percent = (int)(ScreenHeight * 0.40);
            ScreenHeight45Percent = (int)(ScreenHeight * 0.45);

            InputTitleHeight = (int)(ScreenHeight * 0.04);
            InputEntryHeight = (int)(ScreenHeight * 0.07);
            InputHeight = InputTitleHeight + InputEntryHeight + StandardElementMarginHeight;

            ContentHeight = ScreenHeight - (HeaderHeight + FooterHeight);


            DynamicFontSizeXXXL = FontSizeXXXL;
            DynamicFontSizeXXL = FontSizeXXL;
            DynamicFontSizeXL = FontSizeXL;
            DynamicFontSizeL = FontSizeL;
            DynamicFontSizeM = FontSizeM;
            DynamicFontSizeS = FontSizeS;
            DynamicFontSizeXS = FontSizeXS;

            SetPixelSize(width, height);
            SetScalableUnits(scale);

            Console.WriteLine("ScreenUnitL: " + ScreenUnitL);
            Console.WriteLine("ScreenUnitM: " + ScreenUnitM);
            Console.WriteLine("ScreenUnitS: " + ScreenUnitS);
            Console.WriteLine("ScreenUnitXS: " + ScreenUnitXS);


            Console.WriteLine("ScreenWidth: " + ScreenWidth);
            Console.WriteLine("ScreenHeight: " + ScreenHeight);
            Console.WriteLine("Scale: " + Scale);

            Console.WriteLine("HalfScreenWidth: " + HalfScreenWidth);
            Console.WriteLine("ThirdScreenWidth: " + ThirdScreenWidth);
            Console.WriteLine("QuarterScreenWidth: " + QuarterScreenWidth);

            SmallScreen = false;
            TransitionFast = 250;
        }

        public static void SetScalableUnits(float scale)
        {
            Scale = scale;

            DynamicFontSizeXXXL = (int)((ScreenWidth / 24) * Scale);
            DynamicFontSizeXXL = (int)((ScreenWidth / 30) * Scale);
            DynamicFontSizeXL = (int)((ScreenWidth / 38) * Scale);
            DynamicFontSizeL = (int)((ScreenWidth / 40) * Scale);
            DynamicFontSizeM = (int)((ScreenWidth / 54) * Scale);
            DynamicFontSizeS = (int)((ScreenWidth / 60) * Scale);
            DynamicFontSizeXS = (int)((ScreenWidth / 66) * Scale);

            Console.WriteLine("DynamicFontSizeXXXL: " + DynamicFontSizeXXXL);
            Console.WriteLine("DynamicFontSizeXXL: " + DynamicFontSizeXXL);
            Console.WriteLine("DynamicFontSizeXL: " + DynamicFontSizeXL);
            Console.WriteLine("DynamicFontSizeL: " + DynamicFontSizeL);
            Console.WriteLine("DynamicFontSizeM: " + DynamicFontSizeM);
            Console.WriteLine("DynamicFontSizeS: " + DynamicFontSizeS);
            Console.WriteLine("DynamicFontSizeXS: " + DynamicFontSizeXS);
        }

        public static void SetPixelSize(int pixelWidth, int pixelHeight)
        {
            PixelWidth = pixelWidth;
            PixelHeight = pixelHeight;

            SmallScreen = false;

            if (PixelWidth < 400)
            {
                SmallScreen = true;
            }

        }
        
    }
}
