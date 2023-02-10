using System;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components;
using ChaiCooking.Components.Buttons;
using ChaiCooking.Components.Fields;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Layouts.Custom.Modals
{
    public class EditPlanTitleModal : ActiveComponent
    {
        public EditPlanTitleModal(int templateId, string name, string publishedAt)
        {
            StaticLabel titleLabel = new StaticLabel("Rename Meal Template");
            titleLabel.Content.FontSize = Units.FontSizeXL;
            titleLabel.Content.TextColor = Color.White;
            titleLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            titleLabel.CenterAlign();

            StaticLabel closeLabel = new StaticLabel(AppText.CLOSE);
            closeLabel.Content.FontSize = Units.FontSizeL;
            closeLabel.Content.TextColor = Color.White;
            closeLabel.RightAlign();

            TouchEffect.SetNativeAnimation(closeLabel.Content, true);
            TouchEffect.SetCommand(closeLabel.Content,
                new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.HideModalAsync();
                    });
                }));

            StackLayout titleContent = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Padding = Dimensions.GENERAL_COMPONENT_PADDING,
                //BackgroundColor = Color.Red,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    titleLabel.Content,
                    closeLabel.Content
                }
            };

            StackLayout seperator = new StackLayout
            {
                WidthRequest = Units.ScreenWidth,
                HeightRequest = 1,
                BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY)
            };

            StaticLabel nameEntryDetails = new StaticLabel("Enter a new name:");
            nameEntryDetails.Content.FontSize = Units.FontSizeL;
            nameEntryDetails.Content.TextColor = Color.White;
            nameEntryDetails.Content.FontFamily = Fonts.GetRegularAppFont();
            nameEntryDetails.CenterAlign();

            StaticLabel descriptionLabel = new StaticLabel("The meal template will no longer be published after saving your changes.");
            descriptionLabel.Content.FontSize = Units.FontSizeL;
            descriptionLabel.Content.TextColor = Color.White;
            descriptionLabel.Content.FontFamily = Fonts.GetRegularAppFont();
            descriptionLabel.CenterAlign();

            SimpleInputField nameInputField = new SimpleInputField("Enter name", Keyboard.Plain);
            nameInputField.TextEntry.Text = name;
            nameInputField.TextEntry.HorizontalTextAlignment = TextAlignment.Center;
            nameInputField.TextEntry.VerticalTextAlignment = TextAlignment.Center;
            nameInputField.TextEntry.FontSize = Units.FontSizeM;
            nameInputField.TextEntry.FontFamily = Fonts.GetRegularAppFont();
            nameInputField.TextEntry.WidthRequest = 300;
            nameInputField.TextEntry.HeightRequest = Dimensions.SEARCH_INPUT_HEIGHT;
            nameInputField.Content.Margin = 0;

            StackLayout inputCont = new StackLayout()
            {
                HeightRequest = Dimensions.SEARCH_INPUT_HEIGHT,
                Children =
                {
                    nameInputField.Content
                }
            };

            ColourButton confirmBtn = new ColourButton
            (Color.FromHex(Colors.CC_ORANGE), Color.White, AppText.CONFIRM, null);
            confirmBtn.Content.WidthRequest = Dimensions.STANDARD_BUTTON_WIDTH;
            confirmBtn.Content.HeightRequest = Dimensions.STANDARD_BUTTON_HEIGHT;
            confirmBtn.Label.FontSize = Dimensions.STANDARD_BUTTON_FONT_SIZE;

            TouchEffect.SetNativeAnimation(confirmBtn.Content, true);
            TouchEffect.SetCommand(confirmBtn.Content,
                            new Command(() =>
                            {
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    if (nameInputField.TextEntry.Text != null || nameInputField.TextEntry.Text != "")
                                    {
                                        var result = await App.ApiBridge.UpdateUserMealPlanTemplates(AppSession.CurrentUser, templateId, nameInputField.TextEntry.Text, null);
                                        if (result)
                                        {
                                            await App.HideModalAsync();
                                            AppSession.SetMealPlanner(nameInputField.TextEntry.Text, true, 1, true);
                                        }
                                        else
                                        {
                                            App.ShowAlert("Failed to save changes.");
                                        }
                                    }
                                    else
                                    {
                                        App.ShowAlert("Please check missing fields.");
                                    }
                                });
                            }));

            StackLayout masterContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                Spacing = 4,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                Children =
                {
                    titleContent,
                    seperator,
                    nameEntryDetails.Content,
                    inputCont,
                    descriptionLabel.Content,
                    confirmBtn.Content
                }
            };

            Frame frame = new Frame
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(Units.ScreenWidth10Percent, 0, Units.ScreenWidth10Percent, 0),
                Content = masterContainer,
                CornerRadius = 5,
                IsClippedToBounds = true,
                Padding = 0,
            };

            Content.Children.Add(frame);
        }
    }
}
