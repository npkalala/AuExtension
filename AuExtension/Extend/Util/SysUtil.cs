using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace AuExtension.Extend.Util
{
    public class SysUtil
    {
        public static bool KillProcessByName(string name)
        {
            if (name.IsNullOrEmpty())
                return false;
            try
            {
                foreach (var process in Process.GetProcessesByName(name))
                {
                    if (process.Id != Process.GetCurrentProcess().Id)
                        process.Kill();
                }
                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        #region Native methods
        //[ComVisibleAttribute(false),
        // System.Security.SuppressUnmanagedCodeSecurity()]
        internal class NativeMethods
        {
            [DllImport("user32.dll")]

            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            [DllImport("user32.dll")]

            public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

            [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
            public static extern IntPtr GetConsoleWindow();
        }
        #endregion

        public static void HideApplication()
        {
            Console.Title = "HideApplication";
            //put your console window caption here
            IntPtr hWnd = NativeMethods.GetConsoleWindow(); 
            if (hWnd != IntPtr.Zero)
            {
                //Hide the window
                NativeMethods.ShowWindow(hWnd, 0); // 0 = SW_HIDE
            }
        }
    }
}
