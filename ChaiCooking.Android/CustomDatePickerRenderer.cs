using System;
using Android.Content;
using Android.Views;
using ChaiCooking.Components;
using ChaiCooking.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomDatePicker), typeof(CustomDatePickerRenderer))]
namespace ChaiCooking.Droid
{
    public class CustomDatePickerRenderer : DatePickerRenderer
    {
        public CustomDatePickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                //Hardcoded for now. TODO allow us to set this from Shared Code.
                Control.Gravity = GravityFlags.CenterHorizontal;
            }
        }
    }
}
