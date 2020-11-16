using System;
using System.Threading.Tasks;

namespace NCore
{
    public class Commandcraft : Command
    {
        public Commandcraft() : base("craft", "Craft item [deprecated; use craft menu instead]", "craft <material>")
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (!sender.IsPlayer)
            {
                return false;
            }

            if (args.Length == 1)
            {
                try
                {
                    var m = Enum.Parse(typeof(Material), args[0].ToUpper());
                    NetworkPlayer p = (NetworkPlayer)sender;
                    await p.Craft((Material)m);
                    await p.Chat("Crafted!");
                }
                catch (Exception ex)
                {
                    await sender.SendMessage("An internal error occured.");
                }

                return true;
            }

            return false;
        }
    }
}