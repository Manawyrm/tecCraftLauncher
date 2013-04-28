using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;

namespace TecCraftLauncher
{
    class JavaTools
    {
        public static String GetJavaInstallationPath()
        {
            RegistryKey baseKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\JavaSoft\\Java Runtime Environment");
            try
            {
                String currentVersion = baseKey.GetValue("CurrentVersion").ToString();
                using (var homeKey = baseKey.OpenSubKey(currentVersion))
                    return homeKey.GetValue("JavaHome").ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static String GetJavaVersion()
        {
            RegistryKey baseKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\JavaSoft\\Java Runtime Environment");
            try
            {
                String currentVersion = baseKey.GetValue("CurrentVersion").ToString();
                return currentVersion;
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static bool JavaOK(String path)
        {
            return System.IO.File.Exists(System.IO.Path.Combine(path, "bin/javaw.exe"));
        }
        public static void LaunchMinecraft(String username, String session)
        {
            System.Diagnostics.Process jMC = new System.Diagnostics.Process();
            jMC.StartInfo.FileName = System.IO.Path.Combine(Program.LocalConfig.IniReadValue("Launcher", "javapath"), "bin/javaw.exe");
            jMC.StartInfo.Arguments = "-Djava.net.preferIPv4Stack=" + Program.LocalConfig.IniReadValue("Launcher", "ipv6").ToLower() + " -jar JavaLoader.jar \"" + username + "\" \"" + session + "\" teccraft.de 25565";
            jMC.Start();
        }
    
    }
}
