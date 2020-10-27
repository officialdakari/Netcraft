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
            return world.TrimEnd(';');
        }

        public WorldServer Load(string arg0)
        {
            var world = new WorldServer();
            foreach (var block in arg0.Split(";"))
            {
                var blockdata = block.Split("?");
                world.Blocks.Add(new Block(new Point(Conversions.ToInteger(blockdata[1]), Conversions.ToInteger(blockdata[2])), (EnumBlockType)Enum.Parse(typeof(EnumBlockType), blockdata[0]), Conversions.ToBoolean(blockdata[3]), Conversions.ToBoolean(blockdata[4])));
            }

            return world;
        }
    }
}