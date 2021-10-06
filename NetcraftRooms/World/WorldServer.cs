using System;
using System.Collections.Generic;
using System.Linq;

namespace NCore
{
    public class WorldServer
    {
        private string NOTE = "WARNING! DO NOT MODIFY THIS JSON FILE !!";
        public List<Block> Blocks { get; set; } = new List<Block>();
        public string UUID { get; set; } = Guid.NewGuid().ToString();
        public World.Gamerules Gamerules { get; } = new World.Gamerules();

        public Block GetBlockAt(int x, int y)
        {
            try
            {
                return Blocks.Where(b => b.Position.X == x && b.Position.Y == y).ToArray()[0];

            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }

        public Block GetBlockAt(System.Drawing.Point point)
        {
            try
            {
                return Blocks.Where(b => b.Position == point).ToArray()[0];
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }

        public WorldServer()
        {
        }
    }
}