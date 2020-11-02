using Microsoft.VisualBasic.CompilerServices;

namespace NCore
{
    public abstract class Command
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Usage { get; set; }
        public string[] Aliases { get; set; }

        public Command(string a, string b, string c, string[] d = null)
        {
            Name = a;
            Description = b;
            Usage = c;
            if (!NCore.IsNothing(d))
            {
                Aliases = d;
            }
            else
            {
                Aliases = new[] { a };
            }
        }

        public abstract bool OnCommand(CommandSender sender, Command cmd, string[] args, string label);
    }

    public class CommandSender
    {
        internal string senderName;
        internal bool senderAdmin;

        public CommandSender(object a, object b)
        {
            senderName = Conversions.ToString(a);
            senderAdmin = Conversions.ToBoolean(b);
        }

        public string GetName()
        {
            return senderName;
        }

        public bool GetAdmin()
        {
            return senderAdmin;
        }

        public bool IsPlayer
        {
            get
            {
                if (this is NetworkPlayer)
                {
                    return true;
                }

                return false;
            }
        }

        public void SendMessage(string m)
        {
            if (IsPlayer)
            {
                NetworkPlayer p = (NetworkPlayer)this;
                p.Chat(m);
                return;
            }

            NCore.GetNCore().Log(m);
        }
    }
}