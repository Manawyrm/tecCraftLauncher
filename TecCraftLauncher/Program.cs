using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace TecCraftLauncher
{
    static class Program
    {
        static public IniFile LocalConfig;
        static public Boolean UnixExecution = false;
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            String ConfigFile = System.IO.Path.Combine(Application.StartupPath, "config.ini");
            if (!System.IO.File.Exists(ConfigFile))
            {
                System.IO.File.Copy("config.def.ini", ConfigFile);
            }

            try
            {
                if (args.Length != 0)
                {
                    if (args[0].Contains("--help"))
                    {
                        Console.WriteLine("Kommandozeilenargumente zum tecCraft Launcher\n\nEs besteht die Möglichkeit via --cfg <Dateiname> eine Konfigurationsdatei zu spezifizieren.\n\nWenn diese noch nicht vorhanden ist, wird sie erstellt.");
                        System.Environment.Exit(0);
                    }

                    if (args[0].Contains("--cfg"))
                    {
                        if (args.Length != 2)
                        {
                            Console.WriteLine("Bitte nutze teccraftlauncher.exe --help.\n Benutzung: TecCraftLauncher.exe --cfg <Dateiname>");
                            System.Environment.Exit(0);
                        }
                        ConfigFile = System.IO.Path.Combine(Application.StartupPath, args[1]);
                        if (!System.IO.File.Exists(ConfigFile))
                        {
                            System.IO.File.Copy("config.def.ini", ConfigFile);
                        }
                    }
                    if (args[0].Contains("--linux"))
                    {
                        UnixExecution = true;
                    }
                }
            }
            catch (Exception)
            {
                System.Environment.Exit(0);
            }
           
            LocalConfig = new IniFile(ConfigFile);
            System.Net.WebRequest.DefaultWebProxy = null;

            Application.Run(new Main());
        }
    }
}
