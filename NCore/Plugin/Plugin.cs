
namespace NCore
{
    public abstract class Plugin
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string[] Authors { get; set; }

        internal netcraft.server.api.PluginLogger logger;

        public System.Reflection.Assembly Assembly { get; set; }

        public abstract Plugin Create();

        public void SetOptions(string a, string b, string c, string[] e)
        {
            Name = a;
            Description = b;
            Version = c;
            Authors = e;
            logger = new netcraft.server.api.PluginLogger(this);
        }
         
        public netcraft.server.api.PluginLogger GetLogger()
        {
            return logger;
        }

        public abstract string OnLoad();
        public abstract void OnUnload();
    }
}