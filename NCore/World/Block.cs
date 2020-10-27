using global::System.Drawing;

namespace NCore
{
    public class Block
    {
        public Point Position { get; set; }
        public EnumBlockType Type { get; set; }
        public bool Unbreakable { get; set; }
        public bool IsBackground { get; set; }
        public Rectangle Rectangle { get; set; }

        public Block(Point arg0, EnumBlockType arg1, bool arg2, bool arg3)
        {
            Position = arg0;
            Type = arg1;
            Unbreakable = arg2;
            IsBackground = arg3;
            Rectangle = new Rectangle(Position, new Size(32, 32)); 
        }
    }
}