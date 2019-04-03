using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace AuExtension
{
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

        public static string ToDateString(this DateTime date, string format = "")
        {
            return ToDateString((DateTime?)date, format);
        }

        public static string ToDateString(this DateTime? date, string format = "")
        {
            if (date.IsNullOrEmpty())
                return "";
            string type = "";
            switch (format)
            {
                case "D":
                    type = "yyyy-MM-dd HH:mm:ss";
                    break;
                case "D1":
                    type = "yyyy-MM-dd HH:mm:ss.fff";
                    break;
                case "D2":
                    type = "yyyy/MM/dd HH:mm:ss";
                    break;
                case "D3":
                    type = "yyyy/MM/dd HH:mm:ss.fff";
                    break;
                default:
                    type = "yyyy-MM-dd";
                    break;
            }
            return date.Value.ToString(type);
        }

        public static string ToUniversalDateString(this DateTime date, string format = "")
        {
            return ToUniversalDateString((DateTime?)date, format);
        }

        public static string ToUniversalDateString(this DateTime? date, string format = "")
        {
            if (date.IsNullOrEmpty())
                return "";
            string type = "";
            switch (format)
            {
                case "D":
                    type = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
                    break;
                case "D1":
                    type = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'";
                    break;
                case "D2":
                    type = "yyyy'/'MM'/'dd'T'HH':'mm':'ss";
                    break;
                case "D3":
                    type = "yyyy'/'MM'/'dd'T'HH':'mm':'ss'.'fff'Z'";
                    break;
                default:
                    type = "yyyy'-'MM'-'dd'T'";
                    break;
            }
            return date.Value.ToUniversalTime().ToString(type);
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

        public static string GetDescription(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo.IsNullOrEmpty()) return null;
            var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
            return attribute.Description;
        }

        public static EnumNameDescription GetNameDescription(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo.IsNullOrEmpty()) return null;
            var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
            return new EnumNameDescription() { Description = attribute.Description, EnumName = fieldInfo.DeclaringType.Name };
        }

        public static void ShowAsync(this Form toastNotification, bool isAsync = true)
        {
            try
            {
                if (isAsync) //Multi Thread
                {
                    Thread myThread = new Thread((ThreadStart)delegate {
                        try
                        {
                            if (toastNotification.IsNullOrEmpty())
                                return;

                            Application.Run(toastNotification);
                        }
                        catch (Exception ex)
                        {
                            
                        }
                    }); //Initialize a new Thread of name myThread to call Application.Run() on a new instance of ViewSecond                                                                                 //myThread.TrySetApartmentState(ApartmentState.STA); //If you receive errors, comment this out; use this when doing interop with STA COM objects.
                    myThread.Start(); //Start the thread; Run the form     
                }
                else //Single Thread (Focus on Form and wait for lost focus)
                {
                    Application.Run(toastNotification);
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }

    public static class LINQExtension
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }

    public class EnumNameDescription
    {
        public string EnumName { get; set; }
        public string Description { get; set; }
    }
}
