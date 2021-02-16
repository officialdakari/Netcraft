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
    public partial class SplashScreen1 : Form
    {
        public SplashScreen1()
        {
            InitializeComponent();
        }

        Bitmap[] bmps = new Bitmap[] { My.Resources.Resources.dirt1, My.Resources.Resources.grass_side, My.Resources.Resources.stone1, My.Resources.Resources.diamond_ore, My.Resources.Resources.diamond_block, My.Resources.Resources.obsidian1, My.Resources.Resources.bedrock };
        int i = -1;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            i++;
            pictureBox1.Image = bmps[i];
            if(i == bmps.Length - 1)
            {
                i = 0;
                //Close();
                //MainMenu.instance.Show();
                //timer1.Stop();
                //return;
            }
            timer1.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Close();
            MainMenu.instance.Show();
            timer1.Stop();
            My.MyProject.Computer.Audio.Stop();
            timer2.Stop();
        }

        private void SplashScreen1_Load(object sender, EventArgs e)
        {
            My.MyProject.Computer.Audio.Play("./resources/splash.wav", Microsoft.VisualBasic.AudioPlayMode.Background);
        }
    }
}
