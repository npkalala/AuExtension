using System.IO;

namespace AuExtension.Extend.GoogleMap
{
    public class Variables
    {
        public static void replace(string filename, string la, string lo, string country, string city, string marker, string image, string path)
        {
            if (File.Exists(filename))
            {
                StreamReader reader = new StreamReader(filename);
                string readFile = reader.ReadToEnd();
                string sb = "";
                sb = readFile;
                sb = sb.Replace("[la]", la);
                sb = sb.Replace("[lo]", lo);
                sb = sb.Replace("[country]", country);
                sb = sb.Replace("[city]", city);
                sb = sb.Replace("[image]", image);
                sb = sb.Replace("[marker]", marker);

                readFile = sb.ToString();
                reader.Close();
                StreamWriter writer = new StreamWriter(path);
                writer.Write(readFile);
                writer.Close();

                writer = null;
                reader = null;
            }
            else
                System.Windows.Forms.MessageBox.Show("Im Sorry About That !");
        }
    }
}