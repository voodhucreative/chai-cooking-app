using System;
using System.Threading.Tasks;
using ChaiCooking;
using Plugin.InAppBilling;

namespace ChaiCooking.iOS
{
    public class IInAppBillingVerifyPurchace : IInAppBillingVerifyPurchase
    {
        public Task<bool> VerifyPurchase(string signedData, string signature, string productId = null, string transactionId = null)
        {
            Console.WriteLine("VERIFY THIS!!");
#if __ANDROID__
            var key1Transform = Plugin.InAppBilling.InAppBillingImplementation.InAppBillingSecurity.TransformString(key1, 1);
            var key2Transform = Plugin.InAppBilling.InAppBillingImplementation.InAppBillingSecurity.TransformString(key2, 2);
            var key3Transform = Plugin.InAppBilling.InAppBillingImplementation.InAppBillingSecurity.TransformString(key3, 3);
            
            return Task.FromResult(Plugin.InAppBilling.InAppBillingImplementation.InAppBillingSecurity.VerifyPurchase(key1Transform + key2Transform + key3Transform, signedData, signature));
#else
            return Task.FromResult(true);
#endif
        }


        public Task<bool> VerifyPurchase(string signedData, string signature)
        {

#if __ANDROID__
            var key1Transform = Plugin.InAppBilling.InAppBillingImplementation.InAppBillingSecurity.TransformString(key1, 1);
            var key2Transform = Plugin.InAppBilling.InAppBillingImplementation.InAppBillingSecurity.TransformString(key2, 2);
            var key3Transform = Plugin.InAppBilling.InAppBillingImplementation.InAppBillingSecurity.TransformString(key3, 3);
            
            return Task.FromResult(Plugin.InAppBilling.InAppBillingImplementation.InAppBillingSecurity.VerifyPurchase(key1Transform + key2Transform + key3Transform, signedData, signature));
#else
            return Task.FromResult(true);
#endif
        }
    }
}
