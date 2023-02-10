using System.Drawing;
using ChaiCooking.Branding;
using ChaiCooking.Helpers;
using CustomRenderer.iOS;
using CustomSliderColor.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Slider), typeof(CustomSliderRenderer))]
namespace CustomSliderColor.iOS
{
    public class CustomSliderRenderer : SliderRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                //const string colorSlider = "#008000";

                Control.MaximumTrackTintColor = Xamarin.Forms.Color.LightGray.ToUIColor();
                Control.MinimumTrackTintColor = Xamarin.Forms.Color.FromHex(Colors.CC_ORANGE).ToUIColor();
                Control.ThumbTintColor = Xamarin.Forms.Color.FromHex(Colors.CC_ORANGE).ToUIColor();
                //Control.Th
            }
        }
    }
}