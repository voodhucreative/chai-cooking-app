using System;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Composites;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom.Panels.Account;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class UpgradePlan : Page
    {

        StackLayout ContentContainer;

        StackLayout HeaderContainer;
        StaticLabel Title;
        StaticImage HeaderIcon;
 
        Grid BottomSectionBackground;
        StackLayout OptionsContainer;
       
        UpgradePanel Upgrade;

        private const int UPGRADE_DETAILS = 0;
        private const int UPGRADE_SUCCESS = 1;
        private const int UPGRADE_FAILED = 2;
        
        //private const int UPGRADE = 5;

        private int CurrentSubSection;

        public UpgradePlan()
        {
            this.IsScrollable = true;
            this.IsRefreshable = true;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;


            this.Id = (int)AppSettings.PageNames.UpgradePlan;
            this.Name = AppData.AppText.ACCOUNT_PREFERENCES;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.SlideInFromRight;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.SlideOutToRight;

            CurrentSubSection = UPGRADE_DETAILS;

            PageContent = new Grid
            {
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY)
            };

            ContentContainer = BuildContent();
        }

        public StackLayout BuildContent()
        {
            ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING)
            };

            HeaderContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = Dimensions.GENERAL_COMPONENT_SPACING,
                Padding = new Thickness(0, Dimensions.GENERAL_COMPONENT_PADDING)
            };
            
            OptionsContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = Dimensions.GENERAL_COMPONENT_SPACING,
                Padding = Dimensions.GENERAL_COMPONENT_SPACING
            };

            HeaderIcon = new StaticImage("prefsicon.png", Dimensions.STANDARD_ICON_WIDTH, null);

            Title = new StaticLabel(AppData.AppText.PAYMENT);
            Title.Content.TextColor = Color.White;
            Title.Content.FontSize = Units.FontSizeL;
            Title.Content.FontFamily = Fonts.GetBoldAppFont();
            Title.LeftAlign();
            
            HeaderContainer.Children.Add(HeaderIcon.Content);
            HeaderContainer.Children.Add(Title.Content);
            
            Upgrade = new UpgradePanel();


            ContentContainer.Children.Add(HeaderContainer);

            //ContentContainer.Children.Add(BottomSectionBackground);
            PageContent.Children.Add(ContentContainer);

            return ContentContainer;
        }

        private void UpdateVisibleSubSection()
        {
            /*
            BottomSectionContainer.Children.Clear();
            AccountOverViewLabel.ShowUnselected();
            EditProfileLabel.ShowUnselected();
            ChangePasswordLabel.ShowUnselected();
            PaymentSettingsLabel.ShowUnselected();
            TryPlantBasedPlanLabel.ShowUnselected();
            UpgradeLabel.ShowUnselected();

            switch (CurrentSubSection)
            {
                case ACCOUNT_OVERVIEW:
                    BottomSectionContainer.Children.Add(AccountOverview.GetContent());
                    AccountOverViewLabel.ShowSelected();
                    break;
                case EDIT_PROFILE:
                    BottomSectionContainer.Children.Add(EditProfile.GetContent());
                    EditProfileLabel.ShowSelected();
                    break;
                case CHANGE_PASSWORD:
                    BottomSectionContainer.Children.Add(ChangePassword.GetContent());
                    ChangePasswordLabel.ShowSelected();
                    break;
                case PAYMENT_SETTINGS:
                    BottomSectionContainer.Children.Add(PaymentSettings.GetContent());
                    PaymentSettingsLabel.ShowSelected();
                    break;
                case TRY_PLANT_BASED:
                    BottomSectionContainer.Children.Add(TryPlantBased.GetContent());
                    TryPlantBasedPlanLabel.ShowSelected();
                    break;
                case UPGRADE:
                    BottomSectionContainer.Children.Add(Upgrade.GetContent());
                    UpgradeLabel.ShowSelected();
                    break;
                default:
                    break;

            }*/
        }
    }
}
