using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechExpo.Components.Buttons;
using TechExpo.Components.Images;
using TechExpo.Components.Labels;
using TechExpo.Helpers;
using TechExpo.Tools;
using Xamarin.Forms;

namespace TechExpo.Pages
{
    public class Home : Page
    {
        // declare all components for this page
        StackLayout LabelsContainer;

        //protected ActiveLabel LabelTestPage1;
        //protected ActiveLabel LabelGoToPage;
        //protected ActiveLabel LabelGoToNextPage;
        //protected ActiveLabel LabelToggleMenu;
        //protected ActiveLabel LabelToggleHeader;
        //protected ActiveLabel LabelToggleFooter;

        protected StaticImage MainLogoImage;

        protected IconButton FaceBookLoginButton;
        protected StandardButton RegisterButton;

        //protected ActiveImage ImageImage2;
        protected ActiveImage ImageImage3;

        public Home()
        {
            this.IsScrollable = false;
            this.FullScreen = true;
            //this.Id = (int)AppSettings.PageNames.HomeScreen;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.FadeIn;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.FadeOut;

            PageContent = new Grid
            {
                BackgroundColor = Color.White
            };

            // add a background
            AddBackgroundImage("background_img.png");

            // build labels
            LabelsContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Start
            };

            //LabelTestPage1 = new ActiveLabel("Test Page 1", 48, FontName.MontserratBold, Color.Transparent, Color.Black, new Models.Action((int)Actions.ActionName.None, -1));

            //LabelGoToPage = new ActiveLabel("Go To Page", 24, FontName.MontserratBold, Color.Transparent, Color.Blue, new Models.Action((int)Actions.ActionName.GoToPage, -1));

            //LabelGoToNextPage = new ActiveLabel(
            //        "Go To Next Page",
            //        24,
            //        FontName.MontserratBold,
            //        Color.Transparent,
            //        Color.Blue,
            //        new Models.Action((int)Actions.ActionName.GoToNextPage, (int)AppSettings.PageNames.CreateAccount));

            //LabelToggleMenu = new ActiveLabel("Toggle Menu", 24, FontName.MontserratBold, Color.Transparent, Color.Blue, new Models.Action((int)Actions.ActionName.ToggleMenu, -1));

            //LabelToggleHeader = new ActiveLabel("Toggle Header", 24, FontName.MontserratBold, Color.Transparent, Color.Blue, new Models.Action((int)Actions.ActionName.ToggleHeader, -1));

            //LabelToggleFooter = new ActiveLabel("Toggle Footer", 24, FontName.MontserratBold, Color.Transparent, Color.Blue, new Models.Action((int)Actions.ActionName.ToggleFooter, -1));

            // build images
            MainLogoImage = new StaticImage(
                    "logo.png",
                    Units.ScreenWidth45Percent,
                    Units.ScreenWidth30Percent,
                    null);

            MainLogoImage.Content.Margin = new Thickness(Units.ScreenWidth10Percent, Units.ScreenHeight10Percent, Units.ScreenWidth10Percent, Units.ScreenHeight40Percent);

            FaceBookLoginButton = new IconButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Branding.Colors.FACEBOOK_BLUE), Color.White, "Log in with Facebook", "facebook_icon.png", new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.TELandingPage));
            //FaceBookLoginButton.UpAction = new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.CreateAccount);

            RegisterButton = new StandardButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Branding.Colors.PINK), Color.White, "Register", new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.TELandingPage));
            //RegisterButton.UpAction = new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.CreateAccount);


            /*ImageImage2 = new ActiveImage(
                    "test.jpg",
                    Units.ScreenWidth45Percent,
                    Units.ScreenWidth45Percent,
                    new List<FFImageLoading.Work.ITransformation>()
                    {
                        new FFImageLoading.Transformations.GrayscaleTransformation(),
                        //new FFImageLoading.Transformations.BlurredTransformation(40),
                        new FFImageLoading.Transformations.RoundedTransformation(40),
                        //new FFImageLoading.Transformations.RotateTransformation(45)
                    },
                    new Models.Action((int)Actions.ActionName.ToggleHeader, -1));

            Gestures ImageImage3Gestures = new Gestures
            {
                //AnimationEffect = Gestures.AnimationType.atScaling,
                //AnimationScale = -8,
                //AnimationSpeed = 25,

                AnimationEffect = Gestures.AnimationType.atFlashing,
                AnimationColor = Color.FromRgba(0, 0, 0, 50),
                AnimationSpeed = 500,

                BackgroundColor = Color.Transparent,
                SwipeEnabled = false,
                DragEnabled = true,

                //WidthRequest = Units.ScreenWidth25Percent,
                //HeightRequest = Units.ScreenWidth40Percent,
                GestureWidth = Units.ScreenWidth25Percent,
                GestureHeight = Units.ScreenWidth25Percent
            };

            ImageImage3 = new ActiveImage("test.jpg", Units.ScreenWidth25Percent, Units.ScreenWidth25Percent, null, new Models.Action((int)Actions.ActionName.ToggleHeader, -1));

            ImageImage3Gestures.TouchBegan += (s, e) =>
            {

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await ImageImage3.Content.TranslateTo(0, 0, 0, Easing.Linear);
                    //await Task.WhenAll(ImageImage3.Content.FadeTo(0.5, 50, Easing.Linear));
                    //await Task.Delay(250);
                    //await Task.WhenAll(ImageImage3.Content.FadeTo(1.0, 50, Easing.Linear));

                    //await App.PerformActionAsync(new Models.Action((int)Actions.ActionName.GoToNextPage, (int)AppSettings.PageNames.Page2));


                });
            };

            ImageImage3Gestures.Drag += (s, e) =>
            {
                Device.BeginInvokeOnMainThread(async () => {
                    //await ImageImage3Gestures.TranslateTo(e.DistanceX, ImageImage3Gestures.Y, 50, Easing.Linear);
                   
                });

            };


            ImageImage3Gestures.SwipeLeft += (s, e) =>
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await ImageImage3.Content.TranslateTo(-100, 0, 100, Easing.Linear); 
                   });
            };

            ImageImage3Gestures.SwipeRight += (s, e) =>
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await ImageImage3.Content.TranslateTo(100, 0, 100, Easing.Linear);
                });
            };

            ImageImage3.AddGestures(ImageImage3Gestures);

            // build buttons

            // etc


            // add labels
            LabelsContainer.Children.Add(LabelTestPage1.Content);
            LabelsContainer.Children.Add(LabelGoToPage.Content);
            LabelsContainer.Children.Add(LabelGoToNextPage.Content);
            LabelsContainer.Children.Add(LabelToggleMenu.Content);
            LabelsContainer.Children.Add(LabelToggleHeader.Content);
            LabelsContainer.Children.Add(LabelToggleFooter.Content);
            */

            Gestures RegisterGestures = new Gestures();
            RegisterGestures.TouchBegan += (s, e) =>
            {
                RegisterButton.ButtonShape.Color = Color.FromHex(Branding.Colors.DARK_PINK);
            };
            RegisterGestures.TouchEnded += (s, e) =>
            {
                RegisterButton.ButtonShape.Color = Color.FromHex(Branding.Colors.PINK);
                if (RegisterButton.UpAction != null)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await RegisterButton.UpAction.Execute();
                    });
                }
            };
            RegisterButton.Content.Children.Add(RegisterGestures, 0, 0);





            Gestures FacebookGestures = new Gestures();
            FacebookGestures.TouchBegan += (s, e) =>
            {
                FaceBookLoginButton.ButtonShape.Color = Color.FromHex(Branding.Colors.DARK_FACEBOOK_BLUE);
            };

            FacebookGestures.TouchEnded += (s, e) =>
            {
                FaceBookLoginButton.ButtonShape.Color = Color.FromHex(Branding.Colors.FACEBOOK_BLUE);
                if (FaceBookLoginButton.UpAction != null)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await FaceBookLoginButton.UpAction.Execute();
                    });
                }
                Console.WriteLine("BLAA OFF");
            };
            FaceBookLoginButton.Content.Children.Add(FacebookGestures, 0, 0);



            // add images
            LabelsContainer.Children.Add(MainLogoImage.Content);
            LabelsContainer.Children.Add(FaceBookLoginButton.Content);
            LabelsContainer.Children.Add(RegisterButton.Content);
            PageContent.Children.Add(LabelsContainer);
        }

        public override void Destroy()
        {

        }

        public override async Task Update()
        {
            await DebugUpdate(AppSettings.TransitionVeryFast);
        }

        public override async Task DebugUpdate(int time)
        {
            await Task.Delay(time);

            LabelsContainer.Children.Clear();

            // add labels
            //LabelsContainer.Children.Add(LabelTestPage1.Content);

            // add images
            LabelsContainer.Children.Add(MainLogoImage.Content);


            LabelsContainer.Children.Add(FaceBookLoginButton.Content);
            LabelsContainer.Children.Add(RegisterButton.Content);
            //LabelsContainer.Children.Add(LabelGoToPage.Content);

            //LabelsContainer.Children.Add(ImageImage2.GetContent());

            //LabelsContainer.Children.Add(LabelGoToNextPage.Content);
            //LabelsContainer.Children.Add(ImageImage3.GetContent());
            //LabelsContainer.Children.Add(LabelToggleMenu.Content);
            //LabelsContainer.Children.Add(LabelToggleHeader.Content);
            //LabelsContainer.Children.Add(LabelToggleFooter.Content);





        }

        // Override these functions for any page specific transitions. 
        // Otherwise the default base Page class methods will be called instead.

        /*
        public override async Task TransitionIn()
        {
            await Task.WhenAll(
                PageContent.FadeTo(1, 500, Easing.Linear)
            ).ConfigureAwait(false);
        }

        public override async Task TransitionOut()
        {
            await Task.WhenAll(
                PageContent.FadeTo(0, 500, Easing.Linear)
            ).ConfigureAwait(false);
        }*/
    }
}
