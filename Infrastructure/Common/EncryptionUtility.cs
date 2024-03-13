using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public static class EncryptionUtility
    {
        public static string HashSHA256(string input)
        {
            using (var sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string HashPasswordWithSalt(string password, string salt)
        {
            return HashSHA256(password + salt);
        }

        static string _key = "MgYUK9UWRUaC7EuCVABJhg/kpL+uRQA1b99kU7mrLMG=";
        static string _IV = "XsMzzJU5j1Z6xg6Tu7VyyQ==";
        static readonly char[] padding = { '=' };

        public static string Encrypt(string data)
        {
            return data;

            byte[] toEncryptArry = Encoding.UTF8.GetBytes(data);
            byte[] keyArry = Convert.FromBase64String(_key);
            byte[] iv = Convert.FromBase64String(_IV);

            var aes = new AesCryptoServiceProvider
            {
                Key = keyArry,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7,
                IV = iv,
            };

            ICryptoTransform cTransform = aes.CreateEncryptor(keyArry, iv);
            byte[] encrypted = cTransform.TransformFinalBlock(toEncryptArry, 0, toEncryptArry.Length);
            string encryptCode = Convert.ToBase64String(encrypted);

            encryptCode = encryptCode.TrimEnd(padding).Replace('+', '-').Replace('/', '_');
            return encryptCode;
        }
        public static string Decrypt(string data)
        {
            return data;

            string incoming = data.Replace('_', '/').Replace('-', '+');
            switch (data.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }


            byte[] toDecryptArry = Convert.FromBase64String(incoming);
            byte[] keyArry = Convert.FromBase64String(_key);
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider
            {
                Key = keyArry,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = aes.CreateDecryptor();
            byte[] decrypted = cTransform.TransformFinalBlock(toDecryptArry, 0, toDecryptArry.Length);
            return Encoding.UTF8.GetString(decrypted);
        }



        public static string GenerateName()
        {
            return Guid.NewGuid().ToString();
        }



    }
}
