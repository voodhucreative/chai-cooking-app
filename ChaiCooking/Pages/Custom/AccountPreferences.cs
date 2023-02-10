using System;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Composites;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom.Panels.Account;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class AccountPreferences : Page
    {
        
        StackLayout ContentContainer;

        StackLayout HeaderContainer;
        StaticLabel Title;
        StaticImage HeaderIcon;
        ActiveLabel CloseLabel;

        Grid Seperator;
        Grid BottomSectionBackground;
        StackLayout TopSectionContainer;
        StackLayout BottomSectionContainer;
        StackLayout OptionsContainer;
        SelectLabel AccountOverViewLabel;
        SelectLabel EditProfileLabel;
        SelectLabel ChangePasswordLabel;
        SelectLabel PaymentSettingsLabel;
        SelectLabel TryPlantBasedPlanLabel;
        SelectLabel UpgradeLabel;

        Avatar UserAvatar;

        AccountOverviewPanel AccountOverview;
        EditProfilePanel EditProfile;
        ChangePasswordPanel ChangePassword;
        PaymentSettingsPanel PaymentSettings;
        TryPlantBasedPanel TryPlantBased;
        UpgradePanel Upgrade;

        private const int ACCOUNT_OVERVIEW = 0;
        private const int EDIT_PROFILE = 1;
        private const int CHANGE_PASSWORD = 2;
        private const int PAYMENT_SETTINGS = 3;
        private const int TRY_PLANT_BASED = 4;
        private const int UPGRADE = 5;

        private int CurrentSubSection;

        public AccountPreferences()
        {
            this.IsScrollable = true;
            this.IsRefreshable = true;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;


            this.Id = (int)AppSettings.PageNames.AccountPreferences;
            this.Name = AppData.AppText.ACCOUNT_PREFERENCES;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.SlideInFromRight;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.SlideOutToRight;

            CurrentSubSection = ACCOUNT_OVERVIEW;

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

            TopSectionContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.Transparent//Yellow
            };

            BottomSectionContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                Padding = Dimensions.GENERAL_COMPONENT_PADDING
            };

            BottomSectionBackground = new Grid
            {
                Padding = 1,
                BackgroundColor = Color.FromHex(Colors.CC_DARK_GREY)
            };

            OptionsContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 8,//Dimensions.GENERAL_COMPONENT_SPACING,
                Padding = Dimensions.GENERAL_COMPONENT_SPACING
            };

            HeaderIcon = new StaticImage("prefsicon.png", 16/*Dimensions.STANDARD_ICON_WIDTH*/, null);
            
            Title = new StaticLabel(AppData.AppText.ACCOUNT_PREFERENCES);
            Title.Content.TextColor = Color.White;
            Title.Content.FontSize = Units.FontSizeL;
            Title.Content.FontFamily = Fonts.GetBoldAppFont();
            Title.LeftAlign();

            CloseLabel = new ActiveLabel(AppText.CLOSE, Units.FontSizeM, Color.Transparent, Color.White, null);
            CloseLabel.CenterAlign();

            CloseLabel.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await App.PerformActionAsync((int)Actions.ActionName.GoToPage, AppSession.LastPageId);
                                await App.ShowMenu();
                            });
                        })
                    }
                );

            Seperator = new Grid { WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) };

            HeaderContainer.Children.Add(HeaderIcon.Content);
            HeaderContainer.Children.Add(Title.Content);
            HeaderContainer.Children.Add(CloseLabel.Content);

            AccountOverViewLabel = new SelectLabel(AppText.ACCOUNT_OVERVIEW, Color.FromHex(Colors.CC_ORANGE), Color.White, Units.ScreenWidth, 24, true, false);
            //AccountOverViewLabel.SelectedAction = new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Landing);
            AccountOverViewLabel.Title.Content.FontFamily = Fonts.GetBoldAppFont();
            AccountOverViewLabel.Title.Content.FontSize = Units.FontSizeM;
            AccountOverViewLabel.DisableDefaultInteraction();
            AccountOverViewLabel.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                CurrentSubSection = ACCOUNT_OVERVIEW;
                                UpdateVisibleSubSection();
                            });
                        })
                    }
                );
            

            EditProfileLabel = new SelectLabel(AppText.EDIT_PROFILE, Color.FromHex(Colors.CC_ORANGE), Color.White, Units.ScreenWidth, 24, false, false);
            EditProfileLabel.Title.Content.FontFamily = Fonts.GetBoldAppFont();
            EditProfileLabel.Title.Content.FontSize = Units.FontSizeM;
            EditProfileLabel.DisableDefaultInteraction();
            EditProfileLabel.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                CurrentSubSection = EDIT_PROFILE;
                                UpdateVisibleSubSection();
                            });
                        })
                    }
                );

            ChangePasswordLabel = new SelectLabel(AppText.CHANGE_PASSWORD, Color.FromHex(Colors.CC_ORANGE), Color.White, Units.ScreenWidth, 24, false, false);
            ChangePasswordLabel.Title.Content.FontFamily = Fonts.GetBoldAppFont();
            ChangePasswordLabel.Title.Content.FontSize = Units.FontSizeM;
            ChangePasswordLabel.DisableDefaultInteraction();
            ChangePasswordLabel.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                CurrentSubSection = CHANGE_PASSWORD;
                                UpdateVisibleSubSection();
                            });
                        })
                    }
                );

            PaymentSettingsLabel = new SelectLabel(AppText.PAYMENT_SETTINGS, Color.FromHex(Colors.CC_ORANGE), Color.White, Units.ScreenWidth, 24, false, false);
            PaymentSettingsLabel.Title.Content.FontFamily = Fonts.GetBoldAppFont();
            PaymentSettingsLabel.Title.Content.FontSize = Units.FontSizeM;
            PaymentSettingsLabel.DisableDefaultInteraction();
            PaymentSettingsLabel.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                CurrentSubSection = PAYMENT_SETTINGS;
                                UpdateVisibleSubSection();
                            });
                        })
                    }
                );


            TryPlantBasedPlanLabel = new SelectLabel(AppText.TRY_PLANT_BASED_MEAL_PLAN, Color.FromHex(Colors.CC_ORANGE), Color.White, Units.ScreenWidth, 24, false, false);
            TryPlantBasedPlanLabel.Title.Content.FontFamily = Fonts.GetBoldAppFont();
            TryPlantBasedPlanLabel.Title.Content.FontSize = Units.FontSizeM;
            TryPlantBasedPlanLabel.DisableDefaultInteraction();
            TryPlantBasedPlanLabel.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                CurrentSubSection = TRY_PLANT_BASED;
                                UpdateVisibleSubSection();
                            });
                        })
                    }
                );

            UpgradeLabel = new SelectLabel(AppText.UPGRADE, Color.FromHex(Colors.CC_ORANGE), Color.White, Units.ScreenWidth, 24, false, false);
            UpgradeLabel.Title.Content.FontFamily = Fonts.GetBoldAppFont();
            UpgradeLabel.Title.Content.FontSize = Units.FontSizeM;
            UpgradeLabel.DisableDefaultInteraction();
            UpgradeLabel.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                CurrentSubSection = UPGRADE;
                                UpdateVisibleSubSection();
                            });
                        })
                    }
                );

            UserAvatar = new Avatar(AppSession.CurrentUser.Username, AppSession.CurrentUser.AvatarImageUrl, Dimensions.AVATAR_SIZE, Dimensions.AVATAR_SIZE);

            Binding b = new Binding(nameof(AppSession.CurrentUser.Username), source: AppSession.CurrentUser);

            UserAvatar.Title.Label.SetBinding(Label.TextProperty, new Binding(nameof(AppSession.CurrentUser.Username), source: AppSession.CurrentUser));
            UserAvatar.Icon.Image.SetBinding(CachedImage.SourceProperty, new Binding(nameof(AppSession.CurrentUser.AvatarImageUrl), source: AppSession.CurrentUser));



            UserAvatar.Content.VerticalOptions = LayoutOptions.StartAndExpand;

            OptionsContainer.Children.Add(AccountOverViewLabel.Content);
            OptionsContainer.Children.Add(EditProfileLabel.Content);
            OptionsContainer.Children.Add(ChangePasswordLabel.Content);
            OptionsContainer.Children.Add(PaymentSettingsLabel.Content);
            OptionsContainer.Children.Add(TryPlantBasedPlanLabel.Content);
            OptionsContainer.Children.Add(UpgradeLabel.Content);

            TopSectionContainer.Children.Add(OptionsContainer);
            TopSectionContainer.Children.Add(UserAvatar.Content);

            // create sub-panels - sort this later
            AccountOverview = new AccountOverviewPanel();
            EditProfile = new EditProfilePanel();
            ChangePassword = new ChangePasswordPanel();
            PaymentSettings = new PaymentSettingsPanel();
            TryPlantBased = new TryPlantBasedPanel();
            Upgrade = new UpgradePanel();


            BottomSectionContainer.Children.Add(AccountOverview.GetContent());


            BottomSectionBackground.Children.Add(BottomSectionContainer);

            ContentContainer.Children.Add(HeaderContainer);
            ContentContainer.Children.Add(Seperator);
            ContentContainer.Children.Add(TopSectionContainer);
            ContentContainer.Children.Add(BottomSectionBackground);
            PageContent.Children.Add(ContentContainer);

            return ContentContainer;
        }

        private void UpdateVisibleSubSection()
        {
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
                    AccountOverview.Update();
                    BottomSectionContainer.Children.Add(AccountOverview.GetContent());
                    AccountOverViewLabel.ShowSelected();
                    break;
                case EDIT_PROFILE:
                    EditProfile.Update();
                    BottomSectionContainer.Children.Add(EditProfile.GetContent());
                    EditProfileLabel.ShowSelected();
                    break;
                case CHANGE_PASSWORD:
                    ChangePassword.Update();
                    BottomSectionContainer.Children.Add(ChangePassword.GetContent());
                    ChangePasswordLabel.ShowSelected();
                    break;
                case PAYMENT_SETTINGS:
                    PaymentSettings.Update();
                    BottomSectionContainer.Children.Add(PaymentSettings.GetContent());
                    PaymentSettingsLabel.ShowSelected();
                    break;
                case TRY_PLANT_BASED:
                    TryPlantBased.Update();
                    BottomSectionContainer.Children.Add(TryPlantBased.GetContent());
                    TryPlantBasedPlanLabel.ShowSelected();
                    break;
                case UPGRADE:
                    Upgrade.Update();
                    BottomSectionContainer.Children.Add(Upgrade.GetContent());
                    UpgradeLabel.ShowSelected();
                    break;
                default:
                    break;

            }
        }
    }
}
