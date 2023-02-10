using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using KeyboardOverlap.Forms.Plugin.iOSUnified;
using Octane.Xamarin.Forms.VideoPlayer.iOS;
using MediaManager;
using UIKit;
using UserNotifications;
using Xamarin.Forms;
using XFShapeView.iOS;
using StoreKit;

namespace ChaiCooking.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //

        //public App _app;
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            //UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.DarkContent, false);
            Forms.SetFlags("Markup_Experimental");
            Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();

            //KeyboardOverlapRenderer.Init();
            //ButtonCircleRenderer.Init();
            FormsVideoPlayer.Init();
            ShapeRenderer.Init();

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

            //LoadApplication(new App((int)UIScreen.MainScreen.Bounds.Width, (int)UIScreen.MainScreen.Bounds.Height));
            //_app = new App((int)UIScreen.MainScreen.Bounds.Width, (int)UIScreen.MainScreen.Bounds.Height);
            CrossMediaManager.Current.Init();

            XamForms.Controls.iOS.Calendar.Init();

            //App.SetScalables((int)UIScreen.MainScreen.Scale);


            LoadApplication(new App((int)UIScreen.MainScreen.Bounds.Width, (int)UIScreen.MainScreen.Bounds.Height, (int)UIScreen.MainScreen.NativeBounds.Width, (int)UIScreen.MainScreen.NativeBounds.Height, (float)UIScreen.MainScreen.Scale, (int)UIApplication.SharedApplication.StatusBarFrame.Height));
            //LoadApplication(new App((int)UIScreen.MainScreen.NativeBounds.Width, (int)UIScreen.MainScreen.NativeBounds.Height, (int)UIApplication.SharedApplication.StatusBarFrame.Height));

            //App.SetPixelSize((int)UIScreen.MainScreen.NativeBounds.Width, (int)UIScreen.MainScreen.NativeBounds.Height);
            //App.SetScalables((int)UIScreen.MainScreen.Scale);


            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // Request Permissions
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound, (granted, error) =>
                {
                    // Do something if needed
                });
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
                 UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null
                    );

                app.RegisterUserNotificationSettings(notificationSettings);
            }
            //app.StatusBarStyle = UIStatusBarStyle.DarkContent;
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes
            {
                TextColor = UIColor.White
            });

            Plugin.InAppBilling.InAppBillingImplementation.OnShouldAddStorePayment = OnShouldAddStorePayment;
            var current = Plugin.InAppBilling.CrossInAppBilling.Current; //initializes

            return base.FinishedLaunching(app, options);
        }

        private bool OnShouldAddStorePayment(SKPaymentQueue queue, SKPayment payment, SKProduct product)
        {
            //Process and check purchases
            return true;
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            if (Xamarin.Essentials.Platform.OpenUrl(app, url, options))
                return true;

            return base.OpenUrl(app, url, options);
        }

        public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler)
        {
            //if (Xamarin.Essentials.Platform.ContinueUserActivity(application, userActivity, completionHandler))
            //    return true;

            return base.ContinueUserActivity(application, userActivity, completionHandler);
        }
    }
}
