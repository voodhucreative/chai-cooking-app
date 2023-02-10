using ChaiCooking.Helpers;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Panels.Recipe
{
    public class RecipeDetail
    {
        public StackLayout Content;

        public RecipeDetail()
        {
            int fontSize = Units.FontSizeL;

            if (App.IsSmallScreen())
            {
                fontSize = Units.FontSizeM;
            }

            Label totalDetail = new Label
            {
                TextColor = Color.Black,
                Text = "Cooking Time",
                FontSize = fontSize,
                FontAttributes = FontAttributes.Bold,
                LineBreakMode = LineBreakMode.WordWrap
            };

            Label prepDetail = new Label
            {
                TextColor = Color.Black,
                Text = "Prep Time",
                FontSize = fontSize,
                FontAttributes = FontAttributes.Bold,
                LineBreakMode = LineBreakMode.WordWrap
            };

            Label dishDetail = new Label
            {
                TextColor = Color.Black,
                Text = "Dish Type",
                FontSize = fontSize,
                FontAttributes = FontAttributes.Bold,
                LineBreakMode = LineBreakMode.WordWrap
            };

            StackLayout mainDetailStack = new StackLayout // stack em high
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Spacing = 5,
                Children =
                {
                    dishDetail,
                    prepDetail,
                    totalDetail
                },
                MinimumWidthRequest = Units.ScreenWidth25Percent
            };

            Content = mainDetailStack;
        }

        public StackLayout GetContent()
        {
            return Content;
        }
    }
}
