using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft2D
{
    class Stats
    {
        Dictionary<string, int> intStats = new Dictionary<string, int>();

        public void incrementStat(string arg0, int arg1 = 1)
        {
            if(intStats.ContainsKey(arg0))
            {
                intStats[arg0] += arg1;
            }
        }

        public Stats()
        {
            intStats.Add("Blocks Broken", 0);
            intStats.Add("Blocks Placed", 0);
            intStats.Add("Foreground Blocks Placed", 0);
            intStats.Add("Background Blocks Placed", 0);
            intStats.Add("Chat Messages Sent", 0);
        }
    }
}
