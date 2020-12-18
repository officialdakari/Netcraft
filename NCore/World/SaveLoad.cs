using System;
using global::System.Drawing;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;

namespace NCore
{
    public class SaveLoad
    {
        public string Save(WorldServer arg0)
        {
            return JsonConvert.SerializeObject(arg0);
        }

        public WorldServer Load(string arg0)
        {
            var world = JsonConvert.DeserializeObject<WorldServer>(arg0);
            return world;
        }
    }
}