using System;
using System.Threading.Tasks;
using ChaiCooking.AppData;
using ChaiCooking.Branding;
using ChaiCooking.Components.Images;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Layouts.Custom;
using ChaiCooking.Layouts.Custom.Modals;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.MealPlanAPI;
using ChaiCooking.Views.CollectionViews.InfluencerMealPreview;
using ChaiCooking.Views.CollectionViews.SingleInfluencer;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ChaiCooking.Pages.Custom
{
    public class SingleInfluencer : Page
    {
        StackLayout NavContent;
        StaticImage InfluencerImage;
        Paragraph InfluencerIntro, generalInfluencerInfo;
        Influencer.Datum Influencer;
        InfluencerMealPlanCollectionView influencerMealPlanCollectionView;
        PreviewCollectionView previewCollectionView;
        CollectionView influencerMealPlanLayout, previewLayout;
        bool isBusy { get; set; }
        public SingleInfluencer()
        {
            this.IsScrollable = true;// false;
            this.IsRefreshable = false;
            this.HasHeader = true;
            this.HasSubHeader = true;
            this.HasNavHeader = false;
            this.HasFooter = false;

            this.Id = (int)AppSettings.PageNames.SingleInfluencer;
            this.Name = AppData.AppText.INFLUENCERS;
            this.TransitionInType = (int)Helpers.Pages.TransitionTypes.SlideInFromRight;
            this.TransitionOutType = (int)Helpers.Pages.TransitionTypes.SlideOutToRight;
            StaticData.callTemplateView = BuildPreviewView;
            StaticData.isTemplateView = false;
            PageContent = new Grid
            {
                BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY)/*,
                RowDefinitions =
                {
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                }*/
            };
        }

        public void BuildContentNew()
        {
            StackLayout closeContent = new StackLayout { BackgroundColor = Color.FromHex(Colors.CC_BLUE_GREY), WidthRequest = Units.ScreenWidth, HeightRequest = Dimensions.NAVHEADER_HEIGHT, Orientation = StackOrientation.Horizontal, Padding = Dimensions.GENERAL_COMPONENT_PADDING };

            Label influencerLabel = new Label
            {
                Text = AppSession.SelectedInfluencer.display_name,
                FontAttributes = FontAttributes.Bold,
                FontSize = Units.FontSizeL,
                TextColor = Color.White
            };

            ActiveLabel CloseLabel = new ActiveLabel(AppText.CLOSE, Units.FontSizeM, Color.Transparent, Color.White, null);
            TouchEffect.SetNativeAnimation(CloseLabel.Content, true);
            TouchEffect.SetCommand(CloseLabel.Content, new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.PerformActionAsync((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.HealthyLiving);
                });
            }));
            CloseLabel.RightAlign();

            closeContent.Children.Add(influencerLabel);
            closeContent.Children.Add(CloseLabel.Content);

            if (AppSession.SelectedInfluencer != null)
            {
                Influencer = AppSession.SelectedInfluencer;
            }

            if (Influencer.image_url == null || Influencer.image_url == "")
            {
                Influencer.image_url = "charactericon.png";
            }

            InfluencerImage = new StaticImage(Influencer.image_url, Units.HalfScreenWidth, null);

            StackLayout Seperator1 = new StackLayout { WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) };
            StackLayout Seperator2 = new StackLayout { WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) };
            StackLayout Seperator3 = new StackLayout { WidthRequest = Units.ScreenWidth, HeightRequest = 1, BackgroundColor = Color.FromHex(Colors.CC_PALE_GREY) };

            StaticLabel bioLabel = new StaticLabel("Bio");
            bioLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            bioLabel.Content.TextColor = Color.White;
            bioLabel.LeftAlign();

            if (Influencer.bio == null)
            {
                Influencer.bio = "No bio information.";
            }
            InfluencerIntro = new Paragraph("", Influencer.bio, null);
            InfluencerIntro.SetTextColor(Color.White);

            generalInfluencerInfo = new Paragraph("", "All our experts have worked tirelessly to create plans which take the hard work out of deciding what to eat each day and how to minimise waste.", null);
            generalInfluencerInfo.SetTextColor(Color.White);

            StaticLabel mealPlansLabel = new StaticLabel(AppText.MEAL_PLANS);
            mealPlansLabel.Content.FontFamily = Fonts.GetBoldAppFont();
            mealPlansLabel.Content.TextColor = Color.White;
            mealPlansLabel.LeftAlign();

            StackLayout mainContainer = new StackLayout
            {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                Orientation = StackOrientation.Vertical,
                Spacing = 5,
                Padding = new Thickness(Dimensions.GENERAL_COMPONENT_PADDING),
                Children =
                {
                    InfluencerImage.Content,
                    Seperator1,
                    bioLabel.Content,
                    InfluencerIntro.MainContent.Content,
                    Seperator2,
                    generalInfluencerInfo.MainContent.Content,
                    Seperator3,
                    mealPlansLabel.Content,
                    BuildCollectionView()
                }
            };

            ScrollView scrollView = new ScrollView
            {
                Content = mainContainer
            };

            PageContent.Children.Add(new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = Dimensions.GENERAL_COMPONENT_SPACING,
                Children =
                {
                    closeContent,
                    scrollView
                }
            });
            //PageContent.Children.Add(closeContent, 0, 0);
            //PageContent.Children.Add(scrollView, 0, 1);

            //Task.Run(async() =>
            //{
            //    await Task.Delay(500);
            //    Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            //    {
            //        PageContent.ForceLayout();
            //    });
            //});
            
        }

        private CollectionView BuildCollectionView()
        {
            influencerMealPlanCollectionView = new InfluencerMealPlanCollectionView();
            influencerMealPlanLayout = influencerMealPlanCollectionView.GetCollectionView();
            influencerMealPlanCollectionView.ShowMealPlans();
            return influencerMealPlanLayout;
        }

        public override Task Update()
        {
            PageContent.Children.Clear();
            App.SetSubHeaderTitle("", null);
            App.SetSubHeaderTitle(AppText.RECOMMENDED_RECIPES, new Models.Action((int)Actions.ActionName.GoToPage, (int)AppSettings.PageNames.RecommendedRecipes));
            BuildPage();
            return base.Update();
        }

        public void BuildPage()
        {
            if (!StaticData.isTemplateView)
            {
                BuildContentNew();
            }
        }

        public async void CreateNewMealPlan()
        {
            await App.ShowCreateMealPlanPopup();
        }

        public void BuildPreviewView(string name, int numOfWeeks)
        {
            PageContent.Children.Clear();

            // this fails if we come from the browse list...
            PageContent.Children.Add(new MealPreviewModal("" + numOfWeeks, name, true).GetContent());
        }

        public CollectionView BuildCollectionView(string mealPlanName, int week)
        {
            try
            {
                previewCollectionView = new PreviewCollectionView();
                previewLayout = previewCollectionView.GetCollectionView();
                previewCollectionView.ShowCalendar(week.ToString(), mealPlanName);
                return previewLayout;
            }
            catch
            {
                return previewLayout;
            }
        }
    }
}

