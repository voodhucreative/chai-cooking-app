using System.Drawing;
using ChaiCooking.Helpers;
using CustomRenderer.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CoreAnimation;
using CoreGraphics;
using ChaiCooking.Branding;

[assembly: ExportRenderer(typeof(Picker), typeof(MyCustomPickerRenderer))]
namespace CustomRenderer.iOS
{
    public class MyCustomPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Picker element = (Element as Picker);

                if (element != null)
                {
                    Control.Layer.BorderWidth = 0;
                    Control.BorderStyle = UITextBorderStyle.None;
                    Control.TextAlignment = UITextAlignment.Center;
                    Control.MinimumFontSize = 14;
                    Control.TextColor = Xamarin.Forms.Color.FromHex(Colors.CC_DARK_GREY).ToUIColor();
                }
            }
        }
    }
}
