using System;
using System.Linq;
using Minecraft2D;

namespace Minecraft2D
{

    internal static class Utils
    {
        internal const string LANGUAGE = "english";
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
    }

}