using UIKit;
using Xamarin.Forms;
using ChaiCooking.iOS.Services;
using ChaiCooking.Services;


[assembly: Dependency(typeof(StatusBarStyleManager))]
namespace ChaiCooking.iOS.Services
{
    public class StatusBarStyleManager : IStatusBarStyleManager
    {

        public void SetDarkTheme()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, true);
                
                GetCurrentViewController().SetNeedsStatusBarAppearanceUpdate();
            });
        }

        public void SetLightTheme()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.Default, true);
                GetCurrentViewController().SetNeedsStatusBarAppearanceUpdate();
            });
        }

        UIViewController GetCurrentViewController()
        {
            //TODO Investigate Crash on line 36
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
            vc = vc.PresentedViewController;
            return vc;
            
        }
    }
}