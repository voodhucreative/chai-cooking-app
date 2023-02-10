using System;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Fields;
using ChaiCooking.Components.Fields.Custom;
using ChaiCooking.DebugData.Custom;
using ChaiCooking.Helpers;
using ChaiCooking.Tools;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Services;
using Xamarin.Forms;
using System.Globalization;
using Xamarin.CommunityToolkit.Effects;

namespace ChaiCooking.Layouts.Custom.Panels.Account
{
    public class CreateAccountPanel : StandardLayout
    {
        StackLayout ContentContainer;
        StackLayout ButtonsContainer;

        FormInputField EmailInput;
        FormInputField DOBInput; // swap for date picker
        DatePicker DOBPicker;
        FormInputField GenderInput;
        FormInputField FirstNameInput;
        FormInputField SurnameInput;
        FormInputField UsernameInput;
        FormInputField PasswordInput;
        FormInputField ConfirmPasswordInput;

        ColourButton CancelButton;

        Components.Buttons.ImageButton SubmitButton;

        public CreateAccountPanel()
        {
            Container = new Grid { };
            Content = new Grid
            {
            };
            ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
            };

            ButtonsContainer = new StackLayout { Orientation = StackOrientation.Horizontal };

            Label dateLabel = new Label
            {
                FontSize = Units.FontSizeM,
                TextColor = Color.Gray,
                Text = AppText.DATE_OF_BIRTH
            };

            DOBPicker = new DatePicker()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Format = "dd/MM/yyyy",
                MaximumDate = DateTime.Now
            };

            StackLayout dateStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    dateLabel,
                    DOBPicker
                }
            };



            EmailInput = new FormInputField(AppText.EMAIL_ADDRESS, AppText.EMAIL_ADDRESS, Keyboard.Email, true);
            DOBInput = new FormInputField(AppText.DATE_OF_BIRTH, AppText.DATE_OF_BIRTH, Keyboard.Numeric, true);
            GenderInput = new FormInputField(AppText.GENDER, AppText.GENDER, Keyboard.Text, true);
            FirstNameInput = new FormInputField(AppText.FIRST_NAME, AppText.FIRST_NAME, Keyboard.Text, true);
            SurnameInput = new FormInputField(AppText.SURNAME, AppText.SURNAME, Keyboard.Text, true);
            UsernameInput = new FormInputField(AppText.USERNAME, AppText.USERNAME, Keyboard.Plain, true);
            PasswordInput = new FormInputField("Chai " + AppText.PASSWORD, AppText.PASSWORD, Keyboard.Text, true);
            PasswordInput.TextEntry.IsPassword = true;

            ConfirmPasswordInput = new FormInputField(AppText.CONFIRM_PASSWORD, AppText.CONFIRM_PASSWORD, Keyboard.Text, true);
            ConfirmPasswordInput.TextEntry.IsPassword = true;

            if (AppSettings.SimplifiedLoginAndRegister)
            {
                try { EmailInput.TextEntry.Text = AppSession.CurrentUser.EmailAddress; } catch (Exception e) { }
                try { GenderInput.TextEntry.Text = AppSession.CurrentUser.Gender; } catch (Exception e) { }
                try { FirstNameInput.TextEntry.Text = AppSession.CurrentUser.FirstName; } catch (Exception e) { }
                try { SurnameInput.TextEntry.Text = AppSession.CurrentUser.LastName; } catch (Exception e) { }
            }

            CancelButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CANCEL, null);
            CancelButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            CancelButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            CancelButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(CancelButton.Content, true);
            TouchEffect.SetCommand(CancelButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    //Send us home if we cancel our registration.
                    await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Landing);
                });
            }));

            SubmitButton = new Components.Buttons.ImageButton("arrow_right_green_chevron_small.png", "arrow_right_green_chevron_small.png", AppText.SUBMIT, Color.Black, null);
            SubmitButton.RightAlign();
            SubmitButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);

            TouchEffect.SetNativeAnimation(SubmitButton.Content, true);
            TouchEffect.SetCommand(SubmitButton.Content,
            new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (ValidateFormInput())
                    {
                        await App.PerformActionAsync(new Models.Action((int)Actions.ActionName.ShowAccountConfirmationModal));
                    }
                    else
                    {
                        //Changed from error stuff.
                        App.ShowAlert("Account not created\n" + "Please check your passwords match then try again");
                    }
                });
            }));

            //FirstNameInput.TextEntry.SetBinding(InputView.TextProperty, new Binding(nameof(AppSession.CurrentUser.FirstName), source: AppSession.CurrentUser));
            //SurnameInput.TextEntry.SetBinding(InputView.TextProperty, new Binding(nameof(AppSession.CurrentUser.LastName), source: AppSession.CurrentUser));

            //EmailInput.TextEntry.SetBinding(InputView.TextProperty, new Binding(nameof(AppSession.CurrentUser.EmailAddress), source: AppSession.CurrentUser));
            //PasswordInput.TextEntry.SetBinding(InputView.TextProperty, new Binding(nameof(AppSession.CurrentUser.Password), source: AppSession.CurrentUser));
            //ConfirmPasswordInput.TextEntry.SetBinding(InputView.TextProperty, new Binding(nameof(AppSession.CurrentUser.Password), source: AppSession.CurrentUser));
            //DOBInput.TextEntry.SetBinding(InputView.TextProperty, new Binding(nameof(AppSession.CurrentUser.DateOfBirth), source: AppSession.CurrentUser));
            //DOBInput.TextEntry.SetBinding(InputView.TextProperty, new Binding(nameof(AppSession.CurrentUser.DateOfBirth), BindingMode.Default, null, null, "{0:dd/MM/yyyy}", source: AppSession.CurrentUser));
            DOBPicker.SetBinding(DatePicker.DateProperty, new Binding(nameof(AppSession.CurrentUser.DateOfBirth), BindingMode.Default, null, null, "{0:dd/MM/yyyy}", source: AppSession.CurrentUser));
            GenderInput.TextEntry.SetBinding(InputView.TextProperty, new Binding(nameof(AppSession.CurrentUser.Gender), source: AppSession.CurrentUser));
            //UsernameInput.TextEntry.SetBinding(InputView.TextProperty, new Binding(nameof(AppSession.CurrentUser.Username), source: AppSession.CurrentUser));

            ButtonsContainer.Children.Add(CancelButton.Content);
            ButtonsContainer.Children.Add(SubmitButton.Content);
            Container.Children.Add(FirstNameInput.Content, 0, 0);
            Container.Children.Add(SurnameInput.Content, 1, 0);

            if (!AppSettings.SimplifiedLoginAndRegister)
            {
                //Container.Children.Add(DOBInput.Content, 0, 1);
                Container.Children.Add(dateStack, 0, 1);
                Container.Children.Add(GenderInput.Content, 1, 1);

                Container.Children.Add(UsernameInput.Content, 0, 2);
                Container.Children.Add(EmailInput.Content, 0, 3);
                Container.Children.Add(PasswordInput.Content, 0, 4);
                Container.Children.Add(ConfirmPasswordInput.Content, 1, 4);
                Container.Children.Add(CancelButton.Content, 0, 5);
                Container.Children.Add(SubmitButton.Content, 1, 5);
            }
            else
            {
                Container.Children.Add(PasswordInput.Content, 0, 1);
                Container.Children.Add(ConfirmPasswordInput.Content, 1, 1);
                Container.Children.Add(CancelButton.Content, 0, 2);
                Container.Children.Add(SubmitButton.Content, 1, 2);
            }


            //else
            //{
            //    Container.Children.Add(PasswordInput.Content, 0, 1);
            //    Container.Children.Add(ConfirmPasswordInput.Content, 1, 1);
            //    Container.Children.Add(CancelButton.Content, 0, 2);
            //    Container.Children.Add(SubmitButton.Content, 1, 2);
            //}
            ContentContainer.Children.Add(Container);
            //Container.Children.Add(ContentContainer);
            Content.Children.Add(ContentContainer);
            Grid.SetColumnSpan(EmailInput.Content, 2);
            Grid.SetColumnSpan(UsernameInput.Content, 2);
        }

        private bool ValidateFormInput()
        {
            if (!Validation.ValidateInput(FirstNameInput.TextEntry, 20, 3, false)) { return false; }

            if (!Validation.ValidateInput(SurnameInput.TextEntry, 20, 3, false)) { return false; }
            //if (!Validation.IsUsernameValid(UsernameInput.TextEntry.Text)) { return false; }
            //if (!Validation.ValidateInput(GenderInput.TextEntry, 20, 3, false)) { return false; }
            //if (!Validation.IsDOBValid(DOBInput.TextEntry.Text)) { return false; }

            if (PasswordInput.TextEntry.Text != ConfirmPasswordInput.TextEntry.Text)
            {
                return false;
            }

            if (!Validation.IsPasswordValid(PasswordInput.TextEntry.Text)) { return false; };
            if (!Validation.IsValidEmail(EmailInput.TextEntry)) { return false; }

            AppSession.CurrentUser.FirstName = FirstNameInput.TextEntry.Text;
            AppSession.CurrentUser.LastName = SurnameInput.TextEntry.Text;
            AppSession.CurrentUser.Username = UsernameInput.TextEntry.Text;
            AppSession.CurrentUser.Gender = GenderInput.TextEntry.Text;
            AppSession.CurrentUser.DateOfBirth = DOBPicker.Date;
            //AppSession.CurrentUser.DateOfBirth = DateTime.ParseExact(DOBInput.TextEntry.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            AppSession.CurrentUser.EmailAddress = EmailInput.TextEntry.Text;
            AppSession.CurrentUser.Password = PasswordInput.TextEntry.Text;

            return true;
        }
    }
}
