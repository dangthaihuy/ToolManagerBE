using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Manager.DataLayer.Entities;
using JWT.Algorithms;
using JWT;
using JWT.Serializers;
using JWT.Exceptions;
using Newtonsoft.Json;
using Serilog;

namespace Manager.WebApp.Helpers
{
    public class Utility
    {
        public static string GenerateAuthJwtToken(IdentityUser info, string secretKey)
        {
            var token = string.Empty;
            try
            {
                var dtNow = DateTime.UtcNow;
                DateTime expDate = dtNow.AddDays(90);

                var payload = new Dictionary<string, object>
                {
                    { "Id", info.Id},
                    { "UserName", info.UserName},
                    { "iat", dtNow.ToUnixTimestamp()},
                    { "exp", expDate.ToUnixTimestamp() }
                };

                IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
                IJsonSerializer serializer = new JsonNetSerializer();
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

                token = encoder.Encode(payload, secretKey);
            }
            catch (Exception ex)
            {
                Log.ForContext<Utility>().Error("Token has invalid signature", ex.ToString());
            }

            return token;
        }

        public static bool ValidateJwtToken(string token, string secretKey)
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                var provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                var json = decoder.Decode(token, secretKey, verify: true);

                return true;
            }
            catch (TokenExpiredException)
            {
                Log.ForContext<Utility>().Error("Token has expired: {0}", token);
            }
            catch (SignatureVerificationException)
            {
                Log.ForContext<Utility>().Error("Token has invalid signature", token);
            }
            catch (Exception ex)
            {
                Log.ForContext<Utility>().Error("Token is invalid: {0} --- Error: {1}", token, ex.ToString());
            }

            return false;
        }

        public static IdentityUser DecodeJwtAuthTokenData(string token, string secretKey)
        {
            IdentityUser info = null;
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                var provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                var json = decoder.Decode(token, secretKey, verify: true);
                info = JsonConvert.DeserializeObject<IdentityUser>(json);
            }
            catch (TokenExpiredException)
            {
                Log.ForContext<Utility>().Error("Token has expired: {0}", token);
            }
            catch (SignatureVerificationException)
            {
                Log.ForContext<Utility>().Error("Token has invalid signature", token);
            }
            catch (Exception ex)
            {
                Log.ForContext<Utility>().Error("Token is invalid: {0} --- Error: {1}", token, ex.ToString());
            }

            return info;
        }

        public static string GetRandomPasswordUsingGUID(int length)
        {
            string guidResult = System.Guid.NewGuid().ToString();
            guidResult = guidResult.Replace("-", string.Empty);
            if (length <= 0 || length > guidResult.Length)
                throw new ArgumentException("Length must be between 1 and " + guidResult.Length);
            return guidResult.Substring(0, length);
        }

        public static string Md5HashingData(string rawStr)
        {
            StringBuilder hash = new StringBuilder();
            if (!string.IsNullOrEmpty(rawStr))
            {
                MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
                byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(rawStr));

                for (int i = 0; i < bytes.Length; i++)
                {
                    hash.Append(bytes[i].ToString("x2"));
                }
            }

            return hash.ToString();
        }

        public static string GenerateRedisKeyWithPrefixAndSurfix(string prefix, string surfix)
        {
            var md5Surfix = Md5HashingData(surfix);
            return string.Format("{0}_{1}", prefix, md5Surfix);
        }

        //public string GenerateStringByFormat
        public static string GenerateStringByFormat(string format, params object[] paramList)
        {
            return string.Format(format, paramList);
        }

        //public static string GenerateOTPCode()
        //{
        //    Random random = new Random();
        //    return new string(Enumerable.Repeat(Manager.WebAppSettings.CharactersOfOTPCode, Manager.WebAppSettings.NumberCharactersOfOTPCode)
        //     .Select(s => s[random.Next(s.Length)]).ToArray());
        //}

        public static string TripleDESEncrypt(string key, string data)
        {
            data = data.Trim();
            byte[] keydata = Encoding.ASCII.GetBytes(key);
            string md5String = BitConverter.ToString(new
            MD5CryptoServiceProvider().ComputeHash(keydata)).Replace("-", "").ToLower(); byte[] tripleDesKey = Encoding.ASCII.GetBytes(md5String.Substring(0, 24)); TripleDES tripdes = TripleDESCryptoServiceProvider.Create();
            tripdes.Mode = CipherMode.ECB;
            tripdes.Key = tripleDesKey;
            tripdes.GenerateIV();
            MemoryStream ms = new MemoryStream();
            CryptoStream encStream = new CryptoStream(ms, tripdes.CreateEncryptor(), CryptoStreamMode.Write);
            encStream.Write(Encoding.ASCII.GetBytes(data), 0, Encoding.ASCII.GetByteCount(data));
            encStream.FlushFinalBlock();
            byte[] cryptoByte = ms.ToArray();
            ms.Close();
            encStream.Close();
            return Convert.ToBase64String(cryptoByte, 0,
            cryptoByte.GetLength(0)).Trim();
        }

        public static string TripleDESDecrypt(string key, string data)
        {
            byte[] keydata = Encoding.ASCII.GetBytes(key);
            string md5String = BitConverter.ToString(new
            MD5CryptoServiceProvider().ComputeHash(keydata)).Replace("-", "").ToLower();
            byte[] tripleDesKey = Encoding.ASCII.GetBytes(md5String.Substring(0, 24));
            TripleDES tripdes = TripleDESCryptoServiceProvider.Create();
            tripdes.Mode = CipherMode.ECB;
            tripdes.Key = tripleDesKey;
            byte[] cryptByte = Convert.FromBase64String(data);
            MemoryStream ms = new MemoryStream(cryptByte, 0, cryptByte.Length);
            ICryptoTransform cryptoTransform = tripdes.CreateDecryptor();
            CryptoStream decStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Read);
            StreamReader read = new StreamReader(decStream);
            return (read.ReadToEnd());
        }

        public static string GenTranIdWithPrefix(string prefix)
        {
            var tranId = "{0}{1}";
            var surfix = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + GetRandomPasswordUsingGUID(5);
            tranId = string.Format(tranId, prefix, surfix);

            return tranId;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }


        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        public static string EncryptText(string input, string password)
        {
            //// Get the bytes of the string
            //byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            //byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            //// Hash the password with SHA256
            //passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            //byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            //string result = Convert.ToBase64String(bytesEncrypted);

            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            string result = Convert.ToBase64String(bytesToBeEncrypted);

            return result;
        }

        public static string DecryptText(string input, string password)
        {
            // Get the bytes of the string
            //byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
            //byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            //passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            //byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            //string result = Encoding.UTF8.GetString(bytesDecrypted);

            byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
            string result = Encoding.UTF8.GetString(bytesToBeDecrypted);

            return result;
        }
    }
}