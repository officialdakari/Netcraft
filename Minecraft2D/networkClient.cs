using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft2D
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
                c?.Invoke(i.m(new StreamReader(a.GetStream()).ReadLine()));
                a.GetStream().BeginRead(new byte[] { 0 }, 0, 0, e, null);
            }
            catch (Exception ex)
            {
            }
        }

        public void f(string str)
        {
            b = new StreamWriter(a.GetStream());
            b.WriteLine(i.n(str));
            b.Flush();
        }

        public void g(string ip, int port)
        {
            a = new TcpClient(ip, port);
            a.GetStream().BeginRead(new byte[] { 0 }, 0, 0, new AsyncCallback(e), null);
        }

        public void h()
        {
            a.Client.Close();
            a = null;
        }
        public partial class i
        {
            private static byte[] j = new byte[] { 62, 59, 25, 19, 37 };
            private static byte[] k = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            internal const string l = "lpAlgZ0123"; // "HOMECLOUD" & New Random().Next(11111111, 99999999).ToString & New Random().Next(11111111, 99999999).ToString ' & New Random().Next(11111111, 99999999).ToString & New Random().Next(11111111, 99999999).ToString

            public static string m(string stringToDecrypt)
            {
                try
                {
                    var inputByteArray = new byte[stringToDecrypt.Length + 1];
                    j = Encoding.UTF8.GetBytes(Strings.Left(l, 8));
                    var des = new DESCryptoServiceProvider();
                    inputByteArray = Convert.FromBase64String(stringToDecrypt);
                    var ms = new MemoryStream();
                    var cs = new CryptoStream(ms, des.CreateDecryptor(j, k), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    var encoding = Encoding.UTF8;
                    return encoding.GetString(ms.ToArray());
                }
                catch (Exception ex)
                {
                    // oops - add your exception logic
                    // MsgBox("ошибка")
                }

                return null;
            }

            public static string n(string stringToEncrypt)
            {
                try
                {
                    j = Encoding.UTF8.GetBytes(Strings.Left(l, 8));
                    var des = new DESCryptoServiceProvider();
                    var inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                    var ms = new MemoryStream();
                    var cs = new CryptoStream(ms, des.CreateEncryptor(j, k), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());
                }
                catch (Exception ex)
                {
                    // oops - add your exception logic
                    // MsgBox("ошибка")
                }

                return null;
            }
        }
    }
}
