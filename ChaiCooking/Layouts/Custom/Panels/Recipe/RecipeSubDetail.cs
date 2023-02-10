using ChaiCooking.Helpers;
using ChaiCooking.Tools;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Panels.Recipe
{
    public class RecipeSubDetail
    {
        public StackLayout Content;

        public RecipeSubDetail(ChaiCooking.Models.Custom.Recipe recipe, string mealPeriod)
        {
            int fontSize = Units.FontSizeL;

            if (App.IsSmallScreen())
            {
                fontSize = Units.FontSizeM;
            }

            Label totalSubDetail = new Label
            {
                TextColor = Color.Gray,
                Text = EmptyCheck(recipe.CookingTime),
                FontSize = fontSize,
                FontAttributes = FontAttributes.Bold,
                LineBreakMode = LineBreakMode.WordWrap
            };

            Label prepSubDetail = new Label
            {
                TextColor = Color.Gray,
                Text = EmptyCheck(recipe.PrepTime),
                FontSize = fontSize,
                FontAttributes = FontAttributes.Bold,
                LineBreakMode = LineBreakMode.WordWrap
            };

            Label dishSubDetail = new Label
            {
                TextColor = Color.Gray,
                Text = TextTools.FirstCharToUpper(mealPeriod),
                FontSize = fontSize,
                FontAttributes = FontAttributes.Bold,
                LineBreakMode = LineBreakMode.WordWrap
            };

            StackLayout subDetailStack = new StackLayout // stack em high
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Spacing = 5,
                Children =
                {
                    dishSubDetail,
                    prepSubDetail,
                    totalSubDetail
                },
                MinimumWidthRequest = Units.ScreenWidth25Percent
            };

            if (dishSubDetail.Text.Length == 0)
            {
                dishSubDetail.Text = "-";
            }
            Content = subDetailStack;
        }

        public StackLayout GetContent()
        {
            return Content;
        }

        public string EmptyCheck(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "0";// mins";
            }
            else
            {
                return s;// '// + " mins";
            }
        }

    }
}
