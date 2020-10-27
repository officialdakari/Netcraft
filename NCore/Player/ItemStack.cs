
namespace NCore
{
    public class ItemStack
    {
        public Material Type { get; set; }
        public int Count { get; set; }

        public ItemStack(Material arg0, int arg1)
        {
            Type = arg0;
            Count = arg1;
        }
    }
}