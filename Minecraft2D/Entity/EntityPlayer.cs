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
        public TransparentPicBox HandItem { get; set; }
        public int LastWalk { get; set; } = 0;

        private TransparentPicBox _Render;

        public TransparentPicBox Render
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Render;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            internal set
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

        public EntityPlayer(string arg0, string arg1, Point arg2, TransparentPicBox arg3)
        {
            Name = arg0;
            UUID = arg1;
            Location = arg2;
            Render = arg3;
            HandItem = new TransparentPicBox()
            {
                Name = "Testrender",
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            My.MyProject.Forms.Form1.Controls.Add(HandItem);
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
                    HandItem.Hide();
                    return;
                }

                if (Information.IsNothing(ItemImage))
                {
                    HandItem.Hide();
                    return;
                }

                HandItem.Show();
                var lc = Render.Location;
                if (ItemImage.Equals(null))
                    return;
                if (LastWalk == 1)
                {
                    lc.X += Render.Width - 5;
                    HandItem.Image = ItemImage;
                }
                else
                {
                    lc.X -= HandItem.Width - 5;
                    HandItem.Image = ItemImageFlipped;
                }

                lc.Y = (int)(lc.Y + (45d - HandItem.Height / 2d));
                HandItem.Size = new Size(24, 24);
                HandItem.SizeMode = PictureBoxSizeMode.StretchImage;
                HandItem.BringToFront();
                HandItem.Location = lc;
            }
            catch (Exception)
            {
                HandItem.Hide();
            }
        }

        public async Task SetItemInHand(Image i, Image iflipped, string str)
        {
            if (!Information.IsNothing(i))
                HandItem.Image = i;
            ItemImage = i;
            ItemImageFlipped = iflipped;
            ItemInHand = str;
            Render.Update();
            HandItem.Update();
        }

        public void Remove()
        {
            if (My.MyProject.Forms.Form1.InvokeRequired)
            {
                My.MyProject.Forms.Form1.Invoke(new MethodInvoker(Remove));
            }
            else
            {
                My.MyProject.Forms.Form1.Controls.Remove(HandItem);
            }
        }
    }
}