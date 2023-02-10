using System;
using System.Text;
using System.Text.RegularExpressions;
using ChaiCooking.Branding;
using Xamarin.Forms;

namespace ChaiCooking.Helpers
{
    public static class CleanUp
    {
        public static bool CapitalizeInput(Object sender)
        {
            Entry entry = (Entry)sender;
            string txt = entry.Text;
            entry.Text = txt.ToUpper();
            return true;
        }

        public static string CapitalizeFirst(string s)
        {
            bool IsNewSentense = true;
            var result = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                if (IsNewSentense && char.IsLetter(s[i]))
                {
                    result.Append(char.ToUpper(s[i]));
                    IsNewSentense = false;
                }
                else
                    result.Append(s[i]);

                if (s[i] == '!' || s[i] == '?' || s[i] == '.')
                {
                    IsNewSentense = true;
                }
            }
            return result.ToString();
        }

        public static string CleanUpJson(string dirty)
        {
            string clean = dirty;
            clean = clean.Replace("{{", "{"); // Replace the left curly braces
            clean = clean.Replace("}}", "}");
            clean = clean.Replace("\n", "");
            clean = clean.Replace(@"\", "");
            return clean;
        }
    }
}
