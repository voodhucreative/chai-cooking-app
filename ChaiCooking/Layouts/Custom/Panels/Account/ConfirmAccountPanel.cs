//using System;
//using ChaiCooking.AppData;
//using ChaiCooking.Branding;
//using ChaiCooking.Components.Buttons;
//using ChaiCooking.Components.Fields;
//using ChaiCooking.Components.Fields.Custom;
//using ChaiCooking.Components.Labels;
//using ChaiCooking.Helpers;
//using ChaiCooking.Helpers.Custom;
//using ChaiCooking.Services;
//using Xamarin.Forms;

//namespace ChaiCooking.Layouts.Custom.Panels.Account
//{
//    public class ConfirmAccountPanel : StandardLayout
//    {
//        StackLayout masterContainer, contentContainer, buttonContainer;
//        Grid accountDetailsGrid;

//        ColourButton cancelButton;

//        Components.Buttons.ImageButton submitButton;

//        StaticLabel confirmationInfo, EmailTitle, EmailText, DOBTitle, DOBText, GenderTitle,
//            GenderText, FirstNameTitle, FirstNameText, SurnameTitle, SurnametText, UsernameTitle,
//            UsernameText;

//        public ConfirmAccountPanel()
//        {
//            Container = new Grid
//            {
//                VerticalOptions = LayoutOptions.CenterAndExpand
//            };

//            Content = new Grid
//            {
//                VerticalOptions = LayoutOptions.CenterAndExpand
//            };

//            // Initialise our container layouts 
//            #region Containers
//            masterContainer = new StackLayout
//            {
//                Orientation = StackOrientation.Vertical,
//                HorizontalOptions = LayoutOptions.CenterAndExpand,
//                Padding = Dimensions.GENERAL_COMPONENT_PADDING
//            };

//            contentContainer = new StackLayout
//            {
//                Orientation = StackOrientation.Vertical,
//                HorizontalOptions = LayoutOptions.FillAndExpand
//            };

//            buttonContainer = new StackLayout { 
//                Orientation = StackOrientation.Horizontal,
//                Padding = Dimensions.GENERAL_COMPONENT_PADDING 
//            };
//            #endregion

//            confirmationInfo = new StaticLabel("An account will be created with the following information. Please check that all details are correct before proceeding.");
//            confirmationInfo.Content.FontSize = Units.FontSizeM;
//            confirmationInfo.Content.TextColor = Color.FromHex(Colors.CC_DARK_GREY);
//            confirmationInfo.CenterAlign();

//            accountDetailsGrid = new Grid { ColumnSpacing = 4, RowSpacing = 8, VerticalOptions = LayoutOptions.CenterAndExpand };
//            accountDetailsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
//            accountDetailsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
//            accountDetailsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
//            accountDetailsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
//            accountDetailsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
//            accountDetailsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

//            EmailTitle = new StaticLabel("Email address");
//            EmailTitle.Content.FontFamily = Fonts.GetBoldAppFont();
//            EmailTitle.Content.FontSize = Units.FontSizeM;

//            EmailText = new StaticLabel("");
//            EmailText.Content.FontSize = Units.FontSizeM;

//            DOBTitle = new StaticLabel("Date of birth");
//            DOBTitle.Content.FontFamily = Fonts.GetBoldAppFont();
//            DOBTitle.Content.FontSize = Units.FontSizeM;

//            DOBText = new StaticLabel("");
//            DOBText.Content.FontSize = Units.FontSizeM;

//            GenderTitle = new StaticLabel("Gender");
//            GenderTitle.Content.FontFamily = Fonts.GetBoldAppFont();
//            GenderTitle.Content.FontSize = Units.FontSizeM;

//            GenderText = new StaticLabel("");
//            GenderText.Content.FontSize = Units.FontSizeM;

//            FirstNameTitle = new StaticLabel("First name");
//            FirstNameTitle.Content.FontFamily = Fonts.GetBoldAppFont();
//            FirstNameTitle.Content.FontSize = Units.FontSizeM;

//            FirstNameText = new StaticLabel("");
//            FirstNameText.Content.FontSize = Units.FontSizeM;

//            SurnameTitle = new StaticLabel("Surname");
//            SurnameTitle.Content.FontFamily = Fonts.GetBoldAppFont();
//            SurnameTitle.Content.FontSize = Units.FontSizeM;

//            SurnametText = new StaticLabel("");
//            SurnametText.Content.FontSize = Units.FontSizeM;

//            UsernameTitle = new StaticLabel("Username");
//            UsernameTitle.Content.FontFamily = Fonts.GetBoldAppFont();
//            UsernameTitle.Content.FontSize = Units.FontSizeM;

//            UsernameText = new StaticLabel("");
//            UsernameText.Content.FontSize = Units.FontSizeM;


//            FirstNameText.Content.SetBinding(Label.TextProperty, new Binding(nameof(AppSession.CurrentUser.FirstName), source: AppSession.CurrentUser));
//            SurnametText.Content.SetBinding(Label.TextProperty, new Binding(nameof(AppSession.CurrentUser.LastName), source: AppSession.CurrentUser));
//            //DOBText.Content.SetBinding(Label.TextProperty, new Binding(nameof(AppSession.CurrentUser.DateOfBirth), BindingMode.Default, null, null, "{0:dd/MM/yyyy}", source: AppSession.CurrentUser));
//            EmailText.Content.SetBinding(Label.TextProperty, new Binding(nameof(AppSession.CurrentUser.EmailAddress), source: AppSession.CurrentUser));
//            GenderText.Content.SetBinding(Label.TextProperty, new Binding(nameof(AppSession.CurrentUser.Gender), source: AppSession.CurrentUser));
//            UsernameText.Content.SetBinding(Label.TextProperty, new Binding(nameof(AppSession.CurrentUser.Username), source: AppSession.CurrentUser));

//            accountDetailsGrid.Children.Add(EmailTitle.Content, 0, 0);
//            accountDetailsGrid.Children.Add(EmailText.Content, 1, 0);
//            accountDetailsGrid.Children.Add(DOBTitle.Content, 0, 1);
//            accountDetailsGrid.Children.Add(DOBText.Content, 1, 1);
//            accountDetailsGrid.Children.Add(GenderTitle.Content, 0, 2);
//            accountDetailsGrid.Children.Add(GenderText.Content, 1, 2);
//            accountDetailsGrid.Children.Add(FirstNameTitle.Content, 0, 3);
//            accountDetailsGrid.Children.Add(FirstNameText.Content, 1, 3);
//            accountDetailsGrid.Children.Add(SurnameTitle.Content, 0, 4);
//            accountDetailsGrid.Children.Add(SurnametText.Content, 1, 4);
//            accountDetailsGrid.Children.Add(UsernameTitle.Content, 0, 5);
//            accountDetailsGrid.Children.Add(UsernameText.Content, 1, 5);

//            // Initialise the buttons
//            #region Buttons
//            cancelButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE),
//                Color.White, AppText.CANCEL, new Models.Action((int)Actions.ActionName.ShowLoginAndRegister));
//            cancelButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
//            cancelButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
//            cancelButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

//            submitButton = new Components.Buttons.ImageButton("arrow_right_green_chevron.png", "arrow_right_green_chevron.png",
//                AppText.SUBMIT, Color.Black, new Models.Action((int)Actions.ActionName.ShowAgreementModal));
//            submitButton.RightAlign();
//            submitButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);

//            buttonContainer.Children.Add(cancelButton.Content);
//            buttonContainer.Children.Add(submitButton.Content);
//            #endregion

//            // Add the content to the containers
//            #region Containers
//            masterContainer.Children.Add(confirmationInfo.Content);
//            contentContainer.Children.Add(accountDetailsGrid);
//            masterContainer.Children.Add(contentContainer);
//            masterContainer.Children.Add(buttonContainer);
//            Container.Children.Add(masterContainer);
//            Content.Children.Add(Container);
//            #endregion
//        }
//    }
//}
