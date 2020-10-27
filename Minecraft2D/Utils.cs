using System;
using Minecraft2D;

namespace Minecraft2D
{

    internal static class Utils
    {
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
    }

}