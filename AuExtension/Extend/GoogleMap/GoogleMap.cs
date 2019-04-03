using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AuExtension.Extend.GoogleMap
{
    public partial class GoogleMap : UserControl
    {
        public GoogleMap()
        {
            InitializeComponent();
        }

        public void Show(string lat,string lon,string country,string city)
        {
            try
            {
                string marker = "marker.png";
                string image = "image.png";
                string path = Application.StartupPath + "\\Html\\map.html";
                this.Text = path;
                string filename = Application.StartupPath + "\\Html\\mymap.html";
                Variables.replace(filename, lat, lon, country, city, marker, image, path);
                this.webBrowser1.Url = new Uri(path);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
