using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechExpo.Components.Buttons;
using TechExpo.Components.Composites;
using TechExpo.Components.Fields;
using TechExpo.Components.Images;
using TechExpo.Components.Labels;
using TechExpo.Helpers;
using TechExpo.Tools;
using Xamarin.Forms;

namespace TechExpo.Pages
{
    public class CreateCard : Page
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
        protected InputField CardNumberField;
        protected InputField ExpiryDateField;
        protected InputField CVVNumberField;
        protected InputField CountryField;
        protected InputField PostcodeField;

        //protected IconButton FaceBookRegisterButton;
        protected StandardButton NextButton;


        public CreateCard()
        {
            this.IsScrollable = true;
            this.FullScreen = true;
            //this.Id = (int)AppSettings.PageNames.CardDetails;
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

            Header = new Layouts.Custom.Header(false);

            Title = new Layouts.Custom.PageTitle("Add Card Details", Color.White, Color.Black);


            CardNumberField = new InputField("Card Number", "Enter your card number", Keyboard.Text, true);
            ExpiryDateField = new InputField("Expiry Date", "Enter expiry date", Keyboard.Text, true);
            CVVNumberField = new InputField("CVV", "Enter your cvv number", Keyboard.Email, true);
            CountryField = new InputField("Country", "Enter your country", Keyboard.Numeric, true);
            PostcodeField = new InputField("Postcode", "Enter your postcode", Keyboard.Text, true);

            CardNumberField.TextEntry.IsPassword = true;

            CardNumberField.FieldTitle.TextColor = Color.FromHex(Branding.Colors.DARK_GREY);


            CardNumberField.TextEntry.Text = "111111111111";
            ExpiryDateField.TextEntry.Text = "11/21";
            CVVNumberField.TextEntry.Text = "222";
            CountryField.TextEntry.Text = "UK";
            PostcodeField.TextEntry.Text = "AB12 3CD";


            NextButton = new StandardButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Branding.Colors.PINK), Color.White, "Next", null);
            //NextButton.UpAction = new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Welcome);

            Gestures NextGestures = new Gestures();
            NextGestures.TouchBegan += (s, e) =>
            {
                NextButton.ButtonShape.Color = Color.FromHex(Branding.Colors.DARK_PINK);
            };
            NextGestures.TouchEnded += (s, e) =>
            {
                NextButton.ButtonShape.Color = Color.FromHex(Branding.Colors.PINK);
                if (NextButton.UpAction != null)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await NextButton.UpAction.Execute();
                    });
                }
            };
            NextButton.Content.Children.Add(NextGestures, 0, 0);

            ContentContainer.Children.Add(Header.Content);
            ContentContainer.Children.Add(Title.Content);

            ContentContainer.Children.Add(CardNumberField.Content);
            ContentContainer.Children.Add(ExpiryDateField.Content);
            ContentContainer.Children.Add(CVVNumberField.Content);
            ContentContainer.Children.Add(CountryField.Content);
            ContentContainer.Children.Add(PostcodeField.Content);

            ContentContainer.Children.Add(NextButton.Content);

            PageContent.Children.Add(ContentContainer);
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
