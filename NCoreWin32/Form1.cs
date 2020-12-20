using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NCoreWin32
{
    public partial class Form1 : Form
    {
        Process proc;
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                proc.StandardInput.WriteLine(textBox1.Text);
                textBox1.Clear();
                e.SuppressKeyPress = true;
            } 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            proc = new Process();
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = "dotnet";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.WorkingDirectory = Application.StartupPath;
            proc.StartInfo.Arguments = "NCore.dll";
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.StandardErrorEncoding = Encoding.Default;
            proc.StartInfo.StandardOutputEncoding = Encoding.Default;
            proc.Start();
            proc.BeginErrorReadLine();
            proc.BeginOutputReadLine();
            proc.ErrorDataReceived += Proc_ErrorDataReceived;
            proc.OutputDataReceived += Proc_OutputDataReceived;
        }

        private void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            richTextBox1.AppendText(e.Data + "\r\n");
            richTextBox1.Select(richTextBox1.TextLength, 0);
            richTextBox1.ScrollToCaret();
        }

        private void Proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show($"Выход из приложения приведёт к принудительному завершению следующего процесса:\r\n[{proc.Id}] {proc.ProcessName}", "NCore GUI", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                proc.Kill();
            } else
            {
                e.Cancel = true;
            }
        }
    }
}
