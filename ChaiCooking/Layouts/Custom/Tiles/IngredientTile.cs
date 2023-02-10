using System;
using System.Collections.Generic;
using System.Linq;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Fields;
using ChaiCooking.Components.Fields.Custom;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Tiles
{
    public class IngredientTile : ActiveComponent
    {
        public Grid NameLabelContainer;
        StaticLabel NameLabel;
        Frame NameLabelBackground;


        //Entry AmountField;
        SimpleInputField AmountField;


        StaticImage RemoveIcon;

        StackLayout IngredientContainer;

        public Ingredient Ingredient { get; set; }
        CustomPicker UnitPicker;

        Recipe Recipe;

        bool IsMainIngredient;


        public IngredientTile(Recipe recipe, Ingredient ingredient)//string title, double amount, string units)
        {
            Recipe = recipe;
            IsMainIngredient = false;

            IngredientContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HeightRequest = 52,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING)
            };
            Ingredient = ingredient;

            NameLabelContainer = new Grid
            {
                WidthRequest = Units.ThirdScreenWidth,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand

            };

            NameLabelBackground = new Frame
            {
                BackgroundColor = Color.FromHex(Colors.CC_ORANGE),
                HeightRequest = 52,
                CornerRadius = 8,
                HasShadow = false
            };

            NameLabel = new StaticLabel(Ingredient.Name);
            NameLabel.Content.WidthRequest = Units.ThirdScreenWidth;
            NameLabel.CenterAlign();
            NameLabel.Content.TextColor = Color.White;
            NameLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            NameLabel.Content.FontSize = Units.FontSizeL;
            NameLabel.Content.MaxLines = 2;
            NameLabel.Content.LineBreakMode = LineBreakMode.TailTruncation;

            if (Ingredient.Name.Length > 16)
            {
                NameLabel.Content.FontSize = Units.FontSizeM;
            }
            NameLabel.Content.Padding = 2;

            NameLabelContainer.Children.Add(NameLabelBackground, 0, 0);
            NameLabelContainer.Children.Add(NameLabel.Content, 0, 0);

            /*AmountField = new Entry
            {
                Text = Ingredient.Amount.ToString(),
                HeightRequest = 52,
                WidthRequest = 80,
                Placeholder = "0.00",
                PlaceholderColor = Color.FromHex(Colors.CC_PALE_GREY),
                TextColor = Color.FromHex(Colors.CC_DARK_GREY),
                HorizontalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontFamily = Fonts.GetRegularAppFont(),
                FontSize = Units.FontSizeL,
                Keyboard = Keyboard.Numeric
            };*/

            AmountField = new SimpleInputField("0.00", Keyboard.Numeric);
            AmountField.TextEntry.Text = Ingredient.Amount.ToString();
            AmountField.TextEntry.HeightRequest = 52;
            AmountField.TextEntry.WidthRequest = 80;

            AmountField.TextEntry.PlaceholderColor = Color.FromHex(Colors.CC_PALE_GREY);
            AmountField.TextEntry.TextColor = Color.FromHex(Colors.CC_DARK_GREY);
            AmountField.TextEntry.HorizontalTextAlignment = TextAlignment.Center;
            AmountField.TextEntry.BackgroundColor = Color.White;
            AmountField.TextEntry.VerticalOptions = LayoutOptions.CenterAndExpand;
            AmountField.TextEntry.VerticalTextAlignment = TextAlignment.Center;
            AmountField.TextEntry.HorizontalOptions = LayoutOptions.CenterAndExpand;
            AmountField.TextEntry.FontFamily = Fonts.GetRegularAppFont();
            AmountField.TextEntry.FontSize = Units.FontSizeL;
            AmountField.TextEntry.Keyboard = Keyboard.Numeric;

            UnitPicker = new CustomPicker();

            int pickerIndex = 0;
            foreach (IngredientUnit ing in AppDataContent.IngredientUnits)
            {
                UnitPicker.Items.Add(ing.Name);

                if (Ingredient.Unit.Name == ing.Name)
                {
                    UnitPicker.SelectedIndex = pickerIndex;
                    UnitPicker.SelectedItem = UnitPicker.Items[pickerIndex];
                    Console.WriteLine("Found " + UnitPicker.SelectedItem + " at " + UnitPicker.SelectedIndex);
                    UnitPicker.Title = Ingredient.Unit.Name;
                }

                pickerIndex++;
            }

            //UnitPicker.SelectedItem = Ingredient.Unit.Name;

            UnitPicker.FontFamily = Fonts.GetRegularAppFont();
            UnitPicker.FontSize = Units.FontSizeL;
            UnitPicker.BackgroundColor = Color.White;
            UnitPicker.TextColor = Color.FromHex(Colors.CC_DARK_GREY);

            UnitPicker.HorizontalTextAlignment = TextAlignment.Center;
            UnitPicker.HeightRequest = 32;
            UnitPicker.WidthRequest = 80;

            UnitPicker.SelectedIndexChanged += (sender, args) =>
            {
                if (UnitPicker.SelectedIndex == -1)
                {
                    Console.WriteLine("Unpicked " + UnitPicker.Title);
                }
                else
                {
                    //Ingredient.Unit.Name = UnitPicker.Items[UnitPicker.SelectedIndex];

                    /*IngredientUnit newUnit = AppDataContent.GetUnitByName(UnitPicker.Items[UnitPicker.SelectedIndex]);

                    var foundUnit = AppDataContent.IngredientUnits.Find(x => x.Name == Ingredient.Unit.Name);
                    if (foundUnit != null)
                    {
                        Ingredient.Unit.ID = foundUnit.ID;
                        Ingredient.Unit.Abbreviation = foundUnit.Abbreviation;
                    }*/

                    Ingredient.Unit = AppDataContent.GetUnitByName(UnitPicker.Items[UnitPicker.SelectedIndex]);

                    Ingredient.Amount = AmountField.TextEntry.Text;
                    Console.WriteLine("Picked " + UnitPicker.Items[UnitPicker.SelectedIndex]);
                }
            };

            AmountField.TextEntry.TextChanged += AmountChanged;

            RemoveIcon = new StaticImage("trash.png", 24, null);
            RemoveIcon.Content.HeightRequest = 24;
            RemoveIcon.Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            RemoveIcon.Content.HorizontalOptions = LayoutOptions.EndAndExpand;



            RemoveIcon.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               if (AppSession.InfoModeOn)
                               {
                                   double x = Tools.Screen.GetScreenCoordinates(RemoveIcon.Content).X;
                                   double y = Tools.Screen.GetScreenCoordinates(RemoveIcon.Content).Y;
                                   //App.ShowInfoBubble(new Label { Text = "Tap the x button on an ingredient to remove it." }, (int)x, (int)y);
                                   App.ShowInfoBubble(new Paragraph("Remove Ingredient", "Tap the x button on an ingredient to remove it.", null).Content, (int)x, (int)y);

                               }
                               else
                               {

                                   if (IsMainIngredient)
                                   {
                                       App.ShowAlert("Cannot remove current main ingredients");
                                   }
                                   else
                                   {
                                       if (Ingredient.RecipeId > 0)
                                       {
                                           Console.WriteLine("Remove " + Ingredient.Name);
                                           // await DataManager.SaveUserRecipe(AppSession.CurrentUser, Recipe);
                                           if (await DataManager.DeleteUserRecipeIngredient(AppSession.CurrentUser, Recipe.chai.Id.ToString(), Ingredient))
                                           {
                                               await App.ShowRemoveRecipeEditorIngredient(Ingredient);
                                           }
                                       }
                                       else
                                       {
                                           //App.ShowAlert("Please save the recipe first.");
                                           if (await App.DeleteUnsavedIngredient(Ingredient))
                                           {
                                               await App.ShowRemoveRecipeEditorIngredient(Ingredient);
                                           }
                                           //if (await App.RemoveUnsavedIngredientFromEditor(this.Ingredient);
                                       }
                                   }
                               }

                           });
                       })
                   }
               );

            IngredientContainer.Children.Add(NameLabelContainer);
            IngredientContainer.Children.Add(AmountField.Content);

            IngredientContainer.Children.Add(UnitPicker);

            IngredientContainer.Children.Add(RemoveIcon.Content);

            if (Ingredient.RecipeId <= 0)
            {
                SetAsUnsavedIngredient();
            }

            Content.Children.Add(IngredientContainer);
        }

        public void SetAsUnsavedIngredient()
        {
            NameLabelBackground.BackgroundColor = Color.FromHex(Colors.CC_DARK_ORANGE);
            //RemoveIcon.Content.Opacity = 0.0f;
            //RemoveIcon.Content.IsEnabled = false;
            IsMainIngredient = false;
        }

        public void SetAsMainIngredient()
        {
            NameLabelBackground.BackgroundColor = Color.FromHex(Colors.CC_GREEN);
            RemoveIcon.Content.Opacity = 0.0f;
            RemoveIcon.Content.IsEnabled = false;
            IsMainIngredient = true;
        }

        public void SetAsRegularIngredient()
        {
            if (Ingredient.RecipeId <= 0)
            {
                SetAsUnsavedIngredient();
            }
            else
            {
                NameLabelBackground.BackgroundColor = Color.FromHex(Colors.CC_ORANGE);
                RemoveIcon.Content.Opacity = 1.0f;
                RemoveIcon.Content.IsEnabled = true;
                IsMainIngredient = false;
            }
        }

        private void AmountChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            Ingredient.Amount = entry.Text;
            Console.WriteLine("Amount changed to " + Ingredient.Amount);
        }
    }
}
