using System;
using System.Threading.Tasks;
using NCore;

namespace NCore
{
    public class Commanditems : Command
    {
        public Commanditems() : base("items", "Показывает список вещей в игре", "items")
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