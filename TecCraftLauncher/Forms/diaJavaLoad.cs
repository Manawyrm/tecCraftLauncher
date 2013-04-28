using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using System.IO;
namespace TecCraftLauncher
{
    public partial class diaJavaLoad : Form
    {
        String URL;
        Int32 Arch;
        bool Done = false;
        WebClient wc = new WebClient();
        public diaJavaLoad(String _URL, Int32 _Arch)
        {
            URL = _URL;
            Arch = _Arch;
            wc.Proxy = null;
            InitializeComponent();
        }
        
        private void diaJavaLoad_Load(object sender, EventArgs e)
        {
            label1.Text = "Java für Windows - " + Arch.ToString() + " bit" ;

            Application.DoEvents();
            
          
        }

        private void diaJavaLoad_Shown(object sender, EventArgs e)
        {
            wc.DownloadProgressChanged += wc_DownloadProgressChanged;
            wc.DownloadFileCompleted += wc_DownloadFileCompleted;
            try
            {
                System.IO.Directory.Delete("java", true);
            }
            catch (Exception)
            { }
            try
            {
                System.IO.Directory.CreateDirectory("java");
            }
            catch (Exception)
            { } 
            wc.DownloadFileAsync(new Uri(URL), "java/java.tar.gz");
            //wc_DownloadFileCompleted(null, null);
        }

        void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate { DownloadJavaCompleted(); });
        }
        void DownloadJavaCompleted()
        {
            Stream inStream = File.OpenRead("java/java.tar.gz");
            Stream gzipStream = new GZipInputStream(inStream);

            TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
            tarArchive.ProgressMessageEvent += tarArchive_ProgressMessageEvent;
            tarArchive.ExtractContents("java");

            tarArchive.Close();
            gzipStream.Close();
            inStream.Close();

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Done = true;
            System.IO.Directory.Move("java/" + jpath, jpath);
            System.IO.Directory.Delete("java", true);
            System.IO.Directory.Move(jpath, "java");
            Close();
        }
        bool set = false;
        String jpath = "";
        void tarArchive_ProgressMessageEvent(TarArchive archive, TarEntry entry, string message)
        {
            if (set == false)
            {
                jpath = entry.Name;
                set = true;
            }
            label2.Text = "Entpacke " + entry.Name;
            Application.DoEvents();
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label2.Text = "Lädt... - " + (int)e.BytesReceived / 1024 + " KiB von " + (int)e.TotalBytesToReceive / 1024 + " KiB";

        }

        private void diaJavaLoad_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Done == false)
            {
                if (MessageBox.Show("Java wurde noch nicht vollständig heruntergeladen. Ohne Java kann nicht fortgefahren werden.\nWirklich abbrechen?", "TecCraft Launcher" , MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    Done = true;
                    System.Environment.Exit(0);
                }
                e.Cancel = true;
            }
        }

      
    }
}
