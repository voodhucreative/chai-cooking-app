using System;
using System.Threading;
using Xamarin.Forms;

namespace ChaiCooking.Services
{
    public class StoppableTimer
    {
        private readonly TimeSpan Timespan;
        private readonly Action Callback;
        private readonly bool Repeat;

        private bool IsRunning;
        private bool TimerCallbackPending;

        public StoppableTimer(TimeSpan timespan, Action callback, bool repeat)
        {
            Timespan = timespan;
            Callback = callback;
            Repeat = repeat;
        }

        public void Start()
        {
            IsRunning = true;
            ScheduleTimerIfNeeded();
        }

        public void Stop()
        {
            IsRunning = false;
        }

        private void ScheduleTimerIfNeeded()
        {
            if (!TimerCallbackPending)
            {
                {
                    TimerCallbackPending = true;
                    Device.StartTimer(Timespan, TimerCallback);
                }
            }
        }

        private bool TimerCallback()
        {
            if (IsRunning)
            {
                Callback.Invoke();
            }

            bool reschedule = Repeat && IsRunning;

            TimerCallbackPending = reschedule;

            return reschedule;
        }
    }
}
