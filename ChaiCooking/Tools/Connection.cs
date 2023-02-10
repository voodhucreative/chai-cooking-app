using System;
using Xamarin.Essentials;

namespace ChaiCooking.Tools
{
    public static class Connection
    {
        public static bool IsConnected()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                // Connection to internet is available
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
