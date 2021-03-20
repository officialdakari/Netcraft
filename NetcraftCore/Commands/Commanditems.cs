using System;
using System.Threading.Tasks;
using NCore;

namespace NCore
{
    public class Commanditems : Command
    {
        public Commanditems() : base("items", NCore.GetNCore().lang.get("commands.items.description"), "netcraft.command.items", "items")
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (args.Length == 0)
            {
                var a = Enum.GetNames(typeof(Material));
                await sender.SendMessage(string.Join(", ", a).ToLower());
                return true;
            }

            return false;
        }
    }
}