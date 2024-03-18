using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Infrastructure.Common
{
    public interface ICustomEncryption
    {
        string HashSHA256(string input);
        string GenerateNewToken(Guid userGuid, int tokenTimeOut, UserType? userType, string roleCode);
        string GenerateNewRefreshToken();
        string Encrypt(string data);
        string Decrypt(string data);
        Guid GetUserGuidFromToken(string token);
    }
    public class CustomEncryption : ICustomEncryption
    {
        Configs appConfig;
        public CustomEncryption(IOptions<Configs> options)
        {
            this.appConfig = options.Value;
        }

        static readonly char[] padding = { '=' };

        public string Encrypt(string data)
        {
            byte[] toEncryptArry = Encoding.UTF8.GetBytes(data);
            byte[] keyArry = Convert.FromBase64String(appConfig.EncryptionKey);

            var aes = new AesCryptoServiceProvider
            {
                Key = keyArry,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.ISO10126
            };
            ICryptoTransform cTransform = aes.CreateEncryptor();
            byte[] encrypted = cTransform.TransformFinalBlock(toEncryptArry, 0, toEncryptArry.Length);
            string encryptCode = Convert.ToBase64String(encrypted);

            encryptCode = encryptCode.TrimEnd(padding).Replace('+', '-').Replace('/', '_');
            return encryptCode;
        }

        public string Decrypt(string data)
        {
            string incoming = data.Replace('_', '/').Replace('-', '+');
            switch (data.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }


            byte[] toDecryptArry = Convert.FromBase64String(incoming);
            byte[] keyArry = Convert.FromBase64String(appConfig.EncryptionKey);
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider
            {
                Key = keyArry,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.ISO10126
            };
            ICryptoTransform cTransform = aes.CreateDecryptor();
            byte[] decrypted = cTransform.TransformFinalBlock(toDecryptArry, 0, toDecryptArry.Length);
            return Encoding.UTF8.GetString(decrypted);
        }


        public string Hash(string MainPassword)
        {
            int j = 0;
            UnicodeEncoding unc = new UnicodeEncoding();
            byte[] byt = unc.GetBytes(MainPassword);
            byte[] bytM = new byte[byt.Length / 2];
            for (int i = 0; i < byt.Length - 1; i++)
            {
                int z;
                Math.DivRem(i, 2, out z);
                if (z == 0)
                {
                    bytM[j] = byt[i];
                    j += 1;
                }
            }
            string hashString = GetHash(bytM);
            return hashString;
        }

        public string GetHash(byte[] byteArray)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] hash = md5.ComputeHash(byteArray, 0, byteArray.Length);
                string b64 = Convert.ToBase64String(hash);
                b64 = String.Empty;
                for (int n = 0; n < hash.Length - 1; n++)
                {
                    if (hash[n] < 10)
                    {
                        b64 = b64 + "0" + hash[n].ToString("x");
                    }
                    else
                    {
                        b64 = b64 + hash[n].ToString("x");
                    }
                }
                return b64;
            }
        }

        public string HashSHA256(string input)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                var hash = BitConverter.ToString(bytes).Replace("-", "").ToLower();
                return hash;
            }
        }

        public string GenerateNewToken(Guid userGuid, int tokenTimeOut, UserType? userType, string roleCode)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(appConfig.TokenKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim("roleCodes", roleCode),
                        new Claim("userGuid", userGuid.ToString()),
                        new Claim("userType", userType == null ? ((int)UserType.Customer).ToString() : ((int)userType).ToString()),
                        new Claim("TimeOut-Minute", tokenTimeOut.ToString()),
                }),

                Expires = DateTime.UtcNow.AddMinutes(tokenTimeOut),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateNewRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        public Guid GetUserGuidFromToken(string token)
        {
            string secret = appConfig.TokenKey;
            var key = Encoding.UTF8.GetBytes(secret);
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var claims = handler.ValidateToken(token.Replace("bearer ", "").Replace("Bearer ", ""), validations, out var tokenSecure);
            return Guid.Parse(claims.FindFirst("userGuid").Value);
        }
    }

}
