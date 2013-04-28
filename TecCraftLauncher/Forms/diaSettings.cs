using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TecCraftLauncher
{
    public partial class diaSettings : Form
    {
        public diaSettings()
        {
            
            InitializeComponent();
            checkBox1.Checked = bool.Parse(Program.LocalConfig.IniReadValue("Launcher", "pony"));
            checkBox2.Checked = !bool.Parse(Program.LocalConfig.IniReadValue("Launcher", "ipv6"));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbdJavaPath = new FolderBrowserDialog();
            fbdJavaPath.Description = "Bitte wähle den Java Ordner aus. (Er muss einen bin Ordner enthalten!)";
            if (fbdJavaPath.ShowDialog() == DialogResult.OK)
            {
                Program.LocalConfig.IniWriteValue("Launcher", "javapath", fbdJavaPath.SelectedPath);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBox1.Checked)
                {
                    System.IO.File.Copy("bin_client\\mods\\mlp.off", "bin_client\\mods\\mlp.zip");
                    Program.LocalConfig.IniWriteValue("Launcher", "pony", checkBox1.Checked.ToString());
                }
                else
                {
                    System.IO.File.Delete("bin_client\\mods\\mlp.zip");
                    Program.LocalConfig.IniWriteValue("Launcher", "pony", checkBox1.Checked.ToString());
                }
            }
            catch (Exception)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Program.LocalConfig.IniWriteValue("Launcher", "ipv6", (!checkBox2.Checked).ToString());
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBox3.Checked)
                {
                    System.IO.File.Copy("bin_client\\coremods\\nei.off", "bin_client\\coremods\\nei.jar");
                    System.IO.File.Copy("bin_client\\mods\\nei_rp.off", "bin_client\\mods\\nei_rp.jar");
                    Program.LocalConfig.IniWriteValue("Launcher", "nei", checkBox3.Checked.ToString());
                }
                else
                {
                    System.IO.File.Delete("bin_client\\coremods\\nei.jar");
                    System.IO.File.Delete("bin_client\\coremods\\nei_rp.jar");
                    Program.LocalConfig.IniWriteValue("Launcher", "nei", checkBox3.Checked.ToString());
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
