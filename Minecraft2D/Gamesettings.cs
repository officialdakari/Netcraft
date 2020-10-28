using System;
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
            {
                var withBlock = My.MyProject.Forms.Form1;
                withBlock.ButtonJump.Visible = CheckBox1.Checked;
                withBlock.ButtonLeft.Visible = CheckBox1.Checked;
                withBlock.ButtonRight.Visible = CheckBox1.Checked;
                withBlock.ButtonAttack.Visible = CheckBox1.Checked;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            GroupBox1.Visible = !GroupBox1.Visible;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
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
                return;
            CenterToParent();
            My.MyProject.Forms.Form1.Ticker.Stop();
        }

        private void Gamesettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
            if (My.MyProject.Forms.Form1.IsOfficialServer)
                ProcessSuspend.ResumeProcess(My.MyProject.Forms.Form1.ServerProcess);
            My.MyProject.Forms.Form1.Ticker.Start();
        }
        Lang lang;

        private void Gamesettings_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            lang = Lang.FromFile($"./lang/{Utils.LANGUAGE}.txt");
            BackButton.Text = lang.get("settings.button.back");
            SettingsButton.Text = lang.get("settings.button.settings");
            ExitButton.Text = lang.get("settings.button.exit");
            Label1.Text = lang.get("settings.text.paused");
        }
    }
}