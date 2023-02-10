using System;
using System.Threading.Tasks;
using ChaiCooking.DebugData.Custom;
using ChaiCooking.Models;
using ChaiCooking.Models.Custom;

namespace ChaiCooking.Services
{
    public static class AccountManager
    {
        // Account Management
        public static async Task<bool> CreateUser(User userToCreate)
        {
            await Task.Delay(50);
            User createdUser = new User();

            if (AppSettings.UseFakeData)
            {
                //createdUser = (User)FakeData.TestUser.Clone();
                //createdUser.Albums = FakeData.UserAlbums;
                //createdUser = await App.ApiBridge.CreateUser(userToCreate);
            }
            else
            {
                //createdUser = await App.ApiBridge.CreateUser(userToCreate);
            }
            return true;
        }

        public static async Task<bool> DeleteUser(User userToDelete)
        {
            await Task.Delay(50);

            if (AppSettings.UseFakeData)
            {
                return true;
            }
            else
            {
                return await App.ApiBridge.DeleteUser(userToDelete);
            }
        }

        public static async Task<bool> ChangePassword(string currentPassword, string newPassword, string newPasswordConfirmation)
        {
            await Task.Delay(50);

            if (AppSettings.UseFakeData)
            {
                return true;
            }
            else
            {
                return await App.ApiBridge.ChangePassword(currentPassword, newPassword, newPasswordConfirmation);
            }
        }

        public static async Task<bool> ForgotPassword(string email)
        {
            return await App.ApiBridge.ForgotPassword(email);
        }

        public static async Task<bool> LogIn(string email, string password)
        {
            /*await Task.Delay(50);
            User loggedInUser = new User();
            if (AppSettings.UseFakeData)
            {
                //return FakeData.TestUser;
                return await App.ApiBridge.LogIn(email, password);
            }
            else
            {
                return await App.ApiBridge.LogIn(email, password);
            }*/
            return await App.ApiBridge.LogIn(AppSession.CurrentUser);
        }

        public static async Task<bool> LogOut(User user)
        {
            await Task.Delay(50);
            // user.IsLoggedIn = false;
            return true;
        }
    }
}
