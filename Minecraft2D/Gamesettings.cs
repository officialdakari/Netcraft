using System;
using System.Windows.Forms;

namespace Minecraft2D
{
    public partial class Gamesettings
    {
        public Gamesettings()
        {
            InitializeComponent();
            _Button1.Name = "Button1";
            _CheckBox1.Name = "CheckBox1";
            _Button2.Name = "Button2";
            _Button3.Name = "Button3";
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
            ProcessSuspend.ResumeProcess(My.MyProject.Forms.Form1.ServerProcess);
            My.MyProject.Forms.Form1.ServerProcess.CloseMainWindow();
            Environment.Exit(0);
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

        private void Gamesettings_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }
    }
}