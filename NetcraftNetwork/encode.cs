using System;
using global::System.IO;
using global::System.Security.Cryptography;
using global::System.Text;
using Microsoft.VisualBasic; // Install-Package Microsoft.VisualBasic
using NetcraftNetwork;
/* TODO ERROR: Skipped WarningDirectiveTrivia */
public partial class Encode
{
    private static byte[] key = new byte[] { 62, 59, 25, 19, 37 };
    private static byte[] IV = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
    internal const string EncryptionKey = "lpAlgZ0123"; // "HOMECLOUD" & New Random().Next(11111111, 99999999).ToString & New Random().Next(11111111, 99999999).ToString ' & New Random().Next(11111111, 99999999).ToString & New Random().Next(11111111, 99999999).ToString

    public static string Decrypt(string stringToDecrypt)
    {
        try
        {
            var inputByteArray = new byte[stringToDecrypt.Length + 1];
            key = Encoding.UTF8.GetBytes(Utils.Left(EncryptionKey, 8));
            var des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(stringToDecrypt);
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
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

    public static string Encrypt(string stringToEncrypt)
    {
        try
        {
            key = Encoding.UTF8.GetBytes(Utils.Left(EncryptionKey, 8));
            var des = new DESCryptoServiceProvider();
            var inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
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