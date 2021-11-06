using System;
using global::System.Drawing;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;

namespace NCore
{
    public class SaveLoad
    {
        public static string Save(WorldServer arg0)
        {
            return JsonConvert.SerializeObject(arg0, Formatting.Indented);
        }

        public static WorldServer Load(string arg0)
        {
            var world = JsonConvert.DeserializeObject<WorldServer>(arg0);
            //world.Entities.Clear();
            //world.Entities.Add(new Entity.EntityCow());
            // TODO Correct entity saving & loading
            //world.Entities.Add(new Entity.EntityTest());
            return world;
        }
    }
}