using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace TecCraftLauncher
{
    partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            label2.Text = "V." + Application.ProductVersion;
        }

        private void About_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            panel1.Visible = !panel1.Visible;
        }
    }
}
