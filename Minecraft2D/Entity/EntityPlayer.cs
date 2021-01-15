using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Minecraft2D
{
    public class EntityPlayer
    {
        public string Name { get; set; }
        public string UUID { get; set; }
        public string ItemInHand { get; set; } = "";
        public Image ItemImage { get; set; }
        public Image ItemImageFlipped { get; set; }
        public Image Sprite { get; set; } = My.MyProject.Forms.Form1.playerSkin;
        public Point Location { get; set; }
        public TransparentPicBox R1 { get; set; }
        public int LastWalk { get; set; } = 0;

        private PictureBox _Render;

        private PictureBox Render
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Render;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Render != null)
                {
                    _Render.LocationChanged -= (_, __) => Test();
                }

                _Render = value;
                if (_Render != null)
                {
                    _Render.LocationChanged += (_, __) => Test();
                }
            }
        }

        public EntityPlayer(string arg0, string arg1, Point arg2, PictureBox arg3)
        {
            Name = arg0;
            UUID = arg1;
            Location = arg2;
            Render = arg3;
            R1 = new TransparentPicBox()
            {
                Name = "Testrender",
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            My.MyProject.Forms.Form1.Controls.Add(R1);
        }

        private void Test()
        {
            if (LastWalk == 1)
            {
                Render.Image = Sprite;
            }
            else
            {
                Render.Image = Form1.GetInstance().GetFlipped(Sprite);
            }

            try
            {
                if (Information.IsNothing(ItemImageFlipped))
                {
                    R1.Hide();
                    return;
                }

                if (Information.IsNothing(ItemImage))
                {
                    R1.Hide();
                    return;
                }

                R1.Show();
                var lc = Render.Location;
                if (ItemImage.Equals(null))
                    return;
                if (LastWalk == 1)
                {
                    lc.X += Render.Width - 5;
                    R1.Image = ItemImage;
                }
                else
                {
                    lc.X -= R1.Width - 5;
                    R1.Image = ItemImageFlipped;
                }

                lc.Y = (int)(lc.Y + (45d - R1.Height / 2d));
                R1.Size = new Size(24, 24);
                R1.SizeMode = PictureBoxSizeMode.StretchImage;
                R1.BringToFront();
                R1.Location = lc;
            }
            catch (Exception)
            {
                R1.Hide();
            }
        }

        public async Task SetItemInHand(Image i, Image iflipped, string str)
        {
            if (!Information.IsNothing(i))
                R1.Image = i;
            ItemImage = i;
            ItemImageFlipped = iflipped;
            ItemInHand = str;
            Render.Update();
            R1.Update();
        }

        public void Remove()
        {
            if (My.MyProject.Forms.Form1.InvokeRequired)
            {
                My.MyProject.Forms.Form1.Invoke(new MethodInvoker(Remove));
            }
            else
            {
                My.MyProject.Forms.Form1.Controls.Remove(R1);
            }
        }
    }
}