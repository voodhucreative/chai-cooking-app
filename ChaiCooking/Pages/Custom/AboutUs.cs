using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Composites;
using ChaiCooking.Components.Labels;
using ChaiCooking.DebugData.Custom;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts;
using ChaiCooking.Layouts.Custom;
using ChaiCooking.Layouts.Custom.Panels;
using ChaiCooking.Layouts.Custom.Panels.Account;
using ChaiCooking.Layouts.Custom.Tiles;
using ChaiCooking.Models.Custom;
using ChaiCooking.Services;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class AboutUs : Page
    {
        StackLayout ContentContainer;
        StackLayout UpdatableElementsContainer;
        int timesRefreshed;

        public AboutUs()
        {
            this.IsScrollable = true;
            this.IsRefreshable = true;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = true;

            this.Id = (int)AppSettings.PageNames.AboutUs;
            this.Name = "Sand Box";
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.FadeIn;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.FadeOut;
            timesRefreshed = 0;
            PageContent = new Grid
            {
                BackgroundColor = Color.Transparent
            };

            // add a background?
            //AddBackgroundImage("pagebg.jpg");

            // customise
            if(this.RefreshView != null)
            {
                this.RefreshView.RefreshColor = Color.FromHex(Colors.CC_GREEN);
            }
           
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
                BackgroundColor = Color.Transparent,// FromHex("eeeeee"),
                WidthRequest = Units.ScreenWidth,
                //HeightRequest = Units.ScreenHeight
            };

            mainLayout.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               App.ScaleUpBackground();
                               //await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Landing);
                           });
                       })
                   }
               );

            UpdatableElementsContainer = new StackLayout();
            UpdatableElementsContainer.Padding = Units.ScreenUnitM;



            AlbumTile TestAlbumTile = new AlbumTile(true, Dimensions.ALBUM_TILE_WIDTH, Dimensions.ALBUM_TILE_HEIGHT);
            AlbumTile TestAlbumTile1 = new AlbumTile(true, Dimensions.ALBUM_TILE_WIDTH, Dimensions.ALBUM_TILE_HEIGHT);
            AlbumTile TestAlbumTile2 = new AlbumTile(true, Dimensions.ALBUM_TILE_WIDTH, Dimensions.ALBUM_TILE_HEIGHT);
            AlbumTile TestAlbumTile3 = new AlbumTile(true, Dimensions.ALBUM_TILE_WIDTH, Dimensions.ALBUM_TILE_HEIGHT);

            //TestAlbumTile.AddContent(new Label{ Text = "Test 1"});
            //TestAlbumTile1.AddContent(new Label { Text = "Test 2" });
            TestAlbumTile2.AddContent(new Label { Text = "Test 3" });
            TestAlbumTile2.AddContent(new Label { Text = "Test 4" });

            Recipe r = null;
            TestAlbumTile1.SetName("Matthew Howlett");
            TestAlbumTile1.SetRecipe(r, 80, 80);

            ValueAdjuster valueTest = new ValueAdjuster("Height (m)", 100, 32);
            valueTest.SetFloatValueRange(1.0f, 3.0f, 1.75f, 0.01f);

            ValueAdjuster valueTest1 = new ValueAdjuster("Current Weight (kg)", 100, 32);
            valueTest1.SetFloatValueRange(20.0f, 1000.0f, 80.0f, 1.0f);

            ValueAdjuster valueTest2 = new ValueAdjuster("Target Weight (kg)", 100, 32);
            valueTest2.SetFloatValueRange(20.0f, 1000.0f, 80.0f, 1.0f);

            // put this in a custom class

            Section TestSectionParent = new Section("Parent Section", true, false);
            TestSectionParent.Toggle.LeftAlign();

            StackLayout SectionContainer = new StackLayout { Orientation = StackOrientation.Vertical,HorizontalOptions = LayoutOptions.CenterAndExpand };

            Section TestSection1 = new Section("Test Section 1", true, true);
            TestSection1.AddContent(new Xamarin.Forms.Image { Source = "chaismallbag.png" });
            TestSection1.Toggle.LeftAlign();

            Section TestSection2 = new Section("Test Section 2", true, true);
            TestSection2.AddContent(new Xamarin.Forms.Image { Source = "chaismallbag.png" });
            TestSection2.Toggle.RightAlign();

            Section TestSection3 = new Section("Test Section 3", false, false);
            TestSection3.AddContent(new Xamarin.Forms.Image { Source = "chaismallbag.png" });
            TestSection3.Toggle.CenterAlign();
            TestSection3.SetBorder(2, Color.Yellow);
            TestSection3.HideTitle();

            SearchField SearchField = new SearchField("test");

            IconLabel TestLabel = new IconLabel("icon.png", "Test Icon Label", 240, 32);

            Components.Composites.CheckBox TestCheckBox = new Components.Composites.CheckBox("Test Checkbox", "chaismallbag.png", "icon.png", 240, 32, true);
            SelectLabel TestSelectLabel = new SelectLabel("Test Select Label", Color.FromHex(Colors.CC_ORANGE), Color.White, 240, 32, false, true);

            Avatar TestAvatar = new Avatar(AppSession.CurrentUser.Username, "drnow.jpg", 140, 140);

            SectionContainer.Children.Add(TestAvatar.Content);
            SectionContainer.Children.Add(TestSelectLabel.Content);
            SectionContainer.Children.Add(TestCheckBox.Content);
            SectionContainer.Children.Add(TestLabel.Content);


            SectionContainer.Children.Add(valueTest.GetContent());
            SectionContainer.Children.Add(valueTest1.GetContent());
            SectionContainer.Children.Add(valueTest2.GetContent());

            SectionContainer.Children.Add(TestAlbumTile.GetContent());

            StackLayout albumsContainer = new StackLayout { HorizontalOptions = LayoutOptions.CenterAndExpand, Orientation = StackOrientation.Horizontal, Spacing = Dimensions.GENERAL_COMPONENT_PADDING, Margin = new Thickness(0, 32) };
            albumsContainer.Children.Add(TestAlbumTile1.GetContent());
            albumsContainer.Children.Add(TestAlbumTile2.GetContent());
            albumsContainer.Children.Add(TestAlbumTile3.GetContent());

            SectionContainer.Children.Add(albumsContainer);



            SectionContainer.Children.Add(SearchField.Content); 
            SectionContainer.Children.Add(TestSection1.Content);
            //SectionContainer.Children.Add(TestSection2.Content);
            SectionContainer.Children.Add(TestSection3.Content);


            TestSectionParent.AddContent(SectionContainer);

            /*
            StandardButton testButton = new StandardButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Colors.BH_LIGHT_BLUE), Color.White, "Home", new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Landing));
            StandardButton toggleHeaderButton = new StandardButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Colors.BH_DARK_BLUE), Color.White, "Toggle Header", Actions.GetAction((int)Actions.ActionName.ToggleHeader));
            StandardButton toggleSubHeaderButton = new StandardButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Colors.BH_GOLD), Color.White, "Toggle SubHeader", Actions.GetAction((int)Actions.ActionName.ToggleSubHeader));
            StandardButton toggleNavHeaderButton = new StandardButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Colors.BH_PINK), Color.White, "Toggle NavHeader", Actions.GetAction((int)Actions.ActionName.ToggleNavHeader));
            StandardButton toggleNavMenuButton = new StandardButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Colors.BH_PINK), Color.White, "Toggle Menu", Actions.GetAction((int)Actions.ActionName.ToggleMenu));
            StandardButton toggleModalButton = new StandardButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Colors.BH_PINK), Color.White, "Toggle Modal", Actions.GetAction((int)Actions.ActionName.ToggleModal));
            StandardButton togglePanelButton = new StandardButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Colors.BH_PINK), Color.White, "Toggle Panel", Actions.GetAction((int)Actions.ActionName.TogglePanel));
            StandardButton toggleFooterButton = new StandardButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Colors.BH_PINK), Color.White, "Toggle Footer", Actions.GetAction((int)Actions.ActionName.ToggleFooter));
            StandardButton toggleForegroundButton = new StandardButton(Units.LargeButtonWidth, Units.LargeButtonHeight, Color.FromHex(Colors.BH_PINK), Color.White, "Toggle Foreground", Actions.GetAction((int)Actions.ActionName.ToggleForeground));
            */

            //ColourButton TestButton = new ColourButton(Color.FromHex(Colors.CC_LIGHT_GREY), Color.White, "Test Button Round", null);
            ColourButton testButton = new ColourButton(Color.FromHex(Colors.CC_LIGHT_BLUE), Color.White, "Home", new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Landing));
            ColourButton toggleHeaderButton = new ColourButton(Color.FromHex(Colors.CC_MUSTARD), Color.White, "Toggle Header", Actions.GetAction((int)Actions.ActionName.ToggleHeader));
            ColourButton toggleSubHeaderButton = new ColourButton(Color.FromHex(Colors.CC_GREEN), Color.White, "Toggle Subheader", Actions.GetAction((int)Actions.ActionName.ToggleSubHeader));
            ColourButton toggleNavHeaderButton = new ColourButton(Color.FromHex(Colors.CC_BLUE_GREY), Color.White, "Toggle NavHeader", Actions.GetAction((int)Actions.ActionName.ToggleNavHeader));
            ColourButton toggleNavMenuButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Toggle Menu", Actions.GetAction((int)Actions.ActionName.ToggleMenu));
            ColourButton toggleModalButton = new ColourButton(Color.FromHex(Colors.CC_WARNING_RED), Color.White, "Toggle Modal", Actions.GetAction((int)Actions.ActionName.ToggleModal));
            ColourButton togglePanelButton = new ColourButton(Color.FromHex(Colors.CC_RED), Color.White, "Toggle Panel", Actions.GetAction((int)Actions.ActionName.TogglePanel));
            ColourButton toggleFooterButton = new ColourButton(Color.FromHex(Colors.CC_BLACK), Color.White, "Toggle Footer", Actions.GetAction((int)Actions.ActionName.ToggleFooter));
            ColourButton toggleForegroundButton = new ColourButton(Color.FromHex(Colors.CC_DARK_BLUE_GREY), Color.White, "Toggle Foreground", Actions.GetAction((int)Actions.ActionName.ToggleForeground));

            


            for (int i = 0; i < 6; i++)
            {

                UpdatableElementsContainer.Children.Add(new Label { Text = "test" , TextColor = Color.White, FontSize = Units.FontSizeL });
            }

            timesRefreshed = 0;


            mainLayout.Children.Add(UpdatableElementsContainer);

            RemovableLabel removableLabel = new RemovableLabel(Color.Red, Color.White, "Remove me", null);


            ValueAdjuster staticIntValuesAdjuster = new ValueAdjuster("Int Adjuster", 100, 32);
            staticIntValuesAdjuster.SetIntValues(new List<KeyValuePair<string, int>>()
            {
                new KeyValuePair<string, int>("one", 1),
                new KeyValuePair<string, int>("two", 2),
                new KeyValuePair<string, int>("three", 3),
                new KeyValuePair<string, int>("four", 4),
                new KeyValuePair<string, int>("five", 5),
                new KeyValuePair<string, int>("six", 6),
            });

            mainLayout.Children.Add(staticIntValuesAdjuster.GetContent());


            ValueAdjuster staticStringAdjuster = new ValueAdjuster("String Adjuster", 100, 32);
            staticStringAdjuster.SetStringValues(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("One K", "One V"),
                new KeyValuePair<string, string>("Two K", "Two V"),
                new KeyValuePair<string, string>("Three K", "Three V"),
                new KeyValuePair<string, string>("Four K", "Four V"),
                new KeyValuePair<string, string>("Five K", "Five V"),
            });
            mainLayout.Children.Add(staticStringAdjuster.GetContent());


            GeneralInfoPanel infoPanel1 = new GeneralInfoPanel("Test Info Panel", Themes.PANEL_THEME_GREY, Dimensions.INFO_PANEL_WIDTH);

            infoPanel1.AddButton("hello", 0, null, false);
            infoPanel1.AddButton("oh, hi!", 0, null, true);
            infoPanel1.AddButton("goodbye", 0, Actions.GetAction((int)Actions.ActionName.ToggleHeader), true);
            infoPanel1.ActivateButton(2);
            //infoPanel1.DeactivateButton(2);
            infoPanel1.HideHeader();
            infoPanel1.ShowHeader();
            infoPanel1.SetPanelBorder(2, Color.Black, 0.5f, false);
            infoPanel1.AddContent(TestSection2.Content);


            TabbedPanel loginCreatePanel = new TabbedPanel();
            GeneralInfoPanel loginPanel = new GeneralInfoPanel("Login", Themes.PANEL_THEME_ORANGE_AND_WHITE, Dimensions.INFO_PANEL_WIDTH);
            GeneralInfoPanel createAccountPanel = new GeneralInfoPanel("Create an\naccount", Themes.PANEL_THEME_ORANGE_AND_WHITE, Dimensions.INFO_PANEL_WIDTH);

            GeneralInfoPanel forgotPasswordPanel = new GeneralInfoPanel("Forgot Password", Themes.PANEL_THEME_ORANGE_AND_WHITE, Dimensions.INFO_PANEL_WIDTH);
            //loginPanel.AddContent(TestSection2.Content);
            //createAccountPanel.AddContent(TestSection3.Content);


            //loginCreatePanel.AddChildPanel(infoPanel2);
            //createAccountPanel.AddChildPanel(infoPanel3);

            loginPanel.AddContent(new LoginPanel().GetContent());
            createAccountPanel.AddContent(new CreateAccountPanel().GetContent());

            //forgotPasswordPanel.AddContent(new ForgotPasswordPanel().GetContent());

            //forgotPasswordPanel.AddContent(new IncorrectPasswordPanel().GetContent());
            forgotPasswordPanel.AddContent(new PasswordSentPanel().GetContent());
            

            loginCreatePanel.AddChildPanel(loginPanel);
            loginCreatePanel.AddChildPanel(createAccountPanel);
            //loginCreatePanel.AddChildPanel(forgotPasswordPanel);

            mainLayout.Children.Add(forgotPasswordPanel.GetContent());

            mainLayout.Children.Add(new LargeRecipeTile(DataManager.GetSingleRecipe(AppSession.SelectedRecipe.Id)).GetContent());

            mainLayout.Children.Add(loginCreatePanel.GetContent());

            TabbedPanel tabbedPanel = new TabbedPanel();
            GeneralInfoPanel infoPanel2 = new GeneralInfoPanel("Child 2", Themes.PANEL_THEME_ORANGE_AND_WHITE, Dimensions.INFO_PANEL_WIDTH);
            GeneralInfoPanel infoPanel3 = new GeneralInfoPanel("Child 3", Themes.PANEL_THEME_ORANGE_AND_WHITE, Dimensions.INFO_PANEL_WIDTH);
            GeneralInfoPanel infoPanel4 = new GeneralInfoPanel("Child Panel 4", Themes.PANEL_THEME_ORANGE_AND_WHITE, Dimensions.INFO_PANEL_WIDTH);
            infoPanel2.SetPanelBorder(2, Color.Black, 0.5f, false);
            infoPanel2.AddContent(TestSection2.Content);
            infoPanel3.AddContent(TestSection3.Content);


            tabbedPanel.AddChildPanel(infoPanel2);
            tabbedPanel.AddChildPanel(infoPanel3);
            tabbedPanel.AddChildPanel(infoPanel4);

            FolderTile FolderTile = new FolderTile("Hello", true, true);
            mainLayout.Children.Add(FolderTile.GetContent());

            FolderTile FolderTileInactive = new FolderTile("This is an inactive folder", false, true);
            mainLayout.Children.Add(FolderTileInactive.GetContent());

            mainLayout.Children.Add(tabbedPanel.GetContent());









            mainLayout.Children.Add(infoPanel1.GetContent());

            mainLayout.Children.Add(removableLabel.GetContent());

            mainLayout.Children.Add(testButton.GetContent());
            mainLayout.Children.Add(TestSectionParent.Content);
            
            //mainLayout.Children.Add(TestButton.GetContent());
            mainLayout.Children.Add(toggleHeaderButton.GetContent());
            mainLayout.Children.Add(toggleSubHeaderButton.GetContent());
            mainLayout.Children.Add(toggleNavHeaderButton.GetContent());
            mainLayout.Children.Add(toggleNavMenuButton.GetContent());
            mainLayout.Children.Add(toggleModalButton.GetContent());
            mainLayout.Children.Add(togglePanelButton.GetContent());
            mainLayout.Children.Add(toggleFooterButton.GetContent());
            mainLayout.Children.Add(toggleForegroundButton.GetContent());

            /*
            List<List<string>> barValues = new List<List<string>>();
            barValues.Add(new List<string> { "Not Very Likely", "0" });
            barValues.Add(new List<string> { "Quite Likely", "1" });
            barValues.Add(new List<string> { "Likely", "2" });
            barValues.Add(new List<string> { "Very Likely", "3" });
            barValues.Add(new List<string> { "Definitely", "4" });
            barValues.Add(new List<string> { "Oh yes", "5" });
            barValues.Add(new List<string> { "You wot fam", "6" });

            List<List<string>> barValues2 = new List<List<string>>();
            //barValues2.Add(new List<string> { "I'm poor", "0" });
            barValues2.Add(new List<string> { "£40", "1" });
            barValues2.Add(new List<string> { "£60", "2" });
            barValues2.Add(new List<string> { "£80", "3" });
            barValues2.Add(new List<string> { "£100", "4" });
            //barValues2.Add(new List<string> { "I'm minted!", "5" });

            List<List<string>> barValues3 = new List<List<string>>();
            barValues3.Add(new List<string> { "1", "0" });
            barValues3.Add(new List<string> { "2", "1" });
            barValues3.Add(new List<string> { "3", "2" });
            barValues3.Add(new List<string> { "4", "3" });
            barValues3.Add(new List<string> { "5", "4" });
            barValues3.Add(new List<string> { "6", "5" });
            barValues3.Add(new List<string> { "7", "6" });
            barValues3.Add(new List<string> { "8", "7" });
            barValues3.Add(new List<string> { "9", "8" });
            barValues3.Add(new List<string> { "10", "9" });*/



            DragSlider ds = new DragSlider("How Likely", AppDataContent.ConvenienceStores, Dimensions.SLIDER_WIDTH, Dimensions.SLIDER_HEIGHT);
            DragSlider ds2 = new DragSlider("How Much", AppDataContent.ConvenienceStores, Dimensions.SLIDER_WIDTH, Dimensions.SLIDER_HEIGHT);
            DragSlider ds3 = new DragSlider("How Many", AppDataContent.ConvenienceStores, Dimensions.SLIDER_WIDTH, Dimensions.SLIDER_HEIGHT);

            

            mainLayout.Children.Add(ds.Content);
            mainLayout.Children.Add(ds2.Content);
            mainLayout.Children.Add(ds3.Content);

            /*
            try
            {
                ds.SelectedIndex = 3;
                ds.SliderObject.UpAction.Execute();
                //ds.SliderObject.DraggableView.SetPositionCommand.Execute(true);
            }
            catch (Exception e) { }*/

            return mainLayout;
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
                App.ShowMenuButton();
                //App.HideMenuButton();
            }  
        }

        public override void ExecuteRefreshCommand()
        {
            RefreshPage();
        }

        private async void RefreshPage()
        {
            timesRefreshed++;
            await Task.Delay(50);

            UpdatableElementsContainer.Children.Clear();

            Random rnd = new Random();
            int numItems = rnd.Next(4, 8);


            List<Item> items = new List<Item>();

            for(int i=0; i<numItems; i++)
            {
                Item i1 = new Item();
                i1.Name = "Item " + i;
                i1.MainImage = "plus.png";
                
                items.Add(i1);
            }
            

            foreach (Item item in items)
            {
                ItemLayout itemLayout = new ItemLayout(item);
                itemLayout.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;
                //itemLayout.MainImage.Content.HorizontalOptions = LayoutOptions.CenterAndExpand;

                UpdatableElementsContainer.Children.Add(itemLayout.Content);
            }
            



            // Stop refreshing
            IsRefreshing = false;
            //base.RefreshView.IsRefreshing = false;
            RefreshView.IsRefreshing = false;
        }
    }
}