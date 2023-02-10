using System;
using Xamarin.Forms;

namespace ChaiCooking.Components.Fields
{
    public class CustomEditor : Editor
    {
        public event EventHandler UpdateCursor;
        public event EventHandler GetCursor;

        public int CursorPosition;

        public void SetCursorToPosition(int cursorPosition)
        {
            CursorPosition = cursorPosition;
            UpdateCursor(this, EventArgs.Empty);
        }

        public void SetCursorToEnd()
        {
            CursorPosition = Text.Length;
            UpdateCursor(this, EventArgs.Empty);
        }

        public int GetCursorPosition()
        {
            GetCursor(this, EventArgs.Empty);
            return CursorPosition;
        }
    }
}
