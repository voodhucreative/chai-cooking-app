using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChaiCooking.Models;
using Xamarin.Forms;
using static ChaiCooking.Helpers.Custom.Accounts;

namespace ChaiCooking.Services
{
    public static class SubscriptionManager
    {
        public static async Task<bool> CreateChaiPlan(User user)
        {
            return await App.ApiBridge.CreateChaiSubscriptionPlan(user);
        }

        public static string GetAccountInfoText(User user)
        {
            string infoText = "";
            switch(user.Preferences.AccountType)
            {
                case AccountType.ChaiPremiumFlex:
                    infoText = "At least half your diet will consist of vegan meals";
                    break;
                case AccountType.ChaiPremiumTrans:
                    infoText = "We will gradually reduce your meat and dairy intake over the next 12 months.";
                    break;
                case AccountType.ChaiPremiumVegan:
                    infoText = "Enjoy thousands of delicious, premium vegan recipes all year round!";
                    break;
                case AccountType.ChaiFree:
                default:
                    break;

            }
            return infoText;
        }


        public static async Task<bool> CanAccessPaidContent(User user)
        {

            //if(accountType != AccountType.ChaiFree)
            //{
            //    return true;
            //}

            /*if (AppSettings.FreeSubsForInfluencers)
            {
                if (user.IsInfluencer())
                {
                    return true;
                }
            }
            
            if (accountType == AccountType.ChaiFree)
            {
                return false;
            }
            */

            if (AppSettings.FreeSubsForAll)
            {
                return true;
            }


            if (AppSettings.FreeSubsForInfluencers)
            {
                if (user.IsInfluencer())
                {
                    return true;
                }
            }

            List<string> subs = await App.ApiBridge.GetSubscriptions(user);//.ConfigureAwait(false);
            if (subs.Count > 0)
            {
                return true;
            }
            else
            {
                // temporary workaround
                if (Device.RuntimePlatform == Device.iOS)
                {
                    if (AppSession.CurrentUser.Preferences.AccountType != AccountType.ChaiFree)
                    {
                        //return true;
                    }
                    
                    //AccountType accountType = await App.ApiBridge.GetCurrentSubscriptionPlan(user).ConfigureAwait(false);
                    //if (accountType != AccountType.ChaiFree)
                    //{
                    //    return true;
                    //}
                }
            }

            
            return false;
        }
    }
}
