//using System;
//using System.Collections.Generic;
//using ChaiCooking.AppData;
//using ChaiCooking.Branding;
//using ChaiCooking.Components.Composites;
//using ChaiCooking.Components.Fields.Custom;
//using ChaiCooking.Components.Labels;
//using ChaiCooking.Helpers;
//using ChaiCooking.Helpers.Custom;
//using Xamarin.Forms;

//namespace ChaiCooking.Layouts.Custom.Panels
//{
//    public class CreateMealPlanPanel : StandardLayout
//    {
//        StaticLabel Title;
//        StaticLabel CreatePlanInfo;

//        FormInputField ChooseNameField;

//        StaticLabel CreateLabel;
//        StaticLabel CancelLabel;


//        public CreateMealPlanPanel()
//        {
//            Content = new Grid { BackgroundColor = Color.FromHex(Colors.CC_DARK_BLUE_GREY), Padding = 2};

            

//            Container = new Grid
//            {
//                WidthRequest = Units.ScreenWidth,
//                HeightRequest = Units.ScreenHeight,
//                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
//                Padding = Dimensions.GENERAL_COMPONENT_PADDING
//            };

//            Container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
//            Container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
//            Container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
//            Container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
//            Container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });


//            Title = new StaticLabel("Create a Meal Plan");
//            Title.Content.TextColor = Color.White;
//            Title.Content.FontSize = Units.FontSizeXXL;
//            Title.LeftAlign();

//            CreatePlanInfo = new StaticLabel(AppText.CREATE_MEAL_PLAN_INFO_NO_EXISTING);
//            CreatePlanInfo.Content.TextColor = Color.White;
//            CreatePlanInfo.Content.FontSize = Units.ScreenUnitS;

//            if (AppSession.CurrentUser.MealPlans.Count > 0)
//            {
//                CreatePlanInfo.Content.Text = AppText.CREATE_MEAL_PLAN_INFO_WITH_EXISTING;
//            }

//            ChooseNameField = new FormInputField("Choose a name for your Name Plan", "add name", Keyboard.Text, false);
//            ChooseNameField.SetTitleColor(Color.White);
//            ChooseNameField.CenterAlign();
//            ChooseNameField.Content.WidthRequest = Units.ScreenWidth;
            
//            Grid OptionsContainer = new Grid { BackgroundColor = Color.Transparent, /*HeightRequest = 80,*/ Padding = Dimensions.GENERAL_COMPONENT_PADDING };
//            OptionsContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
//            OptionsContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
//            OptionsContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });


//            StaticLabel StartDateLabel = new StaticLabel("Start Date");
//            StartDateLabel.Content.TextColor = Color.White;
//            StartDateLabel.Content.FontFamily = Fonts.GetBoldAppFont();

//            // start date picker
//            ValueAdjuster DurationAdjuster = new ValueAdjuster("Mon 24th Sept", Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
//            DurationAdjuster.SetTitleTop();
//            DurationAdjuster.CenterAlign();
//            DurationAdjuster.Title.Content.TextColor = Color.White;
//            DurationAdjuster.Title.Content.FontFamily = Fonts.GetBoldAppFont();

//            DurationAdjuster.ValueLabel.Content.TextColor = Color.Black;
//            DurationAdjuster.ValueLabel.Content.FontFamily = Fonts.GetBoldAppFont();

//            DurationAdjuster.SetIntValues(new List<KeyValuePair<string, int>>()
//            {
//                new KeyValuePair<string, int>("1WK ", 1),
//                new KeyValuePair<string, int>("2WKS", 2),
//                new KeyValuePair<string, int>("3WKS", 3),
//                new KeyValuePair<string, int>("4WKS", 4),
//            });

//            // make this a component and use for ratings selector too...
//            StaticLabel NumberOfWeeksLabel = new StaticLabel("Number of weeks");
//            NumberOfWeeksLabel.Content.TextColor = Color.White;
//            NumberOfWeeksLabel.Content.FontFamily = Fonts.GetBoldAppFont();

//            SelectLabel NumWeeksOne = new SelectLabel("1", Color.FromHex(Colors.CC_ORANGE), Color.White, 24, 24, false, true);
//            NumWeeksOne.Title.Content.FontFamily = Fonts.GetBoldAppFont();
//            NumWeeksOne.CenterAlign();

//            SelectLabel NumWeeksTwo = new SelectLabel("2", Color.FromHex(Colors.CC_ORANGE), Color.White, 24, 24, false, true);
//            NumWeeksTwo.Title.Content.FontFamily = Fonts.GetBoldAppFont();
//            NumWeeksTwo.CenterAlign();

//            SelectLabel NumWeeksThree = new SelectLabel("3", Color.FromHex(Colors.CC_ORANGE), Color.White, 24, 24, true, true);
//            NumWeeksThree.Title.Content.FontFamily = Fonts.GetBoldAppFont();
//            NumWeeksThree.CenterAlign();

//            SelectLabel NumWeeksFour = new SelectLabel("4", Color.FromHex(Colors.CC_ORANGE), Color.White, 24, 24, false, true);
//            NumWeeksFour.Title.Content.FontFamily = Fonts.GetBoldAppFont();
//            NumWeeksFour.CenterAlign();

//            StackLayout NumWeeksContainer = new StackLayout { Orientation = StackOrientation.Horizontal };

//            NumWeeksContainer.Children.Add(NumWeeksOne.Content);
//            NumWeeksContainer.Children.Add(NumWeeksTwo.Content);
//            NumWeeksContainer.Children.Add(NumWeeksThree.Content);
//            NumWeeksContainer.Children.Add(NumWeeksFour.Content);

//            // number of weeks selector

//            OptionsContainer.Children.Add(StartDateLabel.Content, 0, 0);
//            OptionsContainer.Children.Add(DurationAdjuster.GetContent(), 1, 0);

//            OptionsContainer.Children.Add(NumberOfWeeksLabel.Content, 0, 2);
//            OptionsContainer.Children.Add(NumWeeksContainer, 1, 2);

//            CancelLabel = new StaticLabel("Cancel");
//            CancelLabel.Content.TextColor = Color.White;
//            CancelLabel.Content.FontSize = Units.FontSizeM;
//            CancelLabel.LeftAlign();

//            CreateLabel = new StaticLabel("Create");
//            CreateLabel.Content.TextColor = Color.White;
//            CreateLabel.Content.FontSize = Units.FontSizeM;
//            CreateLabel.RightAlign();


//            Container.Children.Add(Title.Content, 0, 0);
//            Container.Children.Add(CreatePlanInfo.Content, 0, 1);
            
//            Container.Children.Add(ChooseNameField.Content, 0, 2);
            
//            Container.Children.Add(OptionsContainer, 0, 3);


//            Container.Children.Add(CancelLabel.Content, 0, 4);
//            Container.Children.Add(CreateLabel.Content, 1, 4);


//            CancelLabel.Content.GestureRecognizers.Add(
//                    new TapGestureRecognizer()
//                    {
//                        Command = new Command(() =>
//                        {
//                            Device.BeginInvokeOnMainThread(async () =>
//                            {
//                                await App.PerformActionAsync(new Models.Action((int)Actions.ActionName.HideCreateMealPlan));
//                            });
//                        })
//                    }
//                );


//            CreateLabel.Content.GestureRecognizers.Add(
//                    new TapGestureRecognizer()
//                    {
//                        Command = new Command(() =>
//                        {
//                            Device.BeginInvokeOnMainThread(async () =>
//                            {
//                                // perform validation and add the new mealplan here
//                                AppSession.CurrentUser.MealPlans.Add(new Models.Custom.MealPlan
//                                {
//                                    Name = ChooseNameField.TextEntry.Text
//                                });



//                                CreatePlanInfo.Content.Text = AppText.CREATE_MEAL_PLAN_INFO_WITH_EXISTING;

//                                App.ShowAlert(ChooseNameField.TextEntry.Text + " created successfully!");
//                                await App.PerformActionAsync(new Models.Action((int)Actions.ActionName.HideCreateMealPlan));
//                            });
//                        })
//                    }
//                );


//            Grid.SetColumnSpan(Title.Content, 2);
//            Grid.SetColumnSpan(CreatePlanInfo.Content, 2);
//            Grid.SetColumnSpan(ChooseNameField.Content, 2);
//            Grid.SetColumnSpan(OptionsContainer, 2);



//            Content.Children.Add(Container);
//        }
//    }
//}
