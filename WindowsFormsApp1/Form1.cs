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
        
        private void start()
        {
            var BM = new Bitmap(Width, Height);
            this.DrawToBitmap(BM, ClientRectangle);
            this.BackgroundImage = BM;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            foreach (Control C in this.Controls)
                C.Visible = false;
            FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Red;
            this.TransparencyKey = Color.Red;
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //start();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if (this.Height < 40)
            {
                Environment.Exit(0);
            }

            Width -= 4;
            Left += 2;
            Height -= 4;
            Top += 2;
        }
    }
}
