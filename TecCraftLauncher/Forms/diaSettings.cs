using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace TecCraftLauncher
{
    public partial class diaSettings : Form
    {
        public diaSettings()
        {
            
            InitializeComponent();
        }

        private void diaSettings_Load(object sender, EventArgs e)
        {
           javapath.Text =  Program.LocalConfig.IniReadValue("Launcher", "javapath");
           ipv6enable.Checked = !Convert.ToBoolean(Program.LocalConfig.IniReadValue("Launcher", "ipv6"));

           checkedListBox1.SetItemChecked(0, Convert.ToBoolean(Program.LocalConfig.IniReadValue("Mods", "pony")));
           checkedListBox1.SetItemChecked(1, Convert.ToBoolean(Program.LocalConfig.IniReadValue("Mods", "nei")));
           checkedListBox1.SetItemChecked(2, Convert.ToBoolean(Program.LocalConfig.IniReadValue("Mods", "minimap")));
           checkedListBox1.SetItemChecked(3, Convert.ToBoolean(Program.LocalConfig.IniReadValue("Mods", "macro")));
           checkedListBox1.SetItemChecked(4, Convert.ToBoolean(Program.LocalConfig.IniReadValue("Mods", "worldeditcui")));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbdJavaPath = new FolderBrowserDialog();
            fbdJavaPath.Description = "Bitte wähle den Java Ordner aus. (Er muss einen bin Ordner enthalten!)";
            if (fbdJavaPath.ShowDialog() == DialogResult.OK)
            {
                javapath.Text = fbdJavaPath.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.LocalConfig.IniWriteValue("Mods", "pony", checkedListBox1.GetItemChecked(0).ToString());
            Program.LocalConfig.IniWriteValue("Mods", "nei", checkedListBox1.GetItemChecked(1).ToString());
            Program.LocalConfig.IniWriteValue("Mods", "minimap", checkedListBox1.GetItemChecked(2).ToString());
            Program.LocalConfig.IniWriteValue("Mods", "macro", checkedListBox1.GetItemChecked(3).ToString());
            Program.LocalConfig.IniWriteValue("Mods", "worldeditcui", checkedListBox1.GetItemChecked(4).ToString());

            Program.LocalConfig.IniWriteValue("Launcher", "javapath", javapath.Text);
            Program.LocalConfig.IniWriteValue("Launcher", "ipv6", (!ipv6enable.Checked).ToString());

            Close();
        }
        
        //private void checkBox1_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (checkBox1.Checked)
        //        {
        //            System.IO.File.Copy("bin_client\\mods\\mlp.off", "bin_client\\mods\\mlp.zip");
        //            Program.LocalConfig.IniWriteValue("Launcher", "pony", checkBox1.Checked.ToString());
        //        }
        //        else
        //        {
        //            System.IO.File.Delete("bin_client\\mods\\mlp.zip");
        //            Program.LocalConfig.IniWriteValue("Launcher", "pony", checkBox1.Checked.ToString());
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    Close();
        //}

        //private void checkBox2_CheckedChanged(object sender, EventArgs e)
        //{
        //    Program.LocalConfig.IniWriteValue("Launcher", "ipv6", (!checkBox2.Checked).ToString());
        //}

        //private void checkBox3_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (checkBox3.Checked)
        //        {
        //            System.IO.File.Copy("bin_client\\coremods\\nei.off", "bin_client\\coremods\\nei.jar");
        //            Program.LocalConfig.IniWriteValue("Launcher", "nei", checkBox3.Checked.ToString());
    //            }
    //            else
    //            {
    //                System.IO.File.Delete("bin_client\\coremods\\nei.jar");
    //                Program.LocalConfig.IniWriteValue("Launcher", "nei", checkBox3.Checked.ToString());
    //            }
    //        }
    //        catch (Exception)
    //        {
    //        }
    //    }

    //    private void diaSettings_Load(object sender, EventArgs e)
    //    {

    //    }

    //    private void checkBox4_CheckedChanged(object sender, EventArgs e)
    //    {     
    //        try
    //        {
    //            if (checkBox4.Checked)
    //            {
    //                System.IO.File.Copy("bin_client\\mods\\minimap.off", "bin_client\\mods\\minimap.zip");
    //                Program.LocalConfig.IniWriteValue("Launcher", "minimap", checkBox4.Checked.ToString());
    //            }
    //            else
    //            {
    //                System.IO.File.Delete("bin_client\\mods\\minimap.zip");
    //                Program.LocalConfig.IniWriteValue("Launcher", "minimap", checkBox4.Checked.ToString());
    //            }
    //        }
    //        catch (Exception)
    //        {
    //        }
    //    }
    }
}
