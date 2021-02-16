
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NCore
{
    public abstract class Plugin
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Version { get; private set; }
        public string[] Authors { get; private set; }
        public List<Thread> Threads;

        internal netcraft.server.api.PluginLogger logger;

        public System.Reflection.Assembly Assembly { get; internal set; }

        public abstract Plugin Create();

        public void SetOptions(string a, string b, string c, string[] e)
        {
            Name = a;
            Description = b;
            Version = c;
            Authors = e;
            logger = new netcraft.server.api.PluginLogger(this);
            Threads = new List<Thread>();
        }
         
        public netcraft.server.api.PluginLogger GetLogger()
        {
            return logger;
        }

        public abstract string OnLoad();
        public abstract void OnUnload();
    }
}