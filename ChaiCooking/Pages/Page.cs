using System;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using ChaiCooking.Branding;
using ChaiCooking.Helpers;
using ChaiCooking.Components.Images;
using Refractored.XamForms.PullToRefresh;
using Xamarin.Forms;

namespace ChaiCooking.Pages
{
    public class Page// : INotifyPropertyChanged
    {
        public int Id;
        public string Name;
        public Grid PageContent;
        public int ContentHeight;
        public string BackgroundImageSource;
        public StaticImage BackgroundImage;
        public int TransitionInType;
        public int TransitionOutType;
        public uint TransitionSpeed;
        public bool IsScrollable;
        public bool IsRefreshable;
        public bool NeedsRefreshing;
        public ScrollView contentScrollView;
        public Grid Content;
        public RefreshView RefreshView;
        public ICommand RefreshCommand { get; }

        // overrides for page specific layouts
        public bool HasHeader;
        public bool HasFooter;
        public bool HasSubHeader;
        public bool HasNavHeader;


        bool isRefreshing;

        public event PropertyChangedEventHandler PropertyChanged;
        public double ScrollYPosition;
        public double ScrollTriggerYPosition;

        public Page()
        {
            TransitionSpeed = (uint)AppSettings.TransitionVeryFast;
            ContentHeight = Units.ScreenHeight;
            IsScrollable = true;
            IsRefreshable = false;
            HasFooter = AppSettings.HasFooter;
            HasHeader = AppSettings.HasHeader;
            HasSubHeader = AppSettings.HasSubHeader;
            HasNavHeader = AppSettings.HasNavHeader;
            NeedsRefreshing = true;
            RefreshCommand = new Command(ExecuteRefreshCommand);
            ScrollYPosition = 0;
            ScrollTriggerYPosition = ScrollYPosition + Units.QuarterScreenHeight;
        }

       

        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                //OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        //private void OnPropertyChanged(string v)
        //{
        //    throw new NotImplementedException();
        //}

        public virtual void ExecuteRefreshCommand()
        {
            //Items.Clear();
            //Items.Add(new Item { Text = "Refreshed Data", Description = "Whoa!" });
            App.ShowAlert("Finished updating");
            // Stop refreshing
            //NeedsRefreshing = false;
            //This works better than a full page refresh, provided the updated data is correct.
            contentScrollView.ScrollToAsync(0, 0, true);

            IsRefreshing = false;
            RefreshView.IsRefreshing = false;
        }

        public virtual void ExecuteScrollCommand()
        {

        }

        public virtual Grid GetContent()
        {
            if (PageContent == null)
            {
                Create();
            }

            if (IsScrollable)
            {
                Content = new Grid
                {
                    VerticalOptions = LayoutOptions.StartAndExpand,
                };

                contentScrollView = new ScrollView
                {
                    Content = PageContent,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    MinimumHeightRequest = Units.ThirdScreenHeight,
                    BackgroundColor = Color.Transparent,
                    AnchorY = 0
                };

                contentScrollView.Scrolled += (object sender, ScrolledEventArgs e) =>
                {
                    ScrollYPosition = e.ScrollY;

                    //Console.WriteLine("Scrolled to " + ScrollYPosition);
                    //if (ScrollYPosition)
                    //App.ExecuteScrollEvent((int)ScrollYPosition);

                    //ScrollTriggerYPosition = -ScrollYPosition;

                    //Console.WriteLine("Scrolled to " + ScrollYPosition);

                    Console.WriteLine("Screen height " + Units.ThirdScreenHeight);
                    Console.WriteLine("Scrolled to " + ScrollYPosition);
                    Console.WriteLine("Scrolled trigger " + ScrollTriggerYPosition);

                    if (ScrollYPosition > ScrollTriggerYPosition)
                    {
                        Console.WriteLine("TRIGGER UPDATE AT " + ScrollTriggerYPosition);
                        ExecuteScrollCommand();
                        ScrollTriggerYPosition = ScrollYPosition + Units.ThirdScreenHeight;
                    }
                };

                if (IsRefreshable)
                {
                    RefreshView = new RefreshView
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Content = contentScrollView,
                        RefreshColor = Color.FromHex(Colors.REFRESH_SPINNER_COLOUR),
                        Command = RefreshCommand
                    };
                    Content.Children.Add(RefreshView);
                }
                else
                {
                    Content.Children.Add(contentScrollView);
                }

                return Content;
            }
            else
            {
                return PageContent;
            }
        }

        public virtual void AddBackgroundImage(string backgroundImageSource)
        {
            BackgroundImageSource = backgroundImageSource;
            BackgroundImage = new StaticImage(
            BackgroundImageSource,
            Units.ScreenWidth,
            Units.ScreenHeight,
            null);
            BackgroundImage.Content.Aspect = Aspect.Fill;
            PageContent.Children.Add(BackgroundImage.Content);
        }

        public virtual string GetBackgroundImageSource()
        {
            return BackgroundImageSource;
        }

        public virtual void Create()
        {
            PageContent = new Grid();
        }

        public virtual void Destroy()
        {
            PageContent = null;
        }

        public virtual async Task Update()
        {
            await Task.Delay(10);

            if (this.HasHeader)
            {
                App.ShowHeaderAsync();
            }
            else
            {
                App.HideHeaderAsync();
            }

            if (this.HasSubHeader)
            {
                App.ShowSubHeader();
            }
            else
            {
                App.HideSubHeader();
            }

            if (this.HasNavHeader)
            {
                App.ShowNavHeader();
            }
            else
            {
                App.HideNavHeader();
            }

            if (this.HasFooter)
            {
                App.ShowFooter();
            }
            else
            {
                App.HideFooter();
            }

            

        }

        public virtual async Task Reset()
        {
            await Task.Delay(10);
        }

        public virtual async Task ReBuild()
        {
            await Task.Delay(10);
        }

        public virtual async Task DebugUpdate(int time)
        {
            await Task.Delay(time);
        }

        public virtual void ResetScrollView()
        {
           
        }

        public virtual async Task TransitionIn()
        {
            Console.WriteLine("Transition: " + TransitionInType + " opacity before " + PageContent.Opacity + " position x " + PageContent.X + " position y " + PageContent.Y);

            switch (TransitionInType)
            {
                case (int)Helpers.Pages.TransitionTypes.FadeIn:
                    await Task.WhenAll(
                        PageContent.FadeTo(1, TransitionSpeed, Easing.Linear)
                    );
                    break;
                case (int)Helpers.Pages.TransitionTypes.ScaleIn:
                    await Task.WhenAll(
                        PageContent.ScaleTo(1, TransitionSpeed, Easing.Linear)
                    );
                    break;
                case (int)Helpers.Pages.TransitionTypes.RotateIn:
                    await Task.WhenAll(
                        PageContent.RotateTo(360, TransitionSpeed, Easing.Linear)
                    );
                    break;
                case (int)Helpers.Pages.TransitionTypes.ScaleAndRotateIn:
                    await Task.WhenAll(
                        PageContent.ScaleTo(1, TransitionSpeed, Easing.Linear),
                        PageContent.RotateTo(360, TransitionSpeed, Easing.Linear)
                    );
                    break;
                case (int)Helpers.Pages.TransitionTypes.ScaleRotateAndFadeIn:
                    await Task.WhenAll(
                        PageContent.FadeTo(1, TransitionSpeed, Easing.Linear),
                        PageContent.ScaleTo(1, TransitionSpeed, Easing.Linear),
                        PageContent.RotateTo(360, TransitionSpeed, Easing.Linear)
                    );
                    break;
                case (int)Helpers.Pages.TransitionTypes.SlideInFromLeft:
                case (int)Helpers.Pages.TransitionTypes.SlideInFromRight:
                    await Task.WhenAll(
                        PageContent.TranslateTo(0, 0, TransitionSpeed, Easing.Linear)
                    );
                    break;
                case (int)Helpers.Pages.TransitionTypes.SlideInFromTop:
                case (int)Helpers.Pages.TransitionTypes.SlideInFromBottom:
                    await Task.WhenAll(
                        PageContent.TranslateTo(0, 0, TransitionSpeed, Easing.Linear)
                    );
                    break;


            }
            Console.WriteLine("Transition: " + TransitionInType + " opacity after " + PageContent.Opacity + " position x " + PageContent.X + " position y " + PageContent.Y);
        }

        public virtual async Task TransitionOut()
        {
            switch (TransitionOutType)
            {

                case (int)Helpers.Pages.TransitionTypes.FadeOut:
                    await Task.WhenAll(
                        PageContent.FadeTo(0, TransitionSpeed, Easing.Linear)
                    );
                    break;
                case (int)Helpers.Pages.TransitionTypes.ScaleOut:
                    await Task.WhenAll(
                        PageContent.ScaleTo(0, TransitionSpeed, Easing.Linear)
                    );
                    break;
                case (int)Helpers.Pages.TransitionTypes.RotateOut:
                    await Task.WhenAll(
                        PageContent.RotateTo(-360, TransitionSpeed, Easing.Linear)
                    );
                    break;
                case (int)Helpers.Pages.TransitionTypes.ScaleAndRotateOut:
                    await Task.WhenAll(
                        PageContent.ScaleTo(0, TransitionSpeed, Easing.Linear),
                        PageContent.RotateTo(-360, TransitionSpeed, Easing.Linear)
                    );
                    break;
                case (int)Helpers.Pages.TransitionTypes.ScaleRotateAndFadeOut:
                    await Task.WhenAll(
                        PageContent.FadeTo(0, TransitionSpeed, Easing.Linear),
                        PageContent.ScaleTo(0, TransitionSpeed, Easing.Linear),
                        PageContent.RotateTo(-360, TransitionSpeed, Easing.Linear)
                    );
                    break;
                case (int)Helpers.Pages.TransitionTypes.SlideOutToLeft:
                    await Task.WhenAll(
                        PageContent.TranslateTo(-Units.ScreenWidth, 0, TransitionSpeed, Easing.Linear)
                    );
                    break;
                case (int)Helpers.Pages.TransitionTypes.SlideOutToRight:
                    await Task.WhenAll(
                        PageContent.TranslateTo(Units.ScreenWidth, 0, TransitionSpeed, Easing.Linear)
                    );
                    break;
                case (int)Helpers.Pages.TransitionTypes.SlideOutToTop:
                    await Task.WhenAll(
                        PageContent.TranslateTo(0, -Units.ScreenHeight, TransitionSpeed, Easing.Linear)
                    );
                    break;
                case (int)Helpers.Pages.TransitionTypes.SlideOutToBottom:
                    await Task.WhenAll(
                        PageContent.TranslateTo(0, Units.ScreenHeight, TransitionSpeed, Easing.Linear)
                    );
                    break;
            }

        }

        public virtual async Task PositionPage()
        {
            switch(TransitionInType)
            {
                case (int)Helpers.Pages.TransitionTypes.FadeIn: 
                case (int)Helpers.Pages.TransitionTypes.ScaleIn:
                case (int)Helpers.Pages.TransitionTypes.RotateIn:
                case (int)Helpers.Pages.TransitionTypes.ScaleAndRotateIn:
                case (int)Helpers.Pages.TransitionTypes.ScaleRotateAndFadeIn:
                    await PageContent.TranslateTo(0, 0, 0, Easing.Linear);
                    break;
                case (int)Helpers.Pages.TransitionTypes.SlideInFromLeft:
                    await Task.WhenAll(
                        PageContent.TranslateTo(-Units.ScreenWidth, 0, 0, Easing.Linear)
                    );
                    break;
                case (int)Helpers.Pages.TransitionTypes.SlideInFromRight:
                    await Task.WhenAll(
                       PageContent.TranslateTo(Units.ScreenWidth, 0, 0, Easing.Linear)
                    );
                    break;
                case (int)Helpers.Pages.TransitionTypes.SlideInFromTop:
                    await Task.WhenAll(
                       PageContent.TranslateTo(0, -Units.ScreenHeight, 0, Easing.Linear)
                    );
                    break;
                case (int)Helpers.Pages.TransitionTypes.SlideInFromBottom:
                    await Task.WhenAll(
                       PageContent.TranslateTo(0,-Units.ScreenHeight, 0, Easing.Linear)
                    );
                    break;
                
            }
        }

        /*
        public class TestClassThing : INotifyPropertyChanged
        {

            bool canRefresh = true;

            public bool CanRefresh
            {
                get { return canRefresh; }
                set
                {
                    if (canRefresh == value)
                        return;

                    canRefresh = value;
                    OnPropertyChanged("CanRefresh");
                }
            }


            bool isBusy;

            public bool IsBusy
            {
                get { return isBusy; }
                set
                {
                    if (isBusy == value)
                        return;

                    isBusy = value;
                    OnPropertyChanged("IsBusy");
                }
            }

            ICommand refreshCommand;

            public ICommand RefreshCommand
            {
                get { return refreshCommand ?? (refreshCommand = new Command(async () => await ExecuteRefreshCommand())); }
            }

            async Task ExecuteRefreshCommand()
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                //Items.Clear();

                Device.StartTimer(TimeSpan.FromSeconds(5), () =>
                {

                    //for (int i = 0; i < 100; i++)
                    //    Items.Add(DateTime.Now.AddMinutes(i).ToString("F"));

                    IsBusy = false;

                    //DisplayAlert("Refreshed", "You just refreshed the page! Nice job! Pull to refresh is now disabled", "OK");
                    this.CanRefresh = false;

                    return false;
                });
            }

            #region INotifyPropertyChanged implementation

            public event PropertyChangedEventHandler PropertyChanged;

            #endregion

            public void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }*/
    }
}
