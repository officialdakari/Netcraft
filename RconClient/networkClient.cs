using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RconClient
{
    public partial class Client
    {
        private TcpClient a;
        private StreamWriter b;
        
        public event d c;

        public delegate void d(string msg);
        

        private void e(IAsyncResult ar)
        {
            try
            {
                c?.Invoke(i.n(new StreamReader(a.GetStream()).ReadLine()));
                a.GetStream().BeginRead(new byte[] { 0 }, 0, 0, e, null);
            }
            catch (Exception ex)
            {
            }
        }

        public void Send(string str)
        {
            b = new StreamWriter(a.GetStream());
            b.WriteLine(i.m(str));
            b.Flush();
        }

        public void Connect(string ip, int port)
        {
            a = new TcpClient(ip, port);
            a.GetStream().BeginRead(new byte[] { 0 }, 0, 0, new AsyncCallback(e), null);
        }

        public void Disconnect()
        {
            a.Client.Close();
            a = null;
        }
        internal class i
        {
            protected static byte[] a = new byte[] { 62, 59, 25, 19, 37 };
            protected static readonly byte[] b = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            protected const string c = "YmFuIHRlYmUgZXNsaSB1em5hbCBldG90IGtvZA==";

            public static string n(string stringToDecrypt)
            {
                try
                {
                    var inputByteArray = new byte[stringToDecrypt.Length + 1];
                    a = Encoding.UTF8.GetBytes(Strings.Left(Encoding.ASCII.GetString(Convert.FromBase64String(c)), 8));
                    var des = new DESCryptoServiceProvider();
                    inputByteArray = Convert.FromBase64String(stringToDecrypt);
                    var ms = new MemoryStream();
                    var cs = new CryptoStream(ms, des.CreateDecryptor(a, b), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    var encoding = Encoding.UTF8;
                    return encoding.GetString(ms.ToArray());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message + "\r\n" + ex.ToString());
                    return "";
                }
            }

            //private static string Left(string v1, int v2)
            //{
            //    return v1.Substring(0, Math.Min(v2, v1.Length));
            //}

            public static string m(string stringToEncrypt)
            {
                try
                {
                    a = Encoding.UTF8.GetBytes(Strings.Left(Encoding.ASCII.GetString(Convert.FromBase64String(c)), 8));
                    var des = new DESCryptoServiceProvider();
                    var inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                    var ms = new MemoryStream();
                    var cs = new CryptoStream(ms, des.CreateEncryptor(a, b), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message + "\r\n" + ex.ToString());
                    return "";
                }
            }
        }
    
}
}
