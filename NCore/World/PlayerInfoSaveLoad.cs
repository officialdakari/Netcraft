using System;
using Microsoft.VisualBasic.CompilerServices;

namespace NCore
{
    public class PlayerInfoSaveLoad
    {
        public static string Save(NetworkPlayer arg0)
        {
            string data = "";
            foreach (var i in arg0.PlayerInventory.Items)
                data += $"{i.Type}?{i.Count};";
            data = data.TrimEnd(';') + "^";
            data += arg0.IsAdmin.ToString();
            return data;
        }

        public static void Load(NetworkPlayer arg0, string arg1)
        {
            try
            {
                arg0.IsAdmin = Conversions.ToBoolean(arg1.Split("^")[1]);
                foreach (var i in arg1.Split("^")[0].Split(";"))
                    arg0.PlayerInventory.AddItem(new ItemStack((Material)Enum.Parse(typeof(Material), i.Split("?")[0]), Conversions.ToInteger(i.Split("?")[1].TrimEnd('^'))));
            }
            catch (Exception ex)
            {
                NCore.LogError(ex);
            }
        }
    }
}