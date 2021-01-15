using System;
using System.Collections.Generic;
using System.Text;

namespace NCore.Entity
{
    public class EntityMetadata
    {
        public Dictionary<string, string> Data;
        public virtual string GetJson() => Newtonsoft.Json.JsonConvert.SerializeObject(Data);
    }
}
