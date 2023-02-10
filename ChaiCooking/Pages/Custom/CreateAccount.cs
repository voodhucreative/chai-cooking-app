using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechExpo.Components.Buttons;
using TechExpo.Components.Composites;
using TechExpo.Components.Fields;
using TechExpo.Components.Images;
using TechExpo.Components.Labels;
using TechExpo.Helpers;
using TechExpo.Models;
using TechExpo.Tools;
using Xamarin.Forms;

namespace TechExpo.Pages
{
    public class CreateAccount : Page
    {
        // declare all components for this page
        StackLayout ContentContainer;

        // top logo
        protected StaticImage Logo;

        // themed header
        protected Layouts.Custom.Header Header;

        // Title
        protected Layouts.Custom.PageTitle Title;

        // Input fields
        protected InputField FirstNameField;
        protected InputField SurnameField;
        protected InputField EmailField;
        protected InputField MobileNumberField;
        protected InputField PasswordField;

        protected IconButton FaceBookRegisterButton;
        protected StandardButton RegisterButton;
        /*
        protected ActiveLabel LabelTestPage1;
        protected ActiveLabel LabelGoToPage;
        protected ActiveLabel LabelGoToNextPage;
        protected ActiveLabel LabelToggleMenu;
        protected ActiveLabel LabelToggleHeader;
        protected ActiveLabel LabelToggleFooter;

        protected StaticImage ImageImage1;
        protected ActiveImage ImageImage2;
        protected ActiveImage ImageImage3;
        */

        public CreateAccount()
        {
            this.IsScrollable = true;
            this.FullScreen = true;
            //this.Id = (int)AppSettings.PageNames.CreateAccount;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.FadeIn;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.FadeOut;

            PageContent = new Grid
            {
                BackgroundColor = Color.White
            };

            // build labels
            ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Start
            };

            Header = new Layouts.Custom.Header(true);

            Title = new Layouts.Custom.PageTitle("Create a Chai profile", Color.White, Color.Black);


            FirstNameField = new InputField("First Name", "Enter your first name", Keyboard.Text, true);
            SurnameField = new InputField("Surname", "Enter your surname", Keyboard.Text, true);
            EmailField = new InputField("E-Mail", "Enter your email address", Keyboard.Email, true);
            MobileNumberField = new InputField("Mobile Number", "Enter your moble number", Keyboard.Numeric, true);
            PasswordField = new InputField("Password", "Enter your password (min 8 characters)", Keyboard.Text, true);
            PasswordField.TextEntry.IsPassword = true;


            FirstNameField.TextEntry.Text = "";
            SurnameField.TextEntry.Text = "";
            EmailField.TextEntry.Text = "";
            MobileNumberField.TextEntry.Text = "";
            PasswordField.TextEntry.Text = "";
    



            FirstNameField.FieldTitle.TextColor = Color.FromHex(Branding.Colors.DARK_GREY);

            FaceBookRegisterButton = new IconButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Branding.Colors.FACEBOOK_BLUE), Color.White, "Register with Facebook", "facebook_icon.png", null);
            RegisterButton = new StandardButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Branding.Colors.PINK), Color.White, "Register", null);

            //RegisterButton.UpAction = new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.CardDetails);
            //FaceBookRegisterButton.UpAction = new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.CardDetails);



            Gestures RegisterGestures = new Gestures();
            RegisterGestures.TouchBegan += (s, e) =>
            {
                RegisterButton.ButtonShape.Color = Color.FromHex(Branding.Colors.DARK_PINK);
            };
            RegisterGestures.TouchEnded += (s, e) =>
            {
                SaveFormData();

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
                FaceBookRegisterButton.ButtonShape.Color = Color.FromHex(Branding.Colors.DARK_FACEBOOK_BLUE);
            };

            FacebookGestures.TouchEnded += (s, e) =>
            {
                FaceBookRegisterButton.ButtonShape.Color = Color.FromHex(Branding.Colors.FACEBOOK_BLUE);
                if (FaceBookRegisterButton.UpAction != null)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await FaceBookRegisterButton.UpAction.Execute();
                    });
                }
                Console.WriteLine("BLAA OFF");
            };
            FaceBookRegisterButton.Content.Children.Add(FacebookGestures, 0, 0);

            /*
            RegisterButton.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            //SaveFormData();

                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.CardDetails);
                                //new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.CardDetails)
                                //await tile.Action.Execute();
                            });
                        })
                    }
                );
                */

            DividerWithText OrDivider = new DividerWithText(Color.FromHex(Branding.Colors.DARK_GREY), Color.White, 0.5 , "OR");

            ContentContainer.Children.Add(Header.Content);
            ContentContainer.Children.Add(Title.Content);

            ContentContainer.Children.Add(FirstNameField.Content);
            ContentContainer.Children.Add(SurnameField.Content);
            ContentContainer.Children.Add(EmailField.Content);
            ContentContainer.Children.Add(MobileNumberField.Content);
            ContentContainer.Children.Add(PasswordField.Content);

            ContentContainer.Children.Add(RegisterButton.Content);
            ContentContainer.Children.Add(OrDivider.Content);
            ContentContainer.Children.Add(FaceBookRegisterButton.Content);


            /*
            LabelTestPage1 = new ActiveLabel("Test Page 1", 48, FontName.MontserratBold, Color.Transparent, Color.Black, new Models.Action((int)Actions.ActionName.None, -1));

            LabelGoToPage = new ActiveLabel("Go To Page", 24, FontName.MontserratBold, Color.Transparent, Color.Blue, new Models.Action((int)Actions.ActionName.GoToPage, -1));

            LabelGoToNextPage = new ActiveLabel(
                    "Go To Next Page",
                    24,
                    FontName.MontserratBold,
                    Color.Transparent,
                    Color.Blue,
                    new Models.Action((int)Actions.ActionName.GoToNextPage, (int)AppSettings.PageNames.CreateAccount));

            LabelToggleMenu = new ActiveLabel("Toggle Menu", 24, FontName.MontserratBold, Color.Transparent, Color.Blue, new Models.Action((int)Actions.ActionName.ToggleMenu, -1));

            LabelToggleHeader = new ActiveLabel("Toggle Header", 24, FontName.MontserratBold, Color.Transparent, Color.Blue, new Models.Action((int)Actions.ActionName.ToggleHeader, -1));
           
            LabelToggleFooter = new ActiveLabel("Toggle Footer", 24, FontName.MontserratBold, Color.Transparent, Color.Blue, new Models.Action((int)Actions.ActionName.ToggleFooter, -1));

            // build images
            ImageImage1 = new StaticImage(
                    "test.jpg",
                    Units.ScreenWidth45Percent,
                    Units.ScreenWidth30Percent,
                    new List<FFImageLoading.Work.ITransformation>()
                    {
                        new FFImageLoading.Transformations.BlurredTransformation(50)
                    });

            ImageImage2 = new ActiveImage(
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

            // add images
            LabelsContainer.Children.Add(ImageImage1.Content);
            LabelsContainer.Children.Add(ImageImage2.GetContent());
            LabelsContainer.Children.Add(ImageImage3.GetContent());

            */

            PageContent.Children.Add(ContentContainer);
        }

        private void SaveFormData()
        {
            Console.WriteLine("First Name: " + FirstNameField.TextEntry.Text);
            Console.WriteLine("Surname: " + SurnameField.TextEntry.Text);
            Console.WriteLine("Email: " + EmailField.TextEntry.Text);
            Console.WriteLine("Mobile Number: " + MobileNumberField.TextEntry.Text);
            Console.WriteLine("Password: " + PasswordField.TextEntry.Text);

            AppSession.CurrentUser.FirstName = FirstNameField.TextEntry.Text;
            AppSession.CurrentUser.LastName = SurnameField.TextEntry.Text;
            AppSession.CurrentUser.EmailAddress = EmailField.TextEntry.Text;
            AppSession.CurrentUser.MobileNumber = MobileNumberField.TextEntry.Text;
            AppSession.CurrentUser.Password = PasswordField.TextEntry.Text;

            Address address = new Address();
            address.AddressLine1 = "1 Test Street";
            address.AddressLine2 = "Testville";
            address.County = "Testishire";
            address.Country = "Testland";
            address.AreaCode = "TE57 1NG";

            AppSession.CurrentUser.Address = address;

            // LocalStorage.SaveUser();

        }

        public override void Destroy()
        {

        }

        public override async Task Update()
        {
            await DebugUpdate(500);
        }

        public override async Task DebugUpdate(int time)
        {
            await Task.Delay(time);

            /*
            ContentContainer.Children.Clear();

            // add labels
            LabelsContainer.Children.Add(LabelTestPage1.Content);

            // add images
            LabelsContainer.Children.Add(ImageImage1.Content);
            LabelsContainer.Children.Add(LabelGoToPage.Content);

            LabelsContainer.Children.Add(ImageImage2.GetContent());

            LabelsContainer.Children.Add(LabelGoToNextPage.Content);
            LabelsContainer.Children.Add(ImageImage3.GetContent());
            LabelsContainer.Children.Add(LabelToggleMenu.Content);
            LabelsContainer.Children.Add(LabelToggleHeader.Content);
            LabelsContainer.Children.Add(LabelToggleFooter.Content);

            */



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
