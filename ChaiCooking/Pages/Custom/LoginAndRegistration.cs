using System;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Images;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom;
using ChaiCooking.Layouts.Custom.Panels;
using ChaiCooking.Layouts.Custom.Panels.Account;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models;
using ChaiCooking.Services;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class LoginAndRegistration : Page
    {
        StackLayout ContentContainer;
        StackLayout MainLayout;

        TabbedPanel LoginCreatePanel;
        GeneralInfoPanel LoginPanel;
        GeneralInfoPanel CreateAccountPanel;
        GeneralInfoPanel VerifyAccountPanel;
        StaticImage Logo;

        public LoginAndRegistration()
        {
            this.IsScrollable = true;
            this.IsRefreshable = true;
            this.HasHeader = false;// true;
            this.HasSubHeader = false;// true;
            this.HasNavHeader = false;
            this.HasFooter = false;

            this.Id = (int)AppSettings.PageNames.LoginAndRegistration;
            this.Name = "Login And Registration";
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.FadeIn;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.FadeOut;


            PageContent = new Grid
            {
                BackgroundColor = Color.Transparent
            };

            // add a background?
            //AddBackgroundImage("pagebg.jpg");

            ContentContainer = BuildContent();
            PageContent.Children.Add(ContentContainer);
        }

        public StackLayout BuildContent()
        {
            // build labels
            MainLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.Transparent,// FromHex("eeeeee"),
                WidthRequest = Units.ScreenWidth,
                Spacing = Dimensions.GENERAL_COMPONENT_SPACING,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING
            };

            Logo = new StaticImage("chailogo.png", Units.ScreenWidth, null);
            Logo.Content.HeightRequest = 64;

            LoginCreatePanel = new TabbedPanel();
            LoginPanel = new GeneralInfoPanel("Login", Themes.PANEL_THEME_ORANGE_AND_WHITE, Dimensions.INFO_PANEL_WIDTH);
            CreateAccountPanel = new GeneralInfoPanel("Confirm your\nChai account", Themes.PANEL_THEME_ORANGE_AND_WHITE, Dimensions.INFO_PANEL_WIDTH);
         
            VerifyAccountPanel = new GeneralInfoPanel("Verify", Themes.PANEL_THEME_ORANGE_AND_WHITE, Dimensions.INFO_PANEL_WIDTH);

            LoginPanel.AddContent(new LoginPanel().GetContent());
            CreateAccountPanel.AddContent(new CreateAccountPanel().GetContent());

            VerifyAccountPanel.AddContent(new VerifyAccountPanel().GetContent());
            VerifyAccountPanel.Content.IsVisible = false;

            LoginCreatePanel.AddChildPanel(LoginPanel);
            LoginCreatePanel.AddChildPanel(CreateAccountPanel);

            MainLayout.Children.Add(Logo.Content);
            MainLayout.Children.Add(LoginCreatePanel.GetContent());


            Device.BeginInvokeOnMainThread(async () =>
            {
                await ShowLoginAndCreateAccountPanel(AppSession.CurrentUser.IsRegistered);
            });

            return MainLayout;
        }

        public async Task ShowLoginAndCreateAccountPanel(bool isRegisterd)
        {
            await Task.Delay(100);

            MainLayout.Children.Add(LoginCreatePanel.GetContent());

            if (isRegisterd)
            {
                Console.WriteLine("This user is already registered, so show the login panel");
            }
            else
            {
                Console.WriteLine("This user is not registered, so show the create user panel");
            }
        }

        public async Task ShowAccountConfirmationPanel()
        {
            await Task.Delay(100);
            MainLayout.Children.Remove(LoginCreatePanel.GetContent());
        }

        public async Task ShowVerifyAccountPanel()
        {
            await Task.Delay(100);

            MainLayout.Children.Add(VerifyAccountPanel.GetContent());
            VerifyAccountPanel.Content.IsVisible = true;
        }

        public async Task HideVerifyAccountPanel()
        {
            await Task.Delay(100);
            VerifyAccountPanel.Content.IsVisible = false;
            MainLayout.Children.Remove(VerifyAccountPanel.GetContent());
            MainLayout.Children.Add(LoginCreatePanel.GetContent());
        }

        public override async Task Update()
        {
            if (this.NeedsRefreshing)
            {
                await DebugUpdate(AppSettings.TransitionVeryFast);
                await base.Update();
                PageContent.Children.Remove(ContentContainer);
                ContentContainer = BuildContent();
                PageContent.Children.Add(ContentContainer);

                LoginCreatePanel.ClearChildPanels();

                //Keeps the UI order no matter if we are registered or not
                LoginCreatePanel.AddChildPanel(LoginPanel);
                LoginCreatePanel.AddChildPanel(CreateAccountPanel);

                //Sets the selected pannel to create account if we have not yet registered
                if (!AppSession.CurrentUser.IsRegistered)
                {
                    //Shows a popup directing user to create thier chai account to link to whisk
                    //await App.DisplayAlert("CHAI is powered by Whisk", "Please create a Whisk / CHAI user for the nutritional benefits of meal planning.", "Ok");
                    //DependencyService.Get<IMessage>().LongAlert("CHAI is powered by Whisk; create a Whisk / CHAI user for the nutritional benefits of meal planning.");

                    LoginCreatePanel.SetTargetPanel(CreateAccountPanel);
                }
                
                App.ShowMenuButton();
                App.ShowBackgroundImage();
            }
        }
    }
}