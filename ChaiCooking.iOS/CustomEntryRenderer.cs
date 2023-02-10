using System.Drawing;
using ChaiCooking.Helpers;
using CustomRenderer.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(MyEntryRenderer))]
namespace CustomRenderer.iOS
{
    public class MyEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                //Control.Layer.CornerRadius = Units.TapSizeM/2;
                Control.TextColor = UIColor.Black;
                Control.BackgroundColor = UIColor.White;
                Control.BorderStyle = UITextBorderStyle.None;
            }
        }
    }
}
