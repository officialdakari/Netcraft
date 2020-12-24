using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RconClient
{
    class Program
    {
        static Client client;
        static void Main(string[] args)
        {
            client = new Client();
            Console.Write("Введите IP: ");
            client.Connect(Console.ReadLine(), 6575);
            Console.Write("Введите пароль: ");
            client.Send("rconsetup?" + Console.ReadLine());
            while(true)
            {
                client.Send("cmd?" + Console.ReadLine());
            }
        }
    }
}
