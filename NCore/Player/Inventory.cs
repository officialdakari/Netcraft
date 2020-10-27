using System.Collections.Generic;

namespace NCore
{
    public class Inventory
    {
        public List<ItemStack> Items;
        public NetworkPlayer Owner;

        public void AddItem(ItemStack arg0)
        {
            Items.Add(arg0);
        }

        public Inventory(NetworkPlayer arg0)
        {
            Owner = arg0;
            Items = new List<ItemStack>();
        }

        public int CountOf(Material m)
        {
            foreach (var i in Items)
            {
                if (i.Type == m)
                {
                    return i.Count;
                }
            }

            return -1;
        }
    }
}