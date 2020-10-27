using System.Collections.Generic;
using global::System.Collections.Specialized;
using Microsoft.VisualBasic;

namespace NCore
{
    public class SendQueueType
    {
        private NetworkPlayer field1;
        private StringCollection field2 = new StringCollection();

        public void SendQueue()
        {
            string a = "";
            foreach (string f in field2)
                a += f + Constants.vbLf;
            // a = a.TrimEnd(vbCrLf)
            // Console.WriteLine("[DEBUG] Send packet " + a)
            field1.Send(a);
            field2.Clear();
        }

        public void AddQueue(string arg0)
        {
            field2.Add(arg0);
        }

        public void RemoveQueue(string arg0)
        {
            if (!field2.Contains(arg0))
            {
                throw new KeyNotFoundException("Cannot delete unexisting packet in queue");
            }

            field2.Remove(arg0);
        }

        public SendQueueType(ref NetworkPlayer arg0)
        {
            field1 = arg0;
        }
    }
}