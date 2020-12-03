using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Threading.Tasks;

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

        public abstract Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label);
    }

    public class CommandException : Exception
    {
    }

    public class CommandPermissionsException : CommandException
    {
        public override string Message => "Command sender doesn't have right permissions";
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
                if (this is NetcraftPlayer)
                {
                    return true;
                }

                return false;
            }
        }

        public async Task SendMessage(string m)
        {
            if (IsPlayer)
            {
                NetcraftPlayer p = (NetcraftPlayer)this;
                await p.Chat(m);
                return;
            }

            NCore.GetNCore().Log(m);
        }
    }
}