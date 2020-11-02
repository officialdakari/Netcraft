using System;
using System.Linq;
using Minecraft2D;

namespace Minecraft2D
{

    internal static class Utils
    {
        internal static string LANGUAGE = "башкирский";
        public static object IIf(bool expession, object valueIfTrue, object valueIfFalse)
        {
            if(expession == true)
            {
                return valueIfTrue;
            } else
            {
                return valueIfFalse;
            }
            return null;
        }

        public static string GetValue(string var, string cfg, string ifNull = null)
        {
            cfg = cfg.Replace("\r\n", "\n");
            foreach (var a in cfg.Split('\n'))
            {
                var b = a.Split('=');
                if ((b[0].ToLower() ?? "") == (var.ToLower() ?? ""))
                {
                    return string.Join("=", b.Skip(1).ToArray());
                }
            }

            return ifNull;
        }

        internal static string InputBox(string text)
        {
            LoginForm1 i = new LoginForm1();
            Lang lang = Lang.FromFile($"./lang/{LANGUAGE}.txt");
            i.UsernameLabel.Text = lang.get(text);
            if(i.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return i.UsernameTextBox.Text;
            }
            return null;
        }
    }

}