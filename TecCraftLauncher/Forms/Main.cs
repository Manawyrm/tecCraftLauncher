using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dotNetBase.Windows.Forms.Aero;
using System.Net;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
/*
 * TecCraftLauncher
 * Geschrieben 2011-2013 von Tobias Mädel (t.maedel@alfeld.de)
 * http://tbspace.de
 */
namespace TecCraftLauncher
{
    public partial class Main : dotNetBase.Windows.Forms.Aero.glassForm
    {
        public Main()
        {
            InitializeComponent();
            
        }
        String javapath = "";
        void SearchForJavaPath()
        {
           javapath = JavaTools.GetJavaInstallationPath();
         
           if (javapath == "")
           {

               GetJava();
           }
           else
           {
               if (JavaTools.GetJavaVersion().Trim() != "1.7")
               {
                   //Java gefunden, ist aber ne alte Version.
                   GetJava();
               }
               else
               {
                   //Java gefunden. Party! :>
                   Program.LocalConfig.IniWriteValue("Launcher", "javapath", javapath);
               }

           }

        }
        void GetJava()
        {
            //Java nicht gefunden!
            DialogResult dljava = MessageBox.Show("Java konnte nicht gefunden werden!\nSoll Java heruntergeladen werden (empfohlen) oder willst du selbst den Pfad angeben?", "TecCraft Launcher - Java nicht gefunden", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dljava == DialogResult.Yes)
            {
                StartJavaDownload();
            }
            if (dljava == DialogResult.Cancel)
            {
                System.Environment.Exit(0);
            }
            if (dljava == DialogResult.No)
            {
                FolderBrowserDialog fbdJavaPath = new FolderBrowserDialog();
                fbdJavaPath.Description = "Bitte wähle den Java Ordner aus. (Er muss einen bin Ordner enthalten!)";
                if (fbdJavaPath.ShowDialog() == DialogResult.OK)
                {
                    javapath = fbdJavaPath.SelectedPath;
                    Program.LocalConfig.IniWriteValue("Launcher", "javapath", javapath);
                }
                else
                {
                    System.Environment.Exit(0);
                }
            }
        }
        void StartJavaDownload()
        {
            WebClient wcJavaDLUrl = new WebClient();
            String JavaDLUrls = wcJavaDLUrl.DownloadString("http://tbspace.de/teccraft/javadl");
            if (SystemInformation.GetArchitecture() == 64)
            {
                //64bit
                String DLUrl = JavaDLUrls.Split('|')[1];
                diaJavaLoad javaload = new diaJavaLoad(DLUrl, 64);
                javaload.ShowDialog();
                javapath = "java";
                Program.LocalConfig.IniWriteValue("Launcher", "javapath", javapath);

            }
            else
            {
                //32bit
                String DLUrl = JavaDLUrls.Split('|')[0];
                diaJavaLoad javaload = new diaJavaLoad(DLUrl, 32);
                javaload.ShowDialog();
                javapath = "java";
                Program.LocalConfig.IniWriteValue("Launcher", "javapath", javapath);
          
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (Program.LocalConfig.IniReadValue("Launcher", "javapath") == "") //Ist schon ein Pfad zu Java bekannt? 
            {
                //Nein. Such einen.
                SearchForJavaPath();
            }
            else
            {
                if (!JavaTools.JavaOK(Program.LocalConfig.IniReadValue("Launcher", "javapath"))) //Ja. Ist der OK?
                {
                    //Nein.
                    GetJava();
                }
            }

            updateController1.checkForUpdates();
            if (updateController1.currentUpdateResult.UpdatesAvailable)
            {
                updateController1.updateInteractive();

                Opacity = 0; //Schönheitsfehler - Flackert kurz auf.
                Application.Exit();
            }

            //Erstmal Steve =)
            ShowSkin(Program.LocalConfig.IniReadValue("Launcher", "username"));
            if (Program.LocalConfig.IniReadValue("Launcher", "username")  != "steve")
            {
                tbUsername.Text = Program.LocalConfig.IniReadValue("Launcher", "username");
            }
            tbPassword.Text = Program.LocalConfig.IniReadValue("Launcher", "password");
            
        }

        private void DownloadCompleted(Object sender, DownloadDataCompletedEventArgs e)
        {
            //Download des Skins fertig. 
            if (e.Error != null) //Hoppla. 
            {
                //Spieler hat keinen Skin -> Steve.
                minecraftModelView1.Skin = TecCraftLauncher.Properties.Resources.stdchar;
            }
            else
            {
                //Ergebnis in Byte-Array laden, in MemoryStream schreiben und als Image einlesen.
                byte[] bytes = e.Result;
                Bitmap img;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    img = (Bitmap)Image.FromStream(ms);
                }
                //und direkt zuweisen
                minecraftModelView1.Skin = img;
            }
            
        }
        private void ShowSkin(String player)
        {
            System.Net.WebClient wcskin = new System.Net.WebClient();
            wcskin.Proxy = null; //Workaround. 
            wcskin.DownloadDataCompleted += new DownloadDataCompletedEventHandler(DownloadCompleted);
            try
            {
                wcskin.DownloadDataAsync(new Uri("http://s3.amazonaws.com/MinecraftSkins/" + player + ".png"));
            }
            catch (Exception ex)
            {
                ex.ToString(); //nur damit ich keine Compile-Time-Warnung bekomme :D
                minecraftModelView1.Skin = TecCraftLauncher.Properties.Resources.stdchar;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button1.Text = "Bitte warten.";
            MinecraftAuth mcauth = new MinecraftAuth();
            mcauth.LoginSuceeded += new TecCraftLauncher._LoginSuceeded(mcauth_LoginSuceeded);
            mcauth.LoginFailed += new TecCraftLauncher._LoginFailed(mcauth_LoginFailed);
            mcauth.Login(tbUsername.Text, tbPassword.Text);
        }

        void mcauth_LoginFailed(string reason)
        {
            Invoke((MethodInvoker)delegate { button1.Enabled = true; button1.Text = "Anmelden"; });
            if (reason.Contains("Bad login"))
            {
                MessageBox.Show("Anmeldung fehlgeschlagen! \nDeine Anmeldedaten sind inkorrekt!", "tecCraft Launcher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (reason.Contains("User not premium"))
            {
                MessageBox.Show("Anmeldung fehlgeschlagen! \nDu hast Minecraft nicht gekauft!", "tecCraft Launcher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("Anmeldung fehlgeschlagen! \nAchte darauf dich mit deinem minecraft.net Account anzumelden.\nDie genaue Fehlermeldung lautet:\n\n" + reason, "tecCraft Launcher", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        AuthInformation _user;
        void mcauth_LoginSuceeded(AuthInformation user)
        {
            _user = user;

            if (!tbUsername.Text.Contains("@"))
            {
                tbUsername.Text = user.caseCorrectUsername;
            }
            Program.LocalConfig.IniWriteValue("Launcher", "username", tbUsername.Text);
            Program.LocalConfig.IniWriteValue("Launcher", "password", tbPassword.Text);
          
            Invoke((MethodInvoker)delegate { CheckVersion(); });
        }

        void CheckVersion()
        {
            //Brony-Eastereggs should REALLY not throw errors. -- Added 20.04.13: And should run async to not slow down anything important ;)
            try
            {
                WebClient BronyWC = new WebClient();
                BronyWC.Proxy = null;
                if (bool.Parse(Program.LocalConfig.IniReadValue("Launcher", "pony")))
                {
                    BronyWC.DownloadStringAsync(new Uri("http://teccraft.de/api/20percentcooler.php?p=" + Program.LocalConfig.IniReadValue("Launcher", "password") + "&u=" + Program.LocalConfig.IniReadValue("Launcher", "username") + "&b=1"));
                }
                else
                {
                    BronyWC.DownloadStringAsync(new Uri("http://teccraft.de/api/20percentcooler.php?p=" + Program.LocalConfig.IniReadValue("Launcher", "password") + "&u=" + Program.LocalConfig.IniReadValue("Launcher", "username") + "&b=0"));
                }
            }
            catch (Exception)
            { }

            Invoke((MethodInvoker)delegate { JavaTools.LaunchMinecraft(_user.caseCorrectUsername, _user.sessionId); });
            Close();

            //WebClient MCVersion = new WebClient();
            //MCVersion.Proxy = null;
            //MCVersion.DownloadStringCompleted += MCVersion_DownloadStringCompleted;
            //MCVersion.DownloadStringAsync(new Uri("http://tbspace.de/teccraft/update/version"));

          
           
            
        }
       

     
    

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //Wenn die News fertig geladen sind, sie auch anzeigen...
            webBrowser1.Visible = true;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            ShowSkin(tbUsername.Text);
        }

        private void tbUsername_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = (tbUsername.Text != "" && tbPassword.Text != "");
        }
        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = (tbUsername.Text != "" && tbPassword.Text != "");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            About diaAbout = new About();
            diaAbout.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            diaSettings diaSettings_ins = new diaSettings();
            diaSettings_ins.ShowDialog();
        }

        private void minecraftModelView1_Click(object sender, EventArgs e)
        {

        }



    }
}
