using System;
using global::System.Drawing;
using Microsoft.VisualBasic.CompilerServices;

namespace NCore
{
    public class SaveLoad
    {
        public string Save(WorldServer arg0)
        {
            string world = "";
            foreach (var block in arg0.Blocks)
                world += $"{block.Type.ToString()}?{block.Position.X.ToString()}?{block.Position.Y.ToString()}?{block.Unbreakable}?{block.IsBackground};";

            world = world.TrimEnd(';') + "\n";
            foreach (var chest in arg0.Chests)
            {
                string items = "";
                foreach(ItemStack i in chest.items)
                {
                    items += $"{i.Type.ToString()}${i.Count.ToString()}!";
                }
                items = items.TrimEnd('!');
                world += $"{chest.Type.ToString()}?{chest.Position.X.ToString()}?{chest.Position.Y.ToString()}?{chest.Unbreakable}?{chest.IsBackground}?{items};";
            }

            return world.TrimEnd(';');
        }

        public WorldServer Load(string arg0)
        {
            var world = new WorldServer();
            string[] lines = arg0.Split('\n');
            foreach (var block in lines[0].Split(";"))
            {
                if (block.Length < 1) continue;
                var blockdata = block.Split("?");
                try
                {
                    Block b = new Block(new Point(int.Parse(blockdata[1]), int.Parse(blockdata[2])), (EnumBlockType)Enum.Parse(typeof(EnumBlockType), blockdata[0]), Conversions.ToBoolean(blockdata[3]), Conversions.ToBoolean(blockdata[4]));

                    world.Blocks.Add(b);
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine(block);
                    NCore.GetNCore().Log("Fatal error while loading the world:\r\n" + e.ToString(), "ERROR");
                    Environment.Exit(-1);
                }
            }

            foreach(var chest in lines[1].Split(';'))
            {
                if (chest.Length < 1) continue;
                var blockdata = chest.Split("?");
                string[] items = blockdata[5].Split('!');
                BlockChest b = new BlockChest(new Point(Conversions.ToInteger(blockdata[1]), Conversions.ToInteger(blockdata[2])), Conversions.ToBoolean(blockdata[3]));
                foreach (string i in items)
                {
                    try
                    { 
                        ItemStack item = new ItemStack((Material)Enum.Parse(typeof(Material), i.Split('$')[0]), int.Parse(i.Split('$')[1]));
                        b.items.Add(item);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        Console.WriteLine(i);
                    }
                }
                world.Chests.Add(b);
            }

            return world;
        }
    }
}