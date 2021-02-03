using System;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;

namespace NCore
{
    public class PlayerInfoSaveLoad
    {
        public static string Save(NetcraftPlayer arg0)
        {
            string data = "";
            //foreach (var i in arg0.PlayerInventory.Items)
            //    data += $"{i.Type}?{i.Count};";
            //data = data.TrimEnd(';') + "^";
            //data += arg0.IsAdmin.ToString();
            Player.PlayerSave save = new Player.PlayerSave();
            save.isAdmin = arg0.IsAdmin;
            save.items = arg0.PlayerInventory.Items;
            save.pos = arg0.Position;
            data = JsonConvert.SerializeObject(save);
            return data;
        }

        public static void Load(NetcraftPlayer arg0, string arg1)
        {
            try
            {
                Player.PlayerSave save = JsonConvert.DeserializeObject<Player.PlayerSave>(arg1);
                arg0.Teleport(save.pos.X, save.pos.Y);
                arg0.IsAdmin = save.isAdmin;
                foreach(var i in save.items)
                {
                    arg0.PlayerInventory.Items.Add(i);
                }
                arg0.UpdateInventory();
                //arg0.IsAdmin = Conversions.ToBoolean(arg1.Split("^")[1]);
                //foreach (var i in arg1.Split("^")[0].Split(";"))
                //    arg0.PlayerInventory.AddItem(new ItemStack((Material)Enum.Parse(typeof(Material), i.Split("?")[0]), Conversions.ToInteger(i.Split("?")[1].TrimEnd('^'))));
            }
            catch (Exception ex)
            {
                NCore.GetNCore().LogError(ex);
            }
        }
    }
}