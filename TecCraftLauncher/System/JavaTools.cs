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
                RegistryKey homeKey = baseKey.OpenSubKey(currentVersion);
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
            if (Program.UnixExecution)
            {
                return true;
            }
            return System.IO.File.Exists(System.IO.Path.Combine(path, "bin/javaw.exe"));
        }
        public static void LaunchMinecraft(String username, String session)
        {
            System.Diagnostics.Process jMC = new System.Diagnostics.Process();
            if (Program.UnixExecution)
            {
                //Linux
                jMC.StartInfo.FileName = "/bin/sh";
            }
            else
            {
                //Windows
                jMC.StartInfo.FileName = System.IO.Path.Combine(Program.LocalConfig.IniReadValue("Launcher", "javapath"), "bin/javaw.exe");
            }

            if (Program.UnixExecution)
            {
                //Linux
                String javapath = Program.LocalConfig.IniReadValue("Launcher", "javapath");
                jMC.StartInfo.Arguments = "-c \"LD_LIBRARY_PATH=" + javapath + "/lib/amd64/ " + javapath + "/bin/java -Djava.net.preferIPv4Stack=" + Program.LocalConfig.IniReadValue("Launcher", "ipv6").ToLower() + " -jar JavaLoader.jar \"" + username + "\" \"" + session + "\" teccraft.de 25565\"";
            }
            else
            {
                //Windows
                 jMC.StartInfo.Arguments = "-Djava.net.preferIPv4Stack=" + Program.LocalConfig.IniReadValue("Launcher", "ipv6").ToLower() + " -jar JavaLoader.jar \"" + username + "\" \"" + session + "\" teccraft.de 25565";
            }
           
            jMC.Start();
        }
    
    }
}
