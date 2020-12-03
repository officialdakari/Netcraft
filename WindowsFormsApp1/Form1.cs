using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using global::Ionic.Zip;
using IWshRuntimeLibrary;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.DownloadFile("https://cdn.discordapp.com/attachments/764151189016805397/764152900514611292/NetCraft-Client-1.0-ALPHA-R09102020.zip", @".\Netcraft1.zip");
                ZipFile z = ZipFile.Read(@".\Netcraft1.zip");
                z.ExtractAll(@".\Netcraft1Installed");
                z.Dispose();
                z = null;
                System.IO.File.Delete(@".\Netcraft1.zip");
                CreateShortcutNetcraft();
                MessageBox.Show("Netcraft is successfully installed");
            } catch(Exception ex)
            {
                if(MessageBox.Show(ex.ToString(), "Не удалось установить", MessageBoxButtons.RetryCancel, MessageBoxIcon.Asterisk) == DialogResult.Retry)
                {
                    button1_Click(sender, e);
                }
            }
        }

        private void CreateShortcutNetcraft()
        {
            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\Netcraft.lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = "Netcraft";
            shortcut.TargetPath = Application.StartupPath + @"\Netcraft1Installed\Minecraft2D.exe";
            shortcut.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
