using System;
using System.Collections.Generic;
using global::System.Reflection;

namespace Minecraft2D
{
    public class PluginManager
    {
        public static List<Assembly> Plugins { get; set; } = new List<Assembly>();
        // Shared Property Plugin_One As Assembly = Nothing
        public static Assembly Load(string path)
        {
            try
            {
                Form1.instance.log($"Loading mod from: {path}");
                var p = Assembly.LoadFrom(path);
                Plugins.Add(p);
                Form1.instance.log($"Calling load function");
                ((PluginAbstract)GetObject(p, "Plugin")).OnLoad(My.MyProject.Forms.Form1);
                Form1.instance.log($"Loaded: {path}");
                return p;
            }
            catch (Exception ex)
            {
            }

            return default;
        }

        public static object GetObject(Assembly plugin, string obj)
        {
            try
            {
                var Plugin_MainWindowType = plugin.GetType($"{plugin.GetName().Name}.{obj}", true, false);
                var plugin_MainWindow = Activator.CreateInstance(Plugin_MainWindowType);
                return plugin_MainWindow;
            }
            catch (Exception ex)
            {
            }

            return null;
        }
    }
}