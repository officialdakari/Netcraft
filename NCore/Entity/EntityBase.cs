using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using NCore;
using NCore.netcraft.server.api;

namespace NCore.Entity
{
    /// <summary>
    /// Базовый класс Entity.
    /// </summary>
    public class EntityBase
    {
        public System.Drawing.Point Position { get; set; }
        public WorldServer World { get; set; }
        public System.Drawing.Size Size { get; set; }
        public System.Drawing.Rectangle Rectangle { get; set; }
        public string Name { get; set; }

        public EntityBase(string name, System.Drawing.Point point, WorldServer worldServer, System.Drawing.Size sz)
        {
            Position = point;
            World = worldServer;
            Size = sz;
            Rectangle = new System.Drawing.Rectangle(point, sz);
            Name = name;
        }

        public void Move(System.Drawing.Point to)
        {
            bool collision = false;
            foreach(Block b in World.Blocks)
            {
                if (b.IsBackground) continue;
                if (b.Type == EnumBlockType.SAPLING || b.Type == EnumBlockType.WATER) continue;
                if (b.Rectangle.IntersectsWith(Rectangle))
                {
                    collision = true;
                    break;
                }
            }
            if(collision)
            {
                NCore.Log($"{Name} ({this.GetType().ToString()}) moved wrongly!");
                return;
            }
            Position = to;
        }
    }
}
