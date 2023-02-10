using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ChaiCooking.Views.Custom
{
    public class DatePicker : Xamarin.Forms.DatePicker
    {
        public static BindableProperty DateChosenCommandProperty = 
            BindableProperty
            .Create(
            "SelectedDateChangeProprety", 
            typeof(ICommand), typeof(DatePicker));

        public DatePicker()
        {
            this.DateSelected += DatePicker_DateSelected;
        }

        public ICommand DateChosenCommand
        {
            get
            {
                return (ICommand)this.GetValue(DateChosenCommandProperty);
            }
            set
            {
                this.SetValue(DateChosenCommandProperty, value);
            }
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (this.DateChosenCommand != null)
            {
                this.DateChosenCommand.Execute(e);
            }
        }
    }
}
