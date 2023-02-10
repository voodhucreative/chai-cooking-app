using System;
using System.Collections.Generic;
using ChaiCooking.Models;
using ChaiCooking.Models.Custom;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace ChaiCooking.Services.Storage
{
    public static class LocalDataStore
    {
        public static void Init()
        {

        }

        public static void Save(string key, Object data)
        {
            try
            {
                Xamarin.Forms.Application.Current.Properties[key] = JsonConvert.SerializeObject(data);
            }
            catch
            {
                Xamarin.Forms.Application.Current.Properties.Remove(key);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.SavePropertiesAsync();
                });

                //This error forces us to refresh the key as it apears to fail to overwrite the exiting data
                System.Diagnostics.Debugger.Break();

                Xamarin.Forms.Application.Current.Properties[key] = JsonConvert.SerializeObject(data);
            }
        }

        public static void Save(string key, string data)
        {
            Console.WriteLine("Saved: " + key + " : " + data);
            Xamarin.Forms.Application.Current.Properties[key] = data;
        }

        public static string Load(string key)
        {
            string data = (string)Xamarin.Forms.Application.Current.Properties[key];
            Console.WriteLine("Loaded: " + key + " : " + data);
            return data;
        }

        public static void SaveAll()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await App.Current.SavePropertiesAsync();
            });
            Save("user", AppSession.CurrentUser);
        }

        public static void SaveCalendar()
        {
            Save("calendar",AppSession.CurrentUser.CalendarPlans);
        }

        public static List<InternalCalendarPlan> LoadCalendar()
        {
            try
            {
                return JsonConvert.DeserializeObject<List<InternalCalendarPlan>>(Load("calendar"));
            }
            catch (Exception e)
            {
                Console.WriteLine("No calendar saved");
            }
            return null;
        }

        public static void SaveUser()
        {
            Save("user", AppSession.CurrentUser);
        }

        public static User LoadUser()
        {
            try
            {
                return JsonConvert.DeserializeObject<User>(Load("user"));
            }
            catch (Exception e)
            {
                Console.WriteLine("No user saved");
            }
            return null;
        }

    }
}
