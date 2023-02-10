using System;
using ChaiCooking.Components.Fields;
using ChaiCooking.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace ChaiCooking.iOS
{
    public class CustomEditorRenderer : EditorRenderer
    {
        CustomEditor editor;

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

        void GetCursorPosition()
        {
            if (Control != null)
            {
                editor.CursorPosition = (int)Control.GetOffsetFromPosition(Control.BeginningOfDocument, Control.SelectedTextRange.Start);
            }
        }

        void OnCursorChanged()
        {
            if (Control != null)
            {
                //Offset the cursor by a set ammount
                var pos = Control.GetPosition(Control.BeginningOfDocument, editor.CursorPosition);
                
                Control.SelectedTextRange = Control.GetTextRange(fromPosition: pos, toPosition: pos);
            }
        }
    }
}
