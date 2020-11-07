
using System.Threading.Tasks;

namespace NCore
{
    public class Commandversion : Command
    {
        public Commandversion() : base("version", "Показывает версию сервера", "version")
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (args.Length == 0)
            {
                await sender.SendMessage($"Этот сервер использует NCore {NCore.NCORE_VERSION} для Netcraft {NCore.NETCRAFT_VERSION}. Запущено {PluginManager.Plugins.Count} плагинов.");
                return true;
            }

            return false;
        }
    }
}