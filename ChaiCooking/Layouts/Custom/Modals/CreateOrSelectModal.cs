using System;
using System.Linq;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom.Lists;
using ChaiCooking.Models.Custom.MealPlanAPI;
using ChaiCooking.Views.CollectionViews.CreateOrSelect;
using ChaiCooking.Views.CollectionViews.MealPlanner;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Modals
{
    public class CreateOrSelectModal : StandardLayout
    {
        int skipID;
        string mealPlanTitle = "";
        CollectionView createOrSelectLayout;
        CreateOrSelectCollectionView CreateOrSelectCollectionView;
        StackLayout otherMealsCont;
        StaticLabel switchLabel;//, otherMealsLabel;
        bool isMealPlan, isBusy;


        ActiveImage InfoIcon;

        public CreateOrSelectModal()
        {
            Container = new Grid { VerticalOptions = LayoutOptions.CenterAndExpand }; Content = new Grid { VerticalOptions = LayoutOptions.CenterAndExpand };
            isMealPlan = true;
            Container.Children.Add(BuildModal());
            Content.Children.Add(Container);
            isBusy = false;
        }

        public StackLayout BuildModal()
        {
            StaticLabel closeLabel = new StaticLabel(AppText.CLOSE);
            closeLabel.Content.TextColor = Color.White;
            closeLabel.Content.FontSize = Units.FontSizeL;
            closeLabel.Content.FontFamily = Fonts.GetRegularAppFont();
            closeLabel.RightAlign();

            InfoIcon = new ActiveImage("infoicon.png", Dimensions.STANDARD_ICON_WIDTH, Dimensions.STANDARD_ICON_HEIGHT, null, null);
            InfoIcon.Image.HorizontalOptions = LayoutOptions.EndAndExpand;
            InfoIcon.Image.VerticalOptions = LayoutOptions.CenterAndExpand;
            InfoIcon.Content.Margin = Dimensions.GENERAL_COMPONENT_PADDING;

            TouchEffect.SetNativeAnimation(InfoIcon.Content, true);
            TouchEffect.SetCommand(InfoIcon.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        AppSession.InfoModeOn = !AppSession.InfoModeOn;
                        if (AppSession.InfoModeOn)
                        {
                            InfoIcon.Image.Source = "infoiconon.png";
                        }
                        else
                        {
                            InfoIcon.Image.Source = "infoicon.png";
                        }
                    });
                }));

            TouchEffect.SetNativeAnimation(closeLabel.Content, true);
            TouchEffect.SetCommand(closeLabel.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        AppSession.InfoModeOn = false;
                        await App.HideModalAsync();
                    });
                }));

            StackLayout selectOrCreateCont = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    {new Label{Text = "Meal Plans",
                    FontSize = Units.FontSizeXXL,
                    TextColor = Color.White,}}
                }
            };

            Switch creationType = new Switch()
            {
                IsToggled = false,
                OnColor = Color.Orange,
                ThumbColor = Color.White,

            };
            creationType.IsToggled = ((isMealPlan) ? false : true);

            creationType.Toggled += (sender, e) =>
            {
                OnToggled(sender, e);
            };

            switchLabel = new StaticLabel("");
            switchLabel.Content.FontSize = Units.FontSizeXL;
            switchLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            switchLabel.Content.TextColor = Color.White;
            switchLabel.Content.HorizontalTextAlignment = TextAlignment.Center;

            switchLabel.Content.Text = ((isMealPlan) ? AppText.PRIVATE_MEAL_PLAN : AppText.PUBLIC_MEAL_PLAN);

            StackLayout switchContainer = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    switchLabel.Content,
                    creationType
                }
            };

            StackLayout currentDescCont = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    {
                        new Label
                        {
                            Text = "Selected Meal Plan: ",
                            FontSize = Units.FontSizeL,
                            TextColor = Color.White,
                            FontFamily = Fonts.GetRegularAppFont()
                        }
                    }
                }
            };

            if (AppSession.CurrentUser.defaultMealPlanID != -1)
            {
                try
                {
                    int result = AppSession.CurrentUser.defaultMealPlanID;
                    skipID = result;
                    mealPlanTitle = StaticData.userMealPlans.Data.Find(x => x.id == result).name;
                    mealPlanTitle = AppSession.CurrentUser.defaultMealPlanName;
                    //StaticData.userMealPlans.Data.Remove(StaticData.userMealPlans.Data.Find(x => x.id == result));
                }
                catch
                {

                    //mealPlanTitle = StaticData.userMealPlans.Data[0].name;
                    //skipID = StaticData.userMealPlans.Data[0].id;
                    //StaticData.userMealPlans.Data.Remove(StaticData.userMealPlans.Data[0]);
                }
            }
            else
            {
                mealPlanTitle = StaticData.userMealPlans.Data[0].name;
                skipID = StaticData.userMealPlans.Data[0].id;
                //StaticData.userMealPlans.Data.Remove(StaticData.userMealPlans.Data[0]);
            }

            StackLayout selectedMealPlanCont = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    {
                        new Label
                        {
                            Text = AppSession.CurrentUser.defaultMealPlanName,
                            FontSize = Units.FontSizeXL,
                            TextColor = Color.FromHex(Colors.CC_ORANGE),
                        }
                    }
                }
            };

            StackLayout otherMealPlansDescCont = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            /*
            string otherMealPlanDescText = ((isMealPlan) ? "Other Meal Plans you've created:" : "Other Meal Templates you've created:");
            otherMealsLabel = new StaticLabel(otherMealPlanDescText);
            otherMealsLabel.Content.FontSize = Units.FontSizeL;
            otherMealsLabel.Content.TextColor = Color.White;
            otherMealsLabel.Content.FontFamily = Fonts.GetRegularAppFont();

            otherMealPlansDescCont.Children.Add(otherMealsLabel.Content);
            */
            otherMealsCont = new StackLayout()
            {
                HeightRequest = 300,
            };

            if (isMealPlan)
            {
                otherMealsCont.Children.Add(BuildCollectionView(CallBuildConfirm));
            }
            else
            {
                otherMealsCont.Children.Add(BuildTemplatesView(CallBuildConfirm));
            }

            StackLayout upArrowCont = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
                        Text ="▲",
                        TextColor = Color.DarkGray,
                        FontSize = Units.FontSizeL
                    }
                }
            };

            StackLayout downArrowCont = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
                        Text ="▼",
                        TextColor = Color.DarkGray,
                        FontSize = Units.FontSizeL
                    }
                }
            };

            StackLayout otherMealPlansCont = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = Units.ScreenHeight40Percent,
                Children =
                {
                    upArrowCont,
                    otherMealsCont,
                    downArrowCont
                }
            };

            ColourButton createNewBtn = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Create New", null);
            createNewBtn.Content.Padding = 2;
            createNewBtn.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            createNewBtn.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;

            StackLayout createBtnCont = new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH,
                HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT,
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    createNewBtn.Content
                }
            };

            TouchEffect.SetNativeAnimation(createNewBtn.Content, true);
            TouchEffect.SetCommand(createNewBtn.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (AppSession.InfoModeOn)
                    {
                        App.ShowInfoBubble(new Paragraph("Create New", "Tap Create New if you decide that you’d rather make your own plan and auto populate it using CHAI recipe filters.You can adjust it afterwards using the Meal Plan editor.", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
                    }
                    else
                    {
                        StaticData.isCreating = true;
                        UserMealTemplate userMealTemplate = new UserMealTemplate();
                        UserMealTemplate.Datum datum = new UserMealTemplate.Datum();
                        datum.date = "EMPTY";
                        AppSession.mealPlannerCalendar.Data.Clear();
                        AppSession.mealPlannerCalendar.Data.Add(datum);
                        var mealPlannerGroup = new MealPlannerCollectionViewSection(AppSession.mealPlannerCalendar.Data);
                        //Collection View crash
                        AppSession.mealPlannerCollection.Add(mealPlannerGroup);
                        AppSession.mealPlannerCollection.RemoveAt(0);
                        AppSession.SetMealPlanner("", false, 1, false);
                        await App.HideSelectOrCreateModal();
                    }
                });
            }));

            Grid btnsContainer = new Grid
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT,
                WidthRequest = Units.ScreenWidth,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                RowDefinitions =
                {
                    { new RowDefinition { Height = new GridLength(Dimensions.STANDARD_BUTTON_HEIGHT) } },
                },
                Children =
                {
                    { createBtnCont, 0, 0 }
                }
            };

            StackLayout modalContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Spacing = Dimensions.GENERAL_COMPONENT_SPACING,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                WidthRequest = Units.ScreenWidth - Units.ScreenWidth10Percent,
                Children =
                {
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        VerticalOptions = LayoutOptions.Center,
                        Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                        Children = {
                            InfoIcon.Content,
                            closeLabel.Content
                        }
                    },

                    
                    selectOrCreateCont,
                    switchContainer,
                    
                    
                    currentDescCont,
                    selectedMealPlanCont,
                    otherMealPlansDescCont,
                    otherMealPlansCont,
                    btnsContainer
                }
            };

            return modalContainer;
        }

        public StackLayout BuildConfirmation(bool isMealPlan)
        {
            string type = "";

            type = ((isMealPlan) ? "Meal Plan" : "Template");

            StackLayout confirmTitle = new StackLayout
            {
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    {new Label{Text = $"Remove {type}",
                    FontSize = Units.FontSizeXXXL,
                    TextColor = Color.White,}}
                }
            };

            StackLayout currentDescCont = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    {new Label{Text = $"Would you like to remove the chosen {type}:",
                    FontSize = Units.FontSizeL,
                    TextColor = Color.White,
                    FontAttributes = FontAttributes.Bold}}
                }
            };

            string searchedName = "";
            if (isMealPlan)
            {
                searchedName = StaticData.userMealPlans.Data.FirstOrDefault(x => x.id == StaticData.mealPlanToDelete).name;
            }
            else
            {
                searchedName = StaticData.mealTemplates.Data.FirstOrDefault(x => x.id == StaticData.templateToDelete).name;
            }

            StackLayout nameCont = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    {new Label{Text = searchedName,
                    FontSize = Units.FontSizeXL,
                    TextColor = Color.Orange,
                    FontAttributes = FontAttributes.Bold}}
                }
            };

            ColourButton cancelBtn = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Cancel", null);
            cancelBtn.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            cancelBtn.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            cancelBtn.Content.Padding = 2;

            TouchEffect.SetNativeAnimation(cancelBtn.Content, true);
            TouchEffect.SetCommand(cancelBtn.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    Container.Children.Clear();
                    Content.Children.Clear();
                    Container.Children.Add(BuildModal());
                    Content.Children.Add(Container);
                });
            }));

            ColourButton confirmBtn = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Confirm", null);
            confirmBtn.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            confirmBtn.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            confirmBtn.Content.Padding = 2;

            TouchEffect.SetNativeAnimation(confirmBtn.Content, true);
            TouchEffect.SetCommand(confirmBtn.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    DeleteItem(isMealPlan);
                });
            }));


            StackLayout btnsContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = Units.ScreenWidth,
                Children =
                {
                    cancelBtn.Content,
                    confirmBtn.Content
                }
            };

            StackLayout modalContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                //WidthRequest = Units.ScreenWidth - Units.ScreenWidth10Percent,
                Children =
                {
                    confirmTitle,
                    currentDescCont,
                    nameCont,
                    btnsContainer
                }
            };

            return modalContainer;
        }

        public void CallBuildConfirm()
        {
            Container.Children.Clear();
            Content.Children.Clear();
            Container.Children.Add(BuildConfirmation(isMealPlan));
            Content.Children.Add(Container);
        }

        public async void CloseAndBuild()
        {
            await App.HideSelectOrCreateModal();
            AppSession.mealPlannerCalendar.Data.Clear();
            var mealPlannerGroup = new MealPlannerCollectionViewSection(AppSession.mealPlannerCalendar.Data);
            AppSession.mealPlannerCollection.RemoveAt(0);
            AppSession.mealPlannerCollection.Add(mealPlannerGroup);
        }

        public async void DeleteItem(bool isMealPlan)
        {
            if (isMealPlan)
            {
                var result = await App.ApiBridge.DeleteUserMealPlan(AppSession.CurrentUser, StaticData.mealPlanToDelete.ToString());
                if (result)
                {
                    App.ShowAlert("Meal Plan Deleted.");
                    var item = StaticData.userMealPlans.Data.Find(x => x.id == StaticData.mealPlanToDelete);
                    StaticData.userMealPlans.Data.Remove(item);
                    Container.Children.Clear();
                    Content.Children.Clear();
                    Container.Children.Add(BuildModal());
                    Content.Children.Add(Container);
                }
                else
                {
                    App.ShowAlert("Failed to delete meal plan.");
                }
            }
            else
            {
                var a = App.ApiBridge.DeleteUserMealPlanTemplates(AppSession.CurrentUser, StaticData.templateToDelete).Result;
                if (a)
                {
                    App.ShowAlert("Meal Plan Deleted.");
                    var result = App.ApiBridge.GetUserMealPlanTemplates(AppSession.CurrentUser).Result;
                    var createOrSelectTemplateGroup = new CreateOrSelectViewSection(result.Data);
                    AppSession.createOrSelectCollection.Add(createOrSelectTemplateGroup);
                    AppSession.createOrSelectCollection.RemoveAt(0);
                    Container.Children.Clear();
                    Content.Children.Clear();
                    Container.Children.Add(BuildModal());
                    Content.Children.Add(Container);
                }
                else
                {
                    App.ShowAlert("Failed to delete meal template.");
                }
            }
        }

        public CollectionView BuildCollectionView(Action buildConfirm)
        {
            CreateOrSelectCollectionView = new CreateOrSelectCollectionView(buildConfirm);
            createOrSelectLayout = CreateOrSelectCollectionView.GetCollectionView();

            CreateOrSelectCollectionView.ShowMealPlans();

            return createOrSelectLayout;
        }

        public CollectionView BuildTemplatesView(Action buildConfirm)
        {
            CreateOrSelectCollectionView = new CreateOrSelectCollectionView(buildConfirm);
            createOrSelectLayout = CreateOrSelectCollectionView.GetCollectionView();

            CreateOrSelectCollectionView.ShowTemplates();

            return createOrSelectLayout;
        }

        async void OnToggled(object sender, ToggledEventArgs e)
        {
            if (AppSession.InfoModeOn)
            {
                App.ShowInfoBubble(new Paragraph("Private or Public Plan", "This button will give you the option to make a created Meal Plan from scratch, either Public for everyone to view or keep it Private for your viewing only!", null).Content, Units.HalfScreenWidth, Units.HalfScreenHeight);
            }
            else
            {
                if (!isBusy)
                {
                    // Perform an action after examining e.Value
                    isBusy = true;
                    if (e.Value)
                    {
                        // Activated
                        switchLabel.Content.Text = AppText.PUBLIC_MEAL_PLAN;
                        //otherMealsLabel.Content.Text = "Other Meal Templates you've created:";
                        //Collection View Modified Crash
                        otherMealsCont.Children.Clear();
                        otherMealsCont.Children.Add(BuildTemplatesView(CallBuildConfirm));
                        isMealPlan = false;
                    }
                    else
                    {
                        // Deactivated
                        switchLabel.Content.Text = AppText.PRIVATE_MEAL_PLAN;
                        //otherMealsLabel.Content.Text = "Other Meal Plans you've created:";
                        //Collection View Modified Crash
                        otherMealsCont.Children.Clear();
                        otherMealsCont.Children.Add(BuildCollectionView(CallBuildConfirm));
                        isMealPlan = true;
                    }
                    isBusy = false;
                }
            }
        }
    }
}
