using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minecraft2D
{
    public partial class WinFormsConsole : UserControl
    {
        public WinFormsConsole()
        {
            InitializeComponent();
            _TextBox1.Name = "TextBox1";
        }

        private string lastLine = "";

        public string PromptString { get; set; } = ">";
        /// <summary>
        /// Происходит при отправке текста
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public event ProcessCommandEventHandler ProcessCommand;

        public delegate void ProcessCommandEventHandler(object sender, ConsoleBoxEventArgs e);

        public void WriteLine(string a)
        {
            lastLine = (string)TextBox1.Lines.Last().Substring(PromptString.Length).Clone();
            string[] g = (string[])this.TextBox1.Lines;
            
            g[g.Length - 1] = "";
            this.TextBox1.Text = string.Join("\r\n", g);
            this.TextBox1.AppendText(a + "\r\n");
            this.TextBox1.Select(this.TextBox1.TextLength, 0);
            this.TextBox1.ScrollToCaret();
            this.TextBox1.AppendText(PromptString + lastLine);
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            // Dim g As String() = TextBox1.Text.Split(vbCrLf).Clone
            // Dim l = g(g.Length - 1)
            // If l = "" Then
            // g(g.Length - 1) = PromptString
            // End If
        }

        private string getLeft(string a, int b)
        {
            return a.Substring(Math.Min(a.Length, b));
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            this.TextBox1.Select(this.TextBox1.TextLength, 0);
            this.TextBox1.ScrollToCaret();
            if (TextBox1.Lines.Last().Length == PromptString.Length)
            {
                if (e.KeyCode == Keys.Back)
                {
                    e.SuppressKeyPress = true;
                }
                else if (e.KeyCode == Keys.Delete)
                {
                    e.SuppressKeyPress = true;
                }

                if (e.KeyCode == Keys.A)
                {
                    if (e.Control)
                    {
                        e.SuppressKeyPress = true;
                    }
                }
            }

            if (e.KeyCode == Keys.Enter)
            {
                lastLine = (string)(TextBox1.Lines.Last().Substring(PromptString.Length).Clone());
                var ev = new ConsoleBoxEventArgs();
                ev.Message = lastLine;
                ProcessCommand?.Invoke(this, ev);
                e.SuppressKeyPress = true;
                this.TextBox1.AppendText("\r\n" + PromptString);
                this.TextBox1.Select(this.TextBox1.TextLength, 0);
                this.TextBox1.ScrollToCaret();
            }

            lastLine = (string)(TextBox1.Lines.Last().Substring(PromptString.Length).Clone());
        }

        public void reload()
        {
            var a = this.TextBox1.Lines;
            a[a.Length - 1] = PromptString;
            this.TextBox1.Text = string.Join("\r\n", a);
            this.TextBox1.Font = this.Font;
            this.TextBox1.ForeColor = Color.FromArgb(255, 255, 255);
            // ConsoleBox_Load(Nothing, Nothing)
        }

        private void ConsoleBox_Load(object sender, EventArgs e)
        {
            this.TextBox1.Text = PromptString;
            this.TextBox1.Font = this.Font;
            this.TextBox1.ForeColor = Color.FromArgb(255, 255, 255);
        }
    }

    public class ConsoleBoxEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
