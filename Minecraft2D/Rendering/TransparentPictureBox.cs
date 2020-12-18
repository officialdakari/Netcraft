using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minecraft2D
{
    public class TransparentPicBox : PictureBox
    {
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            try
            {
                if (Parent != null)
                {
                    int index = Parent.Controls.GetChildIndex(this);
                    for (int i = Parent.Controls.Count - 1, loopTo = index + 1; i >= loopTo; i -= 1)
                    {
                        var c = Parent.Controls[i];
                        //c.Bounds.IntersectsWith(Bounds) && 
                        if (c.Bounds.IntersectsWith(Bounds) && c.Visible == true)
                        {
                            var bmp = new Bitmap(c.Width, c.Height, e.Graphics);
                            c.DrawToBitmap(bmp, c.ClientRectangle);
                            e.Graphics.TranslateTransform(c.Left - Left, c.Top - Top);
                            e.Graphics.DrawImageUnscaled(bmp, Point.Empty);
                            e.Graphics.TranslateTransform(Left - c.Left, Top - c.Top);
                            bmp.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}