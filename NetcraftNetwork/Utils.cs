using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetcraftNetwork
{
    internal static class Utils
    {
        public static object IIf(bool expession, object valueIfTrue, object valueIfFalse)
        {
            if (expession == true)
            {
                return valueIfTrue;
            }
            else
            {
                return valueIfFalse;
            }
        }

        public static string Left(string a, int b)
        {
            return a.Substring(0, Math.Min(b, a.Length));
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
    }
}
