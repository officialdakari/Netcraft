using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using NCore;
using NCore.netcraft.server.api;
using System.Threading.Tasks;

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
        public string UUID { get; set; } = Guid.NewGuid().ToString();
        public EntityMetadata Metadata { get; set; } = new EntityMetadata();

        public EntityBase(string name, System.Drawing.Point point, WorldServer worldServer, System.Drawing.Size sz)
        {
            Position = point;
            World = Netcraft.GetWorld();
            Size = sz;
            Rectangle = new System.Drawing.Rectangle(point, sz);
            Name = name;
        }

        public virtual void HandleGravity()
        {
            try
            {
                foreach (Block b in Netcraft.GetWorld().Blocks)
                {
                    if (b.IsBackground) continue;
                    if (b.Type == EnumBlockType.SAPLING || b.Type == EnumBlockType.WATER) continue;
                    var bpos = new Point(b.Position.X * 32, b.Position.Y * 32);
                    var brec = new Rectangle(bpos, new Size(32, 32));
                    if (NCore.DistanceBetweenPoint(bpos, Position) > 10 * 32)
                        continue;
                    if (brec.IntersectsWith(Rectangle))
                    {
                        return;
                    }
                }
                Position.Y += 1;
                EntityMoved();
            } catch(Exception ex)
            {
                Netcraft.NCore().Log("HandleGravity in entity " + UUID + "\r\n" + ex.ToString(), "WARNING");
            }
        }

        public virtual void Tick()
        {
            Rectangle = new Rectangle(Position, Size);
            HandleGravity();
        }

        public virtual async Task<bool> Move(System.Drawing.Point to, bool force = false)
        {
            bool collision = false;
            if(!force)
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
                        collision = true;
                        break;
                    }
                }
                if (collision)
                {
                    NCore.GetNCore().Log($"{Name} ({this.GetType().ToString()}) moved wrongly!", "ERROR");
                    return false;
                }
            }
            Position = to;
            EntityMoved();
            return true;
        }

        protected internal virtual void EntityMoved()
        {
            NCore.GetNCore().Send("moveentity?" + UUID + "?" + Position.X.ToString() + "?" + Position.Y.ToString());
        }

        public virtual string GetEntityType()
        {
            return null;
        }

        public virtual bool Despawn()
        {
            return true;
        }
    }
}
