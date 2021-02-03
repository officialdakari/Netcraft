using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;

namespace Minecraft2D.Entity
{
    public class Entity
    {
        public string UUID;
        public TransparentPicBox Renderer;
        public Point Position;
        public Size Size;
        public string Type;

        public Entity(string uuid, TransparentPicBox renderer, Point position, Size size, string type)
        {
            UUID = uuid;
            Renderer = renderer;
            Position = position;
            Size = size;
            Type = type;
        }
    }
}
