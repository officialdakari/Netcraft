
namespace NCore
{
    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }
        public WorldServer World { get; set; }

        public object ToPoint()
        {
            return new System.Drawing.Point(X, Y);
        }

        public Location(int a, int b, WorldServer c)
        {
            X = a;
            Y = b;
            World = c;
        }
    }
}