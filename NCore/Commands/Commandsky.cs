
namespace NCore
{
    public class Commandsky : Command
    {
        public Commandsky() : base("sky", "test", "sky <clr>")
        {
        }

        public override bool OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (args.Length == 1)
            {
                if (!sender.IsPlayer)
                {
                    sender.SendMessage("Только для игрока.");
                    return true;
                }

                NetworkPlayer p = (NetworkPlayer)sender;
                p.SendSkyColorChange(System.Drawing.Color.FromName(args[0]));
                return true;
            }

            return false;
        }
    }
}