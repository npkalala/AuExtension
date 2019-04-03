using System.Collections.Generic;
using System.Linq;
// need add reference System.Management.
using System.Management;

namespace AuExtension.Extend.DeviceManager
{
    //http://stackoverflow.com/questions/3331043/get-list-of-connected-usb-devices
    public class DeviceManager
    {
        public static List<USBDeviceInfo> GetUSBDevices()
        {
            List<USBDeviceInfo> devices = new List<USBDeviceInfo>();

            ManagementObjectCollection collection;
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub"))
                collection = searcher.Get();

            foreach (var device in collection)
            {
                string deviceId = (string)device.GetPropertyValue("DeviceID");
                int vidIndex = deviceId.IndexOf("VID_");
                string startingAtVid = deviceId.Substring(vidIndex + 4); // + 4 to remove "VID_"
                string vid = startingAtVid.Substring(0, 4); // vid is four characters long

                int pidIndex = deviceId.IndexOf("PID_");
                string startingAtPid = deviceId.Substring(pidIndex + 4); // + 4 to remove "PID_"
                string pid = startingAtPid.Substring(0, 4); // pid is four characters long

                devices.Add(new USBDeviceInfo(
                deviceId,
                (string)device.GetPropertyValue("PNPDeviceID"),
                (string)device.GetPropertyValue("Description"),
                pid, vid
                ));
            }

            collection.Dispose();
            return devices;
        }

        //https://msdn.microsoft.com/en-us/library/aa394084(v=VS.85).aspx
        public static List<DeviceDetails> GetDeviceDetail()
        {
            List<DeviceDetails> ls = new List<DeviceDetails>();
            var win32DeviceClassName = "win32_processor";
            var query = string.Format("select * from {0}", win32DeviceClassName);

            using (var searcher = new ManagementObjectSearcher(query))
            {
                ManagementObjectCollection objectCollection = searcher.Get();

                foreach (ManagementBaseObject managementBaseObject in objectCollection)
                {
                    foreach (PropertyData propertyData in managementBaseObject.Properties)
                    {
                        ls.Add(new DeviceDetails(propertyData.Name, propertyData.Value.NullAs("").ToString()));
                    }
                }
            }
            return ls;
        }

        public static List<DriverInfo> GetDriver()
        {
            List<DriverInfo> ls = new List<DriverInfo>();
            ManagementObjectSearcher objSearcher = new ManagementObjectSearcher("Select * from Win32_PnPSignedDriver");
            ManagementObjectCollection objCollection = objSearcher.Get();
            foreach (ManagementObject obj in objCollection)
            {
                //string info = String.Format("Device='{0}',Manufacturer='{1}',DriverVersion='{2}' ", obj["DeviceName"], obj["Manufacturer"], obj["DriverVersion"]);
                //Console.Out.WriteLine(info);
                ls.Add(new DriverInfo(obj["DeviceName"].NullAs("").ToString(), obj["Manufacturer"].NullAs("").ToString(), obj["DriverVersion"].NullAs("").ToString()));
            }
            return ls;
        }

        public static List<BusInfo> GetBusInfo()
        {
            List<BusInfo> devices = new List<BusInfo>();

            ManagementObjectSearcher searcher =
            new ManagementObjectSearcher("SELECT * FROM Win32_Bus where BusType = 5");
            List<string> ls_BusDeviceIds = new List<string>();
            foreach (ManagementObject queryObj in searcher.Get())
            {
                ls_BusDeviceIds.Add(queryObj["DeviceID"].NullAs("").ToString());
            }

            List<string> ls_ids = new List<string>();

            #region //Select* From Win32_PnPEntity Where DeviceId Like '%KINGSTON%'

            searcher =
            new ManagementObjectSearcher("Select * From Win32_PnPEntity Where DeviceId Like '%PCI%' or DeviceId Like '%HDAUDIO%' ");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                var PnP_DeviceId = queryObj["DeviceID"].ToString();

                int vidIndex = PnP_DeviceId.IndexOf("VEN_");
                string startingAtVid = PnP_DeviceId.Substring(vidIndex + 4); // + 4 to remove "VID_"
                string vid = startingAtVid.Substring(0, 4); // vid is four characters long

                int pidIndex = PnP_DeviceId.IndexOf("DEV_");
                string startingAtPid = PnP_DeviceId.Substring(pidIndex + 4); // + 4 to remove "PID_"
                string pid = startingAtPid.Substring(0, 4); // pid is four characters long

                devices.Add(new BusInfo("", PnP_DeviceId, pid, vid));
            }

            #endregion //Select* From Win32_PnPEntity Where DeviceId Like '%KINGSTON%'

            #region 正統方法  但是太慢

            //searcher =
            //new ManagementObjectSearcher("SELECT * FROM Win32_DeviceBus");
            //foreach (ManagementObject queryObj in searcher.Get())
            //{
            //    var Antecedent = queryObj["Antecedent"].ToString();
            //    string DeviceID = AuSearch2(Antecedent, "DeviceID");

            //    if (ls_BusDeviceIds.Contains(DeviceID))
            //    {
            //        var d = queryObj["Dependent"];
            //        var PnP_DeviceId = AuSearch2(d.ToString(), "DeviceID");

            //        int vidIndex = PnP_DeviceId.IndexOf("VEN_");
            //        string startingAtVid = PnP_DeviceId.Substring(vidIndex + 4); // + 4 to remove "VID_"
            //        string vid = startingAtVid.Substring(0, 4); // vid is four characters long

            //        int pidIndex = PnP_DeviceId.IndexOf("DEV_");
            //        string startingAtPid = PnP_DeviceId.Substring(pidIndex + 4); // + 4 to remove "PID_"
            //        string pid = startingAtPid.Substring(0, 4); // pid is four characters long

            //        devices.Add(new BusInfo(DeviceID, PnP_DeviceId,pid,vid));
            //    }
            //}

            #endregion 正統方法  但是太慢

            return devices;
        }

        public static List<ManagementObject> AuSearch(string scope, string query)
        {
            var Asearcher = new ManagementObjectSearcher(scope, "SELECT * FROM Win32_PnPEntity");
            return Asearcher.Get().Cast<ManagementObject>().ToList();
        }

        public static string AuSearch2(string scope, string field)
        {
            ManagementObject Mbj = new ManagementObject(scope);
            return Mbj[field].ToString();
        }
    }

    public class BusInfo
    {
        public BusInfo(string Bus_DeviceID, string Pnp_DeviceID, string PId, string VId)
        {
            this.BusDeviceID = Bus_DeviceID;
            this.PnpDeviceID = Pnp_DeviceID;
            this.PId = PId;
            this.VId = VId;
        }

        public string BusDeviceID { get; private set; }
        public string PnpDeviceID { get; private set; }
        public string PId { get; private set; }
        public string VId { get; private set; }
    }

    public class USBDeviceInfo
    {
        public USBDeviceInfo(string deviceID, string pnpDeviceID, string description, string PId, string VId)
        {
            this.DeviceID = deviceID;
            this.PnpDeviceID = pnpDeviceID;
            this.Description = description;
            this.PId = PId;
            this.VId = VId;
        }

        public string DeviceID { get; private set; }
        public string PnpDeviceID { get; private set; }
        public string Description { get; private set; }
        public string PId { get; private set; }
        public string VId { get; private set; }
    }

    public class DeviceDetails
    {
        public DeviceDetails(string Property, string Value)
        {
            this.Property = Property;
            this.Value = Value;
        }

        public string Property { get; private set; }
        public string Value { get; private set; }
    }

    public class DriverInfo
    {
        public DriverInfo(string Device, string Manufacturer, string DriverVersion)
        {
            this.Device = Device;
            this.Manufacturer = Manufacturer;
            this.DriverVersion = DriverVersion;
        }

        public string Device { get; private set; }
        public string Manufacturer { get; private set; }
        public string DriverVersion { get; private set; }
    }

}
