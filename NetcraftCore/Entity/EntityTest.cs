using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using NCore.netcraft.server.api;

namespace NCore.Entity
{
    public class EntityTest : EntityBase
    {
        public EntityTest() : base("Test", Point.Empty, Netcraft.GetWorld(), new Size(32, 32))
        {
        }

        public override void Tick()
        {
            base.Tick();
        }

        protected internal override void EntityMoved()
        {
            base.EntityMoved();
        }

        public override string GetEntityType()
        {
            return "test";
        }
    }
}
