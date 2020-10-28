using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FOX.BusinessOperations.CommonServices
{
    public class EncryptionDecryption
    {
        public string Gererate_Token(string Username, string Password, string Device_Info, string key, out string time_expire)
        {
            var expiry = GetExpiry();
            string stringToSign = HttpUtility.UrlEncode(Username) + "&" + HttpUtility.UrlEncode(Password) + "\n" + expiry;
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            var sasToken = String.Format(CultureInfo.InvariantCulture, "MTBC-TOKEN_SIGNATURE UN={0}|SIG={1}|ET={2}|DI={3}",
                HttpUtility.UrlEncode(Username), HttpUtility.UrlEncode(signature), expiry, Device_Info);
            time_expire = expiry.ToString();
            return sasToken;

        }
        public string GetExpiry()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            return Convert.ToBase64String(time);

        }

        // To handle encryption/decryption Objective-C,C#
        private string passPhrase = "2657894562368456";

        // To handle encryption/decryption java,C#
        private RijndaelManaged GetRijndaelManaged(string secretKey)
        {
            var keyBytes = new byte[16];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
            return new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                BlockSize = 128,
                Key = keyBytes,
                IV = keyBytes
            };
        }

        private byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }

        private byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        }

        public string Encrypt(string plainText)
        {
            try
            {

                var plainBytes = Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged(passPhrase)));
            }
            catch (Exception ex)
            {
                return "exception : " + ex.Message;
            }


        }

        public string Decrypt(string encryptedText)
        {

            try
            {
                var encryptedBytes = Convert.FromBase64String(encryptedText);
                return Encoding.UTF8.GetString(Decrypt(encryptedBytes, GetRijndaelManaged(passPhrase)));
            }
            catch (Exception ex)
            {
                return "exception : " + ex.Message;
            }

        }
        public string EncryptString(string plainSourceStringToEncrypt)
        {
            using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(passPhrase)))
            {
                byte[] sourceBytes = Encoding.ASCII.GetBytes(plainSourceStringToEncrypt);
                ICryptoTransform ictE = acsp.CreateEncryptor();

                MemoryStream msS = new MemoryStream();

                CryptoStream csS = new CryptoStream(msS, ictE, CryptoStreamMode.Write);
                csS.Write(sourceBytes, 0, sourceBytes.Length);
                csS.FlushFinalBlock();

                byte[] encryptedBytes = msS.ToArray(); //.ToArray() is important, don't mess with the buffer

                return Convert.ToBase64String(encryptedBytes);
            }
        }

        public string DecryptString(string base64StringToDecrypt)
        {
            //string passphrase = "2595874569321569";
            using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(passPhrase)))
            {
                byte[] RawBytes = Convert.FromBase64String(base64StringToDecrypt);
                ICryptoTransform ictD = acsp.CreateDecryptor();
                MemoryStream msD = new MemoryStream(RawBytes, 0, RawBytes.Length);
                CryptoStream csD = new CryptoStream(msD, ictD, CryptoStreamMode.Read);
                return (new StreamReader(csD)).ReadToEnd();
            }
        }
        private AesCryptoServiceProvider GetProvider(byte[] key)
        {
            AesCryptoServiceProvider result = new AesCryptoServiceProvider();
            result.BlockSize = 128;
            result.KeySize = 128;
            result.Mode = CipherMode.CBC;
            result.Padding = PaddingMode.PKCS7;

            result.GenerateIV();
            result.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            byte[] RealKey = GetKey(key, result);
            result.Key = RealKey;
            // result.IV = RealKey;
            return result;
        }
        private byte[] GetKey(byte[] suggestedKey, SymmetricAlgorithm p)
        {
            byte[] kRaw = suggestedKey;
            List<byte> kList = new List<byte>();

            for (int i = 0; i < p.LegalKeySizes[0].MinSize; i += 8)
            {
                kList.Add(kRaw[(i / 8) % kRaw.Length]);
            }
            byte[] k = kList.ToArray();
            return k;
        }

    }
}
