using global::System.Drawing;

namespace NCore
{
    public class TreeGenerator
    {
        public static void GenerateTree(Point pos, WorldServer w)
        {
            for (int i = 0; i <= 6; i++)
            {
                if (i == 0)
                {
                    w.Blocks.Add(new Block(pos, EnumBlockType.WOOD, false, false));
                }

                if (i == 1)
                {
                    var pos1 = pos;
                    pos1.Offset(0, -1);
                    w.Blocks.Add(new Block(pos1, EnumBlockType.WOOD, false, false));
                }

                if (i == 2)
                {
                    var pos1 = pos;
                    pos1.Offset(0, -2);
                    w.Blocks.Add(new Block(pos1, EnumBlockType.WOOD, false, false));
                }

                if (i == 3)
                {
                    var pos1 = pos;
                    pos1.Offset(-1, -3);
                    w.Blocks.Add(new Block(pos1, EnumBlockType.LEAVES, false, false));
                }

                if (i == 4)
                {
                    var pos1 = pos;
                    pos1.Offset(0, -3);
                    w.Blocks.Add(new Block(pos1, EnumBlockType.LEAVES, false, false));
                }

                if (i == 5)
                {
                    var pos1 = pos;
                    pos1.Offset(1, -3);
                    w.Blocks.Add(new Block(pos1, EnumBlockType.LEAVES, false, false));
                }

                if (i == 6)
                {
                    var pos1 = pos;
                    pos1.Offset(0, -4);
                    w.Blocks.Add(new Block(pos1, EnumBlockType.LEAVES, false, false));
                }
            }
        }

        public static void GrowthTree(Point pos, WorldServer w)
        {
            for (int i = 0; i <= 6; i++)
            {
                if (i == 0)
                {
                    if (w.GetBlockAt(pos) != null) continue;
                    w.Blocks.Add(new Block(pos, EnumBlockType.WOOD, false, false));
                    foreach (var p in NCore.clientList)
                        p.SendBlockChange(pos, EnumBlockType.WOOD, false, true, false);
                }

                if (i == 1)
                {
                    var pos1 = pos;
                    pos1.Offset(0, -1);
                    if (w.GetBlockAt(pos1) != null) continue;
                    w.Blocks.Add(new Block(pos1, EnumBlockType.WOOD, false, false));
                    foreach (var p in NCore.clientList)
                        p.SendBlockChange(pos1, EnumBlockType.WOOD, false, true, false);
                }

                if (i == 2)
                {
                    var pos1 = pos;
                    pos1.Offset(0, -2);
                    if (w.GetBlockAt(pos1) != null) continue;
                    w.Blocks.Add(new Block(pos1, EnumBlockType.WOOD, false, false));
                    foreach (var p in NCore.clientList)
                        p.SendBlockChange(pos1, EnumBlockType.WOOD, false, true, false);
                }

                if (i == 3)
                {
                    var pos1 = pos;
                    pos1.Offset(-1, -3);
                    if (w.GetBlockAt(pos1) != null) continue;
                    w.Blocks.Add(new Block(pos1, EnumBlockType.LEAVES, false, false));
                    foreach (var p in NCore.clientList)
                        p.SendBlockChange(pos1, EnumBlockType.LEAVES, false, true, false);
                }

                if (i == 4)
                {
                    var pos1 = pos;
                    pos1.Offset(0, -3);
                    if (w.GetBlockAt(pos1) != null) continue;
                    w.Blocks.Add(new Block(pos1, EnumBlockType.LEAVES, false, false));
                    foreach (var p in NCore.clientList)
                        p.SendBlockChange(pos1, EnumBlockType.LEAVES, false, true, false);
                }

                if (i == 5)
                {
                    var pos1 = pos;
                    pos1.Offset(1, -3);
                    if (w.GetBlockAt(pos1) != null) continue;
                    w.Blocks.Add(new Block(pos1, EnumBlockType.LEAVES, false, false));
                    foreach (var p in NCore.clientList)
                        p.SendBlockChange(pos1, EnumBlockType.LEAVES, false, true, false);
                }

                if (i == 6)
                {
                    var pos1 = pos;
                    pos1.Offset(0, -4);
                    if (w.GetBlockAt(pos1) != null) continue;
                    w.Blocks.Add(new Block(pos1, EnumBlockType.LEAVES, false, false));
                    foreach (var p in NCore.clientList)
                    {
                        p.SendBlockChange(pos1, EnumBlockType.LEAVES, false, true, false);
                        p.PacketQueue.SendQueue();
                    }
                }
            }
        }
    }
}
//  LLL 
// LLLLL
//   W
//   W
//   W