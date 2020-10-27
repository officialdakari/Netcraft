using System;

namespace NCore
{
    public class Commandcraft : Command
    {
        public Commandcraft() : base("craft", "Скрафтить вещь [ИГРОК]", "craft <material>")
        {
        }

        public override bool OnCommand(CommandSender sender, Command cmd, string[] args, string label)
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
                    p.Craft((Material)m);
                    p.Chat("Crafted!");
                }
                catch (Exception ex)
                {
                    sender.SendMessage("An internal error occured.");
                }

                return true;
            }

            return false;
        }
    }
}