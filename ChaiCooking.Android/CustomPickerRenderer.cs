using Android.Content;
using Android.Graphics.Drawables;
using ChaiCooking.Components.Fields;
using CustomRenderer.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ChaiCooking.Components;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace CustomRenderer.Android
{
    class CustomPickerRenderer : PickerRenderer
    {
        public CustomPickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundColor(global::Android.Graphics.Color.Transparent);
                Control.SetTextColor(global::Android.Graphics.Color.Black);


                GradientDrawable gradientDrawable = new GradientDrawable();
                gradientDrawable.SetShape(ShapeType.Rectangle);
                gradientDrawable.SetColor(global::Android.Graphics.Color.White);
                //gradientDrawable.SetStroke(8, global::Android.Graphics.Color.Blue);
                gradientDrawable.SetTint(global::Android.Graphics.Color.White);
                //gradientDrawable.SetStroke(4, )
                gradientDrawable.SetCornerRadius(80.0f);

                Control.SetBackground(gradientDrawable);

            }
        }
    }
}