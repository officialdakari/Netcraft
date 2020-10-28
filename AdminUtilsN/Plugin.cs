using Microsoft.VisualBasic.CompilerServices;
using NCore.netcraft.server.api;

namespace AdminUtilsN
{
    public class Plugin : NCore.Plugin
    {
        internal static Plugin instance;

        public static Plugin GetInstance()
        {
            return instance;
        }

        private const string DEF_CONFIG = "enable-automod=1";

        public override NCore.Plugin Create()
        {
            SetOptions("AdminUtilsN", "Утилиты для администраторов серверов NetCraft.", "1.0", new[] { "GladCypress3030" });
            instance = this;
            return this;
        }

        internal Config conf;
        internal Config passwords;
        private Automod automodInstance;

        public Automod GetAutomod()
        {
            return automodInstance;
        }

        public override string OnLoad()
        {
            GetLogger().Info("Плагин загружен.");
            if (System.IO.Directory.Exists("./data"))
            {
                if (!System.IO.Directory.Exists("./data/AdminUtilsN"))
                {
                    System.IO.Directory.CreateDirectory("./data/AdminUtilsN");
                    System.IO.File.WriteAllText("./data/AdminUtilsN/config.txt", DEF_CONFIG, System.Text.Encoding.UTF8);
                }

                conf = Config.Read("./data/AdminUtilsN/config.txt");
            }
            else
            {
                return "Ошибка при загрузке плагина: Не удалось обнаружить папку data.";
            }

            Netcraft.AddCommand(new Commands.Commandmute());

            if (conf.Parse("enable-automod") == "1")
            {
                automodInstance = new Automod();
                GetLogger().Info("Auto-mod enabled!");
            }

            return null;
        }

        public void AdminMessage(string a)
        {
            foreach (var b in Netcraft.GetOnlinePlayers())
            {
                if (Conversions.ToBoolean(b.IsAdmin))
                {
                    b.Chat(a);
                }
            }

            GetLogger().Info(a);
        }

        public override void OnUnload()
        {
            GetLogger().Info("Плагин выгружен.");
        }
    }
}