using System;
using Xamarin.Forms;

namespace ChaiCooking.Helpers.Custom
{
    // static dimensions for elements
    // specific to each app
    // these units are not screen dependent, nor dynamic
    // so will remain the same across all devices!

    public static class Dimensions
    {
        public const int HEADER_HEIGHT = 52;
        public const int SUBHEADER_HEIGHT = 52;
        public const int NAVHEADER_HEIGHT = 32;
        public const int HEADER_LABEL_FONT_SIZE = 14;
        public const int TABBED_PANEL_HEADER_HEIGHT = 52;

        public const int MAIN_MENU_WIDTH = 240;

        public const int RIGHT_MENU_WIDTH = 120;

        public const int SLIDER_WIDTH = 240;
        public const int SLIDER_HEIGHT = 40;

        public const int MENU_SECTION_WIDTH = 180;
        public const int MENU_SECTION_HEIGHT = 80; // if scrollable, otherwise dymanic

        public const int SLIDE_ON_MENU_VERTICAL_OFFSET = 68;

        public const int SEARCH_INPUT_WIDTH = 240;
        public const int SEARCH_INPUT_HEIGHT = 40;
        public const int SEARCH_INPUT_ICON_WIDTH = 24;
        public const int SEARCH_INPUT_ICON_HEIGHT = 24;

        public const int PANEL_HEADER_HEIGHT = 24;

        public const int INFO_PANEL_WIDTH = 320;
        public const int INFO_PANEL_HEIGHT = 400;
        public const int PANEL_BORDER_WIDTH = 2;

        public const int GENERAL_COMPONENT_PADDING = 8;
        public const int GENERAL_COMPONENT_SPACING = 16;
        public const int HEADER_PADDING = 12;

        public const int AVATAR_SIZE = 140;

        public const int ICON_LABEL_WIDTH = 140;
        public const int ICON_LABEL_HEIGHT = 40;
        public const int ICON_LABEL_ICON_SIZE = 24;
        public const int ICON_LABEL_FONT_SIZE = 12;

        public const int HEADER_LOGO_WIDTH = 92;
        public const int HEADER_LOGO_HEIGHT = 48;

        public const int MENU_COLUMNS_TABLET = 4;
        public const int MENU_COLUMNS_PHONE = 2;

        public const int STANDARD_BUTTON_WIDTH = 160;
        public const int STANDARD_BUTTON_HEIGHT = 40;
        public const int STANDARD_BUTTON_FONT_SIZE = 12;

        public const int STANDARD_ICON_WIDTH = 24;
        public const int STANDARD_ICON_HEIGHT = 24;

        public const int ALBUM_TILE_WIDTH = 120;
        public const int ALBUM_TILE_HEIGHT = 120;

        public const int CHECKBOX_FONT_SIZE = 14;
        public const int CHECKBOX_ICON_SIZE = 20;
        public const int LARGE_CHECKBOX_ICON_SIZE = 30;

        public const int RECIPE_TILE_HEIGHT = 180;

        public const int HOME_PAGE_TILE_TEXT_PANEL_HEIGHT = 100;

        public const int RECIPE_LAYOUT_PADDING = 24;
        
        public static int GetNumberOfMenuColumns()
        {
            if (Device.Idiom == TargetIdiom.Tablet)
            {
                return MENU_COLUMNS_TABLET;
            }
            return MENU_COLUMNS_PHONE;
        }
        
    }
}
