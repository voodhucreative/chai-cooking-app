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
    public class UpgradePanel : StandardLayout
    {
        StackLayout ContentContainer;
        StackLayout SectionContainer;

        StaticLabel Title;

        private const int PLANS_OPTIONS = 0;
        private const int UPGRADE_OPTIONS = 1;
        private const int MEAL_PLAN_STRATEGY = 2;
        private const int TARGET_OPTIONS = 3;
        private const int STATS_OPTIONS = 4;
        private const int CONFIRMATION = 5;
        private const int UPGRADE_SUCCESS = 6;
        private const int UPGRADE_FAILURE = 7;

        private int CurrentSection;

        // maybe put these in seperate layouts / panels.. we'll see
        StackLayout PlanOptionsContainer;
        StackLayout UpgradeOptionsContainer;
        StackLayout MealPlanStrategyContainer;
        StackLayout TargetOptionsContainer;
        StackLayout StatsOptionsContainer;
        StackLayout ConfirmationContainer;
        StackLayout UpgradeSuccessContainer;
        StackLayout UpgradeFailureContainer;

        string NewPlanName;

        int UpgradeType;
        int PlanType;
        int TargetType;

        float UserHeight;
        float UserWeightCurrent;
        float UserWeightTarget;

        public UpgradePanel()
        {
            Container = new Grid { };
            Content = new Grid { };

            ContentContainer = new StackLayout { Orientation = StackOrientation.Vertical, Padding = Dimensions.GENERAL_COMPONENT_PADDING };

            SectionContainer = new StackLayout { Orientation = StackOrientation.Vertical };

            Title = new StaticLabel(AppText.UPGRADE);
            Title.Content.FontSize = Units.FontSizeL;
            Title.Content.TextColor = Color.White;
            Title.LeftAlign();

            CurrentSection = PLANS_OPTIONS;

            PlanOptionsContainer = BuildPlanOptions();
            
            NewPlanName = "";
            UpgradeType = Accounts.UPGRADE_PREMIUM;
            PlanType = Accounts.PLAN_TYPE_FLEXITARIAN;
            TargetType = Accounts.TARGET_NONE;

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
            StackLayout currentContainer = PlanOptionsContainer;
            switch (currentSection)
            {
                case PLANS_OPTIONS:
                    PlanOptionsContainer = BuildPlanOptions();
                    currentContainer = PlanOptionsContainer;
                    break;
                case UPGRADE_OPTIONS:
                    UpgradeOptionsContainer = BuildUpgradeOptions();
                    currentContainer = UpgradeOptionsContainer;
                    break;
                case MEAL_PLAN_STRATEGY:
                    MealPlanStrategyContainer = BuildMealPlanStrategy();
                    currentContainer = MealPlanStrategyContainer;
                    break;
                case TARGET_OPTIONS:
                    TargetOptionsContainer = BuildTargetOptions();
                    currentContainer = TargetOptionsContainer;
                    break;
                case STATS_OPTIONS:
                    StatsOptionsContainer = BuildStatsOptions();
                    currentContainer = StatsOptionsContainer;
                    break;
                case CONFIRMATION:
                    ConfirmationContainer = BuildConfirmation();
                    currentContainer = ConfirmationContainer;
                    break;
                case UPGRADE_SUCCESS:
                    UpgradeSuccessContainer = BuildUpgradeSuccess();
                    currentContainer = UpgradeSuccessContainer;
                    break;
                case UPGRADE_FAILURE:
                    UpgradeFailureContainer = BuildUpgradeFailure();
                    currentContainer = UpgradeFailureContainer;
                    break;

            }
            return currentContainer;
        }

        // the following sections can be placed in seperate classes, if necessary, but for now they can
        // stay here because they're all relative to the upgrade panel itself

        // TO DO: replace placeholder text with live data
        private StackLayout BuildPlanOptions()
        {
            PlanOptionsContainer = new StackLayout
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
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            StaticLabel LeftTitle = new StaticLabel("Upgrade");
            LeftTitle.Content.FontSize = Units.FontSizeM;
            LeftTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            LeftTitle.Content.TextColor = Color.White;
            LeftTitle.LeftAlign();

            StaticLabel RightTitle = new StaticLabel(AppSession.CurrentUser.Preferences.AccountName);
            RightTitle.Content.FontSize = Units.FontSizeM;
            RightTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            RightTitle.Content.TextColor = Color.White;
            RightTitle.RightAlign();
            RightTitle.Content.SetBinding(Label.TextProperty, new Binding(nameof(AppSession.CurrentUser.Preferences.AccountName), source: AppSession.CurrentUser.Preferences));



            StaticLabel TopInfo = new StaticLabel("There are two ways to upgrade your CHAI account");
            TopInfo.Content.FontSize = Units.FontSizeS;
            TopInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            TopInfo.Content.TextColor = Color.White;
            TopInfo.CenterAlign();

            StaticLabel EditorPublisher = new StaticLabel("Editor/Publisher");
            EditorPublisher.Content.FontSize = Units.FontSizeM;
            EditorPublisher.Content.FontFamily = Fonts.GetBoldAppFont();
            EditorPublisher.Content.TextColor = Color.White;
            EditorPublisher.LeftAlign();

            ColourButton EditorPublisherInfoButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "More info", null);
            EditorPublisherInfoButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            EditorPublisherInfoButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING; ;

            StaticLabel EditorInfo = new StaticLabel("A one off In-App purchase to unlock various creator functions for writing and publishing recipes.");
            EditorInfo.Content.FontSize = Units.FontSizeS;
            EditorInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            EditorInfo.Content.TextColor = Color.White;
            EditorInfo.LeftAlign();

            StaticLabel ChaiPremium = new StaticLabel("CHAI premium");
            ChaiPremium.Content.FontSize = Units.FontSizeM;
            ChaiPremium.Content.FontFamily = Fonts.GetBoldAppFont();
            ChaiPremium.Content.TextColor = Color.White;
            ChaiPremium.LeftAlign();

            ColourButton ChaiPremiumInfoButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "More info", null);
            ChaiPremiumInfoButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            ChaiPremiumInfoButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING; ;


            ChaiPremiumInfoButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               UpgradeType = Accounts.UPGRADE_PREMIUM;
                               PlanType = Accounts.PLAN_TYPE_NONE;
                               TargetType = Accounts.TARGET_NONE;

                               //NewPlanName = Accounts.GetAccountName(UpgradeType, PlanType, TargetType);
                               CurrentSection = UPGRADE_OPTIONS;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );

            


            StaticLabel ChaiPremiumInfo = new StaticLabel("A subscription In-App purchase to access Meal Plans, the Meal Planner itself, and Tracker functions.");
            ChaiPremiumInfo.Content.FontSize = Units.FontSizeS;
            ChaiPremiumInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            ChaiPremiumInfo.Content.TextColor = Color.White;
            ChaiPremiumInfo.LeftAlign();

            StaticLabel BottomInfo = new StaticLabel("Together, both upgrades unlock the full app version.");
            BottomInfo.Content.FontSize = Units.FontSizeM;
            BottomInfo.Content.FontFamily = Fonts.GetBoldAppFont();
            BottomInfo.Content.TextColor = Color.White;
            BottomInfo.CenterAlign();

            container.Children.Add(LeftTitle.Content, 0, 0);
            container.Children.Add(RightTitle.Content, 1, 0);
            container.Children.Add(TopInfo.Content, 0, 1);
            container.Children.Add(EditorPublisher.Content, 0, 2);
            container.Children.Add(EditorPublisherInfoButton.Content, 1, 2);
            container.Children.Add(EditorInfo.Content, 0, 3);
            container.Children.Add(ChaiPremium.Content, 0, 4);
            container.Children.Add(ChaiPremiumInfoButton.Content, 1, 4);
            container.Children.Add(ChaiPremiumInfo.Content, 0, 5);
            container.Children.Add(BottomInfo.Content, 0, 6);

            Grid.SetColumnSpan(TopInfo.Content, 2);
            Grid.SetColumnSpan(EditorInfo.Content, 2);
            Grid.SetColumnSpan(ChaiPremiumInfo.Content, 2);
            Grid.SetColumnSpan(BottomInfo.Content, 2);

            PlanOptionsContainer.Children.Add(container);

            return PlanOptionsContainer;
        }

        public override void Update()
        {
            if (this.Id != null)
            {
                Console.WriteLine("Updating Upgrade Panel: " + Id);
            }
        }

        private StackLayout BuildUpgradeOptions()
        {
            //return BuildUpgradeToEditorOrPublisher();
            return BuildUpgradeToPremium();
        }

        private StackLayout BuildUpgradeToEditorOrPublisher()
        {
            UpgradeOptionsContainer = new StackLayout
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


            StaticLabel LeftTitle = new StaticLabel("Upgrade");
            LeftTitle.Content.FontSize = Units.FontSizeM;
            LeftTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            LeftTitle.Content.TextColor = Color.White;
            LeftTitle.LeftAlign();

            StaticLabel RightTitle = new StaticLabel(AppSession.CurrentUser.Preferences.AccountName);
            RightTitle.Content.FontSize = Units.FontSizeM;
            RightTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            RightTitle.Content.TextColor = Color.White;
            RightTitle.RightAlign();
            RightTitle.Content.SetBinding(Label.TextProperty, new Binding(nameof(AppSession.CurrentUser.Preferences.AccountName), source: AppSession.CurrentUser.Preferences));


            StaticLabel TopInfo = new StaticLabel("Would you like to upgrade your account to be an Editor / Publisher?");
            TopInfo.Content.FontSize = Units.FontSizeS;
            TopInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            TopInfo.Content.TextColor = Color.White;
            TopInfo.CenterAlign();

            StaticLabel Price = new StaticLabel("");
            FormattedString fs = new FormattedString();
            fs.Spans.Add(new Span { Text = "One off fee of: ", ForegroundColor = Color.Red, FontSize = Units.FontSizeM, FontFamily = Fonts.GetBoldAppFont(), TextColor = Color.White });
            fs.Spans.Add(new Span { Text = " £1.99 ", ForegroundColor = Color.Red, FontSize = Units.FontSizeM, FontFamily = Fonts.GetBoldAppFont(), TextColor = Color.FromHex(Colors.CC_ORANGE) });
            Price.Content.FormattedText = fs;
            Price.LeftAlign();

            ColourButton UpgradeButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Upgrade", null);
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
                               //NewPlanName = Accounts.GetAccountName(Accounts..ChaiEditorPublisher);
                               //SetSection(CurrentSection);

                               UpgradeType = Accounts.UPGRADE_EDITOR_OR_PUBLISHER;
                               PlanType = Accounts.PLAN_TYPE_NONE;
                               TargetType = Accounts.TARGET_NONE;

                               App.ShowAlert("Coming soon!");
                           });
                       })
                   }
               );

            StaticLabel PlanInfo = new StaticLabel("This upgrade will unlock access to the Recipe Editor and associated tools - find them in Your Menu once you've confirmed the purchase. It includes all you'll need to write our and publish your recipes on CHAI.\n\nWhy not share those delicious meakls you've perfected? It'll help the CHAI community to grow, and it's a great way to start making a name for yourself. High ratings and you might just end up being the latest influencer.");
            PlanInfo.Content.FontSize = Units.FontSizeS;
            PlanInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            PlanInfo.Content.TextColor = Color.White;
            PlanInfo.LeftAlign();

            StaticLabel TryPremiumTitle = new StaticLabel("Get creating");
            TryPremiumTitle.Content.FontSize = Units.FontSizeS;
            TryPremiumTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            TryPremiumTitle.Content.TextColor = Color.White;
            TryPremiumTitle.LeftAlign();

            StaticLabel TryPremiumTagline = new StaticLabel("- quick and easy, ingenious - it's all good!!");
            TryPremiumTagline.Content.FontSize = Units.FontSizeS;
            TryPremiumTagline.Content.FontFamily = Fonts.GetRegularAppFont();
            TryPremiumTagline.Content.TextColor = Color.White;
            TryPremiumTagline.LeftAlign();

            container.Children.Add(LeftTitle.Content, 0, 0);
            container.Children.Add(RightTitle.Content, 1, 0);
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

            UpgradeOptionsContainer.Children.Add(container);

            return UpgradeOptionsContainer;
        }

        private StackLayout BuildUpgradeToPremium()
        {
            UpgradeOptionsContainer = new StackLayout
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
            

            StaticLabel LeftTitle = new StaticLabel("Upgrade");
            LeftTitle.Content.FontSize = Units.FontSizeM;
            LeftTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            LeftTitle.Content.TextColor = Color.White;
            LeftTitle.LeftAlign();

            StaticLabel RightTitle = new StaticLabel(AppSession.CurrentUser.Preferences.AccountName);
            RightTitle.Content.FontSize = Units.FontSizeM;
            RightTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            RightTitle.Content.TextColor = Color.White;
            RightTitle.RightAlign();
            RightTitle.Content.SetBinding(Label.TextProperty, new Binding(nameof(AppSession.CurrentUser.Preferences.AccountName), source: AppSession.CurrentUser));


            StaticLabel TopInfo = new StaticLabel("Would you like to upgrade to the CHAI premium meal plan?");
            TopInfo.Content.FontSize = Units.FontSizeS;
            TopInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            TopInfo.Content.TextColor = Color.White;
            TopInfo.CenterAlign();

            StaticLabel Price = new StaticLabel("");
            FormattedString fs = new FormattedString();
            fs.Spans.Add(new Span { Text = "Monthly fee of: ", ForegroundColor = Color.Red, FontSize = Units.FontSizeM, FontFamily = Fonts.GetBoldAppFont(), TextColor = Color.White });
            fs.Spans.Add(new Span { Text = " £1.99 ", ForegroundColor = Color.Red, FontSize = Units.FontSizeM, FontFamily = Fonts.GetBoldAppFont(), TextColor = Color.FromHex(Colors.CC_ORANGE) });
            Price.Content.FormattedText = fs;
            Price.LeftAlign();

            ColourButton UpgradeButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Upgrade", null);
            UpgradeButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            UpgradeButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING; ;

            UpgradeButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               UpgradeType = Accounts.UPGRADE_PREMIUM;
                               PlanType = Accounts.PLAN_TYPE_NONE;
                               TargetType = Accounts.TARGET_NONE;
                               //NewPlanName = Accounts.GetAccountName(UpgradeType, PlanType, TargetType);

                               CurrentSection = MEAL_PLAN_STRATEGY;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );

            StaticLabel PlanInfo = new StaticLabel("This upgrade will aid you on your journey from 'meat eater' to 'plant-based eater'. Whether you're looking to start gently or go the whole way to transitioning then we can help you do it.\n\nMore importantly, we can help you to do it within your budget and applied preferences within the app.");
            PlanInfo.Content.FontSize = Units.FontSizeS;
            PlanInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            PlanInfo.Content.TextColor = Color.White;
            PlanInfo.LeftAlign();

            StaticLabel TryPremiumTitle = new StaticLabel("Try CHAI premium");
            TryPremiumTitle.Content.FontSize = Units.FontSizeS;
            TryPremiumTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            TryPremiumTitle.Content.TextColor = Color.White;
            TryPremiumTitle.LeftAlign();

            StaticLabel TryPremiumTagline = new StaticLabel("- the best way to start your plant-based journey.");
            TryPremiumTagline.Content.FontSize = Units.FontSizeS;
            TryPremiumTagline.Content.FontFamily = Fonts.GetRegularAppFont();
            TryPremiumTagline.Content.TextColor = Color.White;
            TryPremiumTagline.LeftAlign();

            container.Children.Add(LeftTitle.Content, 0, 0);
            container.Children.Add(RightTitle.Content, 1, 0);
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
            
            UpgradeOptionsContainer.Children.Add(container);

            return UpgradeOptionsContainer;
        }

        private StackLayout BuildMealPlanStrategy()
        {
            MealPlanStrategyContainer = new StackLayout
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
            

            StaticLabel LeftTitle = new StaticLabel("Upgrade");
            LeftTitle.Content.FontSize = Units.FontSizeM;
            LeftTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            LeftTitle.Content.TextColor = Color.White;
            LeftTitle.LeftAlign();

            StaticLabel RightTitle = new StaticLabel("New Plan: " + NewPlanName);
            RightTitle.Content.FontSize = Units.FontSizeM;
            RightTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            RightTitle.Content.TextColor = Color.White;
            RightTitle.RightAlign();

            StaticLabel TopInfo = new StaticLabel("Choose your preferred option:");
            TopInfo.Content.FontSize = Units.FontSizeS;
            TopInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            TopInfo.Content.TextColor = Color.White;
            TopInfo.CenterAlign();

            ColourButton TransitionButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Transition", null);
            TransitionButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            TransitionButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING; ;

            TransitionButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               PlanType = Accounts.PLAN_TYPE_TRANSITION;
                               TargetType = Accounts.TARGET_NONE;
                               //NewPlanName = Accounts.GetAccountName(UpgradeType, PlanType, TargetType);
                               CurrentSection = TARGET_OPTIONS;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );
            ColourButton FlexitarianButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Flexitarian", null);
            FlexitarianButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            FlexitarianButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING; ;

            FlexitarianButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               PlanType = Accounts.PLAN_TYPE_FLEXITARIAN;
                               TargetType = Accounts.TARGET_NONE;
                               //NewPlanName = Accounts.GetAccountName(UpgradeType, PlanType, TargetType);
                               CurrentSection = TARGET_OPTIONS;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );
            StaticLabel TransitionInfo = new StaticLabel("Gradual transition to veganism");
            TransitionInfo.Content.FontSize = Units.FontSizeS;
            TransitionInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            TransitionInfo.Content.TextColor = Color.White;
            TransitionInfo.CenterAlign();

            StaticLabel FlexitarianInfo = new StaticLabel("Cut down on how much meat you eat");
            FlexitarianInfo.Content.FontSize = Units.FontSizeS;
            FlexitarianInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            FlexitarianInfo.Content.TextColor = Color.White;
            FlexitarianInfo.CenterAlign();

            StaticLabel ChoiceInfo = new StaticLabel("Your choice determines the overall meal plan strategy and will tailor the content to help you reach your target. If you want to change your option after you've upgraded, you can do so via Your Menu at any time.");
            ChoiceInfo.Content.FontSize = Units.FontSizeS;
            ChoiceInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            ChoiceInfo.Content.TextColor = Color.White;
            ChoiceInfo.LeftAlign();

            container.Children.Add(LeftTitle.Content, 0, 0);
            container.Children.Add(RightTitle.Content, 1, 0);
            container.Children.Add(TopInfo.Content, 0, 1);
            container.Children.Add(TransitionButton.Content, 0, 2);
            container.Children.Add(FlexitarianButton.Content, 1, 2);
            container.Children.Add(TransitionInfo.Content, 0, 3);
            container.Children.Add(FlexitarianInfo.Content, 1, 3);
            container.Children.Add(ChoiceInfo.Content, 0, 4);

            Grid.SetColumnSpan(TopInfo.Content, 2);
            Grid.SetColumnSpan(ChoiceInfo.Content, 2);

            MealPlanStrategyContainer.Children.Add(container);

            return MealPlanStrategyContainer;
        }

        private StackLayout BuildTargetOptions()
        {
            TargetOptionsContainer = new StackLayout
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
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            StaticLabel LeftTitle = new StaticLabel("Upgrade");
            LeftTitle.Content.FontSize = Units.FontSizeM;
            LeftTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            LeftTitle.Content.TextColor = Color.White;
            LeftTitle.LeftAlign();

            StaticLabel RightTitle = new StaticLabel("New Plan: " + NewPlanName);
            RightTitle.Content.FontSize = Units.FontSizeM;
            RightTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            RightTitle.Content.TextColor = Color.White;
            RightTitle.RightAlign();

            StaticLabel TopInfo = new StaticLabel("Choose your preferred option:");
            TopInfo.Content.FontSize = Units.FontSizeS;
            TopInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            TopInfo.Content.TextColor = Color.White;
            TopInfo.CenterAlign();

            ColourButton BuildMuscleButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Build Muscle", null);
            BuildMuscleButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            BuildMuscleButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING; ;

            BuildMuscleButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               TargetType = Accounts.TARGET_BUILD_MUSCLE;
                               //NewPlanName = Accounts.GetAccountName(UpgradeType, PlanType, TargetType);
                               CurrentSection = STATS_OPTIONS;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );

            StaticLabel BuildMuscleInfo = new StaticLabel("Premium Meal Plan with targets");
            BuildMuscleInfo.Content.FontSize = Units.FontSizeS;
            BuildMuscleInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            BuildMuscleInfo.Content.TextColor = Color.White;
            BuildMuscleInfo.CenterAlign();

            ColourButton LoseWeightButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Lose Weight", null);
            LoseWeightButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            LoseWeightButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING; ;

            LoseWeightButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               TargetType = Accounts.TARGET_LOSE_WEIGHT;
                               //NewPlanName = Accounts.GetAccountName(UpgradeType, PlanType, TargetType);
                               CurrentSection = STATS_OPTIONS;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );
            StaticLabel LoseWeightInfo = new StaticLabel("Premium Meal Plan with targets");
            LoseWeightInfo.Content.FontSize = Units.FontSizeS;
            LoseWeightInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            LoseWeightInfo.Content.TextColor = Color.White;
            LoseWeightInfo.CenterAlign();

            ColourButton NeitherButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Neither", null);
            NeitherButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            NeitherButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING; ;

            NeitherButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               TargetType = Accounts.TARGET_NONE;
                               //NewPlanName = Accounts.GetAccountName(UpgradeType, PlanType, TargetType);
                               CurrentSection = STATS_OPTIONS;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );
            StaticLabel NeitherInfo = new StaticLabel("Premium Meal Plan without targets");
            NeitherInfo.Content.FontSize = Units.FontSizeS;
            NeitherInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            NeitherInfo.Content.TextColor = Color.White;
            NeitherInfo.CenterAlign();

            ColourButton StepBackButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Step Back", null);
            StepBackButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            StepBackButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING; ;


            StepBackButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               CurrentSection = MEAL_PLAN_STRATEGY;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );

            container.Children.Add(LeftTitle.Content, 0, 0);
            container.Children.Add(RightTitle.Content, 1, 0);
            container.Children.Add(TopInfo.Content, 0, 1);
            container.Children.Add(BuildMuscleButton.Content, 0, 2);
            container.Children.Add(LoseWeightButton.Content, 1, 2);
            container.Children.Add(BuildMuscleInfo.Content, 0, 3);
            container.Children.Add(LoseWeightInfo.Content, 1, 3);
            container.Children.Add(NeitherButton.Content, 0, 4);
            container.Children.Add(NeitherInfo.Content, 0, 5);
            container.Children.Add(StepBackButton.Content, 0, 6);



            Grid.SetColumnSpan(TopInfo.Content, 2);
            Grid.SetColumnSpan(NeitherButton.Content, 2);
            Grid.SetColumnSpan(NeitherInfo.Content, 2);
            

            TargetOptionsContainer.Children.Add(container);

            return TargetOptionsContainer;
        }

        private StackLayout BuildStatsOptions()
        {
            StatsOptionsContainer = new StackLayout
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
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });


            StaticLabel LeftTitle = new StaticLabel("Upgrade");
            LeftTitle.Content.FontSize = Units.FontSizeM;
            LeftTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            LeftTitle.Content.TextColor = Color.White;
            LeftTitle.LeftAlign();

            StaticLabel RightTitle = new StaticLabel("New Plan: " + NewPlanName);
            RightTitle.Content.FontSize = Units.FontSizeM;
            RightTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            RightTitle.Content.TextColor = Color.White;
            RightTitle.RightAlign();

            StaticLabel TopInfo = new StaticLabel("Choose your preferred option:");
            TopInfo.Content.FontSize = Units.FontSizeS;
            TopInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            TopInfo.Content.TextColor = Color.White;
            TopInfo.CenterAlign();

            ValueAdjuster HeightAdjuster = new ValueAdjuster("", Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            HeightAdjuster.SetFloatValueRange(1.0f, 3.0f, 1.75f, 0.01f);

            StaticLabel HeightInfo = new StaticLabel("Height (m)");
            HeightInfo.Content.FontSize = Units.FontSizeS;
            HeightInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            HeightInfo.Content.TextColor = Color.White;
            HeightInfo.CenterAlign();

            ValueAdjuster CurrentWeightAdjuster = new ValueAdjuster("", Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            CurrentWeightAdjuster.SetFloatValueRange(20.0f, 1000.0f, 80.0f, 1.0f);

            
            StaticLabel CurrentWeightInfo = new StaticLabel("Current Weight (kg)");
            CurrentWeightInfo.Content.FontSize = Units.FontSizeS;
            CurrentWeightInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            CurrentWeightInfo.Content.TextColor = Color.White;
            CurrentWeightInfo.CenterAlign();

            ValueAdjuster TargetWeightAdjuster = new ValueAdjuster("", Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            TargetWeightAdjuster.SetFloatValueRange(20.0f, 1000.0f, 80.0f, 1.0f);
            TargetWeightAdjuster.CenterAlign();

            StaticLabel TargetWeightInfo = new StaticLabel("Target Weight (kg)");
            TargetWeightInfo.Content.FontSize = Units.FontSizeS;
            TargetWeightInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            TargetWeightInfo.Content.TextColor = Color.White;
            TargetWeightInfo.CenterAlign();

            ColourButton StepBackButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Step Back", null);

            StepBackButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            StepBackButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING; ;
            StepBackButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               CurrentSection = TARGET_OPTIONS;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );

            ColourButton ProceedButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Proceed", null);
            ProceedButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            ProceedButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING; ;

            ProceedButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               CurrentSection = CONFIRMATION;
                               SetSection(CurrentSection);
                               //await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.UpgradePlan);

                           });
                       })
                   }
               );

            container.Children.Add(LeftTitle.Content, 0, 0);
            container.Children.Add(RightTitle.Content, 1, 0);
            container.Children.Add(TopInfo.Content, 0, 1);
            container.Children.Add(HeightAdjuster.GetContent(), 0, 2);
            container.Children.Add(CurrentWeightAdjuster.GetContent(), 1, 2);
            container.Children.Add(HeightInfo.Content, 0, 3);
            container.Children.Add(CurrentWeightInfo.Content, 1, 3);
            container.Children.Add(TargetWeightAdjuster.GetContent(), 0, 4);
            container.Children.Add(TargetWeightInfo.Content, 0, 5);
            container.Children.Add(StepBackButton.Content, 0, 6);
            container.Children.Add(ProceedButton.Content, 1, 6);


            Grid.SetColumnSpan(TopInfo.Content, 2);
            Grid.SetColumnSpan(TargetWeightAdjuster.GetContent(), 2);
            Grid.SetColumnSpan(TargetWeightInfo.Content, 2);


            StatsOptionsContainer.Children.Add(container);

            return StatsOptionsContainer;
        }

        private StackLayout BuildConfirmation()
        {
            ConfirmationContainer = new StackLayout
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
            

            StaticLabel LeftTitle = new StaticLabel("Upgrade");
            LeftTitle.Content.FontSize = Units.FontSizeM;
            LeftTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            LeftTitle.Content.TextColor = Color.White;
            LeftTitle.LeftAlign();

            StaticLabel RightTitle = new StaticLabel("New Plan: " + NewPlanName);
            RightTitle.Content.FontSize = Units.FontSizeM;
            RightTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            RightTitle.Content.TextColor = Color.White;
            RightTitle.RightAlign();

            StaticLabel PriceInfo = new StaticLabel("");
            FormattedString fs = new FormattedString();
            fs.Spans.Add(new Span { Text = "Monthly fee of: ", ForegroundColor = Color.Red, FontSize = Units.FontSizeM, FontFamily = Fonts.GetBoldAppFont(), TextColor = Color.White });
            fs.Spans.Add(new Span { Text = " £1.99 ", ForegroundColor = Color.Red, FontSize = Units.FontSizeM, FontFamily = Fonts.GetBoldAppFont(), TextColor = Color.FromHex(Colors.CC_ORANGE) });
            fs.Spans.Add(new Span { Text = "(cancel anything)", ForegroundColor = Color.Red, FontSize = Units.FontSizeM, FontFamily = Fonts.GetBoldAppFont(), TextColor = Color.White });
            PriceInfo.Content.FormattedText = fs;
            PriceInfo.CenterAlign();

            StaticLabel TopInfo = new StaticLabel("Tapping submit will subscribe you to CHAI premium via the in-app purchase process of your Android / iOS App Store.\n\nAfterwards, if you wish to cancel then you can do so via Account Overview in Account Preferences or your subscription settings on your device.\n\nWe hope you enjoy the extra featues!!");
            TopInfo.Content.FontSize = Units.FontSizeS;
            TopInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            TopInfo.Content.TextColor = Color.White;
            TopInfo.CenterAlign();

            ColourButton CancelButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Cancel", null);
            CancelButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            CancelButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING; ;

            CancelButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               CurrentSection = STATS_OPTIONS;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );

            ColourButton SubmitButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Submit", null);
            SubmitButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            SubmitButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING; ;

            SubmitButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               await PerformSubmit();
                           });
                       })
                   }
               );

            container.Children.Add(LeftTitle.Content, 0, 0);
            container.Children.Add(RightTitle.Content, 1, 0);
            container.Children.Add(PriceInfo.Content, 0, 1);
            container.Children.Add(TopInfo.Content, 0, 2);
            container.Children.Add(CancelButton.Content, 0, 3);
            container.Children.Add(SubmitButton.Content, 1, 3);

            Grid.SetColumnSpan(PriceInfo.Content, 2);
            Grid.SetColumnSpan(TopInfo.Content, 2);

            ConfirmationContainer.Children.Add(container);


            return ConfirmationContainer;
        }

        private StackLayout BuildUpgradeSuccess()
        {
            UpgradeSuccessContainer = new StackLayout
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
            
            StaticLabel LeftTitle = new StaticLabel("Upgraded");
            LeftTitle.Content.FontSize = Units.FontSizeM;
            LeftTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            LeftTitle.Content.TextColor = Color.White;
            LeftTitle.LeftAlign();

            StaticLabel NewPlanInfo = new StaticLabel("You're on: ");// + Accounts.GetAccountName(UpgradeType, PlanType, TargetType));
            NewPlanInfo.Content.FontSize = Units.FontSizeM;
            NewPlanInfo.Content.FontFamily = Fonts.GetBoldAppFont();
            NewPlanInfo.Content.TextColor = Color.White;
            NewPlanInfo.CenterAlign();

            StaticLabel ReceiptInfo = new StaticLabel("A receipt for your order will be sent to your email.");
            ReceiptInfo.Content.FontSize = Units.FontSizeS;
            ReceiptInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            ReceiptInfo.Content.TextColor = Color.White;
            ReceiptInfo.CenterAlign();

            StaticImage SuccessImage = new StaticImage("faveblack.png", Units.QuarterScreenWidth, Units.QuarterScreenWidth, null);

            StaticLabel PickPlanInfo = new StaticLabel("Pick your plan now");
            PickPlanInfo.Content.FontSize = Units.FontSizeS;
            PickPlanInfo.Content.FontFamily = Fonts.GetRegularAppFont();
            PickPlanInfo.Content.TextColor = Color.White;
            PickPlanInfo.CenterAlign();

            ColourButton PlansButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Open", new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.Landing));
            PlansButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            PlansButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING; ;

            /*
            PlansButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               CurrentSection = STATS_OPTIONS;
                               SetSection(CurrentSection);


                           });
                       })
                   }
               );*/

            container.Children.Add(LeftTitle.Content, 0, 0);
            container.Children.Add(NewPlanInfo.Content, 0, 1);
            container.Children.Add(ReceiptInfo.Content, 0, 2);
            container.Children.Add(SuccessImage.Content, 0, 3);
            container.Children.Add(PickPlanInfo.Content, 0, 4);
            container.Children.Add(PlansButton.Content, 0, 5);

            Grid.SetColumnSpan(NewPlanInfo.Content, 2);
            Grid.SetColumnSpan(ReceiptInfo.Content, 2);
            Grid.SetColumnSpan(SuccessImage.Content, 2);
            Grid.SetColumnSpan(PickPlanInfo.Content, 2);
            Grid.SetColumnSpan(PlansButton.Content, 2);
           
            UpgradeSuccessContainer.Children.Add(container);

            return UpgradeSuccessContainer;
        }

        private StackLayout BuildUpgradeFailure()
        {
            UpgradeFailureContainer = new StackLayout
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
            
            StaticLabel LeftTitle = new StaticLabel("Warning");
            LeftTitle.Content.FontSize = Units.FontSizeM;
            LeftTitle.Content.FontFamily = Fonts.GetBoldAppFont();
            LeftTitle.Content.TextColor = Color.White;
            LeftTitle.LeftAlign();

            
            StaticLabel FailInfo = new StaticLabel("Unable to process upgrade at this time");
            FailInfo.Content.FontSize = Units.FontSizeM;
            FailInfo.Content.FontFamily = Fonts.GetBoldAppFont();
            FailInfo.Content.TextColor = Color.White;
            FailInfo.LeftAlign();

            StaticImage FailImage = new StaticImage("homeicon.png", 64, 64, null);

            StaticLabel FailDetail = new StaticLabel("Please try again.\n\nIf this message continues to appear then please contact us for further assistance on:");
            FailDetail.Content.FontSize = Units.FontSizeM;
            FailDetail.Content.FontFamily = Fonts.GetRegularAppFont();
            FailDetail.Content.TextColor = Color.White;
            FailDetail.CenterAlign();

            StaticLabel ContactEmail = new StaticLabel(AppText.CONTACT_EMAIL_ADDRESS);
            ContactEmail.Content.FontSize = Units.FontSizeL;
            ContactEmail.Content.FontFamily = Fonts.GetBoldAppFont();
            ContactEmail.Content.TextColor = Color.White;
            ContactEmail.CenterAlign();

            ColourButton OkButton = new ColourButton(Color.FromHex(Colors.CC_ORANGE), Color.White, "Ok", null);
            OkButton.SetSize(Dimensions.STANDARD_BUTTON_WIDTH, Dimensions.STANDARD_BUTTON_HEIGHT);
            OkButton.Content.Margin = Dimensions.GENERAL_COMPONENT_SPACING; ;

            OkButton.Content.GestureRecognizers.Add(
                   new TapGestureRecognizer()
                   {
                       Command = new Command(() =>
                       {
                           Device.BeginInvokeOnMainThread(async () =>
                           {
                               CurrentSection = CONFIRMATION;
                               SetSection(CurrentSection);
                           });
                       })
                   }
               );
            

            container.Children.Add(LeftTitle.Content, 0, 0);
            container.Children.Add(FailImage.Content, 0, 1);
            container.Children.Add(FailInfo.Content, 1, 1);
            container.Children.Add(FailDetail.Content, 0, 2);
            container.Children.Add(ContactEmail.Content, 0, 3);
            container.Children.Add(OkButton.Content, 0, 4);

            Grid.SetColumnSpan(FailDetail.Content, 2);
            Grid.SetColumnSpan(ContactEmail.Content, 2);
            Grid.SetColumnSpan(OkButton.Content, 2);
            
            UpgradeFailureContainer.Children.Add(container);


            return UpgradeFailureContainer;
        }

        public async Task<bool> PerformSubmit()
        {
            await Task.Delay(1000); // simulate submission process

            bool success = true;

            if (success)
            {
                CurrentSection = UPGRADE_SUCCESS;
                SetSection(CurrentSection);
            }
            else
            {
                CurrentSection = UPGRADE_FAILURE;
                SetSection(CurrentSection);
            }
            return success;
        }
    }
}
