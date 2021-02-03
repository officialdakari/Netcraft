using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheat
{
    public class Plugin : Minecraft2D.PluginAbstract
    {
        Minecraft2D.Form1 clientMain;
        public static Plugin plugin;
        internal CheatWindow window;
        public override Minecraft2D.PluginAbstract OnLoad(Minecraft2D.Form1 main)
        {
            clientMain = main;
            plugin = this;
            window = new CheatWindow();
            window.Show();
            window.Text = "NetCheat 1.0 !!! TEST ONLY";
            window.TopMost = false;
            main.WriteChat("Netcheat !!! ТОЛЬКО ДЛЯ ТЕСТИРОВАНИЯ");
            return this;
        }
    }
}
