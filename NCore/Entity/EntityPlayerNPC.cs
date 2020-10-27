using global::System.Drawing;
using System.Threading;

namespace NCore
{
    public class EntityPlayerNPC : Entity.EntityBase
    {
        Thread thread;

        public EntityPlayerNPC(WorldServer w) : base("NPCPlayer1", new Point(0,0), w, new Size(47,92))
        {
            thread = new Thread((_) => entityAiLoop());
            thread.Start();
        }

        private void entityAiLoop()
        {
            while(true)
            {
                Thread.Sleep(10);
                handleGravity();
            }
        }

        protected internal override void EntityMoved()
        {
            base.EntityMoved();
        }
    }
}