using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Minecraft2D
{
    public partial class Chat
    {
        public Chat()
        {
            InitializeComponent();
            _TextBox1.Name = "TextBox1";
            _Button1.Name = "Button1";
        }

        private void Chat_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
        Lang lang;
        private void Chat_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            lang = Lang.FromFile($"./lang/{Utils.LANGUAGE}.txt");
            Text = lang.get("chat.title");
            Button1.Text = lang.get("chat.button.send");
        }

        public delegate void xUpdateChat(string Addtext);

        public void UpdateChat(string AddText)
        {
            if (InvokeRequired)
            {
                Invoke(new xUpdateChat(UpdateChat), AddText);
            }
            else
            {
                RichTextBox1.AppendText(AddText + Constants.vbCrLf);
                RichTextBox1.Select(RichTextBox1.TextLength, 0);
                RichTextBox1.ScrollToCaret();
            }
        }

        public void SendChat(string a)
        {
            My.MyProject.Forms.Form1.Send("chat?" + a);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (TextBox1.Text == "IDDQDD")
            {
                My.MyProject.Forms.Form1.NoClip = !My.MyProject.Forms.Form1.NoClip;
                if (My.MyProject.Forms.Form1.NoClip)
                {
                    UpdateChat("Toggled no-clip on.");
                    My.MyProject.Forms.Form1.cTicker.Suspend();
                }
                else
                {
                    UpdateChat("Toggled no-clip off.");
                    My.MyProject.Forms.Form1.cTicker.Resume();
                }
            }
            else if (TextBox1.Text == "BUTTONCONTROLS")
            {
                My.MyProject.Forms.Form1.ButtonLeft.Visible = true;
                My.MyProject.Forms.Form1.ButtonRight.Visible = true;
                My.MyProject.Forms.Form1.ButtonJump.Visible = true;
            }
            else
            {
                SendChat(TextBox1.Text);
            }

            TextBox1.Clear();
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Button1.PerformClick();
            }
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(listBox1.SelectedItem != null)
            {
                string t = $"/tell {listBox1.SelectedItem.ToString()} {TextBox1.Text}";
                TextBox1.Text = t;
            }
        }

        private void sendAPrivateMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string t = $"/tell {listBox1.SelectedItem.ToString()} {TextBox1.Text}";
                TextBox1.Text = t;
            }
        }

        private void kickPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string t = $"/kick {listBox1.SelectedItem.ToString()} Kicked from server";
                TextBox1.Text = t;
                Button1.PerformClick();
            }
        }

        private void banPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string t = $"/ban {listBox1.SelectedItem.ToString()} Banned by admin";
                TextBox1.Text = t;
                Button1.PerformClick();
            }
        }

        private void Chat_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FancyMessage.Show(lang.get("info.help.chat"), "Help", FancyMessage.Icon.Info);
        }
    }
}