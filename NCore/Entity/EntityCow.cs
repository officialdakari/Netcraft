using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NCore.Entity
{
    public class EntityCow : EntityBase
    {
        public EntityCow() : base("Cow", new System.Drawing.Point(0, 0), null, new System.Drawing.Size(64, 40))
        {
        }

        public override string GetEntityType()
        {
            return "Cow";
        }

        public override void Tick()
        {
            base.Tick();
            Move(new Point(Position.X + goDirection, Position.Y), true);
            if (Position.X < 1) goDirection = 1;
            if (Position.X >= 50) goDirection = -1; 
        }

        int goDirection = 1;
    }
}
