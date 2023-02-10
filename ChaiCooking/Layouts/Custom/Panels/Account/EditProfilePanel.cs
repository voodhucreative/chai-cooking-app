using System;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Composites;
using ChaiCooking.Components.Fields;
using ChaiCooking.Components.Fields.Custom;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Panels.Account
{
    public class EditProfilePanel : StandardLayout
    {
        StackLayout ContentContainer;
        StackLayout ButtonsContainer;

        StaticLabel Title;

        Grid EntryContainer;
        FormInputField FirstNameInput;
        FormInputField LastNameInput;
        FormInputField UsernameInput;
        FormInputField EmailInput;

        ColourButton CancelButton;
        ColourButton SaveChangesButton;

        public EditProfilePanel()
        {
            Container = new Grid { };
            Content = new Grid { };
            ContentContainer = new StackLayout { Orientation = StackOrientation.Vertical, Padding = Dimensions.GENERAL_COMPONENT_PADDING };

            ButtonsContainer = new StackLayout { Orientation = StackOrientation.Horizontal };

            EntryContainer = new Grid { ColumnSpacing = Dimensions.GENERAL_COMPONENT_SPACING, RowSpacing = Dimensions.GENERAL_COMPONENT_SPACING };

            Title = new StaticLabel(AppText.EDIT_PROFILE);
            Title.Content.FontSize = Units.FontSizeL;
            Title.Content.TextColor = Color.White;
            Title.LeftAlign();

            FirstNameInput = new FormInputField(AppText.FIRST_NAME, AppText.FIRST_NAME, Keyboard.Telephone, false);
            FirstNameInput.CenterAlign();
            FirstNameInput.SetTitleColor(Color.White);
            FirstNameInput.TextEntry.TextColor = Color.White;
            FirstNameInput.TextEntry.WidthRequest = 140;

            LastNameInput = new FormInputField(AppText.SURNAME, AppText.SURNAME, Keyboard.Telephone, false);
            LastNameInput.CenterAlign();
            LastNameInput.SetTitleColor(Color.White);
            LastNameInput.TextEntry.TextColor = Color.White;
            LastNameInput.TextEntry.WidthRequest = 140;

            UsernameInput = new FormInputField(AppText.USERNAME, AppText.USERNAME, Keyboard.Telephone, false);
            UsernameInput.CenterAlign();
            UsernameInput.SetTitleColor(Color.White);
            UsernameInput.TextEntry.TextColor = Color.White;
            UsernameInput.TextEntry.WidthRequest = 240;

            EmailInput = new FormInputField(AppText.EMAIL_ADDRESS, AppText.EMAIL_ADDRESS, Keyboard.Telephone, false);
            EmailInput.CenterAlign();
            EmailInput.SetTitleColor(Color.White);
            EmailInput.TextEntry.TextColor = Color.White;
            EmailInput.TextEntry.WidthRequest = 240;

            CancelButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CANCEL, null);
            CancelButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            CancelButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            CancelButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            CancelButton.LeftAlign();


            CancelButton.Content.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await App.PerformActionAsync((int)Actions.ActionName.GoToPage, AppSession.LastPageId);
                                await App.ShowMenu();
                            });
                        })
                    }
                );


            SaveChangesButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.SAVE_CHANGES, null);
            SaveChangesButton.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            SaveChangesButton.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            SaveChangesButton.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;
            SaveChangesButton.RightAlign();

            SaveChangesButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               if (await SaveChanges())
                               {
                                   App.ShowAlert("Your profile has been updated");
                               }
                               else
                               {
                                   App.ShowAlert("Sorry, we could not update your profile at this time.");
                               }
                           });
                       })
                   }
               );


            FirstNameInput.TextEntry.SetBinding(InputView.TextProperty, new Binding(nameof(AppSession.CurrentUser.FirstName), source: AppSession.CurrentUser));
            LastNameInput.TextEntry.SetBinding(InputView.TextProperty, new Binding(nameof(AppSession.CurrentUser.LastName), source: AppSession.CurrentUser));

            EmailInput.TextEntry.SetBinding(InputView.TextProperty, new Binding(nameof(AppSession.CurrentUser.EmailAddress), source: AppSession.CurrentUser));
            UsernameInput.TextEntry.SetBinding(InputView.TextProperty, new Binding(nameof(AppSession.CurrentUser.Username), source: AppSession.CurrentUser));


            ButtonsContainer.HorizontalOptions = LayoutOptions.CenterAndExpand;
            ButtonsContainer.Children.Add(CancelButton.Content);
            ButtonsContainer.Children.Add(SaveChangesButton.Content);

            EntryContainer.Children.Add(FirstNameInput.Content, 0, 0);
            EntryContainer.Children.Add(LastNameInput.Content, 1, 0);
            EntryContainer.Children.Add(UsernameInput.Content, 0, 1);
            EntryContainer.Children.Add(EmailInput.Content, 0, 2);

            Grid.SetColumnSpan(UsernameInput.Content, 2);
            Grid.SetColumnSpan(EmailInput.Content, 2);

            ContentContainer.Children.Add(Title.Content);
            ContentContainer.Children.Add(EntryContainer);
            ContentContainer.Children.Add(ButtonsContainer);


            Container.Children.Add(ContentContainer);
            Content.Children.Add(Container);
        }

        private async Task<bool> SaveChanges()
        {
            await Task.Delay(10);
            // validate input

            // assign values
            AppSession.CurrentUser.FirstName = FirstNameInput.TextEntry.Text.ToString();
            AppSession.CurrentUser.LastName = LastNameInput.TextEntry.Text.ToString();
            AppSession.CurrentUser.Username = UsernameInput.TextEntry.Text.ToString();
            AppSession.CurrentUser.EmailAddress = EmailInput.TextEntry.Text.ToString();

            // save user locally

            // upload changes

            return true;
        }
    }
}

