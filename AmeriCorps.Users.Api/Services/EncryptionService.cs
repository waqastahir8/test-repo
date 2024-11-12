using System.Data;
using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Data.Core.Model;
using System.Security.Cryptography;

namespace AmeriCorps.Users.Api.Services;

public interface IEncryptionService
{
    string Decrypt(string cipherText);

    string Encrypt(string plainText);

}

public sealed class EncryptionService : IEncryptionService
{

    public string Encrypt(string plainText)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = Convert.FromBase64String("69PhJU1v1SMbE6mRBWalOIQlBqAmvHQ5WCMX4IoCwZ0=");
            aes.IV = Convert.FromBase64String("vNWAOAbK+6wi0NDXbCAncA==");

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                }

                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    public string Decrypt(string cipherText)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = Convert.FromBase64String("69PhJU1v1SMbE6mRBWalOIQlBqAmvHQ5WCMX4IoCwZ0=");
            aes.IV = Convert.FromBase64String("vNWAOAbK+6wi0NDXbCAncA==");

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }
}