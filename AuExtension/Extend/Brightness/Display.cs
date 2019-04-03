using System;
using System.Management;
using br = Brightness;

namespace AuExtension.Extend.Brightness
{
    public static class Display
    {

        public static void SetGamma(short brn = 150)
        {
            br.Brightness.SetBrightness(brn);
        }

        /// <summary>
        /// From 0~100 Default: 60
        /// </summary>
        /// <param name="inttargetBrightness"></param>
        public static void SetBrightness(int inttargetBrightness = 60)
        {
            if (inttargetBrightness > 100)
                inttargetBrightness = 100;
            if (inttargetBrightness < 0)
                inttargetBrightness = 0;
            byte targetBrightness = new byte();
            targetBrightness = Convert.ToByte(inttargetBrightness);
            ManagementScope scope = new ManagementScope("root\\WMI");
            SelectQuery query = new SelectQuery("WmiMonitorBrightnessMethods");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
            {
                using (ManagementObjectCollection objectCollection = searcher.Get())
                {
                    foreach (ManagementObject mObj in objectCollection)
                    {
                        mObj.InvokeMethod("WmiSetBrightness",
                            new Object[] { UInt32.MaxValue, targetBrightness });
                        break;
                    }
                }
            }
        }

        public static string GetBrightness()
        {
            ManagementScope scope;
            SelectQuery query;

            scope = new ManagementScope("root\\WMI");
            query = new SelectQuery("SELECT * FROM WmiMonitorBrightness");

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
            {
                using (ManagementObjectCollection objectCollection = searcher.Get())
                {
                    foreach (ManagementObject mObj in objectCollection)
                    {
                        Console.WriteLine(mObj.ClassPath);
                        foreach (var item in mObj.Properties)
                        {
                            Console.WriteLine(item.Name + " " + item.Value.ToString());
                            if (item.Name == "CurrentBrightness")
                            {
                                //Do something with CurrentBrightness
                                return item.Value.ToString();
                            }
                        }
                    }
                }
            }
            return "";
        }
    }
}
