using System;
using System.Threading.Tasks;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Lists
{
    public class AvailableIngredientsListLayout
    {
        public static StackLayout Content;

        Grid AvailablesContainer;
        Grid IngredientsListContainer;
        Grid Seperator;

        CustomEntry AvailableInput;

        public AvailableIngredientsListLayout()
        {
            Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.Transparent,
                Spacing = Dimensions.GENERAL_COMPONENT_SPACING
            };

            AvailablesContainer = new Grid { };
            AvailablesContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            AvailablesContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            AvailablesContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            AvailablesContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            AvailablesContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            AvailablesContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            AvailablesContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            AvailablesContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            IngredientsListContainer = new Grid { };

            AvailableInput = new CustomEntry();
            AvailableInput.WidthRequest = Units.ScreenWidth * 0.6;
            AvailableInput.Placeholder = "add an ingredient";
            AvailableInput.HorizontalOptions = LayoutOptions.StartAndExpand;

            IconButton addButton = new IconButton(52, 32, Color.FromHex(Colors.CC_ORANGE), Color.White, "", "plus.png", null);
            addButton.SetContentCenter();
            addButton.ContentContainer.Padding = 4;
            addButton.SetIconSize(32, 32);

            addButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               if (AppSession.InfoModeOn)
                               {
                                   double x = Tools.Screen.GetScreenCoordinates(addButton.Content).X;
                                   double y = Tools.Screen.GetScreenCoordinates(addButton.Content).Y;
                                   App.ShowInfoBubble(new Paragraph("Add Ingredient", "Tap the + button, when you’ve typed in one or multiple ingredients, to add them to the list of filters on this page.The recipe finder will update appropriately.", null).Content, (int)x, (int)y);

                               }
                               else
                               {
                                   await AddAvailableIngredient();
                               }
                           });
                       })
                   }
               );

            StackLayout addAvailableSection = new StackLayout { Orientation = StackOrientation.Horizontal };
            addAvailableSection.Children.Add(AvailableInput);
            addAvailableSection.Children.Add(addButton.Content);

            

            StaticLabel AddIngredientsLabel = new StaticLabel("Add your available ingredients.");
            AddIngredientsLabel.Content.TextColor = Color.White;
            AddIngredientsLabel.Content.FontSize = Units.FontSizeM;
            AddIngredientsLabel.Content.FontFamily = Fonts.GetRegularAppFont();
            AddIngredientsLabel.LeftAlign();

            AddIngredientsLabel.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               if (AppSession.InfoModeOn)
                               {
                                   double x = Units.ThirdScreenWidth;
                                   double y = Tools.Screen.GetScreenCoordinates(AddIngredientsLabel.Content).Y;
                                   //App.ShowInfoBubble(new Label { Text = "Type your ingredients here. You can add multiple ingredients at the same time if you seperate each one with a comma." }, (int)x, (int)y);
                                   App.ShowInfoBubble(new Paragraph("Add Ingredient", "Tap the + button, when you’ve typed in one or multiple ingredients, to add them to the list of filters on this page.The recipe finder will update appropriately.", null).Content, (int)x, (int)y);


                               }
                           });
                       })
                   }
               );

            StaticLabel YourIngredientsLabel = new StaticLabel("Your ingredients.");
            YourIngredientsLabel.Content.TextColor = Color.White;
            YourIngredientsLabel.Content.FontSize = Units.FontSizeM;
            YourIngredientsLabel.Content.FontFamily = Fonts.GetRegularAppFont();
            YourIngredientsLabel.LeftAlign();

            StaticLabel YourMatchesLabel = new StaticLabel("Your recipe matches.");
            YourMatchesLabel.Content.TextColor = Color.White;
            YourMatchesLabel.Content.FontSize = Units.FontSizeM;
            YourMatchesLabel.Content.FontFamily = Fonts.GetRegularAppFont();
            YourMatchesLabel.LeftAlign();

            Seperator = new Grid { WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) };

            UpdateAvailableIngredients();

            AvailablesContainer.Children.Add(AddIngredientsLabel.Content, 0, 0);
            AvailablesContainer.Children.Add(addAvailableSection, 0, 1);
            AvailablesContainer.Children.Add(YourIngredientsLabel.Content, 0, 2);
            AvailablesContainer.Children.Add(IngredientsListContainer, 0, 3);
            AvailablesContainer.Children.Add(Seperator, 0, 4);
            AvailablesContainer.Children.Add(YourMatchesLabel.Content, 0, 5);
            AvailablesContainer.Children.Add(YourMatchesLabel.Content, 0, 6);

            Content.Children.Add(AvailablesContainer); 
        }

        public void UpdateAvailableIngredients()
        {
            // filter list based on current available ingredients
            DataManager.FilterRecipesByIngredients(AppDataContent.AvailableIngredients, AppDataContent.AvoidedIngredients);

            if (IngredientsListContainer == null)
            {
                IngredientsListContainer = new Grid { };
            }
            IngredientsListContainer.Children.Clear();

            int row = 0;
            int col = 0;
            int itemCount = 0;

            foreach (Ingredient ingredient in AppDataContent.AvailableIngredients)
            {
                RemovableLabel removeLabel = new RemovableLabel(Color.FromHex(Colors.CC_ORANGE), Color.White, ingredient.Name, null);
                IngredientsListContainer.Children.Add(removeLabel.Content, col, row);

                removeLabel.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               await RemoveAvailableIngredient(ingredient);
                           });
                       })
                   }
               );

                col++;
                if (col >= Dimensions.GetNumberOfMenuColumns()) { col = 0; row++; };

                itemCount++;
            }

            //App.UDataManager.GetWasteLessRecipes(AppSession.CurrentUser, false)
            //AppSession.CurrentUser.Preferences.

            AppSession.WasteLessRecipes = DataManager.GetWasteLessRecipes(AppSession.CurrentUser, true);

            Device.BeginInvokeOnMainThread(async () =>
            {
                await App.UpdatePage((int)AppSettings.PageNames.WasteLess);
            });

            //App.UpdateRecipeList(); // if we use data binding here, this should updte automatically...
        }


        private async Task<bool> RemoveAvailableIngredient(Ingredient ingredientToRemove)
        {
            await Task.Delay(50);
            AppDataContent.AvailableIngredients.Remove(ingredientToRemove);
            UpdateAvailableIngredients();
            await App.UpdatePage((int)AppSettings.PageNames.WasteLess);
            return true;
        }

        private async Task<bool> AddAvailableIngredient()
        {
            if (AvailableInput.Text.Length > 0)
            {
                await Task.Delay(50);
                AppDataContent.AvailableIngredients.Add(new Ingredient { Id = 0, Name = AvailableInput.Text, ShortDescription = "", LongDescription = "", MainImage = "" });
                UpdateAvailableIngredients();
                return true;
            }
            App.ShowAlert("Enter a valid ingredient");

           

            return false;
        }


        public StackLayout GetContent()
        {
            return Content;
        }
    }
}
