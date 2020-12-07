using System;
using System.Collections.Generic;
using System.Linq;

namespace NCore
{
    public class WorldServer
    {
        public List<Block> Blocks { get; set; } = new List<Block>();
        public List<BlockChest> Chests { get; set; } = new List<BlockChest>();
        public string UUID { get; set; } = Guid.NewGuid().ToString();

        public Block GetBlockAt(int x, int y)
        {
            return Blocks.Where(b => b.Position.X == x && b.Position.Y == y).ToArray()[0];
        }

        public Block GetBlockAt(System.Drawing.Point point)
        {
            return Blocks.Where(b => b.Position == point).ToArray()[0];
        }

        public BlockChest GetChestAt(System.Drawing.Point point)
        {
            foreach(BlockChest c in Chests)
            {
                if (c.Position == point) return c;
            }
            return null;
        }

        public WorldServer()
        {
        }
    }
}