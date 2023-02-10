using System;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Plugin.Toasts;
using Refractored.XamForms.PullToRefresh.Droid;
using MediaManager;
using Octane.Xamarin.Forms.VideoPlayer.Android;
using Android.OS;
using Android.App;
using Android.Views;
using Android.Runtime;
using Android.Content.PM;

namespace ChaiCooking.Droid
{
    [Activity(Label = "Chai Cooking", Icon = "@mipmap/icon", Theme = "@style/MainTheme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, Exported = true)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private App _app;

        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = MediaManager.Forms.Resource.Layout.Tabbar;
            ToolbarResource = MediaManager.Forms.Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            //CrossMediaManager.Current.Init();

            PullToRefreshLayoutRenderer.Init();

            //this.Window.AddFlags(WindowManagerFlags.Fullscreen);
            //this.Window.ClearFlags(WindowManagerFlags.Fullscreen);

            //this.Window.SetStatusBarColor(Android.Graphics.Color.Black);
            Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);


            //Xamarin.Forms.Forms.SetTitleBarVisibility(global::Xamarin.Forms.AndroidTitleBarVisibility.Default);

            DependencyService.Register<ToastNotification>();
            ToastNotification.Init(this, new PlatformOptions() { SmallIconDrawable = Android.Resource.Drawable.IcDialogInfo });

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
            //CrossCurrentActivity.Current.Activity = this;



            #region For screen Height & Width  
            var pixels = Resources.DisplayMetrics.WidthPixels;
            var scale = Resources.DisplayMetrics.Density;
            var dps = (double)((pixels - 0.5f) / scale);
            var ScreenWidth = (int)dps;

            pixels = Resources.DisplayMetrics.HeightPixels;
            dps = (double)((pixels - 0.5f) / scale);
            var ScreenHeight = (int)dps;

            //scale = 1.0f;

            //_app = new App((int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density), (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density));
            _app = new App(ScreenWidth, ScreenHeight, Resources.DisplayMetrics.WidthPixels, Resources.DisplayMetrics.HeightPixels, scale,  0);

            //_app = new App((int)Resources.DisplayMetrics.WidthPixels, (int)Resources.DisplayMetrics.HeightPixels);

            //App.ScreenHeight = ScreenHeight;
            //App.screenWidth = ScreenWidth;
            #endregion
            XamForms.Controls.Droid.Calendar.Init();
            FormsVideoPlayer.Init();

            LoadApplication(_app);

            Console.WriteLine("Screen Size: " + ScreenWidth + " x " + ScreenHeight);
            Console.WriteLine("Pixel Size: " + (int)(Resources.DisplayMetrics.WidthPixels) + " x " + (int)(Resources.DisplayMetrics.HeightPixels));
            Console.WriteLine("Scaled Size: " + (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density) + " x " + (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density));

            Console.WriteLine("Density: " + (int)(Resources.DisplayMetrics.Density));
            Console.WriteLine("Scaled Density: " + (int)(Resources.DisplayMetrics.ScaledDensity));
            Console.WriteLine("Xdpi: " + (int)(Resources.DisplayMetrics.Xdpi));
            Console.WriteLine("Ydpi: " + (int)(Resources.DisplayMetrics.Ydpi));

            Console.WriteLine("Device Type: " + Device.Idiom);

            /*
            if (Device.Idiom == TargetIdiom.Tablet)
            {
                App.SetScalables(2);
            }

            App.SetPixelSize((int)(Resources.DisplayMetrics.WidthPixels), (int)(Resources.DisplayMetrics.HeightPixels));
            App.SetScalables((int)(Resources.DisplayMetrics.Density));
            */





            Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            Window.SetStatusBarColor(Android.Graphics.Color.Black);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            if (_app.HandleHardwareBack() == false)
            {
                base.OnBackPressed();
            }
            else
            {
                return;
            }
        }
    }
}