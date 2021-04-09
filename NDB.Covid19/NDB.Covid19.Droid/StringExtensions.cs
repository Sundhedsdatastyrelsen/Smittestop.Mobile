using System;
using System.Globalization;
using Android.Graphics;

namespace NDB.Covid19.Droid
{
    public static class StringExtensions
    {
        public static Color ToColor(this string hexString)
        {
            hexString = hexString.Replace("#", "");

            if (hexString.Length == 3)
                hexString = hexString + hexString;

            if (hexString.Length != 6)
                throw new Exception("Invalid hex string");

            int red = int.Parse(hexString.Substring(0, 2), NumberStyles.AllowHexSpecifier);
            int green = int.Parse(hexString.Substring(2, 2), NumberStyles.AllowHexSpecifier);
            int blue = int.Parse(hexString.Substring(4, 2), NumberStyles.AllowHexSpecifier);

            return new Color(red, green, blue);
        }
    }
}