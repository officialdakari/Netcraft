using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NCore
{
    public abstract class Command
    {
        public string Name { get; }
        public string Description { get; }
        public string Usage { get; }
        public string[] Aliases { get; }
        public string Permission { get; }

        public Command(string name, string desc, string permission, string usage, string[] aliases = null)
        {
            Name = name;
            Description = desc;
            Usage = usage;
            if (!NCore.IsNothing(aliases))
            {
                Aliases = aliases;
            }
            else
            {
                Aliases = new[] { name };
            }
            Permission = permission;
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

        public bool HasPermission(string permission)
        {
            if (permission == "") return true;
            if (!IsPlayer) return true;
            if (((NetcraftPlayer)this).IsAdmin) return true;
            string[] perms = NCore.GetNCore().permissions.GetPermissions(GetName());
            if (perms == null) return false;
            foreach (string p in perms)
            {
                if(p.StartsWith("group."))
                {
                    string group = p.Substring("group.".Length);
                    foreach(string g in NCore.GetNCore().groups[group])
                        perms = perms.Append(g).ToArray();
                }
            }
            if (perms.Contains("*")) return true;
            return perms.Contains(permission);
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
                await p.Chat(m.Replace("\n", ""));
                return;
            }

            NCore.GetNCore().Log(m);
        }
    }
}