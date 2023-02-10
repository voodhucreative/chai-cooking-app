using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.MealPlanAPI;
using ChaiCooking.Services;
using ChaiCooking.Services.Storage;
using Newtonsoft.Json;
using static ChaiCooking.Helpers.Custom.Accounts;

namespace ChaiCooking.Models
{
    public class User : ICloneable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty("id")]
        public string id;
        public string Id
        {
            set
            {
                if (id != value)
                {
                    id = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Id"));
                }
            }
            get
            {
                return id;
            }
        }

        [JsonProperty("first_name")]
        public string first_name;
        public string FirstName
        {
            set
            {
                if (first_name != value)
                {
                    first_name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FirstName"));
                }
            }
            get
            {
                return first_name;
            }
        }

        [JsonProperty("middle_name")]
        public string middle_name;
        public string MiddleName
        {
            set
            {
                if (middle_name != value)
                {
                    middle_name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MiddleName"));
                }
            }
            get
            {
                return middle_name;
            }
        }

        [JsonProperty("last_name")]
        public string last_name;
        public string LastName
        {
            set
            {
                if (last_name != value)
                {
                    last_name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LastName"));
                }
            }
            get
            {
                return last_name;
            }
        }

        [JsonProperty("gender")]
        public string gender;
        public string Gender
        {
            set
            {
                if (gender != value)
                {
                    gender = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Gender"));
                }
            }
            get
            {
                return gender;
            }
        }

        [JsonProperty("avatar_image_url")]
        public string avatar_image_url;
        public string AvatarImageUrl
        {
            set
            {
                if (avatar_image_url != value)
                {
                    avatar_image_url = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AvatarImageUrl"));
                }
            }
            get
            {
                return avatar_image_url;
            }
        }

        [JsonProperty("dob")]
        public DateTime DateOfBirth { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("email_address")]
        public string email_address;
        public string EmailAddress { get; set; }

        [JsonProperty("mobile_number")]
        public string mobile_number;
        public string MobileNumber { get; set; }

        [JsonProperty("password")]
        public string password;
        public string Password { get; set; }

        [JsonProperty("full_name")]
        public string full_name;
        public string FullName { get; set; }

        [JsonProperty("username")]
        public string username;
        public string Username
        {
            set
            {
                if (username != value)
                {
                    username = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Username"));
                }
            }
            get
            {
                return username;
            }
        }

        [JsonProperty("is_registered")]
        public bool IsRegistered { get; set; }

        [JsonProperty("auth_token")]
        public string AuthToken { get; set; }

        [JsonProperty("whisk_token_id")]
        public int WhiskTokenId { get; set; }

        [JsonProperty("bio")]
        public string Bio { get; set; }

        public bool RememberMe { get; set; }

        public UserPreferences Preferences { get; set; }

        public UserMealPlans MealPlans;

        public List<Album> Albums;

        public bool IsAuthorised { get; set; }

        public List<Recipe> recipeHolder { get; set; }

        public int defaultMealPlanID { get; set; }
        public string defaultMealPlanName { get; set; }
        public int defaultMealPlanWeeks { get; set; }

        
        public string Role { get; set; }

        [JsonProperty("calendar_plans")]
        public List<InternalCalendarPlan> CalendarPlans = new List<InternalCalendarPlan>();

        public User()
        {
            Preferences = new UserPreferences();
            MealPlans = new UserMealPlans();
            Albums = new List<Album>();
            Bio = "";
            Role = "";

            CalendarPlans = LocalDataStore.LoadCalendar();

            Console.WriteLine("User created");
        }

        public bool IsInfluencer()
        {
            if (Role == "influencer")
            {
                return true;
            }
            return false;
        }


        public void SetAccountType(AccountType accountType)
        {
            Preferences.AccountType = accountType;
            switch (Preferences.AccountType)
            {
                case Helpers.Custom.Accounts.AccountType.ChaiPremiumFlex:
                    Preferences.DietTypes.Clear();
                    if (AppSettings.IncludeMeatEater)
                    {
                        Preferences.DietTypes.Add(new Preference("No Preference", /*"Meat Eater",*/ "DIET_NONE", true));
                    }
                    else
                    {
                        Preferences.DietTypes.Add(new Preference("Pescatarian", "DIET_PESCATARIAN", true));
                    }
                    break;
                case Helpers.Custom.Accounts.AccountType.ChaiPremiumTrans:
                    Preferences.DietTypes.Clear();
                    if (AppSettings.IncludeMeatEater)
                    {
                        Preferences.DietTypes.Add(new Preference("No Preference", /*"Meat Eater",*/ "DIET_NONE", true));
                    }
                    else
                    {
                        Preferences.DietTypes.Add(new Preference("Pescatarian", "DIET_PESCATARIAN", true));
                    }
                    break;
                case Helpers.Custom.Accounts.AccountType.ChaiPremiumVegan:
                    Preferences.DietTypes.Clear();
                    Preferences.DietTypes.Add(new Preference("Vegan", "DIET_VEGAN", true));
                    break;
                case Helpers.Custom.Accounts.AccountType.ChaiFree:
                    Preferences.DietTypes.Clear();
                    break;
                default:
                    Preferences.DietTypes.Clear();
                    break;
            }
            

        }

        public string GetPlanName()
        {
            string plan = "free";
            switch (Preferences.AccountType)
            {
                case Accounts.AccountType.ChaiPremiumFlex:
                    plan = "flexitarian";
                    break;
                case Accounts.AccountType.ChaiPremiumTrans:
                    plan = "transitioning";
                    break;
                case Accounts.AccountType.ChaiPremiumVegan:
                    plan = "vegan";
                    break;
            }
            return plan;
        }

        public string GetLongPlanName()
        {
            string plan = "FREE";
            switch (Preferences.AccountType)
            {
                case Accounts.AccountType.ChaiPremiumFlex:
                    plan = "PREMIUM FLEXITARIAN";
                    break;
                case Accounts.AccountType.ChaiPremiumTrans:
                    plan = "PREMIUM TRANSITIONING";
                    break;
                case Accounts.AccountType.ChaiPremiumVegan:
                    plan = "PREMIUM VEGAN";
                    break;
            }
            return plan;
        }

        public void ReorderInternalCalendarPlans()
        {
            //Orders the internal calendar list by start date to make itteration checks more efficient.
            List<InternalCalendarPlan> temp = CalendarPlans.OrderBy(x => x.StartDate).ToList();
            CalendarPlans = temp;
        }

        public void Update()
        {
            Preferences.Update();
        }

        public object Clone()
        {
            return (User)this.MemberwiseClone();
        }
    }
}
