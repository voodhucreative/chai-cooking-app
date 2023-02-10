using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using ChaiCooking.Branding;
using ChaiCooking.Components.Fields;
using CustomRenderer.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Slider), typeof(CustomSliderRenderer))]
namespace CustomRenderer.Android
{
    class CustomSliderRenderer : SliderRenderer
    {
        public CustomSliderRenderer(Context context) : base(context)
        {
        }

       

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Slider> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                ShapeDrawable th = new ShapeDrawable(new OvalShape());
                th.SetIntrinsicWidth(80);
                th.SetIntrinsicHeight(80);
                th.SetColorFilter(Xamarin.Forms.Color.FromHex("#f7941e").ToAndroid(), PorterDuff.Mode.SrcOver);
                Control.SetThumb(th);

                Control.SetBackgroundColor(global::Android.Graphics.Color.Transparent);

                //Control.Thumb.SetColorFilter(Xamarin.Forms.Color.FromHex("#f7941e").ToAndroid(), PorterDuff.Mode.SrcIn);

                
                
                Control.ProgressDrawable.SetColorFilter(new PorterDuffColorFilter(Xamarin.Forms.Color.FromHex("#f7941e").ToAndroid(),PorterDuff.Mode.SrcIn));

                Control.ProgressBackgroundTintList = ColorStateList.ValueOf(Xamarin.Forms.Color.FromHex("#8000ff").ToAndroid());
                Control.ProgressBackgroundTintMode = PorterDuff.Mode.SrcIn;
                

                /*.
                Control.MaximumTrackTintColor = Xamarin.Forms.Color.LightGray.ToUIColor();
                Control.MinimumTrackTintColor = Xamarin.Forms.Color.FromHex(Colors.CC_ORANGE).ToUIColor();
                Control.ThumbTintColor = Xamarin.Forms.Color.FromHex(Colors.CC_ORANGE).ToUIColor();
                */



                //Control.SetTextColor(global::Android.Graphics.Color.Black);

                /*GradientDrawable gradientDrawable = new GradientDrawable();
                gradientDrawable.SetShape(ShapeType.Rectangle);
                gradientDrawable.SetColor(global::Android.Graphics.Color.White);
                gradientDrawable.SetStroke(8, global::Android.Graphics.Color.Blue);
                gradientDrawable.SetTint(global::Android.Graphics.Color.White);
                //gradientDrawable.SetStroke(4, )
                gradientDrawable.SetCornerRadius(80.0f);

                Control.SetBackground(gradientDrawable);*/

            }
        }
    }
}
