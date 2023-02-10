using System;
using System.Threading.Tasks;
using ChaiCooking.Helpers.Custom;
using Plugin.InAppBilling;
using Xamarin.Forms;
using static ChaiCooking.Helpers.Custom.Accounts;

namespace ChaiCooking.Services
{
    public static class IAPManager
    {

        public static async Task<bool> WasItemPurchased(int duration)
        {
            string productId = "";
            switch (duration)
            {
                case 1:
                    productId = "com.cooking.chai.app.subs.premium.monthly";
                    break;
                case 12:
                    productId = "com.cooking.chai.app.subs.premium.annual";
                    break;
                default:
                    break;
            }

            var billing = CrossInAppBilling.Current;
            try
            {
                var connected = await billing.ConnectAsync();

                if (!connected)
                {
                    //Couldn't connect
                    return false;
                }

                //check purchases
                var purchases = await billing.GetPurchasesAsync(ItemType.Subscription);

                //check for null just incase
                if (purchases == null)
                {
                    Console.WriteLine("Error: null purchases");
                    return false;
                }
                else if(purchases.GetEnumerator().Current.PurchaseToken != null)
                {
                    Console.WriteLine("Purchase Found");
                    await App.SetUserAccountType(AppSession.SelectedAccountType);
                    return true;
                }
            }
            catch (InAppBillingPurchaseException purchaseEx)
            {
                //Billing Exception handle this based on the type
                Console.WriteLine("Error: " + purchaseEx);
            }
            catch (Exception ex)
            {
                //Something has gone wrong
            }
            finally
            {
                await billing.DisconnectAsync();
            }

            return false;
        }

        public static async Task<bool> MakePurchase(int duration)
        {
            var billing = CrossInAppBilling.Current;

            bool success = true;

            var connected = await billing.ConnectAsync();
            if (!connected)
                return false;

            InAppBillingPurchase purchase = null;

            //TODO This periodically fails, preventing users accessing the paid content, very bad!
            try
            {
                switch (duration)
                {
                    case 1:
                        purchase = await CrossInAppBilling.Current.PurchaseAsync("com.cooking.chai.app.subs.premium.monthly", ItemType.Subscription, /*verify, */AppSession.CurrentUser.Id, AppSession.CurrentUser.Id).ConfigureAwait(false);
                        break;
                    case 12:
                        purchase = await CrossInAppBilling.Current.PurchaseAsync("com.cooking.chai.app.subs.premium.annual", ItemType.Subscription, /*verify, */AppSession.CurrentUser.Id, AppSession.CurrentUser.Id).ConfigureAwait(false);
                        Console.WriteLine(purchase.Id);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception subex)
            {
                Console.WriteLine(subex.ToString());
                await App.SetUserAccountType(AccountType.ChaiFree);
                //App.ShowAlert("ERROR - Subscription failed");
                success = false;
            }

            if (Device.RuntimePlatform == Device.iOS)
            {
                try
                {
                    if (purchase == null)
                    {
                        await App.SetUserAccountType(AccountType.ChaiFree);
                    }
                    else
                    {
                        DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                        DateTime localDateTime, univDateTime;
                        localDateTime = purchase.TransactionDateUtc;
                        univDateTime = localDateTime.ToUniversalTime();

                        string pDate = purchase.TransactionDateUtc.ToString("dd-MM-yyyy HH:mm:ss");
                        
                        string pDateMillis = ""+(long)(univDateTime - UnixEpoch).TotalMilliseconds;

                        if (await App.ApiBridge.SendAppleTransactionCode(AppSession.CurrentUser, purchase.Id, pDate, pDateMillis))
                        {
                            await App.SetUserAccountType(AppSession.SelectedAccountType);// CurrentUser.Preferences.AccountType);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Subscription Error");
                    success = false;
                }
                    
            }
            else
            {
                try
                {
                    // do purchase code stuff here
                    if (purchase == null)
                    {
                        App.ShowAlert("ERROR - Subscription failed");
                        await App.SetUserAccountType(AccountType.ChaiFree);
                    }
                    else
                    {
                        //purchased!
                        if (Device.RuntimePlatform == Device.Android)
                        {
                            // Must call AcknowledgePurchaseAsync else the purchase will be refunded
                            /*var acknowledged = await CrossInAppBilling.Current.AcknowledgePurchaseAsync(purchase.PurchaseToken);

                            if (acknowledged)
                            {
                                await App.SetUserAccountType(AppSession.SelectedAccountType);
                            }*/

                            // start - mat fix 21-11-22
                            var acknowledgedAgain = await CrossInAppBilling.Current.FinalizePurchaseAsync(purchase.PurchaseToken);

                            if (acknowledgedAgain != null)
                            {
                                await App.SetUserAccountType(AppSession.SelectedAccountType);
                            }
                            // end - mat fix 21-11-22
                        }
                        else
                        {
                            await App.SetUserAccountType(AppSession.SelectedAccountType);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Subscription Error");
                    success = false;
                }
            }
            await billing.DisconnectAsync();

            return success;
        }

        
    }
}
