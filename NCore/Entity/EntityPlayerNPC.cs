using global::System.Drawing;

namespace NCore
{
    public class EntityPlayerNPC
    {

        // REM - Сделать это сегодня!

        public Point Position { get; set; }

        private int direction = 2;

        public void NPCTick()
        {
            if (Position.X == 0)
            {
                direction = 2;
            }
            else if (Position.X == 100)
            {
                direction = -2;
            }

            Position = new Point(Position.X + direction, 0);
            UpdateNPCPosition();
        }

        public void UpdateNPCPosition()
        {
            NCore.Send("updateplayerposition?NPC1?" + Position.X.ToString() + "?" + Position.Y.ToString());
        }

        public EntityPlayerNPC()
        {
            Position = new Point(0, 0);
        }
    }
}