using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Xamarin.Essentials;

namespace ChaiCooking.Droid
{
    [Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "chai")]

    public class WebAuthenticationCallbackActivity : WebAuthenticatorCallbackActivity
    {
    }
}
