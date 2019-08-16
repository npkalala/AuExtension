using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace AuExtension.Extend
{
    class SysExt
    {
    }

    public static class ObjectExtension
    {
        public static bool IsNullOrEmpty<T>(this T v)
        {
            if (v is string)
            {
                if (v.ToString() == "") return true;
            }
            if (v == null) return true;
            return false;
        }

        public static string NullAs<T>(this T v, string replace)
        {
            if (v == null) return replace;
            return v.ToString();
        }

        public static string ReplaceAll(this string v, string pattern, string replaceAs)
        {
            Regex digitsOnly = new Regex(pattern);
            return digitsOnly.Replace(v, replaceAs);
        }

        public static bool IsMatch(this string v, string pattern, RegexOptions option = RegexOptions.IgnoreCase)
        {
            Regex r = new Regex(pattern, option);
            return r.IsMatch(v);
        }

        public static bool IsEmpty(this Bitmap image)
        {
            var data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, image.PixelFormat);
            var bytes = new byte[data.Height * data.Stride];
            Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
            image.UnlockBits(data);
            return bytes.All(x => x == 0);
        }

        public static string ToDayHourMinuteString(this int seconds)
        {
            if (seconds == -1)
                return " unknown ";// "-1";
            string msg = "";
            var days = seconds / (60 * 60 * 24);
            seconds -= days * (60 * 60 * 24);
            var hours = seconds / (60 * 60);
            seconds -= hours * (60 * 60);
            var minutes = seconds / 60;
            if (days > 0)
                msg += " " + days + " day";
            if (hours > 0)
                msg += " " + hours + " hrs";
            if (minutes > 0)
                msg += " " + minutes + " minutes";
            return msg;
        }

        public static bool ContainList(this string v, List<string> compares)
        {
            if (v.IsNullOrEmpty()) return false;
            if (compares.Count == 1)
                return v.Contains(compares.First());
            foreach (var dr in compares)
            {
                if (v.Contains(dr))
                    return true;
            }
            return false;
        }

        public static bool ContainList(this string v, string[] compares)
        {
            if (v.IsNullOrEmpty()) return false;
            if (compares.Length == 1)
                return v.Contains(compares.First());
            foreach (string dr in compares)
            {
                if (v.Contains(dr))
                    return true;
            }
            return false;
        }
    }
}
