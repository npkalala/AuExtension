using AuExtension.Extend.HotKey;
using AuExtension;
using System;
using System.Collections.Generic;
using System.Drawing;
// add Reference PresentationFramework
using System.Windows.Forms;
using System.Windows.Input;
using System.Linq;

namespace SampleWpf
{
    public class MyApplicationContext : ApplicationContext
    {
        //Component declarations
        private NotifyIcon _trayIcon;
        private ContextMenuStrip _trayIconContextMenu;
        private HotKey hotkeys;

        public MyApplicationContext()
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            InitializeComponent();
            _trayIcon.Visible = true;
        }

        private void InitializeComponent()
        {
            _trayIcon = new NotifyIcon();
            _trayIcon.BalloonTipIcon = ToolTipIcon.Info;
            //_trayIcon.BalloonTipText = "I noticed that you double-clicked me! What can I do for you?";
            //_trayIcon.BalloonTipTitle = "This is AuExtension";
            //_trayIcon.Text = "AuExtension";

            //The icon is added to the project resources. Here I assume that the name of the file is '_trayIcon.ico'
            _trayIcon.Icon = new Icon(AppDomain.CurrentDomain.BaseDirectory + "\\Icon\\start_ico.ico");

            // RegisterHotKey
            hotkeys = new HotKey();
            hotkeys.RegisterCombo(1001, (int)ModifierKeys.None, (int)Keys.F7);
            hotkeys.RegisterCombo(1002, (int)ModifierKeys.None, (int)Keys.F1);
            hotkeys.HotkeyPressed += Hotkeys_HotkeyPressed;

            //Optional - handle doubleclicks on the icon:
            _trayIcon.DoubleClick += TrayIconDoubleClick;

            //Optional - Add a context menu to the _trayIcon:
            _trayIconContextMenu = new ContextMenuStrip();

            this._trayIconContextMenu.Name = "_trayIconContextMenu";
            this._trayIconContextMenu.Size = new Size(153, 70 * 8);

            _trayIconContextMenu.ResumeLayout(false);

            _trayIcon.ContextMenuStrip = _trayIconContextMenu;
        }

        private List<TooltipItem> _dtToolSet = new List<TooltipItem>();

        public void SetTitle(string balloonTipText, string balloonTipTitle, string Text)
        {
            _trayIcon.BalloonTipText = balloonTipText;
            _trayIcon.BalloonTipTitle = balloonTipTitle;
            _trayIcon.Text = Text;
        }

        public void LoadItems(List<TooltipItem> lsTooltips)
        {
            _dtToolSet = lsTooltips;
            List<ToolStripMenuItem> lsMenuItems = new List<ToolStripMenuItem>();
            foreach(TooltipItem r in lsTooltips)
            {
                var item = new ToolStripMenuItem();

                item.Name = "_"+r.Title;
                item.Size = new Size(152, 22);
                item.Text = r.Title;
                item.Click += Item_Click;
                lsMenuItems.Add(item);
            }

            this._trayIconContextMenu.Items.AddRange(lsMenuItems.ToArray());
            _trayIconContextMenu.ResumeLayout(false);
        }



        private void Item_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                var row = _dtToolSet.Where(t => t.Title.Equals(item.Text)).FirstOrDefault();
                if (!row.IsNullOrEmpty())
                    row.Click();
            }
            catch(Exception ex)
            {

            }
        }

        private async void Hotkeys_HotkeyPressed(int ID)
        {
            switch (ID)
            {
                case 1001:
                    break;
                case 1002:
                    break;
                default:
                    break;
            }
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            try
            {
                hotkeys.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        internal void TrayIconDoubleClick(object sender, EventArgs e)
        {
            try
            {
                _trayIcon.ShowBalloonTip(10000);
            }
            catch (Exception ex)
            {
            }
        }
    }

    public class TooltipItem
    {
        public string Title { get; set; }
        public Action Click { get; set; }
    }
}