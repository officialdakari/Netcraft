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

        Form1 game;

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            game.NoClip = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked)
            {
                game.moveInterval = 1;
            } else
            {
                game.moveInterval = 10;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            game.IsBlink = checkBox3.Checked;
        }

        private void CheatWindow_Load(object sender, EventArgs e)
        {
            game = Form1.GetInstance();
            textBox1.Paint += TextBox1_Paint;
            game.AddKeybind(Keys.N, () =>
            {
                this.Show();
            });
            game.AddKeybind(Keys.Z, () =>
            {
                Point point = game.Normalize(game.ToClientPoint(Cursor.Position));
                numericUpDown1.Value = point.X;
                numericUpDown2.Value = point.Y;
            });
            game.AddKeybind(Keys.X, () =>
            {
                Point point = game.Normalize(game.ToClientPoint(Cursor.Position));
                numericUpDown4.Value = point.X;
                numericUpDown3.Value = point.Y;
            });
            game.MouseClick += CheatWindow_MouseClick;
            game.Move += Game_Move;
            game.BlockPreBreakEvent += Game_BlockPreBreakEvent;
            game.BlockBrokenEvent += Game_BlockBrokenEvent;
            game.BlockPrePlaceEvent += Game_BlockPrePlaceEvent;
            game.BlockPlacedEvent += Game_BlockPlacedEvent;
        }

        private async void Game_BlockPlacedEvent(Point point)
        {
            if (!checkBox5.Checked) return;
            await game.UpdatePlayerPosition();
        }

        private async void Game_BlockPrePlaceEvent(Point point)
        {
            if (!checkBox5.Checked) return;
            await game.Send("entityplayermove?" + point.X.ToString() + "?" + point.Y.ToString());
        }

        private async void Game_BlockBrokenEvent(Point point)
        {
            if (!checkBox5.Checked) return;
            await game.UpdatePlayerPosition();
        }

        private async void Game_BlockPreBreakEvent(Point point)
        {
            if (!checkBox5.Checked) return;
            await game.Send("entityplayermove?" + point.X.ToString() + "?" + point.Y.ToString());
        }

        private void Game_Move(object sender, EventArgs e)
        {
        }

        private void CheatWindow_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (checkBox4.Checked)
                {
                    game._localPlayer.Location = e.Location;
                }
            }
        }

        private void TextBox1_Paint(object sender, PaintEventArgs e)
        {
            textBox1.CreateGraphics().DrawString("Enter Text Here", textBox1.Font, new SolidBrush(Color.LightGray), Point.Empty);
        }

        private void CheatWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        readonly Random rnd = new Random();

        private async void button1_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                for (int i = 0; i < trackBar1.Value; i++)
                {
                    await Task.Delay(trackBar2.Value);
                    await game.Send("chat?" + textBox1.Text.Replace("%R%", rnd.Next(1000, 9999).ToString()));
                }
            });
        }
        const int FASTBUILD_DELAY = 30;
        private async void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            progressBar1.Maximum = (int)((numericUpDown4.Value - numericUpDown1.Value) * (numericUpDown3.Value - numericUpDown2.Value));
            progressBar1.Visible = true;
            progressBar1.Value = 0;
            await Task.Run(async () =>
            {
                for (decimal x = numericUpDown1.Value; x < numericUpDown4.Value + 1; x++)
                {
                    for (decimal y = numericUpDown2.Value; y < numericUpDown3.Value + 1; y++)
                    {
                        await Task.Delay(FASTBUILD_DELAY);
                        if (progressBar1.Value < progressBar1.Maximum) progressBar1.Value++;

                        game.WriteChat($"Placing at [{x.ToString()}; {y.ToString()}]");
                        Game_BlockPrePlaceEvent(new Point((int)x, (int)y));
                        await game.Send($"block_place?{(((int)x) * 32).ToString()}?{(((int)y) * 32).ToString()}");
                        Game_BlockPlacedEvent(new Point((int)x, (int)y));
                    }
                }
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                progressBar1.Visible = false;
            });
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            progressBar1.Maximum = (int)((numericUpDown4.Value - numericUpDown1.Value) * (numericUpDown3.Value - numericUpDown2.Value));
            progressBar1.Visible = true;
            progressBar1.Value = 0;
            await Task.Run(async () =>
            {
                for (decimal x = numericUpDown1.Value; x < numericUpDown4.Value + 1; x++)
                {
                    for (decimal y = numericUpDown2.Value; y < numericUpDown3.Value + 1; y++)
                    {
                        await Task.Delay(FASTBUILD_DELAY);
                        if (progressBar1.Value < progressBar1.Maximum) progressBar1.Value++;

                        Game_BlockPrePlaceEvent(new Point((int)x, (int)y));
                        await game.Send($"block_place_bg?{(((int)x) * 32).ToString()}?{(((int)y) * 32).ToString()}");
                        Game_BlockPlacedEvent(new Point((int)x, (int)y));
                    }
                }
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                progressBar1.Visible = false;
            });
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            progressBar1.Visible = true;
            progressBar1.Maximum = (int)((numericUpDown4.Value - numericUpDown1.Value) * (numericUpDown3.Value - numericUpDown2.Value));
            progressBar1.Value = 0;
            await Task.Run(async () =>
            {
                for (decimal x = numericUpDown1.Value; x < numericUpDown4.Value + 1; x++)
                {
                    for (decimal y = numericUpDown2.Value; y < numericUpDown3.Value + 1; y++)
                    {

                        await Task.Delay(FASTBUILD_DELAY);
                        if (progressBar1.Value < progressBar1.Maximum) progressBar1.Value++;

                        Game_BlockPrePlaceEvent(new Point((int)x, (int)y));
                        await game.Send($"block_break?{x.ToString()}?{y.ToString()}");
                        Game_BlockPlacedEvent(new Point((int)x, (int)y));
                    }
                }
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                progressBar1.Visible = false;
            });
        }
        Panel p = null;
        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
        }

        private async void button5_Click_1(object sender, EventArgs e)
        {
            await game.AddItem("STONE x 255");
        }

        private void button5_Click_2(object sender, EventArgs e)
        {
            Close();
        }

        private async void button5_Click_3(object sender, EventArgs e)
        {
            for(int i = 0; i < 10000; i++)
            {
                await game.UpdatePlayerPosition();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label2.Text = "Count " + trackBar1.Value.ToString();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label3.Text = "Interval " + trackBar2.Value.ToString();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
        }


        private void checkBox6_CheckedChanged_1(object sender, EventArgs e)
        {
            game._localPlayer.BorderStyle = checkBox6.Checked ? BorderStyle.FixedSingle : BorderStyle.None;
        }

        private async void button5_Click_4(object sender, EventArgs e)
        {
            string target = Utils.InputBox("Select target");
            if (target == null) return;
            for(int i = 0; i < 15; i++)
            {
                await game.Send("pvp?" + target);
            }
        }
    }
}
