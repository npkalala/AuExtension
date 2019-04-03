using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AuExtension.Extend.SystemTray
{
    public abstract class IMyApplicationContext : ApplicationContext
    {
        public Form Settings;

        public abstract void Error(bool v, bool? AuIsMute = null);
        public abstract void Close();
        public abstract void LogOut();
        public abstract bool OverWriteConfig();
        public abstract void RemoveIcon();
        public abstract void ForceClose(bool IsListenChinese);
        public abstract void ShowAllMenu();
    }
}
