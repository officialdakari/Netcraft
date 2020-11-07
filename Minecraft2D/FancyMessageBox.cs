using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minecraft2D
{
    public partial class FancyMessageBox : Form
    {
        public FancyMessageBox()
        {
            InitializeComponent();
        }

        internal const string INFO_TEXT = "i";
        internal const string WARNING_TEXT = "ê";
        internal const string ERROR_TEXT = "r";

        internal const int OK_BUTTON_X = 449;
        internal const int OK_BUTTON_Y = 173;

        internal const int CANCEL_BUTTON_X = 360;
        internal const int CANCEL_BUTTON_Y = 173;
        Lang lang;

        private void FancyMessageBox_Load(object sender, EventArgs e)
        {
            lang = Lang.FromFile($"./lang/{Utils.LANGUAGE}.txt");
            button1.Text = lang.get("button.ok");
            button2.Text = lang.get("button.cancel");
            MouseMove += MyForm_MouseMove;
            
            label1.MouseMove += MyForm_MouseMove;
            
            label2.MouseMove += MyForm_MouseMove;

            label3.MouseMove += MyForm_MouseMove;

            iconLabel1.MouseMove += MyForm_MouseMove;
        }

        private void MyForm_MouseMove(object sender, MouseEventArgs e)
        {
            base.Capture = false;
            ((Control)sender).Capture = false;
            Message m = Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            this.WndProc(ref m);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
    public class FancyMessage
    {
        
        public enum Icon
        {
            Info = 1, Warning = 2, Error = 3
        }

        public enum Buttons
        {
            OK = 1, OKCancel = 2
        }

        public enum Result
        {
            OK = 1, Cancel = 2, None = 0
        }

        public static Result Show(string text, string caption = "", Icon icon = Icon.Info, Buttons buttons = Buttons.OK)
        {
            FancyMessageBox msg = new FancyMessageBox();
            msg.label3.Text = caption;
            msg.label1.Text = text;
            if(caption == "" || caption == "Netcraft")
            {
                msg.label1.Location = new Point(130, 48);
                msg.label3.Hide();
            }

            switch(icon)
            {
                case Icon.Info:
                    msg.iconLabel1.Text = FancyMessageBox.INFO_TEXT;
                    msg.iconLabel1.ForeColor = Color.DodgerBlue;
                    My.MyProject.Computer.Audio.PlaySystemSound(System.Media.SystemSounds.Question);
                    break;
                case Icon.Warning:
                    msg.iconLabel1.Hide();
                    msg.label2.Show();
                    msg.iconLabel1.ForeColor = Color.Gold;
                    My.MyProject.Computer.Audio.PlaySystemSound(System.Media.SystemSounds.Exclamation);
                    break;
                case Icon.Error:
                    msg.iconLabel1.Text = FancyMessageBox.ERROR_TEXT;
                    msg.iconLabel1.ForeColor = Color.Red;
                    My.MyProject.Computer.Audio.PlaySystemSound(System.Media.SystemSounds.Asterisk);
                    break;
            }
            if(buttons == Buttons.OKCancel)
            {
                msg.button2.Show();
                Point p1;
                Point p2;
                p1 = new Point(msg.button1.Left, msg.button1.Top);
                p2 = new Point(msg.button2.Left, msg.button2.Top);
                msg.button1.Location = p2;
                msg.button2.Location = p1;
                //msg.button2.Location = new Point(FancyMessageBox.OK_BUTTON_X, FancyMessageBox.OK_BUTTON_Y);
                //msg.button1.Location = new Point(FancyMessageBox.CANCEL_BUTTON_X, FancyMessageBox.CANCEL_BUTTON_Y);
            }
            DialogResult result = msg.ShowDialog();
            if (result == DialogResult.OK) return Result.OK;
            if (result == DialogResult.Cancel) return Result.Cancel;
            return Result.None;
        }
    }
}
