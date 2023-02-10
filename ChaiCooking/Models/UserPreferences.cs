using System;
using System.Collections.Generic;
using System.ComponentModel;
using ChaiCooking.AppData;
using ChaiCooking.Helpers.Custom;
using ChaiCooking.Models.Custom;
using Newtonsoft.Json;
using static ChaiCooking.Helpers.Custom.Accounts;

namespace ChaiCooking.Models
{
    public class UserPreferences: INotifyPropertyChanged
    {
        //public AccountType AccountType { get; set; }

        [JsonProperty("account_type")]
        public AccountType account_type;
        public AccountType AccountType
        {
            set
            {
                if (account_type != value)
                {
                    account_type = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AccountType"));
                }
            }
            get
            {
                return account_type;
            }
        }

        [JsonProperty("account_name")]
        public string account_name;
        public string AccountName
        {
            set
            {
                if (account_name != value)
                {
                    account_name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AccountName"));

                }
            }
            get
            {
                return account_name;
            }
        }

        public List<Preference> Allergens { get; set; }
        public List<Preference> DietTypes { get; set; }
        public List<Preference> Avoids { get; set; }
        public List<Preference> I { get; set; }
        public List<Preference> OtherPrefs { get; set; }

        public string WeeklyBudget { get; set; }
        public string HouseholdSize { get; set; }
        public string WeeklyShop { get; set; }
        public string ConvenienceStores { get; set; }
        public string OnlineShopping { get; set; }
        public string Plants { get; set; }
        public string Alcohol { get; set; }


        public string CurrentCharacterImage { get; set; }

        public UserPreferences()
        {
            //AccountType = AccountType.ChaiFree;

            AccountName = AppText.CURRENT_PLAN + " : " + Accounts.GetAccountName(AccountType);

            Allergens = new List<Preference>();

            DietTypes = new List<Preference>();

            Avoids = new List<Preference>();

            OtherPrefs = new List<Preference>();

            CurrentCharacterImage = "social_reward_chai.png";

            WeeklyBudget = "";
            HouseholdSize = "";
            WeeklyShop = "";
            ConvenienceStores = "";
            OnlineShopping = "";
            Plants = "";
            Alcohol = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Update()
        {
            AccountName = AppText.CURRENT_PLAN + " : " + Accounts.GetAccountName(AccountType);
        }

        public void AddDietType(string name)
        {
            DietTypes.Add(new Preference(name, true));
        }

        public void AddAllergen(string name)
        {
            Allergens.Add(new Preference(name, true));
        }

        public void AddAvoid(string name)
        {
            Avoids.Add(new Preference(name, true));
        }

        public void AddOtherPref(string name)
        {
            OtherPrefs.Add(new Preference(name, true));
        }
    }
}
