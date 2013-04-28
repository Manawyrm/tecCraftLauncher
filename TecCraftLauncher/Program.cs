using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TecCraftLauncher
{
    static class Program
    {
        static public IniFile LocalConfig = new IniFile(System.IO.Path.Combine(Application.StartupPath,"config.ini"));
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Net.WebRequest.DefaultWebProxy = null;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            Application.Run(new Main());
        }
    }
}
