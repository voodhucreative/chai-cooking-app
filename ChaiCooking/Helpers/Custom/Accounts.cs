using System;
using System.Collections.Generic;
using ChaiCooking.Models;

namespace ChaiCooking.Helpers.Custom
{
    public static class Accounts
    {
        public const int UPGRADE_PREMIUM = 0;
        public const int UPGRADE_EDITOR_OR_PUBLISHER = 1;

        public const int PLAN_TYPE_TRANSITION = 0;
        public const int PLAN_TYPE_FLEXITARIAN = 1;
        public const int PLAN_TYPE_NONE = 2;

        public const int TARGET_LOSE_WEIGHT = 0;
        public const int TARGET_BUILD_MUSCLE = 1;
        public const int TARGET_NONE = 2;

        public enum AccountType
        {
            ChaiFree,
            ChaiPremiumVegan, // after transition
            ChaiPremiumFlex,
            ChaiPremiumTrans,

            // these are old and not currently in use
            /*ChaiPremium,
            ChaiTransNoTargets,
            ChaiTransLoseWeight,
            ChaiTransBuildMuscle,
            ChaiFlexNoTargets,
            ChaiFlexLoseWeight,
            ChaiFlexBuildMuscle,
            ChaiEditorPublisher*/
            
        };

        static Dictionary<AccountType, string> AccountTypes;

        public static void Init()
        {

            AccountTypes = new Dictionary<AccountType, string>
            {
                [AccountType.ChaiFree] = "CHAI Free",
                [AccountType.ChaiPremiumVegan] = "CHAI Premium Vegan",
                [AccountType.ChaiPremiumFlex] = "CHAI Premium Flex",
                [AccountType.ChaiPremiumTrans] = "CHAI Premium Trans",

                // these are old and not currently in use
                /*[.ChaiPremium] = "CHAI premium",
                [.ChaiTransNoTargets] = "CHAI trans",
                [.ChaiTransLoseWeight] = "CHAI LW trans",
                [.ChaiTransBuildMuscle] = "CHAI BM trans",
                [.ChaiFlexNoTargets] = "CHAI flex",
                [.ChaiFlexLoseWeight] = "CHAI LW flex",
                [.ChaiFlexBuildMuscle] = "CHAI BM flex",
                [.ChaiEditorPublisher] = "CHAI editor/publisher"*/

            };
        }

        public static string GetAccountName(AccountType accountType)
        {
            if (AppSession.CurrentUser != null)
            {
                if (AppSession.CurrentUser.IsInfluencer())
                {
                    return "CHAI Influencer";
                }
            }
            return AccountTypes[accountType];
        }

        public static string GenerateChaiIdPassword(User user)
        {
            string userChaiPassId = AppSettings.DEFAULT_CHAI_IDPASSWORD;
            // handle new users
            if (user.Id != null)
            {
                Console.WriteLine("User ID " + user.Id);
            }

            userChaiPassId = AppSettings.DEFAULT_CHAI_IDPASSWORD;

            try
            {
                userChaiPassId = user.EmailAddress.Replace("@", "").Replace(".", "");
                userChaiPassId = Tools.TextTools.StringToHex(userChaiPassId);

                while (userChaiPassId.Length < 10)
                {
                    userChaiPassId += "0";
                }

                if (userChaiPassId.Length > 10)
                {
                    userChaiPassId = userChaiPassId.Substring(0, 10);
                }
               
            }
            catch (Exception e) { }

            return userChaiPassId;
        }
    }
}


