using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NetcraftNetwork
{
    class Program
    {

        // ТОЧКА ВХОДА

        static void Main(string[] args)
        {
            Program program = new Program();
            program.Start();
        }

        // PROGRAM

        Hashtable passwords = new Hashtable();
        string file;

        void Start()
        {

            if(!File.Exists("./passwords.txt"))
            {
                File.WriteAllText("./passwords.txt", "", Encoding.UTF8);
            }

            file = File.ReadAllText("./passwords.txt", Encoding.UTF8);
            Server.StartServer(6745);
            Server.OnMessageReceived += MessageReceived;
            Server.OnClientConnect += clientJoin;
            Server.OnClientDisconnect += clientLeave;
            foreach(string i in file.Split("\n"))
            {
                if (i == "") continue;
                passwords.Add(i.Split('=')[0], Utils.GetValue(i.Split('=')[0], file, "ERROR404"));
            }
            while(true)
            {

            }
        }

        // Message
        void MessageReceived(string packet, ref Server.User user)
        {
            string[] s = packet.Split('?');

            if (s[0] == "name")
            {
                string u = String.Join("?", s.Skip(1).ToArray());
                if (!Regex.Match(u, "^[a-zA-Z0-9_]*").Success)
                {
                    user.Send("name");
                    return;
                }
                user.Send("bl?" + File.ReadAllText("./blocked-servers.txt", Encoding.UTF8).Replace("\r\n", "\n").Replace("\n", "?"));
                user.Username = u;
            }
            if (s[0] == "pass")
            {
                string u = String.Join("?", s.Skip(1).ToArray());
                if (!Regex.Match(u, "^[a-zA-Z0-9_]*").Success)
                {
                    user.Send("pass");
                    return;
                }
                if (!passwords.ContainsKey(user.Username)) passwords.Add(user.Username, u);
                if(passwords[user.Username].ToString() != u)
                {
                    user.Send("pass");
                    return;
                }
                user.IsLoggedIn = true;
            }
            if (!user.IsLoggedIn) return;
            if (user.Username == "") return;
            if(s[0] == "blacklist")
            {
                user.Send("bl?" + File.ReadAllText("./blocked-servers.txt", Encoding.UTF8).Replace("\r\n", "\n").Replace("\n", "?"));
            }
        }

        // Join
        void clientJoin(Server.User user)
        {

        }

        // Disconnect
        void clientLeave(Server.User user)
        {
            string txt = "";
            foreach(string i in passwords.Keys.Cast<string>().ToArray())
            {
                txt += $"{i}={passwords[i]}\n";
            }
            file = txt;
            File.WriteAllText("./passwords.txt", txt, Encoding.UTF8);
        }
    }
}
