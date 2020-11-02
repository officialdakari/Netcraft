using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                button1.PerformClick();
            }
        }

        Process proc;
        private void button1_Click(object sender, EventArgs e)
        {
            proc.StandardInput.WriteLine(textBox1.Text);
            textBox1.Clear();
        }

        private void proc_DataReceived(object sender, DataReceivedEventArgs e)
        {
            richTextBox1.AppendText(e.Data + "\r\n");
            richTextBox1.Select(richTextBox1.TextLength, 0);
            richTextBox1.ScrollToCaret();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            proc = new Process();
            proc.StartInfo.FileName = "./Netcraft.exe";
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();

            proc.BeginErrorReadLine();
            proc.BeginOutputReadLine();

            proc.EnableRaisingEvents = true;
            proc.ErrorDataReceived += proc_DataReceived;
            proc.OutputDataReceived += proc_DataReceived;
        }
    }
}
