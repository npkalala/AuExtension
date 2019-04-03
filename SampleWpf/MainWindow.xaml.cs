using System;
using System.Collections.Generic;
using System.Windows;
//using namespace
using AuExtension.Extend.HotKeys;
using AuExtension.Extend.SystemTray;

namespace SampleWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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

        public MainWindow()
        {
            InitializeComponent();
            // 1. System Tray

            #region Declare Title and Action

            List<TooltipItem> ls = new List<TooltipItem>();
            ls.Add(new TooltipItem()
            {
                Title = "close",
                Click = new Action(() =>
                {
                    MessageBox.Show("you press close button");
                })
            });
            ls.Add(new TooltipItem()
            {
                Title = "logout",
                Click = new Action(() =>
                {
                    MessageBox.Show("you press logout button");
                })
            });

            #endregion Declare Title and Action

            SystemTray.SetTitle("I noticed that you double-clicked me! What can I do for you?", "This is AuExtension", "AuExtension");
            SystemTray.SetIcon(AppDomain.CurrentDomain.BaseDirectory + "\\Icon\\start_ico.ico");
            SystemTray.LoadItems(ls);

            // 2. HotKey event
            List<HotKeyItem> lsHotKey = new List<HotKeyItem>();
            lsHotKey.Add(new HotKeyItem()
            {
                Id = 1001,
                Key = System.Windows.Forms.Keys.F1,
                PressEvent = new Action(() =>
                {
                    MessageBox.Show("you press F1 button");
                })
            });
            SystemTray.RegisterHotKey(lsHotKey);
        }
    }
}