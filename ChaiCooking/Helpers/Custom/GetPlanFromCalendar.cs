using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChaiCooking.Models.Custom;
using ChaiCooking.Models.Custom.MealPlanAPI;
using ChaiCooking.Services.Storage;

namespace ChaiCooking.Helpers.Custom
{
    public class GetPlanFromCalendar
    {
        public static string GetCalendarPlan(string date)
        {
            //Genrated data being interacted with in a funky way.
            InternalCalendarPlan posibleCalendar = new InternalCalendarPlan();
            UserMealPlans possiblePlan = new UserMealPlans { Data = new List<Models.Custom.MealPlanAPI.Datum>() };

            string mealPlanId = "-1";

            var planFromDate = App.ApiBridge.GetMealPlanFromDate(AppSession.CurrentUser, date).Result;

            if (planFromDate != null)
            {
                return planFromDate;
            }

            try
            {
                var temp = AppSession.CurrentUser.CalendarPlans.Find(x => (x.StartDate <= DateTime.Parse(date)) && (x.EndDate >= DateTime.Parse(date)));
                if (temp != null)
                {
                    posibleCalendar = temp;
                }}
            catch { /*No possible calendar*/}

            try { var temp = new UserMealPlans { Data = new List<Models.Custom.MealPlanAPI.Datum>() };
                temp.Data.Add(AppSession.CurrentUser.MealPlans.Data.Find(x => (x.start_date <= DateTime.Parse(date)) && (x.end_date >= DateTime.Parse(date))));
                if (temp.Data.Count > 0)
                {
                    possiblePlan.Data.AddRange(temp.Data);
                } } catch { /*No possible Meal Plan*/}
            if (possiblePlan.Data.Count == 0 || possiblePlan.Data[0] == null)
            {
                if (posibleCalendar.CalendarId == -1)
                {
                    //Create a new meal plan and add it to the current users internalPlans
                    var newId = -1;
                    try
                    {
                        newId = int.Parse(App.ApiBridge.CreateMealPlan(AppSession.CurrentUser, $"%Chai_Internal%_{date}", date, 1).Result);
                    }
                    catch
                    {
                        Console.WriteLine("Error creating plan");
                        return "-1";
                    }
                    //generate the list if it's null (logged in users will have an issue else)
                    if (AppSession.CurrentUser.CalendarPlans == null)
                    {
                        AppSession.CurrentUser.CalendarPlans = new List<InternalCalendarPlan>();
                    }
                    AppSession.CurrentUser.CalendarPlans.Add(new InternalCalendarPlan
                    {
                        CalendarId = newId,
                        StartDate = DateTime.Parse(date),
                        EndDate = DateTime.Parse(date)
                    });
                    mealPlanId = newId.ToString();
                    LocalDataStore.SaveCalendar();
                }
                else if (int.Parse(mealPlanId) != posibleCalendar.CalendarId)
                {
                    //Update the plan for the coresponding date
                    mealPlanId = posibleCalendar.CalendarId.ToString();
                }
            }
            else
            {
                mealPlanId = possiblePlan.Data[0].id.ToString();
            }
            return mealPlanId;
        }
    }
}
