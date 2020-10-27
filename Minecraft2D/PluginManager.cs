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
                // If ONE_PLUGIN_LOADED Then
                var p = Assembly.LoadFrom(path);
                // Dim pForm As Form = New Form
                // pForm.Text = $"(TeamExplorer) Расширение {p.GetName.Name}"

                Plugins.Add(p);
                ((PluginAbstract)GetObject(p, "Plugin")).OnLoad(My.MyProject.Forms.Form1);
                return p;
            }
            // GetObject(p, "Plugin").MainForm = pForm
            // GetObject(p, "Plugin").OnMainFormSet()

            // GetMainWindow(Plugins.Last).SetMsgBox(New MessageBox1)
            // Else
            // Plugins(0) = Assembly.LoadFrom(path)

            // ONE_PLUGIN_LOADED = True
            // End If
            // Plugin_One = Assembly.LoadFrom(path)
            catch (Exception ex)
            {
                // Main.Fatal($"Не удалось загрузить расширение. Путь к расширению: {path}{vbCrLf}{vbCrLf}{ex.ToString}")
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

            return default;
        }
    }
}