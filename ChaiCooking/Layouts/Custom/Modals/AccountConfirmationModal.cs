using System;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models;
using ChaiCooking.Services.Storage;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Modals
{
    public class AccountConfirmationModal : StandardLayout
    {
        StackLayout masterContainer;

        Label emailAddressTitle, dateOfBirthTitle, genderTitle,
            firstNameTitle, surnameTitle, usernameTitle, emailText,
            dateOfBirthText, genderText,
            usernameText;

        CustomEntry firstNameText, surnameText;

        Grid splitGrid;

        public AccountConfirmationModal()
        {
            Container = new Grid { VerticalOptions = LayoutOptions.Center }; Content = new Grid { VerticalOptions = LayoutOptions.Center};

            // Initialise our container layouts 
            #region Containers
            masterContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING
            };

            Label detailsText = new Label
            {
                Text = "An account will be created with the following information. Please check that all details are correct before proceeding.",
                FontSize = Units.FontSizeM,
                TextColor = Color.Gray
            };

            StackLayout titleContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children =
                    {
                        detailsText
                    }
            };

            StackLayout buttonContainer = new StackLayout
            {
                VerticalOptions = LayoutOptions.EndAndExpand,
                Orientation = StackOrientation.Horizontal,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING
            };
            #endregion

            emailText = new Label
            {
                Text = AppSession.CurrentUser.EmailAddress,
                FontSize = Units.FontSizeL,
                TextColor = Color.Black
            };

            emailAddressTitle = new Label
            {
                Text = "Email address",
                FontSize = Units.FontSizeL,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            };

            dateOfBirthText = new Label
            {
                Text = AppSession.CurrentUser.DateOfBirth.ToString("dd/MM/yyyy"),
                FontSize = Units.FontSizeL,
                TextColor = Color.Black
            };

            dateOfBirthTitle = new Label
            {
                Text = "Date of birth",
                FontSize = Units.FontSizeL,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            };

            genderText = new Label
            {
                Text = AppSession.CurrentUser.Gender,
                FontSize = Units.FontSizeL,
                TextColor = Color.Black
            };

            genderTitle = new Label
            {
                Text = "Gender",
                FontSize = Units.FontSizeL,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            };

            firstNameText = new CustomEntry()
            {
                Placeholder = AppSession.CurrentUser.FirstName,
                FontSize = Units.FontSizeL,
                TextColor = Color.Black
            };

            firstNameTitle = new Label
            {
                Text = "First name",
                FontSize = Units.FontSizeL,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            };

            surnameText = new CustomEntry
            {
                Placeholder = AppSession.CurrentUser.LastName,
                FontSize = Units.FontSizeL,
                TextColor = Color.Black
            };

            surnameTitle = new Label
            {
                Text = "Surname",
                FontSize = Units.FontSizeL,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            };

            usernameText = new Label
            {
                Text = AppSession.CurrentUser.Username,
                FontSize = Units.FontSizeL,
                TextColor = Color.Black
            };

            usernameTitle = new Label
            {
                Text = "Username",
                FontSize = Units.FontSizeL,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            };

            splitGrid = new Grid
            {
                Margin = Units.MarginM,
                RowSpacing = Units.MarginM,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    //new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    //new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    //new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)}
                },
                Children =
                {
                    /*
                    { emailAddressTitle, 0, 0 },
                    { emailText, 1, 0 },
                    { dateOfBirthTitle, 0, 1 },
                    { dateOfBirthText, 1, 1 },
                    { genderTitle, 0, 2 },
                    { genderText, 1, 2 },
                    { firstNameTitle, 0, 3 },
                    { firstNameText, 1, 3 },
                    { surnameTitle, 0, 4 },
                    { surnameText, 1, 4 },
                    { usernameTitle, 0, 5 },
                    { usernameText, 1, 5 },*/

                    { emailAddressTitle, 0, 0 },
                    { emailText, 1, 0 },
                    { firstNameTitle, 0,1 },
                    { firstNameText, 1, 1 },
                    { surnameTitle, 0, 2 },
                    { surnameText, 1, 2 },
                }
            };

            StackLayout detailsContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children =
                    {
                        splitGrid
                    }
            };

            // Initialise the buttons
            #region Buttons
            ColourButton cancelButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE),
                    Color.White, AppText.CANCEL, new Models.Action((int)Actions.ActionName.HideAccountConfirmationModal));
            cancelButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            cancelButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            cancelButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(cancelButton.Content, true);

            ColourButton confirmButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE),
                Color.White, AppText.CONFIRM, null);

            confirmButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            confirmButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            confirmButton.RightAlign();
            confirmButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(confirmButton.Content, true);
            TouchEffect.SetCommand(confirmButton.Content,
                        new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                UpdateName();
                                BuildAgreementModal();
                            });
                        }));

            buttonContainer.Children.Add(cancelButton.Content);
            buttonContainer.Children.Add(confirmButton.Content);
            #endregion

            // Add the content to the containers
            #region Containers
            masterContainer.Children.Add(titleContainer);
            masterContainer.Children.Add(detailsContainer);
            masterContainer.Children.Add(buttonContainer);
            Container.Children.Add(masterContainer);
            Content.Children.Add(Container);
            #endregion
        }

        private void UpdateName()
        {
            AppSession.CurrentUser.FirstName = string.IsNullOrWhiteSpace(firstNameText.Text)? AppSession.CurrentUser.FirstName : firstNameText.Text;
            AppSession.CurrentUser.LastName = string.IsNullOrWhiteSpace(surnameText.Text) ? AppSession.CurrentUser.LastName : surnameText.Text;
        }

        public void BuildAgreementModal()
        {
            masterContainer.Children.Clear();

            masterContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING
            };

            Label detailsText = new Label
            {
                Text = "By creating an account, you agree to",
                FontSize = Units.FontSizeM,
                TextColor = Color.Gray
            };

            Label termsText = new Label
            {
                Text = "CHAI's Terms of Service",
                FontAttributes = FontAttributes.Bold,
                FontSize = Units.FontSizeM,
                TextColor = Color.Black
            };

            Label privacyText = new Label
            {
                Text = "To learn more about how CHAI collects, uses, shares and protects your personal data please read",
                FontSize = Units.FontSizeM,
                TextColor = Color.Black
            };

            Label privacyLink = new Label
            {
                Text = "CHAI's Privacy Policy",
                FontAttributes = FontAttributes.Bold,
                FontSize = Units.FontSizeM,
                TextColor = Color.Black
            };

            StackLayout titleContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children =
                    {
                        detailsText,
                        termsText,
                        privacyText,
                        privacyLink
                    }
            };

            ColourButton cancelButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE),
                    Color.White, AppText.CANCEL, new Models.Action((int)Actions.ActionName.HideAccountConfirmationModal));
            cancelButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            cancelButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            cancelButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(cancelButton.Content, true);

            ColourButton confirmButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE),
            Color.White, AppText.CONFIRM, null);

            confirmButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            confirmButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            confirmButton.RightAlign();
            confirmButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(confirmButton.Content, true);
            TouchEffect.SetCommand(confirmButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        await App.ApiBridge.CreateUser(AppSession.CurrentUser);

                        if (AppSession.CurrentUser.AuthToken != null)
                        {
                            AppSession.CurrentUser.IsRegistered = true;

                            await App.PerformActionAsync(new Models.Action((int)Actions.ActionName.HideAccountConfirmationModal));
                            await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Questionnaire);

                            App.ShowAlert("", "Psst… CHAI works better when we know your preferences, we can tailor our services to suit your needs");
                            AppSession.settingUpAccount = true;

                            // MH : don't think it's actually necessary to login here, but leaving this here, just in case...
                            
                            /*
                            if (await App.ApiBridge.LoginUser(AppSession.CurrentUser))
                            {
                                //LocalDataStore.SaveAll();
                                AppSession.CurrentUser.IsRegistered = true;
                                var result = await App.ApiBridge.SwapWhiskToken(AppSession.CurrentUser);

                                // get saved user preferences here

                                try
                                {
                                    UserPreferences userPrefs = await App.ApiBridge.GetPreferences(AppSession.CurrentUser);

                                    if (userPrefs != null)
                                    {
                                        AppSession.CurrentUser.Preferences = userPrefs;
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Failed to get user prefs");
                                }

                                LocalDataStore.SaveAll();

                                await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Questionnaire);
                                App.ShowAlert("Psst… CHAI works better when we know your preferences, we can tailor our services to suit your needs");
                            }*/
                            //await App.PerformActionAsync(new Models.Action((int)Actions.ActionName.HideAccountConfirmationModal));
                        }
                    }
                    catch (Exception e)
                    {
                        App.ShowAlert("Failed to create account.");
                    }
                });
            }));

            StackLayout buttonContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                Children =
                {
                cancelButton.Content,
                confirmButton.Content
                }
            };

            masterContainer.Children.Add(titleContainer);
            masterContainer.Children.Add(buttonContainer);
            Container.Children.Add(masterContainer);
            Content.Children.Add(Container);
        }
    }
}

