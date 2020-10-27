
namespace NCore
{
    public class Commandplugins : Command
    {
        public Commandplugins() : base("plugins", "Показывает список плагинов", "plugins", new[] { "pl" })
        {
        }

        public override bool OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (args.Length == 0)
            {
                if (sender.IsPlayer)
                {
                    NetworkPlayer p = (NetworkPlayer)sender;
                    foreach (var a in PluginManager.Plugins)
                        p.PacketQueue.AddQueue($"chat?{a.Name} v{a.Version} => {a.Description}");
                    p.PacketQueue.SendQueue();
                    return true;
                }

                foreach (var a in PluginManager.Plugins)
                    sender.SendMessage($"{a.Name} v{a.Version} => {a.Description}");
                return true;
            }

            return false;
        }
    }
}