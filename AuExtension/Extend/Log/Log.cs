using System;
using System.IO;

namespace AuExtension.Extend.Log
{
    public class LogBase
    {
        #region
        public static bool IsTestMode = false;
        public static bool IsLog = true;
        public static void WriteLine(string value, string ext)
        {
            if (IsTestMode)
                return;
            //Console.OutputEncoding = Encoding.GetEncoding(950);
            Console.WriteLine(value);
            if (IsLog)
                WriteLog(value, ext);
        }

        public static void WriteLine(Exception ex, string ext)
        {
            if (IsTestMode)
                return;
            //Console.OutputEncoding = Encoding.GetEncoding(950);
            Console.WriteLine(ex.StackTrace.ToString());
            if (IsLog)
                WriteLog(ex.StackTrace.ToString(), ext);
        }

        private static object _lockLog = new object();

        public static void WriteLog(string msg, string ext)
        {
            try
            {
                //今日日期
                DateTime date = DateTime.Now;
                string todayTime = date.ToString("yyyy-MM-dd HH:mm:ss");
                string today = date.ToString("yyyy-MM-dd");
                string folder = AppDomain.CurrentDomain.BaseDirectory + "\\Log\\";
                string path = folder + today + ext;//".txt";

                //檢查此路徑有無資料夾
                if (!Directory.Exists(folder))
                {
                    //新增資料夾
                    Directory.CreateDirectory(folder);
                }
                lock (_lockLog)
                {
                    //把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
                    File.AppendAllText(path, "\r\n" + todayTime + "：" + "\r\n" + msg);
                }
            }
            catch
            {
            }
        }
        #endregion
    }

    public class Log : LogBase
    {
        //public static bool IsTestMode = false;
        //public static bool IsLog = true;

        //public static void WriteLine(string value)
        //{
        //    if (IsTestMode)
        //        return;
        //    //Console.OutputEncoding = Encoding.GetEncoding(950);
        //    Console.WriteLine(value);
        //    if (IsLog)
        //        WriteLog(value);
        //}

        //public static void WriteLine(Exception ex)
        //{
        //    if (IsTestMode)
        //        return;
        //    //Console.OutputEncoding = Encoding.GetEncoding(950);
        //    Console.WriteLine(ex.StackTrace.ToString());
        //    if (IsLog)
        //        WriteLog(ex.StackTrace.ToString());
        //}

        //private static object _lockLog = new object();

        //public static void WriteLog(string msg)
        //{
        //    try
        //    {
        //        //今日日期
        //        DateTime date = DateTime.Now;
        //        string todayTime = date.ToString("yyyy-MM-dd HH:mm:ss");
        //        string today = date.ToString("yyyy-MM-dd");
        //        string folder = AppDomain.CurrentDomain.BaseDirectory + "\\Log\\";
        //        string path = folder + today + Instance.FileExt;//".txt";

        //        //檢查此路徑有無資料夾
        //        if (!Directory.Exists(folder))
        //        {
        //            //新增資料夾
        //            Directory.CreateDirectory(folder);
        //        }
        //        lock (_lockLog)
        //        {
        //            //把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
        //            File.AppendAllText(path, "\r\n" + todayTime + "：" + msg);
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}
        private static string _ext = ".txt";
        public static void WriteLine(string value)
        {
            LogBase.WriteLine(value, _ext);
        }

        public static void WriteLine(Exception ex)
        {
            LogBase.WriteLine(ex, _ext);
        }

        public static void WriteLog(string msg)
        {
            LogBase.WriteLine(msg, _ext);
        }
    }
}