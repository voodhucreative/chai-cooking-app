using Android.Content;
using Android.Graphics.Drawables;
using ChaiCooking.Components.Fields;
using CustomRenderer.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace CustomRenderer.Android
{
    class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
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
