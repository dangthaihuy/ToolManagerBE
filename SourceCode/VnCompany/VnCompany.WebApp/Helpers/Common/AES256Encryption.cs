using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Security.Cryptography;
using System.IO;

namespace VnCompany.WebApp.Helpers
{
    public class AES256Encryption
    {
        private byte[] key = null;
        private byte[] vector = null;


        public AES256Encryption()
            : this("A93reRTUJHsCuQSHR+L3GxqOJyDmQpCgps102ciuabc=")
        {
        }

        public AES256Encryption(string secretKey32BytesInBase64)
        {
            //32 bytes
            key = Convert.FromBase64String(secretKey32BytesInBase64);

            //16 bytes first taken from Key
            vector = key.Take(16).ToArray();
        }

        public byte[] GetBytesContent(string contentString)
        {
            var bytesArray = System.Text.Encoding.UTF8.GetBytes(contentString);
            return bytesArray;
        }


        public string GetContentFromBytes(byte[] bytesArray)
        {
            var contentString = System.Text.Encoding.UTF8.GetString(bytesArray);
            return contentString;
        }


        public RijndaelManaged CreateAES256()
        {
            var AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            AES.Mode = CipherMode.CBC;
            AES.Padding = PaddingMode.PKCS7;

            AES.Key = key;      //32 bytes = AES.KeySize / 8
            AES.IV = vector;    //16 bytes = AES.BlockSize / 8 

            return AES;
        }

        public string Encrypt(string unencrypted)
        {
            using (RijndaelManaged AES = CreateAES256())
            {
                var encryptor = AES.CreateEncryptor();
                var buffer = System.Text.Encoding.UTF8.GetBytes(unencrypted);

                var encryptedBytes = Transform(buffer, encryptor);
                var encryptedString = Convert.ToBase64String(encryptedBytes);
                return encryptedString;
            }
        }



        public string Decrypt(string encrypted)
        {
            using (RijndaelManaged AES = CreateAES256())
            {
                var decryptor = AES.CreateDecryptor();
                var encryptedBytes = Convert.FromBase64String(encrypted);

                var decryptedBytes = Transform(encryptedBytes, decryptor);
                var decryptedString = System.Text.Encoding.UTF8.GetString(decryptedBytes);

                return decryptedString;
            }
        }


        protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            MemoryStream stream = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }
            return stream.ToArray();
        }



        public static string GenerateRandomKey()
        {
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                byte[] secretKeyByteArray = new byte[32]; //256 bit
                cryptoProvider.GetBytes(secretKeyByteArray);
                var APIKey = Convert.ToBase64String(secretKeyByteArray);

                return APIKey;
            }            
        }

    }
}