using System;
using System.Collections.Generic;
using System.Text;

namespace NCore.Player
{
    public class PlayerSave
    {
        public List<ItemStack> items;
        public bool isAdmin;
        public System.Drawing.Point pos;
        public Dictionary<string, string> stats;
    }
}
