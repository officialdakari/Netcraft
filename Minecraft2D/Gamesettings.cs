using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minecraft2D
{
    public partial class Gamesettings
    {
        public Gamesettings()
        {
            InitializeComponent();
            BackButton.Name = "Button1";
            _CheckBox1.Name = "CheckBox1";
            SettingsButton.Name = "Button2";
            ExitButton.Name = "Button3";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Close();
            My.MyProject.Forms.Form1.Update();
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            Form1.GetInstance().ToggleButtonControls();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            GroupBox1.Visible = !GroupBox1.Visible;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Form1.GetInstance().Close();
            My.MyProject.Forms.MainMenu.Show();
            Hide();
        }

        public new void Move()
        {
            CenterToParent();
        }

        private void Gamesettings_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible)
            {
                Form1.GetInstance().Show();
                return;
            }
            Form1.GetInstance().Hide();
        }

        private void Gamesettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
            Form1.GetInstance().Location = Location;
            if (My.MyProject.Forms.Form1.IsSingleplayer)
                ProcessSuspend.ResumeProcess(My.MyProject.Forms.Form1.ServerProcess);
            My.MyProject.Forms.Form1.Ticker.Start();
        }
        Lang lang;

        private void Gamesettings_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Location = Form1.GetInstance().Location;
            Size = Form1.GetInstance().Size;
            lang = Lang.FromFile($"./lang/{Utils.LANGUAGE}.txt");
            BackButton.Text = lang.get("settings.button.back");
            SettingsButton.Text = lang.get("settings.button.settings");
            ExitButton.Text = lang.get("settings.button.exit");
            Label1.Text = lang.get("settings.text.paused");
        }

        private void _CheckBox1_Click(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                Form1.GetInstance().audioPlay();
            } else
            {
                Form1.GetInstance().audioStop();
            }
        }

        private void BackButton_MouseEnter(object sender, EventArgs e)
        {
            ((Control)sender).BackgroundImage = My.Resources.Resources.buttonbghover;
            var c = (Control)sender;
            var rect = ClientRectangle;
            rect.Width = 32;
            rect.Height = 32;
            var clr = Color.DodgerBlue;
            int width = 1;

            var g = c.CreateGraphics();
            ControlPaint.DrawBorder(g, rect,
                 clr, width, ButtonBorderStyle.Solid,
                 clr, width, ButtonBorderStyle.Solid,
                 clr, width, ButtonBorderStyle.Solid,
                 clr, width, ButtonBorderStyle.Solid);
        }

        private void BackButton_MouseLeave(object sender, EventArgs e)
        {
            ((Control)sender).BackgroundImage = My.Resources.Resources.buttonbg;
            ((Control)sender).Invalidate();
        }

        private void _CheckBox1_MouseEnter(object sender, EventArgs e)
        {
            BackButton_MouseEnter(sender, e);
        }

        private void _CheckBox1_MouseLeave(object sender, EventArgs e)
        {
            BackButton_MouseLeave(sender, e);
        }
    }
}