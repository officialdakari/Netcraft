using System;
using System.Collections.Generic;
using System.Text;

namespace NCore.World
{
    public abstract class IBlockMetadata
    {
        public Dictionary<string, string> Data;
        public virtual string GetJson() => Newtonsoft.Json.JsonConvert.SerializeObject(Data);
    }
}
