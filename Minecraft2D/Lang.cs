using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft2D
{
    public class Lang
    {

        internal Hashtable formats;
        public static Lang FromText(string t)
        {
            Lang lang = new Lang();
            lang.formats = new Hashtable(new Dictionary<string, string>());
            t = t.Replace("\r\n", "\n");
            foreach(string i in t.Split('\n'))
            {
                lang.formats.Add(i.Split('=')[0], Utils.GetValue(i.Split('=')[0], t, i.Split('=')[0]));
            }
            return lang;
        }

        public static Lang FromFile(string p)
        {
            return FromText(System.IO.File.ReadAllText(p, Encoding.UTF8));
        }

        public string get(string i)
        {
            if (!formats.ContainsKey(i)) return i;
            return formats[i].ToString();
        }

    }
}
