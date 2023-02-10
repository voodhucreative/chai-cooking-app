using System;
using System.Threading.Tasks;
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
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Panels.Account
{
    public class PaymentSettingsPanel : StandardLayout
    {
        StackLayout ContentContainer;
        StackLayout SectionContainer;

        StaticLabel Title;

        private const int CANCEL_SUBSCRIPTION = 0;
        private const int CANCEL_CONFIRM = 1;
        private const int PROCESSING = 2;
        private const int CANCEL_SUCCESS = 3;
        

        private int CurrentSection;

        // maybe put these in seperate layouts / panels.. we'll see
        StackLayout CancelSubscriptionContainer;
        StackLayout CancelConfirmContainer;
        StackLayout ProcessingContainer;
        StackLayout CancelSuccessContainer;

        public PaymentSettingsPanel()
        {
            Container = new Grid { };
            Content = new Grid { };
            ContentContainer = new StackLayout { Orientation = StackOrientation.Vertical, Padding = Dimensions.GENERAL_COMPONENT_PADDING };

            SectionContainer = new StackLayout { Orientation = StackOrientation.Vertical };

            Title = new StaticLabel(AppText.UPGRADE);
            Title.Content.FontSize = Units.FontSizeL;
            Title.Content.TextColor = Color.White;
            Title.LeftAlign();

            CurrentSection = CANCEL_SUBSCRIPTION;

            CancelSubscriptionContainer = BuildCancelSubscription();
            CancelConfirmContainer = BuildCancelConfirm();
            ProcessingContainer = BuildProcessing();
            CancelSuccessContainer = BuildCancelSuccess();

            SectionContainer.Children.Add(GetSection(CurrentSection));

            //ContentContainer.Children.Add(Title.Content);

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
            StackLayout currentContainer = CancelSubscriptionContainer;
            switch (currentSection)
            {
                case CANCEL_SUBSCRIPTION:
                    currentContainer = CancelSubscriptionContainer;
                    break;
                case CANCEL_CONFIRM:
                    currentContainer = CancelConfirmContainer;
                    break;
                case PROCESSING:
                    currentContainer = ProcessingContainer;
                    break;
                case CANCEL_SUCCESS:
                    currentContainer = CancelSuccessContainer;
                    break;
            }
            return currentContainer;
        }

        private StackLayout BuildCancelSubscription()
        {
            CancelSubscriptionContainer = new StackLayout
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


            StaticLabel LeftTitle = new StaticLabel("Payment Settings");
            LeftTitle.Content.FontSize = Units.FontSizeM;
            LeftTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            LeftTitle.Content.TextColor = Color.White;
            LeftTitle.LeftAlign();


            StaticLabel Info = new StaticLabel("You opted for the in-app purchase to upgrade to "+Accounts.GetAccountName(AppSession.CurrentUser.Preferences.AccountType)+".\n\nDo you want to cancel your subcription? This will end your payments and remove the additional features that upgrading brought to your account. You will still be able to use CHAI, but will lose access to the Meal Planner, Recipe Editor and Tracker Facilities.");
            Info.Content.FontSize = Units.FontSizeM;
            Info.Content.FontFamily = Fonts.GetBoldAppFont();
            Info.Content.TextColor = Color.White;
            Info.CenterAlign();

            
            ColourButton ConfirmButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Cancel Subscription", null);
            ConfirmButton.SetSize(Units.ScreenWidth, Dimensions.STANDARD_BUTTON_HEIGHT);
            ConfirmButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING; ;

            ConfirmButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               CurrentSection = CANCEL_CONFIRM;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );

            container.Children.Add(LeftTitle.Content, 0, 0);
            container.Children.Add(Info.Content, 0, 1);
            container.Children.Add(ConfirmButton.Content, 0, 2);
            
            CancelSubscriptionContainer.Children.Add(container);

            return CancelSubscriptionContainer;
        }


        

        private StackLayout BuildCancelConfirm()
        {
            CancelConfirmContainer = new StackLayout
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
           
            StaticLabel LeftTitle = new StaticLabel("Warning");
            LeftTitle.Content.FontSize = Units.FontSizeM;
            LeftTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            LeftTitle.Content.TextColor = Color.White;
            LeftTitle.LeftAlign();

            StaticImage WarningImage = new StaticImage("warningicon.png", 64, 64, null);

            StaticLabel ConfirmWarningInfo = new StaticLabel("This will cancel your subscription and remove associated extra functions");
            ConfirmWarningInfo.Content.FontSize = Units.FontSizeM;
            ConfirmWarningInfo.Content.FontFamily = Fonts.GetBoldAppFont();
            ConfirmWarningInfo.Content.TextColor = Color.White;
            ConfirmWarningInfo.LeftAlign();

            StaticLabel ConfirmInfo = new StaticLabel("You opted for the in-app purchase to upgrade to "+ Accounts.GetAccountName(AppSession.CurrentUser.Preferences.AccountType) + ".\n\nDo you want to cancel your subcription? This will end your payments and remove the additional features that upgrading brought to your account. You will still be able to use CHAI, but will lose access to the Meal Planner, Recipe Editor and Tracker Facilities.");
            ConfirmInfo.Content.FontSize = Units.FontSizeM;
            ConfirmInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            ConfirmInfo.Content.TextColor = Color.White;
            ConfirmInfo.CenterAlign();

            ColourButton OkButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Ok", null);
            OkButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            OkButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;
            OkButton.RightAlign();

            OkButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               CurrentSection = PROCESSING;
                               SetSection(CurrentSection);
                               await PerformSubmit();
                           });
                       })
                   }
               );

            ColourButton CancelButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Cancel", null);
            CancelButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            CancelButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING; ;
            CancelButton.LeftAlign();

            CancelButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               CurrentSection = CANCEL_SUBSCRIPTION;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );

            container.Children.Add(LeftTitle.Content, 0, 0);
            container.Children.Add(WarningImage.Content, 0, 1);
            container.Children.Add(ConfirmWarningInfo.Content, 1, 1);
            container.Children.Add(ConfirmInfo.Content, 0, 2);
            container.Children.Add(CancelButton.Content, 0, 3);
            container.Children.Add(OkButton.Content, 1, 3);

           

            Grid.SetColumnSpan(ConfirmInfo.Content, 2);
            //Grid.SetColumnSpan(ContactEmail.Content, 2);
            //Grid.SetColumnSpan(OkButton.Content, 2);

            CancelConfirmContainer.Children.Add(container);

            return CancelConfirmContainer;
        }

        private StackLayout BuildProcessing()
        {
            ProcessingContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Grid container = new Grid
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            StaticLabel LeftTitle = new StaticLabel("Processing");
            LeftTitle.Content.FontSize = Units.FontSizeM;
            LeftTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            LeftTitle.Content.TextColor = Color.White;
            LeftTitle.LeftAlign();
            /*
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            StaticLabel LeftTitle = new StaticLabel("Warning");
            LeftTitle.Content.FontSize = Units.FontSizeM;
            LeftTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            LeftTitle.Content.TextColor = Color.White;
            LeftTitle.LeftAlign();


            StaticLabel FailInfo = new StaticLabel("Unable to process upgrade at this time");
            FailInfo.Content.FontSize = Units.FontSizeM;
            FailInfo.Content.FontFamily = Fonts.GetBoldAppFont();
            FailInfo.Content.TextColor = Color.White;
            FailInfo.LeftAlign();

            StaticImage FailImage = new StaticImage("homeicon.png", 64, 64, null);

            StaticLabel FailDetail = new StaticLabel("Please try again.\n\nIf this message continues to appear then please contact us for further assistance on:");
            FailDetail.Content.FontSize = Units.FontSizeM;
            FailDetail.Content.FontFamily = Fonts.GetRegularAppFont();
            FailDetail.Content.TextColor = Color.White;
            FailDetail.CenterAlign();

            StaticLabel ContactEmail = new StaticLabel(AppText.CONTACT_EMAIL_ADDRESS);
            ContactEmail.Content.FontSize = Units.FontSizeL;
            ContactEmail.Content.FontFamily = Fonts.GetBoldAppFont();
            ContactEmail.Content.TextColor = Color.White;
            ContactEmail.CenterAlign();

            ColourButton OkButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Ok", null);
            OkButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            OkButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING; ;

            OkButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               CurrentSection = CONFIRMATION;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );


            container.Children.Add(LeftTitle.Content, 0, 0);
            container.Children.Add(FailImage.Content, 0, 1);
            container.Children.Add(FailInfo.Content, 1, 1);
            container.Children.Add(FailDetail.Content, 0, 2);
            container.Children.Add(ContactEmail.Content, 0, 3);
            container.Children.Add(OkButton.Content, 0, 4);

            Grid.SetColumnSpan(FailDetail.Content, 2);
            Grid.SetColumnSpan(ContactEmail.Content, 2);
            Grid.SetColumnSpan(OkButton.Content, 2);
            */
            container.Children.Add(LeftTitle.Content, 0, 0);
            ProcessingContainer.Children.Add(container);

            return ProcessingContainer;
        }

        private StackLayout BuildCancelSuccess()
        {
            CancelSuccessContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Grid container = new Grid
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            StaticLabel LeftTitle = new StaticLabel("Success");
            LeftTitle.Content.FontSize = Units.FontSizeM;
            LeftTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            LeftTitle.Content.TextColor = Color.White;
            LeftTitle.LeftAlign();
            /*
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            


            StaticLabel FailInfo = new StaticLabel("Unable to process upgrade at this time");
            FailInfo.Content.FontSize = Units.FontSizeM;
            FailInfo.Content.FontFamily = Fonts.GetBoldAppFont();
            FailInfo.Content.TextColor = Color.White;
            FailInfo.LeftAlign();

            StaticImage FailImage = new StaticImage("homeicon.png", 64, 64, null);

            StaticLabel FailDetail = new StaticLabel("Please try again.\n\nIf this message continues to appear then please contact us for further assistance on:");
            FailDetail.Content.FontSize = Units.FontSizeM;
            FailDetail.Content.FontFamily = Fonts.GetRegularAppFont();
            FailDetail.Content.TextColor = Color.White;
            FailDetail.CenterAlign();

            StaticLabel ContactEmail = new StaticLabel(AppText.CONTACT_EMAIL_ADDRESS);
            ContactEmail.Content.FontSize = Units.FontSizeL;
            ContactEmail.Content.FontFamily = Fonts.GetBoldAppFont();
            ContactEmail.Content.TextColor = Color.White;
            ContactEmail.CenterAlign();

            ColourButton OkButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Ok", null);
            OkButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            OkButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING; ;

            OkButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               CurrentSection = CONFIRMATION;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );


            container.Children.Add(LeftTitle.Content, 0, 0);
            container.Children.Add(FailImage.Content, 0, 1);
            container.Children.Add(FailInfo.Content, 1, 1);
            container.Children.Add(FailDetail.Content, 0, 2);
            container.Children.Add(ContactEmail.Content, 0, 3);
            container.Children.Add(OkButton.Content, 0, 4);

            Grid.SetColumnSpan(FailDetail.Content, 2);
            Grid.SetColumnSpan(ContactEmail.Content, 2);
            Grid.SetColumnSpan(OkButton.Content, 2);
            */
            container.Children.Add(LeftTitle.Content, 0, 0);

            CancelSuccessContainer.Children.Add(container);

            return CancelSuccessContainer;
        }


        
        public async Task<bool> PerformSubmit()
        {

            await Task.Delay(5000); // simulate submission process

            bool success = true;

            if (success)
            {
                //AppSession.CurrentUser.Preferences.AccountType = Accounts.AccountType.ChaiFree;

                // save user
                AppSession.CurrentUser.Update();

                CurrentSection = CANCEL_SUCCESS;
                SetSection(CurrentSection);
            }
            else
            {
                App.ShowAlert("We cannot cancel your subcription at this time");
                CurrentSection = CANCEL_CONFIRM; // TO DO: NEEDS FAIL PAGE DESIGN!
                SetSection(CurrentSection);
            }
            return success;
        }
    }
}
