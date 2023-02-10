using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Composites;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using ChaiCooking.Services.Storage;
using Xamarin.CommunityToolkit.Core;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class InfluencerBio : Page
    {
        StackLayout ContentContainer;

        bool isBusy;
        StaticLabel UserName;
        StaticLabel IntroInfo;
        StaticLabel BioTitle;
        StaticLabel PicTitle;
        //StaticLabel PlanTitle;
        Editor BioEditor;

        Components.Buttons.ImageButton SaveProfileButton;
        Components.Buttons.ImageButton UploadProfileImageButton;

        Avatar UserAvatar;
        

        private bool PhotoSelected;
        private MediaSource MediaSource;

        string ProfileImageFilePath;

        ILocalMediaStorageManager storageManager;

        public InfluencerBio()
        {
            this.IsScrollable = true;
            this.IsRefreshable = false;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;

            this.Id = (int)AppSettings.PageNames.InfluencerBio;
            this.Name = "Influencer Bio";
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.FadeIn;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.FadeOut;

            isBusy = false;
            PhotoSelected = false;

            PageContent = new Grid
            {
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING)
            };

            ContentContainer = BuildContent();
            PageContent.Children.Add(ContentContainer);
        }

        public StackLayout BuildContent()
        {
            // build labels
            StackLayout mainLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.End,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),// FromHex("eeeeee"),
                WidthRequest = Units.ScreenWidth,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING)
            };

            mainLayout.Children.Clear();

            UserName = new StaticLabel("Your Influencer Profile");
            UserName.Content.TextColor = Color.White;
            UserName.Content.FontSize = Units.FontSizeXXL;
            UserName.LeftAlign();

            UserAvatar = new Avatar("", "characterison.png", Units.ScreenWidth, Units.ScreenWidth);
            UserAvatar.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            UserAvatar.Content.VerticalOptions = LayoutOptions.CenterAndExpand;
            UserAvatar.Icon.Image.Aspect = Aspect.AspectFill;

            if (AppSession.CurrentUser.AvatarImageUrl != null)
            {
                UserAvatar.Icon.Image.Source = AppSession.CurrentUser.AvatarImageUrl;
            }

            UploadProfileImageButton = new Components.Buttons.ImageButton("arrow_right_green_chevron.png", "arrow_right_green_chevron.png", "Change Image", Color.Black, null);
            UploadProfileImageButton.RightAlign();
            UploadProfileImageButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            UploadProfileImageButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;

            TouchEffect.SetNativeAnimation(UploadProfileImageButton.Content, true);
            TouchEffect.SetCommand(UploadProfileImageButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await UploadProfilePictureSelected();
                });
            }));
            /*
            UploadProfileImageButton = new ColourButton(Color.Transparent, Color.FromHex(Colors.CC_ORANGE), "Upload Profile Image, null);
            //UploadProfileImageButton.Label.FontSize = 14;
            UploadProfileImageButton.ButtonShape.BorderColor = Color.FromHex(Colors.CC_GREEN);
            UploadProfileImageButton.ButtonShape.Padding = 2;

            //UploadProfileImageButton.Content.WidthRequest = 140;
            //UploadProfileImageButton.Content.HeightRequest = 48;
            UploadProfileImageButton.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
            UploadProfileImageButton.Content.GestureRecognizers.Add(
                new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await UploadProfilePictureSelected();
                        });
                    })
                }
            );
            */

            BioTitle = new StaticLabel("Influencer Bio");
            BioTitle.Content.TextColor = Color.White;
            BioTitle.Content.FontSize = Units.FontSizeL;
            BioTitle.LeftAlign();

            PicTitle = new StaticLabel("Profile Picture");
            PicTitle.Content.TextColor = Color.White;
            PicTitle.Content.FontSize = Units.FontSizeL;
            PicTitle.LeftAlign();

            //PlanTitle = new StaticLabel("Current plan: " + Accounts.GetAccountName(AppSession.CurrentUser.Preferences.AccountType));
            //PlanTitle.Content.TextColor = Color.White;
            //PlanTitle.Content.FontSize = Units.FontSizeL;
            //PlanTitle.LeftAlign();

            //Had to add some new line breaks to the placeholder as it was not wrapping like the text does.
            BioEditor = new Editor
            {
                Keyboard = Keyboard.Text,
                WidthRequest = Units.ScreenWidth,
                HeightRequest = 240,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.White,
                FontFamily = Fonts.GetRegularAppFont(),
                Placeholder = "Just a little bit about yourself!\nIf you want to publish your own meal plans,\nthis is a great place to describe the kind of\nperson you are and the kind of diets you\nlove!"
            };

            if (AppSession.CurrentUser.Bio != null)
            {
                BioEditor.Text = AppSession.CurrentUser.Bio;
            }

            IntroInfo = new StaticLabel(AppText.QUESTIONNAIRE_INTRO);
            IntroInfo.Content.TextColor = Color.White;
            IntroInfo.Content.FontSize = Units.FontSizeM;
            IntroInfo.Content.Padding = Dimensions.GENERAL_COMPONENT_SPACING;
            IntroInfo.CenterAlign();

            SaveProfileButton = new Components.Buttons.ImageButton("arrow_right_green_chevron.png", "arrow_right_green_chevron.png", AppText.SAVE_CHANGES, Color.Black, null);
            SaveProfileButton.RightAlign();
            SaveProfileButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            SaveProfileButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING;

            TouchEffect.SetNativeAnimation(SaveProfileButton.Content, true);
            TouchEffect.SetCommand(SaveProfileButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (!isBusy)
                    {
                        isBusy = true;
                        PerformSubmitPrefs();
                        //App.ShowAlert("NOT BUSY");
                    }
                    else
                    {
                        //App.ShowAlert("BUSY");
                    }
                });
            }));

            mainLayout.Children.Add(UserName.Content);
            mainLayout.Children.Add(PicTitle.Content);
            mainLayout.Children.Add(UserAvatar.Content);
            mainLayout.Children.Add(UploadProfileImageButton.Content);
            //mainLayout.Children.Add(PlanTitle.Content);
            mainLayout.Children.Add(BioTitle.Content);
            mainLayout.Children.Add(BioEditor);

            mainLayout.Children.Add(SaveProfileButton.Content);

            if (!AppSettings.CutDownQuestionnaire)
            {
                mainLayout.Children.Add(IntroInfo.Content);
            }
            return mainLayout;
        }

        private async void PerformSubmitPrefs()
        {
            if (BioEditor.Text != null)
            {
                AppSession.CurrentUser.Bio = BioEditor.Text;
                LocalDataStore.SaveAll();
            }

            if (await DataManager.UpdateUserPrefs(AppSession.CurrentUser.Preferences.AccountType, false))
            {
                App.ShowAlert("Your information has been saved");
            }
            isBusy = false;
        }

        public async Task UploadProfilePictureSelected()
        {
            var actionSheet = await App.ShowActionSheet("Change Profile Picture", "Cancel", null, new string[] { "Take Photo", "Choose Photo" });
            if (actionSheet == "Cancel")
            {
                return;
            }
            else
            {
                try
                {
                    var usingCamera = actionSheet == "Take Photo";
                    var image = (usingCamera) ? await MediaPicker.CapturePhotoAsync() : await MediaPicker.PickPhotoAsync();
                    var imagePath = (usingCamera) ? await DependencyService.Get<ILocalMediaStorageManager>().getPathForImageAsync(image) : image.FullPath;
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Task.Delay(10);
                        this.ProfileImageFilePath = imagePath;
                        this.UserAvatar.Icon.Image.Source = imagePath;

                        AppSession.CurrentUser.AvatarImageUrl = imagePath;

                    });
                }
                catch (Exception e)
                {
                    if (e is System.NullReferenceException)
                    {
                        Console.WriteLine("user cancelled");
                    }
                    else
                    {
                        App.ShowAlert("An error occured, please try again");
                    }
                }
            }
        }

        public override async Task Update()
        {
            UserPreferences prefs = null;
            try
            {
                prefs = await App.ApiBridge.GetPreferences(AppSession.CurrentUser);
                AppSession.CurrentUser.Preferences = prefs;
                LocalDataStore.SaveAll();
            }
            catch (Exception e)
            {

            }

            if (AppSession.CurrentUser.AvatarImageUrl != null)
            {
                UserAvatar.Icon.Image.Source = AppSession.CurrentUser.AvatarImageUrl;
            }

            await DebugUpdate(AppSettings.TransitionVeryFast);
            await base.Update();
            PageContent.Children.Remove(ContentContainer);
            ContentContainer = BuildContent();
            PageContent.Children.Add(ContentContainer);

            App.SetSubHeaderTitle(AppText.RECOMMENDED_RECIPES, new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.RecommendedRecipes));

            if (AppSession.CurrentUser.Bio != null)
            {
                BioEditor.Text = AppSession.CurrentUser.Bio;
            }
        }
    }
}

