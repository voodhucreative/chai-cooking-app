using System;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Composites;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom.Panels.Account;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class YourCharacter : Page
    {

        StackLayout ContentContainer;

        StackLayout HeaderContainer;
        StaticLabel Title;
        StaticImage HeaderIcon;
        ActiveLabel CloseLabel;

        Grid Seperator;

        StackLayout CharacterSelectionContainer;
        Grid AvailableRewardIconsContainer;
        //StaticImage CurrentUserCharacterIcon;

        SocialRewardTile CurrenReward;
        
        public YourCharacter()
        {
            this.IsScrollable = true;
            this.IsRefreshable = true;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;


            this.Id = (int)AppSettings.PageNames.YourCharacter;
            this.Name = AppData.AppText.YOUR_CHARACTER;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.SlideInFromRight;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.SlideOutToRight;

            

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

            
            HeaderIcon = new StaticImage("charactericon.png", 16, null);

            Title = new StaticLabel(AppData.AppText.YOUR_CHARACTER);
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

            ContentContainer.Children.Add(HeaderContainer);
            ContentContainer.Children.Add(Seperator);








            BuildCharacterSelectionContainer();






            ContentContainer.Children.Add(CharacterSelectionContainer);


            PageContent.Children.Add(ContentContainer);

            return ContentContainer;
        }

        public StackLayout BuildCharacterSelectionContainer()
        {
            CharacterSelectionContainer = new StackLayout { Orientation = StackOrientation.Vertical, BackgroundColor=Color.FromHex(Colors.CC_DARK_BLUE_GREY), Padding = 2 };

            StackLayout InnerContainer = new StackLayout { Orientation = StackOrientation.Vertical, BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY), Padding = Dimensions.GENERAL_COMPONENT_PADDING};


            AvailableRewardIconsContainer = new Grid { Padding = Dimensions.GENERAL_COMPONENT_PADDING , ColumnSpacing = Dimensions.GENERAL_COMPONENT_SPACING, RowSpacing = Dimensions.GENERAL_COMPONENT_SPACING };

            CurrenReward = new SocialRewardTile (AppSession.CurrentUser.Preferences.CurrentCharacterImage, "");
            //CurrentUserCharacterIcon = new StaticImage(AppSession.CurrentUser.Preferences.CurrentCharacterImage, 128, null);

            Grid CurrentCharacterContainer = new Grid { VerticalOptions = LayoutOptions.CenterAndExpand, RowSpacing = Dimensions.GENERAL_COMPONENT_SPACING, ColumnSpacing = Dimensions.GENERAL_COMPONENT_SPACING, Padding = Dimensions.GENERAL_COMPONENT_PADDING };
            CurrentCharacterContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            CurrentCharacterContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });




            StaticLabel ChangeCharacterLabel = new StaticLabel("Change Character");
            ChangeCharacterLabel.LeftAlign();
            ChangeCharacterLabel.Content.FontSize = Units.FontSizeM;
            ChangeCharacterLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            ChangeCharacterLabel.Content.TextColor = Color.White;

            StaticLabel CurrentCharacterLabel = new StaticLabel("Current Character");
            CurrentCharacterLabel.CenterAlign();
            CurrentCharacterLabel.Content.FontSize = Units.FontSizeM;
            CurrentCharacterLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            CurrentCharacterLabel.Content.TextColor = Color.White;

            StaticLabel TapOnCharacterLabel = new StaticLabel("Please tap on a character to be your new user icon.");
            TapOnCharacterLabel.CenterAlign();
            TapOnCharacterLabel.Content.FontSize = Units.FontSizeM;
            TapOnCharacterLabel.Content.FontFamily = Fonts.GetRegularAppFont();
            TapOnCharacterLabel.Content.TextColor = Color.White;

            StaticLabel OtherCharactersLabel = new StaticLabel("Other Characters");
            OtherCharactersLabel.CenterAlign();
            OtherCharactersLabel.Content.FontSize = Units.FontSizeM;
            OtherCharactersLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            OtherCharactersLabel.Content.TextColor = Color.White;

            InnerContainer.Children.Add(ChangeCharacterLabel.Content);
            //CharacterSelectionContainer.Children.Add(CurrentCharacterLabel.Content);

            CurrentCharacterContainer.Children.Add(CurrentCharacterLabel.Content, 1, 0);
            CurrentCharacterContainer.Children.Add(TapOnCharacterLabel.Content, 0, 1);
            CurrentCharacterContainer.Children.Add(CurrenReward.Content, 1, 1);

            InnerContainer.Children.Add(CurrentCharacterContainer);

            InnerContainer.Children.Add(OtherCharactersLabel.Content);


            int row = 0;
            int col = 0;
            
            foreach(SocialReward reward in AppDataContent.SocialRewards)
            {
                AvailableRewardIconsContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

                if (reward.ImageUrl == AppSession.CurrentUser.Preferences.CurrentCharacterImage)
                {
                    continue;
                }

                StaticImage availableRewardImage = new StaticImage(reward.ImageUrl, 128, null);

                SocialRewardTile availableReward = new SocialRewardTile(reward.ImageUrl, reward.Name);

                availableReward.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               AppSession.CurrentUser.AvatarImageUrl = reward.ImageUrl;
                               AppSession.CurrentUser.Preferences.CurrentCharacterImage = reward.ImageUrl;
                               await Update();
                           });
                       })
                   }
                ); ;

                AvailableRewardIconsContainer.Children.Add(availableReward.Content, col, row);

                col++; if (col >= 2) { row++; col = 0; }

            }
            row++;
            AvailableRewardIconsContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            ColourButton SaveButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.SAVE_CHANGES, null);
            SaveButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               App.ShowAlert("Character changed");
                               await App.PerformActionAsync((int)Actions.ActionName.GoToPage, AppSession.LastPageId);
                               await App.ShowMenu();
                           });
                       })
                   }
               );


            AvailableRewardIconsContainer.Children.Add(SaveButton.Content, 1, row);



            InnerContainer.Children.Add(AvailableRewardIconsContainer);

            CharacterSelectionContainer.Children.Add(InnerContainer);
            


            return CharacterSelectionContainer;
        }

        public override Task Update()
        {
            if (CharacterSelectionContainer != null)
            {
                ContentContainer.Children.Remove(CharacterSelectionContainer);
                BuildCharacterSelectionContainer();
                ContentContainer.Children.Add(CharacterSelectionContainer);
            }
            return base.Update();
        }
    }
}
