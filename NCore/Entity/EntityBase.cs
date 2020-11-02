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
        public System.Drawing.Point Position;
        public WorldServer World;
        public System.Drawing.Size Size;
        public System.Drawing.Rectangle Rectangle;
        public string Name { get; set; }

        public EntityBase(string name, System.Drawing.Point point, WorldServer worldServer, System.Drawing.Size sz)
        {
            Position = point;
            World = worldServer;
            Size = sz;
            Rectangle = new System.Drawing.Rectangle(point, sz);
            Name = name;
        }

        protected internal void handleGravity()
        {
            foreach (Block b in World.Blocks)
            {
                if (b.IsBackground) continue;
                if (b.Type == EnumBlockType.SAPLING || b.Type == EnumBlockType.WATER) continue;
                var bpos = new Point(b.Position.X * 32, b.Position.Y * 32);
                var brec = new Rectangle(bpos, new Size(32, 32));
                if (NCore.DistanceBetweenPoint(bpos, Position) > 10 * 32)
                    continue;
                if (brec.IntersectsWith(Rectangle))
                {
                    break;
                }
            }
            Position.Y += 1;
            EntityMoved();
        }

        public bool Move(System.Drawing.Point to)
        {
            bool collision = false;
            foreach(Block b in World.Blocks)
            { 
                if (b.IsBackground) continue;
                if (b.Type == EnumBlockType.SAPLING || b.Type == EnumBlockType.WATER) continue;
                var bpos = new Point(b.Position.X * 32, b.Position.Y * 32);
                var brec = new Rectangle(bpos, new Size(32, 32));
                if (NCore.DistanceBetweenPoint(bpos, Position) > 10 * 32)
                    continue;
                if (brec.IntersectsWith(Rectangle))
                {
                    collision = true;
                    break;
                }
            }
            if(collision)
            {
                NCore.GetNCore().Log($"{Name} ({this.GetType().ToString()}) moved wrongly!", "ERROR");
                return false;
            }
            Position = to;
            EntityMoved();
            return true;
        }

        protected internal virtual void EntityMoved()
        {
        }
    }
}
