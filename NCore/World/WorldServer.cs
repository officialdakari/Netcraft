using System;
using System.Collections.Generic;

namespace NCore
{
    public class WorldServer
    {
        public List<Block> Blocks { get; set; } = new List<Block>();
        public List<BlockChest> Chests { get; set; } = new List<BlockChest>();
        public string UUID { get; set; } = Guid.NewGuid().ToString();

        public Block GetBlockAt(int x, int y)
        {
            foreach (var block in Blocks)
            {
                if (block.Position.X == x)
                {
                    if (block.Position.Y == y)
                    {
                        return block;
                    }
                }
            }

            return null;
        }

        public Block GetBlockAt(System.Drawing.Point point)
        {
            foreach (var block in Blocks)
            {
                if (block.Position.X == point.X)
                {
                    if (block.Position.Y == point.Y)
                    {
                        return block;
                    }
                }
            }

            return null;
        }

        public BlockChest GetChestAt(System.Drawing.Point point)
        {
            foreach (var block in Chests)
            {
                if (block.Position.X == point.X)
                {
                    if (block.Position.Y == point.Y)
                    {
                        return block;
                    }
                }
            }

            return null;
        }

        public WorldServer()
        {
        }
    }
}