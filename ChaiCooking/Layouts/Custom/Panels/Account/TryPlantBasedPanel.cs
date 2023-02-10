using System;
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
    public class TryPlantBasedPanel : StandardLayout
    {
        StackLayout ContentContainer;
        StackLayout SectionContainer;

        StaticLabel Title;

        private const int LIMITED_VERSION_INFO = 0;

        private int CurrentSection;

        StackLayout LimitedVersionInfoContainer;

        public TryPlantBasedPanel()
        {
            Container = new Grid { };
            Content = new Grid { };
            ContentContainer = new StackLayout { Orientation = StackOrientation.Vertical, Padding = Dimensions.GENERAL_COMPONENT_PADDING };

            SectionContainer = new StackLayout { Orientation = StackOrientation.Vertical };


            Title = new StaticLabel(AppText.TRY_PLANT_BASED_MEAL_PLAN);
            Title.Content.FontSize = Units.FontSizeL;
            Title.Content.TextColor = Color.White;
            Title.LeftAlign();

            CurrentSection = LIMITED_VERSION_INFO;

            LimitedVersionInfoContainer = BuildLimitedVersionInfo();

            SectionContainer.Children.Add(GetSection(CurrentSection));

            //ContentContainer.Children.Add(Title.Content);

            ContentContainer.Children.Add(SectionContainer);

            Container.Children.Add(ContentContainer);
            Content.Children.Add(Container);
        }



        public void SetSection(int sectionToShow)
        {
            SectionContainer.Children.Clear();
            SectionContainer.Children.Add(GetSection(sectionToShow));
        }

        private StackLayout GetSection(int currentSection)
        {
            StackLayout currentContainer = LimitedVersionInfoContainer;
            switch (currentSection)
            {
                case LIMITED_VERSION_INFO:
                    currentContainer = LimitedVersionInfoContainer;
                    break;
            }
            return currentContainer;
        }

        private StackLayout BuildLimitedVersionInfo()
        {
            LimitedVersionInfoContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Grid container = new Grid
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });


            StaticLabel LeftTitle = new StaticLabel(AppText.TRY_PLANT_BASED_MEAL_PLAN);
            LeftTitle.Content.FontSize = Units.FontSizeM;
            LeftTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            LeftTitle.Content.TextColor = Color.White;
            LeftTitle.LeftAlign();

            /*StaticLabel RightTitle = new StaticLabel("Current Plan: CHAI free"); // get current plan details here
            RightTitle.Content.FontSize = Units.FontSizeM;
            RightTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            RightTitle.Content.TextColor = Color.White;
            RightTitle.RightAlign();*/

            StaticLabel TopInfo = new StaticLabel("Would you like to try out a limited version of the CHAI premium meal plan?");
            TopInfo.Content.FontSize = Units.FontSizeS;
            TopInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            TopInfo.Content.TextColor = Color.White;
            TopInfo.CenterAlign();

            StaticLabel Price = new StaticLabel("");
            FormattedString fs = new FormattedString();
            fs.Spans.Add(new Span { Text = "1 wk limited: ", ForegroundColor = Color.Red, FontSize = Units.FontSizeM, FontFamily = Fonts.GetBoldAppFont(), TextColor = Color.White });
            fs.Spans.Add(new Span { Text = " £0.00 ", ForegroundColor = Color.Red, FontSize = Units.FontSizeM, FontFamily = Fonts.GetBoldAppFont(), TextColor = Color.FromHex(Colors.CC_ORANGE) });
            Price.Content.FormattedText = fs;
            Price.LeftAlign();

            ColourButton UpgradeButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Free Trial", null);
            UpgradeButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            UpgradeButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING; ;

            UpgradeButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               //CurrentSection = MEAL_PLAN_STRATEGY;
                               //SetSection(CurrentSection);
                           });
                       })
                   }
               );

            StaticLabel PlanInfo = new StaticLabel("This option will aid on your journey from 'meat eater' to 'plant-based eater'. Whether you're looking to start gently or go the whole way to transitioning then we can help you do it.\n\nMore importantly, we can help you do it within your budget and applied preferences within the app.");
            PlanInfo.Content.FontSize = Units.FontSizeS;
            PlanInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            PlanInfo.Content.TextColor = Color.White;
            PlanInfo.LeftAlign();

            StaticLabel TryPremiumTitle = new StaticLabel("Limited trial of CHAI premium");
            TryPremiumTitle.Content.FontSize = Units.FontSizeS;
            TryPremiumTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            TryPremiumTitle.Content.TextColor = Color.White;
            TryPremiumTitle.LeftAlign();

            StaticLabel TryPremiumTagline = new StaticLabel("- a single week meal plan, controls disabled after 1wk");
            TryPremiumTagline.Content.FontSize = Units.FontSizeS;
            TryPremiumTagline.Content.FontFamily = Fonts.GetRegularAppFont();
            TryPremiumTagline.Content.TextColor = Color.White;
            TryPremiumTagline.LeftAlign();

            container.Children.Add(LeftTitle.Content, 0, 0);
            //container.Children.Add(RightTitle.Content, 1, 0);
            container.Children.Add(TopInfo.Content, 0, 1);
            container.Children.Add(Price.Content, 0, 2);
            container.Children.Add(UpgradeButton.Content, 1, 2);
            container.Children.Add(PlanInfo.Content, 0, 3);
            container.Children.Add(TryPremiumTitle.Content, 0, 4);
            container.Children.Add(TryPremiumTagline.Content, 0, 5);

            Grid.SetColumnSpan(TopInfo.Content, 2);
            Grid.SetColumnSpan(PlanInfo.Content, 2);
            Grid.SetColumnSpan(TryPremiumTitle.Content, 2);
            Grid.SetColumnSpan(TryPremiumTagline.Content, 2);

            LimitedVersionInfoContainer.Children.Add(container);


            return LimitedVersionInfoContainer;
        }

    }
}

