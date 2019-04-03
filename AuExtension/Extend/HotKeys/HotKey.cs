using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AuExtension.Extend.HotKeys
{
    public class HotKey : NativeWindow, IDisposable
    {
        private const int WM_HOTKEY = 0x0312;
        private const int WM_DESTROY = 0x0002;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private List<Int32> IDs = new List<int>();

        public delegate void HotkeyDelegate(int ID);

        public event HotkeyDelegate HotkeyPressed;

        public HotKey()
        {
            this.CreateHandle(new CreateParams());
        }

        public void RegisterCombo(Int32 ID, int vlc, int fsModifiers = 0)
        {
            if (RegisterHotKey(this.Handle, ID, fsModifiers, vlc))
            {
                IDs.Add(ID);
            }
        }

        public void Dispose()
        {
            this.DestroyHandle();
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    if (HotkeyPressed != null)
                    {
                        HotkeyPressed(m.WParam.ToInt32());
                    }
                    break;

                case WM_DESTROY: // fires when "Application.Exit();" is called
                    foreach (int ID in IDs)
                    {
                        UnregisterHotKey(this.Handle, ID);
                    }
                    break;
            }
            base.WndProc(ref m);
        }
    }
}