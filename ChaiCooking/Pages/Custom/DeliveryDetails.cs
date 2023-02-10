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
    public class DeliveryDetails : Page
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
        protected InputField AddressField;
        //protected InputField ExpiryDateField;
        //protected InputField CVVNumberField;
        //protected InputField CountryField;
        //protected InputField PostcodeField;

        //protected IconButton FaceBookRegisterButton;
        protected StandardButton CurrentLocationButton;
        protected StandardButton EnterNewAddressButton;
        protected StandardButton SaveButton;


        public DeliveryDetails()
        {
            this.IsScrollable = true;
            this.FullScreen = true;
            //this.Id = (int)AppSettings.PageNames.DeliveryDetails;
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

            Title = new Layouts.Custom.PageTitle("Delivery Details", Color.White, Color.Black);


            CurrentLocationButton = new StandardButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Branding.Colors.PINK), Color.White, "Current Location", new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.TELandingPage));
            EnterNewAddressButton = new StandardButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Branding.Colors.DARK_GREY), Color.White, "Enter a New Address", new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.TELandingPage));
            SaveButton = new StandardButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Branding.Colors.PINK), Color.White, "Save", new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.TELandingPage));

            DividerWithText OrDivider = new DividerWithText(Color.FromHex(Branding.Colors.DARK_GREY), Color.White, 0.5, "OR");
            OrDivider.Content.Margin = Units.ScreenUnitL;

            AddressField = new InputField("Address", "Enter your address", Keyboard.Text, true);

            ContentContainer.Children.Add(Header.Content);
            ContentContainer.Children.Add(Title.Content);

           

            ContentContainer.Children.Add(CurrentLocationButton.Content);
            ContentContainer.Children.Add(OrDivider.Content);
            ContentContainer.Children.Add(EnterNewAddressButton.Content);
            ContentContainer.Children.Add(AddressField.Content);
            ContentContainer.Children.Add(SaveButton.Content);


            PageContent.Children.Add(ContentContainer);
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
