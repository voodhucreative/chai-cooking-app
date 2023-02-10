using System;
using Android.Content;
using ChaiCooking.Components.Fields;
using ChaiCooking.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace ChaiCooking.Droid
{
    public class CustomEditorRenderer : EditorRenderer
    {
        CustomEditor editor;
        public CustomEditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                editor = e.NewElement as CustomEditor;
                ((CustomEditor)Element).UpdateCursor += (sender, evnt) => { OnCursorChanged(); };
                ((CustomEditor)Element).GetCursor += (sender, evnt) => { GetCursorPosition(); };
            }
        }

        private void GetCursorPosition()
        {
            if (Control != null)
            {
                editor.CursorPosition = Control.SelectionStart;
            }
        }

        void OnCursorChanged()
        {
            if (Control != null)
            {
                Control.SetSelection(editor.CursorPosition);
            }
        }
    }
}
