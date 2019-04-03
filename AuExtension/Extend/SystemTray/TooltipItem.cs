using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuExtension.Extend.SystemTray
{
    public class TooltipItem
    {
        public string Title { get; set; }
        public Action Click { get; set; }
    }
}
