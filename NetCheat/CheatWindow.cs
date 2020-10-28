using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Minecraft2D;

namespace NetCheat
{
    public partial class CheatWindow : Form
    {
        public CheatWindow()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Form1.GetInstance().NoClip = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked)
            {
                Form1.GetInstance().moveInterval = 1;
            } else
            {
                Form1.GetInstance().moveInterval = 30;
            }
        }
    }
}
