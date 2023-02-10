using System;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Composites;
using ChaiCooking.Components.Fields;
using ChaiCooking.Components.Fields.Custom;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Panels.Account
{
    public class AccountOverviewPanel : StandardLayout
    {
        StackLayout ContentContainer;
        StackLayout SectionContainer;

        StaticLabel Title;

        /*
        StaticLabel UserName;
        StaticLabel EmailAddress;
        StaticLabel CurrentPlan;

        ColourButton DeleteDisableAccountButton;
        */

        

        private const int ACCOUNT_OVERVIEW = 0;
        private const int DELETE_ACCOUNT = 1;
        private const int DELETE_DETAILS = 2;
        private const int DELETE_CONFIRMATION = 3;
        private const int DISABLE_ACCOUNT = 4;
        private const int DISABLE_DETAILS = 5;
        private const int DISABLE_CONFIRMATION = 6;

        private int CurrentSection;

        // maybe put these in seperate layouts / panels.. we'll see
        StackLayout AccountOverviewContainer;
        StackLayout DeleteAccountContainer;
        StackLayout DeleteDetailsContainer;
        StackLayout DeleteConfirmationContainer;
        StackLayout DisableAccountContainer;
        StackLayout DisableDetailsContainer;
        StackLayout DisableConfirmationContainer;


        public AccountOverviewPanel()
        {
            Container = new Grid { };
            Content = new Grid { };

            ContentContainer = new StackLayout { Orientation = StackOrientation.Vertical, Padding = Dimensions.GENERAL_COMPONENT_PADDING };

            SectionContainer = new StackLayout { Orientation = StackOrientation.Vertical };

            Title = new StaticLabel(AppText.ACCOUNT_OVERVIEW);
            Title.Content.FontSize = Units.FontSizeL;
            Title.Content.TextColor = Color.White;
            Title.LeftAlign();

            CurrentSection = ACCOUNT_OVERVIEW;

            AccountOverviewContainer = BuildAccountOverview();
            DeleteAccountContainer = BuildDeleteAccount();
            DeleteDetailsContainer = BuildDeleteDetails();
            DeleteConfirmationContainer = BuildDeleteConfirmation();
            DisableAccountContainer = BuildDisableAccount();
            DisableDetailsContainer = BuildDisableDetails();
            DisableConfirmationContainer = BuildDisableConfirmation();
            
            SectionContainer.Children.Add(GetSection(CurrentSection));

            ContentContainer.Children.Add(SectionContainer);

            Container.Children.Add(ContentContainer);
            Content.Children.Add(Container);
        }

        public void SetSection(int sectionToShow)
        {
            SectionContainer.Children.Clear();
            SectionContainer.Children.Add(GetSection(sectionToShow));
        }

        private StackLayout GetSection(int currentSection)
        {
            StackLayout currentContainer = AccountOverviewContainer;
            switch (currentSection)
            {
                case ACCOUNT_OVERVIEW:
                    currentContainer = AccountOverviewContainer;
                    break;
                case DELETE_ACCOUNT:
                    currentContainer = DeleteAccountContainer;
                    break;
                case DELETE_DETAILS:
                    currentContainer = DeleteDetailsContainer;
                    break;
                case DELETE_CONFIRMATION:
                    currentContainer = DeleteConfirmationContainer;
                    break;
                case DISABLE_ACCOUNT:
                    currentContainer = DisableAccountContainer;
                    break;
                case DISABLE_DETAILS:
                    currentContainer = DisableDetailsContainer;
                    break;
                case DISABLE_CONFIRMATION:
                    currentContainer = DisableConfirmationContainer;
                    break;
            }
            return currentContainer;
        }

        private StackLayout BuildAccountOverview()
        {
            AccountOverviewContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Grid container = new Grid
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            StaticLabel LeftTitle = new StaticLabel(AppText.ACCOUNT_OVERVIEW);
            LeftTitle.Content.FontSize = Units.FontSizeM;
            LeftTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            LeftTitle.Content.TextColor = Color.White;
            LeftTitle.LeftAlign();

            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            StaticLabel UserName = new StaticLabel(AppText.USERNAME + ": " + AppSession.CurrentUser.Username);
            UserName.Content.FontSize = Units.FontSizeM;
            UserName.Content.TextColor = Color.White;
            UserName.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;
            UserName.LeftAlign();

            StaticLabel EmailAddress = new StaticLabel(AppText.EMAIL_ADDRESS + ": " + AppSession.CurrentUser.EmailAddress);
            EmailAddress.Content.FontSize = Units.FontSizeM;
            EmailAddress.Content.TextColor = Color.White;
            EmailAddress.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;
            EmailAddress.LeftAlign();

            StaticLabel CurrentPlan = new StaticLabel(AppSession.CurrentUser.Preferences.AccountName);
            CurrentPlan.Content.FontSize = Units.FontSizeM;
            CurrentPlan.Content.TextColor = Color.White;
            CurrentPlan.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;
            CurrentPlan.LeftAlign();
            CurrentPlan.Content.SetBinding(Label.TextProperty, new Binding(nameof(AppSession.CurrentUser.Preferences.AccountName), source: AppSession.CurrentUser.Preferences));

            ColourButton DeleteDisableAccountButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.DELETE_DISABLE_ACCOUNT, null);
            DeleteDisableAccountButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            DeleteDisableAccountButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            DeleteDisableAccountButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            DeleteDisableAccountButton.CenterAlign();

            DeleteDisableAccountButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               CurrentSection = DELETE_ACCOUNT;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );

            container.Children.Add(LeftTitle.Content, 0, 0);
            container.Children.Add(UserName.Content, 0, 1);
            container.Children.Add(EmailAddress.Content, 0, 2);
            container.Children.Add(CurrentPlan.Content, 0, 3);
            container.Children.Add(DeleteDisableAccountButton.Content, 0, 4);

            //Grid.SetColumnSpan(UserName.Content, 2);
            //Grid.SetColumnSpan(EmailAddress.Content, 2);
            //Grid.SetColumnSpan(CurrentPlan.Content, 2);
            
            AccountOverviewContainer.Children.Add(container);

            return AccountOverviewContainer;
        }

        private StackLayout BuildDeleteAccount()
        {
            DeleteAccountContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Grid container = new Grid
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            StaticLabel LeftTitle = new StaticLabel(AppText.ACCOUNT_OVERVIEW);
            LeftTitle.Content.FontSize = Units.FontSizeM;
            LeftTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            LeftTitle.Content.TextColor = Color.White;
            LeftTitle.LeftAlign();

            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            StaticLabel DeleteInfo = new StaticLabel("Hi " + AppSession.CurrentUser.Username + ".\n\nWe're sorry that you'd like to delete your account. If you'd like to just take a break, you can temporarily disable your CHAI account.");
            DeleteInfo.Content.FontSize = Units.FontSizeM;
            DeleteInfo.Content.TextColor = Color.White;
            DeleteInfo.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;
            DeleteInfo.LeftAlign();

            StaticLabel DisableInfo = new StaticLabel("Temporarily disables account, keeping details intact.");
            DisableInfo.Content.FontSize = Units.FontSizeM;
            DisableInfo.Content.TextColor = Color.White;
            DisableInfo.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;
            DisableInfo.LeftAlign();

            
            ColourButton DisableAccountButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.DISABLE_ACCOUNT, null);
            DisableAccountButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            DisableAccountButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            DisableAccountButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            DisableAccountButton.CenterAlign();

            DisableAccountButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               //CurrentSection = DISABLE_ACCOUNT;
                               CurrentSection = DISABLE_DETAILS;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );

            ColourButton CancelButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CANCEL, null);
            CancelButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            CancelButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            CancelButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            CancelButton.LeftAlign();

            CancelButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               CurrentSection = ACCOUNT_OVERVIEW;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );

            ColourButton DeleteAccountButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.DELETE_ACCOUNT, null);
            DeleteAccountButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            DeleteAccountButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            DeleteAccountButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            DeleteAccountButton.RightAlign();

            DeleteAccountButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               CurrentSection = DELETE_DETAILS;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );

            container.Children.Add(LeftTitle.Content, 0, 0);
            container.Children.Add(DeleteInfo.Content, 0, 1);
            container.Children.Add(DisableAccountButton.Content, 0, 2);
            container.Children.Add(DisableInfo.Content, 0, 3);
            container.Children.Add(CancelButton.Content, 0, 4);
            container.Children.Add(DeleteAccountButton.Content, 1, 4);

            Grid.SetColumnSpan(DeleteInfo.Content, 2);
            Grid.SetColumnSpan(DisableAccountButton.Content, 2);
            Grid.SetColumnSpan(DisableInfo.Content, 2);

            DeleteAccountContainer.Children.Add(container);

            return DeleteAccountContainer;
        }

        private StackLayout BuildDeleteDetails()
        {
            DeleteDetailsContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Grid container = new Grid
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Grid QuestionsContainer = new Grid { };

            StaticLabel LeftTitle = new StaticLabel(AppText.ACCOUNT_OVERVIEW + " - Delete Account");
            LeftTitle.Content.FontSize = Units.FontSizeM;
            LeftTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            LeftTitle.Content.TextColor = Color.White;
            LeftTitle.LeftAlign();

            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            StaticLabel DeleteQuestionsInfo = new StaticLabel("Please tell us why you'd like to delete your account - this info will help us to understand how we can improve CHAI for everyone.");
            DeleteQuestionsInfo.Content.FontSize = Units.FontSizeM;
            DeleteQuestionsInfo.Content.TextColor = Color.White;
            DeleteQuestionsInfo.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;
            DeleteQuestionsInfo.CenterAlign();

            int row = 0;
            int col = 0;
            //int itemCount = 0;

            foreach (Preference pref in AppDataContent.DeleteAccountPrefs)
            {
                Components.Composites.CheckBox checkBox = new Components.Composites.CheckBox(pref.Name, "tick.png", "tickbg.png", 280, 64, pref.IsSelected);
                checkBox.SetCheckboxLeft();
                checkBox.SetIconSize(Dimensions.CHECKBOX_ICON_SIZE, Dimensions.CHECKBOX_ICON_SIZE);
                checkBox.Title.Content.FontSize = Dimensions.CHECKBOX_FONT_SIZE;
                checkBox.Title.Content.TextColor = Color.White;

                QuestionsContainer.Children.Add(checkBox.Content, col, row);

                col++;
                //if (col >= Dimensions.GetNumberOfMenuColumns()) { col = 0; row++; };
                if (col >= 2) { col = 0; row++; };

                /*
                itemCount++;
                if (itemCount == AppSession.CurrentUser.Preferences.OtherPrefs.Count && AppSession.CurrentUser.Preferences.OtherPrefs.Count % 2 > 0)
                {
                    Grid.SetColumnSpan(checkBox.Content, 2);
                }*/
            }


            ColourButton CancelButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CANCEL, null);
            CancelButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            CancelButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            CancelButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            CancelButton.LeftAlign();

            CancelButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               CurrentSection = ACCOUNT_OVERVIEW;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );

            ColourButton SubmitButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.SUBMIT, null);
            SubmitButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            SubmitButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            SubmitButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            SubmitButton.CenterAlign();

            SubmitButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               CurrentSection = DELETE_CONFIRMATION;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );

            container.Children.Add(LeftTitle.Content, 0, 0);
            container.Children.Add(DeleteQuestionsInfo.Content, 0, 1);
            container.Children.Add(QuestionsContainer, 0, 2);
            container.Children.Add(CancelButton.Content, 0, 3);
            container.Children.Add(SubmitButton.Content, 1, 3);
            //container.Children.Add(DisableInfo.Content, 0, 2);
            //container.Children.Add(CancelButton.Content, 0, 3);
            //container.Children.Add(DeleteAccountButton.Content, 1, 3);

            Grid.SetColumnSpan(LeftTitle.Content, 2);
            Grid.SetColumnSpan(DeleteQuestionsInfo.Content, 2);
            Grid.SetColumnSpan(QuestionsContainer, 2);
            //Grid.SetColumnSpan(DisableInfo.Content, 2);



            DeleteDetailsContainer.Children.Add(container);

            return DeleteDetailsContainer;
        }

        private StackLayout BuildDeleteConfirmation()
        {
            DeleteConfirmationContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Grid container = new Grid
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            

            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            StaticLabel LeftTitle = new StaticLabel(AppText.WARNING);
            LeftTitle.Content.FontSize = Units.FontSizeM;
            LeftTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            LeftTitle.Content.TextColor = Color.White;
            LeftTitle.LeftAlign();

            StaticImage WarningImage = new StaticImage("warningicon.png", 64, 64, null);

            StaticLabel DeleteDetailsInfo = new StaticLabel("This will delete your account data and cancel your subscription");
            DeleteDetailsInfo.Content.FontSize = Units.FontSizeM;
            DeleteDetailsInfo.Content.TextColor = Color.White;
            DeleteDetailsInfo.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;
            DeleteDetailsInfo.CenterAlign();

            StaticLabel DeleteConfirmInfo = new StaticLabel("This step cannot be undone");
            DeleteConfirmInfo.Content.FontSize = Units.FontSizeM;
            DeleteConfirmInfo.Content.TextColor = Color.White;
            DeleteConfirmInfo.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;
            DeleteConfirmInfo.CenterAlign();


            ColourButton CancelButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CANCEL, null);
            CancelButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            CancelButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            CancelButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            CancelButton.LeftAlign();

            CancelButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               CurrentSection = DELETE_DETAILS;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );

            ColourButton SubmitButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.SUBMIT, null);
            SubmitButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            SubmitButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            SubmitButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            SubmitButton.CenterAlign();

            SubmitButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               CurrentSection = ACCOUNT_OVERVIEW;
                               SetSection(CurrentSection);
                               await App.GoToLoginOrRegister();
                               //await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.LoginAndRegistration);
                           });
                       })
                   }
               );

            container.Children.Add(LeftTitle.Content, 0, 0);
            container.Children.Add(WarningImage.Content, 0, 1);
            container.Children.Add(DeleteDetailsInfo.Content, 1, 1);
            container.Children.Add(DeleteConfirmInfo.Content, 0, 2);
            container.Children.Add(CancelButton.Content, 0, 3);
            container.Children.Add(SubmitButton.Content, 1, 3);

            //Grid.SetColumnSpan(DeleteDetailsInfo.Content, 2);
            Grid.SetColumnSpan(DeleteConfirmInfo.Content, 2);

            
            DeleteConfirmationContainer.Children.Add(container);

            return DeleteConfirmationContainer;
        }

        private StackLayout BuildDisableAccount()
        {
            DisableAccountContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Grid container = new Grid
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            DisableAccountContainer.Children.Add(container);

            return DisableAccountContainer;
        }

        private StackLayout BuildDisableDetails()
        {
            DisableDetailsContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Grid container = new Grid
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Grid QuestionsContainer = new Grid { };

            StaticLabel LeftTitle = new StaticLabel(AppText.ACCOUNT_OVERVIEW + " - Disable Account");
            LeftTitle.Content.FontSize = Units.FontSizeM;
            LeftTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            LeftTitle.Content.TextColor = Color.White;
            LeftTitle.LeftAlign();

            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            StaticLabel DisableQuestionsInfo = new StaticLabel("Please tell us why you'd like to disable your account - this info will help us to understand how we can improve CHAI for everyone.");
            DisableQuestionsInfo.Content.FontSize = Units.FontSizeM;
            DisableQuestionsInfo.Content.TextColor = Color.White;
            DisableQuestionsInfo.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;
            DisableQuestionsInfo.CenterAlign();

            int row = 0;
            int col = 0;
            //int itemCount = 0;

            foreach (Preference pref in AppDataContent.DeleteAccountPrefs)
            {
                Components.Composites.CheckBox checkBox = new Components.Composites.CheckBox(pref.Name, "tick.png", "tickbg.png", 280, 64, pref.IsSelected);
                checkBox.SetCheckboxLeft();
                checkBox.SetIconSize(Dimensions.CHECKBOX_ICON_SIZE, Dimensions.CHECKBOX_ICON_SIZE);
                checkBox.Title.Content.FontSize = Dimensions.CHECKBOX_FONT_SIZE;
                checkBox.Title.Content.TextColor = Color.White;

                QuestionsContainer.Children.Add(checkBox.Content, col, row);

                col++;

                if (col >= 2) { col = 0; row++; };
            }


            ColourButton CancelButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CANCEL, null);
            CancelButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            CancelButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            CancelButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            CancelButton.LeftAlign();

            CancelButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               CurrentSection = ACCOUNT_OVERVIEW;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );

            ColourButton SubmitButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.SUBMIT, null);
            SubmitButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            SubmitButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            SubmitButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            SubmitButton.CenterAlign();

            SubmitButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               CurrentSection = DISABLE_CONFIRMATION;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );

            container.Children.Add(LeftTitle.Content, 0, 0);
            container.Children.Add(DisableQuestionsInfo.Content, 0, 1);
            container.Children.Add(QuestionsContainer, 0, 2);
            container.Children.Add(CancelButton.Content, 0, 3);
            container.Children.Add(SubmitButton.Content, 1, 3);
            //container.Children.Add(DisableInfo.Content, 0, 2);
            //container.Children.Add(CancelButton.Content, 0, 3);
            //container.Children.Add(DeleteAccountButton.Content, 1, 3);

            Grid.SetColumnSpan(LeftTitle.Content, 2);
            Grid.SetColumnSpan(DisableQuestionsInfo.Content, 2);
            Grid.SetColumnSpan(QuestionsContainer, 2);
            //Grid.SetColumnSpan(DisableInfo.Content, 2);



            DisableDetailsContainer.Children.Add(container);

            return DisableDetailsContainer;
        }


        private StackLayout BuildDisableConfirmation()
        {
            DisableConfirmationContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Grid container = new Grid
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            StaticLabel LeftTitle = new StaticLabel(AppText.WARNING);
            LeftTitle.Content.FontSize = Units.FontSizeM;
            LeftTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            LeftTitle.Content.TextColor = Color.White;
            LeftTitle.LeftAlign();

            StaticImage WarningImage = new StaticImage("warningicon.png", 64, 64, null);

            StaticLabel DisableDetailsInfo = new StaticLabel("This will temporarily disable your account data and suspends your subscription");
            DisableDetailsInfo.Content.FontSize = Units.FontSizeM;
            DisableDetailsInfo.Content.TextColor = Color.White;
            DisableDetailsInfo.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;
            DisableDetailsInfo.CenterAlign();

            StaticLabel DisableConfirmInfo = new StaticLabel("You can reactivate your account by logging in again. Your account details will be retained by CHAI. If you want to cancel your subscription, you can do this via Payment Settings instead.");
            DisableConfirmInfo.Content.FontSize = Units.FontSizeM;
            DisableConfirmInfo.Content.TextColor = Color.White;
            DisableConfirmInfo.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;
            DisableConfirmInfo.CenterAlign();


            ColourButton CancelButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CANCEL, null);
            CancelButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            CancelButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            CancelButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            CancelButton.LeftAlign();

            CancelButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               CurrentSection = DISABLE_DETAILS;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );

            ColourButton SubmitButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.SUBMIT, null);
            SubmitButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            SubmitButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            SubmitButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            SubmitButton.CenterAlign();

            SubmitButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               CurrentSection = ACCOUNT_OVERVIEW;
                               SetSection(CurrentSection);
                               await App.GoToLoginOrRegister();
                               //await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.LoginAndRegistration);
                           });
                       })
                   }
               );

            container.Children.Add(LeftTitle.Content, 0, 0);
            container.Children.Add(WarningImage.Content, 0, 1);
            container.Children.Add(DisableDetailsInfo.Content, 1, 1);
            container.Children.Add(DisableConfirmInfo.Content, 0, 2);
            container.Children.Add(CancelButton.Content, 0, 3);
            container.Children.Add(SubmitButton.Content, 1, 3);

            //Grid.SetColumnSpan(DisableDetailsInfo.Content,2);
            Grid.SetColumnSpan(DisableConfirmInfo.Content, 2);


            DisableConfirmationContainer.Children.Add(container);

            return DisableConfirmationContainer;
        }
    }
}
