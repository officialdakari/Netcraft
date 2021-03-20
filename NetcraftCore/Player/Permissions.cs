using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NCore.Player
{
    public class Permissions
    {
        public Dictionary<string, string[]> perms = new Dictionary<string, string[]>();

        public string[] GetPermissions(string player)
        {
            if (!perms.ContainsKey(player)) return null;
            return perms[player];
        }

        public bool SetPermissions(string player, string[] perms)
        {
            if (!this.perms.ContainsKey(player))
            {
                this.perms.Add(player, perms);
                return true;
            }
            this.perms[player] = perms;
            return true;
        }

        public bool AddPermission(string player, string perm)
        {
            if (!perms.ContainsKey(player)) return false;
            if (perms[player].Contains(perm)) return false;
            perms[player] = perms[player].Append(perm).ToArray();
            return true;
        }

        public bool RemovePermission(string player, string perm)
        {
            if (!perms.ContainsKey(player)) return false;
            if (!perms[player].Contains(perm)) return false;
            perms[player] = perms[player].Where((x) => x != perm).ToArray();
            return true;
        }

        public bool AddPlayer(string player)
        {
            if (perms.ContainsKey(player)) return false;
            perms.Add(player, new string[] { "group.default" });
            return true;
        }

        public bool RemovePlayer(string player)
        {
            if (!perms.ContainsKey(player)) return false;
            perms.Remove(player);
            return true;
        }
    }
}
