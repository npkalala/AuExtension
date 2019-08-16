using Sample.Dialog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
//using 
using AuExtension;
using AuExtension.Extend.FormAnimator;
using AuExtension.Extend.DeviceManager;
using AuExtension.Extend.HotKeys;
using AuExtension.Extend.SystemTray;
using AuExtension.Extend.NetWork;
using AuExtension.Extend.VoiceControl;
using AuExtension.Extend.Util;
using AuExtension.Extend.HttpExtension;
using AuExtension.Extend.Brightness;
using System.Reflection;
using AuExtension.Extend.Log;

/*
  Remark : Need import Lib CoreAudioApi
*/
namespace Sample
{
    internal class Program
    {
        private static MyApplicationContext _systemTray;
        public static MyApplicationContext SystemTray
        {
            get
            {
                if (_systemTray == null)
                {
                    _systemTray = new MyApplicationContext();
                }
                return _systemTray;
            }
            set
            {
                _systemTray = value;
            }
        }

        private static IHttpExt _httpExtension;
        public static IHttpExt HttpExtension
        {
            get
            {
                return _httpExtension = _httpExtension ?? new HttpExt();
            }
            set { _httpExtension = value; }
        }


        private static HotKey hotkeys;
        [STAThread]
        private static void Main(string[] args)
        {
            #region Test
            Log.WriteLine("Log");
            SqlLog.WriteLine("SqlLog");
            #endregion

            // 1. SystemExt
            Sample_SystemExt();

            // 2. Form Animaton
            FormAnimation frm = new FormAnimation();
            frm.ShowAsync();

            // 3. DeviceManager
            var dtUSBDevice = DeviceManager.GetUSBDevices();
            var dtDriver = DeviceManager.GetDriver();
            var dtDeviceDetail = DeviceManager.GetDeviceDetail();
            var dtBusInfo = DeviceManager.GetBusInfo();

            // 4. NetWork Detect
            // Implement Delegate Event "AvailabilityChanged"
            NetworkStatus.AvailabilityChanged += NetworkStatus_AvailabilityChanged;
            Debug.WriteLine("The Network is " + (NetworkStatus.IsAvailable ? "Connect" : "DisConnect"));
            /* Output
             The Network is Connect
             //close wifi
             The Network is DisConnect
             //open wifi
             The thread 0xa844 has exited with code 0 (0x0).
             The Network is Connect
             */

            // 5. Set System Volume
            //SystemVoice.AuInit();
            Debug.WriteLine("The voice volume is " + SystemVoice.GetVolume());
            SystemVoice.SetVolume(0);
            Debug.WriteLine("The voice volume is " + SystemVoice.GetVolume());
            /* Output
             The voice volume is 10
             The voice volume is 0
             */

            // 6. Minimize Console UI
            SysUtil.HideApplication();
            SysUtil.KillProcessByName("Sample");

            // 7. Http Extension
            // Setup URL and Header(if neccesary)
            HttpExtension.URL = "http://192.168.100.235:9000/dev/api/v1/";
            var header = new Dictionary<string, string>();
            header.Add("Authorization", "Token 3c1fa688462c30c105df08326406d4fb");
            HttpExtension.Headers = header;

            var dtCity = HttpExtension.Get<Cities>("lists/cascades?cascade-id=1");
            var dtCityAsync = HttpExtension.GetAsync<Cities>("lists/cascades?cascade-id=1").Result;
            var strCity = HttpExtension.GetStrAsync("lists/cascades?cascade-id=1").Result;
            //var dtUser = HttpExtension.Post<User>("Users", "{\"ids\":[1,2,3]}");
            //var dtUserAsync = HttpExtension.PostAsync<User>("Users", "{\"ids\":[1,2,3]}").Result;
            //var strUser = HttpExtension.PostStrAsync("Users", "{\"ids\":[1,2,3]}").Result;

            // 8. Monitor Display's Brightness Get/Set
            Debug.WriteLine("The Brightness is "+ Display.GetBrightness());
            Debug.WriteLine("Set Brightness to 20 ");
            Display.SetBrightness(20);
            System.Threading.Thread.Sleep(2000);
            Debug.WriteLine("Set Brightness to 100 ");
            Display.SetBrightness(100);

            while (true)
            {
                var result = Console.ReadLine();
                if (result.Equals("q"))
                    break;
            }
        }

        public class Cities
        {
            public List<City> cities { get; set; }
         
        }

        public class City
        {
            public int city_id { get; set; }
            public string city_name { get; set; }
            public string city_create_time { get; set; }
        }

        public class User
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        private static void NetworkStatus_AvailabilityChanged(object sender, NetworkStatusChangedArgs e)
        {
            Debug.WriteLine("The Network is "+ (NetworkStatus.IsAvailable?"Connect":"DisConnect"));
        }

        private static void Hotkeys_HotkeyPressed(int ID)
        {
            string msg = "";
            switch (ID)
            {
                case 1007:
                    msg = "F7";
                    break;
                case 1001:
                    msg = "F1";
                    break;
            }
            Debug.Write("you press " + msg);
        }

        private static void Sample_SystemExt()
        {
            #region
            // 1. IsNullOrEmpty<T>
            List<string> lsCity = new List<string>() { "Kaohsiung", "Taipei", "Tainan" };
            Debug.WriteLine(lsCity.IsNullOrEmpty());

            string context = "";
            Debug.WriteLine(context.IsNullOrEmpty());

            float? confidence = null;
            Debug.WriteLine(confidence.IsNullOrEmpty());

            double? rate = null;
            Debug.WriteLine(rate.IsNullOrEmpty());

            int? score = 99;
            Debug.WriteLine(score.IsNullOrEmpty());

            /* Output
            False
            True
            True
            True
            False
             */

            //2. NullAs<T>
            string result = null;
            result = result.NullAs("Hello World");
            Debug.WriteLine(result);

            result = "The confidence is " + confidence.NullAs("0");
            Debug.WriteLine(result);

            result = "The rate is " + rate.NullAs("0");
            Debug.WriteLine(result);

            result = "The score is " + score.NullAs("0");
            Debug.WriteLine(result);

            /* Output
            Hello World
            The confidence is 0
            The rate is 0
            The score is 99
             */

            // 3. Regex ReplaceAll
            String s = "John Smith $100,000.00 M";
            s = s.ReplaceAll(@"\s+\$|\s+(?=\w+$)", ",");
            Debug.WriteLine(s);

            /* Output
             John Smith,100,000.00,M
             */

            // 4. Regex IsMatch
            string number = "1298-673-4192";
            string status = number.IsMatch(@"^[a-zA-Z0-9]\d{2}[a-zA-Z0-9](-\d{3}){2}[A-Za-z0-9]$", System.Text.RegularExpressions.RegexOptions.IgnoreCase) ? "is" : "is not";
            Debug.WriteLine($"{number} {status} a valid part number.");

            /* Output
            1298-673-4192 is a valid part number.
            */

            // 5. DateTime To Format String
            DateTime? date = DateTime.Now;
            Debug.WriteLine(date.ToDateString());

            Debug.WriteLine(date.ToDateString("D"));

            Debug.WriteLine(date.ToDateString("D1"));

            Debug.WriteLine(date.ToDateString("D2"));

            Debug.WriteLine(date.ToDateString("D3"));

            Debug.WriteLine(date.ToUniversalDateString());

            Debug.WriteLine(date.ToUniversalDateString("D"));

            Debug.WriteLine(date.ToUniversalDateString("D1"));

            Debug.WriteLine(date.ToUniversalDateString("D2"));

            Debug.WriteLine(date.ToUniversalDateString("D3"));

            /* output
            2019-04-02
            2019-04-02 13:17:36
            2019-04-02 13:17:36.997
            2019/04/02 13:17:36
            2019/04/02 13:17:36.997
            2019-04-02T
            2019-04-02T05:17:36
            2019-04-02T05:17:36.997Z
            2019/04/02T05:17:36
            2019/04/02T05:17:36.997Z
             */

            //6. ContainList
            Debug.WriteLine("This is Kaohsiung".ContainList(lsCity));

            string[] arCity = lsCity.ToArray();
            Debug.WriteLine("This is Tokyo".ContainList(arCity));
            /* Output
               True
               False
            */
            #endregion

            //Linq

            List<City> dtCity = new List<City>()
            {
                new City()
                {
                    city_id = 1,
                    city_name ="KH",
                    city_create_time = "2011/01/01"
                },
                new City()
                {
                    city_id = 1,
                    city_name ="KH",
                    city_create_time = "2011/01/02"
                },
                new City()
                {
                    city_id = 2,
                    city_name ="KH",
                    city_create_time = "2011/11/01"
                }
            };

            var cityName = dtCity.DistinctBy(t => new { t.city_name, t.city_id }).ToList();
        }

    }
}