using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AuExtension.Extend.HotKeys
{
    public class HotKeyItem
    {
        public int Id { get; set; }
        public Keys Key { get; set; }
        public Action PressEvent { get; set; }
    }
}
