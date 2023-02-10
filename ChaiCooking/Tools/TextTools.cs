using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ChaiCooking.Components.Labels;
using ChaiCooking.Helpers;
using Xamarin.Forms;

namespace ChaiCooking.Tools
{
    public static class TextTools
    {
        public static string GetFirstNameFromInput(string fullNameText)
        {
            string firstName = "";


            if (fullNameText.Count(Char.IsWhiteSpace) > 0)
            {
                string[] subNames = fullNameText.Split(' ');
                firstName = subNames[0];
            }
            return firstName;
        }

        public static List<string> TextToArray(string fullText, char splitby)
        {
            List<string> infoSections = fullText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList<string>();
            return infoSections;
        }

        public static string SanitiseForJson(string insane)
        {
            string sane = insane;

            sane = insane.Replace("\n", "JSONNEWLINE");

            return sane;
        }

        public static int MakeStringNumeric(string text)
        {
            string clean = text;

            clean = Regex.Replace(text, "[^0-9]", "");

            return Int32.Parse(clean);
        }

        public static string CleanUpJson(string dirty, bool keepNewLines)
        {
            string clean = dirty;

            clean = clean.Replace("{{", "{"); // Replace the left curly braces
            clean = clean.Replace("}}", "}");

            if (keepNewLines)
            {
                clean = clean.Replace("JSONNEWLINE", "\\n");
            }
            else
            {
                clean = clean.Replace("\n", "");
            }
            //
            clean = clean.Replace(@"\", "");
            return clean;

        }

        public static string CleanUpJson(string dirty)
        {
            string clean = dirty;

            clean = clean.Replace("{{", "{"); // Replace the left curly braces
            clean = clean.Replace("}}", "}");
            //clean = clean.Replace("\"\"", "\"");
            clean = clean.Replace("\n", "");
            clean = clean.Replace(@"\", "");
            clean = clean.Replace("JSONNEWLINE", "\\n");
            return clean;

        }

        public static string FirstCharToUpper(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static string FirstCharToUpper2(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        public static string StringToHex(string hexstring)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char t in hexstring)
            {
                //Note: X for upper, x for lower case letters
                sb.Append(Convert.ToInt32(t).ToString("X"));
            }
            return sb.ToString();
        }
    }
}
