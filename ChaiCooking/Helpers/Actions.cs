using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ChaiCooking.Helpers
{
    public static class Actions
    {
        public enum ActionName
        {
            None,
            GoToPage,
            GoToNextPage,
            GoToLastPage,
            QuitApp,
            ShowLoading,
            HideLoading,
            ShowMenu,
            HideMenu,
            ToggleMenu,
            ShowHeader,
            HideHeader,
            ToggleHeader,
            ShowFooter,
            HideFooter,
            ToggleFooter,
            ShowSubHeader,
            HideSubHeader,
            ToggleSubHeader,
            ShowNavHeader,
            HideNavHeader,
            ToggleNavHeader,
            ShowModal,
            HideModal,
            ToggleModal,
            ShowForeground,
            HideForeground,
            ToggleForeground,
            ShowPanel,
            HidePanel,
            TogglePanel,
            LogIn,
            Logout,
            ShowAccountConfirmation,
            ShowLoginAndRegister,
            ShowCreateMealPlan,
            HideCreateMealPlan,
            ShowForgotPassword,
            HideForgotPassword,
            ShowRecipeSummary,
            HideRecipeSummary,
            ShowAccountConfirmationModal,
            HideAccountConfirmationModal,
            ShowVerifyAccount,
            HideVerifyAccount,
            ShowComingSoon,
            ShowErrorPopup,
            ShowRecipeOptionsPanel,
            ShowRecommendedRecipeFilterPanel,
            ClearPanels,
            ShowCalendar
        }

        public enum PriorityName
        {
            Background,
            Low,
            Medium,
            High,
            Critical,
        };

        private static List<Models.Action> AppActions = new List<Models.Action>();

        public static void Init()
        {
            // load all standard actions
            AppActions.Add(new Models.Action((int)ActionName.None, -1));
            AppActions.Add(new Models.Action((int)ActionName.GoToPage, -1));
            AppActions.Add(new Models.Action((int)ActionName.GoToNextPage));
            AppActions.Add(new Models.Action((int)ActionName.GoToLastPage));
            AppActions.Add(new Models.Action((int)ActionName.QuitApp));
            AppActions.Add(new Models.Action((int)ActionName.ShowLoading));
            AppActions.Add(new Models.Action((int)ActionName.HideLoading));
            AppActions.Add(new Models.Action((int)ActionName.ShowMenu));
            AppActions.Add(new Models.Action((int)ActionName.HideMenu));
            AppActions.Add(new Models.Action((int)ActionName.ToggleMenu));
            AppActions.Add(new Models.Action((int)ActionName.ShowHeader));
            AppActions.Add(new Models.Action((int)ActionName.HideHeader));
            AppActions.Add(new Models.Action((int)ActionName.ToggleHeader));
            AppActions.Add(new Models.Action((int)ActionName.ShowFooter));
            AppActions.Add(new Models.Action((int)ActionName.HideFooter));
            AppActions.Add(new Models.Action((int)ActionName.ToggleFooter));
            AppActions.Add(new Models.Action((int)ActionName.ShowSubHeader));
            AppActions.Add(new Models.Action((int)ActionName.HideSubHeader));
            AppActions.Add(new Models.Action((int)ActionName.ToggleSubHeader));
            AppActions.Add(new Models.Action((int)ActionName.ShowNavHeader));
            AppActions.Add(new Models.Action((int)ActionName.HideNavHeader));
            AppActions.Add(new Models.Action((int)ActionName.ToggleNavHeader));
            AppActions.Add(new Models.Action((int)ActionName.ShowModal));
            AppActions.Add(new Models.Action((int)ActionName.HideModal));
            AppActions.Add(new Models.Action((int)ActionName.ToggleModal));
            AppActions.Add(new Models.Action((int)ActionName.ShowForeground));
            AppActions.Add(new Models.Action((int)ActionName.HideForeground));
            AppActions.Add(new Models.Action((int)ActionName.ToggleForeground));
            AppActions.Add(new Models.Action((int)ActionName.ShowPanel));
            AppActions.Add(new Models.Action((int)ActionName.HidePanel));
            AppActions.Add(new Models.Action((int)ActionName.TogglePanel));
            AppActions.Add(new Models.Action((int)ActionName.ShowCalendar));
            AppActions.Add(new Models.Action((int)ActionName.LogIn));
            AppActions.Add(new Models.Action((int)ActionName.Logout));

        }

        public static void AddAppAction(Models.Action action)
        {
            AppActions.Add(action);
        }

        // 
        public static Models.Action GetAction(int actionId)
        {
            Models.Action action = AppActions[actionId];
            return action;
        }
    }
}
