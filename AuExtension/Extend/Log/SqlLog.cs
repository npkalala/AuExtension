using System;

namespace AuExtension.Extend.Log
{
    public class SqlLog : LogBase
    {
        private static string _ext = ".sql";
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