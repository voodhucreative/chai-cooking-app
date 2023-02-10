using System;
namespace ChaiCooking.Models.Custom
{
    public class InternalCalendarPlan
    {
        public int CalendarId = -1;
        public DateTime StartDate = DateTime.UtcNow;
        public DateTime EndDate = DateTime.UtcNow;

        public bool InRange(DateTime date)
        {
            //Allows us to check if the date we're modifying is part of this plan
            if (date > StartDate && date < EndDate)
            {
                return true;
            }
            return false;
        }
    }
}
