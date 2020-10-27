using System.Drawing;
using System.Windows.Forms;

namespace Minecraft2D
{
    public class OpaqueRichTextBox : RichTextBox
    {
        private static readonly Color DefaultBackground = Color.Transparent;
        private Image TransparentImage;

        public OpaqueRichTextBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = DefaultBackground;
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }

            set
            {
                base.BackColor = value;
            }
        }

        public Image Image
        {
            get
            {
                return TransparentImage;
            }

            set
            {
                TransparentImage = value;
                Invalidate();
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | 0x20;
                return cp;
            }
        }

        // Infrastructure to cause the default background to be transparent
        public bool ShouldSerializeBackColor()
        {
            return BackColor == DefaultBackground;
        }

        // Infrastructure to cause the default background to be transparent
        public void ResetBackground()
        {
            BackColor = DefaultBackground;
        }
    }
}