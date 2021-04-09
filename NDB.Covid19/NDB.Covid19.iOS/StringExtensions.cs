using System;
using System.Globalization;
using UIKit;

namespace NDB.Covid19.iOS
{
    public static class StringExtensions
    {
        public static UIColor ToUIColor(this string hexString)
        {
            hexString = hexString.Replace("#", "");

            if (hexString.Length == 3)
                hexString = hexString + hexString;

            if (hexString.Length != 6)
                throw new Exception("Invalid hex string");

            int red = int.Parse(hexString.Substring(0, 2), NumberStyles.AllowHexSpecifier);
            int green = int.Parse(hexString.Substring(2, 2), NumberStyles.AllowHexSpecifier);
            int blue = int.Parse(hexString.Substring(4, 2), NumberStyles.AllowHexSpecifier);

            return UIColor.FromRGB(red, green, blue);
        }
    }
}